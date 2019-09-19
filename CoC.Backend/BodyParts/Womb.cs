using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Engine.Time;
using CoC.Backend.Pregnancies;
using System;

namespace CoC.Backend.BodyParts
{
	public abstract class Womb : SimpleSaveablePart<Womb, WombData>, ITimeDailyListener, ITimeActiveListener, ITimeLazyListener
	{
		//Note: we don't attach these to vagina and ass b/c it's possible to lose a vagina (and perhaps an asshole too if it's possible to TF to anemone or something)
		//and we wouldn't want this to cause the pregnancy to be lost. 


		//if null, cannot get pregnant via normal vagina.
		public readonly PregnancyStore normalPregnancy;
		public virtual bool canGetPregnant(bool hasVagina) => hasVagina && normalPregnancy != null;

		//if null, cannot get anally pregnant. 
		public readonly PregnancyStore analPregnancy;
		//basically, by default, the normal creature cannot become anally pregnant, unless the source attempting to anally knock them up expressly says they don't care.
		//most anal pregnancy attempts will respect the Womb's stance on anal pregnancies and therefore fail. as of now, the only thing that ignores this is a satyr, or PC with satyr sexuality.
		//note that it's possible to have a womb that prevents all anal pregnancies without overriding this by setting anal pregnacy store to null.
		//Note that this means creatures 
		public virtual bool canGetAnallyPregnant(bool hasAnus, bool sourceOverridesNoAnalPregnancies) => hasAnus && analPregnancy != null && sourceOverridesNoAnalPregnancies;

		//allows a third pregnancy store for creatures with two vaginas. defaults to null, so we can't get pregnant through a second vagina. 
		public readonly PregnancyStore secondaryNormalPregnancy;

		//same as normal pregnancy, though this one uses second vagina. since secondaryNormalPregnancy defaults to null, this defaults to false.
		public virtual bool canGetSecondaryNormalPregnant(bool hasSecondVagina) => hasSecondVagina && secondaryNormalPregnancy != null;

		protected Womb(Creature source, PregnancyStore primaryVagina, PregnancyStore anus, PregnancyStore secondaryVagina) : base(source)
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
			NotifyDataChanged(new WombData(e.oldValues, canGetPregnant, analPregnancy?.AsReadOnlyData(), canGetAnallyPregnant,
				secondaryNormalPregnancy?.AsReadOnlyData(), canGetSecondaryNormalPregnant));
		}

		private void Anal_dataChange(object sender, EventHelpers.SimpleDataChangeEvent<PregnancyStore, ReadOnlyPregnancyStore> e)
		{
			NotifyDataChanged(new WombData(normalPregnancy?.AsReadOnlyData(), canGetPregnant, e.oldValues, canGetAnallyPregnant,
				secondaryNormalPregnancy?.AsReadOnlyData(), canGetSecondaryNormalPregnant));
		}

		private void Secondary_dataChange(object sender, EventHelpers.SimpleDataChangeEvent<PregnancyStore, ReadOnlyPregnancyStore> e)
		{
			NotifyDataChanged(new WombData(normalPregnancy?.AsReadOnlyData(), canGetPregnant, analPregnancy?.AsReadOnlyData(), canGetAnallyPregnant,
				e.oldValues, canGetSecondaryNormalPregnant));
		}


		//public Womb(PregnancyStore normalPregnancy, PregnancyStore analPregnancy, PregnancyStore secondaryVaginalPregnancy)
		//{

		//}

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

		byte ITimeDailyListener.hourToTrigger => hourToTrigger; //midnight.

		protected virtual byte hourToTrigger => byte.MaxValue; //never triggers.

		EventWrapper ITimeDailyListener.reactToDailyTrigger()
		{
			//if (!normalPregnancy.isPregnant && laysEggs && GameEngine.CurrentDay % eggsEveryXDays == 0)
			//{
			//	normalPregnancy.attemptKnockUp(1, new PlayerEggPregnancy());
			//	return new EventWrapper(EggSpawnText());
			//}
			//return null;
			return reactToDailyTrigger();
		}

		protected virtual EventWrapper reactToDailyTrigger()
		{
			return null;
		}

		EventWrapper ITimeActiveListener.reactToHourPassing()
		{
			return reactToHourPassing();
		}

		protected virtual EventWrapper reactToHourPassing()
		{
			EventWrapper wrapper = null;
			//iirc we only do anal normalPregnancy text if it's bigger than regular one. 
			if (normalPregnancy?.isPregnant == true)
			{
				wrapper = normalPregnancy.reactToHourPassing();
			}
			if (secondaryNormalPregnancy.isPregnant)
			{
				EventWrapper secondVagEvent = secondaryNormalPregnancy.reactToHourPassing();
				if (wrapper != null)
				{
					wrapper.Append(secondVagEvent);
				}
				else
				{
					wrapper = secondVagEvent;
				}
			}
			if (analPregnancy.isPregnant)
			{
				EventWrapper analEvent = analPregnancy.reactToHourPassing();
				if (wrapper != null)
				{
					wrapper.Append(analEvent);
				}
				else
				{
					wrapper = analEvent;
				}
			}
			return wrapper;
		}

		EventWrapper ITimeLazyListener.reactToTimePassing(byte hoursPassed)
		{
			return reactToTimePassing(hoursPassed);
		}

		protected virtual EventWrapper reactToTimePassing(byte hoursPassed)
		{
			EventWrapper wrapper = EventWrapper.Empty;
			if (normalPregnancy?.isPregnant == true)
			{
				wrapper = normalPregnancy.reactToTimePassing(hoursPassed);
			}
			if (secondaryNormalPregnancy?.isPregnant == true)
			{
				EventWrapper secondVagEvent = secondaryNormalPregnancy.reactToTimePassing(hoursPassed);
				if (wrapper == null)
				{
					wrapper = secondVagEvent;
				}
				else
				{
					wrapper.Append(secondVagEvent);
				}
			}
			if (analPregnancy?.isPregnant == true)
			{
				EventWrapper analWrapper = analPregnancy.reactToTimePassing(hoursPassed);
				if (wrapper == null)
				{
					wrapper = analWrapper;
				}
				else
				{
					wrapper.Append(analWrapper);
				}
			}
			return wrapper;
		}
	}

	public class WombData
	{
		//if null, cannot get pregnant via normal vagina.
		public readonly ReadOnlyPregnancyStore vaginalPregnancyStore;
		public readonly Func<bool, bool> canGetPregnantIfHasVagina;

		public readonly ReadOnlyPregnancyStore analPregnancyStore;
		public readonly Func<bool, bool, bool> canGetAnallyPregnantIfHasAnus;

		public readonly ReadOnlyPregnancyStore secondVaginaPregnancyStore;
		public readonly Func<bool, bool> canGetPregnantIfHasSecondVagina;

		public WombData(ReadOnlyPregnancyStore vaginalPregnancyStore, Func<bool, bool> canGetPregnantIfHasVagina, 
			ReadOnlyPregnancyStore analPregnancyStore, Func<bool, bool, bool> canGetAnallyPregnantIfHasAnus, 
			ReadOnlyPregnancyStore secondVaginaPregnancyStore, Func<bool, bool> canGetPregnantIfHasSecondVagina)
		{
			this.vaginalPregnancyStore = vaginalPregnancyStore;
			this.canGetPregnantIfHasVagina = canGetPregnantIfHasVagina ?? throw new ArgumentNullException(nameof(canGetPregnantIfHasVagina));
			this.analPregnancyStore = analPregnancyStore;
			this.canGetAnallyPregnantIfHasAnus = canGetAnallyPregnantIfHasAnus ?? throw new ArgumentNullException(nameof(canGetAnallyPregnantIfHasAnus));
			this.secondVaginaPregnancyStore = secondVaginaPregnancyStore;
			this.canGetPregnantIfHasSecondVagina = canGetPregnantIfHasSecondVagina ?? throw new ArgumentNullException(nameof(canGetPregnantIfHasSecondVagina));
		}

		internal WombData(Womb source)
		{
			if (source is null) throw new ArgumentNullException(nameof(source));

			vaginalPregnancyStore = source.normalPregnancy?.AsReadOnlyData();
			analPregnancyStore = source.analPregnancy?.AsReadOnlyData();
			secondVaginaPregnancyStore = source.secondaryNormalPregnancy?.AsReadOnlyData();

			canGetPregnantIfHasVagina = source.canGetPregnant;
			canGetAnallyPregnantIfHasAnus = source.canGetAnallyPregnant;
			canGetPregnantIfHasSecondVagina = source.canGetSecondaryNormalPregnant;
		}


	}
}
