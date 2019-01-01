//EpidermalBodyPartBehavior.cs
//Description: Body Part behavior for epidermal body parts
//Author: JustSomeGuy
//12/30/2018, 11:32 PM
using CoC.BodyParts.SpecialInteraction;
using CoC.Items;

namespace CoC.BodyParts
{
	public abstract class EpidermalBodyPartBehavior<ThisClass, ContainerClass> : BodyPartBehavior<ThisClass, ContainerClass>, IImmutableDyeable, IImmutableToneable where ThisClass : EpidermalBodyPartBehavior<ThisClass, ContainerClass> where ContainerClass : EpidermalBodyPart<ContainerClass, ThisClass>
	{
		public abstract string defaultEpidermalAdjective();
		public abstract bool canDye();
		public abstract bool canTone();
		public abstract bool tryToDye(ref Dyes currentColor, Dyes newColor);
		public abstract bool tryToTone(ref Tones currentTone, Tones newTone);
		public abstract Epidermis epidermis { get; }
	}