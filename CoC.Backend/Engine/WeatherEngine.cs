//WeatherEngine.cs
//Description:
//Author: JustSomeGuy
//Note: date follows MMDDYYYY format.
//10/21/2019 11:34:49 PM

using CoC.Backend.Creatures;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Tools;
using System;

namespace CoC.Backend.Engine
{
//weather is a new game plus thing, i guess. still, might as well integrate it in. I Really have no idea how balls-to-the-wall we wanna make this, so for now we just have
//simple weather patterns - no seasons, so no snow. probably could see about integrating that, idk. i just need it for the camp RN, so i'm putting the barebones in.
	public enum Weather { CLEAR, PARTLY_CLOUDY, CLOUDY, RAINY, THUNDERSTROMS }

	public static class WeatherEngine
	{
		public static Weather CurrentConditions { get; private set; } = Weather.CLEAR;
	}
}
