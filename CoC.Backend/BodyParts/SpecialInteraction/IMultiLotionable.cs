//IMultiLotionable.cs
//Description:
//Author: JustSomeGuy
//4/24/2019, 12:39 AM
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts.SpecialInteraction
{
	//a new interface, so that you an get a soft face or freckled face or something, and another complexion or texture on the body.
	public interface IMultiLotionable
	{
		byte numLotionableMembers { get; }

		string buttonText();

		string memberButtonText(byte index);

		string memberLocationDesc(byte index, out bool isPlural);
		SkinTexture postUseSkinTexture(byte index);

		bool canLotion(byte index);

		bool isDifferentTexture(SkinTexture lotionTexture, byte index);

		bool attemptToLotion(SkinTexture lotionTexture, byte index);
	}

	//allows you to override the text displayed during lotioning
	public interface IMultiLotionableCustomText : IMultiLotionable
	{
		string DisplayResults(SkinTexture lotionTexture, byte index, bool successful);
	}
}

