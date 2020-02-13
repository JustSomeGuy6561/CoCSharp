//CeraphPiercings.cs
//Description:
//Author: JustSomeGuy
//6/18/2019, 7:42 AM
using CoC.Backend.BodyParts;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Items.Materials;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Frontend.Items.Materials.Jewelry;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Items.Wearables.Piercings
{
	internal sealed class CeraphEarPiercings : PiercingJewelry
	{
		public CeraphEarPiercings() : base(JewelryType.BARBELL_STUD, new Obsidian(), false) { }
	}


	internal sealed class CeraphNipplePiercings : PiercingJewelry
	{
		public CeraphNipplePiercings() : base(JewelryType.BARBELL_STUD, new Obsidian(), false) {}

		public override bool CanEquipAt<T, U>(T piercable, U location)
		{
			return piercable is Piercing<NipplePiercingLocation>;
		}
	}

	//Prince albert
	internal sealed class CeraphCockPiercing : PiercingJewelry
	{
		public CeraphCockPiercing() : base(JewelryType.RING, new Obsidian(), false) { }

		public override bool CanEquipAt<T, U>(T piercable, U location)
		{
			return piercable is Piercing<CockPiercingLocation> && location is CockPiercingLocation cockPiercing && cockPiercing.Equals(CockPiercingLocation.PRINCE_ALBERT);
		}
	}

	//Clit Itself (ceraph is cruel like that)
	internal sealed class CeraphClitPiercing : PiercingJewelry
	{
		public CeraphClitPiercing() : base(JewelryType.RING, new Obsidian(), false) { }

		public override bool CanEquipAt<T, U>(T piercable, U location)
		{
			return location is ClitPiercingLocation clitPiercing && (clitPiercing == ClitPiercingLocation.CLIT_ITSELF || clitPiercing == ClitPiercingLocation.LARGE_CLIT_1
				|| clitPiercing == ClitPiercingLocation.LARGE_CLIT_2 || clitPiercing == ClitPiercingLocation.LARGE_CLIT_3);
		}
	}

	internal sealed class CeraphEyebrowPiercing : PiercingJewelry
	{
		public CeraphEyebrowPiercing() : base(JewelryType.BARBELL_STUD, new Obsidian(), false) { }

		public override bool CanEquipAt<T, U>(T piercable, U location)
		{
			return location is EyebrowPiercingLocation;
		}
	}
}
