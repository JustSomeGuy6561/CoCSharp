using CoC.Backend.Save;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CoC.Backend.BodyParts
{
	[DataContract]
	public abstract class SimpleSaveableBodyPart<ThisClass, BehaviorClass> : ISaveableBase 
		where ThisClass : SimpleSaveableBodyPart<ThisClass, BehaviorClass> 
		where BehaviorClass : SimpleSaveableBehavior
	{

		public abstract BehaviorClass type { get; protected set; }
		public virtual int index => type.index;

		internal abstract bool Validate(bool correctDataIfInvalid = false);

		public virtual SimpleDescriptor description => type.desrciption;

		Type ISaveableBase.currentSaveType => currentSaveVersion;
		Type[] ISaveableBase.saveVersionTypes => saveVersions;
		object ISaveableBase.ToCurrentSaveVersion()
		{
			return ToCurrentSave();
		}

		internal abstract Type currentSaveVersion { get; }
		internal abstract Type[] saveVersions { get; }

		internal abstract SimpleSurrogate<ThisClass, BehaviorClass> ToCurrentSave();

	}

	public abstract class SimpleSaveableBehavior
	{
		public abstract int index { get; }

		private protected SimpleSaveableBehavior(SimpleDescriptor shortDesc)
		{
			desrciption = shortDesc;
		}
		public readonly SimpleDescriptor desrciption;
	}

	[DataContract]
	public abstract class SimpleSurrogate<SaveClass, BehaviorClass> : ISurrogateBase where SaveClass : SimpleSaveableBodyPart<SaveClass, BehaviorClass> where BehaviorClass: SimpleSaveableBehavior
	{
		private protected SimpleSurrogate() { }

		internal abstract SaveClass ToBodyPart();

		object ISurrogateBase.ToSaveable()
		{
			return ToBodyPart();
		}
	}
}
