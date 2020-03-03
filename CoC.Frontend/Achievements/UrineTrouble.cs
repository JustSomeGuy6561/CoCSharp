using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend;
using CoC.Backend.Achievements;

namespace CoC.Frontend.Achievements
{
	class UrineTrouble : AchievementBase
	{
		public UrineTrouble() : base(NameText)
		{
		}

		private static string NameText()
		{
			return "Urine Trouble!";
		}

		public override string AchievementLockedText()
		{
			return "Urinate in the realm of Mareth.";
		}

		public override string AchievementUnlockedText()
		{
			return "Urinate in the realm of Mareth.";
		}
	}
}
