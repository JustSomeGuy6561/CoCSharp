//ILotionable.cs
//Description:
//Author: JustSomeGuy
//4/24/2019, 12:34 AM

namespace CoC.Backend.BodyParts.SpecialInteraction
{
	//a new interface, so that you an get a soft face or freckled face or something, and another complexion or texture on the body.
	public interface ILotionable
	{
		string buttonText();

		string locationDesc(out bool isPlural);
		SkinTexture postUseSkinTexture();

		bool canLotion();
		bool isDifferentTexture(SkinTexture lotionTexture);
		bool attemptToLotion(SkinTexture lotionTexture);

	}

	public interface ILotionableCustomText : ILotionable
	{
		string DisplayResults(SkinTexture lotionTexture, bool successful);
	}


}
