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
		public readonly Gender currentGender;

		//enums are passed by value, so this should be fine.
		internal FemininityData(Femininity fem)
		{
			femininity = fem;
			currentGender = fem.currentGender;
		}

		public bool isFemale => atLeastSlightlyFeminine;
		public bool isMale => atLeastSlightlyMasculine;

		public bool isAndrogynous => femininity >= MIN_ANDROGYNOUS && femininity <= MAX_ANDROGYNOUS;

		public bool isSlightlyFeminine => femininity >= SLIGHTLY_FEMININE && femininity < FEMININE;
		public bool atLeastSlightlyFeminine => femininity >= SLIGHTLY_FEMININE && femininity < FEMININE;
		public bool isFeminine => femininity >= FEMININE && femininity < HYPER_FEMININE;
		public bool atLeastFeminine => femininity >= FEMININE;
		public bool isHyperFeminine => femininity >= HYPER_FEMININE;
		public bool isSlightlyMasculine => femininity <= SLIGHTLY_MASCULINE && femininity > MASCULINE;
		public bool atLeastSlightlyMasculine => femininity <= SLIGHTLY_MASCULINE;
		public bool isMasculine => femininity <= MASCULINE && femininity > HYPER_MASCULINE;
		public bool atLeastMasculine => femininity <= MASCULINE;
		public bool isHyperMasculine => femininity <= HYPER_MASCULINE;
	}
}
