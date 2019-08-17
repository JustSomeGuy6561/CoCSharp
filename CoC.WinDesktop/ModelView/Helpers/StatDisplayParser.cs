using CoC.Frontend.UI;
using CoC.Frontend.UI.ControllerData;
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
		
		//note: the stat data collection passed in here may not be valid - we only need it in the constructors to determine what we can output, not what we are outputting.
		public StatDisplayParser(StatDataCollectionBase statData, bool silent = false)
		{
			standardSideBar = new StandardSideBarModelView(statData, silent);
			prisonSideBar = new PrisonSideBarModelView(statData, silent);
			enemySideBar = new EnemySideBarModelView(statData);
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
