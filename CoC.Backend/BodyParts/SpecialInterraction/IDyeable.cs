//IDyeable.cs
//Description: Interface attached to objects that can by dyed. Ensures they have the functions used for dyeing.
//Author: JustSomeGuy
//12/30/2018, 12:28 AM
using CoC.Backend.CoC_Colors;

namespace  CoC.Backend.BodyParts.SpecialInteraction
{
	interface IDyeable
	{
		bool canDye();

		bool attemptToDye(HairFurColors dye);
	}
}
