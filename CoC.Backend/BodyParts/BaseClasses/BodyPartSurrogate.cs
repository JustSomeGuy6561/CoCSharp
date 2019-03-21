using CoC.Backend.Save;
using System.Runtime.Serialization;

namespace CoC.Backend.BodyParts
{
	[DataContract]
	public abstract class BodyPartSurrogate<SaveClass, BehaviorClass> : ISurrogateBase 
		where SaveClass : BodyPartBase<SaveClass, BehaviorClass> 
		where BehaviorClass : BodyPartBehavior<BehaviorClass, SaveClass>
	{
		private protected BodyPartSurrogate(){}

		internal abstract SaveClass ToBodyPart();

		object ISurrogateBase.ToSaveable()
		{
			return ToBodyPart();
		}
	}
}
