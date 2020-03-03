using System;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Strings;
using CoC.Backend.Tools;

namespace CoC.Backend.BodyParts
{

	public enum NippleStatus { NORMAL, FULLY_INVERTED, SLIGHTLY_INVERTED, FUCKABLE, DICK_NIPPLE }

	//this stores the various nipple data that is common throughout all nipples. it also handles the growable/shrinkable for it.
	//All of this data could be handled by the breast collection class, but it's simply easier to break all the nipple data into one nice class.
	//I suppose this is good practice, too. Regardless, due to its status as a helper class, it's declared internal. All of the data from this class will
	//be exposed in whatever classes utilize it.
	internal sealed partial class NippleAggregate : SimpleSaveablePart<NippleAggregate, NippleAggregateData>, IGrowable, IShrinkable, INipple
	{
		#region Nipple Constants
		internal const double MIN_NIPPLE_LENGTH = 0.25f;
		internal const double MAX_NIPPLE_LENGTH = 50f;

		internal const double FULLY_INVERTED_THRESHOLD = 1f; //above this, not fully inverted
		internal const double FUCKABLE_NIPPLE_THRESHOLD = 3f; //above this, fuckable is possible.
		internal const double DICK_NIPPLE_THRESHOLD = 5f; //above this, dick nipples possible.

		internal const double MALE_DEFAULT_NIPPLE = MIN_NIPPLE_LENGTH;
		internal const double FEMALE_DEFAULT_NIPPLE = 0.5f;
		#endregion

		private Creature creature => CreatureStore.GetCreatureClean(creatureID);

		//Nipples now use a common nipple status, as opposed to the original where it was up to the implementer to loop through and set the flag accordingly.
		//this status is partially determined by length, and will be updated whenever length is set. The rules are as follows:

		//Dick nipples and Fuckable Nipples are mutually exclusive: if dick nipples are activated, the creature cannot get fuckable nipples, and if disabled, the creature can obtain
		//fuckable nipples, but cannot obtain dick nipples. Beyond this, the value is determined by length:
		//Dick Nipples (new-ish): only active while the unlocked dick nipples flag is true. when the nipple length exceeds the dick nipple threshold, the creature obtains dick nipples.
		//	when the value drops below dick nipple thresholds, it reverts to a normal nipple.
		//Fuckable Nipples: available whenever the dick nipples are not unlocked. when the nipple length exceeds the fuckable nipple threshold, the creature has fuckable nipples.
		//	when the value drops below the fuckable nipple threshold, it changes to inverted nipples, though the exact state is determined by length.
		//Inverted nipple (new): Alway active. There are two stages: pertially inverted, and fully inverted. these occur when the creature has fuckable nipples,
		//	but they become too short to remain fuckable. They also can be chosen manually, if that's the aesthetic choice of the player or part of an NPC's design.
		//Normal: like it sounds. this is the default state. Note that these become fuckable or dick-nipple when they
		internal NippleStatus nippleStatus { get; private set; } = NippleStatus.NORMAL;

		//fix the nipple status to reflect the new length.
		internal static NippleStatus ValidateNippleStatus(NippleStatus nippleStatus, double length, bool unlockedDickNipples)
		{
			//if dick nipples are available and it's long enough to be dick nipples, become dick nipples.
			if (unlockedDickNipples && length > DICK_NIPPLE_THRESHOLD)
			{
				return NippleStatus.DICK_NIPPLE;
			}
			//normal nipples if length would otherwise be fuckable, but dick nipples enabled - it seems weird to switch from fuckable to dick nipples and vice-versa.
			else if (unlockedDickNipples && length > FUCKABLE_NIPPLE_THRESHOLD)
			{
				return NippleStatus.NORMAL;
			}
			//if long enough to be fuckable, and not dicknipples above, become fuckable nipples.
			else if (length > FUCKABLE_NIPPLE_THRESHOLD)
			{
				return NippleStatus.FUCKABLE;
			}
			//at this point, length is guarenteed to be less than or equal to the fuckable nipple threshold.
			//thus, if we're currently normal, we're going to remain that way.
			else if (nippleStatus == NippleStatus.NORMAL)
			{
				return nippleStatus;
			}
			//fully inverted check - if not fully inverted and below the fully inverted threshold (and not normal, which would have procced earlier)
			else if (length < FULLY_INVERTED_THRESHOLD)
			{
				return NippleStatus.FULLY_INVERTED;
			}
			//partially inverted check. if fuckable, but no longer long enough (and not fully inverted from above) become partially inverted.
			else if (length < FUCKABLE_NIPPLE_THRESHOLD)
			{
				return NippleStatus.SLIGHTLY_INVERTED;
			}
			//we'd only fall to here if we're currently at a threshold. if so, we stay the same, but are very close to switching states.
			return nippleStatus;
		}

		internal bool hasQuadNipples { get; private set; }
		internal bool hasBlackNipples { get; private set; }

		//this toggles us between dick nipples and fuckable nipples as our over-large state. when true, overly large nipples become dick nipples. when false, they become fuckable
		internal bool dickNipplesEnabled
		{
			get => _dickNipplesEnabled;
			private set
			{
				if (_dickNipplesEnabled != value)
				{
					_dickNipplesEnabled = value;
					nippleStatus = ValidateNippleStatus(nippleStatus, _length, _dickNipplesEnabled);
				}
			}
		}
		private bool _dickNipplesEnabled;


		internal double length
		{
			get => _length;
			private set
			{
				if (_length != value)
				{
					_length = value;
					nippleStatus = ValidateNippleStatus(nippleStatus, _length, _dickNipplesEnabled);
				}
			}
		}
		private double _length;

		private double growthMultiplier => creature?.genitals.perkData.NippleGrowthMultiplier ?? 1;
		private double shrinkMultiplier => creature?.genitals.perkData.NippleShrinkMultiplier ?? 1;

		internal BodyType bodyType => creature?.body.type ?? BodyType.defaultValue;

		internal double width => length < 1 ? length / 2 : (length < 2 ? 0.5f : length / 4);

		internal double relativeLust => creature?.relativeLust ?? Creature.DEFAULT_LUST;

		internal double lactationRate => source.lactationRate;
		internal LactationStatus lactationStatus => source.lactationStatus;

		private readonly BreastCollection source;

		internal NippleAggregate(Guid parentGuid, BreastCollection parent) : base(parentGuid)
		{

			hasQuadNipples = false;
			hasBlackNipples = false;

			dickNipplesEnabled = false;

			//set the length. let it auto set the nipple status.
			length = 0.25f;

			source = parent ?? throw new ArgumentNullException(nameof(parent));
		}

		//the nipple status here will be respected if possible, though length gets priority.
		//note that if you set the desired status to fuckable and they aren't long enough, they become inverted.
		internal void InitializeSettings(double length, bool blackNipples, bool quadNipples, NippleStatus desiredNippleStatus, bool dickNipplesActive)
		{
			nippleStatus = desiredNippleStatus;

			//set the unlocked dick nipples flag so it can be used when the length updates the nipple status.
			dickNipplesEnabled = dickNipplesActive;
			//set the length, and in combination with the above, correctly sets the nipple status.
			this.length = length;

			//set these flags based on initial values.
			hasBlackNipples = blackNipples;
			hasQuadNipples = quadNipples;
		}

		public override string BodyPartName()
		{
			return Name();
		}

		public override NippleAggregateData AsReadOnlyData()
		{
			return new NippleAggregateData(this);
		}

		public override bool IsIdenticalTo(NippleAggregateData original, bool ignoreSexualMetaData)
		{
			return !(original is null) && hasBlackNipples == original.hasBlackNipples && hasQuadNipples == original.hasQuadNipples
				&& lactationStatus == original.lactationStatus && dickNipplesEnabled == original.dickNipplesEnabled && length == original.length;
		}

		internal override bool Validate(bool correctInvalidData)
		{
			length = length;
			return true;
		}

		#region Change Settings

		internal bool SetQuadNipples(bool active)
		{
			if (hasQuadNipples == active)
			{
				return false;
			}
			else
			{
				hasQuadNipples = active;
				return true;
			}
		}

		internal bool SetBlackNipples(bool active)
		{
			if (hasBlackNipples == active)
			{
				return false;
			}
			else
			{
				hasBlackNipples = active;
				return true;
			}
		}

		//attempts to set the nipple status to the given value. if limit to current length is set, it will prevent this from setting the nipple status to something the current
		//nipple length does not support (i.e. normal with nipples in the fuckable range, or fuckable while in the inverted range). similarly, if the desired value is dick nipples,
		//but dick nipples are not unlocked, this will fail unless the unlock dick nipples if necessary flag is set to true.
		internal bool SetNippleStatus(NippleStatus desiredStatus, bool limitToCurrentLength = false, bool toggleDickNippleFlagIfNeccesary = false)
		{
			//if we're already the desired state, return false.
			if (nippleStatus == desiredStatus)
			{
				return false;
			}
			//otherwise, if this would require toggling the dick nipple flag, but the toggle flag is false, return false.
			else if (!toggleDickNippleFlagIfNeccesary && ((!dickNipplesEnabled && desiredStatus == NippleStatus.DICK_NIPPLE) ||
				(dickNipplesEnabled && desiredStatus == NippleStatus.FUCKABLE)))
			{
				return false;
			}
			//at this point, we can assume we can toggle the flag if needed.
			//if we allow the nipple length to change to match desired status
			else if (!limitToCurrentLength)
			{
				nippleStatus = desiredStatus;
				if (desiredStatus == NippleStatus.DICK_NIPPLE)
				{
					UnlockDickNipples();
					if (length < DICK_NIPPLE_THRESHOLD)
					{
						length = DICK_NIPPLE_THRESHOLD;
					}
				}
				else if (desiredStatus == NippleStatus.FUCKABLE)
				{
					PreventDickNipples();
					if (length < FUCKABLE_NIPPLE_THRESHOLD)
					{
						length = FUCKABLE_NIPPLE_THRESHOLD;
					}
				}
				else if (desiredStatus == NippleStatus.FULLY_INVERTED)
				{
					if (length > FULLY_INVERTED_THRESHOLD)
					{
						length = FULLY_INVERTED_THRESHOLD;
					}
				}
				else if (desiredStatus == NippleStatus.SLIGHTLY_INVERTED)
				{
					if (length > FUCKABLE_NIPPLE_THRESHOLD)
					{
						length = FUCKABLE_NIPPLE_THRESHOLD;
					}
					else if (length < FULLY_INVERTED_THRESHOLD)
					{
						length = FULLY_INVERTED_THRESHOLD;
					}
				}
				else
				{
					if (length > FUCKABLE_NIPPLE_THRESHOLD)
					{
						length = FUCKABLE_NIPPLE_THRESHOLD;
					}
				}
				return true;
			}
			//otherwise:
			else
			{
				if (desiredStatus == NippleStatus.DICK_NIPPLE)
				{
					if (length >= DICK_NIPPLE_THRESHOLD)
					{
						UnlockDickNipples();
						desiredStatus = NippleStatus.DICK_NIPPLE;
						return true;
					}
				}
				else if (desiredStatus == NippleStatus.FUCKABLE)
				{
					if (length >= FUCKABLE_NIPPLE_THRESHOLD)
					{
						PreventDickNipples();
						nippleStatus = NippleStatus.FUCKABLE;
						return true;
					}
				}
				else if (desiredStatus == NippleStatus.SLIGHTLY_INVERTED)
				{
					if (length <= FUCKABLE_NIPPLE_THRESHOLD && length >= FULLY_INVERTED_THRESHOLD)
					{
						nippleStatus = NippleStatus.SLIGHTLY_INVERTED;
						return true;
					}
				}
				else if (desiredStatus == NippleStatus.FULLY_INVERTED)
				{
					if (length <= FULLY_INVERTED_THRESHOLD)
					{
						nippleStatus = NippleStatus.FULLY_INVERTED;
						return true;
					}
				}
				else
				{
					if (length <= FUCKABLE_NIPPLE_THRESHOLD || (dickNipplesEnabled && length < DICK_NIPPLE_THRESHOLD))
					{
						nippleStatus = NippleStatus.NORMAL;
						return true;
					}
				}
			}

			return false;
		}

		internal bool UnlockDickNipples()
		{
			if (dickNipplesEnabled)
			{
				return false;
			}
			else
			{
				dickNipplesEnabled = true;
				return true;
			}
		}

		internal bool PreventDickNipples()
		{
			if (!dickNipplesEnabled)
			{
				return false;
			}
			else
			{
				dickNipplesEnabled = false;
				return true;
			}
		}

		internal bool SetDickNippleFlag(bool enabled)
		{
			if (enabled == dickNipplesEnabled)
			{
				return false;
			}
			else
			{
				dickNipplesEnabled = enabled;
				return true;
			}
		}

		internal double GrowNipples(double amount, bool ignorePerks = false)
		{
			if (amount <= 0)
			{
				return 0;
			}
			if (!ignorePerks)
			{
				amount *= growthMultiplier;
			}
			double oldSize = length;

			length += amount;

			return length - oldSize;
		}

		internal double ShrinkNipples(double amount, bool ignorePerks = false)
		{
			if (amount <= 0)
			{
				return 0;
			}
			if (!ignorePerks)
			{
				amount *= shrinkMultiplier;
			}
			double oldSize = length;

			length -= amount;

			return oldSize - length;
		}

		internal double ChangeNippleLength(double delta, bool ignorePerks = false)
		{
			if (delta > 0)
			{
				return GrowNipples(delta, ignorePerks);
			}
			else if (delta == 0)
			{
				return 0;
			}
			else
			{
				return -1 * ShrinkNipples(-1 * delta, ignorePerks);
			}
		}

		internal bool SetNippleLength(double size)
		{
			length = size;
			return length == size;
		}

		#endregion

		#region GrowShrink
		bool IGrowable.CanGroPlus()
		{
			return length < MAX_NIPPLE_LENGTH;
		}

		bool IShrinkable.CanReducto()
		{
			return length > MIN_NIPPLE_LENGTH;
		}

		string IGrowable.UseGroPlus()
		{
			if (!((IGrowable)this).CanGroPlus())
			{
				return null;
			}
			var oldStatus = nippleStatus;

			length += Utils.Rand(6) / 20.0 + 0.25; //ranges from 1/4- 1/2 inch.
			return GroPlusNipples(oldStatus);
		}

		string IShrinkable.UseReducto()
		{
			if (!((IShrinkable)this).CanReducto())
			{
				return null;
			}
			var oldStatus = nippleStatus;

			if (length > 0.5f)
			{
				length /= 2;
			}
			else
			{
				length = 0.25;
			}
			return ReductoNipples(oldStatus);
		}
		#endregion

		#region Nipple Text

		double INipple.length => length;

		bool INipple.blackNipples => hasBlackNipples;

		NippleStatus INipple.status => nippleStatus;

		bool INipple.quadNipples => hasQuadNipples;

		LactationStatus INipple.lactationStatus => source.lactationStatus;

		double INipple.lactationRate => source.lactationRate;

		double INipple.relativeLust => relativeLust;

		BodyType INipple.bodyType => bodyType;

		internal string NippleNoun() => NippleStrings.NounText(this);
		internal string NippleNoun(bool plural, bool allowQuadNippleIfApplicable = false) => NippleStrings.NounText(this, plural, allowQuadNippleIfApplicable);

		internal string RowShortDescription(IBreast breast) => NippleStrings.RowShortDescription(this, breast, true, true);

		internal string RowShortDescription(IBreast breast, bool plural, bool allowQuadNippleTextIfApplicable = true) =>
			NippleStrings.RowShortDescription(this, breast, plural, allowQuadNippleTextIfApplicable);

		internal string GenericShortDescription() => NippleStrings.GenericShortDescription(this, true, true);
		internal string GenericShortDescription(bool plural, bool allowQuadNippleTextIfApplicable = true)
			=> NippleStrings.GenericShortDescription(this, plural, allowQuadNippleTextIfApplicable);

		internal string RowSingleDescription(IBreast breast) => NippleStrings.RowSingleItemDescription(this, breast, true);
		internal string RowSingleDescription(IBreast breast, bool allowQuadNippleIfApplicable) => NippleStrings.RowSingleItemDescription(this, breast, allowQuadNippleIfApplicable);


		internal string GenericSingleDescription() => NippleStrings.GenericSingleItemDescription(this, true);
		internal string GenericSingleDescription(bool allowQuadNippleIfApplicable) => NippleStrings.GenericSingleItemDescription(this, allowQuadNippleIfApplicable);

		internal string RowLongDescription(IBreast breast, bool alternateFormat = false, bool plural = true, bool usePreciseMeasurements = false)
			=>NippleStrings.RowLongDescription(this, breast, alternateFormat, plural, usePreciseMeasurements);

		internal string GenericLongDescription(bool alternateFormat = false, bool plural = true, bool usePreciseMeasurements = false)
			=> NippleStrings.GenericLongDescription(this, alternateFormat, plural, usePreciseMeasurements);

		internal string RowFullDescription(IBreast breast, bool alternateFormat = false, bool plural = true, bool usePreciseMeasurements = false)
			=> NippleStrings.RowFullDescription(this, breast, alternateFormat, plural, usePreciseMeasurements);

		internal string GenericFullDescription(bool alternateFormat = false, bool plural = true, bool usePreciseMeasurements = false)
			=> NippleStrings.GenericFullDescription(this, alternateFormat, plural, usePreciseMeasurements);

		internal string RowOneNippleDescription(IBreast breast) => RowOneNippleDescription(breast, Conjugate.YOU);
		internal string RowOneNippleDescription(IBreast breast, Conjugate conjugate)
		{
			return CommonBodyPartStrings.OneOfDescription(hasQuadNipples, conjugate, RowShortDescription(breast));
		}

		internal string GenericOneNippleDescription() => GenericOneNippleDescription(Conjugate.YOU);
		internal string GenericOneNippleDescription(Conjugate conjugate)
		{

			return CommonBodyPartStrings.OneOfDescription(hasQuadNipples, conjugate, GenericShortDescription());
		}

		internal string RowEachNippleDescription(IBreast breast) => RowEachNippleDescription(breast, Conjugate.YOU);
		internal string RowEachNippleDescription(IBreast breast, Conjugate conjugate)
		{
			return RowEachNippleDescription(breast, conjugate, out bool _);
		}

		internal string RowEachNippleDescription(IBreast breast, Conjugate conjugate, out bool isPlural)
		{
			isPlural = hasQuadNipples;

			return CommonBodyPartStrings.EachOfDescription(hasQuadNipples, conjugate, RowShortDescription(breast));
		}

		internal string GenericEachNippleDescription() => GenericEachNippleDescription(Conjugate.YOU);
		internal string GenericEachNippleDescription(Conjugate conjugate)
		{
			return GenericEachNippleDescription(conjugate, out bool _);
		}

		internal string GenericEachNippleDescription(Conjugate conjugate, out bool isPlural)
		{
			isPlural = hasQuadNipples;

			return CommonBodyPartStrings.EachOfDescription(hasQuadNipples, conjugate, GenericShortDescription());
		}
		#endregion

	}

	internal class NippleAggregateData : SimpleData, INipple
	{
		internal readonly NippleStatus nippleStatus;

		internal readonly bool hasQuadNipples;
		internal readonly bool hasBlackNipples;

		internal readonly bool dickNipplesEnabled;

		internal readonly double length;

		internal readonly BodyType bodyType;

		internal double width => length < 1 ? length / 2 : (length < 2 ? 0.5f : length / 4);

		internal readonly double relativeLust;
		internal readonly double lactationRate;
		internal readonly LactationStatus lactationStatus;

		internal NippleAggregateData(NippleAggregate source) : base(source?.creatureID ?? throw new ArgumentNullException(nameof(source)))
		{
			nippleStatus = source.nippleStatus;
			hasBlackNipples = source.hasBlackNipples;
			hasQuadNipples = source.hasQuadNipples;

			dickNipplesEnabled = source.dickNipplesEnabled;

			length = source.length;
			bodyType = source.bodyType;

			relativeLust = source.relativeLust;

			lactationRate = source.lactationRate;
			lactationStatus = source.lactationStatus;
		}

		internal NippleAggregateData(Guid creatureID, NippleStatus nippleStatus, bool hasQuadNipples, bool hasBlackNipples, bool unlockedDickNipples, double length,
			BodyType bodyType, double relativeLust, double lactationRate, LactationStatus lactationStatus) : base(creatureID)
		{
			this.nippleStatus = nippleStatus;
			this.hasQuadNipples = hasQuadNipples;
			this.hasBlackNipples = hasBlackNipples;
			dickNipplesEnabled = unlockedDickNipples;
			this.length = length;
			this.bodyType = bodyType ?? throw new ArgumentNullException(nameof(bodyType));
			this.relativeLust = relativeLust;
			this.lactationRate = lactationRate;
			this.lactationStatus = lactationStatus;
		}

		#region Nipple Text

		double INipple.length => length;

		bool INipple.blackNipples => hasBlackNipples;

		NippleStatus INipple.status => nippleStatus;

		bool INipple.quadNipples => hasQuadNipples;

		LactationStatus INipple.lactationStatus => lactationStatus;

		double INipple.lactationRate => lactationRate;

		double INipple.relativeLust => relativeLust;

		BodyType INipple.bodyType => bodyType;

		internal string NippleNoun() => NippleStrings.NounText(this);
		internal string NippleNoun(bool plural, bool allowQuadNippleIfApplicable = false) => NippleStrings.NounText(this, plural, allowQuadNippleIfApplicable);

		internal string RowShortDescription(IBreast breast) => NippleStrings.RowShortDescription(this, breast, true, true);

		internal string RowShortDescription(IBreast breast, bool plural, bool allowQuadNippleTextIfApplicable = true) =>
			NippleStrings.RowShortDescription(this, breast, plural, allowQuadNippleTextIfApplicable);

		internal string GenericShortDescription() => NippleStrings.GenericShortDescription(this, true, true);
		internal string GenericShortDescription(bool plural, bool allowQuadNippleTextIfApplicable = true)
			=> NippleStrings.GenericShortDescription(this, plural, allowQuadNippleTextIfApplicable);

		internal string RowSingleDescription(IBreast breast) => NippleStrings.RowSingleItemDescription(this, breast, true);
		internal string RowSingleDescription(IBreast breast, bool allowQuadNippleIfApplicable) => NippleStrings.RowSingleItemDescription(this, breast, allowQuadNippleIfApplicable);


		internal string GenericSingleDescription() => NippleStrings.GenericSingleItemDescription(this, true);
		internal string GenericSingleDescription(bool allowQuadNippleIfApplicable) => NippleStrings.GenericSingleItemDescription(this, allowQuadNippleIfApplicable);

		internal string RowLongDescription(IBreast breast, bool alternateFormat = false, bool plural = true, bool usePreciseMeasurements = false)
			=> NippleStrings.RowLongDescription(this, breast, alternateFormat, plural, usePreciseMeasurements);

		internal string GenericLongDescription(bool alternateFormat = false, bool plural = true, bool usePreciseMeasurements = false)
			=> NippleStrings.GenericLongDescription(this, alternateFormat, plural, usePreciseMeasurements);

		internal string RowFullDescription(IBreast breast, bool alternateFormat = false, bool plural = true, bool usePreciseMeasurements = false)
			=> NippleStrings.RowFullDescription(this, breast, alternateFormat, plural, usePreciseMeasurements);

		internal string GenericFullDescription(bool alternateFormat = false, bool plural = true, bool usePreciseMeasurements = false)
			=> NippleStrings.GenericFullDescription(this, alternateFormat, plural, usePreciseMeasurements);

		internal string RowOneNippleDescription(IBreast breast) => RowOneNippleDescription(breast, Conjugate.YOU);
		internal string RowOneNippleDescription(IBreast breast, Conjugate conjugate)
		{
			return CommonBodyPartStrings.OneOfDescription(hasQuadNipples, conjugate, RowShortDescription(breast));
		}

		internal string GenericOneNippleDescription() => GenericOneNippleDescription(Conjugate.YOU);
		internal string GenericOneNippleDescription(Conjugate conjugate)
		{

			return CommonBodyPartStrings.OneOfDescription(hasQuadNipples, conjugate, GenericShortDescription());
		}

		internal string RowEachNippleDescription(IBreast breast) => RowEachNippleDescription(breast, Conjugate.YOU);
		internal string RowEachNippleDescription(IBreast breast, Conjugate conjugate)
		{
			return RowEachNippleDescription(breast, conjugate, out bool _);
		}

		internal string RowEachNippleDescription(IBreast breast, Conjugate conjugate, out bool isPlural)
		{
			isPlural = hasQuadNipples;

			return CommonBodyPartStrings.EachOfDescription(hasQuadNipples, conjugate, RowShortDescription(breast));
		}

		internal string GenericEachNippleDescription() => GenericEachNippleDescription(Conjugate.YOU);
		internal string GenericEachNippleDescription(Conjugate conjugate)
		{
			return GenericEachNippleDescription(conjugate, out bool _);
		}

		internal string GenericEachNippleDescription(Conjugate conjugate, out bool isPlural)
		{
			isPlural = hasQuadNipples;

			return CommonBodyPartStrings.EachOfDescription(hasQuadNipples, conjugate, GenericShortDescription());
		}

		#endregion
	}
}
