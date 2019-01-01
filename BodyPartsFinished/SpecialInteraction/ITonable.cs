﻿//ITonable.cs
//Description: Interface marking body parts that react to changes in skin tone or use of body lotion.
//Author: JustSomeGuy
//12/30/2018, 10:11 PM
using CoC.Items;
namespace CoC.BodyParts.SpecialInteraction
{
	interface IToneable
	{
		bool canTone();

		bool attemptToTone(Tones tone);
	}
}
