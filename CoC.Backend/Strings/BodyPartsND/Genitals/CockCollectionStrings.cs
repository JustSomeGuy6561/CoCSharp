using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using CoC.Backend.Engine;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;

namespace CoC.Backend.BodyParts
{
	internal interface ICockCollection<T> where T:ICock
	{
		ReadOnlyCollection<T> cocks { get; }
		bool hasSheath { get; }

		CockData AverageCock();

	}

	internal static class CockCollectionStrings
	{
		private static int NumCocks<T>(this ICockCollection<T> collection) where T:ICock
		{
			return collection.cocks.Count;
		}


		private static readonly string[] lottaDicksOptions = new string[] { "bundle of ", "obscene group of ", "cluster of ", "wriggling bunch of " };

		private static readonly string[] mismatchedDicksTextOptions = new string[] { "mutated cocks", "mutated dicks", "mixed cocks", "mismatched dicks" };

		#region Cock Text
		internal static string SheathOrBaseStr<T>(ICockCollection<T> collection) where T:ICock
		{
			return collection.hasSheath ? "sheath" : "base";
		}

		internal static string AllCocksShortDescription<T>(ICockCollection<T> collection) where T:ICock
		{
			if (collection.cocks.Count == 0)
			{
				return "";
			}
			else if (collection.cocks.Count == 1)
			{
				return collection.cocks[0].ShortDescription();
			}
			bool mismatched = collection.cocks.Any(x => x.type != collection.cocks[0].type);

			return mismatched ? CockType.GenericCockNoun(true) : collection.cocks[0].ShortDescription(false, true);
		}

		internal static string AllCocksLongDescription<T>(ICockCollection<T> collection) where T:ICock
		{
			return AllCocksDesc(collection, false);
		}

		internal static string AllCocksFullDescription<T>(ICockCollection<T> collection) where T:ICock
		{
			return AllCocksDesc(collection, true);
		}

		internal static string OneCockOrCocksNoun<T>(ICockCollection<T> collection, string pronoun = "your") where T:ICock
		{
			if (collection.cocks.Count == 0)
			{
				return "";
			}

			return CommonBodyPartStrings.OneOfDescription(collection.cocks.Count > 1, pronoun, CockType.GenericCockNoun(collection.cocks.Count > 1));
		}

		internal static string OneCockOrCocksShort<T>(ICockCollection<T> collection, string pronoun = "your") where T:ICock
		{
			if (collection.cocks.Count == 0)
			{
				return "";
			}

			return CommonBodyPartStrings.OneOfDescription(collection.cocks.Count > 1, pronoun, AllCocksShortDescription(collection));
		}

		internal static string EachCockOrCocksNoun<T>(ICockCollection<T> collection, string pronoun = "your") where T:ICock
		{
			return EachCockOrCocksNoun(collection, pronoun, out bool _);
		}

		internal static string EachCockOrCocksShort<T>(ICockCollection<T> collection, string pronoun = "your") where T:ICock
		{
			return EachCockOrCocksShort(collection, pronoun, out bool _);
		}

		internal static string EachCockOrCocksNoun<T>(ICockCollection<T> collection, string pronoun, out bool isPlural) where T:ICock
		{
			isPlural = collection.cocks.Count != 1;
			if (collection.cocks.Count == 0)
			{
				return "";
			}

			return CommonBodyPartStrings.EachOfDescription(collection.cocks.Count > 1, pronoun, CockType.GenericCockNoun(collection.cocks.Count > 1));
		}

		internal static string EachCockOrCocksShort<T>(ICockCollection<T> collection, string pronoun, out bool isPlural) where T:ICock
		{
			isPlural = collection.cocks.Count != 1;
			if (collection.cocks.Count == 0)
			{
				return "";
			}

			return CommonBodyPartStrings.EachOfDescription(collection.cocks.Count > 1, pronoun, AllCocksShortDescription(collection));
		}
		#endregion


		private static string AllCocksDesc<T>(ICockCollection<T> collection, bool full) where T:ICock
		{
			if (collection.cocks.Count == 0)
			{
				return "";
			}
			//If one, return normal cock descript
			else if (collection.cocks.Count == 1)
			{
				return collection.cocks[0].ShortDescription();
			}
			else
			{
				bool mismatched = collection.cocks.Any(x => x.type != collection.cocks[0].type);

				string[] countOptions;
				string description;

				if (collection.cocks.Count == 2)
				{
					countOptions = mismatched ? CommonGenitalStrings.mismatchedPairOptions : CommonGenitalStrings.matchedPairOptions;
				}
				else if (collection.cocks.Count == 3)
				{
					countOptions = mismatched ? CommonGenitalStrings.mismatchedTripleOptions : CommonGenitalStrings.mismatchedTripleOptions;
				}
				else
				{
					countOptions = lottaDicksOptions;
				}

				description = mismatched ? Utils.RandomChoice(mismatchedDicksTextOptions) : collection.cocks[0].ShortDescription(false, true);

				return Utils.RandomChoice(countOptions) + collection.AverageCock().AdjectiveText(full) + description;
			}
		}
	}

	partial class CockCollection : ICockCollection<Cock>
	{
		ReadOnlyCollection<Cock> ICockCollection<Cock>.cocks => cocks;

		private string AllCocksPlayerDesc(PlayerBase player)
		{
			if (_cocks.Count == 0)
			{
				return "";
			}

			StringBuilder sb = new StringBuilder();

			int rando = Utils.Rand(100);

			for (int x = 0; x < _cocks.Count; x++)
			{
				Cock cock = _cocks[x];
				bool plural = false;

				if (player.cocks.Count == 1)
				{
					sb.Append("Your ");
				}
				else if (cock.cockIndex == 0)
				{
					sb.Append("--Your first ");
				}
				else if (rando % 5 == 0)
				{
					sb.Append("--The next ");

				}
				else if (rando % 5 == 1)
				{
					sb.Append("--The " + Utils.NumberAsPlace(x + 1) + " of your ");
					plural = true;
				}
				else if (rando % 5 == 2)
				{
					sb.Append("--One of your ");
					plural = true;
				}
				else if (rando % 5 == 3)
				{
					sb.Append("--The " + Utils.NumberAsPlace(x + 1) + " ");
				}
				else /*if (rando % 5 == 4)*/
				{
					sb.Append("--Another of your ");
					plural = true;
				}

				sb.Append(cock.AdjectiveText(false) + " " + Cock.GenericCockNoun(plural) + " is " + Measurement.ToNearestLargeAndSmallUnit(cock.length, false) + " long and " +
					Measurement.ToNearestSmallUnit(cock.girth, false, false));
				if (rando % 3 == 0)
				{
					sb.Append("wide. ");
				}
				else if (rando % 3 == 1)
				{
					sb.Append("thick. ");
				}
				else
				{
					sb.Append(" in diameter. ");
				}

				sb.Append(cock.PlayerDescription() + Environment.NewLine);

			}

			return sb.ToString();
		}

		public static string Name()
		{
			return "All Cocks";
		}
	}
}
