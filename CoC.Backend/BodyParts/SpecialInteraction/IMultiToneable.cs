//IMultiToneable.cs
//Description:
//Author: JustSomeGuy
//4/17/2019, 2:39 AM
using CoC.Backend.CoC_Colors;
using System;
using System.Collections.Generic;

namespace CoC.Backend.BodyParts.SpecialInteraction
{
	public interface IMultiToneable
	{
		byte numToneableMembers { get; }

		string buttonText(byte index);
		string locationDesc(byte index);

		bool canToneOil(byte index);

		bool isDifferentTone(Tones oilTone, byte index);

		bool attemptToTone(Tones oilTone, byte index);

	}
	public interface ISimultaneousMultiToneable : IMultiToneable
	{
		//bool allowMultiDye = true
	}
	public interface IMultiToneableCustomText : ISimultaneousMultiToneable
	{
		string ApplySingleTone(Tones oilTone, byte index);
		string ApplyMultiTone(Tones oilTone, params byte[] index); //ignored if allowMultiTone is false.
	}

	//i don't feel like forcing this on to the interface itself, as it's identical everywhere, and may not ever be used.
	//i probably should give it a bool to disable this behavior from being possible if that's the desired result.
	public static class MultiToneHelper
	{
		public static bool[] attemptMultiTone(this IMultiToneable multiToneable, Tones lotionTone, params byte[] indices)
		{
			if (lotionTone == null) throw new ArgumentNullException();
			if (multiToneable == null) throw new ArgumentNullException();
			if (indices == null) indices = new byte[0];
			bool[] returnedValues = new bool[indices.Length];

			for (int x = 0; x < indices.Length; x++)
			{
				returnedValues[x] = multiToneable.attemptToTone(lotionTone, indices[x]);
			}
			return returnedValues;
		}

		public static Dictionary<byte, string> AllLocations(this IMultiToneable multiToneable)
		{
			Dictionary<byte, string> returnValue = new Dictionary<byte, string>();
			for (byte x = 0; x < multiToneable.numToneableMembers; x++)
			{
				returnValue.Add(x, multiToneable.locationDesc(x));
			}
			return returnValue;
		}

		public static Dictionary<byte, string> AllButtons(this IMultiToneable multi)
		{
			Dictionary<byte, string> retVal = new Dictionary<byte, string>();
			for (byte x = 0; x < multi.numToneableMembers; x++)
			{
				retVal.Add(x, multi.buttonText(x));
			}
			return retVal;
		}

	}
}
