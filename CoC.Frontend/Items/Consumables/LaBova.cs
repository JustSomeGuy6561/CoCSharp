using System;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.Frontend.Transformations;

namespace CoC.Frontend.Items.Consumables
{


	/**
	 * @since March 31, 2018
	 * @author Stadler76
	 */
	public class LaBova : StandardConsumable
	{
		private readonly TransformationType transformation;

		private bool isPurified => transformation == TransformationType.PURIFIED;
		private bool isEnhanced => transformation == TransformationType.ENHANCED;

		public LaBova(TransformationType transformationType)
		{
			transformation = transformationType;
		}

		public override string AbbreviatedName()
		{
			if (isEnhanced)
			{
				return "ProBova";
			}

			else if (isPurified)
			{
				return "P.LBova";
			}
			else
			{
				return "LaBova";
			}
		}

		public override string ItemName()
		{
			if (isEnhanced)
			{
				return "Pro Bova";
			}

			else if (isPurified)
			{
				return "Purified LaBova";
			}
			else
			{
				return "LaBova";
			}
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";
			string bottleText = (count != 1 ? "bottles" : "bottle");
			string itemText = "\"" + ItemName() + "\"";

			return $"{countText}{bottleText} containing a misty fluid labeled {itemText}";
		}

		public override string AboutItem()
		{
			string end = "";
			if (isEnhanced)
			{
				end = " This cloudy potion has been enhanced by the alchemist Lumi to imbue its drinker with cow-like attributes.";
			}

			else if (isPurified)
			{

				end = " It has been purified by Rathazul.";
			}

			return "A bottle containing a misty fluid with a grainy texture; it has a long neck and a ball-like base."
				+ " The label has a stylized picture of a well-endowed cow-girl nursing two guys while they jerk themselves off." + end;

		}

		protected override int monetaryValue => isPurified ? 20 : DEFAULT_VALUE;

		public override bool CanUse(Creature target, bool currentlyInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		public override bool countsAsLiquid => true;
		public override bool countsAsCum => false;

		public override bool Equals(CapacityItem other)
		{
			return other is LaBova laBova && laBova.transformation == transformation;
		}

		protected override bool OnConsumeAttempt(Creature consumer, out string resultsOfUse, out bool isBadEnd)
		{
			var tf = new CowTFs(this, transformation);
			resultsOfUse = tf.DoTransformation(consumer, out isBadEnd);
			return true;
		}

		protected override bool OnCombatConsumeAttempt(CombatCreature consumer, CombatCreature opponent, out string resultsOfUse, out bool isCombatLoss, out bool isBadEnd)
		{
			var tf = new CowTFs(this, transformation);
			resultsOfUse = tf.DoTransformationFromCombat(consumer, out isCombatLoss, out isBadEnd);
			return true;
		}

		public override byte sateHungerAmount => 20;

		private class CowTFs : CowTransformations
		{
			private readonly LaBova source;

			public CowTFs(LaBova source, TransformationType transformationType) : base(transformationType)
			{
				this.source = source ?? throw new ArgumentNullException(nameof(source));
			}

			protected override string InitialTransformationText(Creature target)
			{
				return "You drink the " + source.ItemName() + ". The drink has an odd texture, but is very sweet. It has a slight aftertaste of milk.";
			}

			protected override string StrengthUpText(double delta)
			{
				if (Utils.Rand(2) == 0)
				{
					return GlobalStrings.NewParagraph() + "There is a slight pain as you feel your muscles shift somewhat. " +
						"Their appearance does not change much, but you feel much stronger.";
				}
				else
				{
					return GlobalStrings.NewParagraph() + "You feel your muscles tighten and clench as they become slightly more pronounced.";
				}
			}

			protected override string ToughnessUpText(double delta)
			{
				if (Utils.Rand(2) == 0)
				{
					return GlobalStrings.NewParagraph() + "You feel your insides toughening up; it feels like you could stand up to almost any blow.";
				}
				else
				{
					return GlobalStrings.NewParagraph() + "Your bones and joints feel sore for a moment, and before long you realize they've gotten more durable.";
				}
			}

			protected override string SpeedDownText(double decrease)
			{
				return GlobalStrings.NewParagraph() + "The body mass you've gained is making your movements more sluggish.";
			}

			//removed last/only cock, obtained a vagina instead.
			protected override string MadeFemale(Creature target, CockData preChange, BallsData oldBalls)
			{
				bool removedBalls = oldBalls.hasBalls;

				StringBuilder sb = new StringBuilder();

				sb.Append("Your " + preChange.LongDescription() + " suddenly starts tingling. It's a familiar feeling, similar to an orgasm. " +
					"However, this one seems to start from the top down, instead of gushing up from your loins. You spend a few seconds frozen to the odd sensation, " +
					"when it suddenly feels as though your own body starts sucking on the base of your shaft. Almost instantly, your cock sinks " +
					"into your crotch with a wet slurp. The tip gets stuck on the front of your body on the way down, but your glans soon loses all volume to turn " +
					"into a shiny new clit.");

				if (target.balls.hasBalls)
				{
					sb.Append(" At the same time, your " + oldBalls.ShortDescription() + " fall victim to the same sensation; eagerly swallowed whole by your crotch.");
				}

				sb.Append(" Curious, you touch around down there, to find you don't have any exterior organs left. All of it got swallowed into the gash you " +
					"now have running between two fleshy folds, like sensitive lips. It suddenly occurs to you; <b>you now have a vagina!</b>");

				return sb.ToString();
			}

			protected override string GrewFirstBreastRow(Creature target, BreastData oldBreasts)
			{
				if (Utils.Rand(2) == 0)
				{
					return GlobalStrings.NewParagraph() + "Your " + oldBreasts.LongDescription() + " tingle for a moment before becoming larger.";
				}
				else
				{
					return GlobalStrings.NewParagraph() + "You feel a little weight added to your chest as your " + oldBreasts.LongDescription() +
						" seem to inflate and settle in a larger size.";
				}
			}

			protected override string RemovedFeatheryHairText(Creature target, HairData oldHair)
			{
				return RemovedFeatheryHairTextGeneric(target, true);
			}

			protected override string StartedLactatingText(Creature target, BreastData preLactation)
			{
				return GlobalStrings.NewParagraph() + "You gasp as your " + preLactation.LongDescription() + " feel like they are filling up with something. " +
					"Within moments, a drop of milk leaks from your " + preLactation.LongDescription() + "; <b> you are now lactating</b>.";
			}

			protected override string GrantQuadNippleText(Creature target, BreastCollectionData oldBreasts)
			{
				string armorText = target.UpperBodyArmorShort();

				if (armorText is null)
				{
					armorText = "You pause to see what's the cause";
				}
				else
				{
					armorText = "You pull back your " + armorText;
				}

				StringBuilder sb = new StringBuilder();

				sb.Append(GlobalStrings.NewParagraph() + "Your " + oldBreasts[0].LongNippleDescription() + "s tingle and itch. " + armorText + " and watch in shock " +
					"as they split into four distinct nipples! <b>You now have four nipples on each side of your chest!</b>");

				if (target.breasts.Count >= 2)
				{
					sb.Append("A moment later your second row of " + oldBreasts[1].LongDescription()+ " does the same. <b>You have sixteen nipples now!</b>");
				}
				if (target.breasts.Count >= 3)
				{
					string rowText = target.breasts.Count > 3 ? "rows quickly follow" : "row quickly follows";
					sb.Append("Your remaining " + rowText + "suit, granting you a wonderland of nipples.");
				}

				sb.Append(" <b>You have a total of " + Utils.NumberAsText(target.genitals.nippleCount) + " nipples.</b>");

				return sb.ToString();
			}

			protected override string BoostedLactationText(Creature target, BreastCollectionData oldBreasts, double nippleLengthDelta)
			{
				var delta = target.genitals.lactationProductionModifier - oldBreasts.lactationProductionModifier;

				if (delta == 0) return "";

				StringBuilder sb = new StringBuilder();


				if (delta < 0)
				{
					if (Utils.Rand(2) == 0)
					{
						sb.Append(GlobalStrings.NewParagraph() + "Your breasts suddenly feel less full, it seems you aren't lactating at quite the level you were.");
					}
					else
					{
						sb.Append(GlobalStrings.NewParagraph() + "The insides of your breasts suddenly feel bloated. There is a spray of milk from them, and they settle closer to a more natural level of lactation.");
					}
				}
				else if (delta < 1.5)
				{

					if (Utils.Rand(2) == 0)
					{
						sb.Append(GlobalStrings.NewParagraph() + "A wave of pleasure passes through your chest as your " + oldBreasts.AllBreastsLongDescription()
							+ " start producing more milk.");
					}
					else
					{
						sb.Append(GlobalStrings.NewParagraph() + "Something shifts inside your " + oldBreasts.AllBreastsLongDescription()
							+ " and they feel fuller and riper. You know that you've started producing more milk.");
					}
				}
				else
				{
					if (Utils.Rand(2) == 0)
					{
						sb.Append(GlobalStrings.NewParagraph() + "A wave of pleasure passes through your chest as your " + oldBreasts.AllBreastsLongDescription()
							+ " start leaking milk from a massive jump in production.");
					}
					else
					{
						sb.Append(GlobalStrings.NewParagraph() + "Something shifts inside your " + oldBreasts.AllBreastsLongDescription()
							+ " and they feel MUCH fuller and riper. You know that you've started producing much more milk.");
					}
				}
				if (nippleLengthDelta > 0)
				{
					sb.Append(" Your " + target.breasts[0].LongNippleDescription() + " swell up, growing larger to accommodate your increased milk flow.");
				}

				return sb.ToString();
			}

			protected override string GainedFeederPerk(Creature target)
			{
				return GlobalStrings.NewParagraph() + "You start to feel a strange desire to give your milk to other creatures. " +
					"For some reason, you know it will be very satisfying." + GlobalStrings.NewParagraph() + "<b>(You have gained the 'Feeder' perk!)</b>";

			}

			protected override string LoosenedTwatText(Creature target, VaginaData preLoosened)
			{
				return (GlobalStrings.NewParagraph() + "You feel a relaxing sensation in your groin. On further inspection you discover your "
						+ preLoosened.LongDescription() + " has somehow relaxed, permanently loosening.");
			}

			protected override string GrowTaller(Creature target, byte delta)
			{
				//Flavor texts. Flavored like 1950's cigarettes. Yum.
				if (delta < 5)
				{
					return GlobalStrings.NewParagraph() + "You shift uncomfortably as you realize you feel off balance. Gazing down, you realize you have grown SLIGHTLY taller.";
				}

				else if (delta < 7)
				{
					return GlobalStrings.NewParagraph() + "You feel dizzy and slightly off, but quickly realize it's due to a sudden increase in height.";
				}

				else //if (delta == 7)
				{
					return GlobalStrings.NewParagraph() + "Staggering forwards, you clutch at your head dizzily. You spend a moment getting your balance, and stand up, feeling noticeably taller.";
				}
			}

			protected override string ChandedFurToSpots(Creature target, BodyData oldData)
			{
				return GlobalStrings.NewParagraph() + "A ripple spreads through your fur as some patches darken and others lighten. " +
					"After a few moments you're left with a black and white spotted pattern that goes the whole way up to the hair on your head! <b>You've got cow fur!</b>";
			}

			protected override string MadeHornsBigger(Creature target, HornData oldHorns)
			{
				return GlobalStrings.NewParagraph() + "Your small horns get a bit bigger, stopping as medium sized nubs.";
			}

			protected override string WidenedHipsText(Creature target, HipData oldHips)
			{
				return GlobalStrings.NewParagraph() + "You stumble as you feel the bones in your hips grinding, expanding your hips noticeably.";
			}

			protected override string GrewButtText(Creature target, ButtData oldButt)
			{
				return GlobalStrings.NewParagraph() + "A sensation of being unbalanced makes it difficult to walk. " +
					"You pause, paying careful attention to your new center of gravity before understanding dawns on you - your ass has grown!";
			}
		}
	}
}
