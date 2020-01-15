//Vagina.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 5:57 PM
using CoC.Backend.Tools;

namespace CoC.Backend.BodyParts
{

	internal sealed class BreastPerkHelper
	{
		internal sbyte FemaleNewCupDelta = 0;
		internal CupSize FemaleNewDefaultCup = Breasts.DEFAULT_FEMALE_SIZE;
		internal sbyte MaleNewCupDelta = 0;
		internal CupSize MaleNewDefaultCup = Breasts.DEFAULT_MALE_SIZE;
		internal CupSize FemaleMinCup = CupSize.FLAT;
		internal CupSize MaleMinCup = CupSize.FLAT;

		internal CupSize MaleNewLength(CupSize? givenCup = null)
		{
			CupSize minCup = Utils.ClampEnum2(EnumHelper.Max(MaleNewDefaultCup, MaleMinCup), CupSize.FLAT, CupSize.JACQUES00);
			if (givenCup is null || givenCup < minCup)
			{
				return minCup;
			}
			return (CupSize)givenCup;
		}

		internal CupSize FemaleNewLength(CupSize? givenCup = null)
		{
			var minCup = Utils.ClampEnum2(EnumHelper.Max(FemaleNewDefaultCup, FemaleMinCup), CupSize.FLAT, CupSize.JACQUES00);
			if (givenCup is null || givenCup < minCup)
			{
				return minCup;
			}
			return (CupSize)givenCup;
		}

		internal float TitsGrowthMultiplier = 1;
		internal float TitsShrinkMultiplier = 1;

		internal float NewNippleSizeDelta = 0;
		internal float NippleGrowthMultiplier = 1;
		internal float NippleShrinkMultiplier = 1;
		internal float NewNippleDefaultLength = Nipples.MIN_NIPPLE_LENGTH;

		public BreastPerkHelper()
		{
		}

		public BreastPerkHelper(sbyte femaleNewCupDelta, CupSize femaleNewDefaultCup, sbyte maleNewCupDelta, CupSize maleNewDefaultCup,
			CupSize femaleMinCup, CupSize maleMinCup, float titsGrowthMultiplier, float titsShrinkMultiplier, float newNippleSizeDelta,
			float nippleGrowthMultiplier, float nippleShrinkMultiplier, float newNippleDefaultLength)
		{
			FemaleNewCupDelta = femaleNewCupDelta;
			FemaleNewDefaultCup = femaleNewDefaultCup;
			MaleNewCupDelta = maleNewCupDelta;
			MaleNewDefaultCup = maleNewDefaultCup;
			FemaleMinCup = femaleMinCup;
			MaleMinCup = maleMinCup;
			TitsGrowthMultiplier = titsGrowthMultiplier;
			TitsShrinkMultiplier = titsShrinkMultiplier;
			NewNippleSizeDelta = newNippleSizeDelta;
			NippleGrowthMultiplier = nippleGrowthMultiplier;
			NippleShrinkMultiplier = nippleShrinkMultiplier;
			NewNippleDefaultLength = newNippleDefaultLength;
		}
	}
}
