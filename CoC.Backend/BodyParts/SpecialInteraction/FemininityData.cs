using CoC.Backend.Tools;

namespace CoC.Backend.BodyParts.SpecialInteraction
{
	public sealed class FemininityData
	{
		public const int MIN_ANDROGENOUS = 35;
		public const int ANDROGENOUS = 50;
		public const int MAX_ANDROGENOUS = 65;

		public const int SLIGHTLY_FEMININE = 60;
		public const int FEMININE = 70;
		public const int HYPER_FEMININE = 90;

		public const int SLIGHTLY_MASCULINE = 40;
		public const int MASCULINE = 30;
		public const int HYPER_MASCULINE = 10;


		public int femininity
		{
			get => _femininity;
			private set
			{
				Utils.Clamp(ref value, 0, 100);
				_femininity = value;
			}
		}
		private int _femininity;
		internal FemininityData(int femValue = 50)
		{
			femininity = femValue;
		}

		public static implicit operator int(FemininityData femininity)
		{
			return femininity.femininity;
		}

		public int feminize(uint amount)
		{
			if (femininity == 100)
			{
				return 0;
			}
			int oldFemininity = femininity;
			femininity += (int)amount;
			return femininity - oldFemininity;
		}

		public int masculinize(uint amount)
		{
			if (femininity == 0)
			{
				return 0;
			}
			int oldFemininity = femininity;
			femininity -= (int)amount;
			return oldFemininity - femininity;
		}

		public void Update(FemininityData other)
		{
			femininity = other.femininity;
		}

		public bool isFemale => atLeastSlightlyFeminine;
		public bool isMale => atLeastSlightlyMasculine;

		public bool isAndrogenous => femininity >= MIN_ANDROGENOUS && femininity <= MAX_ANDROGENOUS;

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
