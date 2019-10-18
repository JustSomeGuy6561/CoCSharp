using CoC.Backend;
using CoC.Backend.BodyParts;
using CoC.Backend.Tools;
using CoC.Frontend.Items.Consumables;
using System;
using System.Text;

namespace CoC.Frontend.Pregnancies
{
	public partial class PlayerEggPregnancy
	{
		private static string EggDesc()
		{
			return "Egg Pregnancy";
		}

		private static string EggSource()
		{
			return "eggs";
		}

		private string EggBiggerStr()
		{
			return Environment.NewLine + Environment.NewLine + "Your pregnant belly suddenly feels heavier and more bloated than before." +
				"You wonder what the elixir just did.";
		}

		private string EggMoreStr()
		{
			return Environment.NewLine + Environment.NewLine + "A rumble radiates from your uterus as it shifts uncomfortably and your belly gets a bit larger.";
		}

		private string BirthStr(bool gainsOviMax, EggBase eggsLaid)
		{
			StringBuilder sb = new StringBuilder();

			//if (gainsOviMax)
			//{
			//	sb.Append("You instantly feel your body seize up and you know something is wrong."
			//			 + " [if (hasWeapon)You let go of your [weapon] before your|Your] legs completely give out from under you and"
			//			 + " a high pitched, death curdle escapes your lips as you fall to your knees. Clutching your stomach,"
			//			 + " you bury your face into the ground, your screaming turning into a violent high pitched wail."
			//			 + " Deep inside your uterus you feel a shuddering, inhuman change as your womb violently and painfully,"
			//			 + " shifts and warps around your unfertilized eggs, becoming a more accommodating, cavernous home for them."
			//			 + " Your wails quieted down and became a mess of heaving sighs and groans. Your eyes weakly register as your belly"
			//			 + " trembles with a vengeance, and you realize there is still more to come." + Environment.NewLine + Environment.NewLine);

			//	if (mother.armor !== ArmorLib.NOTHING)
			//	{
			//		sb.Append("Realizing you're about to give birth, you rip off your [armor] before it can be ruined by what's coming." + Environment.NewLine + Environment.NewLine);
			//	}

			//}


			if (!largeEggs)
			{
				//light quantity
				if (eggCount < 10)
				{
					sb.Append("You are interrupted as you find yourself overtaken by an uncontrollable urge to undress and squat. You berate yourself for giving in to the urge for a moment before feeling something shift. " +
						"You hear the splash of fluid on the ground and look down to see a thick greenish fluid puddling underneath you. There is no time to ponder this development as a rounded object passes down your birth canal, " +
						"spreading your feminine lips apart and forcing a blush to your cheeks. It plops into the puddle with a splash, and you find yourself feeling visibly delighted to be laying such healthy eggs. " +
						"Another egg works its way down and you realize the process is turning you on more and more. In total you lay ");
					sb.Append(eggDescript(eggsLaid));
					sb.Append(", driving yourself to the very edge of orgasm.");
				}
				//High quantity
				else
				{
					sb.Append("A strange desire overwhelms your sensibilities, forcing you to shed your " /*+ mother.armorName */ + " and drop to your hands and knees. " +
						"You manage to roll over and prop yourself up against a smooth rock, looking down over your pregnant-looking belly as green fluids leak from you, soaking into the ground. " +
						"A powerful contraction rips through you and your legs spread instinctively, opening your " + player.vaginas[0].shortDescription() + " to better deposit your precious cargo. " +
						"You see the rounded surface of an egg peek through your lips, mottled with strange colors. You push hard and it drops free with an abrupt violent motion. " +
						"The friction and slimy fluids begin to arouse you, flooding your groin with heat as you feel the second egg pushing down. It slips free with greater ease than the first, " +
						 "arousing you further as you bleat out a moan from the unexpected pleasure. Before it stops rolling on the ground, you feel the next egg sliding down your slime-slicked passage, " +
						"rubbing you perfectly as it slides free. You lose count of the eggs and begin to masturbate, ");

					if (player.vaginas[0].clit.length > 5)
					{
						sb.Append("jerking on your huge clitty as if it were a cock, moaning and panting as each egg slides free of your diminishing belly. You lubricate it with a mix of your juices and the slime until ");
					}

					else if (player.vaginas[0].clit.length > 2)
					{
						sb.Append("playing with your over-large clit as if it were a small cock, moaning and panting as the eggs slide free of your diminishing belly. You spread the slime and cunt juice over it as you tease and stroke until ");
					}

					if (player.vaginas[0].clit.length <= 2)
					{
						sb.Append("pulling your folds wide and playing with your clit as another egg pops free from your diminishing belly. You make wet 'schlick'ing sounds as you spread the slime around, vigorously frigging yourself until ");
					}

					sb.Append("you quiver in orgasm, popping out the last of your eggs as your body twitches nervelessly on the ground. In total you lay " + eggDescript(eggsLaid) + ".");
				}
			}
			//Large egg scene
			else
			{
				sb.Append("A sudden shift in the weight of your pregnant belly staggers you, dropping you to your knees. You realize something is about to be birthed, and you shed your " /*+ mother.armorName */ +
					" before it can be ruined by what's coming. A contraction pushes violently through your midsection, ");

				if (player.vaginas[0].looseness < VaginalLooseness.LOOSE)
				{
					sb.Append("stretching your tight cunt painfully, the lips opening wide ");
				}

				else if (player.vaginas[0].looseness <= VaginalLooseness.GAPING)
				{
					sb.Append("temporarily stretching your cunt-lips wide-open ");
				}

				else //if (mother.vaginas[0].looseness > VaginalLooseness.GAPING)
				{
					sb.Append("parting your already gaping lips wide ");
				}

				sb.Append("as something begins sliding down your passage. A burst of green slime soaks the ground below as the birthing begins in earnest, and the rounded surface of a strangely colored egg " +
					"peaks between your lips. You push hard and the large egg pops free at last, making you sigh with relief as it drops into the pool of slime. The experience definitely turns you on, " +
					"and you feel your clit growing free of its hood as another big egg starts working its way down your birth canal, rubbing your sensitive vaginal walls pleasurably. You pant and moan " +
					"as the contractions stretch you tightly around the next, slowly forcing it out between your nether-lips. The sound of a gasp startles you as it pops free, until you realize " +
					"it was your own voice responding to the sudden pressure and pleasure. Aroused beyond reasonable measure, you begin to masturbate ");

				if (player.vaginas[0].clit.length > 5)
				{
					sb.Append("your massive cock-like clit, jacking it off with the slimy birthing fluids as lube. It pulses and twitches in time with your heartbeats, its sensitive surface overloading your fragile mind with pleasure. ");
				}

				else if (player.vaginas[0].clit.length > 2)
				{
					sb.Append("your large clit like a tiny cock, stroking it up and down between your slime-lubed thumb and fore-finger. It twitches and pulses with your heartbeats, the incredible sensitivity of it overloading your fragile mind with waves of pleasure. ");
				}

				else //if (mother.vaginas[0].clit.length <= 2)
				{
					sb.Append("your " + player.vaginas[0].shortDescription() + " by pulling your folds wide and playing with your clit. Another egg pops free from your diminishing belly, accompanied by an audible burst of relief. You make wet 'schlick'ing sounds as you spread the slime around, vigorously frigging yourself. ");
				}

				sb.Append("You cum hard, the big eggs each making your cunt gape wide just before popping free. You slump down, exhausted and barely conscious from the force of the orgasm. ");

				if (eggCount >= 11)
				{
					sb.Append("Your swollen belly doesn't seem to be done with you, as yet another egg pushes its way to freedom. The stimulation so soon after orgasm pushes you into a pleasure-stupor. " +
						"If anyone or anything discovered you now, they would see you collapsed next to a pile of eggs, your fingers tracing the outline of your " + player.vaginas[0].shortDescription() + " as more and more eggs pop free. " +
						"In time your wits return, leaving you with the realization that you are no longer pregnant. ");
				}

				sb.Append("\n\nYou gaze down at the mess, counting " + eggDescript(eggsLaid) + ".");
			}
			if (gainsOviMax)
			{
				sb.Append("\n\n(<b>Perk Gained: Oviposition</b>)");
			}

			sb.Append("\n\n<b>You feel compelled to leave the eggs behind, ");

			//if (mother.hasStatusEffect(StatusEffects.AteEgg))
			//{
			//	sb.Append("but you remember the effects of the last one you ate.\n</b>");
			//}
			//else
			//{
				sb.Append("but your body's intuition reminds you they shouldn't be fertile, and your belly rumbles with barely contained hunger.\n</b>");
			//}

			return sb.ToString();
			
		}

		private string eggDescript(EggBase eggsLaid, bool plural = true)
		{

			string descript = "";
			descript += Utils.NumberAsText(eggCount) + " ";

			if (largeEggs)
			{ 
				descript += "large ";
			}
			descript += eggsLaid.shortDesc();
			//EGGS
			if (plural)
			{
				descript += "s";
			}
			return descript;
		}
	}
}
