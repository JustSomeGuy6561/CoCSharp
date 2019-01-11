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
		public readonly EpidermisType epidermisType;
		protected EpidermalBodyPartBehavior(EpidermisType type, GenericDescription shortDesc, FullDescription<ContainerClass> fullDesc, PlayerDescription<ContainerClass> playerDesc,
			ChangeType<ContainerClass> transform, ChangeType<ContainerClass> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			epidermisType = type;
		}
	}
}