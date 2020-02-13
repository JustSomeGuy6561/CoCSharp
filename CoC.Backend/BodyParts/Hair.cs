//Hair.cs
//Description:
//Author: JustSomeGuy
//6/25/2019, 1:28 AM
using CoC.Backend.Attacks;
using CoC.Backend.Attacks.BodyPartAttacks;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
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

	//defaults to all hair.
	internal enum HairDyeLocations:byte { ALL_HAIR, MAIN_COLOR, HIGHLIGHT}

	internal static class HairDyeHelper
	{
		internal static bool IsDefined(this HairDyeLocations location)
		{
			return Enum.IsDefined(typeof(HairDyeLocations), location);
		}
	}

	//i'm unsure of behavior for hair color for PCs with NO_HAIR. no hair is for things that literally have no hair, like lizard-morphs and such.
	//but the PC can change that via TFs, so do i store the old color so there's continuity? or should i be pedantic and say that they technically no longer have hair follicles,
	//so there's no way to keep the old color?

	//right now, my solution is to keep the old color, even if the PC is bald/NO_HAIR, but in the event this changes or someone accidently clears the color, every time the hairType changes
	//the new type checks to see if the color is null or empty and replaces it with their default (which is not null or empty) if it is.

	//also: we use the catch-all term 'highlights' for any sort of two-tone styling with hair. if we're being pedantic, this is somewhat incorrect - if the second tone is darker
	//than the primary tone, it instead uses the generic term 'lowlights'. It's possible to convert RGB to HSL and then use the lightness value to compare if two colors are
	//lighter or darker than one another, and thus correctly use "highlight" or "lowlight". But i'm not fucking with that.

	public sealed partial class Hair : FullBehavioralPart<Hair, HairType, HairData>, IMultiDyeable, ICanAttackWith, IBodyPartTimeLazy
	{
		public override string BodyPartName() => Name();


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

		private BuildData buildData => CreatureStore.TryGetCreature(creatureID, out Creature creature) ? creature.build.AsReadOnlyData() : new BuildData(creatureID);
		private Tones skinTone => CreatureStore.GetCreatureClean(creatureID)?.body.primarySkin.tone ?? Tones.LIGHT;
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

		public bool hasHighlights => !HairFurColors.IsNullOrEmpty(highlightColor);

		public bool isGrowing => type?.growsOverTime == true && !growthArtificallyDisabled;

		public bool canGrowNaturally => type.growsOverTime;
		//this bool allows us to disable hair growth, even if the type currently allows hair growth. this is useful primarily for reptile tfs, though
		//in theory, it could also be used to allow players to "magically" stop hair growth, allowing them, for example, to grow cornrows or a mohawk
		//and not have to worry about their hair being too short or too long to realistically be that style.
		//currently, however, it's only used for lizard tfs and anything that resets them. perhaps we can think of a more standardized approach later.
		public bool growthArtificallyDisabled { get; private set; } = false;

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

		public bool isBald => length == 0 || type == HairType.NO_HAIR;

		public bool hairDeactivated => type == HairType.NO_HAIR || (length == 0 && !isGrowing);
		public HairFurColors activeHairColor => hairDeactivated ? HairFurColors.NO_HAIR_FUR : hairColor;

		#region Constructors

		internal Hair(Guid creatureID) : this(creatureID, HairType.defaultValue)
		{ }

		internal Hair(Guid creatureID, HairType hairType, HairFurColors color = null, HairFurColors highlight = null, float? hairLength = null,
			HairStyle? hairStyle = null, bool hairTransparent = false) : base(creatureID)
		{
			_type = hairType ?? throw new ArgumentNullException(nameof(hairType));


			_length = type.defaultHairLength;
			if (!type.isFixedLength && hairLength is float validLength) SetHairLengthPrivate(validLength);

			style = hairStyle ?? HairStyle.NO_STYLE;

			isSemiTransparent = false;

			hairColor = type.defaultColor;
			SetHairColorPrivate(color, true);
			SetHighlightColorPrivate(highlight);

			this.isSemiTransparent = hairTransparent;
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
					value.ChangeTypeFrom(_type, ref len, ref _hairColor, ref _highlightColor, ref sty, skinTone);
					style = sty;
					length = len;

					_type = value;
				}
			}
		}
		private HairType _type;
		public override HairType defaultType => HairType.defaultValue;
		#endregion
		#region  Generate


		#endregion
		#region Update

		internal override bool UpdateType(HairType newType)
		{
			if (newType is null || newType == type)
			{
				return false;
			}

			var oldValue = type;
			var oldData = AsReadOnlyData();
			type = newType;

			CheckDataChanged(oldData);
			NotifyTypeChanged(oldValue);
			return true;
		}


		//i'm gonna be lazy, fuck it. these are internal, anyway. the helpers are on the player class, so i can just parse it there.

		//Updates the type. returns true if it actually changed type. if it didn't change type, all other variables are ignored, and it returns false
		//all variables beyond type are optional. All hair will first do its default action on transform, then update with any values here.
		//this is to prevent overriding when calling the tf on change effects.
		internal bool UpdateType(HairType newType, bool? enableHairGrowth = null, HairFurColors newHairColor = null, HairFurColors newHighlightColor = null, float? newHairLength = null, HairStyle? newStyle = null, bool ignoreCanLengthenOrCut = false)
		{
			if (newType == null || type == newType)
			{
				return false;
			}
			var oldData = AsReadOnlyData();
			var oldType = type;
			//auto call the type tf change function.
			type = newType;

			//if we have a new, valid hair length, and we can set the length
			if (newHairLength is float validLength && !type.isFixedLength)
			{
				//if we can cut it and our new length is shorter, or if we can lengthen and our length is longer, the length is not already correct
				if ((type.canCut && validLength < length) || (type.canLengthen && validLength > length) || (validLength != length && ignoreCanLengthenOrCut))
				{
					SetHairLengthPrivate(validLength);
				}
				//otherwise, we can't do anything or we're already the correct length, so do nothing.
			}
			//set the disabled flag. currently only used by reptile tf and anything that negates it. the behavior isn't standardized across tfs. perhaps we should make it so.
			if (enableHairGrowth is bool isDisabled)
			{
				growthArtificallyDisabled = isDisabled;
			}

			if (!HairFurColors.IsNullOrEmpty(newHairColor))
			{
				SetHairColorPrivate(newHairColor, false);
			}
			if (newHighlightColor != null)
			{
				SetHighlightColorPrivate(newHighlightColor);
			}
			if (newStyle != null)
			{
				SetHairStylePrivate((HairStyle)newStyle);
			}

			CheckDataChanged(oldData);
			NotifyTypeChanged(oldType);
			return true;
		}


		public bool SetHairColor(HairFurColors newHairColor, bool clearHighlights = false)
		{
			return HandleHairChange(() => SetHairColorPrivate(newHairColor, clearHighlights));
		}

		private bool SetHairColorPrivate(HairFurColors newHairColor, bool clearHighlights)
		{
			if (!type.canDye || HairFurColors.IsNullOrEmpty(newHairColor))
			{
				return false;
			}
			hairColor = newHairColor;
			return true;
		}

		public bool SetHighlightColor(HairFurColors newHighlightColor)
		{
			return HandleHairChange(() => SetHighlightColorPrivate(newHighlightColor));
		}

		private bool SetHighlightColorPrivate(HairFurColors newHighlightColor)
		{
			if (!type.canDye || HairFurColors.IsNullOrEmpty(newHighlightColor))
			{
				return false;
			}
			highlightColor = newHighlightColor;
			return true;
		}

		//returns false if either fail. returns true if both succeed. as of now, it's either both or none, though.
		public bool SetBothHairColors(HairFurColors hairColor, HairFurColors highlightColor)
		{
			//single &: force both to run. double &: don't run right if left is false. 99% of time && is ideal. not here.
			return HandleHairChange(() => SetHairColorPrivate(hairColor, false) & SetHighlightColorPrivate(highlightColor));
		}


		public bool SetHairStyle(HairStyle newStyle)
		{
			return HandleHairChange(() => SetHairStylePrivate(newStyle));
		}

		//sets the hair style to the new value, returning true if the hairStyle is now the newStyle.
		//will return false if the current hair type cannot be styled.
		private bool SetHairStylePrivate(HairStyle newStyle)
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

		public bool SetHairLength(float newLength)
		{
			return HandleHairChange(() => SetHairLengthPrivate(newLength));
		}

		private bool SetHairLengthPrivate(float newLength)
		{
			if (type.isFixedLength)
			{
				return false;
			}
			length = newLength;
			return true;

		}

		public bool SetTransparency(bool isTransparent)
		{
			return HandleHairChange(() => SetTransparencyPrivate(isTransparent));
		}


		private bool SetTransparencyPrivate(bool isTransparent)
		{
			isSemiTransparent = isTransparent;
			return true;
		}

		//Use these if you want to directly change your hair's length. if you want to do it naturally, alter the growth rate.
		//by default, this function will take into consideration if your current hair type allows you to artifically lengthen it.
		//though you do have the option to ignore this variable. Note that even if you ignore this variable, types with a fixed hair Length
		//will never change length.
		//returns the amount the hair grew.
		public float GrowHair(float byAmount, bool ignoreCanLengthen = false)
		{
			var oldData = AsReadOnlyData();
			float currLen = length;
			if (type.canLengthen || (ignoreCanLengthen && !type.isFixedLength))
			{
				length += byAmount;
			}

			if (length != currLen)
			{
				NotifyDataChanged(oldData);
			}
			return length - currLen;
		}

		//Use these if you want to directly shorten the hair's length. i can't think of a natural way this would happen,
		//Unless you find a reason to go "all the hair fell out, then grew back in, but shorter"
		//by default, this function will take into consideration if your current hair type allows you to cut it.
		//though you do have the option to ignore this variable. Note that even if you ignore this variable, types with a fixed hair Length
		//will never change length.
		//returns the amount the hair shortened.

		public float ShortenHair(float byAmount, bool ignoreCanCut = false)
		{
			var oldData = AsReadOnlyData();
			float currLen = length;
			if (type.canCut || (!type.isFixedLength && ignoreCanCut))
			{
				length -= byAmount;
			}

			if (length != currLen)
			{
				NotifyDataChanged(oldData);
			}
			return currLen - length;
		}

		public bool SetAll(float? newLength = null, bool? enableHairGrowth = null, HairFurColors mainColor = null, HairFurColors highlight = null, HairStyle? style = null)
		{

			return HandleHairChange(() => SetAllPrivate(newLength, enableHairGrowth, mainColor, highlight, style));
		}

		private bool SetAllPrivate(float? newLength = null, bool? enableHairGrowth = null, HairFurColors mainColor = null, HairFurColors highlight = null, HairStyle? style = null)
		{
			bool retVal = false;
			if (newLength is float length)
			{
				retVal |= SetHairLengthPrivate(length);
			}
			if (enableHairGrowth is bool hairGrowth)
			{
				retVal |= SetHairGrowthStatus(hairGrowth);
			}

			if (mainColor is HairFurColors validHair)
			{
				retVal |= SetHairColorPrivate(validHair, false);
			}
			if (highlight is HairFurColors validHighlight)
			{
				retVal |= SetHighlightColorPrivate(validHighlight);
			}
			if (style is HairStyle validStyle)
			{
				retVal |= SetHairStylePrivate(validStyle);
			}
			return retVal;
		}
		private T HandleHairChange<T>(Func<T> callback)
		{
			var oldData = AsReadOnlyData();
			T retVal = callback();
			CheckDataChanged(oldData);
			return retVal;
		}

		//NOTE: this only affects the artificially disabled flag, it does not have any effect on the current type. if the type does not allow hair growth, it will not have any
		//effect on the is growing flag. it will return true if it updated the artificially disabled flag, false otherwise.
		//growing makes more sense here from a caller standpoint, but unfortunately here we store the opposite bool. so these checks are backward.
		public bool SetHairGrowthStatus(bool growing)
		{
			return HandleHairChange(() => SetHairGrowthStatusPrivate(growing));
		}

		private bool SetHairGrowthStatusPrivate(bool growing)
		{
			//if we want to set it to growing, but it's disabled, or we want to set it to not growing and it's enabled, these two will be equal.
			if (growing == growthArtificallyDisabled)
			{
				growthArtificallyDisabled = !growing;
				return true;
			}
			else
			{
				return false;
			}
		}

		public bool StopNaturalGrowth()
		{
			return SetHairGrowthStatus(false);
		}

		public bool ResumeNaturalGrowth()
		{
			return SetHairGrowthStatus(true);
		}

		private void CheckDataChanged(HairData oldData)
		{
			if (style != oldData.style || length != oldData.length || hairColor != oldData.hairColor || highlightColor != oldData.highlightColor
				|| isSemiTransparent != oldData.isSemiTransparent || oldData.isGrowing != isGrowing)
			{
				NotifyDataChanged(oldData);
			}
		}

		#endregion
		#region Restore

		//default restore is fine.

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
		#region IdenticalTo
		public override bool IsIdenticalTo(HairData original, bool ignoreSexualMetaData)
		{
			return !(original is null) && type == original.type && hairDeactivated == original.hairDeactivated
				&& (hairDeactivated || (hairColor == original.hairColor && highlightColor == original.highlightColor
				&& length == original.length && this.isSemiTransparent == original.isSemiTransparent && this.style == original.style));
		}
		#endregion
		#region Extra Strings
		public string DescriptionWithTransparency(bool alternateFormat = false) => type.DescriptionWithTransparency(isSemiTransparent, alternateFormat);

		public string DescriptionWithColor(bool alternateFormat = false) => type.DescriptionWithColor(AsReadOnlyData(), alternateFormat);

		public string DescriptionWithColorAndStyle(bool alternateFormat = false) => type.DescriptionWithColorAndStyle(AsReadOnlyData(), alternateFormat);

		public string DescriptionWithColorLengthAndStyle(bool alternateFormat = false) => type.DescriptionWithColorLengthAndStyle(AsReadOnlyData(), alternateFormat);

		public string FullDescription(bool alternateFormat = false) => type.FullDescription(AsReadOnlyData(), alternateFormat);



		#endregion
		#region HairAwareHelper
		public override HairData AsReadOnlyData()
		{
			return new HairData(creatureID, type, hairColor, highlightColor, style, length, isSemiTransparent, isGrowing);
		}

		#endregion
		#region Dyeable
		//hair, highlight, both. all 3 are always available, assuming the creature has hair.
		byte IMultiDyeable.numDyeableMembers => 3;

		bool IMultiDyeable.allowsDye(byte index)
		{
			if (index >= numDyeMembers)
			{
				return false;
			}
			else
			{
				return type.canDye;
			}
		}

		bool IMultiDyeable.isDifferentColor(HairFurColors dyeColor, byte index)
		{
			HairDyeLocations location = (HairDyeLocations)index;
			if (!location.IsDefined())
			{
				return false;
			}
			else if (location == HairDyeLocations.MAIN_COLOR)
			{
				return dyeColor != hairColor;
			}
			else if (location == HairDyeLocations.HIGHLIGHT)
			{
				return dyeColor != highlightColor;
			}
			else
			{
				return dyeColor != highlightColor || dyeColor != hairColor;
			}
		}

		bool IMultiDyeable.attemptToDye(HairFurColors dye, byte index)
		{
			IMultiDyeable instance = this;
			if (!instance.allowsDye(index) || !instance.isDifferentColor(dye, index))
			{
				return false;
			}
			//if we've reached this point, we can assume location is defined. no need to check it.
			HairDyeLocations location = (HairDyeLocations)index;

			bool success = false;



			if (location != HairDyeLocations.MAIN_COLOR)
			{
				success |= type.tryToDye(ref _highlightColor, false, _hairColor, dye);
			}

			if (location != HairDyeLocations.HIGHLIGHT)
			{
				success |= type.tryToDye(ref _hairColor, true, _highlightColor, dye);
			}

			return success;
		}

		string IMultiDyeable.buttonText()
		{
			return Name();
		}



		string IMultiDyeable.memberButtonText(byte index)
		{
			HairDyeLocations location = (HairDyeLocations)index;
			switch (location)
			{
				case HairDyeLocations.ALL_HAIR:
					return AllHairStr();
				case HairDyeLocations.HIGHLIGHT:
					return HighlightStr();
				case HairDyeLocations.MAIN_COLOR:
					return RegularHairStr();
				default:
					return "";
			}
		}

		string IMultiDyeable.memberLocationDesc(byte index, out bool isPlural)
		{
			HairDyeLocations location = (HairDyeLocations)index;
			switch (location)
			{
				case HairDyeLocations.ALL_HAIR:
					return YourHairAllStr(out isPlural);
				case HairDyeLocations.HIGHLIGHT:
					return YourHighlightsStr(out isPlural);
				case HairDyeLocations.MAIN_COLOR:
					return YourHairRegularStr(out isPlural);
				default:
					isPlural = false;
					return "";
			}
		}

		string IMultiDyeable.memberPostDyeDescription(byte index)
		{
			HairDyeLocations location = (HairDyeLocations)index;
			if (!location.IsDefined())
			{
				return "";
			}
			else
			{
				return LongDescription();
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

			float unitsGrown;
			if (growthCountdownTimer > 0 && growthAccelerationLevel > 0)
			{
				byte growMin = Math.Min(hoursPassed, growthCountdownTimer);

				unitsGrown = growthMultiplier * growMin;

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
			}
			else
			{
				unitsGrown = hoursPassed;
			}

			//run the hair lengthening regardless, but only do the output if it's the player.
			if (type.reactToTimePassing(ref newLength, ref _hairColor, ref _highlightColor, ref hairStyle, hoursPassed, unitsGrown, out string specialHappenstance) && isPlayer)
			{
				if (!string.IsNullOrWhiteSpace(specialHappenstance))
				{
					sb.Append(specialHappenstance);
				}
				byte tallness = buildData.heightInInches;
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

		public bool IsBasiliskHair()
		{
			return type.IsBasiliskHair();
		}
	}

	public abstract partial class HairType : FullBehavior<HairType, Hair, HairData>
	{
		private static readonly List<HairType> hairTypes = new List<HairType>();
		public static readonly ReadOnlyCollection<HairType> availableTypes = new ReadOnlyCollection<HairType>(hairTypes);
		private static int indexMaker = 0;

		public static HairType defaultValue => NORMAL;

		public readonly HairFurColors defaultColor;
		public readonly float defaultHairLength;


		public override int id => _index;
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

		private protected HairType(HairFurColors defaultHairColor, float defaultLength, SimpleDescriptor shortDesc, SimpleDescriptor strandsOfShort, PartDescriptor<HairData> longDesc,
			SimpleDescriptor growFlavorText, SimpleDescriptor cutFlavorText,
			ChangeType<HairData> transform, RestoreType<HairData> restore) : base(shortDesc, strandsOfShort, longDesc, DefaultPlayerDesc, transform, restore)
		{
			_index = indexMaker++;
			hairTypes.AddAt(this, _index);

			defaultColor = defaultHairColor;
			defaultHairLength = defaultLength;

			flavorTextForCutting = cutFlavorText;
			flavorTextForMagicHairGrowth = growFlavorText;

		}
		private protected HairType(HairFurColors defaultHairColor, float defaultLength, SimpleDescriptor shortDesc, SimpleDescriptor strandsOfShort, PartDescriptor<HairData> longDesc,
			PlayerBodyPartDelegate<Hair> playerDesc, SimpleDescriptor growFlavorText, SimpleDescriptor cutFlavorText,
			ChangeType<HairData> transform, RestoreType<HairData> restore) : base(shortDesc, strandsOfShort, longDesc, playerDesc, transform, restore)
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


		internal abstract void ChangeTypeFrom(HairType oldType, ref float length, ref HairFurColors primaryColor, ref HairFurColors highlightColor, ref HairStyle hairStyle, in Tones skinTone);


		//super complicated but a lot more flexible this way - basically, you can choose to set anything you want as time passes. Does having living hair mean your hair randomly gets
		//tangled? you can now make that happen. The cost is you now HAVE to set a Special Output string, but if you don't have anything to say, just set it to "";
		//Don't worry about saying that the hair grew to a certain length relative to the player - that's taken care of by the Hair class, and it'll be appended after any other
		//special text you set here. also, return true if something happened to the hair - even if it didn't change length, the game needs to know that you messed with the hair style,
		//for example. false otherwise.
		internal abstract bool reactToTimePassing(ref float length, ref HairFurColors primaryColor, ref HairFurColors highlightColor, ref HairStyle hairStyle, byte hoursPassed,
			float unitsGrown, out string SpecialOutput);

		public bool IsBasiliskHair()
		{
			return this == BASILISK_PLUME || this == BASILISK_SPINES;
		}

		protected static Func<float, float> KeepSize() => (x) => x;
		protected static Func<float, float> KeepSizeUnlessBald(float defaultSize) => (x) => x != 0 ? x : defaultSize; //currently unused, idk if that's a behavior someone would prefer.
		protected static Func<float, float> AtLeastThisBig(float defaultSize) => (x) => x > defaultSize ? x : defaultSize;
		protected static Func<float, float> SetTo(float defaultSize) => (x) => defaultSize;

		public static readonly HairType NO_HAIR = new NoHair(); //0.0
		public static readonly HairType NORMAL = new NormalHair();
		public static readonly HairType FEATHER = new GenericHairType(HairFurColors.WHITE, 5.0f, KeepSize(), FeatherDesc, FewFeathersDesc, FeatherLongDesc, FeatherGrowStr, FeatherCutStr, FeatherTransformStr, FeatherRestoreStr);
		public static readonly HairType GOO = new GenericHairType(HairFurColors.CERULEAN, 5.0f, AtLeastThisBig(5.0f), GooDesc, BitOfGooDesc, GooLongDesc, GooGrowStr, GooCutStr, GooTransformStr, GooRestoreStr); //5 is if bald. updating behavior to <5 or bald to 5 inch. just say your old type
		public static readonly HairType ANEMONE = new LivingHair(DefaultValueHelpers.defaultAnemoneHair, 8.0f, new AnemoneSting(), FewTendrilsDesc, AnemoneDesc, AnemoneLongDesc, AnemoneNoGrowStr, AnemoneNoCutStr, AnemoneTransformStr, AnemoneRestoreStr);
		public static readonly HairType QUILL = new GenericHairType(HairFurColors.WHITE, 12.0f, SetTo(12.0f), QuillDesc, FewQuillsDesc, QuillLongDesc, QuillGrowStr, QuillCutStr, QuillTransformStr, QuillRestoreStr); //shoulder length. not set though. whoops.
		public static readonly HairType BASILISK_SPINES = new BasiliskSpines();
		public static readonly HairType BASILISK_PLUME = new GenericHairType(DefaultValueHelpers.defaultBasiliskPlume, 2.0f, SetTo(2.0f), PlumeDesc, SomeOfPlumeDesc, PlumeLongDesc, PlumeGrowStr, PlumeCutStr, PlumeTransformStr, PlumeRestoreStr); //2
		public static readonly HairType WOOL = new GenericHairType(HairFurColors.WHITE, 1.0f, KeepSizeUnlessBald(1.0f), WoolDesc, BitOfWoolDesc, WoolLongDesc, WoolGrowStr, WoolCutStr, WoolTransformStr, WoolRestoreStr); //not defined.
		public static readonly HairType LEAF = new LivingHair(DefaultValueHelpers.defaultVineColor, 12.0f, AttackBase.NO_ATTACK, VineDesc, SomeOfVineDesc, VineLongDesc, VineNoGrowStr, VineNoCutStr, VineTransformStr, VineRestoreStr);

		private class NoHair : HairType
		{
			public NoHair() : base(HairFurColors.BLACK, 0.0f, NoHairDesc, NoHairStrandsDesc, NoHairLongDesc, NoHairToGrow, NoHairToCut, NoHairTransformStr, NoHairRestoreStr) { }

			public override bool growsOverTime => false;

			public override bool canCut => false;

			public override bool canLengthen => false;

			public override bool canDye => false;

			public override bool canStyle => false;

			internal override void ChangeTypeFrom(HairType oldType, ref float length, ref HairFurColors primaryColor, ref HairFurColors highlightColor, ref HairStyle hairStyle, in Tones skinTone)
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
				SimpleDescriptor shortDesc, PartDescriptor<HairData> longDesc, SimpleDescriptor strandDesc, PlayerBodyPartDelegate<Hair> playerDesc, SimpleDescriptor growStr,
				SimpleDescriptor cutStr, ChangeType<HairData> transform, RestoreType<HairData> restore)
				: base(defaultHairColor, defaultLength, shortDesc, strandDesc, longDesc, playerDesc, growStr, cutStr, transform, restore)
			{
				SetHairLengthOnTransform = handleHairLengthOnTransform;
			}

			public GenericHairType(HairFurColors defaultHairColor, float defaultLength, Func<float, float> handleHairLengthOnTransform,
				SimpleDescriptor shortDesc, SimpleDescriptor strandDesc, PartDescriptor<HairData> longDesc, SimpleDescriptor growStr,
				SimpleDescriptor cutStr, ChangeType<HairData> transform, RestoreType<HairData> restore)
				: base(defaultHairColor, defaultLength, shortDesc, strandDesc, longDesc, growStr, cutStr, transform, restore)
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

			internal override void ChangeTypeFrom(HairType oldType, ref float length, ref HairFurColors primaryColor, ref HairFurColors highlightColor, ref HairStyle hairStyle, in Tones skinTone)
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
			public NormalHair() : base(HairFurColors.BLACK, 0.0f, KeepSize(), NormalDesc, NormalStrandDesc, NormalLongDesc, NormalGrowStr, NormalCutStr, NormalTransformStr, NormalRestoreStr) { }

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

			public LivingHair(HairFurColors defaultHairColor, float defaultLength, AttackBase attack, SimpleDescriptor shortDesc, SimpleDescriptor strandDesc,
				PartDescriptor<HairData> longDesc, SimpleDescriptor whyNoGrowingDesc, SimpleDescriptor whyNoCuttingDesc, ChangeType<HairData> transform, RestoreType<HairData> restore)
				: base(defaultHairColor, defaultLength, shortDesc, strandDesc, longDesc, whyNoGrowingDesc, whyNoCuttingDesc, transform, restore)
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

			internal override void ChangeTypeFrom(HairType oldType, ref float length, ref HairFurColors primaryColor, ref HairFurColors highlightColor, ref HairStyle hairStyle, in Tones skinTone)
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
			public BasiliskSpines() : base(DefaultValueHelpers.defaultBasiliskSpines, 2.0f, SpineDesc, BunchOSpinesDesc,
				SpineLongDesc, SpineNoGrowStr, SpineNoCutStr, SpineTransformStr, SpineRestoreStr) { }

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

			internal override void ChangeTypeFrom(HairType oldType, ref float length, ref HairFurColors primaryColor, ref HairFurColors highlightColor, ref HairStyle hairStyle, in Tones skinTone)
			{
				length = defaultHairLength;
				if (HairFurColors.IsNullOrEmpty(primaryColor))
				{
					primaryColor = defaultColor;
				}
				else
				{
					primaryColor = DefaultValueHelpers.ToNearestSpineColor(primaryColor, skinTone);
				}
				if (!HairFurColors.IsNullOrEmpty(highlightColor))
				{
					highlightColor = DefaultValueHelpers.ToNearestSpineColor(highlightColor, skinTone);
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

	public sealed class HairData : FullBehavioralData<HairData, Hair, HairType>
	{
		public readonly HairFurColors hairColor;
		public readonly HairFurColors highlightColor;
		public readonly HairStyle style;
		public readonly float length;
		public readonly bool isSemiTransparent;
		public readonly bool isGrowing;
		public bool isNoHair => type == HairType.NO_HAIR;
		public bool hairDeactivated => type == HairType.NO_HAIR || (length == 0 && !isGrowing);
		public bool isBald => isNoHair || length == 0;

		public HairFurColors activeHairColor => hairDeactivated ? HairFurColors.NO_HAIR_FUR : hairColor;

		public string DescriptionWithTransparency(bool alternateFormat = false) => type.DescriptionWithTransparency(isSemiTransparent, alternateFormat);

		public string DescriptionWithColor(bool alternateFormat = false) => type.DescriptionWithColor(this, alternateFormat);

		public string DescriptionWithColorAndStyle(bool alternateFormat = false) => type.DescriptionWithColorAndStyle(this, alternateFormat);

		public string DescriptionWithColorLengthAndStyle(bool alternateFormat = false) => type.DescriptionWithColorLengthAndStyle(this, alternateFormat);

		public string FullDescription(bool alternateFormat = false) => type.FullDescription(this, alternateFormat);

		public override HairData AsCurrentData()
		{
			return this;
		}

		internal HairData(Guid id, HairType type, HairFurColors color, HairFurColors highlight, HairStyle style, float hairLen, bool semiTransparent, bool growing) : base(id, type)
		{
			hairColor = color;
			highlightColor = highlight;
			this.style = style;
			length = hairLen;
			isSemiTransparent = semiTransparent;
			isGrowing = growing;
		}

		internal HairData(Guid id) : base(id, HairType.defaultValue)
		{
			hairColor = HairFurColors.NO_HAIR_FUR;
			highlightColor = HairFurColors.NO_HAIR_FUR;
			style = HairStyle.NO_STYLE;
			length = 0;
			isSemiTransparent = false;
			isGrowing = true;
		}
	}
}
