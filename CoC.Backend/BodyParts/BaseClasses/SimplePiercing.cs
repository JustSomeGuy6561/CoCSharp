//SimplePiercing.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 5:41 PM
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Save;
using System;
using System.Collections.Generic;
using System.Linq;
namespace CoC.Backend.BodyParts
{
	public abstract class SimplePiercing<ThisClass, PiercingEnum> : IPiercable<PiercingEnum>, ISaveableBase where PiercingEnum : Enum
		where ThisClass : SimplePiercing<ThisClass, PiercingEnum>
	{
		protected bool piercingFetish => BackendSessionData.data.piercingFetish;
		protected Dictionary<PiercingEnum, bool> piercingLookup;
		protected Dictionary<PiercingEnum, PiercingJewelry> jewelryLookup;

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
			return PiercingLocationUnlocked(piercingLocation);
		}

		#region Serialization
		Type ISaveableBase.currentSaveType => throw new NotImplementedException();

		Type[] ISaveableBase.saveVersionTypes => throw new NotImplementedException();
		object ISaveableBase.ToCurrentSaveVersion()
		{
			throw new NotImplementedException();
		}

		internal abstract Type[] currentSaves { get; }
		internal abstract Type currentSaveVersion { get; }

		internal abstract SimplePiercingSurrogate<ThisClass, PiercingEnum> ToCurrentSave();

		#endregion
	}
	public abstract class SimplePiercingSurrogate<SaveClass, PiercingEnum> : ISurrogateBase where SaveClass : SimplePiercing<SaveClass, PiercingEnum> where PiercingEnum : Enum
	{
		private protected SimplePiercingSurrogate() { }

		internal abstract SaveClass ToSimplePiercingPart();
		object ISurrogateBase.ToSaveable()
		{
			return ToSimplePiercingPart();
		}
	}
}
