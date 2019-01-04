//IToneAware.cs
//Description:
//Author: JustSomeGuy
//1/3/2019, 7:32 PM
using CoC.EpidermalColors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.BodyParts.SpecialInteraction
{
	interface IToneAware
	{
		void reactToChangeInSkinTone(Tones newTone);
	}
}
