//PerkLib.cs
//Description:
//Author: JustSomeGuy
//2/1/2019, 6:01 PM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace CoC.Perks
{
	internal class PerkLib
	{
		private static readonly List<Type> knownTypes = new List<Type>();

		static PerkLib()
		{
			knownTypes.Add(typeof(PerkBase));
			knownTypes.Add(typeof(PerkBase[]));
			//add perks here.
		}

		//ideally, you could use data oriented design and just allocate an array with these in it and check for null. 
		//but this will suffice because perks are fluid and could change;
		private Dictionary<Type, PerkBase> perkLib = new Dictionary<Type, PerkBase>();

		[DataMember]
		private PerkBase[] perks;

		[OnSerializing]
		private void serializePerks(StreamingContext context)
		{
			perks = perkLib.Values.ToArray();
		}

		[OnSerialized]
		private void perksSerialized(StreamingContext context)
		{
			perks = null;
		}

		[OnDeserialized]
		private void deserializePerks(StreamingContext context)
		{
			perkLib = perks.ToDictionary(x => x.GetType(), x => x);
			perks = null;
		}


		public Type[] GetKnownTypesNeeded()
		{
			return knownTypes.ToArray();
		}
	}


}
