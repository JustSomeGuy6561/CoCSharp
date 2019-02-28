//IMasculinityAware.cs
//Description:
//Author: JustSomeGuy
//1/6/2019, 12:10 AM

namespace  CoC.Backend.BodyParts.SpecialInteraction
{
	//marks a body part as aware of changes in masculinity or femininity.
	//they may act differently if the player appears feminine or masculine. 
	internal interface IMasculinityChangeAware
	{
		void reactToChangesInMasculinity(object sender, FemininityData masculinity);
	}
}