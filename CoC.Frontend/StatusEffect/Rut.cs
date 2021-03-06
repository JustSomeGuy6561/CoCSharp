﻿using System;
using CoC.Backend;
using CoC.Backend.BodyParts;
using CoC.Backend.Engine;
using CoC.Backend.Engine.Time;
using CoC.Backend.Perks;
using CoC.Backend.Reaction;
using CoC.Backend.Tools;
namespace CoC.Frontend.StatusEffect
{
	public sealed class Rut : TimedPerk
	{
		public sbyte totalAddedLibido { get; private set; }
		public sbyte totalAddedVirility { get; private set; }

		private bool active = false;

		private const sbyte LIBIDO_STACK = 10;
		private const sbyte VIRILITY_STACK = 5;

		public const byte TIMEOUT_STACK = 48;

		private const sbyte MAX_LIBIDO = 50;
		private const sbyte MAX_VIRILITY_BOOST = 25; //25% chance max increase.

		public const ushort MAX_TIMEOUT = 720;

		public Rut() : this(2 * TIMEOUT_STACK) { }

		public Rut(ushort initialTimeout) : base(initialTimeout)
		{
		}

		public override string Name()
		{
			return "Rut";
		}


		public override string ObtainText() => GainedRutText();

		public override string HasPerkText() => throw new NotImplementedException();

		protected override void OnActivation()
		{
			if (!active)
			{
				active = true;

				totalAddedVirility = VIRILITY_STACK;
				totalAddedLibido = LIBIDO_STACK;

				AddModifierToPerk(baseModifiers.minLibidoDelta, new ValueModifierStore<sbyte>(ValueModifierType.RELATIVE, totalAddedLibido));
				AddModifierToPerk(baseModifiers.perkBonusVirility, new ValueModifierStore<sbyte>(ValueModifierType.RELATIVE, totalAddedVirility));

			}
		}

		private void Genitals_onGenderChanged(object sender, Backend.BodyParts.EventHelpers.GenderChangedEventArgs e)
		{
			if (!e.newGender.HasFlag(Gender.MALE))
			{
				GameEngine.AddOneOffReaction(new OneOffTimeReaction(new GenericSimpleReaction(DoReaction)));
			}
		}

		protected override void OnRemoval()
		{
			if (active)
			{


				totalAddedLibido = 0;
				totalAddedVirility = 0;

				sourceCreature.genitals.onGenderChanged -= Genitals_onGenderChanged;

				active = false;
			}
		}

		protected override string OnStatusEffectTimePassing(byte hoursPassedSinceLastUpdate, out bool removeEffect)
		{
			removeEffect = !sourceCreature.hasCock;

			if (removeEffect)
			{
				return RemoveText();
			}
			else
			{
				return null;
			}
		}

		internal bool IncreaseRut(byte stack = 1)
		{
			if (active)
			{
				bool increased = false;
				byte timeDelta = stack;
				if (totalAddedLibido < MAX_LIBIDO)
				{
					totalAddedLibido += LIBIDO_STACK;
					UpdatePerkModifier(baseModifiers.minLibidoDelta, new ValueModifierStore<sbyte>(ValueModifierType.RELATIVE, totalAddedLibido));
					increased = true;

				}
				else
				{
					timeDelta.addIn(2);
				}

				if (totalAddedVirility < MAX_VIRILITY_BOOST)
				{
					totalAddedVirility += VIRILITY_STACK;
					//update the modifier value with the new stack amount.
					UpdatePerkModifier(baseModifiers.perkBonusVirility, new ValueModifierStore<sbyte>(ValueModifierType.RELATIVE, totalAddedVirility));

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

		public string IncreasedRutText()
		{
			string cockText;
			if (sourceCreature.cocks.Count > 1)
			{
				cockText = sourceCreature.genitals.AllCocksFullDescription();
			}
			else if (sourceCreature.cocks.Count > 0)
			{
				cockText = sourceCreature.cocks[0].LongDescription();
			}
			else
			{
				return null;
			}
			return Environment.NewLine + Environment.NewLine + "Your " + cockText + " throbs and dribbles as your desire to mate intensifies. You know that " +
				SafelyFormattedString.FormattedText("you've sunken deeper into rut", StringFormats.BOLD) + ", but all that really matters is unloading into a cum-hungry cunt.";
		}

		protected override string OnStatusEffectWoreOff(byte hoursPassedSinceLastUpdate)
		{
			return RemoveText();
		}

		private string RemoveText()
		{
			return Environment.NewLine + SafelyFormattedString.FormattedText("Your body calms down, at last getting over your rut.", StringFormats.BOLD) + Environment.NewLine;
		}

		private string DoReaction(bool currentlyIdling, bool hasIdleHours)
		{
			sourceCreature.perks.RemoveTimedEffect(this);
			return RemoveText();
		}



		private string GainedRutText()
		{
			string cockText;
			if (sourceCreature.cocks.Count > 1)
			{
				cockText = sourceCreature.genitals.AllCocksFullDescription();
			}
			else if (sourceCreature.cocks.Count > 0)
			{
				cockText = sourceCreature.cocks[0].LongDescription();
			}
			else
			{
				return null;
			}
			return Environment.NewLine + Environment.NewLine + "You stand up a bit straighter and look around, sniffing the air and searching for a mate. " +
				"Wait, what!? It's hard to shake the thought from your head - you really could use a nice fertile hole to impregnate. You slap your forehead and realize " +
				SafelyFormattedString.FormattedText("you've gone into rut", StringFormats.BOLD) + "!";
		}

		public override bool isAilment => true;
	}
}
