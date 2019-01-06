//EpidermalPiercableBehavior - Copy.cs
//Description:
//Author: JustSomeGuy
//1/4/2019, 5:44 PM

//EpidermalPiercableBehavior.cs
//Description: Body Part behavior for epidermal body parts
//Author: JustSomeGuy
//12/30/2018, 11:32 PM
using CoC.BodyParts.SpecialInteraction;
using CoC.EpidermalColors;
using CoC.Tools;

namespace CoC.BodyParts
{
	public abstract class EpidermalPiercableBehavior<ThisClass, ContainerClass, Enum> : PiercableBodyPartBehavior<ThisClass, ContainerClass, Enum>
		where ThisClass : EpidermalPiercableBehavior<ThisClass, ContainerClass, Enum> 
		where ContainerClass : EpidermalPiercableBodyPart<ContainerClass, ThisClass, Enum>
		where Enum : System.Enum
	{
		public readonly EpidermisType epidermisType;
		protected EpidermalPiercableBehavior(EpidermisType type, GenericDescription shortDesc, CreatureDescription<ContainerClass> creatureDesc, PlayerDescription<ContainerClass> playerDesc,
			ChangeType<ContainerClass> transform, ChangeType<ContainerClass> restore) : base(shortDesc, creatureDesc, playerDesc, transform, restore)
		{
			epidermisType = type;
		}
	}
}