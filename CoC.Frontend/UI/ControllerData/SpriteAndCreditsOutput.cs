using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.UI.ControllerData
{
	public static class SpriteAndCreditsOutput
	{
		private static string SpriteName; //name of the sprite. if the GUI needs to parse it to find the actual image, that's the GUI's job.
		private static bool spriteChanged;


		private static string CreatorCredit;

		public static void SetSprite(string name)
		{
			SpriteName = name;
		}

		public static void ClearSprite()
		{
			SpriteName = null;
		}

		private static void SetCreator(string creator)
		{
			CreatorCredit = creator;
		}

		internal static bool QuerySpriteCreditData(out string sprite, out string creator)
		{
			bool retVal = spriteChanged;
			sprite = SpriteName;
			creator = CreatorCredit;
			CreatorCredit = null;
			spriteChanged = false;
			return retVal;
		}
	}
}
