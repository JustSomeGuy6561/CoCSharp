//IToneAware.cs
//Description:
//Author: JustSomeGuy
//1/3/2019, 7:32 PM
using CoC.EpidermalColors;

namespace CoC.BodyParts.SpecialInteraction
{
	public interface IToneAware
	{
		void reactToChangeInSkinTone(object sender, ToneAwareEventArg e);
	}
}
