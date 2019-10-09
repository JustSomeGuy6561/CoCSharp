using CoC.Backend;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Items.Consumables
{
	public abstract partial class EggBase
	{
		protected string ShortDescription()
		{
			return colorStr() + " egg";
		}
	}
}
