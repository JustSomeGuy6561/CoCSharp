//EpidermalBodyPartBehavior.cs
//Description: Body Part behavior for epidermal body parts
//Author: JustSomeGuy
//12/30/2018, 11:32 PM
using CoC.BodyParts.SpecialInteraction;
using CoC.EpidermalColors;
using CoC.Tools;

namespace CoC.BodyParts
{
	public abstract class EpidermalBodyPartBehavior<ThisClass, ContainerClass> : BodyPartBehavior<ThisClass, ContainerClass>
		where ThisClass : EpidermalBodyPartBehavior<ThisClass, ContainerClass> where ContainerClass : EpidermalBodyPart<ContainerClass, ThisClass>
	{
		protected EpidermalBodyPartBehavior(GenericDescription shortDesc, CreatureDescription<ContainerClass> creatureDesc, PlayerDescription<ContainerClass> playerDesc,
			ChangeType<ContainerClass> transform, ChangeType<ContainerClass> restore) : base(shortDesc, creatureDesc, playerDesc, transform, restore)	{}
	}
}