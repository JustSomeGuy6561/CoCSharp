//idea from liadri, added to by others
using System;
using System.Collections.Generic;
using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Engine.Time;
using CoC.Backend.Items.Wearables;
using CoC.Backend.Items.Wearables.Armor;
using CoC.Backend.Reaction;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.Backend.UI;
using CoC.Frontend.Settings.Gameplay;
using CoC.Frontend.Transformations;
using CoC.Frontend.UI;

namespace CoC.Frontend.Items.Wearables.Armor
{

	public sealed class NagaSilkDress : ArmorBase, IWearableDailyFull
	{
		private readonly Tones dressColor;

		public NagaSilkDress() : this(Tones.PURPLE)
		{ }

		public NagaSilkDress(Tones color) : base(ArmorType.CLOTHING)
		{
			dressColor = color;
		}

		public override string AbbreviatedName() => "NagaDress";
		public override string ItemName() => "Naga Dress";

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string dressText = count != 1 ? "dresses" : "dress";

			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";
			return $"{count}{dressColor.AsString()}-silk naga {dressText}";
		}

		public override string AboutItem()
		{
			return "A very seductive dress made for naga or females without a human set of legs. It has a black collar, bikini top, sleeves with golden bangles and a waistcloth, " +
				"all decorated with a golden trim. The bottom has a " + dressColor.AsString() + " silk veil that runs down to what would be the human knee " +
				"while the center of the bikini is also covered by a small strand of silk. It helps accentuate your curves and increase your natural charm. " +
				"The dress obviously is so minimalist that you could as well say you are naked yet it looks quite classy on a tauric or " +
				"naga body giving you the air of a master seducer.";
		}

		//technically tones is for skin tones, but i'm lazy and can reuse it pretty much everywhere.

		protected override int monetaryValue => 0;

		public override float DefensiveRating(Creature wearer) => 0;


		protected override string EquipText(Creature wearer)
		{ //Produces any text seen when equipping the armor normally
			return "You wear the dress and hiss like a snake. " + GlobalStrings.NewParagraph() + " Where did that come from?." + Environment.NewLine;
		}

		public override bool Equals(ArmorBase other)
		{
			return other is NagaSilkDress;//the color is more or less irrelevant, it's just cosmetic.
		}

		byte IWearableDailyFull.hourToTrigger => 5;

		TimeReactionBase IWearableDailyFull.ReactToDailyTrigger(Creature wearer)
		{
			if ((wearer is CombatCreature cc && cc.relativeSpeed < 70) ||
				wearer.neck.type != NeckType.HUMANOID ||
				wearer.back.type != BackType.SHARK_FIN ||
				(wearer.breasts.Count > 1 && !HyperHappySettings.isEnabled) ||
				(wearer.gender.HasFlag(Gender.MALE) && (wearer.cocks.Count != 2 || wearer.genitals.CountCocksOfType(CockType.LIZARD) != 2)) ||
				wearer.wings.type != WingType.NONE ||
				wearer.back.type == BackType.SHARK_FIN ||
				wearer.antennae.type != AntennaeType.NONE ||
				wearer.tongue.type != TongueType.SNAKE ||
				wearer.face.type != FaceType.SNAKE ||
				wearer.lowerBody.type != LowerBodyType.NAGA ||
				wearer.body.type != BodyType.REPTILE ||
				wearer.gills.type != GillType.NONE
			)

			{
				return new Reaction(wearer);
			}
			return null;
		}

		private static readonly string[] dreams;

		static NagaSilkDress()
		{
			List<string> dreamMaker = new List<string>();

			dreamMaker.Add("That night you have a strange dream. You are in the desert basking in the sun. You look down and notice that there are not legs attached to your " +
				"lower body. There is a large snake-like tail. Thinking about it seems to make your tail flip slightly. " + GlobalStrings.NewParagraph() +
				"You can sense some travelers nearby, but not because you can hear them. Can you smell them…. Taste them? The thought of tasting someone causes " +
				"your tongue to fork out. Oh yes, your tongue can sense things some distance away. Wait, did you always have a snake's tongue?" +
				GlobalStrings.NewParagraph() + "When you awaken, you check yourself out, unsure if you are the person who you seemed to be in your dream." +
				GlobalStrings.NewParagraph());

			dreamMaker.Add("When you sleep that night you dream of going home to your sweetheart. You open the door and... slither... inside. Your sweetheart is " +
				"happy to see you. The two of you passionately embrace. " + GlobalStrings.NewParagraph() + "You move to kiss and your forked tongue moves from your mouth. " +
				"At the same time your snake-like lower body entwines around them forcing the two of you closer together. " + GlobalStrings.NewParagraph() +
				"Something doesn't seem like it should be and it startles you awake. Last you checked, you didn't look quite match the image of the you in the dream " +
				"and it weighs heavily on your mind." + GlobalStrings.NewParagraph());

			dreamMaker.Add("Laying down for the night, you find a strange dream awaiting you as soon as you doze off. The sun goes down and the night chill brings a very unwelcome cold that makes your whole body feel heavy." +
				" Your snake-like eyes sense heat just like your old eyes could see color, causing you to notice a pillar of heat off in the distance." + GlobalStrings.NewParagraph() +
				"You slither your way towards the source an detect a campfire smouldering. Next to the campfire is a tent. The tent seems to have a very enticing heat source inside. " + GlobalStrings.NewParagraph() +
				"Your heat sensing eye detects the outline of a person after opening the flap. You quietly enter and proceed to position your snake-like lower half to wrap around the person." +
				" With a snap, you have the figure completely coiled. " + GlobalStrings.NewParagraph() +
				"What happens next becomes very fuzzy in your dream." + GlobalStrings.NewParagraph() +
				"Dawn breaks and awaken to you find yourself craving a warm body close to yours.");

			dreamMaker.Add("That night you have a strange dream about being a naga." + GlobalStrings.NewParagraph() +
				"It is a cold day outside so you go and prepare yourself a warm bath. You strip down and hop into the bath, making a nice splash. " +
				"You coil your tail into the hot water, and plop the tip of your snake-like lower half out of the water, wiggling it around a little bit." +
				GlobalStrings.NewParagraph() + "There is a knock on the door." + GlobalStrings.NewParagraph() +
				"\"Honey, may I come in?\" a familiar voice echoes from the other side of the door." + GlobalStrings.NewParagraph() +
				"You wake up a little confused about your current self. Fantasies about two lovers in a bath together swirl about in your imagination. "
				+ GlobalStrings.NewParagraph());

			dreamMaker.Add("You have a dream that night where you are lounging in a tree planning naughty schemes." + GlobalStrings.NewParagraph() +
				"You are in the tree with your snake-like tail coiled around a branch enjoying the hot sun. A rather attractive person walks into the tree's shade " +
				"and sits down. Likely they are just trying to cool off." + GlobalStrings.NewParagraph() + "A though occurs to you: <i>This is a good time to surprise them,</i>" +
				"and you act on it a moment later. You ball up your snakelike lower half and strike with swiftness. " + GlobalStrings.NewParagraph() +
				"With precision you land a kiss right on the lips of the attractive victim below. They have no idea and stumble to the ground in surprise. " +
				"Eventually, they look up and see you there smirking." + GlobalStrings.NewParagraph() + "Being a naga is so fun." + GlobalStrings.NewParagraph() +
				"What a weird dream..." + GlobalStrings.NewParagraph());

			dreamMaker.Add("Lying down for the night brings strange dreams about yourself as a naga." + GlobalStrings.NewParagraph() +
				"You are lying in bed with your lover. You turn over and move your tail a bit. Your lover gasps in shock after noticing a slithering at the end of the bed." +
				GlobalStrings.NewParagraph() + "\"Snake!\" They scream." + GlobalStrings.NewParagraph() +
				"\"Silly, that's just me.\" You reply." + GlobalStrings.NewParagraph() +
				"When morning comes you have to check to see if your lower half is snake-like or not." + GlobalStrings.NewParagraph());

			dreamMaker.Add("You and your special someone are going in a walk in the hot desert sun. Eventually, you come upon an oasis. After finding a good spot in the sand by the water, you and your " +
			"lover sit down and cuddle together." + GlobalStrings.NewParagraph() +
			"Their arm wraps around your shoulder and your snake tail rises and rests on their shoulder. " + GlobalStrings.NewParagraph() +
			"Hmmm… snake tail." + GlobalStrings.NewParagraph() +
			"When you wake up you have to look yourself over. That dream seemed so real." + GlobalStrings.NewParagraph());

			dreamMaker.Add("In your dreams you are wandering and spot your hometown in Ingram off in the distance. It sure would be nice to go home. " + GlobalStrings.NewParagraph() +
			"You find your house and go inside. You hear a noise and someone comes in from the other room. Shockingly, that person is you, looking completely human. [he] shouts, \"Monster ! What are you doing in my home?" + GlobalStrings.NewParagraph() +
			"You say, \"I'm not a monster, this is my home, why do you look like me? " + GlobalStrings.NewParagraph() +
			"Before protesting more you notice a mirror and see yourself in it; a full naga with snake eyes, a tail and forked tongue." + GlobalStrings.NewParagraph());

			dreamMaker.Add("You have a dream about a naga being hunted by demons." + GlobalStrings.NewParagraph() +
			"The naga is fighting off demon dogs while they bark and bite. Lots of biting, barking and coiling happens. The result is a dozen defeated demon dogs and " +
			"one wounded naga. " + GlobalStrings.NewParagraph() + "You approach the wounded naga who begs for help before the demons that released the dogs find her " +
			"to finish the hunt. " + GlobalStrings.NewParagraph() + "As soon as you agree she hugs you tightly, enveloping your face with her voluptuous breasts." +
			GlobalStrings.NewParagraph() + "You awake wondering why you dream about nagas so often." + GlobalStrings.NewParagraph());

			dreams = dreamMaker.ToArray();
		}

		private void progression()
		{

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
				display.OutputText(Utils.RandomChoice(dreams));
				var tf = new NagaTFs();
				display.OutputText(tf.DoTransformation(target, out bool isBadEnd));
				//silently ignore the bad end. if it happens that feels like some next level bullshit, so...
				display.DoNext(GameEngine.ResumeExection);
				return display;
			}
		}

		private class NagaTFs : NagaTransformations
		{
			protected override string InitialTransformationText(Creature target, bool isInCombat)
			{
				throw new NotImplementedException();
			}
		}
	}
}
