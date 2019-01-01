//IGrowShrinkable.cs
//Description: Interface for body parts can that react to Gro+ and/or Reducto. 
//Author: JustSomeGuy
//12/30/2018, 12:28 AM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.BodyParts.SpecialInteraction
{
	interface IGrowShrinkable
	{
		bool CanReducto();
		int UseReducto();
		bool CanGrowPlus();
		int UseGroPlus();

	}
}
