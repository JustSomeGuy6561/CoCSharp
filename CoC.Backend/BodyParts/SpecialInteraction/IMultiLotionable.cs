//IMultiLotionable.cs
//Description:
//Author: JustSomeGuy
//4/24/2019, 12:39 AM
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts.SpecialInteraction
{
	//a new interface, so that you an get a soft face or freckled face or something, and another complexion or texture on the body.
	public interface IMultiLotionable
	{
		byte numLotionableMembers { get; }

		bool canLotion(byte index);
		bool isDifferentTexture(SkinTexture lotionTexture, byte index);
		bool attemptToLotion(SkinTexture lotionTexture, byte index);

		string buttonText(byte index);
		string locationDesc(byte index);

	}

	public interface ISimultaneousMultiLotionable : IMultiLotionable
	{
		//bool allowMultiDye = true
	}

	public interface IMultiLotionableCustomText : ISimultaneousMultiLotionable
	{
		string ApplySingleLotion(SkinTexture lotionTexture, byte index);
		string ApplyMultiLotion(SkinTexture lotionTexture, params byte[] index);
	}

	//i don't feel like forcing this on to the interface itself, as it's identical everywhere, and may not ever be used.
	//i probably should give it a bool to disable this behavior from being possible if that's the desired result.
	public static class MultiLotionableHelper
	{
		public static bool[] attemptMultiLotion(this IMultiLotionable multiLotionable, SkinTexture lotion, params byte[] indices)
		{
			if (multiLotionable == null) throw new System.ArgumentNullException();
			if (indices == null) indices = new byte[0];
			bool[] returnedValues = new bool[indices.Length];

			for (int x = 0; x < indices.Length; x++)
			{
				returnedValues[x] = multiLotionable.attemptToLotion(lotion, indices[x]);
			}
			return returnedValues;
		}

		public static Dictionary<byte, string> AllLocations(this IMultiLotionable multiLotionable)
		{
			Dictionary<byte, string> returnValue = new Dictionary<byte, string>();
			for (byte x = 0; x < multiLotionable.numLotionableMembers; x++)
			{
				returnValue.Add(x, multiLotionable.locationDesc(x));
			}
			return returnValue;
		}

		public static Dictionary<byte, string> AllButtons(this IMultiLotionable multi)
		{
			Dictionary<byte, string> retVal = new Dictionary<byte, string>();
			for (byte x = 0; x < multi.numLotionableMembers; x++)
			{
				retVal.Add(x, multi.buttonText(x));
			}
			return retVal;
		}

		//

	}
}
