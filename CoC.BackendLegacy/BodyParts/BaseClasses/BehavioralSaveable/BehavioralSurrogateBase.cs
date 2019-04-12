//using CoC.Backend.Save;
//using System.Runtime.Serialization;

//namespace CoC.Backend.BodyParts
//{
//	[DataContract]
//	public abstract class BehavioralSurrogateBase<SaveClass, BehaviorClass> : ISurrogateBase 
//		where SaveClass : BehavioralSaveablePart<SaveClass, BehaviorClass> 
//		where BehaviorClass : SaveableBehavior<BehaviorClass, SaveClass>
//	{
//		private protected BehavioralSurrogateBase(){}

//		internal abstract SaveClass ToBodyPart();

//		object ISurrogateBase.ToSaveable()
//		{
//			return ToBodyPart();
//		}
//	}
//}
