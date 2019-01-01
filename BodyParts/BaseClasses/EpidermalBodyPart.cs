//EpidermalBodyPart.cs
//Description:
//Author: JustSomeGuy
//12/30/2018, 11:32 PM
using CoC.BodyParts.SpecialInteraction;
using CoC.Items;

namespace CoC.BodyParts
{
	abstract class EpidermalBodyPart<T, U> : BodyPartBase<T, U>, IToneable, IDyeable where T : EpidermalBodyPart<T,U> where U : EpidermalBodyPartBehavior<U, T>
	{
		public abstract Epidermis epidermis { get; }

		//activate or deactivate using a tone or dye for this body part.
		//generally, a body part will support one, or perhaps both. 
		public bool usesToneOrDye => epidermis.canDye() || epidermis.canTone();

		//in the event both are set, dye gets priority. imo fur is more visible than skin.
		//but that really shouldn't happen.
		public bool usesDye => epidermis.canDye();
		public bool usesTone => epidermis.canTone() && !usesDye; 


		public bool UpdateType(U newType, Tones currentSkinTone, Dyes currentHairOrFurColor, bool forceUpdateToneAndDye = false)
		{
			Tones oldTone = currentTone;
			Dyes oldDye = currentDye;

			bool passAlongTone = usesTone && newType.canTone() && !forceUpdateToneAndDye;
			bool passAlongDye = usesDye && newType.canDye() && !forceUpdateToneAndDye;

			//update our skin and hair colors.
			currentDye = currentHairOrFurColor;
			currentTone = currentSkinTone;

			//
			bool retVal = ChangeType(newType);
			if (passAlongDye)
			{
				epidermis.tryToDye(ref currentDye, oldDye);
			}
			if (passAlongTone)
			{
				epidermis.tryToTone(ref currentTone, oldTone);
			}
			return retVal;
		}

		protected virtual bool ChangeType(U newType)
		{
			type = newType;
			return true;
		}

		//you can technically override these, as there may be weird edge cases
		//where you want to tone or dye things outside of the epidermis.
		//notably, some claws react to skin tone changes, but the arms themselves
		//dont. but claws themselves are entirely dependant on arms. it's complicated.
		public virtual bool canTone()
		{
			return epidermis.canTone();
		}

		public virtual bool canDye()
		{
			return epidermis.canDye();
		}


		public virtual bool attemptToTone(Tones tone)
		{
			if (canTone())
			{
				return epidermis.tryToTone(ref currentTone, tone);
			}
			return false;
		}
		public virtual bool attemptToDye(Dyes dye)
		{
			if (canDye())
			{
				return epidermis.tryToDye(ref currentDye, dye);
			}
			return false;
		}

		public EpidermalColors currentColor()
		{
			if (!usesToneOrDye)
			{
				return EpidermalColors.NONE;
			}
			else if (usesTone)
			{
				return currentTone;
			}
			else
			{
				return currentDye;
			}
		}
		protected Dyes currentDye;
		protected Tones currentTone;
	}
}