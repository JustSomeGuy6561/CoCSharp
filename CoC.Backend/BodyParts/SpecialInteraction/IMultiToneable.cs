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

		string ButtonText();
		string LocationDesc();

		string MemberButtonText(byte index);

		string MemberLocationDesc(byte index, out bool isPlural);
		string MemberPostToneDescription(byte index);

		bool CanToneOil(byte index);

		bool IsDifferentTone(Tones oilTone, byte index);

		bool AttemptToTone(Tones oilTone, byte index);
	}
	public interface IMultiToneableCustomText : IMultiToneable
	{
		string DisplayResults(Tones oilTone, byte index, bool successful);
	}
}
