//AreaBase.cs
//Description:
//Author: JustSomeGuy
//4/7/2019, 7:37 PM
using System;

namespace CoC.Backend.Areas
{
	public abstract class AreaBase
	{
		public readonly SimpleDescriptor name;

		private protected AreaBase(SimpleDescriptor areaName)
		{
			name = areaName ?? throw new ArgumentNullException();
		}

		public abstract void RunArea();

#warning Should make this virtual later.
		public abstract void Unlock();

	}
}
