using CoC.Backend.CoC_Colors;
using CoC.Backend.Tools;
using System.Collections.Generic;

namespace CoC.Backend.BodyParts.SpecialInteraction
{
	//interface for a class with multiple dyeable members. This acts like the standard dyeable interface, but with multiple members on the same class.
	//Optionally, it can also allow you to dye multiple members at once (though this is NYI atm).
	public interface IMultiDyeable
	{

		byte numDyeableMembers { get; }

		string buttonText(byte index);
		string locationDesc(byte index);

		bool allowsDye(byte index);

		bool isDifferentColor(HairFurColors dyeColor, byte index);

		bool attemptToDye(HairFurColors dye, byte index);

		//allows a single dye to be applied to multiple members at once. For example, the body sets this value to true, allowing the player
		//to simultaneously dye their primary, secondary, and underbody fur/feather colors all at once. 
		//bool allowMultiDye = false
	}

	public interface ISimultaneousMultiDyeable : IMultiDyeable
	{
		//bool allowMultiDye = true
	}

	//process: runs dye code like normal. before completing dye, checks for patternable. if patternable and conditions met, asks if PC wants to apply colors in a pattern.
	//stores the PC choice. runs attempt to dye. 
	public interface IPatternable : ISimultaneousMultiDyeable
	{
		bool canPattern(HairFurColors dye, params byte[] indices);
		bool isDifferentPattern(HairFurColors dye, FurMulticolorPattern pattern, params byte[] indices);
		bool attemptToPatternPostDye(HairFurColors dye, FurMulticolorPattern pattern, params byte[] indices);
	}

	//allows you to override the text displayed during dyeing
	public interface IMultiDyeableCustomText : ISimultaneousMultiDyeable
	{
		string ApplySingleDye(HairFurColors dyeColor, byte index);
		string ApplyMultiDye(HairFurColors dyeColor, params byte[] index); //ignored if allowMultiDye is false.
	}

	//i don't feel like forcing this on to the interface itself, as it's identical everywhere, and may not ever be used.
	//i probably should give it a bool to disable this behavior from being possible if that's the desired result. 
	public static class MultiDyeableHelper
	{
		public static bool[] attemptMultiDye(this IMultiDyeable multiDyeable, HairFurColors dye, params byte[] indices)
		{
			if (dye == null) throw new System.ArgumentNullException();
			if (multiDyeable == null) throw new System.ArgumentNullException();
			if (indices == null) indices = new byte[0];
			bool[] returnedValues = new bool[indices.Length]; 

			for (int x = 0; x < indices.Length; x++)
			{
				returnedValues[x] = multiDyeable.attemptToDye(dye, indices[x]);
			}
			return returnedValues;
		}

		public static Dictionary<byte, string> AllLocations(this IMultiDyeable multiDyeable)
		{
			Dictionary<byte, string> returnValue = new Dictionary<byte, string>();
			for (byte x = 0; x < multiDyeable.numDyeableMembers; x++)
			{
				returnValue.Add(x, multiDyeable.locationDesc(x));
			}
			return returnValue;
		}

		public static Dictionary<byte, string> AllButtons(this IMultiDyeable multi)
		{
			Dictionary<byte, string> retVal = new Dictionary<byte, string>();
			for (byte x = 0; x < multi.numDyeableMembers; x++)
			{
				retVal.Add(x, multi.buttonText(x));
			}
			return retVal;
		}

		//

	}

}
