//Cock.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 10:55 PM


using CoC.Backend.Tools;
using System;

namespace CoC.Backend.BodyParts
{
	internal sealed class CockPerkHelper
	{
		//how much more/less should we grow a cock over the base amount? //big cock perk, cockSock;
		public float CockGrowthMultiplier = 1.0f;

		//how much more/less should we shrink a cock over base amount? //big cock, cockSock;
		public float CockShrinkMultiplier = 1.0f;

		//minimum size for any new cocks; //bro/futa perks for now
		public float NewCockDefaultSize = Cock.DEFAULT_COCK_LENGTH;
		public float NewCockSizeDelta = 0;

		public float MinCockLength = Cock.MIN_COCK_LENGTH;

		public float NewLength(float? givenLength = null)
		{
			float minLength = Utils.Clamp2(Math.Max(MinCockLength, NewCockDefaultSize), Cock.MIN_COCK_LENGTH, Cock.MAX_COCK_LENGTH);
			if (givenLength != null)
			{
				givenLength += NewCockSizeDelta;
			}
			if (givenLength is null || givenLength < minLength)
			{
				return minLength;
			}
			else
			{
				return (float)givenLength;
			}
		}

		public CockPerkHelper()
		{
		}

		public CockPerkHelper(float cockGrowthMultiplier, float cockShrinkMultiplier, float newCockDefaultSize, float newCockSizeDelta, float minCockLength)
		{
			CockGrowthMultiplier = cockGrowthMultiplier;
			CockShrinkMultiplier = cockShrinkMultiplier;
			NewCockDefaultSize = newCockDefaultSize;
			NewCockSizeDelta = newCockSizeDelta;
			MinCockLength = minCockLength;
		}
	}
}
