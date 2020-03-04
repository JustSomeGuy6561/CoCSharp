/**
 * Created by aimozg on 18.01.14.
 */
using System;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Perks;

namespace CoC.Frontend.Items.Consumables
{


	public class BimboLiqueur : StandardConsumable
	{

		public BimboLiqueur() : base()
		{ }

		public override string AbbreviatedName() => "BimboLq";
		public override string ItemName() => "BimboLq";
		public override string ItemDescription(byte count, bool displayCount = false)
		{
			string vialText = count != 1 ? "bottles" : "bottle";

			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";

			return $"{count}potent {vialText} of \"Bimbo Liqueur\"";
		}
		public override string AboutItem() => "This small bottle of liqueur is labelled 'Bimbo Liqueur'. There's a HUGE warning label about the effects being strong and usually permanent, so you should handle this with care.";
		protected override int monetaryValue => 1000;

		public override bool countsAsLiquid => true;
		public override bool countsAsCum => false;

		public override byte sateHungerAmount => 0;

		public override bool Equals(CapacityItem other)
		{
			return other is BimboLiqueur;
		}

		public override bool CanUse(Creature target, bool currentlyInCombat, out string whyNot)
		{
			if (!target.HasPerk<BimBro>() || !target.GetPerkData<BimBro>().futaBody)
			{
				whyNot = null;
				return true;
			}

			whyNot = "Ugh. This stuff is so, like... last year. Maybe you can find someone else to feed it to?" + GlobalStrings.NewParagraph();
			return false;
		}

		protected override string OnConsumeAttempt(Creature consumer, out bool consumeItem, out bool isBadEnd)
		{
			isBadEnd = false;

			BimBro bimbroPerk = consumer.GetPerkData<BimBro>();

			StringBuilder sb = new StringBuilder();
			if (bimbroPerk != null && bimbroPerk.broBody)
			{
				sb.Append("You wince as the stuff hits your stomach, already feeling the insidious effects beginning to take hold. A lengthy belch escapes your lips " +
					"as your stomach gurgles, and you giggle abashedly to yourself.");
				if (consumer.build.heightInInches < 77)
				{
					sb.Append(" ...Did the ground just get farther away? You glance down and realize, you're growing! Like a sped-up flower sprout, you keep on getting " +
						"taller until finally stopping around... six and a half feet, you assume. Huh. You didn't expect that to happen!");
					consumer.build.SetHeight(77);
				}
				CupSize biggestSize = consumer.genitals.BiggestCupSize();
				if (biggestSize < CupSize.E)
				{
					if (biggestSize == CupSize.FLAT)
					{
						sb.Append(" Tingling, your chest begins to itch, then swell into a pair of rounded orbs. ");
					}
					else
					{
						sb.Append(" You feel a tingling inside your breasts. ");
					}

					sb.Append("They quiver ominously, and you can't help but squeeze your tits together to further appreciate the boobquake as another tremor runs through them. " +
						"Unexpectedly, the shaking pushes your hands further apart as your tits balloon against each other, growing rapidly against your now-sunken fingers. " +
						"The quakes continue until calming at around an E-cup.");
					consumer.breasts[0].SetCupSize(CupSize.E);
				}
				//(If vagina = 2tight:
				if (!consumer.hasVagina)
				{
					string armorText = consumer.LowerBodyArmorShort(false) ?? "volumous breasts";
					sb.Append(" Before you can even take a breath, an extremely peculiar sensation emanates from your crotch. You can't see through your " + armorText +
						", but you can certainly feel the vagina splitting " + (consumer.balls.hasBalls ? "from behind your testicles" : "your groin") +
						". Luckily, the cunt-forming doesn't yield any discomfort - on the contrary, you feel yourself falling farther into your chemically-dulled, " +
						"libido-fueled rut.");
					if (consumer.hips.size < 12 || consumer.butt.size < 12)
					{
						sb.Append(" As if realizing the necessity of womanly proportions to attract the hard cocks your body now craves, your waist pinches slightly inward " +
							"and your hips and butt swell. You can't help but run a hand across your newly-feminized pelvis, admiring it.");
					}

					consumer.AddVagina();

					if (consumer.hips.size < 12)
					{
						consumer.hips.SetHipSize(12);
					}

					if (consumer.butt.size < 12)
					{
						consumer.butt.SetButtSize(12);
					}
				}
				sb.Append(GlobalStrings.NewParagraph());
				sb.Append("A wave of numbness rolls through your features, alerting you that another change is happening. You reach up to your feel your jaw narrowing, " +
					"becoming more... feminine? Heavy, filling lips purse in disappointment as your face takes on a very feminine cast. You're probably pretty hot now!"
					+ GlobalStrings.NewParagraph());
				if (consumer.femininity < 80)
				{
					consumer.femininity.SetFemininity(80);
				}

				sb.Append("Your surging, absurdly potent libido surges through your body, reminding you that you need to fuck. Not just bitches, but guys too. " +
					"Hard cocks, wet pussies, hell, you don't care. They can have both or a dozen of either. You just want to get laid and bone something, " +
					"hopefully at the same time!");

				sb.Append(GlobalStrings.NewParagraph() + "<b>(Perks Updated: Bro Body => Futa Form");
				if (bimbroPerk.bimbroBrains)
				{
					sb.Append(", Bro Brains => Futa Faculties");
				}
				sb.Append(")" + Environment.NewLine);

				if (!bimbroPerk.bimbroBrains)
				{
					sb.Append("(Perks Gained: Futa Faculties)");
				}
				sb.Append("</b>" + Environment.NewLine);

				if (consumer.intelligence > 35)
				{
					consumer.SetIntelligence(35);
				}
				if (consumer.libido < 50)
				{
					consumer.SetLibido(50);
				}
			}
			else
			{
				sb.Append("You pop the cork from the flask and are immediately assaulted by a cloying, spiced scent that paints visions of a slutty slave-girl's " +
					"slightly-spread folds. Wow, this is some potent stuff! Well, you knew what you were getting into when you found this bottle! You open wide " +
					"and guzzle it down, feeling the fire of alcohol burning a path to your belly. The burning quickly fades to a pleasant warmth that makes you " +
					"light-headed and giggly." + GlobalStrings.NewParagraph());

				if (consumer.hair.hairColor != HairFurColors.PLATINUM_BLONDE || consumer.hair.isBald || !consumer.hair.type.growsOverTime || consumer.hair.type.isFixedLength)
				{

					sb.Append("The first change that you notice is to your " + consumer.hair.LongDescription()
					+ ". It starts with a tingling in your scalp and intensifies ");

					if (consumer.hair.isBald)
					{
						sb.Append("as hair grows along your previously bald dome, rapidly thickening and lengthening.");
					}
					else if (!consumer.hair.type.growsOverTime || consumer.hair.type.isFixedLength)
					{
						sb.Append("as it begins changing, reverting to something more natural. As it does, it rapidly gets longer and heavier until it's "
							+ (Measurement.UsesMetric ? "nearly a meter" : "several feet") + " in length.");
					}
					else if (consumer.hair.length < 36)
					{
						sb.Append("as you feel the weight of your hair growing heavier and longer.");
					}
					else
					{
						sb.Append("as your hair grows thicker and heavier.");
					}

					sb.Append(" You grab a lock of the silken strands and watch open-mouthed while streaks so blonde they're almost white flow down the "
						+ consumer.hair.hairColor.AsString() + " hair. It goes faster and faster until your hair has changed into perfectly bimbo-blonde, flowing locks."
						+ GlobalStrings.NewParagraph());
				}

				sb.Append("Moaning lewdly, you begin to sway your hips from side to side, putting on a show for anyone who might manage to see you.  You just feel so... sexy. " +
					"Too sexy to hide it. Your body aches to show itself and feel the gaze of someone, anyone upon it. Mmmm, it makes you so wet! ");
				if (!consumer.hasVagina)
				{
					consumer.AddVagina(0.25, VaginalLooseness.NORMAL, VaginalWetness.SLICK);

					if (consumer.isQuadruped)
					{
						sb.Append("Wait!? Wet? You wish you could touch yourself between the " + consumer.lowerBody.LongDescription() + ", " +
							"but you can tell from the fluid running down your hind-legs just how soaked your new vagina is.");
					}
					else
					{
						sb.Append("Wait!? Wet? You touch yourself between the " + consumer.lowerBody.LongDescription() +
							" and groan when your fingers sink into a sloppy, wet cunt.");
					}
				}
				else
				{
					if (consumer.isQuadruped)
					{
						sb.Append("You wish you could sink your fingers into " + (consumer.vaginas.Count > 1 ? "either of your sloppy, wet cunts" : "your sloppy, wet cunt") +
							" , but as a centaur, you can't quite reach.");
						if (consumer.vaginas[0].wetness < VaginalWetness.SLICK)
						{
							consumer.vaginas[0].SetVaginalWetness(VaginalWetness.SLICK);
						}
					}
					else if (consumer.vaginas.Count > 1)
					{
						sb.Append("You alternate slipping your fingers into both of your cunts, moaning with satisfaction at how sloppy and wet they are");

						if (consumer.genitals.SmallestVaginalWetness() < VaginalWetness.SLICK)
						{
							sb.Append(" now");
						}
						sb.Append(".");
					}
					else
					{
						sb.Append("You sink your fingers into your ");
						if (consumer.vaginas[0].wetness < VaginalWetness.SLICK)
						{
							sb.Append("now ");
						}
						sb.Append("sloppy, wet cunt with a groan of satisfaction.");
					}
				}
				//prevent this from messing up futa form.
				if (bimbroPerk is null || bimbroPerk.bimboBody)
				{
					if (consumer.balls.hasBalls)
					{
						sb.Append(GlobalStrings.NewParagraph() + "There's a light pinch against your [sack] that makes you gasp in surprise, followed by an exquisite tightness that makes your " +
							consumer.genitals.AllVaginasNoun() + " drool. Looking down, <b>you see your balls slowly receding into your body, leaving nothing behind but your puffy mons.</b>");

						consumer.balls.RemoveAllBalls();
					}
					if (consumer.hasCock)
					{
						sb.Append(GlobalStrings.NewParagraph() + consumer.genitals.EachCockOrCocksShort(Conjugate.YOU, out bool isPlural) + (isPlural ? " seem" : " seems")
							+ " to be responding to the liqueur in " + (isPlural ? "their" : "its") + " own way. Clenching and relaxing obscenely, " +
							"your genitals begin to drizzle cum onto the ground in front of you, throwing you into paroxysms of bliss. The flow of cum is steady but weak, " +
							"and each droplet that leaves you lets " + consumer.genitals.EachCockOrCocksShort() + " go more flaccid. Even once you're soft and little, " +
							"it doesn't stop. You cum your way down to nothing, a tiny droplet heralding your new, girlish groin. <b>You no longer have " +
							(isPlural ? "any penises" : "a penis") + "!</b>");
					}
				}
				sb.Append(" Somehow, you feel like you could seduce anyone right now!" + GlobalStrings.NewParagraph());

				sb.Append("Another bubbly giggle bursts from your lips, which you then lick hungrily. You, like, totally want some dick to suck! " +
					"Wow, that came out of left field. You shake your head and try to clear the unexpected, like, words from your head but it's getting kind of hard. " +
					"Omigosh, you feel kind of like a dumb bimbo after, like, drinking that weird booze. Oh, well, it doesn't matter anyhow – you can, like, " +
					"still stop the demons and stuff. You'll just have to show off your sexy bod until they're offering to serve you." + GlobalStrings.NewParagraph());

				sb.Append("You sigh and run one hand over your " + consumer.breasts[0].LongNippleDescription());

				consumer.breasts[0].GrowBreasts((byte)(5 + Utils.Rand(5)));

				if (consumer.breasts[0].cupSize < CupSize.EE_BIG)
				{
					sb.Append(", surprised at how large and rounded your expanding breasts have become while fresh tit-flesh continues to spill out around your needy fingers. " +
						"They feel so supple and soft, but when you let them go, they still sit fairly high and firm on your chest. The newer, more generous, "
						+ consumer.breasts[0].cupSize.AsText() + " cleavage has you moaning with how sensitive it is, pinching a nipple with one hand ");
				}
				else
				{
					sb.Append(", admiring how sensitive they're getting. The big breasts start getting bigger and bigger, soft chest-flesh practically oozing out " +
						"between your fingers as the squishy mammaries sprout like weeds, expanding well beyond any hand's ability to contain them. The supple, " +
						consumer.breasts[0].cupSize.AsText() + " boobs still manage to sit high on your chest, almost gravity defying in their ability to generate cleavage. " +
						"You pinch a nipple with one hand ");
				}

				consumer.IncreaseSensitivity(20);
				sb.Append("while the other toys with the juicy entrance of your folds. Mmmm, it, like, feels too good not to touch yourself, and after being worried " +
					"about getting all dumb and stuff, you need to relax. Thinking is hard, but sex is so easy and, like, natural! You lean back and start grunting " +
					"as you plunge four fingers inside yourself, plowing " + consumer.genitals.OneVaginaOrVaginasShort() + " like no tomorrow. " +
					"By now, " + (consumer.vaginas.Count > 1 ? "its " : "your ") + consumer.vaginas[0].clit.LongDescription() + " is throbbing, " +
					"and you give it an experimental ");
				if (consumer.clits[0].length >= 3)
				{
					sb.Append("jerk ");
				}
				else
				{
					sb.Append("caress ");
				}

				sb.Append("that makes your " + consumer.lowerBody.LongDescription() + " give out as you cum, splattering female fluids as you convulse " +
					"nervelessly on the ground." + GlobalStrings.NewParagraph());

				sb.Append("Though the orgasm is intense, you recover a few moments later feeling refreshed, but still hot and horny. Maybe you could find a partner to fuck? " +
					"After all, sex is, like, better with a partner or two. Or that number after two. You brush a lengthy, platinum blonde strand of hair out of your eyes " +
					"and lick your lips - you're ready to have some fun!" + GlobalStrings.NewParagraph());

				if (consumer.hips.size < 12 || consumer.butt.size < 12)
				{
					sb.Append("As you start to walk off in search of a sexual partner, you feel your center of balance shifting.");
					if (consumer.hips.size < 12 && consumer.butt.size < 12)
					{
						sb.Append(" Your ass and hips inflate suddenly, forcing you to adopt a slow, swaying gait. You find that rolling your hips back and forth " +
							"comes naturally to you. You make sure to squeeze your butt-muscles and make your curvy tush jiggle as you go.");
						consumer.butt.SetButtSize(12);
						consumer.hips.SetHipSize(12);
					}
					else if (consumer.hips.size < 12)
					{
						sb.Append(" Your hips widen suddenly, forcing you to adopt a slow, swaying gait. You find that rolling yours hips back and forth " +
							"comes naturally to you, and your big, obscene ass seems to jiggle all on its own with every step you take.");
						consumer.hips.SetHipSize(12);
					}
					else
					{
						sb.Append(" Your " + consumer.build.ButtLongDescription() + " swells dramatically, the puffy cheeks swelling with newfound weight " +
							"that jiggles along with each step. Clenching your glutes to make the posh cheeks jiggle a little more enticingly becomes second nature " +
							"to you in a few seconds.");
						consumer.butt.SetButtSize(12);
					}
					sb.Append(GlobalStrings.NewParagraph());
				}
				if (consumer.build.muscleTone > 0)
				{
					sb.Append("Like, weirdest of all, your muscles seem to be vanishing! Before your eyes, all muscle tone vanishes, leaving your body soft and gently curvy. " +
						"You poke yourself and giggle! Everyone's totally going to want to, like, rub up against you at every opportunity. " +
						"Your thighs are so soft you bet you could squeeze a pair of dicks to orgasm without even touching your moist cunny.");
					sb.Append(" It does get a bit harder to carry yourself around with your diminished strength, but that's, like, what big strong hunks are for anyways! " +
						"You can just flirt until one of them volunteers to help out or something! Besides, you don't need to be strong to jerk off cocks " +
						"or finger slutty pussies!");
					sb.Append(GlobalStrings.NewParagraph());
				}
				if (bimbroPerk == null)
				{
					sb.Append("<b>Perks gained : Bimbo Body, Bimbo Brains</b>" + Environment.NewLine);
				}
				else if (bimbroPerk.bimboBody && !bimbroPerk.bimbroBrains)
				{
					sb.Append("<b>(Bimbo Body - Perk Gained!)</b>" + Environment.NewLine);
				}
				else if (!bimbroPerk.bimboBody && bimbroPerk.bimbroBrains)
				{
					sb.Append("<b>(Bimbo Brains - Perk Gained!)</b>" + Environment.NewLine);//int to 20. max int 50)
				}

				if (consumer.intelligence > 21)
				{
					consumer.SetIntelligence((byte)Math.Floor(Math.Max(consumer.intelligenceTrue / 5.0, 21)));
				}
				consumer.HaveGenericVaginalOrgasm(0, false, true);
				consumer.DeltaCreatureStats(inte: -1, lib: 4, sens: 25);
				//FULL ON BITCHFACE
				sb.Append(consumer.ModifyFemininity(100, 100));
				//Body
				//Tease/Seduce Boost
				//*boosts min lust and lust resistance)
				//*Tit size
				//Brain
				//Max int - 50
			}

			consumeItem = true;
			return sb.ToString();
		}
	}
}
