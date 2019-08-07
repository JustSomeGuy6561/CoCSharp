using CoC.Frontend.UI;
using CoC.UI;
using CoCWinDesktop.CustomControls;
using CoCWinDesktop.CustomControls.SideBarModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCWinDesktop.ModelView.Helpers
{
	public sealed class StatDisplayParser
	{

		private readonly StandardSideBarModelView standardSideBar;
		private readonly EnemySideBarModelView enemySideBar;
		private readonly PrisonSideBarModelView prisonSideBar;
		

		public StatDisplayParser(Controller controller)
		{
			standardSideBar = new StandardSideBarModelView(controller);
			enemySideBar = new EnemySideBarModelView(controller);
			prisonSideBar = new PrisonSideBarModelView(controller);
		}

		public SideBarBase GetSideBarBase(bool isPlayer, PlayerStatus playerStatus)
		{
			if (!isPlayer)
			{
				return enemySideBar;
			}
			else if (playerStatus == PlayerStatus.PRISON)
			{
				return prisonSideBar;
			}
			else
			{
				return standardSideBar;
			}
		}
	}
}
