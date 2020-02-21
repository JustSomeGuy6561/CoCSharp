//forest gown that has a slow progression to a dryad

using System;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Items.Wearables;
using CoC.Backend.Items.Wearables.Armor;
using CoC.Backend.Reaction;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.Backend.UI;
using CoC.Frontend.Transformations;
using CoC.Frontend.UI;

namespace CoC.Frontend.Items.Wearables.Armor
{

	public class Gown : ArmorBase, IWearableDailyFull
	{
		public override string AbbreviatedName() => "FrsGown";

		public override string ItemName() => "Forest Gown";

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string gownText = count != 1 ? "gowns" : "gown";

			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";
			return $"{count}forest {gownText}";
		}

		public override double PhysicalDefensiveRating(Creature wearer) => 1;

		protected override int monetaryValue => 10;

		public override string AboutItem()
		{
			return "This the very earthy gown commonly worn by dryads. It is made from a mixture of plants. The predominate fabric looks like a weave of fresh grass. " +
				"It is decorated by simple flowers that are attached to it. Strangely, everything seems alive like it was still planted. " +
				"There must be some peculiar magic at work here.";
		}



		public Gown() : base(ArmorType.LIGHT_ARMOR)
		{ }

		protected override string EquipText(Creature wearer)
		{
			StringBuilder sb = new StringBuilder();

			if (wearer.gender == Gender.FEMALE)
			{
				sb.Append("You comfortably slide into the forest gown. You spin around several times and giggle happily."
					  + " Where did that come from?");
			}
			else if (wearer.gender == Gender.MALE)
			{
				sb.Append("You slide forest gown over your head and down to your toes. It obviously would fit someone more female."
				  + " You feel sad and wish you had the svelte body that would look amazing in this gown."
				  + " Wait, did you always think that way?");
			}
			else
			{
				sb.Append("You slide the gown over your head and slip it into place.");
			}
			if (wearer.hasCock)
			{
				sb.Append("You notice the bulge from [cock] in the folds of the dress and wish it wasn't there." + Environment.NewLine);
			}

			return sb.ToString();
		}

		public override bool Equals(ArmorBase other)
		{
			return other is Gown;
		}

		byte IWearableDailyFull.hourToTrigger => 5;

		TimeReactionBase IWearableDailyFull.ReactToDailyTrigger(Creature wearer)
		{
			return new Reaction(wearer);
		}

		private class Reaction : FullTimeReaction
		{
			private readonly Creature target;
			public Reaction(Creature wearer)
			{
				target = wearer ?? throw new ArgumentNullException(nameof(wearer));
			}

			protected override DisplayBase AsFullPageScene(bool currentlyIdling, bool hasIdleHours)
			{
				StandardDisplay display = new StandardDisplay();
				var tf = new DryadTFs();
				display.OutputText(tf.DoTransformation(target, out bool isBadEnd));
				//silently ignore the bad end. if it happens that feels like some next level bullshit, so...
				display.DoNext(GameEngine.ResumeExection);
				return display;
			}
		}

		private class DryadTFs : DryadTransformations
		{
			public DryadTFs() : base(false)
			{
			}

			private static readonly string[] dreams;

			static DryadTFs()
			{
				dreams = new string[]
				{
					"In your dream you find yourself in a lush forest standing next to a tree. The tree seems like your best friend and You give it a hug before sitting down next to it. Looking around, there is grass and flowers all about. You can't help but hum a cheery tune enjoying nature as a bright butterfly flutters nearby, holding out your hand the butterfly lands softly on your finger and smile sweetly to it.",
					"You walk through a meadow and down towards a swift running brook, It looks like some clean water. You sit down, pull up your gown and admire your feet. They are brown, the color of bark. You place your feet in the cool water of the brook and soak up the water through your feet, a cool refreshing feeling fills your body. Ah, being a Dryad is so nice.",
					"Your imagination runs a bit wild and you picture yourself dressed in your nice forest gown eyeing a satyr you stumble across in a grove. You can't help but smirk as you consider all the fun you could have with him.",
					"You imagine running into a waterfall. You dance around playfully while torrents of water splash against your body. Your soaked gown scandalously clings to your frame.",
					"You dream about tending to plants in a meadow. Some fairies are following you. One of them buzzes around your head playfully and kisses you on the cheek.",
					"In your dream you are laying out in a lush glade soaking up the sun. You inspect your leafy hair and find that here are some buds forming. You can't help but wonder what the flowers will look like. ",
					"You dream of a starlight dance in the meadow. Several satyrs, nymphs and dryads are gathered together to celebrate the full moon. You take your gown off and gather around the fairy circle. The half dozen naked revelers dance the night away in the pale moonlight.",
					"You dream of skipping joyfully through a flowered meadow. After a while you kneel down and pick several budding flowers, placing them in your hair you find that they merge with the leafy vines. They will bloom in a day or so. It will be so lovely.",
					"In your dream, you and another dryad dance for a reed playing satyr. The dance is very intimate and you can't help wonder how excited the satyr will become from watching. ",
					"You dream of being in a lush grove watching a satyr sleeping. He is leaning up against a tree that you are very fond of! Some mischief is in order. You politely ask the tree for help and it lowers several vines that lift you up and move you above him. The vine lowers you mere inches away from his body. You can practically hear him breathing and his chest rising and falling slowly. Leaning forward, you nibble on his ear, jolting him awake with a surprised look in his eyes just in time to see the vines recede back into the tree. Chuckling to yourself, you watch the satry try to figure out what just happened from a branch above him. ",
					"You dream of being in a field surrounded by butterflies. There are blue ones, yellow ones, and pink ones, buthich ones are your favorite? You spin around in your gown and laugh as you decide they are all your favorite!",
					"You dream about wandering around in the mountains. There is snow everywhere, and you notice something off in the distance digging its way out from under the snow. It's a big bear and two smaller ones pawing their way out! You gleefully saunter up and say, 'Hello Mrs.bear, did you and your children enjoy your nap?' You awake shortly after the strange dream, feeling confused. Was that really you?",
					"You dream about sitting on a tree branch in your lovely gown. You freely hang off the large oak, dangling over the side while your legs hold you up. Your gown falls down to your arms and hangs over your head. You get excited, pondering if someone might be watching and just got flashed. You sit back up and straighten out your gown, coy smile playing across your face.",
				};
			}

			protected override string InitialTransformationText(Creature target)
			{
				return Utils.RandomChoice(dreams);
			}

			protected override string HipChangeText(Creature target, HipData oldHips)
			{
				string verb = target.hips.size - oldHips.size > 0 ? "enlarge" : "shrink";

				return "You wiggle around in your gown, the pleasant feeling of flower petals rubbing against your skin washes over you." +
					" The feeling settles on your " + oldHips.ShortDescription() + "." + Environment.NewLine + "You feel them slowly " +
					verb + ". <b>You now have " + target.build.HipsShortDescription() + "</b>." + Environment.NewLine;

				;
			}

			protected override string ButtChangeText(Creature target, ButtData oldButt)
			{
				string verb = target.butt.size - oldButt.size > 0 ? "enlarge" : "shrink";

				return "You wiggle around in your gown, the pleasant feeling of flower petals rubbing against your skin washes over you." +
					" The feeling settles on your " + oldButt.ShortDescription() + "." + Environment.NewLine + "You feel it slowly " +
					verb + ". <b>You now have a " + target.build.ButtShortDescription() + ".</b>" + Environment.NewLine;
			}

			protected override string CockChangedText(Creature target, GenitalsData oldGenitalData, int changedCock)
			{
				bool grewVagina = oldGenitalData.vaginas.Count == 0 && target.vaginas.Count > 0;
				bool lostBalls = oldGenitalData.balls.hasBalls && !target.balls.hasBalls;
				bool lostCock = oldGenitalData.cocks.Count > target.cocks.Count;

				CockData previousCockData = oldGenitalData.cocks[changedCock];

				string end;

				if (!grewVagina)
				{
					end = target.genitals.allCocks.GenericChangeOneCockLengthText(previousCockData);
				}
				else
				{
					end = target.genitals.allCocks.GenericChangeOneCockLengthText(previousCockData, lostCock, false) +
						"Your " + oldGenitalData.cocks[0].LongDescription() + " suddenly starts tingling. It's a familiar feeling, similar to an orgasm." +
						"However, this one seems to start from the top down, instead of gushing up from your loins. You spend a few seconds frozen to the odd sensation, " +
						"when it suddenly feels as though your own body starts sucking on the base of your shaft. Almost instantly, " +
						"your cock sinks into your crotch with a wet slurp. The tip gets stuck on the front of your body on the way down, " +
						"but your glans soon loses all volume to turn into a shiny new clit." +
						(lostBalls ? " At the same time, your " + oldGenitalData.balls.ShortDescription() + " fall victim to the same sensation; " +
						"eagerly swallowed whole by your crotch." : "") + " Curious, you touch around down there, to find you don't have any exterior organs left." +
						"All of it got swallowed into the gash you now have running between two fleshy folds, like sensitive lips. " +
						"It suddenly occurs to you; <b> you now have a vagina!</b> ";
				}

				return "Your " + previousCockData.LongDescription() + " feels strange as it brushes against the fabric of your gown." + Environment.NewLine +
					end;
			}

			protected override string BreastsChangedText(Creature target, BreastCollectionData oldData)
			{
				string removeText = target.breasts.Count < oldData.breasts.Count ? Environment.NewLine + "Some of your breasts shrink back into your body leaving you with just two." : "";

				string cupText = "";

				if (target.breasts[0].cupSize != oldData[0].cupSize)
				{
					if (target.breasts[0].cupSize > oldData[0].cupSize)
					{
						cupText = Environment.NewLine + "Heat builds in chest and your boobs become bigger.";
					}
					else if (target.breasts[0].cupSize < oldData[0].cupSize)
					{
						cupText = Environment.NewLine + "A chill runs against your chest and your boobs become smaller.";
					}

					cupText += Environment.NewLine + "<b>You now have " + target.breasts[0].LongDescription() + "</b>";
				}

				return "You feel like a beautful flower in your gown. Dawn approaches and you place your hands on your chest"
					+ " as if expecting your nipples to bloom to greet the rising sun." + removeText + cupText;
			}

			protected override string FemininityChangedText(Creature target, FemininityData oldFem)
			{
				return "You run your " + target.hands.ShortDescription() + " across the fabric of your Gown, then against your face as it feels like"
					+ " there is something you need to wipe off. " + target.femininity.FemininityChangedText(oldFem);
			}

			protected override string ChangeEarText(Creature target, EarData oldEars)
			{
				return "There is a tingling on the sides of your head as your ears change to pointed elfin ears.";
			}

			protected override string ChangeSkinText(Creature target, BodyData oldBody)
			{
				if (oldBody.type != BodyType.defaultValue)
				{
					return "A tingling runs through your " + oldBody.LongDescription() + " as it changes back to normal";
				}
				else
				{
					return "Your skin hardens and becomes the consistency of tree's bark.";
				}
			}

			protected override string RestoreLegsText(Creature target, LowerBodyData oldLowerBody)
			{
				return "There is a rumbling in your lower body as it returns to a human shape.";
			}

			protected override string RestoredArmsText(Creature creature, ArmData oldArms)
			{
				if (oldArms.hands.isHands)
				{
					return "Your hands shake and shudder as they slowly transform back into normal human hands. The change radiates through your arms as well, " +
						"finally stopping when they return to normal too.";
				}
				else
				{
					return "Your arms twitch, then slowly shift towards something more normal until they match your human hands.";
				}
			}

			protected override string RestoredFaceText(Creature target, FaceData oldFace)
			{
				return "Your face twitches a few times and slowly morphs itself back to a normal human face.";
			}

			protected override string ChangedHairText(Creature target, HairData oldHair)
			{
				return "Much to your shock, your hair begins falling out in tuffs onto the ground. Moments later, " +
					"your scalp sprouts vines all about that extend down and bloom into leafy hair.";
			}
		}



	}
}
