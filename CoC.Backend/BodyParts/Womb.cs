using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Engine.Time;
using CoC.Backend.Pregnancies;
using System;
using System.Collections.Generic;
using System.Text;

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

		protected Womb(Guid creatureID, PregnancyStore primaryVagina, PregnancyStore anus, PregnancyStore secondaryVagina) : base(creatureID)
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
			return valid;
		}

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
