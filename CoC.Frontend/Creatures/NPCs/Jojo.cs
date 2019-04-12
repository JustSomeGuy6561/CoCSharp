using CoC.Frontend.SaveData;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Creatures.NPCs
{
	internal sealed class Jojo
	{
		private static FrontendSessionSave save => FrontendSessionSave.data;
		public static bool isUnlocked => save.jojoUnlocked;
		public static bool isDisabled => save.jojoDisabled;
		public static bool isFollower => save.jojoFollower;

	}
}
