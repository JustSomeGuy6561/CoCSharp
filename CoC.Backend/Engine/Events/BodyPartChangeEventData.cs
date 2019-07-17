using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts.SpecialInteraction
{
	//
	public delegate void BodyPartChangedEventHandler<T, U>(object sender, BodyPartChangedEventArg<T, U> args) where T : BehavioralPartBase<U> where U : BehaviorBase;

	public sealed class BodyPartChangedEventArg<DataClass, BehaviorClass> : EventArgs where DataClass: BehavioralPartBase<BehaviorClass> where BehaviorClass : BehaviorBase
	{
		public readonly DataClass data;
		public readonly BehaviorClass oldValue;
		public readonly BehaviorClass newValue;

		internal BodyPartChangedEventArg(DataClass dataClass, BehaviorClass oldBehavior, BehaviorClass newBehavior)
		{
			data = dataClass;
			oldValue = oldBehavior;
			newValue = newBehavior;
		}

	}

	public interface IBodyPartChangeAware<DataClass, BehaviorClass> where DataClass : BehavioralPartBase<BehaviorClass> where BehaviorClass : BehaviorBase
	{
		void OnBodyPartTypeChanged(object sender, BodyPartChangedEventArg<DataClass, BehaviorClass> args);
	}

	//public delegate void SimplePartChangedEventHandler<T>(object sender, SimplePartChangedEventArg<T> args) where T : SimpleSaveablePart<T>;

	//public sealed class SimplePartChangedEventArg<T> : EventArgs where T : SimpleSaveablePart<T>
	//{
	//	public readonly T oldValue, newValue;

	//	internal SimplePartChangedEventArg(T oldData, T newData)
	//	{
	//		oldValue = oldData;
	//		newValue = newData;
	//	}

	//}

	//public interface ISimpleBodyPartChangeAware<T> where T : SimpleSaveablePart<T>
	//{
	//	void OnSimplePartDataChanged(object sender, SimplePartChangedEventArg<T> args);
	//}
}
