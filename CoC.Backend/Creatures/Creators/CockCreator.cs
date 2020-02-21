//CockCreator.cs
//Description:
//Author: JustSomeGuy
//6/13/2019, 8:39 PM
using CoC.Backend.BodyParts;
using CoC.Backend.Items.Wearables.Accessories.CockSocks;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Backend.Tools;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CoC.Backend.Creatures
{
	public sealed class CockCreator
	{
		public readonly CockType type;
		public double? length;
		public double? girth;
		public double? knot;
		public readonly ReadOnlyDictionary<CockPiercingLocation, PiercingJewelry> piercings;
		public readonly CockSockBase cockSock;

		public double validLength => length ?? Cock.DEFAULT_COCK_LENGTH;
		public double validGirth => girth ?? Cock.DEFAULT_COCK_GIRTH;
		public double validKnot
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
					return Utils.Clamp2((double)knot, Cock.MIN_KNOT_MULTIPLIER, Cock.MAX_KNOT_MULTIPLIER);
				}
			}
		}
		public CockCreator(double? cockLength = null, double? cockGirth = null, double? knotMultiplier = null, CockSockBase cockSock = null,
			Dictionary<CockPiercingLocation, PiercingJewelry> cockJewelry = null) : this(CockType.HUMAN, cockLength, cockGirth, knotMultiplier, cockSock, cockJewelry) { }
		public CockCreator(CockType cockType, double? cockLength = null, double? cockGirth = null, double? knotMultiplier = null, CockSockBase cockSock = null,
			Dictionary<CockPiercingLocation, PiercingJewelry> cockJewelry = null)
		{
			type = cockType;
			length = cockLength;
			girth = cockGirth;
			knot = knotMultiplier;
			this.cockSock = cockSock;
			piercings = cockJewelry == null ? null : new ReadOnlyDictionary<CockPiercingLocation, PiercingJewelry>(cockJewelry);
		}
	}
}
