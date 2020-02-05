using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CoC.Backend.Perks;
using CoC.Frontend.Perks.SpeciesPerks;

namespace CoC.Frontend.Perks
{
	internal static class PerkManager
	{
		internal static readonly ReadOnlyDictionary<Type, Func<ConditionalPerk>> conditionalPerks;
		private static readonly Dictionary<Type, Func<ConditionalPerk>> conditionalPerksSource;

		internal static readonly ReadOnlyDictionary<Type, Func<StandardPerk>> obtainablePerks;
		private static readonly Dictionary<Type, Func<StandardPerk>> obtainablePerksSource = new Dictionary<Type, Func<StandardPerk>>();

		static PerkManager()
		{
			obtainablePerks = new ReadOnlyDictionary<Type, Func<StandardPerk>>(obtainablePerksSource);
			conditionalPerks = new ReadOnlyDictionary<Type, Func<ConditionalPerk>>(conditionalPerksSource);
			//add your conditional and obtainable perks here;
			//obtainable perks can be added easily with the helper functions provided.

			AddCondtionalPerk<Diapause>();
			AddCondtionalPerk<Oviposition>();
			AddCondtionalPerk<BasiliskWomb>();
		}


		private static void AddObtainablePerk<T>(Func<T> creator) where T : StandardPerk, IAttainablePerk<T>
		{
			if (!obtainablePerksSource.ContainsKey(typeof(T)))
			{
				obtainablePerksSource.Add(typeof(T), creator);
			}
		}

		private static void AddObtainablePerk<T>() where T : StandardPerk, IAttainablePerk<T>, new()
		{
			if (!obtainablePerksSource.ContainsKey(typeof(T)))
			{
				obtainablePerksSource.Add(typeof(T), () => new T());
			}
		}


		private static void AddCondtionalPerk<T>(Func<T> creator) where T : ConditionalPerk
		{
			if (!conditionalPerksSource.ContainsKey(typeof(T)))
			{
				conditionalPerksSource.Add(typeof(T), creator);
			}
		}

		private static void AddCondtionalPerk<T>() where T : ConditionalPerk, new()
		{
			if (!conditionalPerksSource.ContainsKey(typeof(T)))
			{
				conditionalPerksSource.Add(typeof(T), () => new T());
			}
		}
	}
}
