﻿//IGrowShrinkable.cs
//Description: Interface for body parts can that react to Gro+ and/or Reducto.
//Author: JustSomeGuy
//12/30/2018, 12:28 AM

namespace  CoC.Backend.BodyParts.SpecialInteraction
{
	interface IGrowable
	{
		bool CanGroPlus();
		float UseGroPlus();
	}

	interface IShrinkable
	{
		bool CanReducto();
		float UseReducto();
	}


}
