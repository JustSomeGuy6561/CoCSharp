//EpidermalBodyPart.cs
//Description:
//Author: JustSomeGuy
//12/30/2018, 11:32 PM


using CoC.BodyParts.SpecialInteraction;
using CoC.EpidermalColors;

namespace CoC.BodyParts
{
	public abstract class EpidermalBodyPart<T, U> : BodyPartBase<T, U>, IFurAware, IToneAware where T : EpidermalBodyPart<T, U> where U : EpidermalBodyPartBehavior<U, T>
	{
		public Epidermis epidermis { get; protected set; }

		protected EpidermalBodyPart(EpidermisType type, Tones currentTone)
		{
			epidermis = Epidermis.Generate(type, currentTone);
		}

		protected EpidermalBodyPart(EpidermisType type, FurColor currentFur)
		{
			epidermis = Epidermis.Generate(type, currentFur);
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