using CoC.Backend;
using CoC.Backend.BodyParts;
using CoC.Backend.Engine;
using CoC.Backend.Engine.Time;
using CoC.Backend.Reaction;
using CoC.Backend.StatusEffect;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.StatusEffect
{
	public sealed class Heat : TimedStatusEffect
	{
		public sbyte totalAddedLibido { get; private set; }
		public byte totalAddedFertility { get; private set; }

		private bool active = false;

		private const sbyte LIBIDO_STACK = 10;
		private const byte FERTILITY_STACK = 5;

		public const byte TIMEOUT_STACK = 48;

		private const sbyte MAX_LIBIDO = 50;
		private const sbyte MAX_FERTILITY_BOOST = 25; //25% chance max increase.

		public const ushort MAX_TIMEOUT = 720;

		public Heat() : this(2 * TIMEOUT_STACK) { }

		public Heat(ushort initialTimeout) : base(HeatStr, initialTimeout)
		{
		}

		private static string HeatStr()
		{
			return "Heat";
		}

		public override SimpleDescriptor obtainText => GainedHeatText;

		public override SimpleDescriptor ShortDescription => throw new NotImplementedException();

		public override SimpleDescriptor LongDescription => throw new NotImplementedException();

		protected override void OnActivation()
		{
			if (!active)
			{
				active = true;

				sbyte oldMinLibido = perkModifiers.minLibido;
				perkModifiers.minLibido += LIBIDO_STACK;
				totalAddedLibido = perkModifiers.minLibido.subtract(oldMinLibido);

				byte oldBonusFertility = perkModifiers.bonusFertility;
				perkModifiers.bonusFertility += FERTILITY_STACK;
				totalAddedFertility = perkModifiers.bonusFertility.subtract(oldBonusFertility);

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
				perkModifiers.minLibido -= totalAddedLibido;
				perkModifiers.bonusFertility -= totalAddedFertility;

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
					sbyte oldMinLibido = perkModifiers.minLibido;
					perkModifiers.minLibido += LIBIDO_STACK;
					totalAddedLibido += perkModifiers.minLibido.subtract(oldMinLibido);
					increased = true;
				}
				else
				{
					timeDelta.addIn(2);
				}

				if (totalAddedFertility < MAX_FERTILITY_BOOST)
				{
					byte oldBonusFertility = perkModifiers.bonusFertility;
					perkModifiers.bonusFertility += FERTILITY_STACK;
					totalAddedFertility += perkModifiers.bonusFertility.subtract(oldBonusFertility);
					increased = true;
				}
				else
				{
					timeDelta.addIn(2);
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
				vagText = sourceCreature.genitals.AllVaginasShortDescription();
			}
			else if (sourceCreature.vaginas.Count > 0)
			{
				vagText = sourceCreature.vaginas[0].FullDescription();
			}
			else
			{
				return null;
			}
			return Environment.NewLine + Environment.NewLine + "Your mind clouds as your " + vagText + " moistens.  Your hands begin stroking your body from top to bottom, " +
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
				vagText = sourceCreature.vaginas[0].FullDescription();
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
			sourceCollection?.RemoveStatusEffect(this);
			return RemoveText();
		}
	}
}
