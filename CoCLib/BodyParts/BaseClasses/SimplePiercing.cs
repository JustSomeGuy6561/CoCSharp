//EyeBrow.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 5:41 PM
using  CoC.BodyParts.SpecialInteraction;
using CoC.Wearables.Piercings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace  CoC.BodyParts
{
	internal abstract class SimplePiercing<PiercingEnum> : IPiercable<PiercingEnum> where PiercingEnum : System.Enum
	{
		protected readonly PiercingFlags piercingFlags;

		protected SimplePiercing(PiercingFlags flags)
		{
			piercingFlags = flags;
		}

		public int maxPiercingCount => Enum.GetNames(typeof(PiercingEnum)).Length;
		//functional programming ftw!
		//counts the number of trues, and returns it. uses a "fold" function. fold iterates over a list, doing an action for each element, then returning the result
		public int currentPiercingCount => piercingLookup.Values.Aggregate(0, (x, y) => { if (y) x++; return x; });
		public int currentJewelryCount => jewelryLookup.Values.Count;

		public bool EquipPiercingJewelry(PiercingEnum piercingLocation, PiercingJewelry jewelry, bool forceIfEnabled = false)
		{
			if (!canPierce(piercingLocation))
			{
				return false;
			}
			if (!IsPierced(piercingLocation) && !forceIfEnabled)
			{
				return false;
			}
			else if (jewelryLookup.ContainsKey(piercingLocation) && !forceIfEnabled)
			{
				return false;
			}
			else if (!IsPierced(piercingLocation))
			{
				return Pierce(piercingLocation, jewelry);
			}
			else if (!jewelryLookup.ContainsKey(piercingLocation))
			{
				jewelryLookup.Add(piercingLocation, jewelry);
				return jewelryLookup.ContainsKey(piercingLocation); //should always return true after previous line. but that's bad for debugging.
			}
			else
			{
				jewelryLookup[piercingLocation] = jewelry;
				return jewelryLookup[piercingLocation] == jewelry;
			}
		}

		/// <summary>
		/// Attempts to remove a piercing at piercing location, and return the jewelry removed.
		/// will fail to remove seamless jewelry unless forceRemove is set to true.
		/// </summary>
		/// <param name="piercingLocation">location of the piercing to remove.</param>
		/// <param name="forceRemove"></param>
		/// <returns>The jewelry removed, or null if no jewelry was removed.</returns>
		public PiercingJewelry RemovePiercingJewelry(PiercingEnum piercingLocation, bool forceRemove = false)
		{
			if (!jewelryLookup.ContainsKey(piercingLocation))
			{
				return null;
			}
			else if (jewelryLookup[piercingLocation].isSeamless && !forceRemove)
			{
				return null;
			}
			else
			{
				PiercingJewelry jewelry = jewelryLookup[piercingLocation];
				jewelryLookup.Remove(piercingLocation);
				return jewelry;
			}

		}

		public bool HasJewelry(PiercingEnum piercingLocation)
		{
			return jewelryLookup.ContainsKey(piercingLocation);
		}
		public bool IsPierced(PiercingEnum piercingLocation)
		{
			return piercingLookup.ContainsKey(piercingLocation) && piercingLookup[piercingLocation];
		}
		public bool Pierce(PiercingEnum piercingLocation, PiercingJewelry jewelry)
		{
			if (!canPierce(piercingLocation))
			{
				return false;
			}
			if (IsPierced(piercingLocation))
			{
				return false;
			}
			else if (piercingLookup.ContainsKey(piercingLocation))
			{
				piercingLookup[piercingLocation] = true;
				return piercingLookup[piercingLocation];
			}
			else
			{
				piercingLookup.Add(piercingLocation, true);
				return piercingLookup.ContainsKey(piercingLocation) && piercingLookup[piercingLocation];
			}
		}

		public bool EquipPiercingJewelryAndPierceIfNotPierced(PiercingEnum piercingLocation, PiercingJewelry jewelry, bool forceIfEnabled = false)
		{
			if (!IsPierced(piercingLocation))
			{
				return Pierce(piercingLocation, jewelry);
			}
			else
			{
				return EquipPiercingJewelry(piercingLocation, jewelry, forceIfEnabled);
			}
		}

		protected abstract bool PiercingLocationUnlocked(PiercingEnum piercingLocation);

		public bool canPierce(PiercingEnum piercingLocation)
		{
			if (!piercingFlags.enabled)
			{
				return false;
			}
			return PiercingLocationUnlocked(piercingLocation);
		}

		protected Dictionary<PiercingEnum, bool> piercingLookup;
		protected Dictionary<PiercingEnum, PiercingJewelry> jewelryLookup;
	}
}
