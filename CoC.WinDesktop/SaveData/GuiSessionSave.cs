using CoC.Backend.SaveData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCWinDesktop
{
	public sealed class GuiSessionSave : SaveData
	{
		public static GuiSessionSave data => SaveSystem.getSessionSave<GuiSessionSave>();

		//afaik all of this would be global, though i suppose we could have an option to set a background for "just this session"
	}
}
