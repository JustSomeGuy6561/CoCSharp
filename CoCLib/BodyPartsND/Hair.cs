//Hair.cs
//Description:
//Author: JustSomeGuy
//12/27/2018, 9:50 PM
using System;
using System.Collections.Generic;
using CoC.EpidermalColors;
using  CoC.BodyParts.SpecialInteraction;
using CoC.Tools;
using static  CoC.BodyParts.HairStrings;
using static CoC.UI.TextOutput;
using CoC.Engine;
using CoC.Creatures;

namespace  CoC.BodyParts
{


	public enum HairStyle { NORMAL, MESSY, STRAIGHT, BRAIDED, WAVY, CURLY, COILED }

	public class Hair : BodyPartBase<Hair, HairType>, IDyeable, ITimeAware
	{
		private static readonly HairFurColors HUMAN_DEFAULT = HairFurColors.BLACK;

		/*
		 * Hair can grow. if used on a creature where this isn't an issue (basically everyone but the player)
		 * Don't attach it to the time aware system. 
		 */

		//Members
		#region Members
		//system related
		private HashSet<IHairAware> hairAwares = new HashSet<IHairAware>();
		private int hoursCounter = 0;
		//regular
		public bool isGrowing { get; protected set; }
		public bool isSemiTransparent { get; protected set; }
		public float lengthInInches { get; protected set; }
		protected int hoursSinceTrimmed;

		public HairStyle style { get; protected set; }

		public override HairType type { get; protected set; }

		public HairFurColors color
		{
			get => _color;
			private set
			{
				_color = value;
				if (value != HairFurColors.NO_HAIR_FUR)
				{
					lastGoodColor = value;
				}
			}
		}
		private HairFurColors _color;
		private HairFurColors lastGoodColor;

		#endregion
		#region Constructors
		//Constructor
		protected Hair()
		{
			lengthInInches = 0.0f;
			type = HairType.NO_HAIR;
			isGrowing = false;
			color = HUMAN_DEFAULT;
			isSemiTransparent = false;
			style = HairStyle.NORMAL;
		}

		protected Hair(HairType hairType)
		{
			lengthInInches = hairType.defaultHairLength;
			type = hairType;
			isGrowing = hairType != HairType.NO_HAIR;
			color = HUMAN_DEFAULT;
			isSemiTransparent = false;
			style = HairStyle.NORMAL;
		}
		#endregion
		#region Custom Descriptions
		public string shortDescriptionWithColor()
		{
			return ColoredHairDesc(this);
		}

		public string ColoredStyledDescription()
		{
			return ColoredStyledHairDescript(this);
		}
		#endregion
		#region Generate
		public static Hair Generate(HairType hairType)
		{
			return new Hair(hairType);
		}

		public static Hair Generate(HairType hairType, float length, HairFurColors hairColor, HairStyle hairStyle = HairStyle.NORMAL, bool growing = true, bool transparent = false)
		{
			if (hairType == HairType.NO_HAIR) return new Hair();
			else
				return new Hair(hairType)
				{
					lengthInInches = length,
					color = hairColor,
					style = hairStyle,
					isGrowing = growing,
					isSemiTransparent = transparent
				};
		}

		public static Hair GenerateNoHair()
		{
			return new Hair();
		}
		#endregion
		#region Restore
		public override bool Restore()
		{
			if (type == HairType.NORMAL)
			{
				return false;
			}
			type = HairType.NORMAL;
			isSemiTransparent = false;
			return true;
		}

		public bool RestoreKeepTransparency()
		{
			if (type == HairType.NORMAL)
			{
				return false;
			}
			type = HairType.NORMAL;
			return true;
		}

		public override bool RestoreAndDisplayMessage(Player player)
		{
			if (type == HairType.NORMAL)
			{
				return false;
			}
			OutputText(restoreString(player));
			return Restore();
		}

		public bool RestoreKeepTransparencyAndDisplayMessage(Player player)
		{
			if (type == HairType.NORMAL)
			{
				return false;
			}
			OutputText(restoreString(player));
			return RestoreKeepTransparency();
		}
		#endregion


		#region Activate/Deactivate Growth
		//If hair can grow, sets it to grow and returns true. returns true if 
		//hair is already growing. if something prevents it from growing, return false
		public bool ActivateGrowth()
		{
			if (!type.preventHairGrowth)
			{
				isGrowing = true;
				if (color == HairFurColors.NO_HAIR_FUR)
				{
					color = lastGoodColor;
				}
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
		#endregion
		#region Grow/Cut/Style Hair
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
		//unless you have no hair, you can set your style. 
		public bool StyleHair(HairStyle newStyle)
		{
			if (this.type == HairType.NO_HAIR || !this.isGrowing)
			{
				return false;
			}
			style = newStyle;
			return lengthInInches != 0 && style == newStyle;
		}

		public bool GrowAndStyle(HairStyle newStyle, int deltaLength, bool activateIfPossible = false)
		{
			if (type == HairType.NO_HAIR )
			{
				return false;
			}
			if (style == HairStyle.NOT_APPLICABLE && !this.isGrowing)
			{
				return false;
			}
			style = newStyle;
			//delta length?
			//return isGrowing && style == newStyle;
		}

		public bool CutLong(HairStyle newStyle)
		{

		}
		public bool CutMedium(HairStyle newStyle)
		{

		}
		public bool CutShort(HairStyle newStyle)
		{

		}

		public bool ShaveOff(HairStyle newStyle, bool disableHairGrowth = false)
		{

		}

		public bool ShaveOffAndDeactivateGrowth()
		{
		}
		#endregion
		#region Dyeable
		public bool canDye()
		{
			return type.canDye;
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
		#region HairAware related
		public bool AttachIHairAware(IHairAware item)
		{
			bool added = hairAwares.Add(item);
			if (added)
			{
				HairAware += item.reactToChangeInHairColor;
			}
			return added;
		}
		public bool DetachIHairAware(IHairAware item)
		{
			bool removed = hairAwares.Remove(item);
			if (removed)
			{
				HairAware -= item.reactToChangeInHairColor;
			}
			return removed;
		}

		private void HairChanged()
		{

			OnHairAware(new HairColorEventArg(color));
		}


		protected virtual void OnHairAware(HairColorEventArg e)
		{
			HairAware?.Invoke(this, e);
		}

		private event EventHandler<HairColorEventArg> HairAware;
		#endregion

		#region TimeAware
		public void ReactToTimePassing(uint hoursPassed)
		{
			this.hoursCounter += (int)hoursPassed;
			if (hoursCounter >= 12)
			{
				if (!type.preventHairGrowth)
				{
					GrowHair(hoursCounter / 12);
				}
				hoursCounter %= 12;
			}
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

	public class HairType : BodyPartBehavior<HairType, Hair>
	{
		private static int indexMaker = 0;

		public override int index => _index;
		private int _index;
		public virtual bool preventHairGrowth => false;

		public virtual bool canDye => true;

		public virtual bool canCut => true;
		public virtual bool canLengthen => true;

		public readonly float defaultHairLength;
		//Worth noting: As of vanilla, all hair (even basilisk plumes) can be dyed.
		//It can now be disabled, but default behavior is to allow it. to disable,
		//or to conditionally allow dyeing, implement a subclass of Hairtype and 
		//override canDye and possibly tryToDye with the new rules.
		protected HairType(float defaultLength,
			SimpleDescriptor shortDesc, DescriptorWithArg<Hair> fullDesc, TypeAndPlayerDelegate<Hair> playerDesc,
			ChangeType<Hair> transform, RestoreType<Hair> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			defaultHairLength = defaultLength;
		}


		public virtual bool tryToDye(ref HairFurColors currentColor, HairFurColors newColor)
		{
			if (canDye)
			{
				currentColor = newColor;
				return true;
			}
			return false;
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
		public static readonly HairType BASILISK_SPINES = new BasiliskSpines()
		//end complain
		public static readonly HairType BASILISK_PLUME = new HairType(BasiliskPlumesStr);
		public static readonly HairType WOOL = new HairType(WoolStr);
		public static readonly HairType LEAF = new HairType(LeafStr);

		private class NoHair : HairType
		{
			public NoHair() : base(NoHairStr) { }

			public override bool preventHairGrowth => true;
			public override bool canCut => false;
			public override bool canLengthen => false;
			public override bool canDye => false;

			public override bool tryToDye(ref HairFurColors currentColor, HairFurColors newColor)
			{
				return false;
			}

		}


		private class AnemoneHair : HairType
		{
			public AnemoneHair() : base(AnemoneStr) { }

			public override bool canCut => false;

			public override bool canLengthen => false;

		}

		private class BasiliskSpines : HairType
		{
			public BasiliskSpines() : base(BasiliskSpinesStr)
			{

			}

			public override bool canCut => false;

			public override bool canLengthen => false;

			public override bool canDye => false;

			public override bool tryToDye(ref HairFurColors currentColor, HairFurColors newColor)
			{
				return false;
			}

			public override bool preventHairGrowth => true;
		}

	}


	public class HairColorEventArg : EventArgs
	{
		public readonly HairFurColors hairColor;

		public HairColorEventArg(HairFurColors hair)
		{
			hairColor = hair;
		}
	}
}
