//IPiercable.cs
//Description: Interface marking body parts as pierceable.
//Author: JustSomeGuy
//12/30/2018, 12:34 AM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.BodyParts.Special_Interaction
{
	//look, i went with a more involved approach before, 
	interface IPiercable
	{
		int maxPiercingCount { get; }

		int currentPiercingCount { get; }

		int Pierce(int numPiercings, int piercingStyle);

		bool equipPiercingJewelry(int piercingIndex, bool force = false);


	}
}
