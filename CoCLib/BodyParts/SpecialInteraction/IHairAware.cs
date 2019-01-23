//IHairAware.cs
//Description:
//Author: JustSomeGuy
//1/19/2019, 12:00 AM
namespace CoC.BodyParts.SpecialInteraction
{
	public interface IHairAware
	{
		void reactToChangeInHairColor(object sender, HairColorEventArg e);
	}
}
