using System;
using System.Runtime.Serialization;

namespace CoC.Backend.Save
{
	[DataContract]
	public abstract class SaveableData : ISaveableBase
	{
		public abstract Type currentSaveType { get; }
		public abstract Type[] saveVersionTypes { get; }

		public abstract object ToCurrentSaveVersion();

	}
}
