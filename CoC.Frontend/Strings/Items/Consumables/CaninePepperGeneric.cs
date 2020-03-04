using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.Frontend.Transformations;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Items.Consumables
{
	partial class CaninePepperGeneric
	{
		private static string InitialTransformationText(CanineModifiers modifiers, bool crit)
		{
			//standard. simple, to the point.
			if (modifiers == CanineModifiers.STANDARD)
			{
				if (crit)
				{
					return "The pepper tastes particularly potent, searingly hot and spicy.";
				}
				else
				{
					return "The pepper is strangely spicy but very tasty.";
				}
			}

			//compound or non-standard type. start with appearance.
			StringBuilder sb = new StringBuilder();

			//by default, we're not expecting compound types, but we do support them
			//so, we check to see if each modifier flags is set, but we exit early whenever
			//we only have that modifier flag.
			//I'm fully expecting the compound nonsense to never be used, but whatever, it wasn't that hard to write.
			bool plural = modifiers.HasFlag(CanineModifiers.DOUBLE);

			if (modifiers.HasFlag(CanineModifiers.DOUBLE))
			{
				if (modifiers == CanineModifiers.DOUBLE)
				{
					return "The double-pepper is strange, looking like it was formed when two peppers grew together near their bases. Not surprisingly, it takes twice as long to finish, and you could swear it was twice as delicious as well.";
				}
				else
				{
					sb.Append("The double-pepper is strange, looking like it was formed when two peppers grew together near their bases. ");
				}
			}
			if (modifiers.HasFlag(CanineModifiers.LARGE))
			{
				if (modifiers == CanineModifiers.LARGE)
				{
					return "The pepper is so large and thick that you have to eat it in several large bites. It is not as spicy as the normal ones, but is delicious and flavorful.";
				}
				else
				{
					sb.Append((plural ? "Each section " : "This pepper ") + "is so large and thick, you have no doubt it'll take several bites to eat. ");
					if (plural) sb.Append(" them");
					sb.Append(". ");
				}
			}
			if (modifiers.HasFlag(CanineModifiers.BLACK))
			{
				if (modifiers == CanineModifiers.BLACK)
				{
					return "This pepper appears like a standard one, but with a distinct black color. It tastes sweet, but has a bit of a tangy aftertaste.";
				}
				else
				{
					sb.Append("Its color, a distinct shade of black, " + (sb.Length != 0 ? "also " : "") + "sets it apart from other peppers, and you have little doubt that affects the flavor. ");
				}
			}
			if (modifiers.HasFlag(CanineModifiers.BULBY) || modifiers.HasFlag(CanineModifiers.KNOTTY))
			{
				if (modifiers == CanineModifiers.BULBY)
				{
					return "You eat the pepper, even the two orb-like growths that have grown out from the base. It's delicious!";
				}
				else if (modifiers == CanineModifiers.KNOTTY)
				{
					return "The pepper is a bit tough to eat due to the swollen bulge near the base, but you manage to cram it down and munch on it. It's extra spicy!";
				}
				else
				{
					sb.Append("Its base has a distinct shape, bulging in a manner unlike what you're used to. It'll probably further complicate eating the thing, but you're sure you'll manage. ");
				}
			}

			//compound shape generic text.
			sb.Append("Despite the strage shape, you eventually manage to eat the entire thing. ");

			//now for the flavor. We only use one modifier for this, whichever procs first. Any modifiers with distinct
			//flavors are given first priority.
			if (modifiers.HasFlag(CanineModifiers.BLACK))
			{
				sb.Append("It initially tasted sweet, but your definitely getting a tangy after-taste. ");
			}
			else if (modifiers.HasFlag(CanineModifiers.KNOTTY))
			{
				sb.Append("It is definitely spicier than most other peppers, causing you to blink a few times. ");
			}
			else if (modifiers.HasFlag(CanineModifiers.LARGE))
			{
				sb.Append("It's far more mellow than your standard pepper, but it makes up for that in flavor");
			}
			else
			{
				sb.Append("It's quite delicious! ");
			}
			return sb.ToString();
		}

		private static string BadEndText(Creature target)
		{
			StringBuilder sb = new StringBuilder(Environment.NewLine + Environment.NewLine);
			if (Utils.RandBool())
			{
				sb.Append("As you swallow the pepper, you note that the spicy hotness on your tongue seems to be spreading. Your entire body seems to tingle and burn, " +
					"making you feel far warmer than normal, feverish even. Unable to stand it any longer you tear away your clothes, hoping to cool down a little. " +
					"Sadly, this does nothing to aid you with your problem. On the bright side, the sudden feeling of vertigo you've developed is more than enough " +
					"to take your mind off your temperature issues. You fall forward onto your hands and knees, well not really hands and knees to be honest - more like paws and knees. " +
					"That can't be good, you think for a moment, before the sensation of your bones shifting into a quadrupedal configuration robs you of your concentration. " +
					"After that, it is only a short time before your form is remade completely into that of a large dog, or perhaps a wolf. The distinction would mean little to you now, " +
					"even if you were capable of comprehending it. ");

#pragma warning disable CS0162 // Unreachable code detected
#warning Implement me when marble milk perk or status effect or whatever i make it is implemented.
				if (false)
				//if (target.perks.HasPerk<MarbleMilk>())
				{
					sb.Append("All you know is that there is a scent on the wind, it is time to hunt, and at the end of the day you need to come home for your milk.");
				}
#pragma warning restore CS0162 // Unreachable code detected
				else
				{
					sb.Append("All you know is that there is a scent on the wind, and it is time to hunt.");
				}
			}
			else
			{
				sb.Append("You devour the sweet pepper, carefully licking your fingers for all the succulent juices of the fruit, and are about to go on your way when suddenly " +
					"a tightness begins to build in your chest and stomach, horrid cramps working their way first through your chest, then slowly flowing out to your extremities, " +
					"the feeling soon joined by horrible, blood-curdling cracks as your bones begin to reform, twisting and shifting, your mind exploding with pain. " +
					"You fall to the ground, reaching one hand forward. No... A paw, you realize in horror, as you try to push yourself back up. You watch in horror, " +
					"looking down your foreleg as thicker fur erupts from your skin, a " + target.ActiveHairOrFurColor().AsString() + " coat slowly creeping from your bare flesh " +
					"to cover your body. Suddenly, you feel yourself slipping away, as if into a dream, your mind warping and twisting, your body finally settling into its new form. " +
					"With one last crack of bone you let out a yelp, kicking free of the cloth that binds you, wresting yourself from its grasp and fleeing into the now setting sun, " +
					"eager to find prey to dine on tonight.");
			}
			return sb.ToString();
		}

		private static string DoggoWarningText(Creature target, bool wasPreviouslyWarned)
		{
			string endText = wasPreviouslyWarned ? ". At this point, you almost feel like you're pushing your luck" : "";
			return Environment.NewLine + Environment.NewLine + SafelyFormattedString.FormattedText("Eating the pepper, you realize how dog-like you've become, " +
				"and you wonder what else the peppers could change" + endText + "...", StringFormats.BOLD);
		}
		private static string StatChangeText(Creature target, CanineModifiers modifiers, double strengthIncrease, double speedIncrease, double intelligenceDecrease)
		{
			StringBuilder sb = new StringBuilder();
			if (modifiers.HasFlag(CanineModifiers.BLACK))
			{
				sb.Append(GlobalStrings.NewParagraph() + "You feel yourself relaxing as gentle warmth spreads through your body. Honestly you don't think you'd mind running into a demon or monster right now, they'd make for good entertainment.");
				if (target.relativeCorruption < 50)
				{
					sb.Append(" You shake your head, blushing hotly. Where did that thought come from?");
				}
			}

			if (strengthIncrease > 0)
			{
				sb.Append(GlobalStrings.NewParagraph());
				if (strengthIncrease > 1)
				{
					sb.Append("Your muscles ripple and grow, bulging outwards.");
				}
				else
				{
					sb.Append("Your muscles feel more toned.");
				}
			}

			if (speedIncrease > 0)
			{
				sb.Append(GlobalStrings.NewParagraph());
				if (speedIncrease > 1)
				{
					sb.Append("You find your muscles responding quicker, faster, and you feel an odd desire to go for a walk.");
				}
				else
				{
					sb.Append("You feel quicker.");
				}
			}
			if (intelligenceDecrease > 0)
			{
				string howDumb = intelligenceDecrease > 1 ? "MUCH " : "";
				sb.Append(GlobalStrings.NewParagraph() + "You feel " + howDumb + "dumber.");
			}
			return sb.ToString();
		}

		private static string EnlargedSmallestKnotText(Creature target, int cockIndex, double knotMultiplierDelta, bool hasMoreThanOneDogCock)
		{
			var smallestKnot = target.cocks[cockIndex];
			if (knotMultiplierDelta < .06)
			{
				return "Your " + smallestKnot.ShortDescription() + " feels unusually tight in your sheath as your knot grows.";
			}
			else if (knotMultiplierDelta <= .12)
			{
				return "Your " + smallestKnot.ShortDescription() + " pops free of your sheath, thickening nicely into a bigger knot.";
			}
			else
			{
				return "Your " + smallestKnot.ShortDescription() + " surges free of your sheath, swelling thicker with each passing second. Your knot bulges out at the base, growing far beyond normal.";
			}
		}
		private static string GrewTwoDogCocksHadNone(Creature target)
		{
			return GlobalStrings.NewParagraph() + "A painful lump forms on your groin, nearly doubling you over as it presses against your " + target.armor.ItemName() + ". " +
				"You rip open your gear and watch, horrified as the discolored skin splits apart, revealing a pair of red-tipped points. A feeling of relief, " +
				"and surprising lust grows as they push forward, glistening red and thickening. The skin bunches up into an animal-like sheath, while a pair of fat bulges pop free. " +
				"You now have two nice thick dog-cocks, with decent sized knots. Both pulse and dribble animal-pre, arousing you in spite of your attempts at self-control.";
		}
		private static string ConvertedFirstDogCockGrewSecond(Creature target, CockData oldCockData)
		{
			return GlobalStrings.NewParagraph() + "Your " + oldCockData.LongDescription() + " vibrates, the veins clearly visible as it reddens and distorts. The head narrows into a pointed tip " +
				"while a gradually widening bulge forms around the base. Where it meets your crotch, the skin bunches up around it, forming a canine-like sheath. " +
				"You feel something slippery wiggling inside the new sheath, and another red point peeks out. In spite of yourself, you start getting turned on by the change, " +
				"and the new dick slowly slides free, eventually stopping once the thick knot pops free. The pair of dog-dicks hang there, " +
				"leaking pre-cum and arousing you far beyond normal.";
		}
		private static string ConvertedTwoCocksToDog(Creature target, CockData firstOldData, CockData secondOldData)
		{
			return GlobalStrings.NewParagraph() + "Your crotch twitches, and you pull open your " + target.armor.ItemName() + " to get a better look. You watch in horror and arousal as your " +
				firstOldData.LongDescription() + " and " + secondOldData.LongDescription() + " both warp and twist, becoming red and pointed, growing thick bulges near the base. " +
				"When it stops you have two dog-cocks and an animal-like sheath. The whole episode turns you on far more than it should, leaving you dripping animal pre " +
				"and ready to breed.";
		}
		private static string GrewSecondDogCockHadOne(Creature target)
		{
			return "You feel something slippery wiggling inside your sheath, and another red point peeks out. In spite of yourself, you start getting turned on by the change, " +
				"and the new dick slowly slides free, eventually stopping once the thick knot pops free. The pair of dog-dicks hang there, " +
				"leaking pre-cum and arousing you far beyond normal.";
		}

		private static string ConvertedOneCockToDogHadOne(Creature target, int index, CockData oldData)
		{
			string armorText;

			if (target.wearingArmor || target.wearingLowerGarment)
			{
				armorText = "you pull open your " + (target.wearingArmor ? target.armor.ItemName() : target.lowerGarment.ItemName() + "to get a better look");
			}
			else
			{
				armorText = "you look down to see what's happening";
			}

			return "Your crotch twitches, and " + armorText + ". You watch in horror and arousal as your " + oldData.LongDescription() +
				" warps and twists, becoming red and pointed, just like other dog-dick, growing thick bulges near the base. When it stops you have two dog-cocks " +
				"and an animal-like sheath. The whole episode turns you on far more than it should, leaving you dripping animal pre and ready to breed.";
		}

		private static string WastedKnottyText(Creature target)
		{
			return GlobalStrings.NewParagraph() + "A slight wave of nausea passes through you. It seems this pepper does not quite agree with your body.";
		}

		private static string GrewBallsText(Creature target)
		{
			return GlobalStrings.NewParagraph() + "A spike of pain doubles you up, nearly making you vomit. You stay like that, nearly crying, as a palpable sense of relief suddenly washes over you. " +
				"You look down and realize you now have a small sack, complete with two relatively small balls.";
		}
		private static string EnlargedBallsText(Creature target, BallsData oldData)
		{
			if (oldData.count == 1)
			{
				return GlobalStrings.NewParagraph() + "A wave of heat reaches your groin, quickly followed by a bit of discomfort. A sudden weight develops in your groin, alongside your uniball. " +
					"A quick examination confirms the obvious - your uniball is now a pair of regular balls.";
			}
			else if (oldData.size <= 2)
			{
				return GlobalStrings.NewParagraph() + "A flash of warmth passes through you, finally settling in your groin, which feels heavier. You pause to examine the changes and " +
					"your roving fingers discover your " + target.balls.ShortDescription() + " have grown larger than a human's.";
			}
			else
			{
				return GlobalStrings.NewParagraph() + "A sudden onset of heat envelops your groin, focusing on your " + target.balls.SackDescription() + ". Walking becomes difficult as you discover your " +
					target.balls.ShortDescription() + " have enlarged further.";
			}
		}

		private static string CouldntConvertDemonCockThickenedInstead(Creature target, int index, double delta)
		{
			Cock demonSpecialCase = target.cocks[index];
			return "Your " + demonSpecialCase.LongDescription() + " color shifts red for a moment and begins to swell at the base, but within moments it smooths out, " +
				"retaining its distinctive demonic shape, only perhaps a bit thicker.";
		}
		private static string AddedCumText(Creature target, double delta, bool isMultiplier)
		{
			StringBuilder sb = new StringBuilder(GlobalStrings.NewParagraph());
			if (!target.hasBalls)
			{
				sb.Append("You feel a churning inside your gut as something inside you changes.");
			}
			else
			{
				sb.Append("You feel a churning in your " + target.balls.ShortDescription() + ". It quickly settles, leaving them feeling somewhat more dense.");
			}
			if (!isMultiplier || delta > 1)
			{
				sb.Append(" A bit of milky pre dribbles from your " + target.genitals.AllCocksShortDescription() + ", pushed out by the change.");
			}
			return sb.ToString();
		}

		private static string GrewSmallestCockText(Creature target, int index, CockData oldData)
		{
			var smallest = target.cocks[index];
			double deltaLength = smallest.length - oldData.length;
			if (deltaLength > 2)
			{
				return GlobalStrings.NewParagraph() + "Your " + smallest.LongDescription() + " tightens painfully, inches of bulging dick-flesh pouring out from your crotch as it grows longer. " +
					"Thick pre forms at the pointed tip, drawn out from the pleasure of the change.";
			}
			else if (deltaLength > 1)
			{
				return GlobalStrings.NewParagraph() + "Aching pressure builds within your crotch, suddenly releasing as an inch or more of extra dick-flesh spills out. A dollop of pre " +
					"beads on the head of your enlarged " + smallest.LongDescription() + " from the pleasure of the growth.";
			}
			else
			{
				return GlobalStrings.NewParagraph() + "A slight pressure builds and releases as your " + smallest.LongDescription() + " pushes a bit further out of your crotch.";
			}
		}

		private static string UpdateAndGrowAdditionalRowText(Creature target, BreastCollectionData oldBreastData, bool crit, bool uberCrit)
		{
			var breastCount = target.breasts.Count;
			var newBreast = target.breasts[breastCount - 1];

			StringBuilder sb = new StringBuilder(GlobalStrings.NewParagraph());

			int rowsAlteredSoFar = 0;

			for (int x = 0; x < breastCount - 1; x++)
			{
				if (oldBreastData.breasts[x].cupSize != target.breasts[x].cupSize)
				{
					if (rowsAlteredSoFar == 0)
					{
						if (target.wearingArmor || target.wearingUpperGarment)
						{
							sb.Append("Your " + oldBreastData.breasts[x].LongDescription() + " feel constrained and painful against your top ");
						}
						else
						{
							sb.Append("Your " + oldBreastData.breasts[x].LongDescription() + "suddenly feel heavy, and you watch ");
						}
						sb.Append("as they grow larger by the moment, finally stopping as they reach " + target.breasts[x].cupSize.AsText() + "size. ");
					}
					else
					{
						sb.Append("Your " + Utils.NumberAsPlace(x + 1) + "row of breasts quickly follow suit, growing util they are at least " +
							target.breasts[x].cupSize.AsText() + "s.");
					}
					rowsAlteredSoFar++;
				}
			}

			if (rowsAlteredSoFar > 0)
			{
				sb.Append("But it doesn't stop there, as you feel a tightness beginning lower on your torso...");
			}


			if (breastCount == 2)
			{
				if (newBreast.cupSize == CupSize.FLAT)
				{
					sb.Append("A second set of breasts forms under your current pair, stopping while they are still fairly flat and masculine looking.");
				}
				else
				{
					sb.Append("A second set of breasts bulges forth under your current pair, stopping as they reach " + newBreast.cupSize.AsText() + "s.");
				}
			}
			else
			{
				if (newBreast.cupSize == CupSize.FLAT)
				{
					sb.Append("Your abdomen tingles and twitches as a new row of breasts sprouts below the others. Your new breasts stay flat and masculine, not growing any larger.");
				}
				else
				{
					sb.Append("Your abdomen tingles and twitches as a new row of " + newBreast.LongDescription() + " sprouts below your others.");
				}

			}
			sb.Append(" A sensitive nub grows on the summit of each new tit, becoming a new nipple.");

			if (uberCrit)
			{
				sb.Append(" You heft your new chest experimentally, exploring the new flesh with tender touches. Your eyes nearly roll back in your head from the intense feelings.");
			}
			else if (crit)
			{
				sb.Append(" You touch your new nipples with a mixture of awe and desire, the experience arousing beyond measure. You squeal in delight, nearly orgasming, " +
					"but in time finding the willpower to stop yourself.");
			}

			return sb.ToString();
		}

		private static string NormalizedBreastSizeText(Creature target, BreastCollectionData oldBreasts)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(Environment.NewLine + "The weight of your multiple rows of breasts starts to shift, though it the overall weight doesn't seem to be changing. " +
					"You quickly realize your breasts are evening themselves out, more evenly distributing the weight.");

			for (int x = 0; x < target.breasts.Count; x++)
			{
				short changeInCupSize = ((byte)target.breasts[x].cupSize).delta((byte)oldBreasts.breasts[x].cupSize);

				sb.Append("Your " + Utils.NumberAsPlace(x + 1) + " row of breasts " + (changeInCupSize > 0 ? "grows " : "shrinks ") +
					(changeInCupSize > 2 || changeInCupSize < -2 ? "significantly" : "") + ", and is now a " + target.breasts[x].cupSize.AsText());
			}

			return sb.ToString();
		}

		private static string DoggoFantasyText(Creature target)
		{
			StringBuilder sb = new StringBuilder(GlobalStrings.NewParagraph() + "Images and thoughts come unbidden to your mind, overwhelming your control as you rapidly lose yourself in them, " +
				"daydreaming of... ");
			//cawk fantasies
			if (target.gender == Gender.MALE || target.gender == Gender.HERM && Utils.RandBool())
			{
				sb.Append("bounding through the woods, hunting with your master. Feeling the wind in your fur and the thrill of the hunt coursing through your veins intoxicates you. " +
					"You have your nose to the ground, tracking your quarry as you run, until a heavenly scent stops you in your tracks.");
				//break1
				if (target.corruption < 33)
				{
					sb.Append(Environment.NewLine + "You shake your head to clear the unwanted fantasy from your mind, repulsed by it.");
				}
				else
				{
					sb.Append(" Heart pounding, your shaft pops free of its sheath on instinct, as you take off after the new scent. Caught firmly in the grip of a female's heat, " +
						"you ignore your master's cry as you disappear into the wild, " + CockType.DOG.ShortDescription() + " growing harder as you near your quarry. " +
						"You burst through a bush, spotting a white-furred female. She drops, exposing her dripping fem-sex to you, the musky scent of her sex " +
						"channeling straight through your nose and sliding into your " + CockType.DOG.ShortDescription() + ".");
					//Break 2
					if (target.corruption < 66)
					{
						sb.Append(Environment.NewLine + "You blink a few times, the fantasy fading as you master yourself. That daydream was so strange, yet so hot.");
					}
					else
					{
						sb.Append(" Unable to wait any longer, you mount her, pressing your bulging knot against her vulva as she yips in pleasure. The heat of her sex is unreal, " +
							"the tight passage gripping you like a vice as you jackhammer against her, biting her neck gently in spite of the violent pounding.");
						//break3
						if (target.corruption < 80)
						{
							if (target.hasVagina)
							{
								sb.Append(Environment.NewLine + "You reluctantly pry your one hand off your " + target.cocks[0].LongDescription() + " and the other from your aching " +
									target.vaginas[0].LongDescription() + " as you drag yourself out of your fantasy.");
							}
							else
							{
								sb.Append(Environment.NewLine + "You reluctantly pry your hand from your aching " + target.cocks[0].LongDescription() +
									" as you drag yourself out of your fantasy.");
							}
						}
						else
						{
							sb.Append(" At last your knot pops into her juicy snatch, splattering her groin with a smattering of her arousal. The scents of your mating " +
								"reach a peak as the velvet vice around your " + CockType.DOG.ShortDescription() + " quivers in the most indescribably pleasant way. " +
								"You clamp down on her hide as your whole body tenses, unleashing a torrent of cum into her sex. Each blast is accompanied by a squeeze " +
								"of her hot passage, milking you of the last of your spooge. Your " + target.lowerBody.ShortDescription() + " give out as your fantasy " +
								"nearly brings you to orgasm, the sudden impact with the ground jarring you from your daydream.");
						}
					}
				}
			}
			//Pure female fantasies
			else if (target.gender.HasFlag(Gender.FEMALE))
			{
				sb.Append("wagging your dripping " + target.vaginas[0].ShortDescription() + " before a pack of horny wolves, watching their shiny red doggie-pricks practically " +
					"jump out of their sheaths at your fertile scent.");
				//BREAK 1
				if (target.corruption < 33)
				{
					sb.Append(Environment.NewLine + "You shake your head to clear the unwanted fantasy from your mind, repulsed by it.");
				}
				else
				{
					sb.Append(" In moments they begin their advance, plunging their pointed beast-dicks into you, one after another. You yip and howl with pleasure" +
						" as each one takes his turn knotting you.");
					//BREAK 2
					if (target.corruption <= 66)
					{
						sb.Append(Environment.NewLine + "You blink a few times, the fantasy fading as you master yourself. That daydream was so strange, yet so hot.");
					}
					else
					{
						sb.Append(" The feeling of all that hot wolf-spooge spilling from your overfilled snatch and running down your thighs is heavenly, " +
							"nearly making you orgasm on the spot. You see the alpha of the pack is hard again, and his impressive member is throbbing with the need to breed you.");
						//break3
						if (target.corruption < 80)
						{
							sb.Append(Environment.NewLine + "You reluctantly pry your hand from your aching " + target.vaginas[0].LongDescription() +
								" as you drag yourself out of your fantasy.");
						}
						else
						{
							sb.Append(" You growl with discomfort as he pushes into your abused wetness, stretching you tightly, every beat of his heart vibrating through your nethers. " +
								"With exquisite force, he buries his knot in you and begins filling you with his potent seed, impregnating you for sure. " +
								"Your knees give out as your fantasy nearly brings you to orgasm, the sudden impact with the ground jarring you from your daydream.");
						}
					}
				}
			}
			else
			{
				sb.Append("wagging your " + target.ass.LongDescription() + " before a pack of horny wolves, watching their shiny red doggie-pricks practically " +
					"jump out of their sheaths at you after going so long without a female in the pack.");
				//BREAK 1
				if (target.corruption < 33)
				{
					sb.Append(Environment.NewLine + "You shake your head to clear the unwanted fantasy from your mind, repulsed by it.");
				}
				else
				{
					sb.Append(" In moments they begin their advance, plunging their pointed beast-dicks into you, one after another. You yip and howl with pleasure " +
						"as each one takes his turn knotting you.");
					//BREAK 2
					if (target.corruption <= 66)
					{
						sb.Append(Environment.NewLine + "You blink a few times, the fantasy fading as you master yourself. That daydream was so strange, yet so hot.");
					}
					else
					{
						sb.Append(" The feeling of all that hot wolf-spooge spilling from your overfilled ass and running down your thighs is heavenly, " +
							"nearly making you orgasm on the spot. You see the alpha of the pack is hard again, and his impressive member is throbbing " +
							"with the need to spend his lust on you.");
						//break3
						if (target.corruption < 80)
						{
							sb.Append(Environment.NewLine + "You reluctantly pry your hand from your aching asshole as you drag yourself out of your fantasy.");
						}
						else
						{
							sb.Append(" You growl with discomfort as he pushes into your abused, wet hole, stretching you tightly, every beat of his heart " +
								"vibrating through your hindquarters. With exquisite force, he buries his knot in you and begins filling you amounts seed that would easily" +
								"impregnate any female. Your knees give out as your fantasy nearly brings you to orgasm, the sudden impact with the ground jarring you from your daydream.");
						}
					}
				}
			}
			return sb.ToString();
		}

		private static string ChangedFurText(Creature target, ReadOnlyFurColor oldFurColor)
		{
			return GlobalStrings.NewParagraph() + SafelyFormattedString.FormattedText("Your fur tingles, growing in thicker than ever " +
				"as darkness begins to spread from the roots, turning it midnight black.", StringFormats.BOLD);
		}

		private static string ChangedBodyTypeText(Creature target, BodyData oldBodyData)
		{
			return GlobalStrings.NewParagraph() + SafelyFormattedString.FormattedText("Your " + target.body.primarySkin.ShortDescription() + " itches like crazy as fur grows out from it, coating your body. " +
				"It's incredibly dense and black as the middle of a moonless night.", StringFormats.BOLD);
		}

		private static string ChangedArmsText(Creature target, ArmData oldArmData)
		{
			return GlobalStrings.NewParagraph() + "Weakness overcomes your arms, and no matter what you do, you can't muster the strength to raise or move them."
			 + " Did the pepper have some drug-like effects? Sitting on the ground, you wait for the limpness to end."
			 + " As you do so, you realize that the bones at your hands are changing, as well as the muscles on your arms."
			 + " They're soon covered, from the shoulders to the tip of your digits, on a layer of soft,"
			 + " fluffy " + target.ActiveHairOrFurColor().AsString() + " fur."
			 + " Your hands gain pink, padded paws where your palms were once, and your nails become short claws,"
			 + " not sharp enough to tear flesh, but nimble enough to make climbing and exploring easier. "
			 + SafelyFormattedString.FormattedText("Your arms have become like those of a dog!", StringFormats.BOLD);
		}

		private static string FallbackToughenUpText(Creature target)
		{
			return GlobalStrings.NewParagraph() + "You become more... solid. Sinewy. A memory comes unbidden from your youth of a grizzled wolf you encountered while hunting, covered in scars, " +
				"yet still moving with an easy grace. You imagine that must have felt something like this.";
		}

		private static string NothingHappenedGainHpText(Creature target)
		{
			return GlobalStrings.NewParagraph() + "Inhuman vitality spreads through your body, invigorating you!" + Environment.NewLine;
		}
	}
}
