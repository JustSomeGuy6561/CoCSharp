//VaginaCreator.cs
//Description:
//Author: JustSomeGuy
//6/13/2019, 9:12 PM
using CoC.Backend.BodyParts;
using CoC.Backend.Items.Wearables.Piercings;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CoC.Backend.Creatures
{
	public sealed class VaginaCreator
	{
		public readonly VaginaType type;

		public float? clitLength;
		public readonly ReadOnlyDictionary<ClitPiercingLocation, PiercingJewelry> clitPiercings;
		public readonly ReadOnlyDictionary<LabiaPiercingLocation, PiercingJewelry> labiaPiercings;
		public VaginalWetness wetness;
		public VaginalLooseness looseness;
		public bool virgin;
		public readonly bool hasClitCock;
		public float validClitLength => clitLength ?? Clit.DEFAULT_CLIT_SIZE;

		public VaginaCreator(float? clitLengthInInches = null, VaginalWetness vaginalWetness = VaginalWetness.NORMAL,
			VaginalLooseness vaginalLooseness = VaginalLooseness.TIGHT, bool omnibusClit = false, bool isVirgin = true,
			Dictionary<ClitPiercingLocation, PiercingJewelry> clitJewelry = null, Dictionary<LabiaPiercingLocation, PiercingJewelry> labiaJewelry = null)
			: this(VaginaType.HUMAN, clitLengthInInches, vaginalWetness, vaginalLooseness, omnibusClit, isVirgin, clitJewelry, labiaJewelry) { }
		public VaginaCreator(VaginaType vaginaType, float? clitLengthInInches = null, VaginalWetness vaginalWetness = VaginalWetness.NORMAL,
			VaginalLooseness vaginalLooseness = VaginalLooseness.TIGHT, bool omnibusClit = false, bool isVirgin = true,
			Dictionary<ClitPiercingLocation, PiercingJewelry> clitJewelry = null, Dictionary<LabiaPiercingLocation, PiercingJewelry> labiaJewelry = null)
		{
			type = vaginaType;
			clitLength = clitLengthInInches;
			wetness = vaginalWetness;
			looseness = vaginalLooseness;
			hasClitCock = omnibusClit;
			clitPiercings = clitJewelry == null ? null : new ReadOnlyDictionary<ClitPiercingLocation, PiercingJewelry>(clitJewelry);
			labiaPiercings = labiaJewelry == null ? null : new ReadOnlyDictionary<LabiaPiercingLocation, PiercingJewelry>(labiaJewelry);
			virgin = isVirgin;
		}
	}


}
