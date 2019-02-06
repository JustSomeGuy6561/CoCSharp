//Serializer.cs
//Description:
//Author: JustSomeGuy
//1/30/2019, 10:11 PM
using CoC.Creatures;
using CoC.Engine;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace CoC.Serialization
{
	[DataContract]
	internal class Serializer : ISerializable
	{
		private static readonly Dictionary<string, Type> classWithStaticVariables = new Dictionary<string, Type>();


		static Serializer()
		{
			classWithStaticVariables.Add(typeof(GlobalSettings).FullName, typeof(GlobalSettings));
			//add classes with static variables here. 
		}

		public Serializer(Player currPlayer)
		{
			player = currPlayer;
		}

		[OnDeserialized]
		private void UpdatePlayer(StreamingContext context)
		{
			Program.currPlayer = player;
			player = null;
		}

		[OnSerializing]
		private void UpdateSerializer(StreamingContext context)
		{
			player = Program.currPlayer;
		}
		[OnSerialized]
		private void ResetSerializer(StreamingContext context)
		{
			player = null;
		}


		[DataMember]
		internal Player player;

		protected Serializer(SerializationInfo info, StreamingContext context)
		{
			Dictionary<string, SerializationEntry> serializeData = new Dictionary<string, SerializationEntry>();
			foreach (SerializationEntry entry in info)
			{
				string[] data = entry.Name.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
				string classFullName = data[0];
				string memberName = data[1];
				if (classWithStaticVariables.ContainsKey(classFullName))
				{
					Console.WriteLine(classWithStaticVariables[classFullName].Namespace);
					FieldInfo field = classWithStaticVariables[classFullName].GetField(memberName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
					if (field != null)
					{
						if (field.GetCustomAttribute<StaticSaveAttribute>() != null && field.FieldType == entry.ObjectType)
						{
							field.SetValue(null, entry.Value);
						}
						//else throw or warn of non-matching types.
						continue;
					}
					PropertyInfo property = classWithStaticVariables[classFullName].GetProperty(memberName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
					if (property != null)
					{
						if (property.GetCustomAttribute<StaticSaveAttribute>() != null && property.PropertyType == entry.ObjectType)
						{
							property.SetValue(null, entry.Value);
						}
						//else throw or warn of non-matching types.
						continue;
					}
					//throw or warn of not found value.
				}
			}

			player = (Player)info.GetValue(nameof(player), typeof(Player));
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			foreach (var value in classWithStaticVariables.Values)
			{
				List<PropertyInfo> props = saveableProperties(value);
				foreach (var prop in props)
				{
					info.AddValue(value.FullName + ":" + prop.Name, prop.GetValue(this), prop.PropertyType);
				}
				List<FieldInfo> fields = saveableFields(value);
				foreach (var field in fields)
				{
					info.AddValue(value.FullName + ":" + field.Name, field.GetValue(this), field.FieldType);
				}
			}
		}

		private List<PropertyInfo> saveableProperties(Type type)
		{
			List<PropertyInfo> saveables = new List<PropertyInfo>();
			PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
			foreach (PropertyInfo prop in props)
			{
				StaticSaveAttribute attrs = prop.GetCustomAttribute<StaticSaveAttribute>();
				if (attrs != null)
				{
					saveables.Add(prop);
				}
			}
			return saveables;
		}

		private List<FieldInfo> saveableFields(Type type)
		{
			List<FieldInfo> saveables = new List<FieldInfo>();
			FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
			foreach (FieldInfo field in fields)
			{
				StaticSaveAttribute attrs = field.GetCustomAttribute<StaticSaveAttribute>();
				if (attrs != null)
				{
					saveables.Add(field);
				}
			}
			return saveables;
		}
	}
}
