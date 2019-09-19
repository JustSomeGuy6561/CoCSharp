using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts.SpecialInteraction
{
	internal interface IFemininityListenerInternal
	{
		string reactToFemininityChangeFromTimePassing(bool isPlayer, byte hoursPassed, byte oldFemininity);
	}
}
