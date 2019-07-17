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
		public readonly ReadOnlyDictionary<ClitPiercings, PiercingJewelry> clitPiercings;
		public readonly ReadOnlyDictionary<LabiaPiercings, PiercingJewelry> labiaPiercings;
		public VaginalWetness wetness;
		public VaginalLooseness looseness;
		public bool virgin;
		public float validClitLength => clitLength ?? Clit.DEFAULT_CLIT_SIZE;

		public VaginaCreator(float? clitLengthInInches = null, VaginalWetness vaginalWetness = VaginalWetness.NORMAL, VaginalLooseness vaginalLooseness = VaginalLooseness.TIGHT, 
			bool isVirgin = true, Dictionary<ClitPiercings, PiercingJewelry> clitJewelry = null, Dictionary<LabiaPiercings, PiercingJewelry> labiaJewelry = null)
			: this(VaginaType.HUMAN, clitLengthInInches, vaginalWetness, vaginalLooseness, isVirgin, clitJewelry, labiaJewelry) { }
		public VaginaCreator(VaginaType vaginaType, float? clitLengthInInches = null, VaginalWetness vaginalWetness = VaginalWetness.NORMAL, VaginalLooseness vaginalLooseness = VaginalLooseness.TIGHT,
			bool isVirgin = true, Dictionary<ClitPiercings, PiercingJewelry> clitJewelry = null, Dictionary<LabiaPiercings, PiercingJewelry> labiaJewelry = null)
		{
			type = vaginaType;
			clitLength = clitLengthInInches;
			wetness = vaginalWetness;
			looseness = vaginalLooseness;
			clitPiercings = clitJewelry == null ? null : new ReadOnlyDictionary<ClitPiercings, PiercingJewelry>(clitJewelry);
			labiaPiercings = labiaJewelry == null ? null : new ReadOnlyDictionary<LabiaPiercings, PiercingJewelry>(labiaJewelry);
			virgin = isVirgin;
		}
	}


}
