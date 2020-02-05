using CoC.Backend;
using CoC.Backend.BodyParts;
using CoC.Backend.Engine;
using CoC.Backend.Engine.Time;
using CoC.Backend.Reaction;
using CoC.Backend.Perks;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.StatusEffect
{
	public sealed class Heat : TimedPerk
	{
		//we don't use v1, v2, v3 anymore because they're generic and not useful. we now have static types and each has its own variables. but that's not overly useful when porting.
		//so, for reference when porting:
		//this is old v1.
		public byte totalAddedFertility { get; private set; }
		//old v2.
		public sbyte totalAddedLibido { get; private set; }
		//time until wears off is old v3.


		private bool active = false;

		private const sbyte LIBIDO_STACK = 10;
		private const byte FERTILITY_STACK = 5;

		public const byte TIMEOUT_STACK = 48;

		private const sbyte MAX_LIBIDO = 50;
		private const sbyte MAX_FERTILITY_BOOST = 25; //25% chance max increase.

		public const ushort MAX_TIMEOUT = 720;

		public Heat() : this(2 * TIMEOUT_STACK) { }

		public Heat(ushort initialTimeout) : base(initialTimeout)
		{
		}

		public override string Name()
		{
			return "Heat";
		}

		public override string ObtainText() => GainedHeatText();

		public override string HasPerkText() => throw new NotImplementedException();

		protected override void OnActivation()
		{
			if (!active)
			{
				active = true;

				sbyte oldMinLibido = baseModifiers.minLibido;
				baseModifiers.minLibido += LIBIDO_STACK;
				totalAddedLibido = baseModifiers.minLibido.subtract(oldMinLibido);

				byte oldBonusFertility = baseModifiers.bonusFertility;
				baseModifiers.bonusFertility += FERTILITY_STACK;
				totalAddedFertility = baseModifiers.bonusFertility.subtract(oldBonusFertility);

				sourceCreature.womb.onKnockup -= Womb_onKnockup;
				sourceCreature.womb.onKnockup += Womb_onKnockup;

				sourceCreature.genitals.onGenderChanged -= Genitals_onGenderChanged;
				sourceCreature.genitals.onGenderChanged += Genitals_onGenderChanged;
			}
		}

		private void Genitals_onGenderChanged(object sender, Backend.BodyParts.EventHelpers.GenderChangedEventArgs e)
		{
			if (!e.newGender.HasFlag(Gender.FEMALE))
			{
				GameEngine.AddOneOffReaction(new OneOffTimeReactionBase(new GenericSimpleReaction(DoReaction)));
			}
		}

		private void Womb_onKnockup(object sender, Backend.Pregnancies.KnockupEvent e)
		{
			if (e.currentSpawnSource != null)
			{
				GameEngine.AddOneOffReaction(new OneOffTimeReactionBase(new GenericSimpleReaction(DoReaction)));
			}
		}

		protected override void OnRemoval()
		{
			if (active)
			{
				baseModifiers.minLibido -= totalAddedLibido;
				baseModifiers.bonusFertility -= totalAddedFertility;

				totalAddedLibido = 0;
				totalAddedFertility = 0;

				sourceCreature.womb.onKnockup -= Womb_onKnockup;
				sourceCreature.genitals.onGenderChanged -= Genitals_onGenderChanged;

				active = false;
			}
		}

		protected override string OnStatusEffectTimePassing(byte hoursPassedSinceLastUpdate, out bool removeEffect)
		{

			removeEffect = sourceCreature.womb.isPregnant || !sourceCreature.hasVagina;
			if (removeEffect)
			{
				return RemoveText();
			}
			else
			{
				return null;
			}
		}

		protected override string OnStatusEffectWoreOff()
		{

			return Environment.NewLine + SafelyFormattedString.FormattedText("Your body calms down, at last getting over your heat.", StringFormats.BOLD) + Environment.NewLine;
		}

		internal bool IncreaseHeat(byte stack = 1)
		{
			if (active)
			{
				bool increased = false;
				byte timeDelta = stack;
				if (totalAddedLibido < MAX_LIBIDO)
				{
					sbyte oldMinLibido = baseModifiers.minLibido;
					baseModifiers.minLibido += LIBIDO_STACK;
					totalAddedLibido += baseModifiers.minLibido.subtract(oldMinLibido);
					increased = true;
				}
				else
				{
					timeDelta.addIn(1);
				}

				if (totalAddedFertility < MAX_FERTILITY_BOOST)
				{
					byte oldBonusFertility = baseModifiers.bonusFertility;
					baseModifiers.bonusFertility += FERTILITY_STACK;
					totalAddedFertility += baseModifiers.bonusFertility.subtract(oldBonusFertility);
					increased = true;
				}
				else
				{
					timeDelta.addIn(1);
				}

				if (hoursRemaining + (timeDelta * TIMEOUT_STACK) < MAX_TIMEOUT)
				{
					timeWearsOff = timeWearsOff.Delta(timeDelta * TIMEOUT_STACK);
				}
				else if (hoursRemaining != MAX_TIMEOUT)
				{
					timeWearsOff = GameDateTime.Now.Delta(MAX_TIMEOUT);
					increased = true;
				}

				return increased;
			}
			else
			{
				return false;
			}
		}

		private string GainedHeatText()
		{
			string vagText;
			if (sourceCreature.vaginas.Count > 1)
			{
				vagText = sourceCreature.genitals.AllVaginasShortDescription() + " moisten";
			}
			else if (sourceCreature.vaginas.Count > 0)
			{
				vagText = sourceCreature.vaginas[0].LongDescription() + " moistens";
			}
			else
			{
				return null;
			}
			return Environment.NewLine + Environment.NewLine + "Your mind clouds as your " + vagText + " . Your hands begin stroking your body from top to bottom, " +
				"your sensitive skin burning with desire. Fantasies about bending over and presenting your needy pussy to a male overwhelm you as " +
				SafelyFormattedString.FormattedText("you realize you have gone into heat!", StringFormats.BOLD);

		}

		public string IncreasedHeatText()
		{
			string vagText;
			if (sourceCreature.vaginas.Count > 1)
			{
				vagText = sourceCreature.genitals.AllVaginasShortDescription();
			}
			else if (sourceCreature.vaginas.Count > 0)
			{
				vagText = sourceCreature.vaginas[0].LongDescription();
			}
			else
			{
				return null;
			}
			return Environment.NewLine + Environment.NewLine + "Your mind clouds as your " + vagText + " moistens. Despite already being in heat, " +
				"the desire to copulate constantly grows even larger.";
		}

		private string RemoveText()
		{
			return Environment.NewLine + "You calm down a bit and realize you no longer fantasize about getting fucked constantly. It seems your heat has ended." + Environment.NewLine;
		}

		private string DoReaction(bool currentlyIdling, bool hasIdleHours)
		{
			sourceCreature.perks.RemoveTimedEffect(this);
			return RemoveText();
		}


		public override bool isAilment => true;
	}
}
