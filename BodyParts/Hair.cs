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


	public enum HAIR_STYLE {NOT_APPLICABLE, MESSY, STRAIGHT, BRAIDED, WAVY, CURLY, COILED }

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

		public string ColoredStyledDescription()
		{
			return ColoredStyledHairDescript(this);
		}

		public string fullDescription()
		{
			return FullDesc(this);
		}

		#endregion
		//Constructor
		protected Hair()
		{
			lengthInInches = 0.0f;
			hairType = HairType.NO_HAIR;
			isGrowing = false;
			color = HairFurColors.HUMAN_DEFAULT;
			isSemiTransparent = false;
			shortDescription = () => HairDesc(this);
		}

		public void Restore()
		{
			hairType = HairType.NORMAL;
		}

		public void Reset()
		{
			lengthInInches = 0.0f;
			hairType = HairType.NO_HAIR;
			isGrowing = false;
			color = HairFurColors.HUMAN_DEFAULT;
			isSemiTransparent = false;
		}

		public static Hair Generate()
		{
			return new Hair();
		}

		public bool StyleHair(HAIR_STYLE newStyle)
		{
			if (this.style == HAIR_STYLE.NOT_APPLICABLE)
			{
				return false;
			}
			style = newStyle;
			return lengthInInches != 0 && style == newStyle;
		}

		public bool GrowAndStyle(HAIR_STYLE newStyle, int deltaLength, bool activateIfPossible = false)
		{
			if (this.)
			if (this.hairType.preventHairGrowth)
			{
				return false;
			}
			if (style == HAIR_STYLE.NOT_APPLICABLE && !this.isGrowing)
			{
				return false;
			}
			style = newStyle;
			//delta length?
			//return isGrowing && style == newStyle;
		}

		public bool CutLong(HAIR_STYLE newStyle)
		{

		}
		public bool CutMedium(HAIR_STYLE newStyle)
		{

		}
		public bool CutShort(HAIR_STYLE newStyle)
		{

		}

		public bool ShaveOff(HAIR_STYLE newStyle, bool disableHairGrowth = false)
		{

		}

		public bool ShaveOffAndDeactivateGrowth()
		{
		}

		#region Dyeable

		public HairFurColors color { get; private set; }

		public override int index => hairType.index;

		public override GenericDescription shortDescription { get; protected set; }

		public bool canDye()
		{
			return hairType.canDye();
		}

		public bool attemptToDye(HairFurColors dye)
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

		public virtual bool canCut()
		{
			return true;
		}

		public virtual bool canLengthen()
		{
			return true;
		}

		public virtual bool tryToDye(ref HairFurColors currentColor, HairFurColors newColor)
		{
			currentColor = newColor;
			return currentColor == newColor;
		}


		protected HairType(GenericDescription desc)
		{
			_index = indexMaker++;
			shortDescription = desc;
			preventHairGrowth = false;
		}

		public static readonly HairType NO_HAIR = new NoHair();
		public static readonly HairType NORMAL = new HairType(NormalStr);
        public static readonly HairType FEATHER = new HairType(FeatherStr);
        public static readonly HairType GOO = new HairType(GooStr);
        public static readonly HairType ANEMONE = new AnemoneHair();
        public static readonly HairType QUILL = new HairType(QuillStr);
		//Spines are a huge dick - they break the format that hair actually is hair (or hair-like)
		
		//Solution: Spines take the hair color of the original and convert it to the closest lizard tone
		//you can say something like "the spines clash with the rest of your scales, hinting at their unnatural origins"
		//then if you really want to, the next TF you can force the spines in line by dyeing them with a lizard related dye closest to their current tone. 
		//at that point you can then say "the spines 
		public static readonly HairType BASILISK_SPINES = new HairType(BasiliskSpinesStr); 
		//end complain
        public static readonly HairType BASILISK_PLUME = new HairType(BasiliskPlumesStr);
        public static readonly HairType WOOL = new HairType(WoolStr);
        public static readonly HairType LEAF = new HairType(LeafStr);

		private class NoHair : HairType
		{
			public NoHair() : base(NoHairStr) { }

			public override bool canCut()
			{
				return false;
			}

			public override bool canLengthen()
			{
				return false;
			}

			public override bool canDye()
			{
				return false;
			}

			public override bool tryToDye(ref HairFurColors currentColor, HairFurColors newColor)
			{
				return false;
			}

		}


		private class AnemoneHair : HairType
		{
			public AnemoneHair() : base(AnemoneStr) {}

			public override bool canCut()
			{
				return false;
			}

			public override bool canLengthen()
			{
				return false;
			}

		}

		private class BasiliskSpines : HairType
		{
			protected BasiliskSpines() : base(BasiliskSpinesStr)
			{
				
			}

			public override bool canCut()
			{
				return false;
			}

			public override bool canLengthen()
			{
				return false;
			}

			public override bool canDye()
			{
				return false;
			}

			public override bool tryToDye(ref HairFurColors currentColor, HairFurColors newColor)
			{
				return false;
			}
		}

	}

}
