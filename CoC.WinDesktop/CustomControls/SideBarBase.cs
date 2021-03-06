﻿using CoC.Frontend.UI.ControllerData;
using CoC.UI;
using CoC.WinDesktop.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoC.WinDesktop.CustomControls
{
	public abstract partial class SideBarBase : NotifierBase
	{
		public void UpdateSidebar(StatDataCollectionBase statData)
		{
			GetData(statData);
		}
		protected abstract void GetData(StatDataCollectionBase statData);
		internal abstract void ClearArrows();

		protected SideBarBase(StatDataCollectionBase statData)
		{

		}
	}
}
