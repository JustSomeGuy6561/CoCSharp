//PiercableBodyPart.cs
//Description:
//Author: JustSomeGuy
//1/1/2019, 9:09 AM

using CoC.BodyParts.SpecialInteraction;
using CoC.Items;
using CoC.Wearables.Piercings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoC.BodyParts
{
	public abstract class PiercableBodyPart<T, U, V> : BodyPartBase<T, U>, IPiercable<V> where T : PiercableBodyPart<T, U, V> where U : PiercableBodyPartBehavior<U, T, V> where V : System.Enum
	{
		protected abstract PiercingFlags piercingFlags { get; set; }

		public int maxPiercingCount => Enum.GetNames(typeof(V)).Length;
		//functional programming ftw!
		//counts the number of trues, and returns it. uses a "fold" function. fold iterates over a list, doing an action for each element, then returning the result
		public int currentPiercingCount => piercingLookup.Values.Aggregate(0, (x, y) => { if (y) x++; return x; });
		public int currentJewelryCount => jewelryLookup.Values.Count;

		public bool EquipPiercingJewelry(V piercingLocation, PiercingJewelry jewelry, bool forceIfEnabled = false)
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
		public PiercingJewelry RemovePiercingJewelry(V piercingLocation, bool forceRemove = false)
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

		public bool HasJewelry(V piercingLocation)
		{
			return jewelryLookup.ContainsKey(piercingLocation);
		}
		public bool IsPierced(V piercingLocation)
		{
			return piercingLookup.ContainsKey(piercingLocation) && piercingLookup[piercingLocation];
		}
		public bool Pierce(V piercingLocation, PiercingJewelry jewelry)
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

		public bool EquipPiercingJewelryAndPierceIfNotPierced(V piercingLocation, PiercingJewelry jewelry, bool forceIfEnabled = false)
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

		public abstract bool canPierceAtLocation(V piercingLocation);

		public bool canPierce(V piercingLocation)
		{
			if (!piercingFlags.enabled)
			{
				return false;
			}
			return canPierceAtLocation(piercingLocation);
		}

		protected Dictionary<V, bool> piercingLookup;
		protected Dictionary<V, PiercingJewelry> jewelryLookup;

	}
}