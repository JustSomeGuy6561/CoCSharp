//IMasculinityAware.cs
//Description:
//Author: JustSomeGuy
//1/6/2019, 12:10 AM

//Horns.cs
//Description:
//Author: JustSomeGuy
//12/28/2018, 1:50 AM
namespace CoC.BodyParts.SpecialInteraction
{
	//marks a body part as aware of changes in masculinity or femininity.
	//they may act differently if the player appears feminine or masculine. 
	public interface IMasculinityChangeAware
	{
		void reactToChangesInMasculinity(int masculinity);
	}
}