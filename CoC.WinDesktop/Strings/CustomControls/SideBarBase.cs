using CoC.Backend.Engine.Time;
using CoC.Frontend.UI.ControllerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCWinDesktop.CustomControls
{
    public partial class SideBarBase
    {
		protected string CategoryString(CreatureStatCategory category)
		{
			switch (category)
			{
				case CreatureStatCategory.ADVANCEMENT:
					return "Advancement:";
				case CreatureStatCategory.COMBAT:
					return "Combat Stats:";
				case CreatureStatCategory.CORE:
					return "Core Stats:";
				case CreatureStatCategory.GENERAL:
					return "General Stats:";
				case CreatureStatCategory.PRISON:
					return "Prison Stats:";
				case CreatureStatCategory.OTHER:
				default:
					return "Misc. Stats:";
			}
		}

		protected string NameStr(string playerName)
		{
			return "Name: " + playerName ?? "";
		}

		protected string DateStr(GameDateTime time)
		{
			return "Date: " + time?.day.ToString() ?? "";
		}

		protected string HourStr(GameDateTime time)
		{
			return "Hour: " + time?.GetFormattedHourString() ?? "";
		}

	}
}
