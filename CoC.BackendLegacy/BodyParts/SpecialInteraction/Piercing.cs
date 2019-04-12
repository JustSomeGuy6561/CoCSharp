using CoC.Backend.Save;
using CoC.Backend.Save.Internals;
using CoC.Backend.Tools;
using CoC.Backend.Wearables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CoC.Backend.BodyParts.SpecialInteraction
{

	[DataContract]
	public class Piercing<Locations> : ISaveableBase where Locations : Enum
	{
		public bool piercingFetish => BackendSessionData.data.piercingFetish;

		public int maxPiercingCount => Enum.GetNames(typeof(Locations)).Length;
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
				return jewelryEquipped.ContainsKey(piercingLocation); //should always return true after previous line. but that's bad for debugging.
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
			PiercingJewelry[] retVal = new PiercingJewelry[Enum.GetNames(typeof(Locations)).Length];
			foreach (var data in locations)
			{
				var ind = (int)Convert.ChangeType(data.first, typeof(int));
				retVal[ind] = data.second;
			}
			return retVal;
		}

		Type ISaveableBase.currentSaveType => typeof(PiercingSurrogate<Locations>);

		Type[] ISaveableBase.saveVersionTypes => new Type[] { typeof(PiercingSurrogate<Locations>) };

		object ISaveableBase.ToCurrentSaveVersion()
		{
			return new PiercingSurrogate<Locations>(maxPiercingCount, piercedAt, jewelryEquipped);
		}

		internal Piercing(PiercingSurrogate<Locations> surrogate)
		{
			this.piercedAt.Clear();
			this.jewelryEquipped.Clear();
			if (surrogate.piercings != null && surrogate.piercings.Length > 0)
			{
				int iter = Math.Min(surrogate.piercings.Length, maxPiercingCount);
				for (int x = 0; x < iter; x++)
				{
					if (surrogate.piercings[x])
					{
						Locations piercing = (Locations)Enum.ToObject(typeof(Locations), x);
						piercedAt.Add(piercing, true);
					}
				}
			}
			if (surrogate.jewelry != null && surrogate.jewelry.Length > 0)
			{
				int iter = Math.Min(surrogate.jewelry.Length, maxPiercingCount);
				for (int x = 0; x < iter; x++)
				{
					if (surrogate.jewelry[x]!= null)
					{
						Locations piercing = (Locations)Enum.ToObject(typeof(Locations), x);
						jewelryEquipped.Add(piercing, surrogate.jewelry[x]);
					}
				}
			}
		}

		internal Piercing(Pair<Locations, PiercingJewelry>[] creatorPairs)
		{

		}
	}

	[DataContract]
	[KnownType(typeof(PiercingJewelry))]
	public sealed class PiercingSurrogate<PiercingLocations> : ISurrogateBase
		where PiercingLocations : Enum
	{
		[DataMember]
		public bool[] piercings;
		[DataMember]
		public PiercingJewelry[] jewelry;

		object ISurrogateBase.ToSaveable()
		{
			return new Piercing<PiercingLocations>(this);
		}

		internal PiercingSurrogate(int maxPiercings, Dictionary<PiercingLocations, bool> piercingData, Dictionary<PiercingLocations, PiercingJewelry> jewelryData)
		{
			piercings = new bool[maxPiercings];
			jewelry = new PiercingJewelry[maxPiercings];
			foreach (var val in piercingData)
			{
				if (val.Value)
				{
					var ind = (int)Convert.ChangeType(val.Key, typeof(int));
					piercings[ind] = true;
				}
			}
			foreach (var val in jewelryData)
			{
				if (val.Value != null)
				{
					var ind = (int)Convert.ChangeType(val.Key, typeof(int));
					jewelry[ind] = val.Value;
				}
			}
		}
	}

	public static class PiercingHelper
	{
		public static bool[] CreatePiercingListForCreator<T, U>(params U[] piercingLocations) where T : Piercing<U> where U : Enum
		{
			bool[] retVal = new bool[Enum.GetNames(typeof(U)).Length];
			foreach (U param in piercingLocations)
			{
				var ind = (int)Convert.ChangeType(param, typeof(int));
				retVal[ind] = true;
			}
			return retVal;
		}
	}
}
