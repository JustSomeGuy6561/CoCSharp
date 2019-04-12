using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Areas
{
	public abstract class AreaBase
	{
		public readonly SimpleDescriptor name;

		private protected AreaBase(SimpleDescriptor areaName)
		{
			name = areaName ?? throw new ArgumentNullException();
		}
	}
}
