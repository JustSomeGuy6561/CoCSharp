//PlayerAppearanceStrings.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 11:35 PM
using CoC.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static CoC.UI.TextOutput; // Output text. right now it points to Console.WriteLine.
namespace CoC.Strings
{
	public static class PlayerAppearanceStrings
	{
		private static string startingRaceString(Player player)
		{
			if (player.startingRace != player.race)
			{
				return "You began your journey as a " + player.startingRace + ", but gave that up as you explored the dangers of this realm.";
			}
			return "";
		}

		private static string weaponsAndArmorString(Player player)
		{
			return "\n<b>You are currently " + (player.isWearingAnything() ? "wearing your " + player.armorDescript() : "naked") 
				+ " and using your " + player.weapon.shortDescription() + " as a weapon.</b>";
		}

		private static string facialFeaturesString(Player player)
		{
			return "It has " + player.describeFacialFeatures() + ".";
		}

		public static void Appearance(this Player player)
		{
			OutputText(startingRaceString(player));
			//Creature class.
			OutputText(player.generalDescription());
			OutputText(weaponsAndArmorString(player));

			if (player.Accessories.hasHeadPiece)
			{
				OutputText(player.Accesories.headPiece.TypeAndPlayerDelegate());
			}
			if (player.Accessories.hasNeckAccessory)
			{
				OutputText(player.Accesories.neckPiece.TypeAndPlayerDelegate());
			}
			if (player.Accessories.hasArmOrHandJewelry)
			{
				OutputText(player.Accesories.handPiece.TypeAndPlayerDelegate());
			}
			//Face. Face class.
			OutputText(player.face.TypeAndPlayerDelegate(player));
			//Feminine/Masculine description. In the character class.
			OutputText(facialFeaturesString(player));
			//eyes
			OutputText(player.eyes.TypeAndPlayerDelegate(player));
			//hair and ears.
			OutputText(player.hair.TypeAndPlayerDelegate(player));
			//beards
			OutputText(player.beard.TypeAndPlayerDelegate(player));
			//Tongue
			OutputText(player.tongue.TypeAndPlayerDelegate(player));
			//Horns
			OutputText(player.horns.TypeAndPlayerDelegate(player));
			//neck
			OutputText(player.neck.TypeAndPlayerDelegate(player));
			//gills
			OutputText(player.gills.TypeAndPlayerDelegate());
			//arms
			OutputText(player.arms.TypeAndPlayerDelegate(player));
			//core. Now includes gills!
			OutputText(player.gills.TypeAndPlayerDelegate(player.gills, player));
			OutputText(player.core.TypeAndPlayerDelegate(player.core, player));
			//wings
			OutputText(player.wings.TypeAndPlayerDelegate(player));
			//back/spine
			OutputText(player.back.TypeAndPlayerDelegate(player));
			//butt/hips/legs
			OutputText(player.lowerBody.TypeAndPlayerDelegate(player));
			//incorporeal? no idea how i'll add this.
			OutputText(player.perk.incorporeal.TypeAndPlayerDelegate());
			//tail
			OutputText(player.tail.TypeAndPlayerDelegate(player));
			//pregnancy
			if (player.buttpregnant && player.normalpregnant)
			{
				OutputText(player.doublyPregnant(player.buttPregnancy, player.normalPregnancy));
			}
			else if (player.buttpregnant)
			{
				OutputText(player.buttPregnancy.TypeAndPlayerDelegate());
			}
			else
			{
				OutputText(player.normalPregnancy.TypeAndPlayerDelegate());
			}
			//"Chesticles..I mean bewbz" - original author. credit where it's due.
			OutputText(player.describeAllBoobs());
			//For naga's, quadrupeds. normal probably will return an empty string. 
			//alternatively, some flavor text.
			OutputText(player.genitals.describeLocation());
			//_ALL_ the dicks.
			OutputText(player.describeAllCocks());
			//same as incorporeal. 
			OutputText(player.perk.infested.TypeAndPlayerDelegate());
			//Balls
			OutputText(player.describeAllBalls());
			//VAGOOZ - author unknown
			OutputText(player.describeAllVags());

			OutputText(player.lowerbody.assDescript());

			OutputText(player.describePiercings());

			OutputText(player.gemsString());
		}

	}
}
