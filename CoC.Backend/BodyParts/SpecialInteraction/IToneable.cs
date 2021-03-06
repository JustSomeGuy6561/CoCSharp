﻿//ITonable.cs
//Description: Interface marking body parts that react to changes in skin tone or use of body lotion.
//Author: JustSomeGuy
//12/30/2018, 10:11 PM

//As of current implementation, This is unused. all parts use multitoneable.
//but it could be used, so here it is.
using CoC.Backend.CoC_Colors;

namespace  CoC.Backend.BodyParts.SpecialInteraction
{
	public interface IToneable
	{
		string buttonText();

		string locationDesc(out bool isPlural);
		string postToneDescription();


		bool canToneLotion();

		bool isDifferentTone(Tones oilTone);

		bool attemptToTone(Tones oilTone);
	}

	public interface IToneableCustomText : IToneable
	{
		string DisplayResults(Tones oilTone, bool successful);
	}
}
