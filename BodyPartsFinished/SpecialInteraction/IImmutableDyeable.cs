//IImmutableDyeable.cs
//Description: Immutable version of IDyeable class, so immutable objects follow the same format.
//Author: JustSomeGuy
//12/31/2018, 1:22 AM
using CoC.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.BodyParts.SpecialInteraction
{
	interface IImmutableDyeable
	{
		bool canDye();
		bool tryToDye(ref Dyes currentColor, Dyes newColor);
	}
}
