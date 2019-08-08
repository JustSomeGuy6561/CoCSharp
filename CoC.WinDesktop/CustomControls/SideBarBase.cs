using CoC.Frontend.UI.ControllerData;
using CoC.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCWinDesktop.CustomControls
{
	public abstract class SideBarBase : INotifyPropertyChanged
	{
		public abstract event PropertyChangedEventHandler PropertyChanged;

		public void UpdateSidebar(StatDataCollection statData)
		{
			GetData(statData);
		}
		protected abstract void GetData(StatDataCollection statData);
		internal abstract void ClearArrows();

		protected SideBarBase(StatDataCollection statData)
		{

		}
	}
}
