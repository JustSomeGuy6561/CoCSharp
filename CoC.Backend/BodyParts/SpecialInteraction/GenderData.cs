//GenderData.cs
//Description:
//Author: JustSomeGuy
//3/24/2019, 1:03 AM
using CoC.Backend.Tools;

namespace CoC.Backend.BodyParts.SpecialInteraction
{
	public sealed class FemininityData
	{
		public const byte MOST_FEMININE = Femininity.MOST_FEMININE;
		public const byte MOST_MASCULINE = Femininity.MOST_MASCULINE;

		public const byte MIN_ANDROGYNOUS = Femininity.MIN_ANDROGYNOUS;
		public const byte ANDROGYNOUS = Femininity.ANDROGYNOUS;
		public const byte MAX_ANDROGYNOUS = Femininity.MAX_ANDROGYNOUS;

		public const byte SLIGHTLY_FEMININE = Femininity.SLIGHTLY_FEMININE;
		public const byte FEMININE = Femininity.FEMININE;
		public const byte HYPER_FEMININE = Femininity.HYPER_FEMININE;

		public const byte SLIGHTLY_MASCULINE = Femininity.SLIGHTLY_MASCULINE;
		public const byte MASCULINE = Femininity.MASCULINE;
		public const byte HYPER_MASCULINE = Femininity.HYPER_MASCULINE;

		public readonly byte femininity;

		//enums are passed by value, so this should be fine.
		internal FemininityData(Femininity fem)
		{
			femininity = fem;
		}

		internal FemininityData(byte fem)
		{
			femininity = fem;
		}

		public bool isFemale => Femininity.valueIsFemale(femininity);
		public bool isMale => Femininity.valueIsMale(femininity);

		public bool isAndrogynous => Femininity.valueIsAndrogynous(femininity);

		public bool isSlightlyFeminine => Femininity.valueIsSlightlyFeminine(femininity);
		public bool atLeastSlightlyFeminine => Femininity.valueAtLeastSlightlyFeminine(femininity);
		public bool isFeminine => Femininity.valueIsFeminine(femininity);
		public bool atLeastFeminine => Femininity.valueAtLeastFeminine(femininity);
		public bool isHyperFeminine => Femininity.valueIsHyperFeminine(femininity);
		public bool isSlightlyMasculine => Femininity.valueIsSlightlyMasculine(femininity);
		public bool atLeastSlightlyMasculine => Femininity.valueAtLeastSlightlyMasculine(femininity);
		public bool isMasculine => Femininity.valueIsMasculine(femininity);
		public bool atLeastMasculine => Femininity.valueAtLeastMasculine(femininity);
		public bool isHyperMasculine => Femininity.valueIsHyperMasculine(femininity);
	}
}
