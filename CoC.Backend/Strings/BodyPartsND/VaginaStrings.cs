using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System;
using System.Text;

namespace CoC.Backend.BodyParts
{
	public partial class LabiaPiercingLocation
	{
		private static string Left1Button()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string Left1Location()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string Left2Button()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string Left2Location()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string Left3Button()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string Left3Location()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string Left4Button()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string Left4Location()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string Left5Button()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string Left5Location()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string Left6Button()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string Left6Location()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string Right1Button()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string Right1Location()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string Right2Button()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string Right2Location()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string Right3Button()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string Right3Location()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string Right4Button()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string Right4Location()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string Right5Button()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string Right5Location()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string Right6Button()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string Right6Location()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
	}

	internal interface IVagina
	{
		#region Text
		string ShortDescription(bool plural);

		string ShortDescription();

		string LongDescription(bool alternateFormat = false);

		string LongDescriptionPrimary();

		string LongDescriptionAlternate();

		string AdjectiveText(bool multipleAdjectives);

		string FullDescriptionPrimary();

		string FullDescriptionAlternate();

		string FullDescription(bool alternateFormat);

		VaginaType type { get; }
		#endregion


	}

	public partial class Vagina : IVagina
	{
		public static string Name()
		{
			return "Vagina";
		}

		private string AllLabiaPiercingsShort(Creature creature)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private string AllLabiaPiercingsLong(Creature creature)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private string VaginaTightenedUpDueToInactivity(VaginalLooseness oldLooseness)
		{

			string recoverText;

			if (oldLooseness <= VaginalLooseness.ROOMY)
			{
				recoverText = " recovers from your ordeals, tightening up a bit.";
			}
			else if (oldLooseness == VaginalLooseness.GAPING)
			{
				recoverText = " recovers from your ordeals and becomes tighter.";
			}
			else //if (oldLooseness >= VaginalLooseness.CLOWN_CAR_WIDE)
			{
				string amount = looseness >= VaginalLooseness.ROOMY ? ", but not much." : ".";
				recoverText = " recovers from the brutal stretching it has received and tightens up a little bit" + amount;
			}
			return Environment.NewLine + "Your " + ShortDescription() + recoverText + Environment.NewLine;
		}
	}

	public partial class VaginaType
	{
		private static readonly string[] sfwVagDesc = new string[] { "vagina", "pussy", "cooter", "snatch", "muff" };
		private static readonly string[] sfwVagDescPlural = new string[] { "vaginas", "pussies", "cooters", "snatches", "muffs" };
		private static readonly string[] allVagDesc = new string[] { "vagina", "pussy", "cooter", "twat", "cunt", "snatch", "fuck-hole", "muff" };
		private static readonly string[] allVagDescPlural = new string[] { "vaginas", "pussies", "cooters", "twats", "cunts", "snatches", "fuck-holes", "muffs" };

		private static readonly string[] sandTrapStrings = new string[] { "black", "onyx", "ebony", "dusky", "sable", "obsidian", "midnight-hued", "jet black" };
		private static readonly string[] sandTrapStringsWithArticle = new string[] { "a black", "an onyx", "an ebony", "a dusky", "a sable", "an obsidian", "a midnight-hued", "a jet black" };


		public static string VaginaNoun(bool plural)
		{

			string[] items;
			if (isSFW)
			{
				items = plural ? sfwVagDescPlural : sfwVagDesc;
			}
			else
			{
				items = plural ? allVagDescPlural : allVagDesc;
			}

			return Utils.RandomChoice(items);
		}

		private static bool isSFW => SaveData.BackendSessionSave.data.SFW_Mode;

		private static string HumanShortDesc(bool plural)
		{
			return VaginaNoun(plural);
		}

		private static string HumanSingleDesc()
		{
			return Utils.RandomChoice("an ordinary ", "a normal ", "a human") + VaginaNoun(false);
		}

		private static string HumanLongDesc(VaginaData vagina, bool alternateFormat)
		{
			return GenericDesc(vagina, alternateFormat, false);

		}
		private static string HumanFullDesc(VaginaData vagina, bool alternateFormat)
		{
			return GenericDesc(vagina, alternateFormat, true);

		}
		//ngl, no idea how to make this sound unique, so i've just gone with capacity based text here.
		private static string HumanPlayerStr(Vagina vagina, PlayerBase player)
		{
			ushort capacity = vagina.VaginalCapacity();
			if (capacity > 1000)
			{
				return " It is capable of handling utterly massive penetrations, far beyond what any normal human pussy could. ";
			}
			else if (capacity > 500)
			{
				return " It is capable of handling penetrators of incredible size, well beyond what any normal human pussy could. ";
			}
			else if (capacity > 100)
			{
				return " Despite its relatively normal appearance, it is able to handle larger penetrations without much issue. ";
			}
			else if (capacity > 50)
			{
				return " While it appears relatively normal, it is able to handle above-average penetrations without too much of an issue. ";
			}
			else
			{
				return " Its inner folds have a certain appeal, but are otherwise relatively normal. ";
			}
		}

		private static string HumanTransformStr(VaginaData previousVagina, PlayerBase player)
		{
			return previousVagina.type.RestoredString(previousVagina, player);
		}

		private static string HumanGrewVaginaStr(PlayerBase player, byte grownVaginaIndex)
		{
			return GenericGrewVaginaDesc(player, grownVaginaIndex);
		}

		private static string HumanRestoreStr(VaginaData originalVagina, PlayerBase player)
		{
			return GlobalStrings.RevertAsDefault(originalVagina, player);
		}

		private static string HumanRemovedVaginaStr(VaginaData removedVagina, PlayerBase player)
		{
			return GenericLoseVaginaDesc(removedVagina, player);
		}

		private static string EquineDesc(bool plural)
		{
			return "equine " + VaginaNoun(plural);
		}

		private static string EquineSingleDesc()
		{
			return "an equine " + VaginaNoun(false);
		}

		private static string EquineLongDesc(VaginaData vagina, bool alternateFormat)
		{
			return GenericDesc(vagina, alternateFormat, false, "equine");
		}
		private static string EquineFullDesc(VaginaData vagina, bool alternateFormat)
		{
			return GenericDesc(vagina, alternateFormat, true, "equine");
		}
		private static string EquinePlayerStr(Vagina vagina, PlayerBase player)
		{
			string endlessCapacity = vagina.VaginalCapacity() > 1000 ? ", which it pretty much can." : ".";

			return " It is longer and wider than a human cunt, and is surrounded by a dark-purple. With its full, puffy lips and a naturally larger size, it almost appears tailored to " +
				"take some of the largest cocks you can imagine" + endlessCapacity;

		}
		//some text taken from kelly pussy->horse-cunt text, and largely altered to work here.
		private static string EquineTransformStr(VaginaData oldVagina, PlayerBase player)
		{
			var currentVag = player.vaginas[oldVagina.vaginaIndex];
			//should never happen but would cause strangeness in my lazy check later, so we'll explicitely handle it.
			if (oldVagina.type == currentVag.type)
			{
				return "Your " + oldVagina.ShortDescription() + " feels odd, but a quick inspection reveals nothing changed. Odd.";
			}

			StringBuilder sb = new StringBuilder(GlobalStrings.NewParagraph());

			//2 vaginas, and both used to be the same type - we want to tell them which one we're talking about.
			if (player.vaginas.Count == 2)
			{
				sb.Append($"Your {Utils.NumberAsPlace(oldVagina.vaginaIndex)} clit");
			}
			else
			{
				sb.Append($"your {oldVagina.clit.ShortDescription()}");
			}

			//basically, cheat this by forcing the player to orgasm, then realize the changes once they come to. allows me to not care what the old type was.
			sb.Append(" tingles, as if something delicate was being brushed against it. The feeling builds");

			if (player.wearingAnything)
			{
				sb.Append(" until you can't help yourself but undress, desperate to find the source. Strangely, the feeling doesn't go away even though you're completely naked. ");
			}
			else
			{
				sb.Append(" despite the fact that you aren't wearing anything that could cause it, which only serves to confuse you. ");
			}

			if (player.corruption < 25)
			{
				sb.Append("Briefly, you consider masturbating to see if it will relieve the sensation, but quickly push that aside. What are you thinking? Your self-control," +
					"however, has little effect, as the sensation only seems to be getting worse. ");
			}
			else if (player.corruption < 50)
			{
				sb.Append("You consider masturbating to see if it will relieve the sensation, and eventually give in, rubbing your " + oldVagina.clit.ShortDescription() + " gently. " +
					"Unfortunately, that only seems to make things worse. You knew it was a bad idea! ");
			}
			else
			{
				sb.Append("Well, whatever. You suppose you could use an excuse to masturbate, and quickly being rubbing your " + oldVagina.clit.ShortDescription() + " with as much" +
					"fervor as you can manage. At this point you're absolutely certain this will make things worse, but you don't really care. ");
			}

			sb.Append("The sensation builds, heat flooding your " + oldVagina.LongDescription(false) + "until the telltale signs of an oncoming orgasm threaten to overwhelm you. ");

			if (player.corruption < 50)
			{
				sb.Append("Seeing as you have little choice, you begin to masturbate in earnest, desperate to deal with your sudden sexual need. ");
			}
			else
			{
				sb.Append("You attack your clit with renewed purpose, eager to sate your needs, slipping your other" + player.hands.ShortDescription(false) + "into your folds. ");
			}
			//explain why the orgasm doesn't affect breasts or cocks (if applicable)
			if (player.gender == Gender.HERM || player.genitals.isLactating || player.genitals.nippleType == NippleStatus.DICK_NIPPLE)
			{
				sb.Append("Strangely, the urge is centered exclusively in your " + oldVagina.LongDescription(false) + "; ");
				if (player.gender == Gender.HERM)
				{
					string hangStr = player.cocks.Count > 1 ? "hang" : "hangs";
					sb.Append($" your {player.genitals.AllCocksShortDescription()} just {hangStr} there limply");
				}
				else
				{
					sb.Append(" your nipples aren't even remotely aroused.");
				}
			}
			//
			sb.Append(" Accelerated by your aide, it isn't long before you climax. You to scream in ecstasy before passing out, suddenly completely spent. " +
				"You awake shortly after and realize the cause of all this commotion: " + SafelyFormattedString.FormattedText("you now have an equine pussy!", StringFormats.BOLD));
			//describe it.
			sb.Append(" Your vertical slit is much longer and wider than anything you'd normally see on a human, and your inner lips are significantly more plump. Your vulva, " +
				"which has also shifted to accomodate your new size, is also a dark-purple, almost black color, contrasting with your pink inner folds. ");

			//and explain capacity bonus.
			if (player.corruption < 25)
			{
				sb.Append($"You clean up a bit and move to a more secluded spot to regain some semblence of decency before checking examining your {currentVag.LongDescription(false)} " +
					"further. ");
			}

			sb.Append("A quick examination reveals you're just as tight and as wet as you were before, ");
			if (currentVag.VaginalCapacity() >= 60 && oldVagina.capacity <= 50)
			{
				sb.Append("but if the fact that you can now fit part of your arm inside you is any indication, you can take larger insertions");
			}
			else
			{
				string penetrator = currentVag.VaginalCapacity() >= 60 ? "arm " : "hand ";
				sb.Append("but if the fact that you can now fit more of your " + penetrator + " inside you is any indication, you're pretty sure you can take larger insertions");
			}

			return sb.ToString();
		}

		private static string EquineGrewVaginaStr(PlayerBase player, byte grownVaginaIndex)
		{
			return GenericGrewVaginaDesc(player, grownVaginaIndex);
		}

		//largely stolen from kelly horse-cunt-> humanoid pussy, and adapted to make it work here.
		private static string EquineRestoreStr(VaginaData oldVagina, PlayerBase player)
		{
			string intro = "Something invisible brushes against your sex, making you twinge.";

			string vagDesc;
			//2 vaginas, and both used to be the same type - we want to tell them which one we're talking about.
			if (player.vaginas.Count == 2 && player.genitals.CountVaginasOfType(oldVagina.type) == 1)
			{
				vagDesc = $"{Utils.NumberAsPlace(oldVagina.vaginaIndex)} {oldVagina.ShortDescription()}";
			}
			else
			{
				vagDesc = oldVagina.LongDescription(false);
			}
			string wearingText;
			if (!player.wearingAnything)
			{
				wearingText = $" You watch, dumbfounded, as your {vagDesc} shrinks down, finally stopping as it reaches a more human size and shape.";
			}
			else
			{
				wearingText = $" You strip off your clothes to inspect your nethers, and immediately notice your {vagDesc} is much smaller than it was just a few moments ago. " +
					$"It continues shrinking before your eyes, finally stopping when it reaches a more human size and shape.";
			}
			return intro + wearingText + "Your outer lips also shift from their current dark-purple, back to a more tone that blends more naturally with the rest of your body and " +
				"complements your feminine folds. With a start, you realize " + SafelyFormattedString.FormattedText("You have a human pussy once again!", StringFormats.BOLD) +
				" While smaller than it used to be, it appears just as tight, relatively speaking, and is still just as wet as it was before. Still, you're almost certain " +
				"you've lost some of your former capacity, and a quick probe into your folds proves that is indeed the case. ";
		}

		private static string EquineRemovedVaginaStr(VaginaData removedVagina, PlayerBase player)
		{
			return GenericLoseVaginaDesc(removedVagina, player);
		}

		private static string SandTrapDesc(bool plural)
		{
			return Utils.RandomChoice(sandTrapStrings) + VaginaNoun(plural);
		}

		private static string SandTrapSingleDesc()
		{
			return Utils.RandomChoice(sandTrapStringsWithArticle) + VaginaNoun(false);
		}

		private static string SandTrapLongDesc(VaginaData vagina, bool alternateFormat)
		{
			return GenericDesc(vagina, alternateFormat, false, Utils.RandomChoice(sandTrapStrings));
		}

		private static string SandTrapFullDesc(VaginaData vagina, bool alternateFormat)
		{
			return GenericDesc(vagina, alternateFormat, true, Utils.RandomChoice(sandTrapStrings));
		}
		private static string SandTrapPlayerStr(Vagina vagina, PlayerBase player)
		{
			string endlessCapacity = vagina.VaginalCapacity() > 1000 ? ", which it pretty much is." : ".";
			return " The inner folds are smooth and incredibly dark, making it appear to be endless" + endlessCapacity;
		}
		private static string SandTrapTransformStr(VaginaData oldVagina, PlayerBase player)
		{
			var currentVag = player.vaginas[oldVagina.vaginaIndex];
			//should never happen but would cause strangeness in my lazy check later, so we'll explicitely handle it.
			if (oldVagina.type == currentVag.type)
			{
				return "Your vagina feels odd, but a quick inspection reveals nothing changed. Odd.";
			}

			StringBuilder sb = new StringBuilder(GlobalStrings.NewParagraph());

			//2 vaginas, and both used to be the same type - we want to tell them which one we're talking about.
			if (player.vaginas.Count == 2 && player.genitals.CountVaginasOfType(oldVagina.type) == 1)
			{
				sb.Append($"Your {Utils.NumberAsPlace(oldVagina.vaginaIndex)} {oldVagina.ShortDescription()}");
			}
			else
			{
				sb.Append($"Your {oldVagina.LongDescription(false)}");
			}

			if (oldVagina.type == HUMAN)
			{
				sb.Append(" feels... odd. You undo your clothes and gingerly inspect your nether regions. " +
					"The tender pink color of your sex has disappeared, replaced with smooth, marble blackness starting at your lips and working inwards.");
			}
			else
			{
				sb.Append(" feels... odd. You undo your clothes and gingerly inspect your nether regions. You quickly notice it has changed back toward something more distincly human, "
					+ "though it is most definitely not. Instead of tender pink inner folds you'd expect, your sex is a smooth marble blackness, "
					+ "beginning at your outer lips and moving inward.");
			}
			//(Wet:
			if (currentVag.wetness >= VaginalWetness.SLICK) sb.Append(" Your natural lubrication makes it gleam invitingly.");
			//(Corruption <50:
			if (player.corruption < 50)
			{
				sb.Append(" After a few cautious touches you decide it doesn't feel any different");

				if (oldVagina.type != VaginaType.HUMAN)
				{
					sb.Append(" than you'd expect");
				}
				sb.Append(" - it does certainly look odd, though.");
			}
			else
			{
				sb.Append(" After a few cautious touches you decide it doesn't feel any different - the sheer bizarreness of it is a big turn on though, " +
				  "and you feel it beginning to shine with anticipation at the thought of using it.");
			}

			sb.Append(SafelyFormattedString.FormattedText("Your vagina is now ebony in color.", StringFormats.BOLD));

			return sb.ToString();
		}

		private static string SandTrapGrewVaginaStr(PlayerBase player, byte grownVaginaIndex)
		{
			return GenericGrewVaginaDesc(player, grownVaginaIndex);
		}

		private static string SandTrapRestoreStr(VaginaData oldVagina, PlayerBase player)
		{
			string vagDesc;
			//2 vaginas, and both used to be the same type - we want to tell them which one we're talking about.
			if (player.vaginas.Count == 2 && player.genitals.CountVaginasOfType(oldVagina.type) == 1)
			{
				vagDesc = $"{Utils.NumberAsPlace(oldVagina.vaginaIndex)} {oldVagina.ShortDescription()}";
			}
			else
			{
				vagDesc = oldVagina.LongDescription(false);
			}

			string wearingText;
			if (!player.wearingAnything)
			{
				wearingText = $" You watch as your {vagDesc} shifts and the inner folds return to a more natural shape and color.";
			}
			else
			{
				wearingText = $" Undoing your clothes, you take a look at your {vagDesc} and find that it has turned back to its natural flesh colour.";
			}
			return GlobalStrings.NewParagraph() + "Something invisible brushes against your sex, making you twinge. " + wearingText;
		}

		private static string SandTrapRemovedVaginaStr(VaginaData removedVagina, PlayerBase player)
		{
			return GenericLoseVaginaDesc(removedVagina, player);
		}

		internal static string VaginaAdjectiveText(VaginaData vagina, bool fullDesc)
		{
			StringBuilder sb = new StringBuilder();
			//less confusing way to display values.

			bool showLooseness;
			bool showWetness;

			//guarenteed if virgin, tight, or forced.
			if (fullDesc || vagina.isVirgin || vagina.looseness == VaginalLooseness.TIGHT)
			{
				showLooseness = true;
				showWetness = true;
			}
			//50% each if gaping or looser
			else if (vagina.looseness == VaginalLooseness.GAPING || vagina.looseness == VaginalLooseness.CLOWN_CAR_WIDE)
			{
				showLooseness = Utils.RandBool();
				showWetness = Utils.RandBool();
			}
			//else variable.
			else
			{
				//tightness descript - 40% display rate
				showLooseness = Utils.Rand(5) < 2;
				//wetness descript - 30% display rate
				showWetness = Utils.Rand(10) < 3;
			}

			if (showLooseness)
			{
				sb.Append(vagina.looseness.AsDescriptor());
			}
			if (showWetness && vagina.wetness != VaginalWetness.NORMAL)
			{
				if (showLooseness) sb.Append(", ");
				sb.Append(vagina.wetness.AsDescriptor());
			}
			if (vagina.labiaPiercings.isPierced && (fullDesc || Utils.Rand(3) == 0))
			{
				if (showWetness || showLooseness) sb.Append(", ");
				sb.Append("pierced");
			}

			var creature = CreatureStore.GetCreatureClean(vagina.creatureID);

			if (sb.Length == 0 && creature?.body.type == BodyType.GOO)
			{
				if (sb.Length != 0) sb.Append(", ");
				sb.Append(Utils.RandomChoice("gooey", "slimy"));
			}

			if (!vagina.everPracticedVaginal)
			{
				if (sb.Length != 0) sb.Append(", ");


				sb.Append("true virgin");
			}
			else if (vagina.isVirgin)
			{
				if (sb.Length != 0) sb.Append(", ");

				sb.Append("virgin");
			}
			return sb.ToString();
		}

		private static string GenericDesc(VaginaData vagina, bool alternateFormat, bool fullDesc, string typeAdjective = null)
		{
			string baseAdjectives = VaginaAdjectiveText(vagina, fullDesc);
			if (!string.IsNullOrWhiteSpace(typeAdjective))
			{
				typeAdjective = typeAdjective.Trim() + " ";
			}

			if (!string.IsNullOrWhiteSpace(baseAdjectives))
			{
				baseAdjectives += " ";
			}

			//NOTE FROM VANILLA:
			//	Something that would be nice to have but needs a variable in Creature or Character.
			//if (i_creature.bunnyScore() >= 3) sb.Append("rabbit hole");
			//END NOTE
			//Response: agreed, but species implementation is in the frontend, plus that's weird with say, equine type or sand trap.
			//possible to add to frontend as an extension method, but would require some edge case detection requiring vag type to be humanoid/normal/whatever the default is called.


			return (alternateFormat ? "a " : "") + baseAdjectives + typeAdjective + VaginaNoun(false);

		}

		private static string GenericGrewVaginaDesc(PlayerBase player, byte grownVaginaIndex)
		{
			string multiVagText = player.vaginas.Count > 1 ? "alongside your previous one!" : "!";
			return "An itching starts in your crotch and spreads vertically. You reach down and discover an opening.  You have grown a " +
				SafelyFormattedString.FormattedText("new " + player.vaginas[grownVaginaIndex].LongDescription() + multiVagText, StringFormats.BOLD);
		}

		private static string GenericLoseVaginaDesc(VaginaData removedVagina, PlayerBase player)
		{
			string byeVag = "You slip a hand down to check on it, only to feel the slit growing smaller and smaller until it disappears, taking your clit with it! ";
			if (player.vaginas.Count == 0)
			{
				return "Your " + removedVagina.ShortDescription() + " clenches in pain, doubling you over. " + byeVag +
					SafelyFormattedString.FormattedText("Your vagina is gone!", StringFormats.BOLD);
			}
			else
			{
				if (player.genitals.CountVaginasOfType(removedVagina.type) == 1)
				{
					return "One of your " + removedVagina.ShortDescription() + "clenches in pain, causing you to double over. " + byeVag +
						SafelyFormattedString.FormattedText("One of your " + removedVagina.ShortDescription(true) + "is gone", StringFormats.BOLD) +
						", leaving you with just your " + player.vaginas[0].LongDescription();
				}
				else
				{
					return "Your " + removedVagina.ShortDescription() + " clenches in pain, doubling you over. " + byeVag +
						SafelyFormattedString.FormattedText("Your " + removedVagina.LongDescription() + "is gone", StringFormats.BOLD) +
						", leaving you with just your " + player.vaginas[0].LongDescription();
				}
			}
		}
	}

	public static class VaginaHelpers
	{
		public static string AsText(this VaginalLooseness vaginalLooseness)
		{
			switch (vaginalLooseness)
			{
				case VaginalLooseness.CLOWN_CAR_WIDE:
					return "clown-care wide";
				case VaginalLooseness.GAPING:
					return "gaping";
				case VaginalLooseness.ROOMY:
					return "roomy";
				case VaginalLooseness.LOOSE:
					return "loose";
				case VaginalLooseness.TIGHT:
					return "tight";
				case VaginalLooseness.NORMAL:
				default:
					return "normal";
			}
		}

		public static string AsDescriptor(this VaginalLooseness vaginalLooseness)
		{
			switch (vaginalLooseness)
			{
				case VaginalLooseness.CLOWN_CAR_WIDE:
					return "clown-car wide";
				case VaginalLooseness.GAPING:
					return "gaping";
				case VaginalLooseness.ROOMY:
					return "roomy";
				case VaginalLooseness.LOOSE:
					return "loose";
				case VaginalLooseness.TIGHT:
					return "tight";
				case VaginalLooseness.NORMAL:
				default:
					return "";
			}
		}

		public static string AsText(this VaginalWetness vaginalWetness)
		{
			switch (vaginalWetness)
			{
				case VaginalWetness.SLAVERING:
					return "slavering";
				case VaginalWetness.DROOLING:
					return "drooling";
				case VaginalWetness.SLICK:
					return "slick";
				case VaginalWetness.WET:
					return "wet";
				case VaginalWetness.DRY:
					return "dry";
				case VaginalWetness.NORMAL:
				default:
					return "normal";
			}
		}

		public static string AsDescriptor(this VaginalWetness vaginalWetness)
		{
			switch (vaginalWetness)
			{
				case VaginalWetness.SLAVERING:
					return "slavering";
				case VaginalWetness.DROOLING:
					return "drooling";
				case VaginalWetness.SLICK:
					return "slick";
				case VaginalWetness.WET:
					return "wet";
				case VaginalWetness.DRY:
					return "dry";
				case VaginalWetness.NORMAL:
				default:
					return "";
			}
		}

		public static string AsAdjective(this VaginalWetness vaginalWetness)
		{
			switch (vaginalWetness)
			{
				case VaginalWetness.SLAVERING:
					return "absolutely drenched in fem-spunk";
				case VaginalWetness.DROOLING:
					return "dripping natural lubricant";
				case VaginalWetness.SLICK:
					return "slick with fem-spunk";
				case VaginalWetness.WET:
					return "wetter than normal";
				case VaginalWetness.DRY:
					return "rather dry";
				case VaginalWetness.NORMAL:
				default:
					return "only a little wet";
			}
		}
	}
}
