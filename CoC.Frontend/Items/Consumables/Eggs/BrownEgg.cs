using System;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;

namespace CoC.Frontend.Items.Consumables
{


	/**
	 * @since March 31, 2018
	 * @author Stadler76
	 */
	public class BrownEgg : EggBase
	{

		public BrownEgg(bool isLarge) : base(isLarge)
		{ }

		public override string AbbreviatedName()
		{
			return isLarge ? "L.BrnEg" : "BrownEg";
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";
			string sizeText = isLarge ? "large " : "";
			string eggText = count == 1 ? "egg" : "eggs";

			return $"{count}{sizeText}brown and white mottled {eggText}";
		}
		public override byte sateHungerAmount => isLarge ? (byte)60 : (byte)20;

		public override string Color()
		{
			return Tones.BROWN.AsString();
		}

		public override bool CanUse(Creature target, bool currentlyInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}
		public override bool EqualsIgnoreSize(EggBase other)
		{
			return other is BrownEgg;
		}

		protected override string OnConsumeAttempt(Creature consumer, out bool consumeItem, out bool isBadEnd)
		{
			isBadEnd = false;

			StringBuilder sb = new StringBuilder();
			sb.Append("You devour the egg, momentarily sating your hunger." + GlobalStrings.NewParagraph());
			if (!isLarge)
			{
				sb.Append("You feel a bit of additional weight on your backside as your " + consumer.butt.ShortDescription() + " gains a bit more padding.");
				consumer.butt.GrowButt();
			}
			else
			{
				sb.Append("Your " + consumer.build.ButtLongDescription() + " wobbles, nearly throwing you off balance as it grows much bigger!");
				consumer.butt.GrowButt((byte)(2 + Utils.Rand(3)));

			}
			if (Utils.Rand(3) == 0)
			{
				if (isLarge)
				{
					sb.Append(consumer.ModifyTone(100, 8));
				}
				else
				{
					sb.Append(consumer.ModifyThickness(95, 3));
				}
			}
			consumeItem = true;
			return sb.ToString();
		}
	}
}
