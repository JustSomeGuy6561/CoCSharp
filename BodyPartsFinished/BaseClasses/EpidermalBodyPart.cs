////EpidermalBodyPart.cs
////Description:
////Author: JustSomeGuy
////12/30/2018, 11:32 PM
//using CoC.BodyParts.SpecialInteraction;
//using CoC.Items;

using CoC.BodyParts.SpecialInteraction;
using CoC.Items;
using CoC.Tools;

namespace CoC.BodyParts
{
	public abstract class EpidermalBodyPart<T, U> : BodyPartBase<T, U>, IFurAware, IToneAware where T : EpidermalBodyPart<T, U> where U : EpidermalBodyPartBehavior<U, T>
	{
		public Epidermis epidermis { get; protected set; }

		protected EpidermalBodyPart(EpidermisType type)
		{
			epidermis = Epidermis.Generate(type);
		}

		public virtual void reactToChangeInFurColor(FurColor furColor)
		{
			epidermis.reactToChangeInFurColor(furColor);
		}
		public virtual void reactToChangeInSkinTone(Tones newTone)
		{
			epidermis.reactToChangeInSkinTone(newTone);
		}
	}
}