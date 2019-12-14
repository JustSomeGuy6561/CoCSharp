//./classes/Appearance.as
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParseAndDelete
{
	class DeleteMe
	{
		//./classes/Appearance.as
		//NOT IMPLEMENTED
		/*
		public static string beardDescription(Creature creature)
		{
			string description = "";
			string[] options;
			// LENGTH ADJECTIVE!
			if (creature.beard.length == 0)
			{
				options = new string[]
				{
					"shaved",
					"bald",
					"smooth",
					"hairless",
					"glabrous"
				};
				description = Utils.RandomChoice(options) + " chin and cheeks";
				return description;
			}
			if (creature.beard.length < 0.2)
			{
				options = new string[]
				{
					"close-cropped, ",
					"trim, ",
					"very short, "
				};
				description += Utils.RandomChoice(options);
			}
			if (creature.beard.length >= 0.2 && creature.beard.length < 0.5) description += "short, ";
			if (creature.beard.length >= 0.5 && creature.beard.length < 1.5) description += "medium, ";
			if (creature.beard.length >= 1.5 && creature.beard.length < 3) description += "moderately long, ";
			if (creature.beard.length >= 3 && creature.beard.length < 6)
			{
				if (Utils.Rand(2) == 0) description += "long, ";
				else description += "neck-length, ";
			}
			if (creature.beard.length >= 6)
			{
				if (Utils.Rand(2) == 0) description += "very long, ";
				description += "chest-length, ";
			}

			// COLORS
			//
			description += creature.hair.color + " ";
			//
			// BEARD WORDS
			// Follows hair type.
			if (creature.hair.type == 1) description += "";
			else if (creature.hair.type == 2) description += "transparent ";
			else if (creature.hair.type == 3) description += "gooey ";
			else if (creature.hair.type == 4) description += "tentacley ";

			if (creature.beard.style == 0) description += "beard"
			else if (creature.beard.style == 1) description += "goatee"
			else if (creature.beard.style == 2) description += "clean-cut beard"
			else if (creature.beard.style == 3) description += "mountain-man beard"
			return description;
		}
		*/

		//./classes/Appearance.as


		//./classes/Character.as
		//public string beardDesc()
		//{
		//	if (hasBeard())
		//		return "beard";
		//	else
		//	{
		//		//CoC_Settings.error("");
		//		return "ERROR: NO BEARD! <b>YOU ARE NOT A VIKING AND SHOULD TELL KITTEH IMMEDIATELY.</b>";
		//	}
		//}



	}
}