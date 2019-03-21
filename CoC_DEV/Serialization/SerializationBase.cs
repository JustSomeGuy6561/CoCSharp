using System;
using System.Runtime.Serialization;

namespace CoC.Serialization
{
	public abstract class SerializationBase
	{
		public abstract Type currentSaveType { get; }
		public abstract Type[] saveVersionTypes { get; }

		public abstract object ToCurrentSaveVersion();
		public abstract object FromSave(object saveData, Type saveType);


	}
}
