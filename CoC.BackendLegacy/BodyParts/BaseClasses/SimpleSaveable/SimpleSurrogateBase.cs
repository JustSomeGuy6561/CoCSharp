using CoC.Backend.Save;
using System.Runtime.Serialization;

namespace CoC.Backend.BodyParts
{
	[DataContract]
	public abstract class SimpleSurrogateBase<SaveClass> : ISurrogateBase where SaveClass : SimpleSaveablePart<SaveClass>
	{
		private protected SimpleSurrogateBase() { }

		internal abstract SaveClass ToBodyPart();

		object ISurrogateBase.ToSaveable()
		{
			return ToBodyPart();
		}
	}
}
