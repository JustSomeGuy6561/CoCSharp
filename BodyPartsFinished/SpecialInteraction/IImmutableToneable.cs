//IImmutableToneable.cs
//Description: Immutable version of ITonable. So all tonable, immutable objects follow the same format.
//Author: JustSomeGuy
//12/31/2018, 1:23 AM
using CoC.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.BodyParts.SpecialInteraction
{
	interface IImmutableToneable
	{
		bool canTone();
		bool tryToTone(ref Tones currentTone, Tones newTone);
	}
}
