using CoC.Tools;
namespace CoC.BodyParts
{
	public abstract class SimpleBodyPart
	{
		public abstract int index { get; }
		public abstract GenericDescription shortDescription { get; protected set; }
	}
}
