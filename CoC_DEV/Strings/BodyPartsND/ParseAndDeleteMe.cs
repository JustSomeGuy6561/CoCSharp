////ParseAndDeleteMe.cs
////Description:
////Author: JustSomeGuy
////12/29/2018, 10:15 PM
//using CoC;
//using System;

//public static class ParsingMagic
//{
//	public static void appearance(this Player player)
//	{	
//		if (player.featheryHairPinEquipped())
//		{
//			// This may be relocated into a method later. Probably something, like player.headAccessory()
//			// Note, that earrings count as piercings, meaning, that head accessories and earrings are to be handled seperately
//			string hairPinText = "";
//			hairPinText += "  <b>You have a hair-pin with a single red feather plume";
//			if (player.hair.length > 0)
//				hairPinText += " in your [hair].</b>";
//			else
//				hairPinText += " on your head.</b>";
//			Console.WriteLine(hairPinText);
//		}
//		if (player.jewelryName != "nothing")
//			Console.WriteLine("  <b>Girding one of your fingers is " + player.jewelryName + ".</b>");
//		//Face
		
//		//</mod>
//		//M/F stuff!
//		Console.WriteLine("  It has " + player.faceDesc() + ".");
//		//Eyes
//		if (player.eyes.type == Eyes.SPIDER)
//			Console.WriteLine(" Your eyes are normal, save for their black irises, making them ominous and hypnotizing.");
//		else if (player.eyes.type == Eyes.BLACK_EYES_SAND_TRAP)
//			Console.WriteLine("  Your eyes are solid spheres of inky, alien darkness.");
//		else if (player.eyes.type == Eyes.WOLF)
//			Console.WriteLine("  Your amber eyes are circled by darkness to help keep the sun from obscuring your view and have a second eyelid to keep them wet. You're rather near-sighted, but your peripherals are great!");
//		else if (player.eyes.type == Eyes.COCKATRICE)
//			Console.WriteLine("  You have electric blue eyes spiderwebbed with lightning like streaks that signal their power and slit reptilian pupils."
//					  + " When excited your pupils dilate into wide circles.");
//		else if (player.eyes.type == Eyes.CAT)
//			Console.WriteLine("  Your eyes are similar to those of a cat, with slit pupils.");
//		else if (player.hasReptileEyes())
//		{
//			Console.WriteLine("  Your eyes are");
//			switch (player.eyes.type)
//			{
//				case Eyes.DRAGON: Console.WriteLine(" prideful, fierce dragon eyes with vertically slitted pupils and burning orange irises. They glitter even in the darkness and they"); break;
//				case Eyes.LIZARD: Console.WriteLine(" those of a lizard with vertically slitted pupils and green-yellowish irises. They"); break;
//				case Eyes.BASILISK: Console.WriteLine(" basilisk eyes, grey reptilian pools with vertically slitted pupils. They"); break;
//				default: //Move along
//			}
//			Console.WriteLine(" come with the typical second set of eyelids, allowing you to blink twice as much as others.");
//			if (player.eyes.type == Eyes.BASILISK)
//				Console.WriteLine(" Others seem compelled to look into them.");
//		}
//		if (player.eyes.count > 2)
//			Console.WriteLine(" In addition to your primary two eyes, you have [extraEyesShort] positioned on your forehead.");

//		//Hair
//		//Hair
//		//if bald
//		if (player.hair.length == 0)
//		{
//			if (player.hasFur())
//				Console.WriteLine("  You have no hair, only a thin layer of fur atop of your head.  ");
//			else if (player.hasWool())
//				Console.WriteLine("  You have no hair, only a thin layer of wool atop of your head.  ");
//			else Console.WriteLine("  You are totally bald, showing only shiny [skinTone] [skinDesc] where your hair should be.");
//			switch (player.ears.type)
//			{
//				case Ears.HORSE: Console.WriteLine("  A pair of horse-like ears rise up from the top of your head."); break;
//				case Ears.SHEEP: Console.WriteLine("  Two tear drop shaped ears peek out from the sides of your head, their fluffy texture and lazy positioning giving you a cute and sleepy air."); break;
//				case Ears.FERRET: Console.WriteLine("  Big, [furColor] furred, ferret ears lie atop your head, doing a good job detecting nearby sounds."); break;
//				case Ears.DOG: Console.WriteLine("  A pair of dog ears protrude from your skull, flopping down adorably."); break;
//				case Ears.COW: Console.WriteLine("  A pair of round, floppy cow ears protrude from the sides of your skull."); break;
//				case Ears.ELFIN: Console.WriteLine("  A pair of large pointy ears stick out from your skull."); break;
//				case Ears.CAT: Console.WriteLine("  A pair of cute, fuzzy cat ears have sprouted from the top of your head."); break;
//				case Ears.LIZARD: Console.WriteLine("  A pair of rounded protrusions with small holes on the sides of your head serve as your ears."); break;
//				case Ears.BUNNY: Console.WriteLine("  A pair of floppy rabbit ears stick up from the top of your head, flopping around as you walk."); break;
//				case Ears.FOX: Console.WriteLine("  A pair of large, adept fox ears sit high on your head, always listening."); break;
//				case Ears.DRAGON: Console.WriteLine("  A pair of rounded protrusions with small holes on the sides of your head serve as your ears.  Bony fins sprout behind them."); break;
//				case Ears.RACCOON: Console.WriteLine("  A pair of vaguely egg-shaped, furry raccoon ears adorns your head."); break;
//				case Ears.MOUSE: Console.WriteLine("  A pair of large, dish-shaped mouse ears tops your head."); break;
//				//<mod>
//				case Ears.PIG: Console.WriteLine("  A pair of pointy, floppy pig ears have sprouted from the top of your head."); break;
//				case Ears.RHINO: Console.WriteLine("  A pair of open tubular rhino ears protrude from your head."); break;
//				case Ears.ECHIDNA: Console.WriteLine("  A pair of small rounded openings appear on your head that are your ears."); break;
//				case Ears.DEER: Console.WriteLine("  A pair of deer-like ears rise up from the top of your head."); break;
//				case Ears.WOLF: Console.WriteLine("  A pair of wolf ears stick out from your head, attuned to every sound around you."); break;
//				case Ears.RED_PANDA: Console.WriteLine("  Big, white furred, red-panda ears lie atop your head, keeping you well aware to your surroundings."); break;
//				//</mod>
//				default:
//			}
//			if (player.antennae.type == Antennae.BEE)
//				Console.WriteLine("  Floppy antennae also appear on your skull, bouncing and swaying in the breeze.");
//			else if (player.antennae.type == Antennae.COCKATRICE)
//				Console.WriteLine("  Two long antennae like feathers sit on your hairline, curling over the shape of your head.");
//		}
//		//not bald
//		else
//		{
//			switch (player.ears.type)
//			{
//				case Ears.HUMAN: Console.WriteLine("  Your [hair] looks good on you, accentuating your features well."); break;
//				case Ears.FERRET: Console.WriteLine("  Big, [furColor] furred, ferret ears lie atop your head, doing a good job detecting nearby sounds."); break;
//				case Ears.SHEEP: Console.WriteLine("  Two tear drop shaped ears part your [hair] and peek out from the sides of your head, their fluffy texture and lazy positioning giving you a cute and sleepy air."); break;
//				case Ears.HORSE: Console.WriteLine("  The [hair] on your head parts around a pair of very horse-like ears that grow up from your head."); break;
//				case Ears.DOG: Console.WriteLine("  The [hair] on your head is overlapped by a pair of pointed dog ears."); break;
//				case Ears.COW: Console.WriteLine("  The [hair] on your head is parted by a pair of rounded cow ears that stick out sideways."); break;
//				case Ears.ELFIN: Console.WriteLine("  The [hair] on your head is parted by a pair of cute pointed ears, bigger than your old human ones."); break;
//				case Ears.CAT: Console.WriteLine("  The [hair] on your head is parted by a pair of cute, fuzzy cat ears, sprouting from atop your head and pivoting towards any sudden noises."); break;
//				case Ears.LIZARD: Console.WriteLine("  The [hair] atop your head makes it nigh-impossible to notice the two small rounded openings that are your ears."); break;
//				case Ears.BUNNY: Console.WriteLine("  A pair of floppy rabbit ears stick up out of your [hair], bouncing around as you walk."); break;
//				case Ears.KANGAROO: Console.WriteLine("  The [hair] atop your head is parted by a pair of long, furred kangaroo ears that stick out at an angle."); break;
//				case Ears.FOX: Console.WriteLine("  The [hair] atop your head is parted by a pair of large, adept fox ears that always seem to be listening."); break;
//				case Ears.DRAGON: Console.WriteLine("  The [hair] atop your head is parted by a pair of rounded protrusions with small holes on the sides of your head serve as your ears.  Bony fins sprout behind them."); break;
//				case Ears.RACCOON: Console.WriteLine("  The [hair] on your head parts around a pair of egg-shaped, furry raccoon ears."); break;
//				case Ears.MOUSE: Console.WriteLine("  The [hair] atop your head is funneled between and around a pair of large, dish-shaped mouse ears that stick up prominently."); break;
//				//<mod> Mod-added ears
//				case Ears.PIG: Console.WriteLine("  The [hair] on your head is parted by a pair of pointy, floppy pig ears. They often flick about when you're not thinking about it."); break;
//				case Ears.RHINO: Console.WriteLine("  The [hair] on your head is parted by a pair of tubular rhino ears."); break;
//				case Ears.ECHIDNA: Console.WriteLine("  Your [hair] makes it near-impossible to see the small, rounded openings that are your ears."); break;
//				case Ears.DEER: Console.WriteLine("  The [hair] on your head parts around a pair of deer-like ears that grow up from your head."); break;
//				case Ears.WOLF: Console.WriteLine("  A pair of wolf ears stick out from your head, parting your [hair] and remaining alert to your surroundings."); break;
//				case Ears.RED_PANDA: Console.WriteLine("  Big, white furred, red-panda ears lie atop your head that part your [hair], keeping you well aware to your surroundings."); break;
//				//</mod>
//				default:
//			}
//			if (player.gills.type == Gills.FISH)
//			{
//				Console.WriteLine("  A set of fish like gills reside on your neck, several small slits that can close flat against your skin."
//						   + " They allow you to stay in the water for quite a long time.");
//			}
//			// Gills.ANEMONE are handled below
//			if (player.antennae.type == Antennae.BEE)
//			{
//				if (player.ears.type == Ears.BUNNY)
//					Console.WriteLine("  Limp antennae also grow from just behind your hairline, waving and swaying in the breeze with your ears.");
//				else Console.WriteLine("  Floppy antennae also grow from just behind your hairline, bouncing and swaying in the breeze.");
//			}
//			else if (player.antennae.type == Antennae.COCKATRICE)
//			{
//				Console.WriteLine("  Two long antennae like feathers sit on your hairline, curling over the shape of your head.");
//			}

//		}
//		if (player.ears.type == Ears.COCKATRICE)
//		{
//			Console.WriteLine("  From the sides of your head protrude a quartet of feathers, the longest being vertical while the 3 shorter ones come"
//					  + " out at a 1 o'clock, 2 o'clock and 3 o'clock angle. Behind them hides the avian hole that is your ear.");
//		}

//		//Beards!
//		if (player.beard.length > 0)
//		{
//			Console.WriteLine("  You have a " + player.beardDescript() + " ");
//			if (player.beard.style != Beard.GOATEE)
//			{
//				Console.WriteLine("covering your ");
//				if (rand(2) == 0) Console.WriteLine("jaw");
//				else Console.WriteLine("chin and cheeks")
//				}
//			else
//			{
//				Console.WriteLine("protruding from your chin");
//			}
//			Console.WriteLine(".");
//		}

//		//Tongue
//		switch (player.tongue.type)
//		{
//			case Tongue.SNAKE:
//				Console.WriteLine("  A snake-like tongue occasionally flits between your lips, tasting the air.");
//				break;

//			case Tongue.DEMONIC:
//				Console.WriteLine("  A slowly undulating tongue occasionally slips from between your lips."
//						  + " It hangs nearly two feet long when you let the whole thing slide out, though you can retract it to appear normal.");
//				break;

//			case Tongue.DRACONIC:
//				Console.WriteLine("  Your mouth contains a thick, fleshy tongue that, if you so desire, can telescope to a distance of about four feet."
//						  + " It has sufficient manual dexterity that you can use it almost like a third arm.");
//				break;

//			case Tongue.ECHIDNA:
//				Console.WriteLine("  A thin echidna tongue, at least a foot long, occasionally flits out from between your lips.");
//				break;

//			case Tongue.LIZARD:
//				Console.WriteLine("  Your mouth contains a thick, fleshy lizard tongue, bringing to mind the tongue of large predatory reptiles."
//						  + " It can reach up to one foot, its forked tips tasting the air as they flick at the end of each movement.");
//				break;

//			case Tongue.CAT:
//				Console.WriteLine("  Your tongue is rough like that of a cat. You sometimes groom yourself with it.");
//				break;

//			default:
//		}

//		if (player.horns.type == Horns.IMP)
//		{
//			Console.WriteLine(" A set of pointed imp horns rest atop your head.");
//		}
//		//Demonic horns
//		if (player.horns.type == Horns.DEMON)
//		{
//			if (player.horns.value == 2)
//				Console.WriteLine("  A small pair of pointed horns has broken through the [skinDesc] on your forehead, proclaiming some demonic taint to any who see them.");
//			if (player.horns.value == 4)
//				Console.WriteLine("  A quartet of prominent horns has broken through your [skinDesc].  The back pair are longer, and curve back along your head.  The front pair protrude forward demonically.");
//			if (player.horns.value == 6)
//				Console.WriteLine("  Six horns have sprouted through your [skinDesc], the back two pairs curve backwards over your head and down towards your neck, while the front two horns stand almost " + numInchesOrCentimetres(8) + " long upwards and a little forward.");
//			if (player.horns.value >= 8)
//				Console.WriteLine("  A large number of thick demonic horns sprout through your [skinDesc], each pair sprouting behind the ones before.  The front jut forwards nearly " + numInchesOrCentimetres(10) + " while the rest curve back over your head, some of the points ending just below your ears.  You estimate you have a total of " + num2Text(player.horns.value) + " horns.");
//		}
//		//Minotaur horns
//		if (player.horns.type == Horns.COW_MINOTAUR)
//		{
//			if (player.horns.value < 3)
//				Console.WriteLine("  Two tiny horn-like nubs protrude from your forehead, resembling the horns of the young livestock kept by your village.");
//			if (player.horns.value >= 3 && player.horns.value < 6)
//				Console.WriteLine("  Two moderately sized horns grow from your forehead, similar in size to those on a young bovine.");
//			if (player.horns.value >= 6 && player.horns.value < 12)
//				Console.WriteLine("  Two large horns sprout from your forehead, curving forwards like those of a bull.");
//			if (player.horns.value >= 12 && player.horns.value < 20)
//				Console.WriteLine("  Two very large and dangerous looking horns sprout from your head, curving forward and over a foot long.  They have dangerous looking points.");
//			if (player.horns.value >= 20)
//				Console.WriteLine("  Two huge horns erupt from your forehead, curving outward at first, then forwards.  The weight of them is heavy, and they end in dangerous looking points.");
//		}
//		//Lizard horns
//		if (player.horns.value > 0 && player.horns.type == Horns.DRACONIC_X2)
//		{
//			Console.WriteLine("  A pair of " + numInchesOrCentimetres(player.horns.value) + " horns grow from the sides of your head, sweeping backwards and adding to your imposing visage.");
//		}
//		//Super lizard horns
//		if (player.horns.type == Horns.DRACONIC_X4_12_INCH_LONG)
//			Console.WriteLine("  Two pairs of horns, roughly a foot long, sprout from the sides of your head.  They sweep back and give you a fearsome look, almost like the dragons from your village's legends.");
//		//Antlers!
//		if (player.horns.type == Horns.ANTLERS)
//		{
//			if (player.horns.value > 0)
//				Console.WriteLine("  Two antlers, forking into " + num2Text(player.horns.value) + " points, have sprouted from the top of your head, forming a spiky, regal crown of bone.");
//		}
//		if (player.horns.type == Horns.SHEEP)
//		{
//			if (player.horns.value == 1)
//				Console.WriteLine("  A pair of small sheep horns sit atop your head. They curl out and upwards in a slight crescent shape.");
//			else
//				Console.WriteLine("  A pair of large sheep horns sit atop your head. They curl out and upwards in a crescent shape.");
//		}
//		if (player.horns.type == Horns.RAM)
//		{
//			if (player.horns.value == 1)
//				Console.WriteLine("  A set of " + player.horns.value + " inch ram horns sit atop your head, curling around in a tight spiral at the side of your head before coming to an upwards hook around your ears.");
//			else
//				Console.WriteLine("  A set of large " + player.horns.value + " inch ram horns sit atop your head, curling around in a tight spiral at the side of your head before coming to an upwards hook around your ears.");
//		}

//		if (player.horns.type == Horns.GOAT)
//		{
//			if (player.horns.value == 1)
//				Console.WriteLine("  A pair of stubby goat horns sprout from the sides of your head.");
//			else
//				Console.WriteLine("  A pair of tall-standing goat horns sprout from the sides of your head.  They are curved and patterned with ridges.");
//		}
//		if (player.horns.type == Horns.RHINO)
//		{
//			if (player.horns.value >= 2)
//			{
//				if (player.face.type == Face.RHINO)
//					Console.WriteLine("  A second horn sprouts from your forehead just above the horn on your nose.");
//				else
//					Console.WriteLine("  A single horn sprouts from your forehead.  It is conical and resembles a rhino's horn.");
//				Console.WriteLine("  You estimate it to be about " + numInchesOrCentimetres(7) + " long.");
//			}
//			else
//			{
//				Console.WriteLine("  A single horn sprouts from your forehead.  It is conical and resembles a rhino's horn.  You estimate it to be about " + numInchesOrCentimetres(6) + " long.");
//			}
//		}
//		if (player.horns.type == Horns.UNICORN)
//		{
//			Console.WriteLine("  A single sharp nub of a horn sprouts from the center of your forehead.");
//			if (player.horns.value < 12)
//				Console.WriteLine("  You estimate it to be about " + numInchesOrCentimetres(6) + " long.");
//			else
//				Console.WriteLine("  It has developed its own cute little spiral. You estimate it to be about " + numInchesOrCentimetres(12) + " long, " + numInchesOrCentimetres(2) + " thick and very sturdy. A very useful natural weapon.");
//		}
//		// neckLen
//		if (player.neck.type == Neck.DRACONIC)
//		{
//			// length description
//			if (player.hasDragonNeck())
//				Console.WriteLine("  Your neck starts at the backside of your head and is about two and a half feet long, roughly six inches longer, than your arm length.");
//			else
//			{
//				string lengthText = "";
//				if (player.neck.len < 8) lengthText = "a few inches longer";
//				else if (player.neck.len < 13) lengthText = "somewhat longer";
//				else if (player.neck.len < 18) lengthText = "very long";
//				else lengthText = "extremely long";
//				Console.WriteLine("  Where normal humans have a short neck, yours is " + lengthText + ", measuring " + player.neck.len + " inches.");
//			}

//			// bending your neck
//			if (player.hasDragonNeck())
//				Console.WriteLine("  You manage to bend it in every direction you want and can easily take a look at your back.");
//			else
//			{
//				if (player.neck.len < 10) Console.WriteLine("  You can bend it a bit more than others with some effort.");
//				else if (player.neck.len < 16) Console.WriteLine("  You can bend it more than others with low effort.");
//				else Console.WriteLine("  You are able to bend it in almost every direction and with some effort you even manage to take a glimpse at your back.");
//			}
//		}
//		else if (player.neck.type == Neck.COCKATRICE)
//		{
//			Console.WriteLine("  Around your neck is a ruff of [neckColor] feathers which tends to puff out with your emotions.");
//		}
//		//BODY PG HERE
//		Console.WriteLine("\n\nYou have a humanoid shape with the usual torso, arms, hands, and fingers.");
//		//WINGS!
//		if (player.wings.type == Wings.BEE_LIKE_SMALL)
//			Console.WriteLine("  A pair of tiny-yet-beautiful bee-wings sprout from your back, too small to allow you to fly.");
//		if (player.wings.type == Wings.BEE_LIKE_LARGE)
//			Console.WriteLine("  A pair of large bee-wings sprout from your back, reflecting the light through their clear membranes beautifully.  They flap quickly, allowing you to easily hover in place or fly.");
//		if (player.wings.type == Wings.IMP)
//			Console.WriteLine(" A pair of imp wings sprout from your back, flapping cutely but otherwise being of little use.");
//		if (player.wings.type == Wings.IMP_LARGE)
//			Console.WriteLine(" A pair of large imp wings fold behind your shoulders. With a muscle-twitch, you can extend them, and use them to soar gracefully through the air.");
//		if (player.wings.type == Wings.BAT_LIKE_TINY)
//			Console.WriteLine("  A pair of tiny bat-like demon-wings sprout from your back, flapping cutely, but otherwise being of little use.");
//		if (player.wings.type == Wings.BAT_LIKE_LARGE)
//			Console.WriteLine("  A pair of large bat-like demon-wings fold behind your shoulders.  With a muscle-twitch, you can extend them, and use them to soar gracefully through the air.");
//		if (player.wings.type == Wings.FEATHERED_LARGE)
//			Console.WriteLine("  A pair of large, feathery wings sprout from your back.  Though you usually keep the " + player.wings.color + "-colored wings folded close, they can unfurl to allow you to soar as gracefully as a harpy.");
//		if (player.wings.type == Wings.DRACONIC_SMALL)
//			Console.WriteLine("  Small, vestigial wings sprout from your shoulders. They might look like bat wings,"
//					  + " but the membranes are covered in fine, delicate [wingColor] scales supported by [wingColor2] bones.");
//		else if (player.wings.type == Wings.DRACONIC_LARGE)
//			Console.WriteLine("  Magnificent wings sprout from your shoulders. When unfurled they stretch further than your arm span,"
//					  + " and a single beat of them is all you need to set out toward the sky. They look a bit like bat wings,"
//					  + " but the membranes are covered in fine, delicate [wingColor] scales supported by [wingColor2] bones."
//					  + " A wicked talon juts from the end of each bone.");
//		else if (player.wings.type == Wings.GIANT_DRAGONFLY)
//			Console.WriteLine("  Giant dragonfly wings hang from your shoulders.  At a whim, you could twist them into a whirring rhythm fast enough to lift you off the ground and allow you to fly.");

//		// <mod name="BodyParts.RearBody" author="Stadler76">
//		// rearBody
//		switch (player.rearBody.type)
//		{
//			case RearBody.SHARK_FIN:
//				Console.WriteLine("  A large shark-like fin has sprouted between your shoulder blades."
//						  + " With it you have far more control over swimming underwater.");
//				break;
//			case RearBody.DRACONIC_MANE:
//				Console.WriteLine("  Tracing your spine, a mane of [rearBodyColor] hair grows; starting at the base of your neck and continuing down"
//						  + " your tail, ending on the tip of your tail in a small tuft. It grows in a thick vertical strip,"
//						  + " maybe two inches wide. It reminds you vaguely of a horse's mane.");
//				break;
//			case RearBody.DRACONIC_SPIKES:
//				// Teh spiky mane, similar to the hairy one.
//				Console.WriteLine("  Tracing your spine, a row of short steel-gray and curved backwards spikes protrude; starting at the base of your"
//						  + " neck and continuing down your tail, ending on the tip of your tail. They've grown in a thick vertical strip,"
//						  + " maybe an inch wide and two inches high. It reminds you very vaguely of a horse's mane.");
//				break;
//			default:
//				//Nothing here, move along!
//		}
//		// </mod>

//		// arms

//		//Done with head bits. Move on to body stuff
//		// <mod name="BodyParts.UnderBody" author="Stadler76">
//		if (player.hasCockatriceSkin())
//		{
//			Console.WriteLine("  You've got a thick layer of [furColor] feathers covering your body, while [skinFurScales] coat you from"
//					  + " chest to groin.");
//		}
//		else if (player.hasDifferentUnderBody())
//		{
//			Console.WriteLine("  While most of your body is covered by [skinFurScales] you have [underBody.skinFurScales] covering your belly.");
//		}
//		// </mod>
//		//Horse lowerbody, other lowerbody texts appear lower
//		if (player.isTaur())
//		{
//			if (player.lowerBody.type == LowerBody.HOOFED)
//				Console.WriteLine("  From the waist down you have the body of a horse, with all [legCountText] legs capped by hooves.");
//			else if (player.lowerBody.type == LowerBody.PONY)
//				Console.WriteLine("  From the waist down you have an incredibly cute and cartoonish parody of a horse's body, with all [legCountText] legs ending in flat, rounded feet.");
//			else
//				Console.WriteLine("  Where your legs would normally start you have grown the body of a feral animal, with all [legCountText] legs.");
//		}
//		if (player.isDrider())
//			Console.WriteLine("  Where your legs would normally start you have grown the body of a spider, with [legCountText] spindly legs that sprout from its sides.");
//		//Hip info only displays if you aren't a centaur. 
//		if (!player.isTaur())
//		{
//			if (player.thickness > 70)
//			{
//				Console.WriteLine("  You have [hips]");
//				if (player.hips.rating < 6)
//				{
//					if (player.tone < 65)
//						Console.WriteLine(" buried under a noticeable muffin-top, and");
//					else Console.WriteLine(" that blend into your pillar-like waist, and");
//				}
//				if (player.hips.rating >= 6 && player.hips.rating < 10)
//					Console.WriteLine(" that blend into the rest of your thick form, and");
//				if (player.hips.rating >= 10 && player.hips.rating < 15)
//					Console.WriteLine(" that would be much more noticeable if you weren't so wide-bodied, and");
//				if (player.hips.rating >= 15 && player.hips.rating < 20)
//					Console.WriteLine(" that sway and emphasize your thick, curvy shape, and");
//				if (player.hips.rating >= 20)
//					Console.WriteLine(" that sway hypnotically on your extra-curvy frame, and");
//			}
//			else if (player.thickness < 30)
//			{
//				Console.WriteLine("  You have [hips]");
//				if (player.hips.rating < 6)
//					Console.WriteLine(" that match your trim, lithe body, and");
//				if (player.hips.rating >= 6 && player.hips.rating < 10)
//					Console.WriteLine(" that sway to and fro, emphasized by your trim body, and");
//				if (player.hips.rating >= 10 && player.hips.rating < 15)
//					Console.WriteLine(" that swell out under your trim waistline, and");
//				if (player.hips.rating >= 15 && player.hips.rating < 20)
//					Console.WriteLine(", emphasized by your narrow waist, and");
//				if (player.hips.rating >= 20)
//					Console.WriteLine(" that swell disproportionately wide on your lithe frame, and");
//			}
//			//STANDARD
//			else
//			{
//				Console.WriteLine("  You have [hips]");
//				if (player.hips.rating < 6)
//					Console.WriteLine(", and");
//				if (player.femininity > 50)
//				{
//					if (player.hips.rating >= 6 && player.hips.rating < 10)
//						Console.WriteLine(" that draw the attention of those around you, and");
//					if (player.hips.rating >= 10 && player.hips.rating < 15)
//						Console.WriteLine(" that make you walk with a sexy, swinging gait, and");
//					if (player.hips.rating >= 15 && player.hips.rating < 20)
//						Console.WriteLine(" that make it look like you've birthed many children, and");
//					if (player.hips.rating >= 20)
//						Console.WriteLine(" that make you look more like an animal waiting to be bred than any kind of human, and");
//				}
//				else
//				{
//					if (player.hips.rating >= 6 && player.hips.rating < 10)
//						Console.WriteLine(" that give you a graceful stride, and");
//					if (player.hips.rating >= 10 && player.hips.rating < 15)
//						Console.WriteLine(" that add a little feminine swing to your gait, and");
//					if (player.hips.rating >= 15 && player.hips.rating < 20)
//						Console.WriteLine(" that force you to sway and wiggle as you move, and");
//					if (player.hips.rating >= 20)
//					{
//						Console.WriteLine(" that give your ");
//						if (player.balls > 0)
//							Console.WriteLine("balls plenty of room to breathe");
//						else if (player.hasCock())
//							Console.WriteLine(player.multiCockDescript() + " plenty of room to swing");
//						else if (player.hasVagina())
//							Console.WriteLine(player.vaginaDescript() + " a nice, wide berth");
//						else Console.WriteLine("vacant groin plenty of room");
//						Console.WriteLine(", and");
//					}
//				}
//			}
//		}
//		//ASS
//		//Horse version
//		if (player.isTaur())
//		{
//			//FATBUTT
//			if (player.tone < 65)
//			{
//				Console.WriteLine("  Your [butt]");
//				if (player.butt.rating < 4)
//					Console.WriteLine(" is lean, from what you can see of it.");
//				if (player.butt.rating >= 4 && player.butt.rating < 6)
//					Console.WriteLine(" looks fairly average.");
//				if (player.butt.rating >= 6 && player.butt.rating < 10)
//					Console.WriteLine(" is fairly plump and healthy.");
//				if (player.butt.rating >= 10 && player.butt.rating < 15)
//					Console.WriteLine(" jiggles a bit as you trot around.");
//				if (player.butt.rating >= 15 && player.butt.rating < 20)
//					Console.WriteLine(" jiggles and wobbles as you trot about.");
//				if (player.butt.rating >= 20)
//					Console.WriteLine(" is obscenely large, bordering freakish, even for a horse.");
//			}
//			//GIRL LOOK AT DAT BOOTY
//			else
//			{
//				Console.WriteLine("  Your [butt]");
//				if (player.butt.rating < 4)
//					Console.WriteLine(" is barely noticeable, showing off the muscles of your haunches.");
//				if (player.butt.rating >= 4 && player.butt.rating < 6)
//					Console.WriteLine(" matches your toned equine frame quite well.");
//				if (player.butt.rating >= 6 && player.butt.rating < 10)
//					Console.WriteLine(" gives hints of just how much muscle you could put into a kick.");
//				if (player.butt.rating >= 10 && player.butt.rating < 15)
//					Console.WriteLine(" surges with muscle whenever you trot about.");
//				if (player.butt.rating >= 15 && player.butt.rating < 20)
//					Console.WriteLine(" flexes its considerable mass as you move.");
//				if (player.butt.rating >= 20)
//					Console.WriteLine(" is stacked with layers of muscle, huge even for a horse.");
//			}
//		}
//		//Non-horse PCs
//		else
//		{
//			//TUBBY ASS
//			if (player.tone < 60)
//			{
//				Console.WriteLine(" your [butt]");
//				if (player.butt.rating < 4)
//					Console.WriteLine(" looks great under your gear.");
//				if (player.butt.rating >= 4 && player.butt.rating < 6)
//					Console.WriteLine(" has the barest amount of sexy jiggle.");
//				if (player.butt.rating >= 6 && player.butt.rating < 10)
//					Console.WriteLine(" fills out your clothing nicely.");
//				if (player.butt.rating >= 10 && player.butt.rating < 15)
//					Console.WriteLine(" wobbles enticingly with every step.");
//				if (player.butt.rating >= 15 && player.butt.rating < 20)
//					Console.WriteLine(" wobbles like a bowl full of jello as you walk.");
//				if (player.butt.rating >= 20)
//					Console.WriteLine(" is obscenely large, bordering freakish, and makes it difficult to run.");
//			}
//			//FITBUTT
//			else
//			{
//				Console.WriteLine(" your [butt]");
//				if (player.butt.rating < 4)
//					Console.WriteLine(" molds closely against your form.");
//				if (player.butt.rating >= 4 && player.butt.rating < 6)
//					Console.WriteLine(" contracts with every motion, displaying the detailed curves of its lean musculature.");
//				if (player.butt.rating >= 6 && player.butt.rating < 10)
//					Console.WriteLine(" fills out your clothing nicely.");
//				if (player.butt.rating >= 10 && player.butt.rating < 15)
//					Console.WriteLine(" stretches your gear, flexing it with each step.");
//				if (player.butt.rating >= 15 && player.butt.rating < 20)
//					Console.WriteLine(" threatens to bust out from under your kit each time you clench it.");
//				if (player.butt.rating >= 20)
//					Console.WriteLine(" is marvelously large, but completely stacked with muscle.");
//			}
//		}
//		//TAILS
//		switch (player.tail.type)
//		{
//			case Tail.HORSE:
//				Console.WriteLine("  A long [hairColor] horsetail hangs from your [butt], smooth and shiny.");
//				break;
//			case Tail.FERRET:
//				Console.WriteLine("  Sprouting from your backside, you have a long, bushy tail. It's covered in a fluffy layer of [hairOrFurColor] fur."
//						  + " It twitches and moves happily with your body when you are excited.");
//				break;
//			case Tail.SHEEP:
//				Console.WriteLine("  A fluffy sheep tail hangs down from your [butt]. It occasionally twitches and shakes, its puffy fluff begging to be touched.");
//				break;
//			case Tail.DOG:
//				Console.WriteLine("  A fuzzy [furColor] dogtail sprouts just above your [butt], wagging to and fro whenever you are happy.");
//				break;
//			case Tail.DEMONIC:
//				Console.WriteLine("  A narrow tail ending in a spaded tip curls down from your [butt], wrapping around your [leg] sensually at every opportunity.");
//				break;
//			case Tail.COW:
//				Console.WriteLine("  A long cowtail with a puffy tip swishes back and forth as if swatting at flies.");
//				break;
//			case Tail.SPIDER_ABDOMEN:
//				Console.WriteLine("  A large, spherical spider-abdomen has grown out from your backside, covered in shiny black chitin.  Though it's heavy and bobs with every motion, it doesn't seem to slow you down.");
//				if (player.tail.venom > 50 && player.tail.venom < 80)
//					Console.WriteLine("  Your bulging arachnid posterior feels fairly full of webbing.");
//				if (player.tail.venom >= 80 && player.tail.venom < 100)
//					Console.WriteLine("  Your arachnid rear bulges and feels very full of webbing.");
//				if (player.tail.venom == 100)
//					Console.WriteLine("  Your swollen spider-butt is distended with the sheer amount of webbing it's holding.");
//				break;
//			case Tail.BEE_ABDOMEN:
//				Console.WriteLine("  A large insectile bee-abdomen dangles from just above your backside, bobbing with its own weight as you shift.  It is covered in hard chitin with black and yellow stripes, and tipped with a dagger-like stinger.");
//				if (player.tail.venom > 50 && player.tail.venom < 80)
//					Console.WriteLine("  A single drop of poison hangs from your exposed stinger.");
//				if (player.tail.venom >= 80 && player.tail.venom < 100)
//					Console.WriteLine("  Poisonous bee venom coats your stinger completely.");
//				if (player.tail.venom == 100)
//					Console.WriteLine("  Venom drips from your poisoned stinger regularly.");
//				break;
//			case Tail.SHARK:
//				Console.WriteLine("  A long shark-tail trails down from your backside, swaying to and fro while giving you a dangerous air.");
//				break;
//			case Tail.CAT:
//				Console.WriteLine("  A soft [furColor] cat-tail sprouts just above your [butt], curling and twisting with every step to maintain perfect balance.");
//				break;
//			case Tail.LIZARD:
//				if (player.hasDifferentUnderBody())
//				{
//					Console.WriteLine("  A tapered tail, covered in [skinFurScales] with [underBody.skinFurScales] along its underside hangs down from just"
//							  + " above your [ass].  It sways back and forth, assisting you with keeping your balance.");
//				}
//				else
//				{
//					Console.WriteLine("  A tapered tail hangs down from just above your [ass].  It sways back and forth, assisting you with keeping your balance.");
//				}
//				break;
//			case Tail.SALAMANDER:
//				Console.WriteLine("  A tapered, covered in red scales tail hangs down from just above your [ass].  It sways back and forth, assisting you with keeping your balance. When you are in battle or when you want could set ablaze whole tail in red-hot fire.");
//				break;
//			case Tail.RABBIT:
//				Console.WriteLine("  A short, soft bunny tail sprouts just above your [ass], twitching constantly whenever you don't think about it.");
//				break;
//			case Tail.HARPY:
//				Console.WriteLine("  A tail of feathers fans out from just above your [ass], twitching instinctively to help guide you if you were to take flight.");
//				break;
//			case Tail.KANGAROO:
//				Console.WriteLine("  A conical, ");
//				if (player.hasGooSkin())
//					Console.WriteLine("gooey, [skinTone]");
//				else Console.WriteLine("furry, [furColor]");
//				Console.WriteLine(" tail extends from your [ass], bouncing up and down as you move and helping to counterbalance you.");
//				break;
//			case Tail.FOX:
//				if (player.tail.venom <= 1)
//					Console.WriteLine("  A swishing [hairOrFurColors] fox's brush extends from your [ass], curling around your body - the soft fur feels lovely.");
//				else Console.WriteLine("  " + Num2Text(player.tail.venom) + " swishing [hairOrFurColors] fox's tails extend from your [ass], curling around your body - the soft fur feels lovely.");
//				break;
//			case Tail.DRACONIC:
//				if (player.hasDifferentUnderBody())
//				{
//					Console.WriteLine("  A thick, muscular, reptilian tail covered in [skinFurScales] with [underBody.skinFurScales] along its"
//							  + " underside, almost as long as you are tall, swishes slowly from side to side behind you."
//							  + " Its tip menaces with sharp spikes of bone, and could easily cause serious harm with a good sweep.");
//				}
//				else
//				{
//					Console.WriteLine("  A thick, muscular, reptilian tail, almost as long as you are tall, unconsciously swings behind you slowly"
//							  + " from side to side. Its tip menaces with sharp spikes of bone, and could easily cause grievous harm"
//							  + " with a single, powerful sweep.");
//				}
//				break;
//			case Tail.RACCOON:
//				Console.WriteLine("  A black-and-[furColor]-ringed raccoon tail waves behind you.");
//				break;
//			case Tail.MOUSE:
//				Console.WriteLine("  A naked, [skinTone] mouse tail pokes from your butt, dragging on the ground and twitching occasionally.");
//				break;
//			//<mod>
//			case Tail.BEHEMOTH:
//				Console.WriteLine("  A long seemingly-tapering tail pokes from your butt, ending in spikes just like behemoth's.");
//				break;
//			case Tail.PIG:
//				Console.WriteLine("  A short, curly pig tail sprouts from just above your butt.");
//				break;
//			case Tail.SCORPION:
//				Console.WriteLine("  A chitinous scorpion tail sprouts from just above your butt, ready to dispense venom.");
//				break;
//			case Tail.GOAT:
//				Console.WriteLine("  A very short, stubby goat tail sprouts from just above your butt.");
//				break;
//			case Tail.RHINO:
//				Console.WriteLine("  A ropey rhino tail sprouts from just above your butt, swishing from time to time.");
//				break;
//			case Tail.ECHIDNA:
//				Console.WriteLine("  A stumpy echidna tail forms just about your [ass].");
//				break;
//			case Tail.DEER:
//				Console.WriteLine("  A very short, stubby deer tail sprouts from just above your butt.");
//				break;
//			case Tail.WOLF:
//				Console.WriteLine("  A thick-furred wolf tail hangs above your [ass].");
//				break;
//			case Tail.IMP:
//				Console.WriteLine("  A thin imp tail almost as long as you are tall hangs from above your [butt], dotted at the end with a small puff of hair.");
//				break;
//			case Tail.COCKATRICE:
//				Console.WriteLine("  A thick, scaly, prehensile reptilian tail hangs from your [butt], about half as long as you are tall."
//						  + " The first inch or so is feathered, terminating in a 'v'shape and giving way to your [skinTone] scales.");
//				break;
//			case Tail.RED_PANDA:
//				string tailColors = player.hasFur() ? (player.skin.furColor + " and " + player.redPandaTailColor2()) : "russet and orange";
//				Console.WriteLine("  Sprouting from your backside, you have a long, bushy tail. It has a beautiful pattern of rings in " + tailColors
//											  + "  fluffy fur. It waves playfully as you walk giving to your step a mesmerizing touch.");
//				break;
//			//</mod>
//			default:
//				//Nothing here, move along!
//		}
//		//LOWERBODY SPECIAL
//		switch (player.lowerBody.type)
//		{
//			case LowerBody.HUMAN:
//				Console.WriteLine("  [legCountTextUC] normal human legs grow down from your waist, ending in normal human feet.");
//				break;

//			case LowerBody.FERRET:
//				Console.WriteLine("  Your [legCountText] legs are equally covered in [hairOrFurColor] fur, the lower half having a darker shade."
//						  + " They end on digitigrade ferret paws with short claws.");
//				break;

//			case LowerBody.HOOFED:
//				Console.WriteLine("  Your [legCountText] legs are muscled and jointed oddly, covered in fur, and end in a bestial hooves.");
//				break;

//			case LowerBody.WOLF:
//				Console.WriteLine("  You have [legCountText] digitigrade legs that end in wolf paws.");
//				break;

//			case LowerBody.DOG:
//				Console.WriteLine("  [legCountTextUC] digitigrade legs grow downwards from your waist, ending in dog-like hind-paws.");
//				break;

//			case LowerBody.NAGA:
//				if (player.hasReptileUnderBody(true))
//				{
//					Array nagaColors = ["", ""];
//					if (player.underBody.type == UnderBody.NAGA)
//						nagaColors = [player.underBody.skin.tone, player.nagaLowerBodyColor2()];
//					else
//						nagaColors = [player.skin.tone, player.underBody.skin.tone];
//					Console.WriteLine("  Below your waist, in place of where your legs would be, your body transitions into a long snake like tail."
//							  + " Your snake-like lower body is covered by " + nagaColors[0] + " color scales,"
//							  + " with " + nagaColors[1] + " color ventral scales along your underside.");
//				}
//				else
//					Console.WriteLine("  Below your waist your flesh is fused together into a very long snake-like tail.");
//				break;

//			case LowerBody.DEMONIC_HIGH_HEELS:
//				Console.WriteLine("  Your [legCountText] perfect lissome legs end in mostly human feet, apart from the horn protruding straight down from the heel that forces you to walk with a sexy, swaying gait.");
//				break;

//			case LowerBody.DEMONIC_CLAWS:
//				Console.WriteLine("  Your [legCountText] lithe legs are capped with flexible clawed feet.  Sharp black nails grow where once you had toe-nails, giving you fantastic grip.");
//				break;

//			case LowerBody.BEE:
//				Console.WriteLine("  Your [legCountText] legs are covered in a shimmering insectile carapace up to mid-thigh, looking more like a set of 'fuck-me-boots' than exoskeleton.  A bit of downy yellow and black fur fuzzes your upper thighs, just like a bee.");
//				break;

//			case LowerBody.GOO:
//				Console.WriteLine("  In place of legs you have a shifting amorphous blob.  Thankfully it's quite easy to propel yourself around on.  The lowest portions of your " + player.armorName + " float around inside you, bringing you no discomfort.");
//				break;

//			case LowerBody.CAT:
//				Console.WriteLine("  [legCountTextUC] digitigrade legs grow downwards from your waist, ending in soft, padded cat-paws.");
//				break;

//			case LowerBody.LIZARD:
//				Console.WriteLine("  [legCountTextUC] digitigrade legs grow down from your [hips], ending in clawed feet.  There are three long toes on the front, and a small hind-claw on the back.");
//				break;

//			case LowerBody.SALAMANDER:
//				Console.WriteLine("  [legCountTextUC] digitigrade legs covered in thick, leathery red scales up to the mid-thigh grow down from your [hips], ending in clawed feet.  There are three long toes on the front, and a small hind-claw on the back.");
//				break;

//			case LowerBody.BUNNY:
//				Console.WriteLine("  Your [legCountText] legs thicken below the waist as they turn into soft-furred rabbit-like legs.  You even have large bunny feet that make hopping around a little easier than walking.");
//				break;

//			case LowerBody.HARPY:
//				Console.WriteLine("  Your [legCountText] legs are covered with [furColor] plumage.  Thankfully the thick, powerful thighs are perfect for launching you into the air, and your feet remain mostly human, even if they are two-toed and tipped with talons.");
//				break;

//			case LowerBody.KANGAROO:
//				Console.WriteLine("  Your [legCountText] furry legs have short thighs and long calves, with even longer feet ending in prominently-nailed toes.");
//				break;

//			case LowerBody.CHITINOUS_SPIDER_LEGS:
//				Console.WriteLine("  Your [legCountText] legs are covered in a reflective black, insectile carapace up to your mid-thigh, looking more like a set of 'fuck-me-boots' than exoskeleton.");
//				break;

//			case LowerBody.FOX:
//				Console.WriteLine("  Your [legCountText] legs are crooked into high knees with hocks and long feet, like those of a fox; cute bulbous toes decorate the ends.");
//				break;

//			case LowerBody.DRAGON:
//				Console.WriteLine("  [legCountTextUC] human-like legs grow down from your [hips], sheathed in scales and ending in clawed feet.  There are three long toes on the front, and a small hind-claw on the back.");
//				break;

//			case LowerBody.RACCOON:
//				Console.WriteLine("  Your [legCountText] legs, though covered in fur, are humanlike.  Long feet on the ends bear equally long toes, and the pads on the bottoms are quite sensitive to the touch.");
//				break;

//			case LowerBody.CLOVEN_HOOFED:
//				Console.WriteLine("  [legCountTextUC] digitigrade legs form below your [hips], ending in cloven hooves.");
//				break;

//			case LowerBody.IMP:
//				Console.WriteLine("  [legCountTextUC] digitigrade legs form below your [hips], ending in clawed feet. Three extend out the front, and one smaller one is in the back to keep your balance.");
//				break;

//			case LowerBody.COCKATRICE:
//				Console.WriteLine("  [legCountTextUC] digitigrade legs grow down from your [hips], ending in clawed feet."
//						  + " There are three long toes on the front, and a small hind-claw on the back."
//						  + " A layer of " + (player.hasCockatriceSkin() ? player.skin.furColor : player.hair.color) + " feathers covers your legs from the"
//						  + " hip to the knee, ending in a puffy cuff.");
//				break;

//			case LowerBody.RED_PANDA:
//				Console.WriteLine("  Your [legCountText] legs are equally covered in [if (hasFurryUnderBody)[underBody.furColor]|black-brown] fur,"
//						  + " ending on red-panda paws with short claws. They have a nimble and strong build,"
//						  + " in case you need to escape from something.");
//				break;

//			default:
//				//Nothing here, move along!
//		}
//		if (player.findPerk(PerkLib.Incorporeality) >= 0)
//			Console.WriteLine("  Of course, your [legs] are partially transparent due to their ghostly nature."); // isn't goo transparent anyway?
//		Console.WriteLine("\n");
//		if (player.hasStatusEffect(StatusEffects.GooStuffed))
//		{
//			Console.WriteLine("\n<b>Your gravid-looking belly is absolutely stuffed full of goo. There's no way you can get pregnant like this, but at the same time, you look like some fat-bellied breeder.</b>\n");
//		}












////-------------------------------------------------------------------------------------




//		//Pregnancy Shiiiiiitz
//		if ((player.buttPregnancyType == PregnancyStore.PREGNANCY_FROG_GIRL) || (player.buttPregnancyType == PregnancyStore.PREGNANCY_SATYR) || player.isPregnant())
//		{
//			if (player.pregnancyType == PregnancyStore.PREGNANCY_OVIELIXIR_EGGS)
//			{
//				Console.WriteLine("<b>");
//				//Compute size
//				temp = player.statusEffectv3(StatusEffects.Eggs) + player.statusEffectv2(StatusEffects.Eggs) * 10;
//				if (player.pregnancyIncubation <= 50 && player.pregnancyIncubation > 20)
//				{
//					Console.WriteLine("Your swollen pregnant belly is as large as a ");
//					if (temp < 10)
//						Console.WriteLine("basketball.");
//					if (temp >= 10 && temp < 20)
//						Console.WriteLine("watermelon.");
//					if (temp >= 20)
//						Console.WriteLine("beach ball.");
//				}
//				if (player.pregnancyIncubation <= 20)
//				{
//					Console.WriteLine("Your swollen pregnant belly is as large as a ");
//					if (temp < 10)
//						Console.WriteLine("watermelon.");
//					if (temp >= 10 && temp < 20)
//						Console.WriteLine("beach ball.");
//					if (temp >= 20)
//						Console.WriteLine("large medicine ball.");
//				}
//				Console.WriteLine("</b>");
//				temp = 0;
//			}
//			//Satur preggos - only shows if bigger than regular pregnancy or not pregnancy
//			else if (player.buttPregnancyType == PregnancyStore.PREGNANCY_SATYR && player.buttPregnancyIncubation > player.pregnancyIncubation)
//			{
//				if (player.buttPregnancyIncubation < 125 && player.buttPregnancyIncubation >= 75)
//				{
//					Console.WriteLine("<b>You've got the beginnings of a small pot-belly.</b>");
//				}
//				else if (player.buttPregnancyIncubation >= 50)
//				{
//					Console.WriteLine("<b>The unmistakable bulge of pregnancy is visible in your tummy, yet it feels odd inside you - wrong somehow.</b>");
//				}
//				else if (player.buttPregnancyIncubation >= 30)
//				{
//					Console.WriteLine("<b>Your stomach is painfully distended by your pregnancy, making it difficult to walk normally.</b>");
//				}
//				else
//				{ //Surely Benoit and Cotton deserve their place in this list
//					if (player.pregnancyType == PregnancyStore.PREGNANCY_IZMA || player.pregnancyType == PregnancyStore.PREGNANCY_MOUSE || player.pregnancyType == PregnancyStore.PREGNANCY_AMILY || player.pregnancyType == PregnancyStore.PREGNANCY_JOJO && (flags[kFLAGS.JOJO_STATUS] <= 0 || flags[kFLAGS.JOJO_BIMBO_STATE] >= 3) || player.pregnancyType == PregnancyStore.PREGNANCY_EMBER || player.pregnancyType == PregnancyStore.PREGNANCY_BENOIT || player.pregnancyType == PregnancyStore.PREGNANCY_COTTON || player.pregnancyType == PregnancyStore.PREGNANCY_URTA || player.pregnancyType == PregnancyStore.PREGNANCY_BEHEMOTH)
//						Console.WriteLine("\n<b>Your belly protrudes unnaturally far forward, bulging with the spawn of one of this land's natives.</b>");
//					else if (player.pregnancyType != PregnancyStore.PREGNANCY_MARBLE)
//						Console.WriteLine("\n<b>Your belly protrudes unnaturally far forward, bulging with the unclean spawn of some monster or beast.</b>");
//					else Console.WriteLine("\n<b>Your belly protrudes unnaturally far forward, bulging outwards with Marble's precious child.</b>");
//				}
//			}
//			//URTA PREG
//			else if (player.pregnancyType == PregnancyStore.PREGNANCY_URTA)
//			{
//				if (player.pregnancyIncubation <= 432 && player.pregnancyIncubation > 360)
//				{
//					Console.WriteLine("<b>Your belly is larger than it used to be.</b>\n");
//				}
//				if (player.pregnancyIncubation <= 360 && player.pregnancyIncubation > 288)
//				{
//					Console.WriteLine("<b>Your belly is more noticeably distended.   You're pretty sure it's Urta's.</b>");
//				}
//				if (player.pregnancyIncubation <= 288 && player.pregnancyIncubation > 216)
//				{
//					Console.WriteLine("<b>The unmistakable bulge of pregnancy is visible in your tummy, and the baby within is kicking nowadays.</b>");
//				}
//				if (player.pregnancyIncubation <= 216 && player.pregnancyIncubation > 144)
//				{
//					Console.WriteLine("<b>Your belly is large and very obviously pregnant to anyone who looks at you.  It's gotten heavy enough to be a pain to carry around all the time.</b>");
//				}
//				if (player.pregnancyIncubation <= 144 && player.pregnancyIncubation > 72)
//				{
//					Console.WriteLine("<b>It would be impossible to conceal your growing pregnancy from anyone who glanced your way.  It's large and round, frequently moving.</b>");
//				}
//				if (player.pregnancyIncubation <= 72 && player.pregnancyIncubation > 48)
//				{
//					Console.WriteLine("<b>Your stomach is painfully distended by your pregnancy, making it difficult to walk normally.</b>");
//				}
//				if (player.pregnancyIncubation <= 48)
//				{
//					Console.WriteLine("\n<b>Your belly protrudes unnaturally far forward, bulging with the spawn of one of this land's natives.</b>");
//				}
//			}
//			else if (player.buttPregnancyType == PregnancyStore.PREGNANCY_FROG_GIRL)
//			{
//				if (player.buttPregnancyIncubation >= 8)
//					Console.WriteLine("<b>Your stomach is so full of frog eggs that you look about to birth at any moment, your belly wobbling and shaking with every step you take, packed with frog ovum.</b>");
//				else Console.WriteLine("<b>You're stuffed so full with eggs that your belly looks obscenely distended, huge and weighted with the gargantuan eggs crowding your gut. They make your gait a waddle and your gravid tummy wobble obscenely.</b>");
//			}
//			else if (player.pregnancyType == PregnancyStore.PREGNANCY_FAERIE)
//			{ //Belly size remains constant throughout the pregnancy
//				Console.WriteLine("<b>Your belly remains swollen like a watermelon. ");
//				if (player.pregnancyIncubation <= 100)
//					Console.WriteLine("It's full of liquid, though unlike a normal pregnancy the passenger you're carrying is tiny.</b>");
//				else if (player.pregnancyIncubation <= 140)
//					Console.WriteLine("It feels like it's full of thick syrup or jelly.</b>");
//				else Console.WriteLine("It still feels like there's a solid ball inside your womb.</b>");
//			}
//			else
//			{
//				if (player.pregnancyIncubation <= 336 && player.pregnancyIncubation > 280)
//				{
//					Console.WriteLine("<b>Your belly is larger than it used to be.</b>");
//				}
//				if (player.pregnancyIncubation <= 280 && player.pregnancyIncubation > 216)
//				{
//					Console.WriteLine("<b>Your belly is more noticeably distended.   You are probably pregnant.</b>");
//				}
//				if (player.pregnancyIncubation <= 216 && player.pregnancyIncubation > 180)
//				{
//					Console.WriteLine("<b>The unmistakable bulge of pregnancy is visible in your tummy.</b>");
//				}
//				if (player.pregnancyIncubation <= 180 && player.pregnancyIncubation > 120)
//				{
//					Console.WriteLine("<b>Your belly is very obviously pregnant to anyone who looks at you.</b>");
//				}
//				if (player.pregnancyIncubation <= 120 && player.pregnancyIncubation > 72)
//				{
//					Console.WriteLine("<b>It would be impossible to conceal your growing pregnancy from anyone who glanced your way.</b>");
//				}
//				if (player.pregnancyIncubation <= 72 && player.pregnancyIncubation > 48)
//				{
//					Console.WriteLine("<b>Your stomach is painfully distended by your pregnancy, making it difficult to walk normally.</b>");
//				}
//				if (player.pregnancyIncubation <= 48)
//				{ //Surely Benoit and Cotton deserve their place in this list
//					if (player.pregnancyType == PregnancyStore.PREGNANCY_IZMA || player.pregnancyType == PregnancyStore.PREGNANCY_MOUSE || player.pregnancyType == PregnancyStore.PREGNANCY_AMILY || (player.pregnancyType == PregnancyStore.PREGNANCY_JOJO && flags[kFLAGS.JOJO_STATUS] <= 0) || player.pregnancyType == PregnancyStore.PREGNANCY_EMBER || player.pregnancyType == PregnancyStore.PREGNANCY_BENOIT || player.pregnancyType == PregnancyStore.PREGNANCY_COTTON || player.pregnancyType == PregnancyStore.PREGNANCY_URTA || player.pregnancyType == PregnancyStore.PREGNANCY_MINERVA || player.pregnancyType == PregnancyStore.PREGNANCY_BEHEMOTH)
//						Console.WriteLine("\n<b>Your belly protrudes unnaturally far forward, bulging with the spawn of one of this land's natives.</b>");
//					else if (player.pregnancyType != PregnancyStore.PREGNANCY_MARBLE)
//						Console.WriteLine("\n<b>Your belly protrudes unnaturally far forward, bulging with the unclean spawn of some monster or beast.</b>");
//					else Console.WriteLine("\n<b>Your belly protrudes unnaturally far forward, bulging outwards with Marble's precious child.</b>");
//				}
//			}
//			Console.WriteLine("\n");
//		}

////---------------------------------------------------------------------------------------------------



//		Console.WriteLine("\n");
//		if (player.gills.type == Gills.ANEMONE)
//			Console.WriteLine("A pair of feathery gills are growing out just below your neck, spreading out horizontally and draping down your chest.  They allow you to stay in the water for quite a long time.  ");
//		//Chesticles..I mean bewbz.
//		if (player.breastRows.length == 1)
//		{
//			Console.WriteLine("You have " + num2Text(player.breastRows[temp].breasts) + " " + player.breastDescript(temp) + ", each supporting ");
//			Console.WriteLine(player.breastRows[temp].nipplesPerBreast == 1 ? "a" : num2Text(player.breastRows[temp].nipplesPerBreast)); //Number of nipples.
//			Console.WriteLine(" " + shortSuffix(player.nippleLength) + " ");         // Length of nipples
//			Console.WriteLine(player.nippleDescript(temp) + (player.breastRows[0].nipplesPerBreast == 1 ? "." : "s.")); //Nipple description and plural
//			if (player.breastRows[0].milkFullness > 75)
//				Console.WriteLine("  Your " + player.breastDescript(temp) + " are painful and sensitive from being so stuffed with milk.  You should release the pressure soon.");
//			if (player.breastRows[0].breastRating >= 1)
//				Console.WriteLine("  You could easily fill a " + player.breastCup(temp) + " bra.");
//			//Done with tits.  Move on.
//			Console.WriteLine("\n");
//		}
//		//many rows
//		else
//		{
//			Console.WriteLine("You have " + num2Text(player.breastRows.length) + " rows of breasts, the topmost pair starting at your chest.\n");
//			while (temp < player.breastRows.length)
//			{
//				if (temp == 0)
//					Console.WriteLine("--Your uppermost rack houses ");
//				if (temp == 1)
//					Console.WriteLine("\n--The second row holds ");
//				if (temp == 2)
//					Console.WriteLine("\n--Your third row of breasts contains ");
//				if (temp == 3)
//					Console.WriteLine("\n--Your fourth set of tits cradles ");
//				if (temp == 4)
//					Console.WriteLine("\n--Your fifth and final mammary grouping swells with ");
//				Console.WriteLine(num2Text(player.breastRows[temp].breasts) + " " + player.breastDescript(temp) + " with ");
//				Console.WriteLine(num2Text(player.breastRows[temp].nipplesPerBreast)); //Number of nipples per breast
//				Console.WriteLine(" " + shortSuffix(player.nippleLength) + " ");       // Length of nipples
//				Console.WriteLine(player.nippleDescript(temp) + (player.breastRows[0].nipplesPerBreast == 1 ? " each." : "s each.")); //Description and Plural
//				if (player.breastRows[temp].breastRating >= 1)
//					Console.WriteLine("  They could easily fill a " + player.breastCup(temp) + " bra.");
//				if (player.breastRows[temp].milkFullness > 75)
//					Console.WriteLine("  Your " + player.breastDescript(temp) + " are painful and sensitive from being so stuffed with milk.  You should release the pressure soon.");
//				temp++;
//			}
//			//Done with tits.  Move on.
//			Console.WriteLine("\n");
//		}
		
////---------------------------------------------------------------------------

//		//Crotchial stuff - mention snake
//		if (player.lowerBody.type == LowerBody.NAGA && player.gender > 0)
//		{
//			Console.WriteLine("\nYour sex");
//			if (player.gender == 3 || player.cocks.length > 1)
//				Console.WriteLine("es are ");
//			else Console.WriteLine(" is ");
//			Console.WriteLine("concealed within a cavity in your tail when not in use, though when the need arises, you can part your concealing slit and reveal your true self.\n");
//		}
////----------------------------------------------------------------------------
//		// Cock Descriptions //
//		if (player.hasCock())
//		{
//			rando = rand(100);

//			// Is taur and has multiple cocks?
//			if (player.isTaur() && player.cocks.length == 1)
//				Console.WriteLine("\nYour equipment has shifted to lie between your hind legs, like a feral animal.");
//			else if (player.isTaur())
//				Console.WriteLine("\nBetween your hind legs, you have grown " + player.multiCockDescript() + "!\n");
//			else if (player.cocks.length == 1)
//				Console.WriteLine("\n");
//			else
//				Console.WriteLine("\nWhere a penis would normally be located, you have instead grown " + player.multiCockDescript() + "!\n");

//			for (int cock_index = 0; cock_index < player.cocks.length; cock_index++)
//			{
//				rando++;

//				// How to start the sentence?
//				if (player.cocks.length == 1) Console.WriteLine("Your ");
//				else if (cock_index == 0) Console.WriteLine("--Your first ");
//				else if (rando % 5 == 0) Console.WriteLine("--The next ");
//				else if (rando % 5 == 1) Console.WriteLine("--The " + num2Text2(cock_index + 1) + " of your ");
//				else if (rando % 5 == 2) Console.WriteLine("--One of your ");
//				else if (rando % 5 == 3) Console.WriteLine("--The " + num2Text2(cock_index + 1) + " ");
//				else if (rando % 5 == 4) Console.WriteLine("--Another of your ");

//				// How large?
//				Console.WriteLine(player.cockDescript(cock_index) + ((rando % 5) % 3 == 0 || cock_index == 0 ? "" : "s") + " is " + inchesOrCentimetres(player.cocks[cock_index].cockLength) + " long and ");
//				Console.WriteLine(inchesOrCentimetres(player.cocks[cock_index].cockThickness));
//				if (rando % 3 == 0) Console.WriteLine(" wide.");
//				else if (rando % 3 == 1) Console.WriteLine(" thick.");
//				else if (rando % 3 == 2) Console.WriteLine(" in diameter.");

//				// What flavor of cock do you have?
//				switch (player.cocks[cock_index].cockType)
//				{
//					case CockTypesEnum.HORSE: Console.WriteLine("  It's mottled black and brown in a very animalistic pattern.  The 'head' of its shaft flares proudly, just like a horse's."); break;
//					case CockTypesEnum.DOG: Console.WriteLine("  It is shiny, pointed, and covered in veins, just like a large dog's cock."); break;
//					case CockTypesEnum.WOLF: Console.WriteLine("  It is shiny red, pointed, and covered in veins, just like a large wolf's cock."); break;
//					case CockTypesEnum.FOX: Console.WriteLine("  It is shiny, pointed, and covered in veins, just like a large fox's cock."); break;
//					case CockTypesEnum.DEMON: Console.WriteLine("  The crown is ringed with a circle of rubbery protrusions that grow larger as you get more aroused.  The entire thing is shiny and covered with tiny, sensitive nodules that leave no doubt about its demonic origins."); break;
//					case CockTypesEnum.TENTACLE: Console.WriteLine("  The entirety of its green surface is covered in perspiring beads of slick moisture.  It frequently shifts and moves of its own volition, the slightly oversized and mushroom-like head shifting in coloration to purplish-red whenever you become aroused."); break;
//					case CockTypesEnum.CAT: Console.WriteLine("  It ends in a single point, much like a spike, and is covered in small, fleshy barbs. The barbs are larger at the base and shrink in size as they get closer to the tip.  Each of the spines is soft and flexible, and shouldn't be painful for any of your partners."); break;
//					case CockTypesEnum.LIZARD: Console.WriteLine("  It's a deep, iridescent purple in color.  Unlike a human penis, the shaft is not smooth, and is instead patterned with multiple bulbous bumps."); break;
//					case CockTypesEnum.ANEMONE: Console.WriteLine("  The crown is surrounded by tiny tentacles with a venomous, aphrodisiac payload.  At its base a number of similar, longer tentacles have formed, guaranteeing that pleasure will be forced upon your partners."); break;
//					case CockTypesEnum.KANGAROO: Console.WriteLine("  It usually lies coiled inside a sheath, but undulates gently and tapers to a point when erect, somewhat like a taproot."); break;
//					case CockTypesEnum.DRAGON: Console.WriteLine("  With its tapered tip, there are few holes you wouldn't be able to get into.  It has a strange, knot-like bulb at its base, but doesn't usually flare during arousal as a dog's knot would."); break;
//					case CockTypesEnum.BEE: Console.WriteLine("  It's a long, smooth black shaft that's rigid to the touch.  Its base is ringed with a layer of " + shortSuffix(4) + " long soft bee hair.  The tip has a much finer layer of short yellow hairs.  The tip is very sensitive, and it hurts constantly if you don't have bee honey on it."); break;
//					case CockTypesEnum.PIG: Console.WriteLine("  It's bright pinkish red, ending in a prominent corkscrew shape at the tip."); break;
//					case CockTypesEnum.AVIAN: Console.WriteLine("  It's a red, tapered cock that ends in a tip.  It rests nicely in a sheath."); break;
//					case CockTypesEnum.RHINO: Console.WriteLine("  It's a smooth, tough pink colored and takes on a long and narrow shape with an oval shaped bulge along the center."); break;
//					case CockTypesEnum.ECHIDNA: Console.WriteLine("  It is quite a sight to behold, coming well-equipped with four heads."); break;
//					case CockTypesEnum.RED_PANDA: Console.WriteLine("  It lies protected in a soft, fuzzy sheath."); break;
//					default: //Nothing here, move along!
//				}

//				// Knot?
//				if (player.cocks[cock_index].knotMultiplier > 1)
//				{
//					if (player.cocks[cock_index].knotMultiplier >= 1.8)
//						Console.WriteLine("  The obscenely swollen lump of flesh near the base of your " + player.cockDescript(cock_index) + " looks almost comically mismatched for your cock.");
//					else if (player.cocks[cock_index].knotMultiplier >= 1.4)
//						Console.WriteLine("  A large bulge of flesh nestles just above the bottom of your " + player.cockDescript(cock_index) + ", to ensure it stays where it belongs during mating.");
//					else // knotMultiplier < 1.4
//						Console.WriteLine("  A small knot of thicker flesh is near the base of your " + player.cockDescript(cock_index) + ", ready to expand to help you lodge it inside a female.");
//					Console.WriteLine("  The knot is " + inchesOrCentimetres(player.cocks[cock_index].cockThickness * player.cocks[cock_index].knotMultiplier) + " thick when at full size.");
//				}

//				// Sock Flavor
//				if (player.cocks[cock_index].sock != "" && player.cocks[cock_index].sock != null)
//				{
//					// I dunno what was happening, but it looks like .sock is null, as it doesn't exist. I guess this is probably more left over from some of the restucturing.
//					// Anyways, check against null values, and stuff works again.
//					//trace("Found a sock description (WTF even is a sock?)", player.cocks[cock_index].sock);
//					sockDescript(cock_index);
//				}
//				Console.WriteLine("\n");
//			}

//			//Worm flavor
//			if (player.hasStatusEffect(StatusEffects.Infested))
//				Console.WriteLine("Every now and again slimy worms coated in spunk slip partway out of your " + player.multiCockDescriptLight() + ", tasting the air like tongues of snakes.\n");
//		}
////-------------------------------------------

//		//Of Balls and Sacks!
//		if (player.balls > 0)
//		{
//			if (player.hasStatusEffect(StatusEffects.Uniball))
//			{
//				if (player.hasGooSkin())
//					Console.WriteLine("Your [sack] clings tightly to your groin, dripping and holding " + player.ballsDescript() + " snugly against you.");
//				else
//					Console.WriteLine("Your [sack] clings tightly to your groin, holding " + player.ballsDescript() + " snugly against you.");
//			}
//			else if (player.cocks.length == 0)
//			{
//				if (player.hasPlainSkin())
//					Console.WriteLine("A " + player.sackDescript() + " with " + player.ballsDescript() + " swings heavily under where a penis would normally grow.");
//				if (player.hasFur())
//					Console.WriteLine("A fuzzy " + player.sackDescript() + " filled with " + player.ballsDescript() + " swings low under where a penis would normally grow.");
//				if (player.hasScales())
//					Console.WriteLine("A scaley " + player.sackDescript() + " hugs your " + player.ballsDescript() + " tightly against your body.");
//				if (player.hasGooSkin())
//					Console.WriteLine("An oozing, semi-solid sack with " + player.ballsDescript() + " swings heavily under where a penis would normally grow.");
//			}
//			else
//			{
//				if (player.hasPlainSkin())
//					Console.WriteLine("A " + player.sackDescript() + " with " + player.ballsDescript() + " swings heavily beneath your " + player.multiCockDescriptLight() + ".");
//				if (player.hasFur())
//					Console.WriteLine("A fuzzy " + player.sackDescript() + " filled with " + player.ballsDescript() + " swings low under your " + player.multiCockDescriptLight() + ".");
//				if (player.hasScales())
//					Console.WriteLine("A scaley " + player.sackDescript() + " hugs your " + player.ballsDescript() + " tightly against your body.");
//				if (player.hasGooSkin())
//					Console.WriteLine("An oozing, semi-solid sack with " + player.ballsDescript() + " swings heavily beneath your " + player.multiCockDescriptLight() + ".");
//			}
//			Console.WriteLine("  You estimate each of them to be about " + numInchesOrCentimetres(player.ballSize) + " across\n");
//		}

////-------------------------------------------------------------------------------------------------------
//		//VAGOOZ
//		if (player.vaginas.length > 0)
//		{
//			if (player.gender == 2 && player.isTaur())
//				Console.WriteLine("\nYour womanly parts have shifted to lie between your hind legs, in a rather feral fashion.");
//			Console.WriteLine("\n");
//			if (player.vaginas.length == 1)
//				Console.WriteLine("You have a " + player.vaginaDescript(0) + ", with a " + inchesOrCentimetres(player.getClitLength()) + " clit");
//			if (player.vaginas[0].virgin)
//				Console.WriteLine(" and an intact hymen"); // Wait, won't this fuck up, with multiple vaginas?
//			Console.WriteLine(".  ");
//			if (player.vaginas.length > 1)
//				Console.WriteLine("You have " + player.vaginas.length + " " + player.vaginaDescript(0) + "s, with " + inchesOrCentimetres(player.getClitLength()) + "-centimetre clits each.  ");
//			if (player.lib100 < 50 && player.lust100 < 50) //not particularly horny

//			{
//				//Wetness
//				if (player.vaginas[0].vaginalWetness >= Vagina.WETNESS_WET && player.vaginas[0].vaginalWetness < Vagina.WETNESS_DROOLING)
//					Console.WriteLine("Moisture gleams in ");
//				if (player.vaginas[0].vaginalWetness >= Vagina.WETNESS_DROOLING)
//				{
//					Console.WriteLine("Occasional beads of ");
//					Console.WriteLine("lubricant drip from ");
//				}
//				//Different description based on vag looseness
//				if (player.vaginas[0].vaginalWetness >= Vagina.WETNESS_WET)
//				{
//					if (player.vaginas[0].vaginalLooseness < Vagina.LOOSENESS_LOOSE)
//						Console.WriteLine("your " + player.vaginaDescript(0) + ". ");
//					if (player.vaginas[0].vaginalLooseness >= Vagina.LOOSENESS_LOOSE && player.vaginas[0].vaginalLooseness < Vagina.LOOSENESS_GAPING_WIDE)
//						Console.WriteLine("your " + player.vaginaDescript(0) + ", its lips slightly parted. ");
//					if (player.vaginas[0].vaginalLooseness >= Vagina.LOOSENESS_GAPING_WIDE)
//						Console.WriteLine("the massive hole that is your " + player.vaginaDescript(0) + ".  ");
//				}
//			}
//			if ((player.lib100 >= 50 || player.lust100 >= 50) && (player.lib100 < 80 && player.lust100 < 80)) //kinda horny

//			{
//				//Wetness
//				if (player.vaginas[0].vaginalWetness < Vagina.WETNESS_WET)
//					Console.WriteLine("Moisture gleams in ");
//				if (player.vaginas[0].vaginalWetness >= Vagina.WETNESS_WET && player.vaginas[0].vaginalWetness < Vagina.WETNESS_DROOLING)
//				{
//					Console.WriteLine("Occasional beads of ");
//					Console.WriteLine("lubricant drip from ");
//				}
//				if (player.vaginas[0].vaginalWetness >= Vagina.WETNESS_DROOLING)
//				{
//					Console.WriteLine("Thin streams of ");
//					Console.WriteLine("lubricant occasionally dribble from ");
//				}
//				//Different description based on vag looseness
//				if (player.vaginas[0].vaginalLooseness < Vagina.LOOSENESS_LOOSE)
//					Console.WriteLine("your " + player.vaginaDescript(0) + ". ");
//				if (player.vaginas[0].vaginalLooseness >= Vagina.LOOSENESS_LOOSE && player.vaginas[0].vaginalLooseness < Vagina.LOOSENESS_GAPING_WIDE)
//					Console.WriteLine("your " + player.vaginaDescript(0) + ", its lips slightly parted. ");
//				if (player.vaginas[0].vaginalLooseness >= Vagina.LOOSENESS_GAPING_WIDE)
//					Console.WriteLine("the massive hole that is your " + player.vaginaDescript(0) + ".  ");
//			}
//			if ((player.lib100 > 80 || player.lust100 > 80)) //WTF horny!

//			{
//				//Wetness
//				if (player.vaginas[0].vaginalWetness < Vagina.WETNESS_WET)

//				{
//					Console.WriteLine("Occasional beads of ");
//					Console.WriteLine("lubricant drip from ");
//				}
//				if (player.vaginas[0].vaginalWetness >= Vagina.WETNESS_WET && player.vaginas[0].vaginalWetness < Vagina.WETNESS_DROOLING)

//				{
//					Console.WriteLine("Thin streams of ");
//					Console.WriteLine("lubricant occasionally dribble from ");
//				}
//				if (player.vaginas[0].vaginalWetness >= Vagina.WETNESS_DROOLING)

//				{
//					Console.WriteLine("Thick streams of ");
//					Console.WriteLine("lubricant drool constantly from ");
//				}
//				//Different description based on vag looseness
//				if (player.vaginas[0].vaginalLooseness < Vagina.LOOSENESS_LOOSE)
//					Console.WriteLine("your " + player.vaginaDescript(0) + ". ");
//				if (player.vaginas[0].vaginalLooseness >= Vagina.LOOSENESS_LOOSE && player.vaginas[0].vaginalLooseness < Vagina.LOOSENESS_GAPING_WIDE)
//					Console.WriteLine("your " + player.vaginaDescript(0) + ", its lips slightly parted. ");
//				if (player.vaginas[0].vaginalLooseness >= Vagina.LOOSENESS_GAPING_WIDE)
//					Console.WriteLine("the massive hole that is your cunt.  ");
//			}
//			//Line Drop for next descript!
//			Console.WriteLine("\n");
//		}
//		//Genderless lovun'
//		if (player.cocks.length == 0 && player.vaginas.length == 0)
//			Console.WriteLine("\nYou have a curious lack of any sexual endowments.\n");

////-----------------------------------------------------
//		//BUNGHOLIO
//		if (player.ass)
//		{
//			Console.WriteLine("\n");
//			Console.WriteLine("You have one " + player.assholeDescript() + ", placed between your butt-cheeks where it belongs.\n");
//		}

////-----------------------------------------------------
//		//Piercings!
//		if (player.eyebrowPierced > 0)
//			Console.WriteLine("\nA solitary " + player.eyebrowPShort + " adorns your eyebrow, looking very stylish.");
//		if (player.earsPierced > 0)
//			Console.WriteLine("\nYour ears are pierced with " + player.earsPShort + ".");
//		if (player.nosePierced > 0)
//			Console.WriteLine("\nA " + player.nosePShort + " dangles from your nose.");
//		if (player.lipPierced > 0)
//			Console.WriteLine("\nShining on your lip, a " + player.lipPShort + " is plainly visible.");
//		if (player.tonguePierced > 0)
//			Console.WriteLine("\nThough not visible, you can plainly feel your " + player.tonguePShort + " secured in your tongue.");
//		if (player.nipplesPierced == 3)
//			Console.WriteLine("\nYour " + player.nippleDescript(0) + "s ache and tingle with every step, as your heavy " + player.nipplesPShort + " swings back and forth.");
//		else if (player.nipplesPierced > 0)
//			Console.WriteLine("\nYour " + player.nippleDescript(0) + "s are pierced with " + player.nipplesPShort + ".");
//		if (player.cocks.length > 0)
//		{
//			if (player.cocks[0].pierced > 0)
//			{
//				Console.WriteLine("\nLooking positively perverse, a " + player.cocks[0].pShortDesc + " adorns your " + player.cockDescript(0) + ".");
//			}
//		}
//		if (flags[kFLAGS.CERAPH_BELLYBUTTON_PIERCING] == 1)
//			Console.WriteLine("\nA magical, ruby-studded bar pierces your belly button, allowing you to summon Ceraph on a whim.");
//		if (player.hasVagina())
//		{
//			if (player.vaginas[0].labiaPierced > 0)
//				Console.WriteLine("\nYour " + player.vaginaDescript(0) + " glitters with the " + player.vaginas[0].labiaPShort + " hanging from your lips.");
//			if (player.vaginas[0].clitPierced > 0)
//				Console.WriteLine("\nImpossible to ignore, your " + player.clitDescript() + " glitters with its " + player.vaginas[0].clitPShort + ".");
//		}

////------------------------------------------------------
//		//MONEY!
//		if (player.gems == 0)
//			Console.WriteLine("\n\n<b>Your money-purse is devoid of any currency.</b>");
//		else if (player.gems == 1)
//			Console.WriteLine("\n\n<b>You have " + addComma(Math.floor(player.gems)) + " shining gem, collected in your travels.</b>");
//		else if (player.gems > 1)
//			Console.WriteLine("\n\n<b>You have " + addComma(Math.floor(player.gems)) + " shining gems, collected in your travels.</b>");
//	}
//}
