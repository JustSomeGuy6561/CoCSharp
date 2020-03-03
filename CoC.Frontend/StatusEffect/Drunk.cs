using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Engine;
using CoC.Backend.Engine.Time;
using CoC.Backend.Perks;
using CoC.Backend.Strings;

namespace CoC.Frontend.StatusEffect
{
	//in original, it seems to never wear off, idk. i'm going with 6 hours because i can, i guess.
	//Afaik this has no effects on stats - it's just so you can drink enough to get an achievement for drinking too much.
	internal class Drunk : TimedPerk
	{
		public const byte DEFAULT_TIMEOUT = 6;
		private const byte REFRESH_TIMER = 4;

		public Drunk(ushort initialTimeout) : base(initialTimeout)
		{
		}

		public Drunk() : this(DEFAULT_TIMEOUT)
		{
		}

		public byte stacks { get; private set; } = 0;


		public override bool isAilment => true;

		protected override void OnActivation()
		{
			stacks = 1;
		}

		public string StackEffect()
		{
			stacks++;
			if (GameDateTime.Now.HoursTo(timeWearsOff) < 6)
			{
				timeWearsOff = timeWearsOff.Delta(REFRESH_TIMER);
			}
			//Get so drunk you end up peeing! Genderless can still urinate.
			if (stacks >= 3)
			{
				StringBuilder sb = new StringBuilder();
				//print out piss like racehorse text.
				sb.Append(GlobalStrings.NewParagraph() + "You feel so drunk; your vision is blurry and you realize something's not feeling right. Gasp! " +
					"You have to piss like a racehorse! You stumble toward the nearest bush");
				string target = sourceCreature.hasCock ? "wall" : "ground";
				sb.Append(sourceCreature.LowerBodyArmorTextHelper(", open up your " + sourceCreature.armor.ItemName(), ", remove your " + sourceCreature.lowerGarment.ItemName(),
						"") + " and release your pressure onto the " + target + ". ");

				sb.Append("It's like as if the floodgate has opened! ");
				GameEngine.UnlockAchievement<Achievements.Smashed>();
				GameEngine.UnlockAchievement<Achievements.UrineTrouble>();
				sb.Append(GlobalStrings.NewParagraph() + "It seems to take forever but it eventually stops. You look down to see that your urine has been absorbed into the ground.");

				//grant achievements.
				stacks = 0;
				timeWearsOff = GameDateTime.Now;
				return sb.ToString();
			}
			else if (stacks == 2)
			{
				return GlobalStrings.NewParagraph() + "<b>You feel a bit drunk. Maybe you should cut back on the beers?</b>";
				//print out very drunk text.
			}
			else
			{
				//print out drunk test. Currently no text here, idk.
				return null;
			}
		}

		protected override string OnStatusEffectWoreOff(byte hoursPassedSinceLastUpdate)
		{
			if (hoursPassedSinceLastUpdate >= 4)
			{
				return "Over the last few hours, you've gotten over the buzz of alcohol (and the subsequent hangover), and are once again right as rain.";
			}
			else
			{
				return "The slight buzz you've been feeling has worn off, though you almost wish it wasn't. You're gonna be hung over for a little while.";
			}
		}

		protected override void OnRemoval()
		{

		}

		public override string Name()
		{
			return stacks > 1 ? "Very Drunk" : "Drunk";
		}

		public override string HasPerkText()
		{
			return stacks > 1
				? "You've been drinking heavily - perhaps you should stop?"
				: "The alcohol you've consumed has made you drunk, though you're still mostly in control.";
		}
	}
}
