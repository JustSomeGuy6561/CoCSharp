using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
namespace TestingClassLibrary
{
	[Serializable]
	public abstract class SaveData2Test : ISerializable
	{
		private const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

		internal Assembly definedAssembly => Assembly.GetAssembly(GetType());

		internal Type[] externalTypes => KnownExternalTypes();
		protected virtual Type[] KnownExternalTypes() => null;

		public bool ParseData<T>(string name, Dictionary<string, SerializationEntry> entries, out T data)
		{
			if (!entries.ContainsKey(name))
			{
				data = default(T);
				return false;
			}
			else if (typeof(T).IsAssignableFrom(entries[name].ObjectType))
			{
				data = (T)entries[name].Value;
				return true;
			}
			//null won't proc above, as null is serialized as the type object. null will proc here, but only if the type allows null.
			//unfortunately, we can't cast a generic as null - it won't allow it. fortunately, anything that allows null also defaults to null.
			else if (entries[name].Value == null && (!typeof(T).IsValueType || (Nullable.GetUnderlyingType(typeof(T)) != null)))
			{
				data = default(T); //null. not gonna lie, weird af syntax, but you can't expressly use (null) as that won't compile.
				return true;
			}
			data = default(T);
			return false;
		}

		public SaveData2Test() { }

		protected SaveData2Test(SerializationInfo info, StreamingContext context)
		{
			Dictionary<string, SerializationEntry> serializedData = new Dictionary<string, SerializationEntry>();
			foreach (var inf in info)
			{
				serializedData.Add(inf.Name, inf);
			}
			foreach (var data in GetType().GetProperties(bindingFlags))
			{
				if (!data.CanWrite || !serializedData.ContainsKey(data.Name))
				{
					continue;
				}
				else if (data.PropertyType.IsAssignableFrom(serializedData[data.Name].ObjectType))
				{
					Type type = serializedData[data.Name].Value.GetType();
					Console.WriteLine(type);
					data.SetValue(this, serializedData[data.Name].Value);
				}
				//null edge case. technically, since this is a constructor, it's possible for a property to not be null by default, so we need to set it to null anyway
				else if (serializedData[data.Name].Value == null && (!data.PropertyType.IsValueType || (Nullable.GetUnderlyingType(data.PropertyType) != null)))
				{
					data.SetValue(this, null);
				}
			}
			foreach (var data in GetType().GetFields(bindingFlags))
			{
				if (!serializedData.ContainsKey(data.Name))
				{
					continue;
				}
				else if (data.FieldType.IsAssignableFrom(serializedData[data.Name].ObjectType))
				{
					Type type = serializedData[data.Name].Value.GetType();
					Console.WriteLine(type);
					data.SetValue(this, serializedData[data.Name].Value);
				}
				//null edge case in the event a value does not default to null but should be
				else if (serializedData[data.Name].Value == null && (!data.FieldType.IsValueType || (Nullable.GetUnderlyingType(data.FieldType) != null)))
				{
					data.SetValue(this, null);
				}
			}

			ToCurrentVersion(serializedData);
		}

		protected abstract void ToCurrentVersion(Dictionary<string, SerializationEntry> availableDataToParse);

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			Array.ForEach(GetType().GetProperties(bindingFlags), (x) =>
			{
				Type type = x.GetValue(this)?.GetType() ?? (!x.PropertyType.IsAbstract ? x.PropertyType : typeof(object));
				if (x.CanWrite && x.CanRead) info.AddValue(x.Name, x.GetValue(this), type);
			});
			Array.ForEach(GetType().GetFields(bindingFlags), (x) =>
			{
				Type type = x.GetValue(this)?.GetType() ?? x.FieldType;
				info.AddValue(x.Name, x.GetValue(this), type);
			});
		}
	}
}
