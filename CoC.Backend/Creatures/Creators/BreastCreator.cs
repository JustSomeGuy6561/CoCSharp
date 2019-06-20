using CoC.Backend.BodyParts;
using CoC.Backend.Items.Wearables.Piercings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CoC.Backend.Creatures
{
	public sealed class BreastCreator
	{
		public CupSize cupSize;
		public float? nippleLength;
		public readonly ReadOnlyDictionary<NipplePiercings, PiercingJewelry> nipplePiercings;

		public float validNippleLength => nippleLength ?? (cupSize < CupSize.B ? 0.25f : 0.5f);

		public BreastCreator(CupSize cup, float? nippleLengthInInches = null, Dictionary<NipplePiercings, PiercingJewelry> nippleJewelry = null)
		{
			cupSize = cup;
			nippleLength = nippleLengthInInches;
			nipplePiercings = nippleJewelry == null ? null : new ReadOnlyDictionary<NipplePiercings, PiercingJewelry>(nippleJewelry);
		}
	}
}
