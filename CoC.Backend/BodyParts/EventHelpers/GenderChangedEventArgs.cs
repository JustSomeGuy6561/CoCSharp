using CoC.Backend.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts.EventHelpers
{
	public sealed class GenderChangedEventArgs : EventArgs
	{
		public readonly Gender oldGender;
		public readonly Gender newGender;

		public GenderChangedEventArgs(Gender oldValue, Gender newValue)
		{
			oldGender = oldValue;
			newGender = newValue;
		}
	}
}
