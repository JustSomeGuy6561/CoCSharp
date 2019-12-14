//IDyeable.cs
//Description: Interface attached to objects that can by dyed. Ensures they have the functions used for dyeing.
//Author: JustSomeGuy
//12/30/2018, 12:28 AM
using CoC.Backend.CoC_Colors;

namespace  CoC.Backend.BodyParts.SpecialInteraction
{
	public interface IDyeable
	{
		string buttonText();

		string locationDesc(out bool isPlural);
		string postDyeDescription();

		bool allowsDye();

		bool isDifferentColor(HairFurColors dyeColor);

		bool attemptToDye(HairFurColors dyeColor);
	}

	public interface IDyeableCustomText : IDyeable
	{
		string DisplayResults(HairFurColors dyeColor, bool successful);
	}

	public interface IPatternable : IDyeable
	{
		bool allowsPatterning();

		bool isDifferetPrimaryColor(HairFurColors dyeColor);
		bool isDifferentSecondaryColor(HairFurColors dyeColor);

		bool attemptToPattern(HairFurColors dyeColor, FurMulticolorPattern pattern, bool primaryColor);

	}

	public interface IPatternableCustomText : IPatternable
	{
		string DisplayResults(HairFurColors dyeColor, FurMulticolorPattern pattern, bool primaryColor, bool successful);

	}
}
