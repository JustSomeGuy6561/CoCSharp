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

		public void UpdateSidebar(Controller controller)
		{
			GetData(controller);
		}
		protected abstract void GetData(Controller controller);
		internal abstract void ClearArrows();

		protected SideBarBase(Controller controller)
		{

		}
	}
}
