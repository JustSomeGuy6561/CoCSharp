using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Tools;
using System;

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
		internal const float MIN_NIPPLE_LENGTH = 0.25f;
		internal const float MAX_NIPPLE_LENGTH = 50f;

		internal const float FULLY_INVERTED_THRESHOLD = 1f; //above this, not fully inverted
		internal const float FUCKABLE_NIPPLE_THRESHOLD = 3f; //above this, fuckable is possible.
		internal const float DICK_NIPPLE_THRESHOLD = 5f; //above this, dick nipples possible.

		internal const float MALE_DEFAULT_NIPPLE = MIN_NIPPLE_LENGTH;
		internal const float FEMALE_DEFAULT_NIPPLE = 0.5f;
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
		internal static NippleStatus ValidateNippleStatus(NippleStatus nippleStatus, float length, bool unlockedDickNipples)
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
		internal bool dickNipplesEnabled { get; private set; }

		internal float length
		{
			get => _length;
			private set
			{
				if (_length != value)
				{
					_length = value;
					nippleStatus = ValidateNippleStatus(nippleStatus, _length, dickNipplesEnabled);
				}
			}
		}
		private float _length;


		internal BodyType bodyType => creature?.body.type ?? BodyType.defaultValue;

		internal float width => length < 1 ? length / 2 : (length < 2 ? 0.5f : length / 4);

		internal float relativeLust => creature?.relativeLust ?? Creature.DEFAULT_LUST;

		internal float lactationRate => source.lactationRate;
		internal LactationStatus lactationStatus => source.lactationStatus;

		private readonly BreastCollection source;

		internal NippleAggregate(Guid parentGuid, BreastCollection parent) : base(parentGuid)
		{

			hasQuadNipples = false;
			hasBlackNipples = false;

			dickNipplesEnabled = false;

			//set the length. let it auto set the nipple status.
			this.length = 0.25f;

			source = parent ?? throw new ArgumentNullException(nameof(parent));
		}

		//the nipple status here will be respected if possible, though length gets priority.
		//note that if you set the desired status to fuckable and they aren't long enough, they become inverted.
		internal void InitializeSettings(float length, bool blackNipples, bool quadNipples, NippleStatus desiredNippleStatus, bool dickNipplesActive)
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
			if (dickNipplesEnabled) return false;
			else
			{
				dickNipplesEnabled = true;
				return true;
			}
		}

		internal bool PreventDickNipples()
		{
			if (!dickNipplesEnabled) return false;
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

		float IGrowable.UseGroPlus()
		{
			if (!((IGrowable)this).CanGroPlus())
			{
				return 0;
			}
			float oldLength = length;
			length += Utils.Rand(6) / 20.0f + 0.25f; //ranges from 1/4- 1/2 inch.
			return length - oldLength; //returns that change in value. limited only if it reaches the max.
		}

		float IShrinkable.UseReducto()
		{
			if (!((IShrinkable)this).CanReducto())
			{
				return 0;
			}
			float oldLength = length;
			if (length > 0.5f)
			{
				length /= 2f;
			}
			else
			{
				length = 0.25f;
			}
			return oldLength - length;
		}
		#endregion

		#region Nipple Text

		float INipple.length => length;

		bool INipple.blackNipples => hasBlackNipples;

		NippleStatus INipple.status => nippleStatus;

		bool INipple.quadNipples => hasQuadNipples;

		LactationStatus INipple.lactationStatus => source.lactationStatus;

		float INipple.lactationRate => source.lactationRate;

		float INipple.relativeLust => relativeLust;

		BodyType INipple.bodyType => bodyType;

		internal  string NippleNoun() => NippleStrings.NounText(this);
		internal  string NippleNoun(bool plural, bool allowQuadNippleIfApplicable = false) => NippleStrings.NounText(this, plural, allowQuadNippleIfApplicable);

		internal  string ShortNippleDescription(IBreast breast) => NippleStrings.ShortDescription(this, breast, true, true);

		internal  string ShortNippleDescription(IBreast breast, bool plural, bool allowQuadNippleTextIfApplicable = true) =>
			NippleStrings.ShortDescription(this, breast, plural, allowQuadNippleTextIfApplicable);

		internal  string SingleNippleDescription(IBreast breast) => NippleStrings.SingleItemDescription(this, breast, true);
		internal  string SingleNipplpeDescription(IBreast breast, bool allowQuadNippleIfApplicable) => NippleStrings.SingleItemDescription(this, breast, allowQuadNippleIfApplicable);

		internal  string LongNippleDescription(IBreast breast, bool alternateFormat = false, bool plural = true, bool usePreciseMeasurements = false)
		{
			return NippleStrings.LongDescription(this, breast, alternateFormat, plural, usePreciseMeasurements);
		}
		internal string FullNippleDescription(IBreast breast, bool alternateFormat = false, bool plural = true, bool usePreciseMeasurements = false)
		{
			return NippleStrings.FullDescription(this, breast, alternateFormat, plural, usePreciseMeasurements);
		}

		internal string OneNippleOrOneOfQuadNipplesShort(IBreast breast, string pronoun = "your")
		{
			return CommonBodyPartStrings.OneOfDescription(hasQuadNipples, pronoun, ShortNippleDescription(breast));
		}

		internal string OneNippleOrEachOfQuadNipplesShort(IBreast breast, string pronoun = "your")
		{
			return OneNippleOrEachOfQuadNipplesShort(breast, pronoun, out bool _);
		}

		internal string OneNippleOrEachOfQuadNipplesShort(IBreast breast, string pronoun, out bool isPlural)
		{
			isPlural = hasQuadNipples;

			return CommonBodyPartStrings.EachOfDescription(hasQuadNipples, pronoun, ShortNippleDescription(breast));
		}

		#endregion
	}

	internal class NippleAggregateData : SimpleData, INipple
	{
		internal readonly NippleStatus nippleStatus;

		internal readonly bool hasQuadNipples;
		internal readonly bool hasBlackNipples;

		internal readonly bool unlockedDickNipples;

		internal readonly float length;

		internal readonly BodyType bodyType;

		internal float width => length < 1 ? length / 2 : (length < 2 ? 0.5f : length / 4);

		internal readonly float relativeLust;
		internal readonly float lactationRate;
		internal readonly LactationStatus lactationStatus;

		internal NippleAggregateData(NippleAggregate source) : base(source?.creatureID ?? throw new ArgumentNullException(nameof(source)))
		{
			this.nippleStatus = source.nippleStatus;
			this.hasBlackNipples = source.hasBlackNipples;
			this.hasQuadNipples = source.hasQuadNipples;

			this.unlockedDickNipples = source.dickNipplesEnabled;

			this.length = source.length;
			this.bodyType = source.bodyType;

			this.relativeLust = source.relativeLust;

			this.lactationRate = source.lactationRate;
			this.lactationStatus = source.lactationStatus;
	}

		internal NippleAggregateData(Guid creatureID, NippleStatus nippleStatus, bool hasQuadNipples, bool hasBlackNipples, bool unlockedDickNipples, float length,
			BodyType bodyType, float relativeLust, float lactationRate, LactationStatus lactationStatus) : base(creatureID)
		{
			this.nippleStatus = nippleStatus;
			this.hasQuadNipples = hasQuadNipples;
			this.hasBlackNipples = hasBlackNipples;
			this.unlockedDickNipples = unlockedDickNipples;
			this.length = length;
			this.bodyType = bodyType ?? throw new ArgumentNullException(nameof(bodyType));
			this.relativeLust = relativeLust;
			this.lactationRate = lactationRate;
			this.lactationStatus = lactationStatus;
		}

		#region Nipple Text

		float INipple.length => length;

		bool INipple.blackNipples => hasBlackNipples;

		NippleStatus INipple.status => nippleStatus;

		bool INipple.quadNipples => hasQuadNipples;

		LactationStatus INipple.lactationStatus => lactationStatus;

		float INipple.lactationRate => lactationRate;

		float INipple.relativeLust => relativeLust;

		BodyType INipple.bodyType => bodyType;

		internal string NippleNoun() => NippleStrings.NounText(this);
		internal string NippleNoun(bool plural, bool allowQuadNippleIfApplicable = false) => NippleStrings.NounText(this, plural, allowQuadNippleIfApplicable);

		internal string ShortNippleDescription(IBreast breast) => NippleStrings.ShortDescription(this, breast, true, true);

		internal string ShortNippleDescription(IBreast breast, bool plural, bool allowQuadNippleTextIfApplicable = true) =>
			NippleStrings.ShortDescription(this, breast, plural, allowQuadNippleTextIfApplicable);

		internal string SingleNippleDescription(IBreast breast) => NippleStrings.SingleItemDescription(this, breast, true);
		internal string SingleNipplpeDescription(IBreast breast, bool allowQuadNippleIfApplicable) => NippleStrings.SingleItemDescription(this, breast, allowQuadNippleIfApplicable);

		internal string LongNippleDescription(IBreast breast, bool alternateFormat = false, bool plural = true, bool usePreciseMeasurements = false)
		{
			return NippleStrings.LongDescription(this, breast, alternateFormat, plural, usePreciseMeasurements);
		}
		internal string FullNippleDescription(IBreast breast, bool alternateFormat = false, bool plural = true, bool usePreciseMeasurements = false)
		{
			return NippleStrings.FullDescription(this, breast, alternateFormat, plural, usePreciseMeasurements);
		}

		internal string OneNippleOrOneOfQuadNipplesShort(IBreast breast, string pronoun = "your")
		{
			return CommonBodyPartStrings.OneOfDescription(hasQuadNipples, pronoun, ShortNippleDescription(breast));
		}

		internal string OneNippleOrEachOfQuadNipplesShort(IBreast breast, string pronoun = "your")
		{
			return OneNippleOrEachOfQuadNipplesShort(breast, pronoun, out bool _);
		}

		internal string OneNippleOrEachOfQuadNipplesShort(IBreast breast, string pronoun, out bool isPlural)
		{
			isPlural = hasQuadNipples;

			return CommonBodyPartStrings.EachOfDescription(hasQuadNipples, pronoun, ShortNippleDescription(breast));
		}

		#endregion
	}
}
