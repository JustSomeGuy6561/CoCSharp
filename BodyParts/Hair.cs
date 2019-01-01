//Hair.cs
//Description:
//Author: JustSomeGuy
//12/27/2018, 9:50 PM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoC.Items;
using CoC.BodyParts.SpecialInteraction;
using CoC.Tools;
using static CoC.Strings.BodyParts.HairStrings;

namespace CoC.BodyParts
{
	public enum HAIR_STYLE {NOT_APPLICABLE, MANGEY, MESSY, STRAIGHT, WAVY, CURLY, COILED }
	//Can't use readonly, as beards aren't constant - they grow.
	//NOT FULLY DONE. There's no way to check if you can groom to a certain type of beard based on existing hair. 
	public class Hair : SimpleBodyPart, IDyeable
	{
		/*
		 * Hair can grow. if used on a creature where this isn't an issue (basically everyone but the player)
		 * Just don't call the growth function.
		 */

		//Members
		#region Members
		public bool isGrowing { get; protected set; }
		public bool isSemiTransparent { get; protected set; }
		public float lengthInInches { get; protected set; }
		protected int hoursSinceTrimmed;

		public HAIR_STYLE style { get; protected set; }

		public HairType hairType { get; protected set; }

		public string shortDescriptionWithColor()
		{
			return ColoredHairDesc(this);
		}
		public string fullDescription()
		{
			return FullDesc(this);
		}

		#endregion
		//Constructor
		private Hair()
		{
			lengthInInches = 0.0f;
			hairType = HairType.NO_HAIR;
			isGrowing = false;
			color = Dyes.HUMAN_DEFAULT;
			isSemiTransparent = false;
			shortDescription = () => HairDesc(this);
		}

		#region Dyeable

		public Dyes color { get; private set; }

		public override int index => hairType.index;

		public override GenericDescription shortDescription { get; protected set; }

		public bool canDye()
		{
			return hairType.canDye();
		}

		public bool attemptToDye(Dyes dye)
		{
			if (!canDye() || dye == color)
			{
				return false;
			}
			color = dye;
			return true;
		}
		#endregion

		#region Specific

		//If hair can grow, sets it to grow and returns true. returns true if 
		//hair is already growing. if something prevents it from growing, return false
		public bool ActivateGrowth()
		{
			if (!hairType.preventHairGrowth)
			{
				isGrowing = true;
			}
			return isGrowing;
		}

		//Deactivates hair growth. should never return false, but i suppose something could
		//exist in the future that prevents hair from not growing.
		public bool DeactivateGrowth()
		{
			isGrowing = false;
			return !isGrowing;
		}

		public float GrowHair(int rateOfGrowth = 1)
		{
			hoursSinceTrimmed += 12 * rateOfGrowth;
			//early exit if we can't grow hair.
			if (!isGrowing)
			{
				return 0;
			}
			float growAmount = growthRate(hoursSinceTrimmed <= 3 * ONE_DAY, rateOfGrowth, lengthInInches);
			lengthInInches += growAmount;
			return growAmount;
		}
		#endregion

		//Helpers
		#region Private Helpers
		private const int ONE_DAY = 24;
		private const int ONE_MONTH = ONE_DAY * 31;
		private const int ONE_WEEK = ONE_DAY * 7;

		private float growthRate(bool trimmedRecently, float baseGrowthRate, float length)
		{
			//hair grows slightly faster if it's been trimmed recently
			float boostValue = trimmedRecently ? 1.1f : 1.0f;
			//diminishing returns of growth after 2 inches.
			if (length <= 6.0f)
			{
				return 0.2f * baseGrowthRate * boostValue;
			}
			else if (length < 20.0f)
			{
				return (1.2f / length) * baseGrowthRate * boostValue;
			}
			//but we'll level it out after 10 inches, because that seems fair/accurate.
			else
			{
				return 0.06f * baseGrowthRate * boostValue;
			}			
		}
		#endregion 
	}

	public class HairType : SimpleBodyPart, IImmutableDyeable
	{
		private static int indexMaker = 0;

		public override int index => _index;
		private int _index;
		public bool preventHairGrowth { get; protected set; }
		public override GenericDescription shortDescription { get; protected set; }

		//Worth noting: As of vanilla, all hair (even basilisk plumes) can be dyed.
		//It can now be disabled, but default behavior is to allow it. to disable,
		//or to conditionally allow dyeing, implement a subclass of Hairtype and 
		//override canDye and possibly tryToDye with the new rules.

		public virtual bool canDye()
		{
			return true;
		}

		public virtual bool tryToDye(ref Dyes currentColor, Dyes newColor)
		{
			currentColor = newColor;
			return currentColor == newColor;
		}

		protected HairType(GenericDescription desc, bool canGrow = true)
		{
			_index = indexMaker++;
			shortDescription = desc;
			preventHairGrowth = !canGrow;
		}

		public static readonly HairType NO_HAIR = new HairType(NoHairStr, false);
		public static readonly HairType NORMAL = new HairType(NormalStr);
        public static readonly HairType FEATHER = new HairType(FeatherStr);
        public static readonly HairType GOO = new HairType(GooStr);
        public static readonly HairType ANEMONE = new HairType(AnemoneStr);
        public static readonly HairType QUILL = new HairType(QuillStr);
        public static readonly HairType BASILISK_SPINES = new HairType(BasiliskSpinesStr);
        public static readonly HairType BASILISK_PLUME = new HairType(BasiliskPlumesStr);
        public static readonly HairType WOOL = new HairType(WoolStr);
        public static readonly HairType LEAF = new HairType(LeafStr);
	}

}
