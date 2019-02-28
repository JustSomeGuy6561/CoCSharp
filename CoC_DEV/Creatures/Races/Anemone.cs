//Anemone.cs
//Description:
//Author: JustSomeGuy
//2/19/2019, 9:42 AM
using System;
using System.Collections.Generic;
using System.Text;
using CoC.EpidermalColors;
using CoC.Tools;

namespace CoC.Creatures
{
	internal class Anemone : Species
	{
		public HairFurColors defaultHair => HairFurColors.CERULEAN;
		public Tones defaultTone => Tones.CERULEAN;
		public Anemone() : base(AnemoneStr) {}
	}
}
