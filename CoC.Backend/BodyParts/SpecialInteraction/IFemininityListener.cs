//IFemininityListener.cs
//Description:
//Author: JustSomeGuy
//3/27/2019, 12:07 AM
namespace CoC.Backend.BodyParts.SpecialInteraction
{
	internal interface IFemininityListener : IFemininityAware
	{
		//output a string to match whatever you did. If it's not the player, this string will not be used. 
		string reactToChangeInFemininity(byte oldFemininity);
	}
}
