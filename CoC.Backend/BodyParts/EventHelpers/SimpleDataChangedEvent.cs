using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts.EventHelpers
{
	public class SimpleDataChangedEvent<T, U> : EventArgs
	{
		public readonly T dataSource;
		public readonly U oldData;

		public SimpleDataChangedEvent(T dataSource, U oldData)
		{
			this.dataSource = dataSource;
			this.oldData = oldData;
		}
	}
}
