/**
 * Created by aimozg on 11.01.14.
 */
namespace CoC.Frontend.Items.Wearables.Armor
{
	using System;
	using System.Linq;
	using System.Text;
	using CoC.Backend.BodyParts;
	using CoC.Backend.Creatures;
	using CoC.Backend.Engine.Time;
	using CoC.Backend.Items.Wearables.Armor;
	using CoC.Backend.Items.Wearables.LowerGarment;
	using CoC.Backend.Items.Wearables.UpperGarment;
	using CoC.Backend.Strings;
	using CoC.Backend.Tools;

	public sealed class LustyMaidensArmor : ArmorBase, ITimeDailyListenerSimple
	{
		private byte paizuriStacks = 0;


		public override string AbbreviatedName() => "LMArmor";

		public override string ItemName() => "lusty maiden's armor";
		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string setText = count != 1 ? "sets" : "set";

			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";

			return $"{count}bikini-like {setText} of armor that could only belong to a lusty maiden";
		}

		public override string AboutItem()
		{
			return "This skimpy chain bikini barely qualifies as armor. Indeed, the chain is made from links much finer and lighter than normal, so fine that it feels almost silken under your fingertips. A simple seal in the g-string-like undergarment states, \"Virgins only.\" " + Environment.NewLine + "Requirements: breast size of at least DD-cups and be a female.";
		}

		public override bool isNearlyNaked => true;


		protected override int monetaryValue => 400;

		public override bool CanWearWithUpperGarment(Creature wearer, UpperGarmentBase upperGarment, out string whyNot)
		{
			if (!UpperGarmentBase.IsNullOrNothing(upperGarment))
			{
				whyNot = GenericArmorIncompatText(upperGarment);
				return false;
			}
			else
			{
				whyNot = null;
				return true;
			}
		}

		public override bool CanWearWithLowerGarment(Creature wearer, LowerGarmentBase lowerGarment, out string whyNot)
		{
			if (!LowerGarmentBase.IsNullOrNothing(lowerGarment))
			{
				whyNot = GenericArmorIncompatText(lowerGarment);
				return false;
			}
			else
			{
				whyNot = null;
				return true;
			}
		}

		public LustyMaidensArmor() : base(ArmorType.LIGHT_ARMOR)
		{
		}

		public override double PhysicalDefensiveRating(Creature wearer)
		{
			if (wearer.genitals.HasVirginVagina())
			{
				return 9 + paizuriStacks;
			}

			return 6 + paizuriStacks;
		}

		protected override bool CanWearWithBodyData(Creature creature, out string whyNot)
		{
			//can't be worn if you have a cock, are genderless, have balls, or have small tits.
			if (creature.gender != Gender.FEMALE || creature.balls.hasBalls || creature.genitals.BiggestCupSize() < CupSize.DD)
			{
				whyNot = TryItOnText(creature);
				return false;
			}
			else
			{
				whyNot = null;
				return true;
			}
		}

		protected override string EquipText(Creature wearer)
		{
			return TryItOnText(wearer);
		}

		protected override void OnEquip(Creature wearer)
		{
			//make sure we actually equip the item and remove any other items too.
			base.OnEquip(wearer);

			wearer.IncreaseLust(25);
		}

		protected override ArmorBase OnRemove(Creature wearer)
		{
			paizuriStacks = 0;

			return this;
		}

		//try to put the armor on, and return the text accordingly. this is used both for the equip text and the can't use text.
		private string TryItOnText(Creature wearer)
		{
			CupSize largestCup = wearer.genitals.BiggestCupSize();
			if (largestCup < CupSize.A)
			{ //{No titties}
				return "You slide the bikini top over your chest and buckle it into place, but the material hangs almost comically across your flat chest. " +
					"The cold chain dangles away from you, swaying around ridiculously before smacking, cold and hard into your " + wearer.breasts[0].LongNippleDescription() +
					". This simply won't do - it doesn't fit you, and you switch back to your old armor." + GlobalStrings.NewParagraph();
			}
			else if (largestCup <= CupSize.D)
			{ //{Too small titties}
				return "You slide the bikini top over your chest, shivering when the cold chains catch on your nipples, stiffening them nicely. The material nicely accentuates " +
					"your chest, but there's a definite problem. Your " + wearer.genitals.ChestOrAllBreastsShort() + " aren't big enough! Sure, they look nice done up " +
					"in glittering silver and gold trim. If only the metal wasn't hanging loosely around your underbust, flopping around whenever you move. It doesn't even " +
					"look that sexy on you! You'll need a bigger chest to truly make use of this armor. For now, you switch back to your old equipment." + GlobalStrings.NewParagraph();
			}

			StringBuilder sb = new StringBuilder();

			sb.Append("You slide the bikini top over your more than ample chest, shivering at the touch of the cold metal on your sensitive nipples. It stretches taut " +
				"around each of your globes, and by the time you're snapping the narrow leather strap behind your back, the exotic metal bra has grown warm enough to make " +
				"your chest tingle pleasantly. Your hands find their way to your jiggling, gilded mounds and grab hold, fingers sinking into the shimmering flesh without meaning to. " +
				"Your nipples scrape along a diaphanous inner lining so pleasantly that a moan slips out of your mouth as you admire how your cleavage bulges out " +
				"above the glittery cups. A narrow band of steel with a shiny black leather thong underneath connects the two halfs of the top, padded for comfort but pulled away " +
				"from you by the sheer size of your straining bosoms.");

			sb.Append(GlobalStrings.NewParagraph() + "As you examine the material, you realize that leather band isn't just padding. It's as slippery as butter on grease " +
				"and has a subtle indentation, one that would let it perfectly cushion something round, thick... and throbbing. Your cheeks color when you catch yourself " +
				"thinking of titfucking some beast while dressed in this outfit, taking a thick load of monster or dick-girl seed right over your cleavage, face, and hair. " +
				"You could even line it up with your mouth and drink down a few swallows if you wanted to.");

			sb.Append(GlobalStrings.NewParagraph() + "You shake your head and smile ruefully - maybe once you finish getting dressed! There's still a bottom to put on, after all. " +
				"Regardless, one of your hands keeps coming to rest on your boob, idly groping and fondling your heavy tit whenever you have a free moment. " +
				"This sure is some fun armor!");

			sb.Append(GlobalStrings.NewParagraph() + "Now, the bottom is a leather thong and skirt combination. The thong itself is leather dyed radiant white, " +
				"with intricate gold filigree covering the front triangle. On the back triangle, there's a similar pattern, though you could swear that from a distance " +
				"the pattern looks a bit like arrows pointing towards where your " + wearer.ass.LongDescription() + " will be with golden sperm surrounding them. " +
				"No, that has to be your imagination. All this time in this strange land must really be getting to you! Both pieces are molded to accentuate the female form, " +
				"with a crease in the gusset that is tailor-made to rest over a vagina.");



			if (wearer.hasCock || wearer.balls.hasBalls)
			{

				sb.Append(GlobalStrings.NewParagraph() + "Without so much as a second thought, you try sliding the white thong into place. Unfortunately, ");
				if (wearer.hasCock && wearer.hasVagina)
				{
					sb.Append("your " + wearer.genitals.AllCocksShortDescription(out bool isPlural) + (isPlural ? "get" : "gets") + "in the way, preventing it from aligning with "
						+ "your " + wearer.genitals.AllVaginasShortDescription() + ".");
				}
				else if (wearer.hasCock)
				{
					sb.Append("your " + wearer.genitals.AllCocksShortDescription(out bool isPlural) + (isPlural ? "ensure" : "ensures") + "that it won't fit you at all!");
				}
				else if (wearer.hasVagina)
				{
					sb.Append("your " + wearer.balls.LongDescription(false, false, false, out bool isPlural) + (isPlural ? "get" : "gets") + "in the way, preventing it from " +
						"aligning with your " + wearer.genitals.AllVaginasShortDescription() + ".");
				}
				else
				{
					sb.Append("your " + wearer.balls.LongDescription(false, false, false, out bool isPlural) + (isPlural ? "ensure" : "ensures") + "that it won't fit you at all!");
				}

				sb.Append("<b>You put your old gear back on with a sigh</b>.");
				return sb.ToString();
			}
			else if (!wearer.hasVagina)
			{
				sb.Append(GlobalStrings.NewParagraph() + "Without so much as a second thought, you try sliding the white thong into place. Unfortunately, it" +
					"digs uncomfortably into your featureless groin, and despite your desire to bear through it, it's simply too much. <b>You put your " +
					"old gear back on with a sigh</b>.");
				return sb.ToString();
			}
			else if (wearer.vaginas.Count > 1)
			{
				sb.Append(GlobalStrings.NewParagraph() + "Without so much as a second thought, you try sliding the white thong into place, but quickly realize you have a problem: " +
					"it's not designed wih dual cunts in mind! With a bit of finagling, though, you manage to align it with " + wearer.genitals.OneVaginaOrVaginasShort() + ".");

			}
			else
			{
				sb.Append(GlobalStrings.NewParagraph() + "You don't give it a second thought, sliding the white thong snugly into place.");
			}

			sb.Append("Snug warmth slides right up against your mound, the perfectly formed crease slipping right into your labia, where it belongs, causing your ");

			//try to protect a virgin vagina. if we have 2 vaginas, and one is still virgin, we choose that. otherwise, randomly pick one.
			Vagina vag;
			int virginVaginas = wearer.vaginas.Count(x => x.isVirgin);
			if (wearer.vaginas.Count == 1)
			{
				vag = wearer.vaginas[0];
			}
			else if (virginVaginas > 0 && virginVaginas < wearer.vaginas.Count)
			{
				vag = wearer.vaginas.First(x => x.isVirgin);
			}
			else
			{
				vag = wearer.vaginas[Utils.RandomChoice(wearer.vaginas.Count)];
			}


			sb.Append(vag.LongDescription() + " to prominently displaying your camel-toe for all to see.");

			if (virginVaginas == wearer.vaginas.Count)
			{
				sb.Append("It forms a tight seal over your chastity, displaying your womanly status while guarding your maidenhead at the same time");

				if (wearer.vaginas.Count > 1)
				{
					sb.Append(", though you're a little worried since it doesn't cover both or your virgin cunts. You suppose you'll have to switch it around from time to time to" +
						"maximize your chances of remaining fully chaste. That said, <i>who would take your virginity when they can tit-fuck your tits or fuck your butt?</i> You note with a smirk.");
				}
				sb.Append(".");
			}
			//this will only be true when we have multiple vaginas and one is virgin.
			else if (virginVaginas > 0)
			{
				sb.Append("It forms a tight seal over your remaining unclaimed vagina, protecting what remains of your chastity. You're suddenly disappointed that you couldn't remain "
					+ "completely chaste, but confident that you can at least keep your " + vag.LongDescription() + " unsullied. After all, <i>who would take your virginity when they can " +
					"tit-fuck your tits or fuck your butt?</i> You note with a smirk.");
			}
			else
			{
				sb.Append("a tight seal over your previously-claimed cunt. Regret fills you when you realize you could have kept your chastity intact simply by servicing the lusty studs " +
					"and monsters with your ass and tits.");
			}
			if (vag.wetness >= VaginalWetness.SLICK)
			{
				sb.Append(" The moisture you normally drip seems to soak right into the gusset instead of running down your " + wearer.lowerBody.LongDescription() + " like normal, " +
					"giving you a much more chaste appearance in spite of the lewd garments that even now seem to shape your femininity and " + wearer.build.ButtLongDescription() +
					" into perfectly arousing shapes.");
			}

			sb.Append(GlobalStrings.NewParagraph() + "Last is the chain skirt - perhaps the easiest part to put on. It's barely three inches long, such that it exposes your "
				+ wearer.build.ButtLongDescription() + " almost entirely, and when you bend over, fully. The bottom of your vaginal crease can be spied as well, " +
				"and should you desire to show yourself off, a simple stretch or tug would put you completely on display. You wiggle about, " +
				"watching the reflective material ripple almost hypnotically, one hand still on your boobs, mauling at your own tits with passion. " +
				"THIS is how a chaste champion should dress - perfectly modest but full of erotic energy to overwhelm her enemies with!" + GlobalStrings.NewParagraph());

			return sb.ToString();
		}

		public override bool Equals(ArmorBase other)
		{
			return other is LustyMaidensArmor;
		}

		public override double BonusTeaseDamage(Creature wearer)
		{
			if (wearer.genitals.HasVirginVagina())
			{
				return paizuriStacks.add(10);
			}
			else
			{
				return paizuriStacks.add(6);
			}
		}


		//"Chaste" Paizuri - works for most foes with penises.
		public string LustyMaidenPaizuri(Creature wearer, Creature monster)
		{
			if (monster is null || !monster.hasCock)
			{
				return "";
			}

			StringBuilder sb = new StringBuilder();

			sb.Append("You make sure " + monster.Article(true) + monster.name + " is comfortably lying down, " + monster.possessiveNoun + " " + monster.cocks[0].ShortDescription() +
				" exposed to the air");
			if (monster.relativeLust < 50)
			{
				sb.Append(", soft and not yet ready. You purr throatily as you touch the burgeoning boner, tracing your thumb across the sensitive urethral bulge. " +
					"It pulses slowly at your touch, and the base begins to fill with blood, thickening against your palm. You splay your remaining fingers just under the "
					+ monster.cocks[0].HeadDescription() + ", tickling around the glans until that too is flooding with blood, expanding under your caresses until it slowly " +
					"lifts away from " + monster.possessiveNoun + " abdomen.");
			}
			else if (monster.relativeLust < 100)
			{
				sb.Append(", nicely turgid but quite ready to feel the sensuous pleasure of your girls' tight squeeze. You lean over the defeated foe and kiss the rod just under the "
					+ monster.cocks[0].HeadDescription() + ", smiling when it expands under your slow kisses. Your fingers move up to play with the sensitive, " +
					"urethral bulge that runs along the underside, and in no time, " + monster.Article(true) + monster.name + " is hard as a rock, so ready that " +
					monster.possessiveNoun + " member is lifting up on its own.");
			}
			else
			{
				sb.Append(", bouncing with each beat of " + monster.possessiveNoun + " heart, thick beads of pre dribbling from " + monster.possessiveNoun + " tip as you bat " +
					monster.possessiveNoun + " hands away before " + monster.personalNoun + " can waste the load " + monster.personalNounWithHave + "saved up for you.");
			}

			sb.Append(GlobalStrings.NewParagraph() + "Your own moistness has risen to uncomfortable levels, and the sticky seal of your g-string's curvy front panel slips " +
				"oh-so-slightly across your hot, hard clitty, something that makes your " + wearer.lowerBody.LongDescription() + " weak and your arms quake. " +
				"The leather fold on the front of your undergarments is so slippery that each movement has it shifting and shuffling across your nethers, a tiny bit at a time. " +
				"Already, you have your " + wearer.build.ButtLongDescription() + " up in the air, shaking it back and forth for more of the delicious friction. " +
				"The motion only exacerbates the jiggling your " + wearer.genitals.ChestOrAllBreastsShort() + " are doing inside their tight containment. " +
				monster.Article(true).CapitalizeFirstLetter() + monster.name + "'s head tilts up to watch, an unashamedly lusty look overtaking " + monster.possessiveNoun +
				" features as " + monster.personalNoun + " enjoys the inadvertent show you're giving.");

			//count this as a tit fuck, without an orgasm.
			wearer.GetTittyFucked(0, monster.cocks[0], false, false);

			sb.Append(GlobalStrings.NewParagraph() + "\"<i>Such lascivious behavior! I'll have to make sure you're thoroughly purified,</i>\" you state matter-of-factly " +
				"with a feigned serious look on your blushing " + wearer.face.LongDescription() + ". To put proof to your taunt, you grab the throbbing shaft by the base " +
				"and aim it straight up, dropping your " + wearer.genitals.ChestOrAllBreastsShort() + " down on either side. The slippery, self-lubricating leather " +
				"that joins the cups of your sexy, chainmail bra together loops over the top of the " + monster.cocks[0].ShortDescription() + " to properly restrain it, " +
				"pinned in the slick, sweaty valley you call your cleavage. It thrums happily against your " + wearer.body.LongEpidermisDescription() +
				" when you compress the jiggly flesh around it, leaning down to let it feel pleasure that rivals any pussy, no matter how wet or skilled.");

			sb.Append(GlobalStrings.NewParagraph() + "You smile at your defeated foe as you begin to bob over " + monster.objectNoun + ", and you find more words " +
				"coming from your lips without meaning to speak. \"<i>That's better. You really shouldn't go around trying to fuck everyone like that! Pussies are ");

			if (!wearer.genitals.HasVirginVagina())
			{
				sb.Append("a gift too fine for a selfish brute like you");
			}
			else
			{
				sb.Append("sacred and to be shared only with a cherished loved one");
			}

			sb.Append("! Now, I'm going to squeeze all the impure thoughts out of you through your cock, so you just lie there and focus on letting them out all over my breasts.</i>\"");

			sb.Append(GlobalStrings.NewParagraph() + monster.Article(true).CapitalizeFirstLetter() + monster.name + " nods solemnly while " + monster.possessiveNoun +
				" eyes half-cross from pleasure. You bottom out around " + monster.possessiveNoun + " base");

			if (monster.balls.hasBalls)
			{
				sb.Append(" and fondle " + monster.possessiveNoun + " balls one-handed, squeezing the virile orbs, to try and coax out even more of " + monster.possessiveNoun +
					" dirty, perverted thoughts and convert them into salty seed");
			}
			else if (monster.ass.location != AssholeLocation.BUTT)
			{
				sb.Append(" and stroke " + monster.possessiveNoun + " taint, even brushing over the featureless spot where an asshole would normally be, if "
					+ monster.personalNoun + " had one, to try and coax out even more of " + monster.possessiveNoun + " dirty, perverted thoughts and convert them into salty seed");
			}
			else
			{
				sb.Append(" and stroke " + monster.possessiveNoun + " taint, even brushing close to " + monster.possessiveNoun + " asshole in hopes you can coax out even more of "
					+ monster.possessiveNoun + " dirty, perverted thoughts and convert them into salty seed");
			}

			sb.Append(". A startled moan slips out of " + monster.possessiveNoun + " lips, but you're just getting warmed up. You dive down onto " + monster.possessiveNoun + " "
				+ monster.cocks[0].ShortDescription() + ", taking the " + monster.cocks[0].HeadDescription() + " straight into your mouth with a smooth gulp.");
			if (monster.cocks[0].area >= 80)
			{
				sb.Append(" It's so big and strong that it pushes right into your throat, stretching out your neck in the shape of the intruding cock.");
			}

			//count this as oral sex, but don't count it toward an oral orgasm.
			wearer.TakeOralSex(monster.cocks[0], false, false);

			sb.Append(" The strong, pulsing cock feels so good inside your mouth, like it belongs there, and you can't help but think that you're doing a good deed by helping "
				+ monster.Article(true) + monster.name + " empty every last perverse desire onto your purifying breasts.");

			sb.Append(GlobalStrings.NewParagraph() + "Up and down, up and down, you slide across the expansive member with unhurried, slow strokes, each time making your "
				+ wearer.genitals.ChestOrAllBreastsShort() + " bounce beautifully. Your " + wearer.breasts[0].LongNippleDescription() + " are so hard");
			if (wearer.genitals.nippleType == NippleStatus.FUCKABLE || wearer.genitals.currentLactationAmount >= 100)
			{
				sb.Append(", dripping,");
			}

			var vag = wearer.vaginas.FirstOrDefault(x => x.isVirgin) ?? Utils.RandomChoice(wearer.vaginas.ToArray());

			sb.Append(" and sensitive, scraping around the nebulous inner lining of your bikini and occasionally catching on the metal that feels even warmer than normal. " +
				"Behind you, your " + wearer.build.ButtLongDescription() + " is bouncing happily to the rhythm your corruption-devouring breasts have set, " +
				"the thong digging tightly into your " + vag.LongDescription() + " in the most exquisite way. You feel so hot and sensual, but still secure in the knowledge " +
				"that you won't have to worry about such a creature ravaging your ");

			if (wearer.vaginas.All(x => x.isVirgin))
			{
				sb.Append("maidenhead");
			}
			else if (vag.isVirgin)
			{
				sb.Append("remaining unsullied womanhood");
			}
			else
			{
				sb.Append("sloppy gash");
			}

			sb.Append(". Still, you're not sure how much hotter you can get before you're cumming all over your g-string, letting your own dark thoughts seep " +
				"into your magical underwear.");

			sb.Append(GlobalStrings.NewParagraph() + "Below you, " + monster.Article(true) + monster.name + " is moaning out loud and roughly thrusting "
				+ monster.possessiveNoun + " hips to meet your every motion, their tip expanding slightly in your mouth as " + monster.possessiveNoun +
				" passion mounts. You pull back");

			if (monster.cocks[0].area >= 80)
			{
				sb.Append(" with a messy cough to clear your throat");
			}

			sb.Append(" and tease, \"<i>Oh, you're going to cum already, aren't you? Well, go ahead then.</i>\" You pump your " + wearer.genitals.ChestOrAllBreastsShort() +
				" faster against the twitching rod and smile when a thick bead of pre sloughs off into your squishy boobs, smearing across your " +
				wearer.body.LongEpidermisDescription() + ". You kiss it, licking the dollop that slips out of the dilating cum-slit before commanding, \"<i>Cum for me, "
				+ (monster.genitals.AppearsMoreMaleThanFemale() ? "boy" : "girl") + ". Let it allll out.</i>\"");
			sb.Append(GlobalStrings.NewParagraph() + monster.Article(true).CapitalizeFirstLetter() + monster.name + " groans and shakes");
			if (monster.balls.hasBalls)
			{
				sb.Append(", " + monster.possessiveNoun + " balls pumping and bouncing in " + monster.possessiveNoun + " sack");
			}

			sb.Append(", " + monster.possessiveNoun + " urethra swollen with the heavy load about to explode out of it. \"<i>Drain out all that nasty jizz,</i>\" you quip " +
				"as you bottom your breasts down on " + monster.objectNoun + " and slurp the quivering cock-head into your sperm-hungry lips. Salty warmth fires " +
				"in a long rope into your well-prepared mouth and over your tongue. The blissed out look on your captive foe's face combined with the feel of " +
				monster.objectNoun + " giving up all " + monster.possessiveNoun + " naughty thoughts thanks to your cleavage gets you so fucking hot that your " +
				wearer.build.HipsLongDescription() + " begin to shake spastically.");

			sb.Append(GlobalStrings.NewParagraph() + "You do your best to hold on to the pumping cock while it fires spastic ropes into your mouth, but the way your undies " +
				"are digging into your " + vag.LongDescription() + " and grinding across your " + vag.clit.LongDescription() + ", you simply lack the control to keep it up. " +
				"You throw back your head and cry out ecstatically, taking the next ejaculation in a long line across your cheek, up your nose, and onto your forehead. " +
				"Again and again, long ropes of tainted jizz spatter all over your face, dripping messily over the exposed tops of your teats. You lick your lips " +
				"while you cream the inside of your " + wearer.armor.ItemName() + " with girlish love-goo, feeling such immense pleasure at letting your own impure desires " +
				"out into the armor. More jets, weaker than the early ones, crest from the bouncing cock-tip to fall weakly over your well-slicked mammaries.");

			sb.Append(GlobalStrings.NewParagraph() + "You seize " + monster.Article(true) + monster.name + " by " + monster.possessiveNoun + " base and jerk " + monster.objectNoun +
				" off with quick, sharp little strokes, commanding, \"<i>All of it! Give me all of your lusts and cruel desires!</i>\". " +
				(monster.genitals.AppearsMoreMaleThanFemale() ? "His" : "Her") + " back arches as " + monster.possessiveNoun + " orgasm redoubles, and fresh ropes begin to spout out " +
				"again, ensuring your face and breasts are soaked with the sloppy spooge. It runs in moist, warm rivulets into your concealing top, and what doesn't drip down, " +
				"you compulsively rub into your " + wearer.body.ShortEpidermisDescription() + ", feeling a positively healthy glow from the feeling. You don't free the " +
				monster.cocks[0].ShortDescription() + " from your chesty embrace until every single drop is splattered all over you, and when you do, you leave " +
				"a thoroughly wiped-out " + monster.name + " behind you.");

			sb.Append(GlobalStrings.NewParagraph() + "The stink of sperm slowly fades as you move, almost seeming to absorb into your " + wearer.body.ShortEpidermisDescription() +
				". It leaves you with a healthy glow and a surety to your movements, sure that your revealing armor is going to protect you.");

			//Slime feed, succubus feed, minus slight corruption if PC is a virgin, raise sensitivity
			wearer.IngestCum();

			paizuriStacks += 2;
			if (paizuriStacks > 8)
			{
				paizuriStacks = 8;
			}

			wearer.HaveGenericVaginalOrgasm(vag.vaginaIndex, true, true);
			wearer.IncreaseSensitivity(2);
			if (wearer.genitals.HasVirginVagina())
			{
				wearer.DecreaseCorruption();
			}

			return sb.ToString();
		}

		byte ITimeDailyListenerSimple.hourToTrigger => 0;

		string ITimeDailyListenerSimple.ReactToDailyTrigger()
		{
			if (paizuriStacks > 0)
			{
				paizuriStacks--;
			}

			return null;
		}
	}
}
