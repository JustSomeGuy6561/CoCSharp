using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Engine.Time;
using CoC.Backend.Pregnancies;
using System;
using System.Collections.Generic;
using System.Text;
using WeakEvent;

namespace CoC.Backend.BodyParts
{
	public abstract class Womb : SimpleSaveablePart<Womb, WombData>
	{
		//Note: we don't attach these to vagina and ass b/c it's possible to lose a vagina (and perhaps an asshole too if it's possible to TF to anemone or something)
		//and we wouldn't want this to cause the pregnancy to be lost. 


		//if null, cannot get pregnant via normal vagina.
		public readonly PregnancyStore normalPregnancy;
		public bool canGetPregnant(bool hasVagina) => normalPregnancy != null && canGetPregnantCheck(hasVagina);
		protected virtual bool canGetPregnantCheck(bool hasVagina) => hasVagina;

		//if null, cannot get anally pregnant. 
		public readonly PregnancyStore analPregnancy;
		//basically, by default, the normal creature cannot become anally pregnant, unless the source attempting to anally knock them up expressly says they don't care.
		//most anal pregnancy attempts will respect the Womb's stance on anal pregnancies and therefore fail. as of now, the only thing that ignores this is a satyr, or PC with satyr sexuality.
		//note that it's possible to have a womb that prevents all anal pregnancies without overriding this by setting anal pregnacy store to null.
		//Note that this means creatures 
		public bool canGetAnallyPregnant(bool hasAnus, bool sourceOverridesNoAnalPregnancies) => analPregnancy != null && canGetAnallyPregnantCheck(hasAnus, sourceOverridesNoAnalPregnancies);
		protected virtual bool canGetAnallyPregnantCheck(bool hasAnus, bool sourceOverridesNoAnalPregnancies) => hasAnus && sourceOverridesNoAnalPregnancies;

		//allows a third pregnancy store for creatures with two vaginas. defaults to null, so we can't get pregnant through a second vagina. 
		public readonly PregnancyStore secondaryNormalPregnancy;

		//same as normal pregnancy, though this one uses second vagina. since secondaryNormalPregnancy defaults to null, this defaults to false.
		public bool canGetSecondaryNormalPregnant(bool hasSecondVagina) => secondaryNormalPregnancy != null && canGetSecondaryNormalPregnantCheck(hasSecondVagina);
		protected virtual bool canGetSecondaryNormalPregnantCheck(bool hasSecondVagina) => hasSecondVagina;

		public uint totalBirthCount => normalPregnancy?.birthCount + analPregnancy?.birthCount + secondaryNormalPregnancy?.birthCount ?? 0;

		protected Womb(Guid creatureID, VaginalPregnancyStore primaryVagina, AnalPregnancyStore anus, VaginalPregnancyStore secondaryVagina) : base(creatureID)
		{
			normalPregnancy = primaryVagina;
			analPregnancy = anus;
			secondaryNormalPregnancy = secondaryVagina;
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
				secondaryNormalPregnancy?.AsReadOnlyData(), canGetSecondaryNormalPregnant));
		}

		private void Anal_dataChange(object sender, EventHelpers.SimpleDataChangeEvent<PregnancyStore, ReadOnlyPregnancyStore> e)
		{
			NotifyDataChanged(new WombData(creatureID, normalPregnancy?.AsReadOnlyData(), canGetPregnant, e.oldValues, canGetAnallyPregnant,
				secondaryNormalPregnancy?.AsReadOnlyData(), canGetSecondaryNormalPregnant));
		}

		private void Secondary_dataChange(object sender, EventHelpers.SimpleDataChangeEvent<PregnancyStore, ReadOnlyPregnancyStore> e)
		{
			NotifyDataChanged(new WombData(creatureID, normalPregnancy?.AsReadOnlyData(), canGetPregnant, analPregnancy?.AsReadOnlyData(), canGetAnallyPregnant,
				e.oldValues, canGetSecondaryNormalPregnant));
		}

		protected internal bool AttemptNormalKnockUp(float knockupChance, StandardSpawnType type)
		{
			return normalPregnancy.attemptKnockUp(knockupChance, type);
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

		protected internal bool AttemptSecondaryKnockUp(float knockupChance, StandardSpawnType type)
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

		protected internal bool AttemptAnalKnockUp(float knockupChance, StandardSpawnType type)
		{
			return analPregnancy.attemptKnockUp(knockupChance, type);
		}

		protected internal void ResetAnal()
		{
			analPregnancy.Reset(true);
		}

		protected readonly WeakEventSource<BirthEvent> birthEventSource =
			new WeakEventSource<BirthEvent>();

		public event EventHandler<BirthEvent> onBirth
		{
			add => birthEventSource.Subscribe(value);
			remove => birthEventSource.Unsubscribe(value);
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

		protected abstract bool ExtraValidations(bool currentlyValid, bool correctInvalidData);

		internal List<ITimeActiveListener> GetActiveListeners()
		{
			List<ITimeActiveListener> activeListeners = new List<ITimeActiveListener>();
			ITimeActiveListener activeListener = null;
			activeListener = normalPregnancy as ITimeActiveListener;
			if (activeListener != null)
			{
				activeListeners.Add(activeListener);
			}
			activeListener = analPregnancy as ITimeActiveListener;
			if (activeListener != null)
			{
				activeListeners.Add(activeListener);
			}
			activeListener = secondaryNormalPregnancy as ITimeActiveListener;
			if (activeListener != null)
			{
				activeListeners.Add(activeListener);
			}
			return activeListeners;
		}

		internal List<ITimeLazyListener> GetLazyListeners()
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
			return lazyListeners;
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

		public WombData(Guid creatureID, ReadOnlyPregnancyStore vaginalPregnancyStore, Func<bool, bool> canGetPregnantIfHasVagina, 
			ReadOnlyPregnancyStore analPregnancyStore, Func<bool, bool, bool> canGetAnallyPregnantIfHasAnus, 
			ReadOnlyPregnancyStore secondVaginaPregnancyStore, Func<bool, bool> canGetPregnantIfHasSecondVagina) : base(creatureID)
		{
			this.vaginalPregnancyStore = vaginalPregnancyStore;
			this.canGetPregnantIfHasVagina = canGetPregnantIfHasVagina ?? throw new ArgumentNullException(nameof(canGetPregnantIfHasVagina));
			this.analPregnancyStore = analPregnancyStore;
			this.canGetAnallyPregnantIfHasAnus = canGetAnallyPregnantIfHasAnus ?? throw new ArgumentNullException(nameof(canGetAnallyPregnantIfHasAnus));
			this.secondVaginaPregnancyStore = secondVaginaPregnancyStore;
			this.canGetPregnantIfHasSecondVagina = canGetPregnantIfHasSecondVagina ?? throw new ArgumentNullException(nameof(canGetPregnantIfHasSecondVagina));
		}

		internal WombData(Womb source) : base(source?.creatureID ?? throw new ArgumentNullException(nameof(source)))
		{
			vaginalPregnancyStore = source.normalPregnancy?.AsReadOnlyData();
			analPregnancyStore = source.analPregnancy?.AsReadOnlyData();
			secondVaginaPregnancyStore = source.secondaryNormalPregnancy?.AsReadOnlyData();

			canGetPregnantIfHasVagina = source.canGetPregnant;
			canGetAnallyPregnantIfHasAnus = source.canGetAnallyPregnant;
			canGetPregnantIfHasSecondVagina = source.canGetSecondaryNormalPregnant;
		}


	}
}
