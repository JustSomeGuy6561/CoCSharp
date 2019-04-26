using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts.SpecialInteraction
{
	//a new interface, so that you an get a soft face or freckled face or something, and another complexion or texture on the body.
	public interface ILotionable
	{
		bool canLotion();
		bool isDifferentTexture(SkinTexture lotionTexture);
		bool attemptToLotion(SkinTexture lotionTexture);

		string buttonText();
		string locationDesc();
	}

	public interface ILotionableCustomText : ILotionable
	{
		string ApplyLotion(SkinTexture lotionTexture);
	}
}
