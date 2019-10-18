using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.UI.ControllerData
{
	public class SpriteAndCreditsOutput
	{
		private string SpriteName; //name of the sprite. if the GUI needs to parse it to find the actual image, that's the GUI's job.
		private bool spriteChanged;


		private string CreatorCredit;

		internal void SetSprite(string name)
		{
			SpriteName = name;
		}

		internal void ClearSprite()
		{
			SpriteName = null;
		}

		internal void SetCreator(string creator)
		{
			CreatorCredit = creator;
		}

		internal bool QuerySpriteCreditData(out string sprite, out string creator)
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
