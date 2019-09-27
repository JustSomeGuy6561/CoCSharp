using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts.EventHelpers
{
	public class SimpleDataChangeEvent<Source, Data> : EventArgs where Source : SimpleSaveablePart<Source, Data> where Data:SimpleData
	{
		public readonly Data oldValues;
		public readonly Data newValues;

		public SimpleDataChangeEvent(Data oldData, Data newData)
		{
			oldValues = oldData ?? throw new ArgumentNullException(nameof(oldData));
			newValues = newData ?? throw new ArgumentNullException(nameof(newData));
		}
	}

}
