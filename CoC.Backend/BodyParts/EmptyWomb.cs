using CoC.Backend.Creatures;

namespace CoC.Backend.BodyParts
{
	public sealed class EmptyWomb : Womb
	{
		public EmptyWomb(Creature source ) : base(source, null, null, null) { }
	}
}
