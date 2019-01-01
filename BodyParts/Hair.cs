//Hair.cs
//Description:
//Author: JustSomeGuy
//12/27/2018, 9:50 PM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoC.Items;
using CoC.BodyPart.SpecialInteraction;
namespace CoC.BodyParts
{
	public enum HAIR_STYLE {NOT_APPLICABLE, STRAIGHT, WAVY, CURLY, COILED }
	//Can't use readonly, as beards aren't constant - they grow.
	//NOT FULLY DONE. There's no way to check if you can groom to a certain type of beard based on existing hair. 
	public class Hair : BodyPartBehavior, IDyeable
	{
		/*
		 * Hair can grow. if used on a creature where this isn't an issue (basically everyone but the player)
		 * Just don't call the growth function.
		 */

		//Members
		#region Members
		public bool isGrowing { get; protected set; }

		protected float lengthInInches;
		protected int hoursSinceTrimmed;

		public HAIR_STYLE style { get; protected set; }

		public HairType hairType
		{
			get
			{
				return _hairType;
			}
			private set
			{
				_hairType = value;
				index = _hairType.index;
				updateDescriptor();
			}
		}

		private HairType _hairType;

		#endregion
		//Constructor
		private Hair()
		{
			lengthInInches = 0.0f;
			hairType = HairType.NO_HAIR;
			isGrowing = false;
			index = _hairType.index;
			descriptor = hairType.GetDescriptor();
			color = Dyes.BLACK;
		}

		//Body Part class
		#region BaseBodyPart
		public override string GetDescriptor()
		{
			if (hairType != HairType.NO_HAIR)
			{
				return Math.Round(lengthInInches).ToString() + " inch " + asString(style) + descriptor;
			}
			else
			{
				return descriptor;
			}
		}
		#endregion

		#region Dyeable

		public Dyes color { get; private set; }

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

		private void updateDescriptor()
		{
			index = hairType.index;
			descriptor = hairType.GetDescriptor();
		}

		private string asString(HAIR_STYLE style)
		{
			switch (style)
			{
				case HAIR_STYLE.STRAIGHT:
					return "straight ";
				case HAIR_STYLE.WAVY:
					return "wavy ";
				case HAIR_STYLE.CURLY:
					return "curly ";
				case HAIR_STYLE.COILED:
					return "coiled ";
				case HAIR_STYLE.NOT_APPLICABLE:
				default:
					return "";
				
			}
		}
		#endregion 
	}

	public class HairType : BodyPartBehavior
	{
		private static int indexMaker = 0;
		public bool preventHairGrowth { get; protected set; }
		public readonly bool dyeable;

		public override string GetDescriptor()
		{
			return descriptor;
		}

		public bool canDye()
		{
			return dyeable;
		}

		//Worth noting: As of vanilla, all hair (even basilisk plumes) can be dyed.
		//It can now be disabled, but default behavior is to allow it.

		protected HairType(string desc, bool cannotGrow = false, bool canDye = true)
		{
			index = indexMaker++;
			descriptor = desc;
			preventHairGrowth = cannotGrow;
			dyeable = canDye;
		}

		public static readonly HairType NO_HAIR = new HairType("bald head", true);
		public static readonly HairType NORMAL = new HairType("hair");
        public static readonly HairType FEATHER = new HairType("hair-feathers");
        public static readonly HairType GHOST = new HairType("semi-transparent hair");
        public static readonly HairType GOO = new HairType("gooey hair");
        public static readonly HairType ANEMONE = new HairType("hair-like tendrils");
        public static readonly HairType QUILL = new HairType("quill-hair");
        public static readonly HairType BASILISK_SPINES = new HairType("basilisk spines");
        public static readonly HairType BASILISK_PLUME = new HairType("basilisk plume");
        public static readonly HairType WOOL = new HairType("woolen hair");
        public static readonly HairType LEAF = new HairType("leafy hair");

	}

}
