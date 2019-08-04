//Hair.cs
//Description:
//Author: JustSomeGuy
//6/25/2019, 1:28 AM
using CoC.Backend.Attacks;
using CoC.Backend.Attacks.BodyPartAttacks;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Races;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CoC.Backend.BodyParts
{
	//feel free to add other styles. 
	//try to keep the styles somewhat simplistic, as it needs to make sense no matter how long the hair gets. we're making an erotic text game, not the Sims (or a more realistic depiction thereof)
	//i've added it because i think it's kinda cool to be able to say "wavy hair" or "curly tentacle-hair", or allow your pc to more accurately match what you have in mind (or yourself, if that's what you prefer)
	//IMO it's nice to have the option to create a black character with type-4 (coiled) hair, as that's a common thing that doesn't often get represented in games (mostly because it's hard as hell to render)
	//But at the same time, i'm aware there's a lot more hairstyles, and we're not really able to make all of them. For example, i've never seen an afro that's 6 feet tall, and i doubt physics supports it.

	//in theory, you could do something where you choose a "cut" (not a style), like a bob or whatever, and as it grows out it just gradually turns into a curly/wavy/straight mess.
	//It can be done, but from my experience it's a massive headache with little gain (tried it with beards). If someone wants to try it, go for it. 

	//not included, but contemplated: Mohawk, Undercut, Pixi-Cut, Afro/Jew-fro, cornrows. they all seemed really dependant on hair length being a certain size.  
	//also, contemplating removing ponytail (and to lesser extent braid), as it doesn't work with really short hair (i suppose it'd be more of a bun at short lengths), but Easter Egg demands it.
	//An aside: some webcomic i read a while ago called an undercut a "sideways mohawk" or something of the like in passing (iirc a guy was apologizing to his friend for calling it that), and now i can't unsee it. 
	//in the event you know the above webcomic feel free to add a url here and give it an unofficial shout-out. Mad props if you do - JSG

	public enum HairStyle { NO_STYLE, MESSY, STRAIGHT, BRAIDED, WAVY, CURLY, COILED, PONYTAIL }

	//i'm unsure of behavior for hair color for PCs with NO_HAIR. no hair is for things that literally have no hair, like lizard-morphs and such.
	//but the PC can change that via TFs, so do i store the old color so there's continuity? or should i be pedantic and say that they technically no longer have hair follicles,
	//so there's no way to keep the old color? 

	//right now, my solution is to keep the old color, even if the PC is bald/NO_HAIR, but in the event this changes or someone accidently clears the color, every time the hairType changes
	//the new type checks to see if the color is null or empty and replaces it with their default (which is not null or empty) if it is. 

	public sealed partial class Hair : BehavioralSaveablePart<Hair, HairType>, IBuildAware, ISimultaneousMultiDyeable, ICanAttackWith, IBodyPartTimeLazy
	{
		internal static readonly HairFurColors DEFAULT_COLOR = HairFurColors.BLACK;

		//right now, accelerated growth lasts 8 hours. changing the accelorator level to non-zero will reset this to 8 hours. Changing it to zero will immediately set the duration to zero.
		public const byte ACCELERATOR_DURATION = 8;
		//accelerated growth can be stacked 3 times. 
		//if you want to go nuts, you can set this up to a max of 126. after that, we are no longer in the range of float. personally, i'd highly recommend leaving it at 3, as that's already
		//1.1 inches of growth an hour, up from the default of 0.1, though you could probably get away with a value up to 5 or 6 (5 is 4.7 inches per hour, 6 is 9.5)
		//alternatively, you could alter the growth rate function, but whatever.
		public const byte MAX_ACCELERATOR_LEVEL = 3;


		//currently only limited by the range of a float in centimeters. we store things in imperial measures, however, so this is actually limited to 
		//max float divided by 2.54. In the future a more convenient value (like say, 360, or 30 feet long) may be used, but for now, i'm not limiting it. Go nuts, people!
		public const float MAX_LENGTH = (float)(float.MaxValue * Measurement.TO_INCHES);
		public const float MIN_LENGTH = 0;

		#region Unique Members
		//make sure to check this when deserializing - if you dont use the property is may cause errors.
		public HairFurColors hairColor
		{
			get => _hairColor;
			private set
			{
				if (!HairFurColors.IsNullOrEmpty(value))
				{
					_hairColor = value;
				}
			}
		}
		private HairFurColors _hairColor = null;

		//used for accelerated growth. as of now the extension serum is the only way to procc this, but it could technically be caused by anything. 

		//make sure to leave room for this when we are loading a save. it's rare, but it could be saved.
		public byte growthAccelerationLevel
		{
			get => _growthAccelerationLevel;
			private set
			{
				if (value > _growthAccelerationLevel)
				{
					growthCountdownTimer = ACCELERATOR_DURATION;
				}
				_growthAccelerationLevel = Utils.Clamp2<byte>(value, 0, MAX_ACCELERATOR_LEVEL);
			}
		}
		private byte _growthAccelerationLevel = 0;
		private byte growthCountdownTimer = 0;

		public HairFurColors highlightColor
		{
			get => _highlightColor;
			private set => _highlightColor = value ?? HairFurColors.NO_HAIR_FUR;
		}
		private HairFurColors _highlightColor = HairFurColors.NO_HAIR_FUR;

		public bool isGrowing => type?.growsOverTime == true && !growthArtificallyDisabled;

		//in theory, this bool would allow players to "magically" stop hair growth, allowing them, for example, to grow cornrows or a mohawk
		//and not have to worry about their hair being too short or too long to realistically be that style.
		//right now it's always false, because i don't know how we'd deal with this bool on tf - always reset it?
		private bool growthArtificallyDisabled = false;

		public bool isSemiTransparent { get; private set; } = false;

		public float length
		{
			get => _length;
			private set => _length = Utils.Clamp2(value, MIN_LENGTH, MAX_LENGTH);
		}
		private float _length = 10;

		//growthAcceleration level is capped at 3 (right now)
		//i use the sequence currently used in game (1,2), (2,5), (3,11), which implies => (4,23), (5,47), etc.
		//that sequence is a(n) = 2 * a(n-1) + 1; n > 0. 
		//my discreet math is a bit rusty, but fortunately wolfram exists. That's equal to 
		//a(n) = 3*2^(n-1) - 1, which holds for all n > 0. 
		//for 0, we use 1. 

		//use growth rate for all values not 0, otherwise use 1.
		private float growthMultiplier => growthAccelerationLevel > 0 ? growthRateFn(growthAccelerationLevel) : 1.0f;

		//max for amount: 127 after that we're out of range for a float. 
		//floats are weird, man. max is 1.99999999999999999999999^128 iirc
		private float growthRateFn(byte amount)
		{
			//3*2^(n-1) -1;
			//takes advantage of the fact that 2^n == 1 << (n+1). a bit shift right is the same as multiplying a number by 2 each time you shift it. right shift 5 of x and x * 2^5 are identical.
			//thus, 2^(n-1) == 1 << n. bit shifts use massively less resources than Pow, though idk if they were smart enough to optimize in powers of 2.
			//for reference, a bit shift is a single opcode in every assembly i'm aware of. technically a multiply is two opcodes, and pow is a series of multiplies. 
			//plus C# abstraction, accounting for floating point values, etc. long story short, this is faster. 
			return 3 * (1 << amount) - 1;
		}

		public HairStyle style { get; private set; }
		#endregion

		#region Constructors

		private Hair(HairType hairType)
		{
			_type = hairType ?? throw new ArgumentNullException(nameof(hairType));
			_length = type.defaultHairLength;
			hairColor = type.defaultColor;
			isSemiTransparent = false;
			style = HairStyle.NO_STYLE;
		}
		#endregion
		#region BodyPartProperties
		public override HairType type
		{
			get => _type;
			protected set
			{
				if (_type != value)
				{
					float len = length;
					HairStyle sty = style;
					value.changeTypeFrom(_type, ref len, ref _hairColor, ref _highlightColor, ref sty);
					style = sty;
					length = len;

					_type = value;
				}
			}
		}
		private HairType _type;
		public static HairType defaultType => HairType.NORMAL;
		public override bool isDefault => type == defaultType;
		#endregion
		#region  Generate

		internal static Hair GenerateDefault()
		{
			return new Hair(HairType.NORMAL);
		}

		internal static Hair GenerateDefaultOfType(HairType hairType)
		{
			return new Hair(hairType);
		}
		internal static Hair GenerateWithLength(HairType hairType, float hairLength)
		{
			Hair retVal = new Hair(hairType);
			retVal.SetHairLength(hairLength);
			return retVal;
		}

		internal static Hair GenerateWithColor(HairType hairType, HairFurColors color, float? hairLength = null)
		{
			Hair retVal = new Hair(hairType);
			if (hairLength != null) retVal.SetHairLength((float)hairLength);
			retVal.SetHairColor(color);
			return retVal;
		}

		internal static Hair GenerateWithColorAndHighlight(HairType hairType, HairFurColors color, HairFurColors highlight, float? hairLength = null)
		{
			Hair retVal = new Hair(hairType);
			if (hairLength != null) retVal.SetHairLength((float)hairLength);
			retVal.SetHairColor(color);
			retVal.SetHighlightColor(highlight);
			return retVal;
		}
		#endregion
		#region Update

		internal override bool UpdateType(HairType newType)
		{
			if (newType == null || type == newType)
			{
				return false;
			}
			type = newType;
			return true;

		}

		//i'm gonna be lazy, fuck it. these are internal, anyway. the helpers are on the player class, so i can just parse it there. 

		//Updates the type. returns true if it actually changed type. if it didn't change type, all other variables are ignored, and it returns false
		//all variables beyond type are optional. All hair will first do its default action on transform, then update with any values here. 
		//this is to prevent overriding when calling the tf on change effects. 
		internal bool UpdateType(HairType newType, HairFurColors newHairColor = null, HairFurColors newHighlightColor = null, float? newHairLength = null, HairStyle? newStyle = null, bool ignoreCanLengthenOrCut = false)
		{
			if (newType == null || type == newType)
			{
				return false;
			}
			//auto call the type tf change function. 
			type = newType;

			//if we have a new, valid hair length, and we can set the length
			if (newHairLength is float validLength && !type.isFixedLength)
			{
				//if we can cut it and our new length is shorter, or if we can lengthen and our length is longer, the length is not already correct
				if ((type.canCut && validLength < length) || (type.canLengthen && validLength > length) || (validLength != length && ignoreCanLengthenOrCut))
				{
					SetHairLength(validLength);
				}
				//otherwise, we can't do anything or we're already the correct length, so do nothing.
			}
			if (!HairFurColors.IsNullOrEmpty(newHairColor))
			{
				SetHairColor(newHairColor);
			}
			if (newHighlightColor != null)
			{
				SetHighlightColor(newHighlightColor);
			}
			if (newStyle != null)
			{
				SetHairStyle((HairStyle)newStyle);
			}
			return true;
		}

		//only returns false if it cannot 
		internal bool SetHairColor(HairFurColors newHairColor, bool clearHighlights = false)
		{
			if (!type.canDye || HairFurColors.IsNullOrEmpty(newHairColor))
			{
				return false;
			}
			hairColor = newHairColor;
			return true;
		}

		internal bool SetHighlightColor(HairFurColors newHighlightColor)
		{
			if (!type.canDye || HairFurColors.IsNullOrEmpty(newHighlightColor))
			{
				return false;
			}
			highlightColor = newHighlightColor;
			return true;
		}

		//returns false if either fail. returns true if both succeed. as of now, it's either both or none, though.
		internal bool SetBothHairColors(HairFurColors hairColor, HairFurColors highlightColor)
		{
			//single &: force both to run. double &: don't run right if left is false. 99% of time && is ideal. not here.
			return SetHairColor(hairColor) & SetHighlightColor(highlightColor);
		}

		//sets the hair style to the new value, returning true if the hairStyle is now the newStyle.
		//will return false if the current hair type cannot be styled.
		internal bool SetHairStyle(HairStyle newStyle)
		{
			if (type.canStyle)
			{
				style = newStyle;
				return true;
			}
			else
			{
				return false;
			}
		}

		//use this to force a hair to be a certain length
		//variables such as can cut or can lengthen will be ignored. 
		//of course, if the type has a single, fixed size for hair length, this will not be possible, and therefore return false.
		//otherwise, it will return true. 
		internal bool SetHairLength(float newLength)
		{
			if (type.isFixedLength)
			{
				return false;
			}
			length = newLength;
			return true;

		}

		internal bool SetTransparency(bool isTransparent)
		{
			isSemiTransparent = isTransparent;
			return true;
		}

		//Use these if you want to directly change your hair's length. if you want to do it naturally, alter the growth rate.
		//by default, this function will take into consideration if your current hair type allows you to artifically lengthen it. 
		//though you do have the option to ignore this variable. Note that even if you ignore this variable, types with a fixed hair Length
		//will never change length.
		//returns the amount the hair grew.
		internal float GrowHair(float byAmount, bool ignoreCanLengthen = false)
		{
			float currLen = length;
			if (type.canLengthen || (ignoreCanLengthen && !type.isFixedLength))
			{
				length += byAmount;
			}
			return length - currLen;
		}

		//Use these if you want to directly shorten the hair's length. i can't think of a natural way this would happen,
		//Unless you find a reason to go "all the hair fell out, then grew back in, but shorter"
		//by default, this function will take into consideration if your current hair type allows you to cut it. 
		//though you do have the option to ignore this variable. Note that even if you ignore this variable, types with a fixed hair Length
		//will never change length.
		//returns the amount the hair shortened.

		internal float ShortenHair(float byAmount, bool ignoreCanCut = false)
		{
			float currLen = length;
			if (type.canCut || (!type.isFixedLength && ignoreCanCut))
			{
				length -= byAmount;
			}
			return currLen - length;
		}

		#endregion
		#region Restore
		internal override bool Restore()
		{
			if (type == HairType.NORMAL)
			{
				return false;
			}
			type = HairType.NORMAL;
			return true;
		}
		internal void Reset()
		{
			type = HairType.NORMAL;
			length = 0f;
			style = HairStyle.NO_STYLE;
			hairColor = type.defaultColor;
			highlightColor = HairFurColors.NO_HAIR_FUR;
		}
		#endregion
		#region Validate
		internal override bool Validate(bool correctInvalidData)
		{

			float len = length;
			HairStyle hairStyle = style;
			bool valid = HairType.Validate(ref _type, ref len, ref _hairColor, ref _highlightColor, ref hairStyle, correctInvalidData);
			style = hairStyle;
			length = len;
			return valid;
		}
		#endregion
		#region HairAwareHelper
		private HairData ToHairData()
		{
			return new HairData(type, hairColor, highlightColor, style, length, isSemiTransparent, !isGrowing);
		}

		internal void SetupHairAware(IHairAware hairAware)
		{
			hairAware.GetHairData(ToHairData);
		}

		#endregion
		#region Dyeable
		byte IMultiDyeable.numDyeableMembers => 2;

		bool IMultiDyeable.allowsDye(byte index)
		{
			if (index >= numDyeMembers)
			{
				return false;
			}
			else if (index <= 1)
			{
				return type.canDye;
			}
			else
			{
				throw new NotImplementedException("Somebody added a new dyeable member and didn't implement it correctly.");
			}
		}

		bool IMultiDyeable.isDifferentColor(HairFurColors dyeColor, byte index)
		{
			if (index >= numDyeMembers)
			{
				return false;
			}
			else if (index == 1)
			{
				return dyeColor != highlightColor;
			}
			else if (index == 0)
			{
				return dyeColor != hairColor;
			}
			else
			{
				throw new NotImplementedException("Somebody added a new dyeable member and didn't implement it correctly.");
			}
		}

		bool IMultiDyeable.attemptToDye(HairFurColors dye, byte index)
		{
			IMultiDyeable instance = this;
			if (!instance.allowsDye(index) || !instance.isDifferentColor(dye, index))
			{
				return false;
			}
			else if (index == 1)
			{
				return type.tryToDye(ref _highlightColor, false, in _hairColor, dye);
			}
			else
			{
				return type.tryToDye(ref _hairColor, true, in _highlightColor, dye);
			}
		}

		string IMultiDyeable.buttonText(byte index)
		{
			if (index >= numDyeMembers)
			{
				return "";
			}
			else if (index == 1)
			{
				return HighlightStr();
			}
			else if (index == 0)
			{
				return HairStr();
			}
			else
			{
				throw new System.NotImplementedException("Hair's multidyeable was not fully implemented. Consider fixing this. ");
			}
		}

		string IMultiDyeable.locationDesc(byte index)
		{
			if (index >= numDyeMembers)
			{
				return "";
			}
			else if (index == 1)
			{
				return YourHighlightsStr();
			}
			else if (index == 0)
			{
				return YourHairStr();
			}
			else
			{
				throw new System.NotImplementedException("Hair's multidyeable was not fully implemented. Consider fixing this. ");
			}
		}

		private IMultiDyeable dyeable => this;
		private byte numDyeMembers => dyeable.numDyeableMembers;
		#endregion
		#region ICanAttackWith
		AttackBase ICanAttackWith.attack => type.attack;
		bool ICanAttackWith.canAttackWith() => type.canAttackWith(this);
		#endregion
		#region ITimeListener
		string IBodyPartTimeLazy.reactToTimePassing(bool isPlayer, byte hoursPassed)
		{

			StringBuilder sb = new StringBuilder();
			HairStyle hairStyle = style;
			float newLength = length;

			byte growMin = Math.Min(hoursPassed, growthCountdownTimer);

			float unitsGrown = growthMultiplier * growMin;

			if (hoursPassed >= growthCountdownTimer)
			{
				growthCountdownTimer = 0;
				growthAccelerationLevel = 0;

				unitsGrown += hoursPassed.subtract(growthCountdownTimer);
				if (isPlayer)
				{
					sb.Append(NoMoreAcceleratedGrowthFrownyFace());
				}
			}
			else
			{
				growthCountdownTimer -= hoursPassed;
			}

			//run the hair lengthening regardless, but only do the output if it's the player.
			if (type.reactToTimePassing(ref newLength, ref _hairColor, ref _highlightColor, ref hairStyle, hoursPassed, unitsGrown, out string specialHappenstance) && isPlayer)
			{
				if (!string.IsNullOrWhiteSpace(specialHappenstance))
				{
					sb.Append(specialHappenstance);
				}
				byte tallness = buildData().heightInInches;
				if (newLength > 0 && length < 0.01)
				{
					sb.Append(NoLongerBaldStr());
				}
				else if ((newLength >= 1 && length < 1) || (newLength >= 3 && length < 3) || (newLength >= 6 && length < 6) ||
					(newLength >= 10 && length < 10) || (newLength >= 16 && length < 16) || (newLength >= 26 && length < 26) ||
					(newLength >= 40 && length < 40) || (newLength >= 40 && newLength >= tallness && length < tallness))
				{
					sb.Append(HairLongerStr());

				}

			}
			style = hairStyle;
			length = newLength;

			return sb.ToString();
		}

		#endregion
		#region BuildData
		private BuildDataGetter buildData;
		void IBuildAware.GetBuildData(BuildDataGetter getter)
		{
			buildData = getter;
		}
		#endregion
	}

	public abstract partial class HairType : SaveableBehavior<HairType, Hair>
	{
		private static readonly List<HairType> hairTypes = new List<HairType>();
		public static readonly ReadOnlyCollection<HairType> availableTypes = new ReadOnlyCollection<HairType>(hairTypes);
		private static int indexMaker = 0;



		public readonly HairFurColors defaultColor;
		public readonly float defaultHairLength;


		public override int index => _index;
		private readonly int _index;

		//a boolean that determines if hair can change length at all. 
		//which by default is if they grow over time or can be artificially lengthened.

		//note that living hair override this, because they can be forced via TF items, but 
		//otherwise do not grow or get cut.
		public virtual bool isFixedLength => !growsOverTime && !canLengthen;

		public abstract bool growsOverTime { get; }

		//for now, simple descriptors. if it's discovered during conversion that i need to know the player for things like ears or antennae or whatever, or the hair length,
		//i could convert to player str or player and type delegate. 
		public readonly SimpleDescriptor flavorTextForCutting;
		public readonly SimpleDescriptor flavorTextForMagicHairGrowth;

		public abstract bool canCut { get; }
		public abstract bool canLengthen { get; }

		public abstract bool canStyle { get; } //lets you prevent styling. 

		public abstract bool canDye { get; }
		private protected HairType(HairFurColors defaultHairColor, float defaultLength, SimpleDescriptor shortDesc, DescriptorWithArg<Hair> fullDesc, TypeAndPlayerDelegate<Hair> playerDesc,
			SimpleDescriptor growFlavorText, SimpleDescriptor cutFlavorText,
			ChangeType<Hair> transform, RestoreType<Hair> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			hairTypes.AddAt(this, _index);

			defaultColor = defaultHairColor;
			defaultHairLength = defaultLength;

			flavorTextForCutting = cutFlavorText;
			flavorTextForMagicHairGrowth = growFlavorText;

		}

		//you also get the other color you're not dyeing, and a bool to tell you which one you're currently dyeing. may not really be necessary, but you can have them anyway. 
		internal virtual bool tryToDye(ref HairFurColors currentColor, bool isPrimaryHair, in HairFurColors otherHairColor, HairFurColors newColor)
		{
			if (canDye)
			{
				currentColor = newColor;
				return true;
			}
			return false;
		}
		internal virtual AttackBase attack => AttackBase.NO_ATTACK;
		internal virtual bool canAttackWith(Hair hair) => attack != AttackBase.NO_ATTACK;

		//validate is called after deserialization. This means that the data passed in is NOT null, with exception to type. length is positive.

		internal static bool Validate(ref HairType type, ref float length, ref HairFurColors primaryColor, ref HairFurColors highlightColor, ref HairStyle hairStyle, bool correctInvalidData)
		{
			bool valid = true;
			if (!hairTypes.Contains(type))
			{
				if (!correctInvalidData)
				{
					return false;
				}
				valid = false;
				type = NORMAL;
			}
			return valid & type.ValidateData(ref length, ref primaryColor, ref highlightColor, ref hairStyle, correctInvalidData);
		}

		private protected abstract bool ValidateData(ref float length, ref HairFurColors primaryColor, ref HairFurColors highlightColor, ref HairStyle hairStyle, bool correctInvalidData);

		internal abstract void changeTypeFrom(HairType oldType, ref float length, ref HairFurColors primaryColor, ref HairFurColors highlightColor, ref HairStyle hairStyle);


		//super complicated but a lot more flexible this way - basically, you can choose to set anything you want as time passes. Does having living hair mean your hair randomly gets tangled?
		//you can now make that happen. The cost is you now HAVE to set a Special Output string, but if you don't have anything to say, just set it to "";
		//Don't worry about saying that the hair grew to a certain length relative to the player - that's taken care of by the Hair class, and it'll be appended after any other special text you set here. 
		//also, return true if something happened to the hair - even if it didn't change length, the game needs to know that you messed with the hair style, for example. false otherwise.

		internal abstract bool reactToTimePassing(ref float length, ref HairFurColors primaryColor, ref HairFurColors highlightColor, ref HairStyle hairStyle, byte hoursPassed,
			float unitsGrown, out string SpecialOutput);


		private static Func<float, float> KeepSize() => (x) => x;
		private static Func<float, float> KeepSizeUnlessBald(float defaultSize) => (x) => x != 0 ? x : defaultSize; //currently unused, idk if that's a behavior someone would prefer.
		private static Func<float, float> AtLeastThisBig(float defaultSize) => (x) => x > defaultSize ? x : defaultSize;
		public static Func<float, float> SetTo(float defaultSize) => (x) => defaultSize;

		public static readonly HairType NO_HAIR = new NoHair(); //0.0
		public static readonly HairType NORMAL = new NormalHair();
		public static readonly HairType FEATHER = new GenericHairType(HairFurColors.WHITE, 5.0f, KeepSize(), FeatherDesc, FeatherFullDesc, FeatherPlayerStr, FeatherGrowStr, FeatherCutStr, FeatherTransformStr, FeatherRestoreStr);
		public static readonly HairType GOO = new GenericHairType(HairFurColors.CERULEAN, 5.0f, AtLeastThisBig(5.0f), GooDesc, GooFullDesc, GooPlayerStr, GooGrowStr, GooCutStr, GooTransformStr, GooRestoreStr); //5 is if bald. updating behavior to <5 or bald to 5 inch. just say your old type 
		public static readonly HairType ANEMONE = new LivingHair(Species.ANEMONE.defaultHair, 8.0f, new AnemoneSting(), AnemoneDesc, AnemoneFullDesc, AnemonePlayerStr, AnemoneNoGrowStr, AnemoneNoCutStr, AnemoneTransformStr, AnemoneRestoreStr);
		public static readonly HairType QUILL = new GenericHairType(HairFurColors.WHITE, 12.0f, SetTo(12.0f), QuillDesc, QuillFullDesc, QuillPlayerStr, QuillGrowStr, QuillCutStr, QuillTransformStr, QuillRestoreStr); //shoulder length. not set though. whoops.
		public static readonly HairType BASILISK_SPINES = new BasiliskSpines();
		public static readonly HairType BASILISK_PLUME = new GenericHairType(Species.BASILISK.defaultPlume, 2.0f, SetTo(2.0f), PlumeDesc, PlumeFullDesc, PlumePlayerStr, PlumeGrowStr, PlumeCutStr, PlumeTransformStr, PlumeRestoreStr); //2
		public static readonly HairType WOOL = new GenericHairType(HairFurColors.WHITE, 1.0f, KeepSizeUnlessBald(1.0f), WoolDesc, WoolFullDesc, WoolPlayerStr, WoolGrowStr, WoolCutStr, WoolTransformStr, WoolRestoreStr); //not defined. 
		public static readonly HairType LEAF = new LivingHair(Species.DRYAD.defaultVineColor, 12.0f, AttackBase.NO_ATTACK, VineDesc, VineFullDesc, VinePlayerStr, VineNoGrowStr, VineNoCutStr, VineTransformStr, VineRestoreStr);

		private class NoHair : HairType
		{
			public NoHair() : base(HairFurColors.BLACK, 0.0f, NoHairDesc, NoHairFullDesc, NoHairPlayerStr, NoHairToGrow, NoHairToCut, NoHairTransformStr, NoHairRestoreStr) { }

			public override bool growsOverTime => false;

			public override bool canCut => false;

			public override bool canLengthen => false;

			public override bool canDye => false;

			public override bool canStyle => false;

			internal override void changeTypeFrom(HairType oldType, ref float length, ref HairFurColors primaryColor, ref HairFurColors highlightColor, ref HairStyle hairStyle)
			{
				length = defaultHairLength;
				//keep the main color. See top of this file for why i chose this, but feel free to override me.

				//we'll lose the highlight though, that wouldn't make sense to come back.
				highlightColor = HairFurColors.NO_HAIR_FUR;
				//we'll also lose the hair style.
				hairStyle = HairStyle.NO_STYLE;
			}

			//all data is guarenteed to be NOT NULL. length is guarenteed to be positive.
			private protected override bool ValidateData(ref float length, ref HairFurColors primaryColor, ref HairFurColors highlightColor, ref HairStyle hairStyle, bool correctInvalidData)
			{
				bool valid = true;

				if (length > 0.001 || length < 0.001) //let weird floating point shit go.
				{
					if (correctInvalidData)
					{
						length = 0f;
					}
					valid = false;
				}
				//leave primary color, for now, at least.

				if ((valid || correctInvalidData) && highlightColor != HairFurColors.NO_HAIR_FUR)
				{
					if (correctInvalidData)
					{
						highlightColor = HairFurColors.NO_HAIR_FUR;
					}
					valid = false;
				}

				if ((valid || correctInvalidData) && hairStyle != HairStyle.NO_STYLE)
				{
					if (correctInvalidData)
					{
						hairStyle = HairStyle.NO_STYLE;
					}
					valid = false;
				}

				return valid;
			}

			internal override bool reactToTimePassing(ref float length, ref HairFurColors primaryColor, ref HairFurColors highlightColor,
				ref HairStyle hairStyle, byte hoursPassed, float unitsGrown, out string SpecialOutput)
			{
				SpecialOutput = "";
				if (length != 0)
				{
					length = 0;
					return true;
				}
				return false;
			}
		}

		//basically, this lets us "cheat", so we don't need to implement each hair type. most act the same way, so we can do this.
		private class GenericHairType : HairType
		{
			//one exception is that some hair types act differently when transforming. some keep the hair length, some reset it to a specific length, 
			//and still others force the hair to be at least a certain length, or they will grow to that length. So, we use a function callback here to set the size on transform
			//and this solves our problem. 
			private readonly Func<float, float> SetHairLengthOnTransform;

			//this allows you to define text for magically growing hair, or cutting hair. It's supposed to be useable for anything, but right now the only place this happens is the hair salon.
			//i'll modify existing text to a sort of generic intro text for both the hair you can cut, and the hair you can't. this will be appended on to it. 

			public override bool canLengthen => true;

			public override bool canCut => true;

			public override bool canStyle => true;

			public GenericHairType(HairFurColors defaultHairColor, float defaultLength, Func<float, float> handleHairLengthOnTransform,
				SimpleDescriptor shortDesc, DescriptorWithArg<Hair> fullDesc, TypeAndPlayerDelegate<Hair> playerDesc, SimpleDescriptor growStr,
				SimpleDescriptor cutStr, ChangeType<Hair> transform, RestoreType<Hair> restore)
				: base(defaultHairColor, defaultLength, shortDesc, fullDesc, playerDesc, growStr, cutStr, transform, restore)
			{
				SetHairLengthOnTransform = handleHairLengthOnTransform;
			}

			public override bool growsOverTime => true;
			public override bool canDye => true;

			//all data is guarenteed to be NOT NULL. length is guarenteed to be positive.
			private protected override bool ValidateData(ref float length, ref HairFurColors primaryColor, ref HairFurColors highlightColor, ref HairStyle hairStyle, bool correctInvalidData)
			{
				//highlight is fine, length is fine. so is style.
				//so this is all we need to check.

				if (primaryColor.isEmpty)
				{
					if (correctInvalidData)
					{
						primaryColor = defaultColor;
					}
					return false;
				}
				return true;
			}

			internal override void changeTypeFrom(HairType oldType, ref float length, ref HairFurColors primaryColor, ref HairFurColors highlightColor, ref HairStyle hairStyle)
			{
				length = SetHairLengthOnTransform(length);
				if (HairFurColors.IsNullOrEmpty(primaryColor))
				{
					primaryColor = defaultColor;
				}
				//ehh, we'll keep the hair color if not empty, and we'll keep the highlight too. if you want to change this either derive it or put in a callback, i don't really care.
				//same with the hair style. 
			}

			internal override bool reactToTimePassing(ref float length, ref HairFurColors primaryColor, ref HairFurColors highlightColor, ref HairStyle hairStyle,
				byte hoursPassed, float unitsGrown, out string SpecialOutput)
			{
				SpecialOutput = "";
				length += unitsGrown * 0.1f;
				return true;
			}
		}

		private class NormalHair : GenericHairType
		{
			public NormalHair() : base(HairFurColors.BLACK, 0.0f, KeepSize(), NormalDesc, NormalFullDesc, NormalPlayerStr, NormalGrowStr, NormalCutStr, NormalTransformStr, NormalRestoreStr) { }

			internal override AttackBase attack => _attack;
			private static readonly AttackBase _attack = new HairWhip();
			//this could have been done as a special extended content in the frontend, but i've already got the attackwith interface for anemone hair, might as well do it here. 
			internal override bool canAttackWith(Hair hair) //only allow it if it's Ret2Go! (or if it's braided a la harem style)
			{
				return hair.length >= 36.0f && (hair.style == HairStyle.BRAIDED || hair.style == HairStyle.PONYTAIL) && hair.hairColor == HairFurColors.PURPLE;
			}
		}

		//living hair cannot be cut or lengthened, but you may force this with certain items (like anemone hair getting longer when you use an anemone tf item)
		private class LivingHair : HairType
		{
			//can't cut or lengthen
			public override bool canCut => false;
			public override bool canLengthen => false;
			//but we still can change length. 
			public override bool isFixedLength => false;

			public override bool canDye => true;
			public override bool growsOverTime => false;

			public override bool canStyle => true; //i dunno, you may want to prevent this. for now i'll allow it.

			internal override AttackBase attack => _attack; //caught a stack overflow here because attack was attack.
			private readonly AttackBase _attack;

			public LivingHair(HairFurColors defaultHairColor, float defaultLength, AttackBase attack, SimpleDescriptor shortDesc,
				DescriptorWithArg<Hair> fullDesc, TypeAndPlayerDelegate<Hair> playerDesc, SimpleDescriptor whyNoGrowingDesc,
				SimpleDescriptor whyNoCuttingDesc, ChangeType<Hair> transform, RestoreType<Hair> restore)
				: base(defaultHairColor, defaultLength, shortDesc, fullDesc, playerDesc, whyNoGrowingDesc, whyNoCuttingDesc, transform, restore)
			{
				_attack = attack;
			}

			private protected override bool ValidateData(ref float length, ref HairFurColors primaryColor, ref HairFurColors highlightColor, ref HairStyle hairStyle, bool correctInvalidData)
			{
				//highlight is fine, length is fine. so is style.
				//so this is all we need to check.

				if (primaryColor.isEmpty)
				{
					if (correctInvalidData)
					{
						primaryColor = defaultColor;
					}
					return false;
				}
				return true;
			}

			internal override void changeTypeFrom(HairType oldType, ref float length, ref HairFurColors primaryColor, ref HairFurColors highlightColor, ref HairStyle hairStyle)
			{
				length = defaultHairLength;
				if (HairFurColors.IsNullOrEmpty(primaryColor))
				{
					primaryColor = defaultColor;
				}
				//clear the highlights.
				highlightColor = HairFurColors.NO_HAIR_FUR;
				//we technically don't prevent highlights from happening, as you can dye these. i dunno, all hair was dyeable before. i'm just going with it - JSG.
				//living hair wants to be free, so remove the styles. maybe you want the vines to be braided, idk. if so, derive it, do what you want. 
				hairStyle = HairStyle.NO_STYLE;

			}

			internal override bool reactToTimePassing(ref float length, ref HairFurColors primaryColor, ref HairFurColors highlightColor, ref HairStyle hairStyle,
				byte hoursPassed, float unitsGrown, out string SpecialOutput)
			{
				SpecialOutput = "";
				length += unitsGrown * 0.1f;
				return true;
			}
		}

		private class BasiliskSpines : HairType
		{
			public BasiliskSpines() : base(Species.BASILISK.defaultSpines, 2.0f, SpineDesc, SpineFullDesc, SpinePlayerStr, SpineNoGrowStr, SpineNoCutStr, SpineTransformStr, SpineRestoreStr) { }

			public override bool growsOverTime => false;

			public override bool canCut => false;

			public override bool canLengthen => false;

			public override bool canDye => false;

			public override bool canStyle => false;

			//all data is guarenteed to be NOT NULL. length is guarenteed to be positive.
			private protected override bool ValidateData(ref float length, ref HairFurColors primaryColor, ref HairFurColors highlightColor, ref HairStyle hairStyle, bool correctInvalidData)
			{
				bool valid = true;

				if (length > defaultHairLength + 0.01 || length < defaultHairLength - 0.01) //handle floating point shit.
				{
					if (correctInvalidData)
					{
						length = defaultHairLength;
					}
					valid = false;
				}
				else if (length != defaultHairLength) //catch weird floating point BS. treat this as valid though.
				{
					length = defaultHairLength;
				}
#warning when basilisk spine colors are defined, make sure it's a spine color. correct with basilisk spine color function.


				if ((valid || correctInvalidData) && highlightColor != HairFurColors.NO_HAIR_FUR)
				{
					if (correctInvalidData)
					{
						highlightColor = HairFurColors.NO_HAIR_FUR;
					}
					valid = false;
				}

				if ((valid || correctInvalidData) && hairStyle != HairStyle.NO_STYLE)
				{
					if (correctInvalidData)
					{
						hairStyle = HairStyle.NO_STYLE;
					}
					valid = false;
				}

				return valid;
			}

			internal override void changeTypeFrom(HairType oldType, ref float length, ref HairFurColors primaryColor, ref HairFurColors highlightColor, ref HairStyle hairStyle)
			{
				length = defaultHairLength;
				if (HairFurColors.IsNullOrEmpty(primaryColor))
				{
					primaryColor = defaultColor;
				}
				else
				{
					primaryColor = Species.BASILISK.ToNearestSpineColor(primaryColor);
				}
				if (!HairFurColors.IsNullOrEmpty(highlightColor))
				{
					highlightColor = Species.BASILISK.ToNearestSpineColor(highlightColor);
				}
				hairStyle = HairStyle.NO_STYLE;

			}

			internal override bool reactToTimePassing(ref float length, ref HairFurColors primaryColor, ref HairFurColors highlightColor, ref HairStyle hairStyle,
				byte hoursPassed, float unitsGrown, out string SpecialOutput)
			{
				SpecialOutput = "";
				return false;
			}
		}
	}

	internal sealed class HairData
	{
		internal readonly HairFurColors hairColor;
		internal readonly HairFurColors highlightColor;
		internal readonly HairType hairType;
		internal readonly HairStyle hairStyle;
		internal readonly float hairLength;
		internal readonly bool isSemiTransparent;
		internal readonly bool isNotGrowing;
		internal bool isNoHair => hairType == HairType.NO_HAIR;
		internal bool hairDeactivated => hairType == HairType.NO_HAIR || (hairLength == 0 && isNotGrowing);
		internal bool isBald => isNoHair || hairLength == 0;

		internal HairFurColors activeHairColor => hairDeactivated ? HairFurColors.NO_HAIR_FUR : hairColor;

		internal HairData(HairType type, HairFurColors color, HairFurColors highlight, HairStyle style, float hairLen, bool semiTransparent, bool notGrowing)
		{
			hairType = type;
			hairColor = color;
			highlightColor = highlight;
			hairStyle = style;
			hairLength = hairLen;
			isSemiTransparent = semiTransparent;
			isNotGrowing = notGrowing;
		}

		internal HairData()
		{
			hairType = HairType.NO_HAIR;
			hairColor = HairFurColors.NO_HAIR_FUR;
			highlightColor = HairFurColors.NO_HAIR_FUR;
			hairStyle = HairStyle.NO_STYLE;
			hairLength = 0;
			isSemiTransparent = false;
			isNotGrowing = true;
		}
	}
}
