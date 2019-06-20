using CoC.Backend.BodyParts;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CoC.Backend.Creatures
{
	public sealed class CockCreator
	{
		public readonly CockType type;
		public float? length;
		public float? girth;
		public float? knot;
		public readonly ReadOnlyDictionary<CockPiercings, PiercingJewelry> piercings;

		public float validLength => length ?? Cock.DEFAULT_COCK_LENGTH;
		public float validGirth => girth ?? Cock.DEFAULT_COCK_GIRTH;
		public float validKnot
		{
			get
			{
				if (!type.hasKnot)
				{
					return 0f;
				}
				else if (knot == null)
				{
					return type.baseKnotMultiplier;
				}
				else
				{
					return Utils.Clamp2((float)knot, Cock.MIN_KNOT_MULTIPLIER, Cock.MAX_KNOT_MULTIPLIER);
				}
			}
		}
		public CockCreator(float? cockLength = null, float? cockGirth = null, float? knotMultiplier = null, Dictionary<CockPiercings, PiercingJewelry> cockJewelry = null)
			: this(CockType.HUMAN, cockLength, cockGirth, knotMultiplier, cockJewelry) { }
		public CockCreator(CockType cockType, float? cockLength = null, float? cockGirth = null, float? knotMultiplier = null, Dictionary<CockPiercings, PiercingJewelry> cockJewelry = null)
		{
			type = cockType;
			length = cockLength;
			girth = cockGirth;
			knot = knotMultiplier;
			piercings = cockJewelry == null ? null : new ReadOnlyDictionary<CockPiercings, PiercingJewelry>(cockJewelry);
		}
	}
}
