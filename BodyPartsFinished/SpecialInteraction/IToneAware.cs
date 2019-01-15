//IToneAware.cs
//Description:
//Author: JustSomeGuy
//1/3/2019, 7:32 PM
using CoC.EpidermalColors;

namespace CoC.BodyParts.SpecialInteraction
{
	public interface IToneAware
	{
		/// <summary>
		/// A function allowing anything that implements it to react to changes in the core skin tones. 
		/// </summary>
		/// <param name="newPrimary">The new primary tone. it will never be empty</param>
		/// <param name="primaryToneInUse">true if the current epidermis for the body uses the skin tone</param>
		/// <param name="newSecondary">the new secondary tone. it may be empty</param>
		void reactToChangeInSkinTone(Tones newPrimary, bool primaryToneInUse, Tones newSecondary);
	}
}
