//ILotionable.cs
//Description:
//Author: JustSomeGuy
//4/24/2019, 12:34 AM

namespace CoC.Backend.BodyParts.SpecialInteraction
{
	//a new interface, so that you an get a soft face or freckled face or something, and another complexion or texture on the body.
	public interface ILotionable
	{
		string ButtonText();

		string LocationDesc(out bool isPlural);
		SkinTexture PostUseSkinTexture();

		bool CanLotion();
		bool IsDifferentTexture(SkinTexture lotionTexture);
		bool AttemptToLotion(SkinTexture lotionTexture);

	}

	public interface ILotionableCustomText : ILotionable
	{
		string DisplayResults(SkinTexture lotionTexture, bool successful);
	}


}
