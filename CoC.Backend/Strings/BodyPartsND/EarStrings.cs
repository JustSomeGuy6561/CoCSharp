//EarStrings.cs
//Description:
//Author: JustSomeGuy
//01/04/2019, 6:09 PM
using CoC.Backend.Creatures;
using CoC.Backend.Tools;

namespace CoC.Backend.BodyParts
{
	public partial class Ears
	{
		public static string Name()
		{
			return "Ears";
		}
	}

	public partial class EarType
	{
		#region Human
		private static string HumanDesc(bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HumanLongDesc(EarData ears, bool alternateFormat, bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		//ears also use Hair. because reasons.
		private static string HumanPlayerStr(Ears ears, PlayerBase player)
		{
			Hair hair = player.hair;

			if (hair.isBald)
			{
				return "Your head is completely devoid of hair leaving your head and ears completely exposed";
			}
			else
			{
				return "Your " + hair.FullDescription() + " looks good on you, accentuating your features well.";
			}
		}
		private static string HumanTransformStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HumanRestoreStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Horse
		private static string HorseDesc(bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HorseLongDesc(EarData ears, bool alternateFormat, bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HorsePlayerStr(Ears ears, PlayerBase player)
		{
			if (player.hair.isBald)
			{
				return PlayerBaldFlavorText(player) + " A pair of horse-like ears rise up from the top of your head.";
			}
			else
			{
				return "The " + player.hair.FullDescription() + " on your head parts around a pair of very horse-like ears that grow up from your head.";
			}
		}
		private static string HorseTransformStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HorseRestoreStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Dog
		private static string DogDesc(bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DogLongDesc(EarData ears, bool alternateFormat, bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DogPlayerStr(Ears ears, PlayerBase player)
		{
			if (player.hair.isBald)
			{
				return PlayerBaldFlavorText(player) + " A pair of dog ears protrude from your skull, flopping down adorably.";
			}
			else
			{
				return "The " + player.hair.FullDescription() + " on your head is overlapped by a pair of pointed dog ears.";
			}
		}
		private static string DogTransformStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DogRestoreStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Cow
		private static string CowDesc(bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CowLongDesc(EarData ears, bool alternateFormat, bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CowPlayerStr(Ears ears, PlayerBase player)
		{
			if (player.hair.isBald)
			{
				return PlayerBaldFlavorText(player) + " A pair of round, floppy cow ears protrude from the sides of your skull.";
			}
			else
			{
				return "The " + player.hair.FullDescription() + " on your head is parted by a pair of rounded cow ears that stick out sideways.";
			}
		}
		private static string CowTransformStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CowRestoreStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Elfin
		private static string ElfinDesc(bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ElfinLongDesc(EarData ears, bool alternateFormat, bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ElfinPlayerStr(Ears ears, PlayerBase player)
		{
			if (player.hair.isBald)
			{
				return PlayerBaldFlavorText(player) + " A pair of large pointy ears stick out from your skull.";
			}
			else
			{
				return "The " + player.hair.FullDescription() + " on your head is parted by a pair of cute pointed ears, bigger than your old human ones.";
			}
		}
		private static string ElfinTransformStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ElfinRestoreStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Cat
		private static string CatDesc(bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CatLongDesc(EarData ears, bool alternateFormat, bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CatPlayerStr(Ears ears, PlayerBase player)
		{
			if (player.hair.isBald)
			{
				return PlayerBaldFlavorText(player) + " A pair of cute, fuzzy cat ears have sprouted from the top of your head.";
			}
			else
			{
				return "The " + player.hair.FullDescription() + " on your head is parted by a pair of cute, fuzzy cat ears, sprouting from atop your head and pivoting towards any sudden noises.";
			}
		}
		private static string CatTransformStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CatRestoreStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Lizard
		private static string LizardDesc(bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LizardLongDesc(EarData ears, bool alternateFormat, bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LizardPlayerStr(Ears ears, PlayerBase player)
		{
			if (player.hair.isBald)
			{
				return PlayerBaldFlavorText(player) + " A pair of rounded protrusions with small holes on the sides of your head serve as your ears.";
			}
			else
			{
				return "The " + player.hair.FullDescription() + " atop your head makes it nigh-impossible to notice the two small rounded openings that are your ears.";
			}
		}
		private static string LizardTransformStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LizardRestoreStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BunnyDesc(bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BunnyLongDesc(EarData ears, bool alternateFormat, bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BunnyPlayerStr(Ears ears, PlayerBase player)
		{
			if (player.hair.isBald)
			{
				return PlayerBaldFlavorText(player) + " A pair of floppy rabbit ears stick up from the top of your head, flopping around as you walk.";
			}
			else
			{
				return "A pair of floppy rabbit ears stick up out of your " + player.hair.FullDescription() + ", bouncing around as you walk.";
			}
		}
		private static string BunnyTransformStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BunnyRestoreStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Kangaroo
		private static string KangarooDesc(bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string KangarooLongDesc(EarData ears, bool alternateFormat, bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string KangarooPlayerStr(Ears ears, PlayerBase player)
		{
			return "The " + player.hair.FullDescription() + " atop your head is parted by a pair of long, furred kangaroo ears that stick out at an angle.";
		}
		private static string KangarooTransformStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string KangarooRestoreStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Fox
		private static string FoxDesc(bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FoxLongDesc(EarData ears, bool alternateFormat, bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FoxPlayerStr(Ears ears, PlayerBase player)
		{
			if (player.hair.isBald)
			{
				return PlayerBaldFlavorText(player) + " A pair of large, adept fox ears sit high on your head, always listening.";
			}
			else
			{
				return "The " + player.hair.FullDescription() + " atop your head is parted by a pair of large, adept fox ears that always seem to be listening.";
			}
		}
		private static string FoxTransformStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FoxRestoreStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Dragon
		private static string DragonDesc(bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DragonLongDesc(EarData ears, bool alternateFormat, bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DragonPlayerStr(Ears ears, PlayerBase player)
		{
			if (player.hair.isBald)
			{
				return PlayerBaldFlavorText(player) + " A pair of rounded protrusions with small holes on the sides of your head serve as your ears.  Bony fins sprout behind them.";
			}
			else
			{
				return "The " + player.hair.FullDescription() + " atop your head is parted by a pair of rounded protrusions with small holes on the sides of your head serve as your ears.  Bony fins sprout behind them.";
			}
		}
		private static string DragonTransformStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DragonRestoreStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Raccoon
		private static string RaccoonDesc(bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RaccoonLongDesc(EarData ears, bool alternateFormat, bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RaccoonPlayerStr(Ears ears, PlayerBase player)
		{
			if (player.hair.isBald)
			{
				return PlayerBaldFlavorText(player) + " A pair of vaguely egg-shaped, furry raccoon ears adorns your head.";
			}
			else
			{
				return "The " + player.hair.FullDescription() + " on your head parts around a pair of egg-shaped, furry raccoon ears.";
			}
		}
		private static string RaccoonTransformStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RaccoonRestoreStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Mouse
		private static string MouseDesc(bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string MouseLongDesc(EarData ears, bool alternateFormat, bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string MousePlayerStr(Ears ears, PlayerBase player)
		{
			if (player.hair.isBald)
			{
				return PlayerBaldFlavorText(player) + " A pair of large, dish-shaped mouse ears tops your head.";
			}
			else
			{
				return "The " + player.hair.FullDescription() + " atop your head is funneled between and around a pair of large, dish-shaped mouse ears that stick up prominently.";
			}
		}
		private static string MouseTransformStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string MouseRestoreStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Ferret
		private static string FerretDesc(bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FerretLongDesc(EarData ears, bool alternateFormat, bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FerretPlayerStr(Ears ears, PlayerBase player)
		{
			if (player.hair.isBald)
			{
				return PlayerBaldFlavorText(player) + " Big, " + ears.earFurColor.AsString() + " furred ferret ears lie atop your head, doing a good job detecting nearby sounds.";
			}
			else
			{
				return "Big, " + ears.earFurColor.AsString() + " furred ferret ears poke out from your " + player.hair.FullDescription() + ", doing a good job detecting nearby sounds.";
			}
		}
		private static string FerretTransformStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FerretRestoreStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Pig
		private static string PigDesc(bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PigLongDesc(EarData ears, bool alternateFormat, bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PigPlayerStr(Ears ears, PlayerBase player)
		{
			//<mod>
			if (player.hair.isBald)
			{
				return PlayerBaldFlavorText(player) + " A pair of pointy, floppy pig ears have sprouted from the top of your head.";
			}
			else
			{
				return "The " + player.hair.FullDescription() + " on your head is parted by a pair of pointy, floppy pig ears. They often flick about when you're not thinking about it.";
			}
		}
		private static string PigTransformStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PigRestoreStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Rhino
		private static string RhinoDesc(bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RhinoLongDesc(EarData ears, bool alternateFormat, bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RhinoPlayerStr(Ears ears, PlayerBase player)
		{
			if (player.hair.isBald)
			{
				return PlayerBaldFlavorText(player) + " A pair of open tubular rhino ears protrude from your head.";
			}
			else
			{
				return "The " + player.hair.FullDescription() + " on your head is parted by a pair of tubular rhino ears.";
			}
		}
		private static string RhinoTransformStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RhinoRestoreStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Echidna
		private static string EchidnaDesc(bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string EchidnaLongDesc(EarData ears, bool alternateFormat, bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string EchidnaPlayerStr(Ears ears, PlayerBase player)
		{
			if (player.hair.isBald)
			{
				return PlayerBaldFlavorText(player) + " A pair of small rounded openings appear on your head that are your ears.";
			}
			else
			{
				return "Your " + player.hair.FullDescription() + " makes it near-impossible to see the small, rounded openings that are your ears.";
			}
		}
		private static string EchidnaTransformStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string EchidnaRestoreStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Deer
		private static string DeerDesc(bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DeerLongDesc(EarData ears, bool alternateFormat, bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DeerPlayerStr(Ears ears, PlayerBase player)
		{
			if (player.hair.isBald)
			{
				return PlayerBaldFlavorText(player) + " A pair of deer-like ears rise up from the top of your head.";
			}
			else
			{
				return "The " + player.hair.FullDescription() + " on your head parts around a pair of deer-like ears that grow up from your head.";
			}
		}
		private static string DeerTransformStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DeerRestoreStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Wolf
		private static string WolfDesc(bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WolfLongDesc(EarData ears, bool alternateFormat, bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WolfPlayerStr(Ears ears, PlayerBase player)
		{
			if (player.hair.isBald)
			{
				return PlayerBaldFlavorText(player) + " A pair of wolf ears stick out from your head, attuned to every sound around you.";
			}
			else
			{
				return "A pair of wolf ears stick out from your head, parting your " + player.hair.FullDescription() + " and remaining alert to your surroundings.";
			}
		}
		private static string WolfTransformStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WolfRestoreStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Sheep
		private static string SheepDesc(bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SheepLongDesc(EarData ears, bool alternateFormat, bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SheepPlayerStr(Ears ears, PlayerBase player)
		{
			if (player.hair.isBald)
			{
				return PlayerBaldFlavorText(player) + " Two tear drop shaped ears peek out from the sides of your head, their fluffy texture and lazy positioning giving you a cute and sleepy air.";
			}
			else
			{
				return "Two tear drop shaped ears part your " + player.hair.FullDescription() + " and peek out from the sides of your head, their fluffy texture and lazy positioning giving you a cute and sleepy air.";
			}
		}
		private static string SheepTransformStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SheepRestoreStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Imp
		private static string ImpDesc(bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ImpLongDesc(EarData ears, bool alternateFormat, bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		//fun fact: this was never implemented in the original game. IDK why. Google shows imp ears to basically just be pointed ears like an elf, but larger and curved backward
		//slightly. so i'm just stealing elfin ears and modifying it slightly.
		private static string ImpPlayerStr(Ears ears, PlayerBase player)
		{
			if (player.hair.isBald)
			{
				return PlayerBaldFlavorText(player) + " A pair of large pointy ears stick out from your skull, elfin in shape, but much wider. " +
					"The point is also bent backward slightly, a distinctly impish trait.";
			}
			else
			{
				return "The " + player.hair.FullDescription() + " on your head is parted by a pair of wide pointed ears that curve backward slightly as they taper off.";
			}
		}
		private static string ImpTransformStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ImpRestoreStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Cockatrice
		private static string CockatriceDesc(bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CockatriceLongDesc(EarData ears, bool alternateFormat, bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CockatricePlayerStr(Ears ears, PlayerBase player)
		{
			return player.hair.PlayerDescription() + " From the sides of your head, below your hairline, protrude a quartet of feathers, the longest being vertical " +
				"while the 3 shorter ones come out at a 1 o'clock, 2 o'clock and 3 o'clock angle. Behind them hides the avian hole that is your ear.";
		}
		private static string CockatriceTransformStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CockatriceRestoreStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region RedPanda
		private static string RedPandaDesc(bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RedPandaLongDesc(EarData ears, bool alternateFormat, bool plural)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RedPandaPlayerStr(Ears ears, PlayerBase player)
		{
			if (player.hair.isBald)
			{
				return PlayerBaldFlavorText(player) + " Big, white furred, red-panda ears lie atop your head, keeping you well aware to your surroundings.";
			}
			else
			{
				return "Big, white furred, red-panda ears lie atop your head that part your " + player.hair.FullDescription() + ", keeping you well aware to your surroundings.";
			}
		}
		private static string RedPandaTransformStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RedPandaRestoreStr(EarData previousEarData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion

		//<mod> Mod-added ears

		/*
			if (player.hair.length == 0)
			{
				if (player.hasFur())
					outputText("You have no hair, only a thin layer of fur atop of your head.  ");
				else if (player.hasWool())
					outputText("You have no hair, only a thin layer of wool atop of your head.  ");
				else outputText("You are totally bald, showing only shiny [skinTone] [skinDesc] where your hair should be.");
				switch (player.ears.type) {
	*/

		private static string PlayerBaldFlavorText(PlayerBase player)
		{
			if (player.face.primary.usesFur)
			{
				return "Your head is completely bald, though a thin layer of " + player.face.primary.DescriptionWithColor() + " from your face does cover the outer edges. ";
			}
			else
			{
				return "You are totally bald, your " + player.face.primary.LongDescription() + "completely exposed to the elements. ";
			}
		}
	}
}
