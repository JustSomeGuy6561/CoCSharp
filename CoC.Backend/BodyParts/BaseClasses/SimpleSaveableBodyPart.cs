using CoC.Backend.Save;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts
{
	public abstract class SimpleSaveableBodyPart<ThisClass, BehaviorClass> : SimpleBodyPart<BehaviorClass>, ISaveableBase 
		where ThisClass : SimpleSaveableBodyPart<ThisClass, BehaviorClass> 
		where BehaviorClass : SimpleSaveableBehavior
	{
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

	public abstract class SimpleSaveableBehavior : SimpleBodyPartType
	{
		private protected SimpleSaveableBehavior(SimpleDescriptor shortDesc) : base(shortDesc)	{}
	}

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
