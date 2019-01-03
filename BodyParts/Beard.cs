//Beard.cs
//Description:
//Author: JustSomeGuy
//12/26/2018, 8:49 PM
using CoC.Items;
using System;
using CoC.BodyParts.SpecialInteraction;

namespace CoC.BodyParts
{
	//Can't use readonly, as beards aren't constant - they grow.
	//NOT FULLY DONE. There's no way to check if you can groom to a certain type of beard based on existing hair. 
	class FacialHair : SimpleBodyPart, IDyeable
	{
		/*
		 * Facial Hair can grow. This means styles may become other styles as they grow. for example, a van dyke may 
		 * connect if left alone, becoming a circle beard. Sideburns can become muttonchops; 
		 * Note that a full beard and Mountain man are actually different: mountain man is a full beard, but it is styled
		 * and kept (relatively) clean. 
		 */

		//Possible "silly" idea: shaving is a skillset that you start out bad at. Which means that until you have experience/training, 
		// every time you shave yourself you look terrible (unless it's clean shaven, i guess)
		//the only solution is for the stylist to clean shave you and then regrow it. growth serum costs money though.
		public enum BeardStyle {NONE, CLEAN_SHAVEN, FIVE_OCLOCK_SHADOW, GOATEE, HANDLEBAR, SIDEBURNS, FU_MANCHU, VAN_DYKE, MUTTONCHOPS, CIRCLE_BEARD, CHIN_CURTAIN, MOUNTAIN_MAN, FULL_BEARD }

		//Members
		private readonly bool isActive;
		protected float lengthInInches;
		protected int hoursSinceTrimmed;
		public BeardStyle style
		{
			get
			{
				return _style;
			}
			private set
			{
				_style = value;
				updateDescriptor();
			}
		}

		public HairFurColors color { get; private set; }

		private BeardStyle _style;

		//NONE Instance
		public static readonly FacialHair NONE = new FacialHair(false);


		//Constructor
		private FacialHair(bool active = true)
		{
			color = HairFurColors.BLACK;
			isActive = active;
			if (isActive)
			{
				style = BeardStyle.CLEAN_SHAVEN;
				lengthInInches = 0.0f;
			}
			else
			{
				style = BeardStyle.NONE;
			}
			index = (int)style;
		}

		//Body Part class
		public override string GetDescriptor()
		{
			return descriptor;
		}

		//Beard Methods
		public static void ActivateFacialHair(ref FacialHair face)
		{
			if (face == NONE)
			{
				face = new FacialHair();
			}
		}

		public static void DeactivateFacialHair(ref FacialHair face)
		{
			if (face != NONE)
			{
				face = NONE;
			}
		}

		public bool Groom()
		{
			if (this == NONE)
			{
				return false;
			}
			hoursSinceTrimmed = 0;
			throw new NotImplementedException("Still no specifications on hair types. Can't groom to a type w/o them");
			//return true;
		}

		//Use this to groom existing facial hair into applicable styles. 
		//Note that this has no validation. Only make available styles if they fit the requirements.
		public bool Groom(BeardStyle newStyle)
		{
			if (this == NONE)
			{
				return false;
			}
			style = newStyle;
			return Groom();
			
		}

		//Use this to magically grow facial hair long enough for this style.
		public bool GrowAndGroom(BeardStyle newStyle)
		{
			return Groom(newStyle);
		}

		//call every 12 hours.
		public bool GrowBeard(int rateOfGrowth = 1)
		{
			if (this == NONE)
			{
				throw new Exception("Called GrowBeard, but Beard is disabled. Try checking if this is not equal to Beard.NONE first");
			}
			BeardStyle oldStyle = style;
			hoursSinceTrimmed += 12 * rateOfGrowth;
			lengthInInches += growthRate(hoursSinceTrimmed <= 3 * ONE_DAY, rateOfGrowth, lengthInInches);

			if (hoursSinceTrimmed > ONE_MONTH)
			{
				style = BeardStyle.FULL_BEARD;
			}
			else if (style == BeardStyle.FU_MANCHU || style == BeardStyle.HANDLEBAR && hoursSinceTrimmed > ONE_WEEK)
			{
				style = BeardStyle.CIRCLE_BEARD;
			}
			else if (style == BeardStyle.VAN_DYKE && hoursSinceTrimmed > ONE_WEEK)
			{
				style = BeardStyle.CIRCLE_BEARD;
			}
			else if (style == BeardStyle.GOATEE && hoursSinceTrimmed > 2 * ONE_WEEK)
			{
				style = BeardStyle.CIRCLE_BEARD;
			}
			else if (style == BeardStyle.FIVE_OCLOCK_SHADOW && hoursSinceTrimmed > 3 * ONE_DAY)
			{

			}
			else if (hoursSinceTrimmed > 12 && style == BeardStyle.CLEAN_SHAVEN)
			{
				style = BeardStyle.FIVE_OCLOCK_SHADOW;
			}
			else
			{
				throw new NotImplementedException("Need to add remaining beard style growths. alternatively, could add a better way of changing, IDK");
			}
			updateDescriptor();
			return oldStyle != style;
		}

		public bool canStyleAs(BeardStyle beard)
		{
			throw new NotImplementedException("Need to add specifications for each beard");
		}


		//Helpers

		private const int ONE_DAY = 24;
		private const int ONE_MONTH = ONE_DAY * 31;
		private const int ONE_WEEK = ONE_DAY * 7;

		private float growthRate(bool trimmedRecently, float baseGrowthRate, float length)
		{
			//hair grows slightly faster if it's been trimmed recently
			float boostValue = trimmedRecently ? 1.1f : 1.0f;
			//diminishing returns of growth after 2 inches.
			if (length <= 2.0f)
			{
				return 0.1f * baseGrowthRate * boostValue;
			}
			else if (length < 10.0f)
			{
				return (0.2f / length) * baseGrowthRate * boostValue;
			}
			//but we'll level it out after 10 inches, because that seems fair/accurate.
			else
			{
				return 0.02f * baseGrowthRate * boostValue;
			}			
		}

		private void updateDescriptor()
		{
			index = (int)style;
			throw new NotImplementedException("Update Descriptor for beard not implemented.");
		}

		public bool canDye()
		{
			return isActive;
		}

		public bool attemptToDye(HairFurColors dye)
		{
			if (!this.isActive || this.color == dye)
			{
				return false;
			}
			color = dye;
			return true;
		}
	}


}
