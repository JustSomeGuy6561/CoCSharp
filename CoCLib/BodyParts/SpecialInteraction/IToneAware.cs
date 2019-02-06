//IToneAware.cs
//Description:
//Author: JustSomeGuy
//1/3/2019, 7:32 PM

namespace CoC.BodyParts.SpecialInteraction
{
	internal interface IToneAware
	{
		void reactToChangeInSkinTone(object sender, ToneAwareEventArg e);
	}
}
