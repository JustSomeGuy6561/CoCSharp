//Basilisk.cs
//Description:
//Author: JustSomeGuy
//2/19/2019, 9:52 AM
using System;
using System.Collections.Generic;
using System.Text;
using CoC.BodyParts;
using CoC.EpidermalColors;
using CoC.Tools;

namespace CoC.Creatures
{

	public class Basilisk : Species
	{
		public HairFurColors defaultSpines = HairFurColors.GREEN;
		public HairFurColors defaultPlume = HairFurColors.RED;
		public Tones defaultTone => Tones.GREEN;
		public EyeColor defaultEyeColor = EyeColor.BLUE;
		internal Basilisk(SimpleDescriptor name) : base(BasiliskStr) { }
	}

	public class Bee : Species
	{
		public HairFurColors defaultHair = HairFurColors.BLACK;
		public FurColor defaultFur = FurColor.Generate(HairFurColors.BLACK, HairFurColors.YELLOW, FurMulticolorPattern.STRIPED);
		public Tones defaultTone => Tones.BLACK;
		public EyeColor defaultEyeColor = EyeColor.BLUE;
		public Basilisk(SimpleDescriptor name) : base(BasiliskStr) { }
	}

	internal class Basilisk : Species
	{
		public HairFurColors defaultSpines = HairFurColors.GREEN;
		public HairFurColors defaultPlume = HairFurColors.RED;
		public Tones defaultTone => Tones.GREEN;
		public EyeColor defaultEyeColor = EyeColor.BLUE;
		public Basilisk(SimpleDescriptor name) : base(BasiliskStr) { }
	}

	internal class Basilisk : Species
	{
		public HairFurColors defaultSpines = HairFurColors.GREEN;
		public HairFurColors defaultPlume = HairFurColors.RED;
		public Tones defaultTone => Tones.GREEN;
		public EyeColor defaultEyeColor = EyeColor.BLUE;
		public Basilisk(SimpleDescriptor name) : base(BasiliskStr) { }
	}
}
