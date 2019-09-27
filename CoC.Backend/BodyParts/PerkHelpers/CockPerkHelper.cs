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

		public CockPerkHelper(float newCockDefaultSize, float newCockSizeDelta, float minCockLength)
		{
			NewCockDefaultSize = newCockDefaultSize;
			NewCockSizeDelta = newCockSizeDelta;
			MinCockLength = minCockLength;
		}
	}
}
