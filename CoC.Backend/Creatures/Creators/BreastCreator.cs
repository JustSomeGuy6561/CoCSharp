//BreastCreator.cs
//Description:
//Author: JustSomeGuy
//6/13/2019, 9:23 PM
using CoC.Backend.BodyParts;
using CoC.Backend.Items.Wearables.Piercings;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CoC.Backend.Creatures
{
	public sealed class BreastCreator
	{
		public CupSize cupSize;

		public readonly ReadOnlyDictionary<NipplePiercingLocation, PiercingJewelry> nipplePiercings;

		public BreastCreator(CupSize cup, float? nippleLengthInInches = null, Dictionary<NipplePiercingLocation, PiercingJewelry> nippleJewelry = null)
		{
			cupSize = cup;
			nipplePiercings = nippleJewelry == null ? null : new ReadOnlyDictionary<NipplePiercingLocation, PiercingJewelry>(nippleJewelry);
		}
	}
}
