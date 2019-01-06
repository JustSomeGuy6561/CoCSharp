//EpidermalPiercableBodyPart - Copy.cs
//Description:
//Author: JustSomeGuy
//1/4/2019, 5:44 PM

//EpidermalPiercableBodyPart.cs
//Description:
//Author: JustSomeGuy
//12/30/2018, 11:32 PM


using CoC.BodyParts.SpecialInteraction;
using CoC.EpidermalColors;

namespace CoC.BodyParts
{
	public abstract class EpidermalPiercableBodyPart<ThisClas, BehaviorClass, Enum> : PiercableBodyPart<ThisClas, BehaviorClass, Enum>, IFurAware, IToneAware
		where ThisClas : EpidermalPiercableBodyPart<ThisClas, BehaviorClass, Enum>
		where BehaviorClass : EpidermalPiercableBehavior<BehaviorClass, ThisClas, Enum>
		where Enum : System.Enum
	{
		public Epidermis epidermis { get; protected set; }

		protected EpidermalPiercableBodyPart(EpidermisType type, Tones currentTone, FurColor furColor, PiercingFlags flags) : base(flags)
		{
			epidermis = Epidermis.Generate(type, currentTone, furColor);
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