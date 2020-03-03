using System;
using System.Linq;
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
	public class BlueEgg : EggBase
	{
		public override string AbbreviatedName()
		{
			return isLarge ? "L.BluEgg" : "BlueEgg";
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";
			string sizeText = isLarge ? "large " : "";
			string eggText = count == 1 ? "egg" : "eggs";

			return $"{count}{sizeText}blue and white mottled {eggText}";
		}

		public BlueEgg(bool isLarge) : base(isLarge)
		{ }

		protected override int monetaryValue => DEFAULT_VALUE;

		public override byte sateHungerAmount => isLarge ? (byte)60 : (byte)20;

		public override string Color()
		{
			return Tones.BLACK.AsString();
		}

		public override bool CanUse(Creature target, bool currentlyInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		//MOD: large eggs will now firmly make you male, just like pink eggs do (except female, of course)
		//This means they will grow balls and a cock if you don't have them.
		//small eggs will now remove one vagina at a time, and are able to handle multiple vaginas.
		//similarly, small eggs will now remove one extra breast row at a time.
		//a large egg will remove each additional breast row, and will also make the last pair as manly as possible. they will remove fuckable nipples while doing so,
		//but they will become inverted if fuckable.
		protected override bool OnConsumeAttempt(Creature consumer, out string resultsOfUse, out bool isBadEnd)
		{
			isBadEnd = false;

			StringBuilder sb = new StringBuilder();

			sb.Append("You devour the egg, momentarily sating your hunger.");

			//MOD: doing this is logical order - start with breasts and work our way down.

			//remove extra breast rows.
			//if only 1 extra row or it's a small egg, remove 1 extra row.
			if (consumer.breasts.Count > 1 && (consumer.breasts.Count == 2 || !isLarge))
			{
				sb.Append("Your back relaxes as extra weight vanishes from your chest. <b>Your lowest " + consumer.breasts[consumer.breasts.Count - 1].LongDescription()
					+ " have vanished.</b>");

				consumer.RemoveBreastRow();
				sb.Append(GlobalStrings.NewParagraph());
			}
			//if large egg, remove all extra breast rows.
			else if (consumer.breasts.Count > 2 && isLarge)
			{
				sb.Append("Your back relaxes as a significant amount of weight vanishes from your chest. <b>You've lost all but your top-most row of breasts!</b>");

				consumer.RemoveExtraBreastRows();
				sb.Append(GlobalStrings.NewParagraph());
			}

			//Kill pussies!
			//if small on only have 1, remove 1.
			if (consumer.hasVagina && (consumer.vaginas.Count == 1 || !isLarge))
			{
				sb.Append(consumer.genitals.OneVaginaOrVaginasNoun(Conjugate.YOU, out bool isPlural).CapitalizeFirstLetter() + (isPlural ? "clench" : "clenches")
					+ " in pain, doubling you over. You slip a hand down to check on it, only to feel the slit growing smaller and smaller until it disappears");

				//if large, and we don't have a cock, grow one out of old clit.
				if (isLarge && !consumer.hasCock)
				{
					sb.Append(". Your clit, however, seems to be resisting the change, and instead expands outward. The top flares outward into a distinct mushroom-shape," +
						"and you quickly realize " + SafelyFormattedString.FormattedText("Your vagina has been replaced with a cock!", StringFormats.BOLD));

					var length = consumer.clits[0].length + 5;
					var width = length / 5;

					//also grow a pair of balls if needed.
					consumer.AddCock(CockType.defaultValue, length, width);

					if (!consumer.hasBalls)
					{
						consumer.balls.GrowBalls();
						sb.Append(SafelyFormattedString.FormattedText("A pair of balls drop in below it, completing the change in gender.", StringFormats.BOLD));
					}
				}
				else
				{
					sb.Append(", taking your clit with it!");
					if (consumer.vaginas.Count == 1)
					{
						sb.Append(" <b>Your vagina is gone!</b>");
					}
				}
				consumer.RemoveVagina();

				sb.Append(GlobalStrings.NewParagraph());

			}
			//if large, and more than one, remove them both.
			else if (consumer.hasVagina && isLarge)
			{
				sb.Append("You double over in pain, and quickly pinpoint the source - your female sexes. You slip a hand down to check on them, and realize " +
					"they are both shrinking inward, your feminine entrances shrinking smaller and smaller until both disappear completely");

				if (!consumer.hasCock)
				{
					sb.Append(". Your clits, however, seem to resist the change, at least momentarily. Instead, they merge together, then expand outward, growing wider and longer." +
						"The top flares outward into a distinct mushroom-shape, and you quickly realize " + SafelyFormattedString.FormattedText("Your feminine sexes have been " +
						"replaced by a cock!", StringFormats.BOLD));

					var length = consumer.clits.Sum(x => x.length) + 5;
					var width = length / 5;

					consumer.AddCock(CockType.defaultValue, length, width);


					if (!consumer.hasBalls)
					{
						consumer.balls.GrowBalls();
						sb.Append(SafelyFormattedString.FormattedText("A pair of balls drop in below it, completing the change in gender.", StringFormats.BOLD));
					}
				}
				else
				{
					sb.Append(", taking their clits with them! " + SafelyFormattedString.FormattedText("Your vaginas are gone!", StringFormats.BOLD));
				}
				consumer.RemoveAllVaginas();
				sb.Append(GlobalStrings.NewParagraph());
			}

			//Dickz
			//if you have any, grow them, regardless of size. the large egg grows more.
			if (consumer.hasCock)
			{
				sb.Append(GlobalStrings.NewParagraph() + "Your " + consumer.genitals.AllCocksLongDescription(out bool isPlural)
					+ (isPlural ? " fill" : "fills") + " to full-size... and " + (isPlural ? "begin" : "begins") + " growing obscenely.");

				CockCollectionData cockCollection = consumer.genitals.allCocks.AsReadOnlyData();

				double averageWidthDelta = 0;
				foreach (var cock in consumer.cocks)
				{
					cock.IncreaseLength(3 + Utils.Rand(isLarge ? 5 : 2));
					averageWidthDelta += cock.IncreaseThickness(1);
				}

				averageWidthDelta /= consumer.cocks.Count;

				sb.Append(consumer.genitals.allCocks.GenericChangeCockLengthText(cockCollection));
				sb.Append(GlobalStrings.NewParagraph());
				//Display the degree of thickness change.
				if (averageWidthDelta >= 1)
				{
					sb.Append("Your " + consumer.genitals.AllCocksShortDescription() + (isPlural ? " spread" : " spreads") +
						" rapidly, swelling with over an inch of added girth, making " + (isPlural ? "them" : "it") + " feel fat and floppy.");
				}
				else if (averageWidthDelta > 0.5)
				{
					sb.Append("Your " + consumer.genitals.AllCocksShortDescription() + (isPlural ? " seem" : " seems") + " to swell up, feeling heavier. " +
						"You look down and watch " + (isPlural ? "them growing fatter as they thicken." : "it growing fatter as it thickens."));
				}
				else
				{
					sb.Append("Your " + consumer.genitals.AllCocksShortDescription() + (isPlural ? " feel" : " feels") + " swollen and heavy. " +
						"With a firm, but gentle, squeeze, you confirm your suspicions - " + (isPlural ? "they are" : "it is") + " definitely thicker.");
				}

				if (!consumer.balls.hasBalls && isLarge)
				{
					sb.Append("A pair of balls grow in, complementing your enlarged " + (isPlural ? "cocks" : "cock") + " nicely, completing your manly sex.");

					consumer.balls.GrowBalls();
				}

				consumer.DeltaCreatureStats(lib: 1, sens: 1, lus: 20);


			}
			//if you don't, and it's a large egg, get one. it's just gonna be default size.
			else if (isLarge)
			{
				consumer.AddCock();
				sb.Append("A sudden pressure forms in your groin, mixed with an inescapable sense of enquenched sexual need, like an itch you can't reach. " +
					"It threatens to overwhelm you until suddenly a length of flesh bursts from your loins. You immediately realize "
					+ SafelyFormattedString.FormattedText("you now have a cock!", StringFormats.BOLD));

				consumer.AddCock();

				if (!consumer.hasBalls)
				{
					sb.Append(SafelyFormattedString.FormattedText("A pair of balls drop below your newly formed cock, completing the look.", StringFormats.BOLD));

					consumer.balls.GrowBalls();
				}
			}

			//butt and hips.
			if (isLarge)
			{
				//Ass/hips shrinkage!
				if (consumer.butt.size > consumer.butt.smallestPossibleButtSize)
				{
					sb.Append("Muscles firm and tone as you feel your " + consumer.build.ButtLongDescription() + " becomes smaller and tighter.");
					if (consumer.hips.size > 5)
					{
						sb.Append(" ");
					}

					consumer.butt.ShrinkButt(2);
				}
				if (consumer.hips.size > consumer.hips.smallestPossibleHipSize)
				{
					sb.Append("Feeling the sudden burning of lactic acid in your " + consumer.build.HipsLongDescription() + ", you realize they have slimmed down and firmed up some.");

					consumer.hips.ShrinkHips(2);
				}
				//Shrink tits!
				if (consumer.breasts[0].cupSize > consumer.genitals.smallestPossibleMaleCupSize)
				{
					consumer.breasts[0].MakeMale();
				}
			}

			//femininity if possible.
			if (Utils.Rand(3) == 0)
			{
				byte target, delta;
				if (isLarge)
				{
					target = 0;
					delta = 8;
				}
				else
				{
					target = 5;
					delta = 3;
				}
				sb.Append(consumer.ModifyFemininity(target, delta));
			}

			resultsOfUse = sb.ToString();
			return true;
		}

		public override bool Equals(EggBase other)
		{
			return other is BlueEgg && other.isLarge == isLarge;
		}

		public override bool EqualsIgnoreSize(EggBase other)
		{
			return other is BlueEgg;
		}
	}
}
