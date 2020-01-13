using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts
{
	public abstract class SimpleData
	{
		public readonly Guid creatureID;

		protected SimpleData(Guid creatureID)
		{
			this.creatureID = creatureID;
		}
	}
}
