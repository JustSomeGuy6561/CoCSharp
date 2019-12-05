//Piercing.cs
//Description:
//Author: JustSomeGuy
//3/27/2019, 11:41 AM
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Backend.SaveData;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WeakEvent;

namespace CoC.Backend.BodyParts.SpecialInteraction
{


	public class Piercing<Locations> where Locations : Enum
	{
		//tbh, not the cleanest tool, but idgaf. it works. As far as anyone implementing this shit will know or care, it's basically identical to the standard event.
		private readonly WeakEventSource<PiercingDataChangedEventArgs<Locations>> piercingChangeSource = new WeakEventSource<PiercingDataChangedEventArgs<Locations>>();
		public event EventHandler<PiercingDataChangedEventArgs<Locations>> OnPiercingChange
		{
			add { piercingChangeSource.Subscribe(value); }
			remove { piercingChangeSource.Unsubscribe(value); }
		}


		public bool piercingFetish => BackendSessionSave.data.piercingFetishEnabled;

		public static int maxPiercingCount => EnumHelper.Length<Locations>();

		protected readonly Dictionary<Locations, bool> piercedAt = new Dictionary<Locations, bool>();
		protected readonly Dictionary<Locations, PiercingJewelry> jewelryEquipped = new Dictionary<Locations, PiercingJewelry>();

		private readonly Func<Locations, bool> piercingUnlocked;
		private readonly Func<Locations, JewelryType> jewelryTypesAllowed;

		internal Piercing(Func<Locations, bool> piercingUnlockedFunction, Func<Locations, JewelryType> supportedJewelryTypesFunction)
		{
			jewelryTypesAllowed = supportedJewelryTypesFunction ?? throw new ArgumentNullException(nameof(supportedJewelryTypesFunction));
			piercingUnlocked = piercingUnlockedFunction ?? throw new ArgumentNullException(nameof(piercingUnlockedFunction));
		}

		public ReadOnlyPiercing<Locations> AsReadOnlyData()
		{
			return new ReadOnlyPiercing<Locations>(piercedAt, jewelryEquipped);
		}

		public PiercingJewelry this[Locations location]
		{
			get => jewelryEquipped[location];
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

		internal bool EquipPiercingJewelry(Locations piercingLocation, PiercingJewelry jewelry, bool forceEquip = false)
		{
			if (jewelry == null) throw new ArgumentNullException(nameof(jewelry));
			//if it's an unknown location: fail. occurs when people do arithmatic on an enum.
			//consider having this throw. I'm thinking index out of range, but idk.
			if (!Enum.IsDefined(typeof(Locations), piercingLocation))
			{
				return false;
			}
			//if we can't pierce this location: fail
			else if (!CanPierce(piercingLocation))
			{
				return false;
			}
			//or we can't equip this type of piercing: fail.
			else if (!jewelryTypesAllowed(piercingLocation).HasFlag(jewelry.jewelryType))
			{
				return false;
			}
			//or it isn't pierced and we aren't forcing it: fail.
			else if (!isPiercedAt(piercingLocation) && !forceEquip)
			{
				return false;
			}
			//but if it isn't pierced and we are forcing it, run that function. should return true.
			else if (!isPiercedAt(piercingLocation))
			{
				return Pierce(piercingLocation, jewelry);
			}
			//if we are already wearing jewelry at that location and not force, fail
			else if (WearingJewelryAt(piercingLocation) && !forceEquip)
			{
				return false;
			}
			//if we are already wearing jewelry at that location and force
			else if (!WearingJewelryAt(piercingLocation))
			{
				//swap out the jewelry
				jewelryEquipped.Add(piercingLocation, jewelry);
				ProcChange(piercingLocation, jewelry);
				return jewelryEquipped.ContainsKey(piercingLocation); //should always return true, but i'd like to proc it on unit tests if we somehow broke Dictionaries.
			}
			//final case - we are pierced at this location and don't have any jewelry here and our jewelry type is valid for this location.
			else
			{
				//put in the jewelry.
				jewelryEquipped[piercingLocation] = jewelry;
				ProcChange(piercingLocation, jewelryEquipped[piercingLocation], jewelry);
				return jewelryEquipped[piercingLocation] == jewelry;
			}
		}



		internal PiercingJewelry RemovePiercingJewelry(Locations piercingLocation, bool forceRemove = false)
		{
			if (!jewelryEquipped.ContainsKey(piercingLocation))
			{
				return null;
			}
			else if (!jewelryEquipped[piercingLocation].removable && !forceRemove)
			{
				return null;
			}
			else
			{
				PiercingJewelry jewelry = jewelryEquipped[piercingLocation];
				jewelryEquipped.Remove(piercingLocation);
				ProcChange(piercingLocation, jewelry, null);
				return jewelry;
			}

		}

		internal bool Pierce(Locations piercingLocation, PiercingJewelry jewelry)
		{
			if (jewelry == null) throw new ArgumentNullException(nameof(jewelry));

			if (!CanPierce(piercingLocation))
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
				jewelryEquipped[piercingLocation] = jewelry;
			}
			else
			{
				jewelryEquipped.Add(piercingLocation, jewelry);
			}

			ProcChange(piercingLocation, true, jewelry);

			return piercedAt[piercingLocation];

		}

		internal bool EquipPiercingJewelryAndPierceIfNotPierced(Locations piercingLocation, PiercingJewelry jewelry, bool forceEquip = false)
		{
			if (!isPiercedAt(piercingLocation))
			{
				return Pierce(piercingLocation, jewelry);
			}
			else
			{
				return EquipPiercingJewelry(piercingLocation, jewelry, forceEquip);
			}
		}

		public bool CanPierce(Locations piercingLocation)
		{
			return piercingUnlocked(piercingLocation);
		}

		public bool CanWearThisJewelryType(Locations piercingLocation, JewelryType jewelryType)
		{
			return jewelryTypesAllowed(piercingLocation).HasFlag(jewelryType);
		}

		internal bool Validate(bool correctInvalidData)
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
				if (piercedAt.ContainsKey(entry) && !CanPierce(entry))
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
			ProcChange(piercedAt, jewelryEquipped);
			piercedAt.Clear();
			jewelryEquipped.Clear();
		}

		private void ProcChange(Dictionary<Locations, bool> piercedAt, Dictionary<Locations, PiercingJewelry> jewelryEquipped)
		{
			piercingChangeSource.Raise(this, new PiercingDataChangedEventArgs<Locations>(piercedAt, jewelryEquipped));
		}

		//if theres some magic in the future that lets you un-pierce a location and reject any jewelry inside, this will handle it. but it's mostly for the other way - piercing something with new jewelry.
		private void ProcChange(Locations piercingLocation, bool isNowPierced, PiercingJewelry deltaJewelry)
		{
			if (isNowPierced)
			{
				if (deltaJewelry != null)
				{
					piercingChangeSource.Raise(this, new PiercingDataChangedEventArgs<Locations>(piercingLocation, isNowPierced, null, deltaJewelry, piercingCount, jewelryCount));
				}
				else
				{
					piercingChangeSource.Raise(this, new PiercingDataChangedEventArgs<Locations>(piercingLocation, isNowPierced, piercingCount, jewelryCount));
				}
			}
			else
			{
				if (deltaJewelry != null)
				{
					piercingChangeSource.Raise(this, new PiercingDataChangedEventArgs<Locations>(piercingLocation, isNowPierced, deltaJewelry, null, piercingCount, jewelryCount));
				}
				else
				{
					piercingChangeSource.Raise(this, new PiercingDataChangedEventArgs<Locations>(piercingLocation, isNowPierced, piercingCount, jewelryCount));
				}
			}
		}

		//oldJewelry assumed to be null.
		private void ProcChange(Locations piercingLocation, PiercingJewelry newJewelry)
		{
			piercingChangeSource.Raise(this, new PiercingDataChangedEventArgs<Locations>(piercingLocation, null, newJewelry, piercingCount, jewelryCount));
		}

		private void ProcChange(Locations piercingLocation, PiercingJewelry oldJewelry, PiercingJewelry newJewelry)
		{
			piercingChangeSource.Raise(this, new PiercingDataChangedEventArgs<Locations>(piercingLocation, oldJewelry, newJewelry, piercingCount, jewelryCount));
		}

		internal Piercing(Dictionary<Locations, PiercingJewelry> creatorPairs)
		{

		}

		public static IEnumerable<Locations> AsIteratable()
		{
			return Enum.GetValues(typeof(Locations)).Cast<Locations>();
		}
	}

	public delegate void PiercingDataChangedEventHandler<T>(object sender, PiercingDataChangedEventArgs<T> args) where T : Enum;

	public class PiercingDataChangedEventArgs<T> : EventArgs
	{
		public readonly ReadOnlyDictionary<T, ValueDifference<bool>> piercingDiffs;
		public readonly ReadOnlyDictionary<T, ValueDifference<PiercingJewelry>> jewelryDiffs;

		public readonly int oldPiercingCount;
		public readonly int newPiercingCount;
		public readonly int oldJewelryCount;
		public readonly int newJewelryCount;

		internal PiercingDataChangedEventArgs(T location, bool piercingChange, int newPiercingCount, int newJewelryCount)
		{
			this.newPiercingCount = newPiercingCount;
			oldPiercingCount = piercingChange ? newPiercingCount - 1 : newPiercingCount + 1;
			this.newJewelryCount = newJewelryCount;
			oldJewelryCount = newJewelryCount;

			Dictionary<T, ValueDifference<bool>> pDiff = new Dictionary<T, ValueDifference<bool>>
			{
				{ location, new ValueDifference<bool>(!piercingChange, piercingChange) }
			};
			piercingDiffs = new ReadOnlyDictionary<T, ValueDifference<bool>>(pDiff);
			jewelryDiffs = new ReadOnlyDictionary<T, ValueDifference<PiercingJewelry>>(new Dictionary<T, ValueDifference<PiercingJewelry>>());
		}

		internal PiercingDataChangedEventArgs(T location, PiercingJewelry oldJewelry, PiercingJewelry newJewelry, int newPiercingCount, int newJewelryCount)
		{
			this.newPiercingCount = newPiercingCount;
			oldPiercingCount = newPiercingCount;
			this.newJewelryCount = newJewelryCount;
			if (oldJewelry == null != (newJewelry == null))
			{
				if (oldJewelry == null)
				{
					oldJewelryCount = newJewelryCount - 1;
				}
				else
				{
					oldJewelryCount = newJewelryCount + 1;
				}
			}
			else
			{
				oldJewelryCount = newJewelryCount;
			}

			piercingDiffs = new ReadOnlyDictionary<T, ValueDifference<bool>>(new Dictionary<T, ValueDifference<bool>>());

			Dictionary<T, ValueDifference<PiercingJewelry>> jDiff = new Dictionary<T, ValueDifference<PiercingJewelry>>
			{
				{ location, new ValueDifference<PiercingJewelry>(oldJewelry, newJewelry) }
			};
			jewelryDiffs = new ReadOnlyDictionary<T, ValueDifference<PiercingJewelry>>(jDiff);
		}

		internal PiercingDataChangedEventArgs(T location, bool piercingChange, PiercingJewelry oldJewelry, PiercingJewelry newJewelry, int newPiercingCount, int newJewelryCount)
		{
			this.newPiercingCount = newPiercingCount;
			oldPiercingCount = piercingChange ? newPiercingCount - 1 : newPiercingCount + 1;

			Dictionary<T, ValueDifference<bool>> pDiff = new Dictionary<T, ValueDifference<bool>>
			{
				{ location, new ValueDifference<bool>(!piercingChange, piercingChange) }
			};
			piercingDiffs = new ReadOnlyDictionary<T, ValueDifference<bool>>(pDiff);


			this.newJewelryCount = newJewelryCount;
			if (oldJewelry == null != (newJewelry == null))
			{
				if (oldJewelry == null)
				{
					oldJewelryCount = newJewelryCount - 1;
				}
				else
				{
					oldJewelryCount = newJewelryCount + 1;
				}
			}
			else
			{
				oldJewelryCount = newJewelryCount;
			}

			Dictionary<T, ValueDifference<PiercingJewelry>> jDiff = new Dictionary<T, ValueDifference<PiercingJewelry>>
			{
				{ location, new ValueDifference<PiercingJewelry>(oldJewelry, newJewelry) }
			};
			jewelryDiffs = new ReadOnlyDictionary<T, ValueDifference<PiercingJewelry>>(jDiff);
		}

		internal PiercingDataChangedEventArgs(Dictionary<T, bool> unclearedPiercingDict, Dictionary<T, PiercingJewelry> unclearedJewelryDict)
		{
			Dictionary<T, ValueDifference<bool>> pDiffs = unclearedPiercingDict.Where(x => x.Value == true).ToDictionary(x => x.Key, x => new ValueDifference<bool>(x.Value, false));
			piercingDiffs = new ReadOnlyDictionary<T, ValueDifference<bool>>(pDiffs);
			oldPiercingCount = unclearedPiercingDict.Count;
			newPiercingCount = 0;

			Dictionary<T, ValueDifference<PiercingJewelry>> jDiffs = unclearedJewelryDict.Where(x => x.Value != null).ToDictionary(x => x.Key, x => new ValueDifference<PiercingJewelry>(x.Value, null));
			jewelryDiffs = new ReadOnlyDictionary<T, ValueDifference<PiercingJewelry>>(jDiffs);
			oldJewelryCount = unclearedJewelryDict.Count;
			newJewelryCount = 0;
		}
	}

	public sealed class ReadOnlyPiercing<Location> where Location : Enum
	{
		private readonly Dictionary<Location, bool> piercedAt = new Dictionary<Location, bool>();
		private readonly Dictionary<Location, PiercingJewelry> jewelryEquipped = new Dictionary<Location, PiercingJewelry>();

		public readonly int piercingCount;
		public bool isPierced => piercingCount != 0;

		public bool isPiercedAt(Location location)
		{
			if (location == null)
			{
				return false;
			}
			piercedAt.TryGetValue(location, out bool isPierced);
			return isPierced;
		}

		public readonly int jewelryCount;
		public bool wearingJewelry => jewelryCount > 0;
		public bool WearingJewelryAt(Location location)
		{
			if (location == null)
			{
				return false;
			}
			piercedAt.TryGetValue(location, out bool jewelryAt);
			return jewelryAt;
		}

		public ReadOnlyPiercing(Dictionary<Location, bool> piercedAt, Dictionary<Location, PiercingJewelry> jewelryEquipped)
		{
			this.piercedAt = new Dictionary<Location, bool>(piercedAt);
			this.jewelryEquipped = new Dictionary<Location, PiercingJewelry>(jewelryEquipped);

			piercingCount = piercedAt.Values.Aggregate(0, (x, y) => { if (y) x++; return x; });

			jewelryCount = jewelryEquipped.Values.Aggregate(0, (x, y) => { if (y != null) x++; return x; });
		}

		public ReadOnlyPiercing()
		{
			piercedAt = new Dictionary<Location, bool>();
			jewelryEquipped = new Dictionary<Location, PiercingJewelry>();

			piercingCount = 0;
			jewelryCount = 0;
		}
	}
}
