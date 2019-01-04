//PiercableBodyPartBehavior.cs
//Description:
//Author: JustSomeGuy
//1/1/2019, 9:09 AM

using CoC.BodyParts.SpecialInteraction;
using CoC.EpidermalColors;
using CoC.Tools;

namespace CoC.BodyParts
{
	//i suppose you could prevent piercings if the body type doesn't support it - dragon ears (which are just slits iirc) might not support earrings for example.
	//but i'm not implementing that. if you want to, you have the option using this class and rewriting the implementations accordingly.
	public abstract class PiercableBodyPartBehavior<ThisClass, ContainerClass, PiercingEnum> : BodyPartBehavior<ThisClass, ContainerClass> where ThisClass : PiercableBodyPartBehavior<ThisClass, ContainerClass, PiercingEnum>
		where ContainerClass : PiercableBodyPart<ContainerClass, ThisClass, PiercingEnum> where PiercingEnum : System.Enum
	{
		protected PiercableBodyPartBehavior(GenericDescription shortDesc, CreatureDescription<ContainerClass> creatureDesc, PlayerDescription<ContainerClass> playerDesc,
			ChangeType<ThisClass> transform, ChangeType<ThisClass> restore) : base(shortDesc, creatureDesc, playerDesc, transform, restore) { }
	}
}