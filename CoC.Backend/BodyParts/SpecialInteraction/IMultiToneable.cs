//IMultiToneable.cs
//Description:
//Author: JustSomeGuy
//4/17/2019, 2:39 AM
using CoC.Backend.CoC_Colors;
using System;
using System.Collections.Generic;

namespace CoC.Backend.BodyParts.SpecialInteraction
{
	public interface IMultiToneable
	{
		byte numToneableMembers { get; }

		string buttonText();

		string memberButtonText(byte index);

		string memberLocationDesc(byte index, out bool isPlural);
		string memberPostToneDescription(byte index);

		bool canToneOil(byte index);

		bool isDifferentTone(Tones oilTone, byte index);

		bool attemptToTone(Tones oilTone, byte index);

	}
	public interface IMultiToneableCustomText : IMultiToneable
	{
		string DisplayResults(Tones oilTone, byte index, bool successful);
	}
}
