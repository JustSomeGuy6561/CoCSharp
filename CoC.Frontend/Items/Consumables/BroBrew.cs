using System;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Perks;

namespace CoC.Frontend.Items.Consumables
{


	/**
	 * Maximum manliness!
	 */
	public class BroBrew : StandardConsumable

	{
		private const int ITEM_VALUE = 1000;

		public override string AbbreviatedName() => "BroBrew";
		public override string ItemName() => "BroBrew";
		public override string ItemDescription(byte count, bool displayCount = false)
		{
			string vialText = count != 1 ? "cans" : "can";

			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";

			return $"{count}{vialText} of Bro Brew";
		}
		public override string AboutItem() => "This aluminum can is labelled as 'Bro Brew'. It even has a picture of a muscly, bare-chested man flexing on it. A small label in the corner displays: \"Demon General's Warning: Bro Brew's effects are as potent (and irreversible) as they are refreshing.";
		protected override int monetaryValue => ITEM_VALUE;

		public BroBrew() : base()
		{ }

		public override bool CanUse(Creature target, bool currentlyInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		public override bool countsAsLiquid => true;
		public override bool countsAsCum => false;

		public override byte sateHungerAmount => 30;


		public override bool Equals(CapacityItem other)
		{
			return other is BroBrew;
		}

		//MOD NOTE: this has been changed to work like bimbo liqueur - it will remove all vaginas and restore breasts to manly defaults if
		//the futa perk is not active.
		protected override bool OnConsumeAttempt(Creature consumer, out string resultsOfUse, out bool isBadEnd)
		{
			isBadEnd = false;

			StringBuilder sb = new StringBuilder();
			//get our common bimbo/bro/futa perk store. Note that it may be null (if we don't have it).
			BimBro bimbro = consumer.GetPerkData<BimBro>();

			//intro text.

			//first check: we don't have any bro/futa/bimbo perks, because it's not in our perk list. do the standard faire.
			sb.Append(Intro(consumer, bimbro));

			if (bimbro?.hasBroEffect == true)
			{
				(consumer as CombatCreature)?.RecoverFatigue(33);
				(consumer as CombatCreature)?.AddHPPercent(1);
			}

			VaginaCollectionData oldVaginaCollection = consumer.genitals.allVaginas.AsReadOnlyData();

			//get taller. only occurs if the player does not already have the bro perk in any form.
			if ((bimbro is null || !bimbro.broBody) && consumer.build.heightInInches < 77)
			{
				short delta = consumer.build.SetHeight(77);

				sb.Append(GrewTaller(consumer, delta));
			}

			//(Tits b' gone)
			//Does not affect creature if they already have bimbo or futa perks.
			if (bimbro is null || bimbro.broBody)
			{
				//Note: minimum cup size respects current gender. we'll need to make the creature male (or genderless) first.
				consumer.RemoveAllVaginas();

				if (consumer.genitals.BiggestCupSize() >= consumer.genitals.smallestPossibleMaleCupSize)
				{
					var oldRows = consumer.genitals.allBreasts.AsReadOnlyData();
					consumer.RemovePerk<Feeder>();
					bool stoppedLactation = consumer.genitals.SetLactationTo(LactationStatus.NOT_LACTATING);

					sb.Append(ChangedBreastsText(consumer, bimbro, oldRows, stoppedLactation));
				}
			}

			//Body tone Flavor text. No effect on stats (yet)
			sb.Append(BecomeRippedFlavorText(consumer, bimbro));



			//Dick&balls growth. Affects all.

			//(has dick less than 10 inches)
			if (consumer.hasCock)
			{
				bool grewBalls = !consumer.hasBalls;
				bool lengthenedCock = false;

				if (consumer.cocks[0].length < 10)
				{
					lengthenedCock = true;
					consumer.cocks[0].SetLength(10);
					if (consumer.cocks[0].girth < 2.75)
					{
						consumer.cocks[0].SetGirth(2.75);
					}
				}

				if (!consumer.hasBalls)
				{
					consumer.balls.GrowBalls(2, 3);
				}

				sb.Append(LengthenedCock(consumer, bimbro, lengthenedCock, grewBalls));

			}
			//(No dick)
			else
			{
				consumer.AddCock(CockType.defaultValue, 12, 2.75);
				bool grewBalls = false;
				if (!consumer.balls.hasBalls)
				{
					grewBalls = true;
					consumer.balls.GrowBalls(2, 3);
				}
				sb.Append(GrewCock(consumer, bimbro, grewBalls));

			}

			//(Pussy b gone)
			//Note: only applies if consumer does not have bimbo or futa perks. also note that we already silently removed them so the breasts would resize correctly.
			//so all this does is print out the data.
			if ((bimbro is null || bimbro.broBody) && oldVaginaCollection.hasVagina)
			{
				sb.Append(RemovedAllVaginas(consumer, oldVaginaCollection));
			}

			//(below max masculinity)
			if ((bimbro is null || bimbro.broBody) && consumer.femininity > 0)
			{
				FemininityData oldFem = consumer.femininity.AsReadOnlyData();
				sb.Append(consumer.ModifyFemininity(0, 100));
				sb.Append(Masculinize(consumer, bimbro is null, oldFem));
			}

			//max tone. Thickness + 50
			//both are silent.
			consumer.build.ChangeMuscleToneToward(100, 100);
			consumer.build.ChangeThicknessToward(100, 50);

			if (consumer.intelligence > 21)
			{
				consumer.SetIntelligence((byte)Math.Floor(Math.Max(consumer.intelligenceTrue / 5.0, 21)));
			}
			consumer.DeltaCreatureStats(str: 33, tou: 33, inte: -1, lib: 4, lus: 40);

			sb.Append(Outro(consumer, bimbro, consumer.perks.HasPerk<Feeder>()));

			//apply the perks. this will also correct any silent data we missed.
			if (bimbro is null)
			{
				consumer.AddPerk(new BimBro(Gender.MALE));
			}
			else
			{
				bimbro.Broify();
			}


			////Bonus cum production!
			//sb.Append("<b>(Bro Body - Perk Gained!)" + Environment.NewLine);
			//sb.Append("(Bro Brains - Perk Gained!)</b>" + Environment.NewLine);//int to 20. max int 50)
			//if (consumer.HasPerk<Feeder>())
			//{
			//	sb.Append("<b>(Perk Lost - Feeder!)</b>" + Environment.NewLine);
			//	consumer.RemovePerk<Feeder>();
			//}

			resultsOfUse = sb.ToString();
			return true;
		}

		private string Intro(Creature creature, BimBro bimBro)
		{
			if (bimBro is null)
			{
				string inteText = creature.intelligence > 50 ? ", though you're a bit worried by how much you enjoyed the simple, brutish act." : ".";

				return ("Well, maybe this will give you the musculature that you need to accomplish your goals. You pull on the tab at the top and hear the distinctive " +
					"snap-hiss of venting, carbonating pressure. A smoky haze wafts from the opened container, smelling of hops and alcohol. You lift it to your lips, " +
					"the cold, metallic taste of the can coming to your tongue before the first amber drops of beer roll into your waiting mouth. It tingles, but it's very, " +
					"very good. You feel compelled to finish it as rapidly as possible, and you begin to chug it. You finish the entire container in seconds." +
					GlobalStrings.NewParagraph() +
					"A churning, full sensation wells up in your gut, and without thinking, you open wide to release a massive burp. It rumbles through your chest, " +
					"startling birds into flight in the distance. Awesome! You slam the can into your forehead hard enough to smash the fragile aluminum into a flat, " +
					"crushed disc. Damn, you feel stronger already" + inteText + GlobalStrings.NewParagraph());
			}

			else if (bimBro.bimboBody)
			{
				return ("The stuff hits you like a giant cube, nearly staggering you as it begins to settle.");
			}
			else
			{
				return ("You crack open the can and guzzle it in a hurry. Goddamn, this shit is the best. As you crush the can against your forehead, " +
					"you wonder if you can find a six-pack of it somewhere?" + GlobalStrings.NewParagraph());
			}
		}

		private string GrewTaller(Creature consumer, short delta)
		{
			return "... Did the ground just get farther away? You glance down and realize, you're growing! Like a sped-up flower sprout, " +
			"you keep on getting taller until finally stopping " + (Measurement.UsesMetric ? "when you're about two meters tall, give or take"
			: "around... six and a half feet, you assume") + ". Huh. You didn't expect that to happen!";

		}

		private string ChangedBreastsText(Creature consumer, BimBro bimbro, BreastCollectionData oldBreastRows, bool stoppedLactation)
		{
			StringBuilder sb = new StringBuilder();
			bool removedRows = oldBreastRows.breasts.Count > consumer.breasts.Count;
			bool madeLastRowMale = !oldBreastRows.breasts[0].isMaleBreasts;


			if (bimbro is null)
			{
				sb.Append("A tingle starts in your " + consumer.genitals.CommonLongNippleDescription() + " before the tight buds grow warm, hot even. ");
				if (stoppedLactation)
				{
					sb.Append("Somehow, you know that the milk you had been producing is gone, reabsorbed by your body. ");
				}

				sb.Append("They pinch in towards your core, shrinking along with your flattening " + oldBreastRows.ChestOrAllBreastsShort() +
					". You shudder and flex in response. Your chest isn't just shrinking, it's reforming, sculping itself into a massive pair of chiseled pecs. ");

				if (removedRows)
				{
					sb.Append("The breasts below vanish entirely. ");
				}
				sb.Append("All too soon, your boobs are gone. Whoa!" + GlobalStrings.NewParagraph());
			}
			else
			{
				if (removedRows || madeLastRowMale)
				{
					sb.Append("As the potent drink runs through your body, a familiar feeling enters your chest, restructuring it until you have a pair of flat, " +
						"manly pectorals once again.");

					if (consumer.genitals.isLactating)
					{
						sb.Append(" With such a manly chest now, you're sure you're no longer lactating.");
					}
				}
				else
				{
					sb.Append("As the potent drink runs through your body, you feel a tightness in your chest, no doubt stopping your lactation " +
						"so your chest is as manly as possible.");
				}
			}
			return sb.ToString();
		}

		private string BecomeRippedFlavorText(Creature consumer, BimBro bimbro)
		{
			StringBuilder sb = new StringBuilder();
			if (bimbro is null)
			{
				sb.Append("Starting at your hands, your muscles begin to contract and release, each time getting tighter, stronger, and more importantly - larger. " +
					"The oddness travels up your arms, thickens your biceps, and broadens your shoulders. Soon, your neck and chest are as built as your arms. " +
					"You give a few experimental flexes as your abs ");
				if (consumer.build.muscleTone >= 70)
				{
					sb.Append("further define themselves");
				}
				else
				{
					sb.Append("become extraordinarily visible");
				}
				sb.Append(". The strange, muscle-building changes flow down your " + consumer.lowerBody.LongDescription() + ", making them just as fit and strong " +
					"as the rest of you. You curl your arm and kiss your massive, flexing bicep. You're awesome!" + GlobalStrings.NewParagraph());

				sb.Append("Whoah, you're fucking ripped and strong, not at all like the puny weakling you were before. Yet, you feel oddly wool-headed. " +
					"Your thoughts seem to be coming slower and slower, like they're plodding through a marsh. You grunt in frustration at the realization. " +
					"Sure, you're a muscle-bound hunk now, but what good is it if you're as dumb as a box of rocks? Your muscles flex in the most beautiful way, " +
					"so you stop and strike a pose, mesmerized by your own appearance. Fuck thinking, that shit's for losers!" + GlobalStrings.NewParagraph());
			}
			else if (bimbro.bimboBody)
			{
				sb.Append(" A tingling in your arm draws your attention just in time to see your biceps and triceps swell with new-found energy, " +
					"skin tightening until thick cords of muscle run across the whole appendage. Your other arm surges forward with identical results. " +
					"To compensate, your shoulders and neck widen to bodybuilder-like proportions while your chest and abs tighten to a firm, statuesque physique. " +
					"Your " + consumer.lowerBody.LongDescription() + " and glutes are the last to go, bulking up to proportions that would make any female martial artist proud. " +
					"You feel like you could kick forever with legs this powerful.");
			}
			else
			{
				sb.Append("The feeling continues through your body, ensuring your body is in peak physical condition.");
			}

			return sb.ToString();
		}

		private string LengthenedCock(Creature consumer, BimBro bimbro, bool lengthened, bool grewBalls)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("As if on cue, the familiar tingling gathers in your groin");

			if (bimbro is null)
			{
				sb.Append(", and you dimly remember you have one muscle left to enlarge. If only you had the intelligence left to realize that " +
					"your penis is not a muscle. In any event, your ");
			}
			else
			{
				sb.Append(", reaching your cock" + (consumer.cocks.Count > 1 ? "s." : ".") + "Your ");
			}

			sb.Append(consumer.cocks[0].LongDescription() + " swells in size, ");

			if (consumer.cocks[0].girth < 2.75)
			{
				sb.Append("thickening and ");
				consumer.cocks[0].SetGirth(2.75);
			}
			sb.Append("lengthening until it's " + (Measurement.UsesMetric ? "roughly 25 centimeters long and 7 centimeters wide"
				: "ten inches long and almost three inches wide"));

			if (bimbro is null)
			{
				sb.Append(". Fuck, you're hung! ");
			}

			//Dick already big enough!BALL CHECK!
			if (consumer.balls.hasBalls)
			{
				sb.Append("Churning audibly, your " + consumer.balls.SackDescription() + " sways, but doesn't show any outward sign of change. " +
					"Oh well, it's probably just like, getting more endurance or something.");
			}
			else
			{
				sb.Append("Two rounded orbs drop down below, filling out a new, fleshy sac above your " + consumer.lowerBody.LongDescription() +
					". Sweet! You can probably cum buckets with balls like these.");
				consumer.balls.GrowBalls(2, 3);
			}
			sb.Append(GlobalStrings.NewParagraph());

			return sb.ToString();
		}

		private string GrewCock(Creature consumer, BimBro bimbro, bool grewBalls)
		{

			if (bimbro is null)
			{
				StringBuilder sb = new StringBuilder();

				if (consumer.wearingLowerGarment)
				{
					sb.Append("You hear the sound of fabric straining, and it takes a moment before your realize it's coming from your " + consumer.lowerGarment.ItemName()
						+ (consumer.wearingArmor ? "Pulling open your " + consumer.armor.ItemName() + ", you" : "You ") + " start to remove your " +
						consumer.lowerGarment.ItemName() + " and gasp as a huge, throbbing cock flops free. ");
				}
				else
				{
					if (consumer.wearingArmor)
					{
						sb.Append("You feel a sudden pressure against the lower sections of your " + consumer.armor.ItemName() + ". Pulling it aside, you");
					}
					else
					{
						sb.Append("A sudden sensation in your groin draws your eyes toward the your exposed nethers. You");
					}
					sb.Append(" gasp in surprise at the huge, throbbing manhood that now lies between your " + consumer.hips.ShortDescription());
				}

				sb.Append(". It rapidly stiffens to " + (Measurement.UsesMetric ? "over 25 centimeters" : "a full ten inches") +
					", and goddamn, it feels fucking good. You should totally find a warm hole to fuck!");

				if (!consumer.balls.hasBalls)
				{
					sb.Append(" Two rounded orbs drop down below, filling out a new, fleshy sac above your " + consumer.lowerBody.LongDescription() + ". Sweet! You can probably cum buckets with balls like these.");
				}

				sb.Append(GlobalStrings.NewParagraph());
				return sb.ToString();
			}
			else if (bimbro.bimboBody)
			{
				string armorText = consumer.LowerBodyArmorTextHelper(
					"loosen the bottoms of your " + consumer.armor.ItemName() + "and slide down your " + consumer.lowerGarment.ItemName(),
					"loosen the bottoms of your " + consumer.armor.ItemName(),
					"loosen your " + consumer.lowerGarment.ItemName(),
					"look down");

					return "The beverage isn't done yet, however, and it makes it perfectly clear with a building pleasure in your groin. " +
						"You can only cry in ecstasy and " + armorText + " just in time for a little penis to spring forth. " +
						"You watch, enthralled, as blood quickly stiffens the shaft to its full length – then keeps on going! Before long, you have a quivering " +
						(Measurement.UsesMetric ? "roughly 25-centimeter" : "10-inch") + " maleness, just ready to stuff into a welcoming box." + GlobalStrings.NewParagraph();
			}
			//should realistically never happen but may occur if a tf item removes your cock and then you drink this before the perk can correct.
			else
			{
				return "As the sensation reaches your groin, the drink ensures your manly endowments are in place, growing a " +
					(Measurement.UsesMetric ? "roughly 25-centimeter" : "10-inch") + "dick" + (grewBalls ? " and a set of balls to match." : ".") + GlobalStrings.NewParagraph();
			}
		}

		private string RemovedAllVaginas(Creature consumer, VaginaCollectionData oldVaginaCollection)
		{
			return "At the same time, your " + oldVaginaCollection.AllVaginasLongDescription(out bool isPlural) + (isPlural ? " burn" : " burns") +
				" hot, nearly feeling on fire. You cuss in a decidedly masculine way for a moment before the pain fades to a dull itch. " +
				"Scratching it, you discover your lady-parts are gone. Only a sensitive patch of skin remains." + GlobalStrings.NewParagraph();
		}

		private string Masculinize(Creature consumer, bool firstTime, FemininityData oldFem)
		{
			if (firstTime)
			{
				return "Lastly, the change hits your face. You can feel your jawbones shifting and sliding around, your skin changing to accommodate your face's new shape. " +
					"Once it's finished, you feel your impeccable square jaw and give a wide, easy-going grin. You look awesome!" + GlobalStrings.NewParagraph();

			}
			else
			{
				return "The changes finally return to your face, ensuring your face matches your fully masculine body. After finding something to reflective, you note your" +
					"square jawline and manly cheekbones. You snap your fingers and point toward your reflection. " + SafelyFormattedString.FormattedText("Still got it!",
					StringFormats.ITALIC) + GlobalStrings.NewParagraph();
			}
		}

		private string Outro(Creature consumer, BimBro bimbro, bool lostFeeder)
		{
			string perks;

			if (bimbro is null)
			{
				perks = SafelyFormattedString.FormattedText("Perks Gained: BroBody, Bro Brains!", StringFormats.BOLD) + Environment.NewLine;

				if (lostFeeder)
				{
					perks += SafelyFormattedString.FormattedText("Perks Lost: Feeder!", StringFormats.BOLD);
				}

				StringBuilder sb = new StringBuilder();
				if (consumer.wearingAnything)
				{
					sb.Append("You finish admiring yourself and adjust your " + (consumer.wearingArmor ? consumer.armor.ItemName() : "clothing") +
						" to better fit your new physique.");
				}
				else
				{
					sb.Append("You take a moment to admire your nude body in all its manly, ripped glory, especially your manly endowment" + (consumer.cocks.Count > 1 ? "s" : "") + ".");
				}
				sb.Append(" Maybe there's some bitches around you can fuck. Hell, as good as you look, you might have other dudes wanting you to fuck them too, no homo." + GlobalStrings.NewParagraph() + perks);
				return sb.ToString();
			}
			else if (bimbro.bimboBody)
			{

				perks = SafelyFormattedString.FormattedText("Perks Updated: Bimbo Body => Futa Form", StringFormats.BOLD);

				if (bimbro.bimbroBrains)
				{
					perks += SafelyFormattedString.FormattedText(", Bro Brains", StringFormats.BOLD);
				}
				perks += SafelyFormattedString.FormattedText("!", StringFormats.BOLD);

				if (lostFeeder)
				{
					perks += Environment.NewLine + SafelyFormattedString.FormattedText("Perks Lost: Feeder!", StringFormats.BOLD);
				}

				return "Finally, you feel the transformation skittering to a halt, leaving you to openly roam your new chiseled and sex-ready body. "
					+ "So what if you can barely form coherent sentences anymore? A body like this does all the talking you need, you figure!" + GlobalStrings.NewParagraph() + perks;

			}
			else
			{
				perks = "";
				if (lostFeeder)
				{
					perks += Environment.NewLine + SafelyFormattedString.FormattedText("Perks Lost: Feeder!", StringFormats.BOLD);
				}

				string dudeSex = bimbro.futaBody
					? "Or studs to get fucked by, you don't really care. Hell, if it lets you fuck it or fucks you, you'd bang just about anything."
					: "Or dudes, if they're into that sort of thing (no homo). Not that you'd blame them, you're a fucking stud!";
				return "As the beer finishes its course, you're able to admire your body once more. Goddamn, you look good. Time to find a few bitches to fuck. " + dudeSex +
					GlobalStrings.NewParagraph() + perks;
			}
		}
	}
}