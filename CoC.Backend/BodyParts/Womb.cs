using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Engine.Time;
using CoC.Backend.Pregnancies;
using System;
using System.Collections.Generic;
using System.Text;
using WeakEvent;

namespace CoC.Backend.BodyParts
{
	//womb is not sealed; if you need a womb that does custom things, feel free to do so.
	//womb has several constructors, but

	public partial class Womb : SimpleSaveablePart<Womb, WombData>, ITimeDailyListenerSimple
	{
		public override string BodyPartName()
		{
			return Name();
		}

		//Note: we don't attach these to vagina and ass b/c it's possible to lose a vagina (and perhaps an asshole too if it's possible to TF to anemone or something)
		//and we wouldn't want this to cause the pregnancy to be lost.
		public double pregnancyMultiplier
		{
			get
			{
				if (pregnancyMultiplierCounter == 0)
				{
					return 1;
				}
				else if (pregnancyMultiplierCounter > 0)
				{
					return 1 + pregnancyMultiplierCounter / 2.0f;
				}
				else
				{
					return 1 / (1 - pregnancyMultiplierCounter / 2.0f);
				}
			}
		}
		protected int pregnancyMultiplierCounter => CreatureStore.GetCreatureClean(creatureID)?.perks.baseModifiers.pregnancySpeedModifier.GetValue() ?? 0;

		public bool isPregnant => normalPregnancy?.isPregnant == true || analPregnancy?.isPregnant == true || secondaryNormalPregnancy?.isPregnant == true;


		//if null, cannot get pregnant via normal vagina.
		public readonly VaginalPregnancyStore normalPregnancy;
		public bool canGetPregnant(bool hasVagina) => normalPregnancy != null && hasVagina && AllowsVaginalPregnancies();

		protected virtual bool AllowsVaginalPregnancies() => true;

		//if null, cannot get anally pregnant.
		public readonly AnalPregnancyStore analPregnancy;

		//default case: has anus, and source forces us to allow it.
		public bool canGetAnallyPregnant(bool hasAnus, bool sourceOverridesNoAnalPregnancies) => analPregnancy != null && hasAnus && sourceOverridesNoAnalPregnancies;

		protected virtual bool AllowsAnalPregnancies(bool sourceIgnoresPregnancyPreferences) => sourceIgnoresPregnancyPreferences;

		//allows a third pregnancy store for creatures with two vaginas. defaults to null, so we can't get pregnant through a second vagina.
		public readonly VaginalPregnancyStore secondaryNormalPregnancy;

		//same as normal pregnancy, though this one uses second vagina. since secondaryNormalPregnancy defaults to null, this defaults to false.
		public bool canGetSecondaryNormalPregnant(bool hasSecondVagina) => secondaryNormalPregnancy != null && AllowsAdditionalVaginalPregnancies();
		protected virtual bool AllowsAdditionalVaginalPregnancies() => true;

		public uint totalBirthCount => normalPregnancy?.birthCount + analPregnancy?.birthCount + secondaryNormalPregnancy?.birthCount ?? 0;

		#region Oviposition

		public virtual bool allowsOviposition => false;
		public virtual bool allowsOvipositionRemoval => false;

		public virtual byte eggsEveryXDays => 15;

		public bool canObtainOviposition => allowsOviposition && !hasOviposition;
		public bool canRemoveOviposition => allowsOvipositionRemoval && hasOviposition;

		public bool hasOviposition { get; protected set; } = false;

		public bool GrantOviposition()
		{
			if (hasOviposition || !canObtainOviposition)
			{
				return false;
			}
			hasOviposition = true;
			return true;
		}

		public bool ClearOviposition()
		{
			if (!hasOviposition || !canRemoveOviposition)
			{
				return false;
			}
			hasOviposition = false;
			return true;
		}

		#endregion

		#region Diapause

		public virtual bool allowsDiapause => false;
		public virtual bool allowsDiapauseRemoval => false;

		public bool canObtainDiapause => allowsDiapause && !hasDiapause;
		public bool canRemoveDiapause => allowsDiapauseRemoval && hasDiapause;

		public bool hasDiapause { get; protected set; } = false;

		//does not throw a data changed, could. idk.
		internal void onConsumeLiquid()
		{
			if (hasDiapause)
			{
				normalPregnancy?.onConsumeLiquid();
				secondaryNormalPregnancy?.onConsumeLiquid();
				analPregnancy?.onConsumeLiquid();
			}
		}

		public bool EnableDiapause()
		{
			if (hasDiapause || !canObtainDiapause)
			{
				return false;
			}
			hasDiapause = true;
			return true;
		}

		public bool DisableDiapause()
		{
			if (!hasDiapause || !canRemoveDiapause)
			{
				return false;
			}
			hasDiapause = false;
			return true;
		}

		#endregion


		//allows full customization.
		protected Womb(Guid creatureID, VaginalPregnancyStore primaryVagina, AnalPregnancyStore anus, VaginalPregnancyStore secondaryVagina) : base(creatureID)
		{
			normalPregnancy = primaryVagina;
			analPregnancy = anus;
			secondaryNormalPregnancy = secondaryVagina;
		}

		//default constructor.
		protected internal Womb(Guid creatureID, bool allowsVaginalPregnancies, bool allowsAnalPregnancies) : base(creatureID)
		{
			normalPregnancy = allowsVaginalPregnancies ? new VaginalPregnancyStore(creatureID, 0) : null;
			secondaryNormalPregnancy = allowsVaginalPregnancies ? new VaginalPregnancyStore(creatureID, 1) : null;

			analPregnancy = allowsAnalPregnancies ? new AnalPregnancyStore(creatureID) : null;
		}

		public override WombData AsReadOnlyData()
		{
			return new WombData(this);
		}

		protected internal override void PostPerkInit()
		{
			normalPregnancy?.PostPerkInit();
			analPregnancy?.PostPerkInit();
			secondaryNormalPregnancy?.PostPerkInit();
		}

		protected internal override void LateInit()
		{
			normalPregnancy?.LateInit();
			analPregnancy?.LateInit();
			secondaryNormalPregnancy?.LateInit();

			if (normalPregnancy != null)
			{
				normalPregnancy.dataChange += Normal_dataChange;
			}
			if (analPregnancy != null)
			{
				analPregnancy.dataChange += Anal_dataChange;
			}
			if (secondaryNormalPregnancy != null)
			{
				secondaryNormalPregnancy.dataChange += Secondary_dataChange;
			}
		}

		private void Normal_dataChange(object sender, EventHelpers.SimpleDataChangeEvent<PregnancyStore, ReadOnlyPregnancyStore> e)
		{
			NotifyDataChanged(new WombData(creatureID, e.oldValues, canGetPregnant, analPregnancy?.AsReadOnlyData(), canGetAnallyPregnant,
				secondaryNormalPregnancy?.AsReadOnlyData(), canGetSecondaryNormalPregnant, eggsEveryXDays, eggSize, allowsDiapause, hasDiapause, allowsDiapauseRemoval,
				allowsOviposition, hasOviposition, allowsOvipositionRemoval));
		}

		private void Anal_dataChange(object sender, EventHelpers.SimpleDataChangeEvent<PregnancyStore, ReadOnlyPregnancyStore> e)
		{
			NotifyDataChanged(new WombData(creatureID, normalPregnancy?.AsReadOnlyData(), canGetPregnant, e.oldValues, canGetAnallyPregnant,
				secondaryNormalPregnancy?.AsReadOnlyData(), canGetSecondaryNormalPregnant, eggsEveryXDays, eggSize, allowsDiapause, hasDiapause, allowsDiapauseRemoval,
				allowsOviposition, hasOviposition, allowsOvipositionRemoval));
		}

		private void Secondary_dataChange(object sender, EventHelpers.SimpleDataChangeEvent<PregnancyStore, ReadOnlyPregnancyStore> e)
		{
			NotifyDataChanged(new WombData(creatureID, normalPregnancy?.AsReadOnlyData(), canGetPregnant, analPregnancy?.AsReadOnlyData(), canGetAnallyPregnant,
				e.oldValues, canGetSecondaryNormalPregnant,  eggsEveryXDays, eggSize, allowsDiapause, hasDiapause, allowsDiapauseRemoval,
				allowsOviposition, hasOviposition, allowsOvipositionRemoval));
		}

		protected internal bool AttemptNormalKnockUp(double knockupChance, StandardSpawnType type)
		{
			return normalPregnancy.attemptKnockUp(knockupChance, type);
		}

		public virtual bool? eggSize => normalPregnancy?.eggSizeKnown == true ? (bool?)normalPregnancy.eggsLarge : null;

		public void SetEggSize(bool isLarge)
		{
			SetNormalEggSize(isLarge);
			SetSecondaryEggSize(isLarge);
		}

		//clears egg size "perk". now eggs are sized randomly.
		public void ClearEggSize()
		{
			ClearNormalEggSize();
			ClearSecondaryEggSize();
		}

		//sets the egg size for all future egg pregnancies in this womb.
		protected internal void SetNormalEggSize(bool isLarge)
		{
			normalPregnancy.SetEggSize(isLarge);
		}
		//clears any set egg size that would otherwise affect all future egg pregnancies in this womb.
		protected internal void ClearNormalEggSize()
		{
			normalPregnancy.ClearEggSize();
		}


		protected internal void ResetNormal(bool clearEggSize = false)
		{
			normalPregnancy.Reset(clearEggSize);
		}

		protected internal bool AttemptSecondaryKnockUp(double knockupChance, StandardSpawnType type)
		{
			return secondaryNormalPregnancy.attemptKnockUp(knockupChance, type);
		}

		//sets the egg size for all future egg pregnancies in this womb.
		protected internal void SetSecondaryEggSize(bool isLarge)
		{
			secondaryNormalPregnancy.SetEggSize(isLarge);
		}
		//clears any set egg size that would otherwise affect all future egg pregnancies in this womb.
		protected internal void ClearSecondaryEggSize()
		{
			secondaryNormalPregnancy.ClearEggSize();
		}


		protected internal void ResetSecondary(bool clearEggSize = false)
		{
			secondaryNormalPregnancy.Reset(clearEggSize);
		}

		//note: allows standard spawn type for sake of convenience, however, if a spawn type does not derive SpawnTypeIncludeAnal, this will always fail.
		protected internal bool AttemptAnalKnockUp(double knockupChance, StandardSpawnType type)
		{
			return analPregnancy.attemptKnockUp(knockupChance, type);
		}

		protected internal void ResetAnal()
		{
			analPregnancy.Reset(true);
		}

		protected readonly WeakEventSource<KnockupEvent> knockupEventSource =
			new WeakEventSource<KnockupEvent>();

		public event EventHandler<KnockupEvent> onKnockup
		{
			add => knockupEventSource.Subscribe(value);
			remove => knockupEventSource.Unsubscribe(value);
		}

		protected readonly WeakEventSource<BirthEvent> birthEventSource =
			new WeakEventSource<BirthEvent>();

		public event EventHandler<BirthEvent> onBirth
		{
			add => birthEventSource.Subscribe(value);
			remove => birthEventSource.Unsubscribe(value);
		}

		internal void RaiseKnockupEvent(PregnancyStore pregnancyStore)
		{
			knockupEventSource.Raise(pregnancyStore, new KnockupEvent(creatureID, pregnancyStore.AsReadOnlyData()));
		}

		internal void RaiseKnockupEvent(PregnancyStore pregnancyStore, StandardSpawnData oldSpawnData)
		{
			knockupEventSource.Raise(pregnancyStore, new KnockupEvent(creatureID, pregnancyStore.AsReadOnlyData(), oldSpawnData));
		}

		internal void RaiseBirthEvent(StandardSpawnType spawnType, PregnancyStore pregnancyStore)
		{
			birthEventSource.Raise(pregnancyStore, new BirthEvent(creatureID, pregnancyStore.AsReadOnlyData(), spawnType.AsReadOnlyData(), totalBirthCount));
		}

		internal override bool Validate(bool correctInvalidData)
		{
			bool valid = normalPregnancy?.Validate(correctInvalidData) ?? true;
			if (valid || correctInvalidData)
			{
				valid &= analPregnancy?.Validate(correctInvalidData) ?? true;
			}
			if (valid || correctInvalidData)
			{
				valid &= secondaryNormalPregnancy?.Validate(correctInvalidData) ?? true;
			}

			valid &= ExtraValidations(valid, correctInvalidData);
			return valid;
		}

		protected virtual bool ExtraValidations(bool currentlyValid, bool correctInvalidData)
		{
			return currentlyValid;
		}

		internal IEnumerable<ITimeActiveListenerFull> GetActiveListeners()
		{
			List<ITimeActiveListenerFull> activeListeners = new List<ITimeActiveListenerFull>();
			ITimeActiveListenerFull activeListener = null;
			activeListener = normalPregnancy as ITimeActiveListenerFull;
			if (activeListener != null)
			{
				activeListeners.Add(activeListener);
			}
			activeListener = analPregnancy as ITimeActiveListenerFull;
			if (activeListener != null)
			{
				activeListeners.Add(activeListener);
			}
			activeListener = secondaryNormalPregnancy as ITimeActiveListenerFull;
			if (activeListener != null)
			{
				activeListeners.Add(activeListener);
			}
			if (this is ITimeActiveListenerFull active)
			{
				activeListeners.Add(active);
			}
			return activeListeners;
		}

		internal IEnumerable<ITimeLazyListener> GetLazyListeners()
		{
			List<ITimeLazyListener> lazyListeners = new List<ITimeLazyListener>();
			ITimeLazyListener lazyListener = null;
			lazyListener = normalPregnancy as ITimeLazyListener;
			if (lazyListener != null)
			{
				lazyListeners.Add(lazyListener);
			}
			lazyListener = analPregnancy as ITimeLazyListener;
			if (lazyListener != null)
			{
				lazyListeners.Add(lazyListener);
			}
			lazyListener = secondaryNormalPregnancy as ITimeLazyListener;
			if (lazyListener != null)
			{
				lazyListeners.Add(lazyListener);
			}
			if (this is ITimeLazyListener lazy)
			{
				lazyListeners.Add(lazy);
			}
			return lazyListeners;
		}

		internal IEnumerable<ITimeDailyListenerFull> GetDailyListeners()
		{
			List<ITimeDailyListenerFull> dailyListeners = new List<ITimeDailyListenerFull>();
			if (this is ITimeDailyListenerFull daily)
			{
				dailyListeners.Add(daily);
			}
			return dailyListeners;
		}

		internal IEnumerable<ITimeDayMultiListenerFull> GetDayMultiListeners()
		{
			List<ITimeDayMultiListenerFull> dayMultiListeners = new List<ITimeDayMultiListenerFull>();
			if (this is ITimeDayMultiListenerFull dayMulti)
			{
				dayMultiListeners.Add(dayMulti);
			}
			return dayMultiListeners;
		}

		byte ITimeDailyListenerSimple.hourToTrigger => eggHourTrigger;

		string ITimeDailyListenerSimple.ReactToDailyTrigger() => ReactToDailyTrigger();

		protected virtual byte eggHourTrigger => 0;

		protected virtual string ReactToDailyTrigger()
		{
			return null;
		}

		public override bool IsIdenticalTo(WombData originalData, bool ignoreSexualData)
		{
			return !(originalData is null) && eggsEveryXDays == originalData.eggsEveryXDays
				&& BodyPartHelpers.AreIdentical(normalPregnancy, originalData.vaginalPregnancyStore, ignoreSexualData)
				&& BodyPartHelpers.AreIdentical(analPregnancy, originalData.analPregnancyStore, ignoreSexualData)
				&& BodyPartHelpers.AreIdentical(secondaryNormalPregnancy, originalData.secondVaginaPregnancyStore, ignoreSexualData)
				&& allowsDiapause == originalData.allowsDiapause && hasDiapause == originalData.hasDiapause && allowsDiapauseRemoval == originalData.allowsDiapauseRemoval
				&& allowsOviposition == originalData.allowsOviposition && hasOviposition == originalData.hasOviposition
				&& allowsOvipositionRemoval == originalData.allowsOvipositionRemoval && eggSize == originalData.eggSize;
		}
	}

	public class WombData : SimpleData
	{
		//if null, cannot get pregnant via normal vagina.
		public readonly ReadOnlyPregnancyStore vaginalPregnancyStore;
		public readonly Func<bool, bool> canGetPregnantIfHasVagina;

		public readonly ReadOnlyPregnancyStore analPregnancyStore;
		public readonly Func<bool, bool, bool> canGetAnallyPregnantIfHasAnus;

		public readonly ReadOnlyPregnancyStore secondVaginaPregnancyStore;
		public readonly Func<bool, bool> canGetPregnantIfHasSecondVagina;

		public readonly bool hasDiapause;
		public readonly bool allowsDiapause;
		public readonly bool allowsDiapauseRemoval;

		public readonly bool hasOviposition;
		public readonly bool allowsOviposition;
		public readonly bool allowsOvipositionRemoval;

		public readonly byte eggsEveryXDays;
		public readonly bool? eggSize;

		public bool eggSizeKnown => eggSize != null;
		public bool defaultKnownEggSize => eggSize ?? false;

		public WombData(Guid creatureID, ReadOnlyPregnancyStore vaginalPregnancyStore, Func<bool, bool> canGetPregnantIfHasVagina,
			ReadOnlyPregnancyStore analPregnancyStore, Func<bool, bool, bool> canGetAnallyPregnantIfHasAnus,
			ReadOnlyPregnancyStore secondVaginaPregnancyStore, Func<bool, bool> canGetPregnantIfHasSecondVagina, byte eggsAfterDays, bool? eggSizeKnown,
			bool canActivateDiapause, bool diapause, bool canDeactivateDiapause, bool canActivateOviposition, bool oviposition, bool canDeactivateOviposition) : base(creatureID)
		{
			this.vaginalPregnancyStore = vaginalPregnancyStore;
			this.canGetPregnantIfHasVagina = canGetPregnantIfHasVagina ?? throw new ArgumentNullException(nameof(canGetPregnantIfHasVagina));
			this.analPregnancyStore = analPregnancyStore;
			this.canGetAnallyPregnantIfHasAnus = canGetAnallyPregnantIfHasAnus ?? throw new ArgumentNullException(nameof(canGetAnallyPregnantIfHasAnus));
			this.secondVaginaPregnancyStore = secondVaginaPregnancyStore;
			this.canGetPregnantIfHasSecondVagina = canGetPregnantIfHasSecondVagina ?? throw new ArgumentNullException(nameof(canGetPregnantIfHasSecondVagina));

			eggsEveryXDays = eggsAfterDays;

			eggSize = eggSizeKnown;

			this.allowsDiapause = canActivateDiapause;
			hasDiapause = diapause;
			allowsDiapauseRemoval = canDeactivateDiapause;

			allowsOviposition = canActivateOviposition;
			hasOviposition = oviposition;
			allowsOvipositionRemoval = canDeactivateOviposition;

		}

		protected internal WombData(Womb source) : base(source?.creatureID ?? throw new ArgumentNullException(nameof(source)))
		{
			vaginalPregnancyStore = source.normalPregnancy?.AsReadOnlyData();
			analPregnancyStore = source.analPregnancy?.AsReadOnlyData();
			secondVaginaPregnancyStore = source.secondaryNormalPregnancy?.AsReadOnlyData();

			canGetPregnantIfHasVagina = source.canGetPregnant;
			canGetAnallyPregnantIfHasAnus = source.canGetAnallyPregnant;
			canGetPregnantIfHasSecondVagina = source.canGetSecondaryNormalPregnant;

			allowsDiapause = source.allowsDiapause;
			hasDiapause = source.hasDiapause;
			allowsDiapauseRemoval = source.allowsDiapauseRemoval;

			allowsOviposition = source.allowsOviposition;
			hasOviposition = source.hasOviposition;
			allowsOvipositionRemoval = source.allowsOvipositionRemoval;

			eggsEveryXDays = source.eggsEveryXDays;
			eggSize = source.normalPregnancy.eggSizeKnown ? (bool?)source.normalPregnancy.eggsLarge : null;
		}



	}
}
