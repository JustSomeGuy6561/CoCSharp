//PlayerAppearanceStrings.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 11:35 PM
using CoC.Creatures;
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
				OutputText(player.Accesories.headPiece.playerDescription(player));
			}
			if (player.Accessories.hasNeckAccessory)
			{
				OutputText(player.Accesories.neckPiece.playerDescription(player));
			}
			if (player.Accessories.hasArmOrHandJewelry)
			{
				OutputText(player.Accesories.handPiece.playerDescription(player));
			}
			//Face. Face class.
			OutputText(player.face.playerDescription(player));
			//Feminine/Masculine description. In the character class.
			OutputText(facialFeaturesString(player));
			//eyes
			OutputText(player.eyes.playerDescription(player));
			//hair and ears.
			OutputText(player.hair.playerDescription(player));
			//beards
			OutputText(player.beard.playerDescription(player));
			//Tongue
			OutputText(player.tongue.playerDescription(player));
			//Horns
			OutputText(player.horns.playerDescription(player));
			//neck
			OutputText(player.neck.playerDescription(player));
			//gills
			OutputText(player.gills.playerDescription(player));
			//arms
			OutputText(player.arms.playerDescription(player));
			//core.
			OutputText(player.body.playerDescription(player));
			//wings
			OutputText(player.wings.playerDescription(player));
			//back/spine
			OutputText(player.back.playerDescription(player));
			//butt/hips/legs
			OutputText(player.lowerBody.playerDescription(player));
			//incorporeal? no idea how i'll add this.
			OutputText(player.perk.incorporeal.playerDescription());
			//tail
			OutputText(player.tail.playerDescription(player));
			//pregnancy
			if (player.buttpregnant && player.normalpregnant)
			{
				OutputText(player.doublyPregnant(player.buttPregnancy, player.normalPregnancy));
			}
			else if (player.buttpregnant)
			{
				OutputText(player.buttPregnancy.playerDescription());
			}
			else
			{
				OutputText(player.normalPregnancy.playerDescription());
			}
			//"Chesticles..I mean bewbz" - original author. credit where it's due.
			OutputText(player.describeAllBoobs());
			//For naga's, quadrupeds. normal probably will return an empty string. 
			//alternatively, some flavor text.
			OutputText(player.genitals.describeLocation());
			//_ALL_ the dicks.
			OutputText(player.describeAllCocks());
			//Balls
			OutputText(player.describeAllBalls());
			//VAGOOZ - author unknown
			OutputText(player.describeAllVags());

			OutputText(player.lowerBody.assDescript());

			OutputText(player.describePiercings());

			OutputText(player.gemsString());
		}

	}
}
