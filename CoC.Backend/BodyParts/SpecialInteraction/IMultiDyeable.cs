//IMultiDyeable.cs
//Description:
//Author: JustSomeGuy
//4/17/2019, 2:36 AM
using CoC.Backend.CoC_Colors;
using CoC.Backend.Tools;
using System.Collections.Generic;

namespace CoC.Backend.BodyParts.SpecialInteraction
{
	//interface for a class with multiple dyeable members. This acts like the standard dyeable interface, but with multiple members on the same class.
	//it's possible to extend this to allow behaviors to work on multiple members at once, but that is up to the implementer. eventually body will allow you to do this.
	public interface IMultiDyeable
	{
		byte numDyeableMembers { get; }

		string buttonText();

		string memberButtonText(byte index);

		string memberLocationDesc(byte index, out bool isPlural);
		string memberPostDyeDescription(byte index);

		//check if the current member at the given index allows a dye - types may change and thus no longer allow dyeing.
		bool allowsDye(byte index);

		//check if the color is different from the given color. if this is true, we will allow a dye, but warn the player the color is the same.
		bool isDifferentColor(HairFurColors dyeColor, byte index);

		bool attemptToDye(HairFurColors dye, byte index);
		string LocationDesc();
	}

	//allows you to override the text displayed during dyeing
	public interface IMultiDyeableCustomText : IMultiDyeable
	{
		string DisplayResults(HairFurColors dyeColor, byte index, bool successful);
	}

	//extends multi-dyeable entries with the ability to apply a pattern. should only be applied to things that can be patterned (namely, fur colors).
	public interface IMultiPatternable : IMultiDyeable
	{
		bool allowsPatterning(byte index);

		bool isDifferentPrimaryColor(HairFurColors dyeColor, byte index);
		bool isDifferentSecondaryColor(HairFurColors dyeColor, byte index);

		bool attemptToPattern(HairFurColors dyeColor, FurMulticolorPattern pattern, bool primaryColor, byte index);
	}

	public interface IMultiPatternableCustomText : IMultiPatternable
	{
		string DisplayResults(HairFurColors dyeColor, FurMulticolorPattern pattern, bool primaryColor, byte index, bool successful);

	}
}
