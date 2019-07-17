//CeraphPiercings.cs
//Description:
//Author: JustSomeGuy
//6/18/2019, 7:42 AM
using CoC.Backend.Items.Materials;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Frontend.Items.Materials.Jewelry;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Strings.Items.Wearables.Piercings
{
	internal sealed class CeraphEarPiercings : PiercingJewelry
	{
		public CeraphEarPiercings() : base(JewelryType.BARBELL_STUD, new Obsidian(), false) { }
	}


	internal sealed class CeraphNipplePiercings : PiercingJewelry
	{
		public CeraphNipplePiercings() : base(JewelryType.BARBELL_STUD, new Obsidian(), false) {}
	}

	//Prince albert
	internal sealed class CeraphCockPiercing : PiercingJewelry
	{
		public CeraphCockPiercing() : base(JewelryType.RING, new Obsidian(), false) { }
	}

	//Clit Itself (ceraph is cruel like that)
	internal sealed class CeraphClitPiercing : PiercingJewelry
	{
		public CeraphClitPiercing() : base(JewelryType.RING, new Obsidian(), false) { }
	}

	internal sealed class CeraphEyebrowPiercing : PiercingJewelry
	{
		public CeraphEyebrowPiercing() : base(JewelryType.BARBELL_STUD, new Obsidian(), false) { }
	}
}
