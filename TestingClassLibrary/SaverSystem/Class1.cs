using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;
/*
namespace TestingClassLibrary.SaverSystem
{
abstract class SaveBase<T> : ISerializable where T : SaverBase, new()
{
protected SaveBase(SerializationInfo info, StreamingContext context)
{
T saver = new T();
saver.Init(info, context);

}

void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
{

}

protected abstract Type[] Savers { get; }
}

abstract class SaverBase
{
private const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
public SaverBase()
{ }

internal void Init(SerializationInfo info, StreamingContext context)
{
Dictionary<string, SerializationEntry> serializedData = new Dictionary<string, SerializationEntry>();
int version = 1;
foreach (var inf in info)
{
serializedData.Add(inf.Name, inf);
}
if (serializedData.ContainsKey("version") && typeof(int).IsAssignableFrom(serializedData["version"].ObjectType))
{
version = (int)serializedData["version"].Value;
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
}
}
}
*/

namespace TestingClassLibrary.SaverSystem
{
	public abstract class Obj<ThisClass, SaverClass> where ThisClass : Obj<ThisClass, SaverClass>
		where SaverClass : Saver<SaverClass, ThisClass>
	{
		protected abstract SaverClass ToCurrentSave();

		internal Surrogate ToSurrogate()
		{
			SaverClass data = ToCurrentSave();
			return new Surrogate(data, data.GetType());
		}
	}
	public abstract class Saver<ThisClass, ClassToSave> where ThisClass : Saver<ThisClass, ClassToSave>
		where ClassToSave : Obj<ClassToSave, ThisClass>
	{
		public Saver() { }
		internal protected abstract ClassToSave ToClass();
	}

	//public abstract class Surrogate<SaverClass, ObjectClass> : ISurrogateBase, ISerializable where SaverClass : Saver<SaverClass, ObjectClass>
	//		where ObjectClass : Obj<ObjectClass, SaverClass>
	public sealed class Surrogate : ISerializable
	{
		private readonly Type type;
		private readonly object data;
		public Surrogate(object d, Type t)
		{
			type = t;
			data = d;
		}

		//deserialization
		private Surrogate(SerializationInfo info, StreamingContext context)
		{
			//run try-catch on type getValue below. if it fails, set type to object and data to null, and return immediately.
			//TODO: figure a way around this - perhaps fallback to version1, though this requires making surrogate abstract and 
			//an abstract getter. this would require generic parameters, and doing so prevents the ability to call ToClass, 
			//as we can't cast it without knowing the generic parameters. could get around this with an internal interface attached to surrogate
			//that is explicitly implemented and calls the toclass. unfortunately, to deal with old save version, you still need reflection, so
			//there's no point.
			try
			{
				type = (Type)info.GetValue("version", typeof(Type));
			}
			catch (SerializationException)
			{
				type = typeof(object);
				data = null;
				return;
			}
			
			data = Activator.CreateInstance(type);
			Dictionary<string, SerializationEntry> serializedData = new Dictionary<string, SerializationEntry>();
			foreach (var inf in info)
			{
				serializedData.Add(inf.Name, inf);
			}
			foreach (var prop in type.GetProperties())
			{
				if (prop.CanWrite && serializedData.ContainsKey(prop.Name) && prop.PropertyType.IsAssignableFrom(serializedData[prop.Name].ObjectType))
				{
					prop.SetValue(data, serializedData[prop.Name].Value);
				}
			}
			foreach (var field in type.GetFields())
			{
				if (serializedData.ContainsKey(field.Name) && field.FieldType.IsAssignableFrom(serializedData[field.Name].ObjectType))
				{
					field.SetValue(data, serializedData[field.Name].ObjectType);
				}
			}
		}

		//serialization. known.
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			foreach (var prop in type.GetProperties())
			{
				info.AddValue(prop.Name, prop.GetValue(data));
			}
			foreach (var field in type.GetFields())
			{
				info.AddValue(field.Name, field.GetValue(data));
			}
			info.AddValue("version", type, typeof(Type));
		}

		internal object ToClass()
		{
			if (data == null)
			{
				return null;
			}
			var method = type.GetMethod("ToClass", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			return method?.Invoke(data, null);
		}
	}
}