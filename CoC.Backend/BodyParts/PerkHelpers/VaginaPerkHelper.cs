//Vagina.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 5:57 PM

using CoC.Backend.Tools;
using System;

namespace CoC.Backend.BodyParts
{
	internal sealed class VaginaPerkHelper
	{
		internal float NewClitSizeDelta = 0;
		internal float ClitGrowthMultiplier = 1;
		internal float ClitShrinkMultiplier = 1;
		internal float DefaultNewClitSize = Clit.DEFAULT_CLIT_SIZE;
		internal float MinClitSize = Clit.DEFAULT_CLIT_SIZE;
		internal float NewClitSize(float? givenSize = null)
		{
			float minValue = Utils.Clamp2(Math.Max(DefaultNewClitSize, MinClitSize), Clit.MIN_CLIT_SIZE, Clit.MAX_CLIT_SIZE);
			if (givenSize != null)
			{
				givenSize += NewClitSizeDelta;
			}
			if (givenSize is null || givenSize < minValue)
			{
				return minValue;
			}
			else
			{
				return (float)givenSize;
			}
		}

		internal VaginalWetness defaultWetnessNew = VaginalWetness.DRY;
		internal VaginalLooseness defaultLoosenessNew = VaginalLooseness.TIGHT;
		internal ushort perkBonusCapacity = 0;
		internal VaginalLooseness minLooseness = VaginalLooseness.TIGHT;
		internal VaginalLooseness maxLooseness = VaginalLooseness.CLOWN_CAR_WIDE;
		internal VaginalWetness minWetness = VaginalWetness.DRY;
		internal VaginalWetness maxWetness = VaginalWetness.SLAVERING;


		public VaginaPerkHelper()
		{
		}

		public VaginaPerkHelper(float newClitSizeDelta, float clitGrowthMultiplier, float clitShrinkMultiplier, float minNewClitSize, 
			float minClitSize, VaginalWetness defaultWetnessNew, VaginalLooseness defaultLoosenessNew, ushort perkBonusCapacity,
			VaginalLooseness minLooseness, VaginalLooseness maxLooseness, VaginalWetness minWetness, VaginalWetness maxWetness)
		{
			NewClitSizeDelta = newClitSizeDelta;
			ClitGrowthMultiplier = clitGrowthMultiplier;
			ClitShrinkMultiplier = clitShrinkMultiplier;
			DefaultNewClitSize = minNewClitSize;
			MinClitSize = minClitSize;
			this.defaultWetnessNew = defaultWetnessNew;
			this.defaultLoosenessNew = defaultLoosenessNew;
			this.perkBonusCapacity = perkBonusCapacity;
			this.minLooseness = minLooseness;
			this.maxLooseness = maxLooseness;
			this.minWetness = minWetness;
			this.maxWetness = maxWetness;
		}
	}
}
