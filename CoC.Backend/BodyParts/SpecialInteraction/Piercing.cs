using CoC.Backend.Items.Wearables.Piercings;
using CoC.Backend.SaveData;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoC.Backend.BodyParts.SpecialInteraction
{


	public class Piercing<Locations> where Locations : Enum
	{
		public bool piercingFetish => BackendSessionData.data.piercingFetish;

		public int maxPiercingCount => EnumHelper.Length<Locations>();
		protected readonly Dictionary<Locations, bool> piercedAt = new Dictionary<Locations, bool>();
		protected readonly Dictionary<Locations, PiercingJewelry> jewelryEquipped = new Dictionary<Locations, PiercingJewelry>();

		private readonly Func<Locations, bool> piercingUnlocked;
		public readonly JewelryType jewelryTypesAllowed;

		internal Piercing(JewelryType supportedJewelryTypes, Func<Locations, bool> piercingUnlockedFunction)
		{
			jewelryTypesAllowed = supportedJewelryTypes;
			piercingUnlocked = piercingUnlockedFunction;
		}

		public int piercingCount => piercedAt.Values.Aggregate(0, (x, y) => { if (y) x++; return x; });
		public bool isPierced => piercedAt.Values.Any((x) => x);
		public bool isPiercedAt(Locations location)
		{
			if (location == null)
			{
				return false;
			}
			piercedAt.TryGetValue(location, out bool isPierced);
			return isPierced;
		}

		public int jewelryCount => jewelryEquipped.Values.Aggregate(0, (x, y) => { if (y != null) x++; return x; });
		public bool wearingJewelry => jewelryEquipped.Values.Any((x) => x != null);
		public bool WearingJewelryAt(Locations location)
		{
			if (location == null)
			{
				return false;
			}
			piercedAt.TryGetValue(location, out bool jewelryAt);
			return jewelryAt;
		}

		public bool EquipPiercingJewelry(Locations piercingLocation, PiercingJewelry jewelry, bool forceIfEnabled = false)
		{
			if (!canPierce(piercingLocation))
			{
				return false;
			}
			if (!isPiercedAt(piercingLocation) && !forceIfEnabled)
			{
				return false;
			}
			else if (WearingJewelryAt(piercingLocation) && !forceIfEnabled)
			{
				return false;
			}
			else if (!isPiercedAt(piercingLocation))
			{
				return Pierce(piercingLocation, jewelry);
			}
			else if (!WearingJewelryAt(piercingLocation))
			{
				jewelryEquipped.Add(piercingLocation, jewelry);
				return jewelryEquipped.ContainsKey(piercingLocation); //should always return true after previous line. but that's bad for debugging; i'd like to know if we broke dictionaries, lol.
			}
			else
			{
				jewelryEquipped[piercingLocation] = jewelry;
				return jewelryEquipped[piercingLocation] == jewelry;
			}
		}

		public PiercingJewelry RemovePiercingJewelry(Locations piercingLocation, bool forceRemove = false)
		{
			if (!jewelryEquipped.ContainsKey(piercingLocation))
			{
				return null;
			}
			else if (jewelryEquipped[piercingLocation].isSeamless && !forceRemove)
			{
				return null;
			}
			else
			{
				PiercingJewelry jewelry = jewelryEquipped[piercingLocation];
				jewelryEquipped.Remove(piercingLocation);
				return jewelry;
			}

		}

		public bool Pierce(Locations piercingLocation, PiercingJewelry jewelry)
		{
			PiercingJewelry jewel = (PiercingJewelry)jewelry;
			if (!canPierce(piercingLocation))
			{
				return false;
			}
			if (isPiercedAt(piercingLocation))
			{
				return false;
			}

			if (piercedAt.ContainsKey(piercingLocation))
			{
				piercedAt[piercingLocation] = true;
			}
			else
			{
				piercedAt.Add(piercingLocation, true);
			}
			if (jewelryEquipped.ContainsKey(piercingLocation))
			{
				jewelryEquipped[piercingLocation] = jewel;
			}
			else
			{
				jewelryEquipped.Add(piercingLocation, jewel);
			}
			return piercedAt[piercingLocation];

		}

		public bool EquipPiercingJewelryAndPierceIfNotPierced(Locations piercingLocation, PiercingJewelry jewelry, bool forceIfEnabled = false)
		{
			if (!isPiercedAt(piercingLocation))
			{
				return Pierce(piercingLocation, jewelry);
			}
			else
			{
				return EquipPiercingJewelry(piercingLocation, jewelry, forceIfEnabled);
			}
		}

		public bool canPierce(Locations piercingLocation)
		{
			return piercingUnlocked(piercingLocation);
		}

		public static PiercingJewelry[] CreatePiercingDataForCreator(params Pair<Locations, PiercingJewelry>[] locations)
		{
			PiercingJewelry[] retVal = new PiercingJewelry[EnumHelper.Length<Locations>()];
			foreach (var data in locations)
			{
				var ind = (int)Convert.ChangeType(data.first, typeof(int));
				retVal[ind] = data.second;
			}
			return retVal;
		}


		internal bool Validate(bool correctInvalidData = false)
		{
			bool valid = true;
			foreach (Locations entry in Enum.GetValues(typeof(Locations)).Cast<Locations>())
			{

				//if not pierced at current location and there is jewelry there.
				if ((!piercedAt.TryGetValue(entry, out bool x) || x == false) && jewelryEquipped.TryGetValue(entry, out PiercingJewelry y) && y != null)
				{
					if (correctInvalidData)
					{
						jewelryEquipped.Remove(entry);
					}
					valid = false;
				}
				//if you can't pierce there.
				if (piercedAt.ContainsKey(entry) && !canPierce(entry))
				{
					if (correctInvalidData)
					{
						piercedAt.Remove(entry);
						jewelryEquipped.Remove(entry);
					}
					valid = false;
				}
			}
			return valid;
		}

		internal void Reset()
		{
			piercedAt.Clear();
			jewelryEquipped.Clear();
		}

		internal Piercing(Pair<Locations, PiercingJewelry>[] creatorPairs)
		{

		}
	}
}
