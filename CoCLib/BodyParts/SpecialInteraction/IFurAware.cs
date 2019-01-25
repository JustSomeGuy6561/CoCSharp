//IFurAware.cs
//Description:
//Author: JustSomeGuy
//1/3/2019, 7:31 PM
using CoC.EpidermalColors;

namespace  CoC.BodyParts.SpecialInteraction
{
	internal interface IFurAware
	{
		//stores current fur color. 
		void reactToChangeInFurColor(object sender, FurAwareEventArg e);
	}
}
