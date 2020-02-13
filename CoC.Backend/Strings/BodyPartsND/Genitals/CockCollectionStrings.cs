using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Tools;

namespace CoC.Backend.BodyParts
{
	internal interface ICockCollection<T> where T : ICock
	{
		ReadOnlyCollection<T> cocks { get; }
		bool hasSheath { get; }

		CockData AverageCock();

	}

	internal static class CockCollectionStrings
	{
		private static int NumCocks<T>(this ICockCollection<T> collection) where T : ICock
		{
			return collection.cocks.Count;
		}


		private static readonly string[] lottaDicksOptions = new string[] { "bundle of ", "obscene group of ", "cluster of ", "wriggling bunch of " };

		private static readonly string[] mismatchedDicksTextOptions = new string[] { "mutated cocks", "mutated dicks", "mixed cocks", "mismatched dicks" };

		#region Cock Text
		internal static string SheathOrBaseStr<T>(ICockCollection<T> collection) where T : ICock
		{
			return collection.hasSheath ? "sheath" : "base";
		}

		internal static string AllCocksShortDescription<T>(ICockCollection<T> collection, out bool isPlural) where T : ICock
		{
			if (collection.cocks.Count == 0)
			{
				isPlural = true;
				return "";
			}
			else if (collection.cocks.Count == 1)
			{
				isPlural = false;
				return collection.cocks[0].ShortDescription();
			}
			bool mismatched = collection.cocks.Any(x => x.type != collection.cocks[0].type);

			isPlural = true;
			return mismatched ? CockType.GenericCockNoun(true) : collection.cocks[0].ShortDescription(false, true);
		}

		internal static string AllCocksLongDescription<T>(ICockCollection<T> collection, out bool isPlural) where T : ICock
		{
			return AllCocksDesc(collection, false, out isPlural);
		}

		internal static string AllCocksFullDescription<T>(ICockCollection<T> collection, out bool isPlural) where T : ICock
		{
			return AllCocksDesc(collection, true, out isPlural);
		}

		internal static string OneCockOrCocksNoun<T>(ICockCollection<T> collection, string pronoun = "your") where T : ICock
		{
			if (collection.cocks.Count == 0)
			{
				return "";
			}

			return CommonBodyPartStrings.OneOfDescription(collection.cocks.Count > 1, pronoun, CockType.GenericCockNoun(collection.cocks.Count > 1));
		}

		internal static string OneCockOrCocksShort<T>(ICockCollection<T> collection, string pronoun = "your") where T : ICock
		{
			if (collection.cocks.Count == 0)
			{
				return "";
			}

			return CommonBodyPartStrings.OneOfDescription(collection.cocks.Count > 1, pronoun, AllCocksShortDescription(collection, out bool _));
		}

		internal static string EachCockOrCocksNoun<T>(ICockCollection<T> collection, string pronoun = "your") where T : ICock
		{
			return EachCockOrCocksNoun(collection, pronoun, out bool _);
		}

		internal static string EachCockOrCocksShort<T>(ICockCollection<T> collection, string pronoun = "your") where T : ICock
		{
			return EachCockOrCocksShort(collection, pronoun, out bool _);
		}

		internal static string EachCockOrCocksNoun<T>(ICockCollection<T> collection, string pronoun, out bool isPlural) where T : ICock
		{
			isPlural = collection.cocks.Count != 1;
			if (collection.cocks.Count == 0)
			{
				return "";
			}

			return CommonBodyPartStrings.EachOfDescription(collection.cocks.Count > 1, pronoun, CockType.GenericCockNoun(collection.cocks.Count > 1));
		}

		internal static string EachCockOrCocksShort<T>(ICockCollection<T> collection, string pronoun, out bool isPlural) where T : ICock
		{
			if (collection.cocks.Count == 0)
			{
				isPlural = true;
				return "";
			}

			return CommonBodyPartStrings.EachOfDescription(collection.cocks.Count > 1, pronoun, AllCocksShortDescription(collection, out isPlural));
		}
		#endregion


		private static string AllCocksDesc<T>(ICockCollection<T> collection, bool full, out bool isPlural) where T : ICock
		{
			if (collection.cocks.Count == 0)
			{
				isPlural = true;
				return "";
			}
			//If one, return normal cock descript
			else if (collection.cocks.Count == 1)
			{
				isPlural = false;
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

				isPlural = true;
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


		//displays

		public string GenericChangeOneCockLengthText(Creature target, CockData oldData, bool wasRemovedBecauseTooSmall)
		{
			if (target == null)
			{
				throw new ArgumentNullException(nameof(target));
			}

			if (oldData == null)
			{
				throw new ArgumentNullException(nameof(oldData));
			}

			StringBuilder sb = new StringBuilder();
			float deltaAmount = wasRemovedBecauseTooSmall ? -oldData.length : target.cocks[oldData.cockIndex].length - oldData.length;
			Cock newValue = wasRemovedBecauseTooSmall ? null : target.cocks[oldData.cockIndex];

			if (deltaAmount <= 1 && deltaAmount > 0)
			{
				if (target.cocks.Count == 1)
				{
					sb.Append("Your " + oldData.LongDescription() + " has grown slightly longer.");
				}
				else //if (target.cocks.Count > 1)
				{
					sb.Append("One of your " + target.genitals.AllCocksShortDescription() + " grows slightly longer.");
				}
			}
			else if (deltaAmount > 1 && deltaAmount < 3)
			{
				if (target.cocks.Count == 1)
				{
					sb.Append("A very pleasurable feeling spreads from your groin as your " + oldData.LongDescription() + " grows permanently longer - at least "
						+ (Measurement.UsesMetric ? "a few centimeters" : "an inch") + " - and leaks pre-cum from the pleasure of the change.");
				}

				else //if (target.cocks.Count > 1)
				{
						sb.Append("A very pleasurable feeling spreads from your groin as one of your " + target.genitals.AllCocksShortDescription() + " grows permanently longer, " +
							"by at least " + (Measurement.UsesMetric ? "a few centimeters" : "an inch") + ", and leaks plenty of pre-cum from the pleasure of the change.");
				}
			}
			else if (deltaAmount >= 3)
			{
				if (target.cocks.Count == 1)
				{
					sb.Append("Your " + oldData.LongDescription() + " feels incredibly tight as a few more " + (Measurement.UsesMetric ? "centimeters" : "inches") +
						" of length seem to pour out from your crotch.");
				}

				else //if (target.cocks.Count > 1)
				{
						sb.Append("Your " + target.genitals.AllCocksShortDescription() + " feel incredibly tight as one of their number begins to grow, "
							+ (Measurement.UsesMetric ? "centimeter after centimeter" : "inch after inch") + ".");
				}
			}


			//Display the degree of length loss.
			else if (deltaAmount >= -1)
			{
				if (target.cocks.Count == 1)
				{
					sb.Append("Your " + oldData.ShortDescription() + " has shrunk to a slightly shorter length.");
				}
				else //if (target.cocks.Count > 1)
				{
					sb.Append("You feel one of your " + target.genitals.AllCocksShortDescription() + " shrink to a slightly shorter length.");
				}
			}
			else if (deltaAmount > -3)
			{
				if (target.cocks.Count == 1)
				{
					sb.Append("Your " + oldData.ShortDescription() + " shrinks smaller, flesh vanishing into your groin.");
				}
				else //if (target.cocks.Count > 1)
				{
					sb.Append("You feel one of your " + target.genitals.AllCocksShortDescription() + " shrink smaller, the flesh vanishing into your groin.");
				}
			}
			else //if (deltaAmount <= -3)
			{
				if (target.cocks.Count == 1)
				{
					sb.Append("A large portion of your " + oldData.ShortDescription() + "'s length shrinks and vanishes.");
				}
				else //if (target.cocks.Count > 1)
				{
					sb.Append("A single member of your " + target.genitals.AllCocksShortDescription() + " vanishes into your groin, receding rapidly in length.");
				}
			}

			Cock largestUnchangedCock;

			if (wasRemovedBecauseTooSmall)
			{
				largestUnchangedCock = target.genitals.LongestCock();
			}
			else
			{
				largestUnchangedCock = target.cocks.Where((x, y) => y != x.cockIndex).MaxItem(x => x.length);
			}


			//Display LengthChange
			if (deltaAmount > 0)
			{
				//this now handles multiple cocks better - if we already had a cock at the current threshold, we don't mention what having a cock of that length entails.
				//we may, however, mention we have another cock at that threshold.

				//start with the largest and work our way down.
				if (newValue.length >= 20 && newValue.length - deltaAmount < 20)
				{
					if (largestUnchangedCock is null || largestUnchangedCock.length < 20)
					{
						sb.Append(SafelyFormattedString.FormattedText("As if the pulsing heat of your " + newValue.LongDescription() + " wasn't bad enough, your " +
							newValue.ShortDescription() + "'s " + newValue.HeadDescription() + " keeps poking its way into your view every time you get hard.", StringFormats.BOLD));

						//then, describe what this entails.
						if (target.corruption > 80)
						{
							sb.Append(" You find yourself fantasizing about impaling nubile young champions on your " + target.genitals.AllCocksLongDescription() + " in a year's time.");
						}
						else if (target.corruption > 60)
						{
							sb.Append(" You daydream about being attacked by a massive tentacle beast, its tentacles engulfing your " + target.genitals.AllCocksLongDescription() + " to the hilt, milking it of all your cum.\n\nYou smile at the pleasant thought.");
						}
						else if (target.corruption > 40)
						{
							if (target.cocks.Count == 1)
							{
								sb.Append(" You wonder if there is a demon or beast out there that could handle your full length.");
							}
							else
							{
								sb.Append(" You wonder - is a demon or beast out there that could take the full length of the largest of your " + target.genitals.AllCocksShortDescription() + "?");
							}
						}
					}
					//otherwise, just mention that you now have more that can obscure your vision
					else
					{
						sb.Append("It's grown so long that you now have yet another cock that partially obscures your vision when erect.");
					}
				}
				else if (newValue.length >= 16 && newValue.length - deltaAmount < 16)
				{

					sb.Append(" <b>");
					sb.Append("Your " + newValue.LongDescription() + " would look more at home on a large horse than you");

					//if multiple cocks, and all the cocks that grew longer are still shorter than the longest, make note of that.
					if (!(largestUnchangedCock is null) && newValue.length < largestUnchangedCock.length)
					{
						sb.Append(", though it's still not as long as your " + largestUnchangedCock.LongDescription());
					}
					sb.Append(".</b>");

					//if it is the longest, that means we've just reached this threshold. mention that we can now tit-fuck ourselves.
					if (largestUnchangedCock is null || largestUnchangedCock.length < 16)
					{
						if (target.genitals.BiggestCupSize() >= CupSize.C)
						{
							sb.Append(" You could easily stuff your " + newValue.LongDescription() + " between your breasts and give yourself the titty-fuck of a lifetime.");
						}
						else
						{
							sb.Append(" Your " + newValue.LongDescription() + " is so long it easily reaches your chest. " +
								"The possibility of autofellatio is now a foregone conclusion.");
						}
					}
				}
				else if (newValue.length >= 12 && newValue.length - deltaAmount < 12)
				{
					sb.Append(" <b>");

					sb.Append("Your " + target.cocks[0].LongDescription() + " is so long it nearly swings to your knee at its full length");

					//if multiple cocks, and all the cocks that grew longer are still shorter than the longest, make note of that.
					if (!(largestUnchangedCock is null) && newValue.length < largestUnchangedCock.length)
					{
						sb.Append(", though it's still not as long as your " + largestUnchangedCock.LongDescription());
					}

					sb.Append(".</b>");
				}
				else if (newValue.length >= 8 && newValue.length - deltaAmount < 8)
				{
					sb.Append(" <b>");

					sb.Append(" Most men would be overly proud to have a tool as long as yours");

					//if multiple cocks, and all the cocks that grew longer are still shorter than the longest, make note of that.
					if (!(largestUnchangedCock is null) && newValue.length < largestUnchangedCock.length)
					{
						sb.Append(", and it's still not as long as your " + largestUnchangedCock.LongDescription() + "!");
					}
					else
					{
						sb.Append(".");
					}

					sb.Append("</b>");
				}
			}
			else if (wasRemovedBecauseTooSmall)
			{

			}

			return sb.ToString();
		}


	}
		//handles any amount of cocks growing or shrinking, even cases where some shrink and other grow. note that if you set all to try and reach a certain threshold,
		//it will probably be a better idea to simply use your own text.


		//public string GenericChangeCockLengthText(Creature target, CockCollectionData oldCockData, CollectionChangedHelper adjustedIndices)
		//{
		//	//handle null checks so we don't have to worry about them.
		//	if (target == null)
		//	{
		//		throw new ArgumentNullException(nameof(target));
		//	}

		//	if (oldCockData == null)
		//	{
		//		throw new ArgumentNullException(nameof(oldCockData));
		//	}

		//	if (adjustedIndices == null)
		//	{
		//		throw new ArgumentNullException(nameof(adjustedIndices));
		//	}

		//	List<ValueDifference<CockData>> changed = adjustedIndices.AdjustedElements<CockData>(oldCockData.cocks, target.cocks.Select(x => x.AsReadOnlyData()).ToList());
		//	return GenericChangeCockLengths(target, changed);
		//}

		////note: any cocks that were added will be ignored. any cocks that are removed are presumed to have shrunk so much they were removed.
		//private string GenericChangeCockLengths(Creature target, List<ValueDifference<CockData>> changedCocks)
		//{
		//	var changed = changedCocks.Where(x => !(x.oldValue is null)).ToArray();

		//	float[] delta = new float[changed.Length];


		//	int oneIndex = -1;
		//	for (int x = 0; x < changed.Length; x++)
		//	{
		//		if (changed[x].newValue is null)
		//		{
		//			delta[x] = changed[x].oldValue.length;
		//		}
		//		else
		//		{
		//			delta[x] = changed[x].oldValue.length - changed[x].newValue.length;
		//			oneIndex = x;
		//		}
		//	}

		//	int cockChangeCount = delta.Count(x => x != 0);
		//	//handle no cocks changed (either because growCocks is null or delta is 0) with a quick exit.
		//	if (cockChangeCount == 0)
		//	{
		//		return "";
		//	}
		//	if (cockChangeCount == 1)
		//	{
		//		return GenericChangeOneCockLengthText(target, changed[oneIndex].oldValue, changed[oneIndex].newValue is null);
		//	}

		//	int mostGrownIndex = delta.MaxIndex(x => x);
		//	int mostShrunkIndex = delta.MinIndex(x => x);

		//	//growing.
		//	Cock largestGrownCock = target.cocks.Where((x, y) => cockDeltas[y] > 0).OrderByDescending(x => x.).FirstOrDefault();
		//	float growthDelta = target.cocks.Where((x, y))

		//	//shrinking.
		//	Cock shortestShrunkCock = target.cocks.Where(x, y)

		//	StringBuilder sb = new StringBuilder();
		//	//DIsplay the degree of length change.
		//	if (deltaAmount <= 1 && deltaAmount > 0)
		//	{
		//		if (target.cocks.Count == 1)
		//		{
		//			sb.Append("Your " + target.cocks[0].LongDescription() + " has grown slightly longer.");
		//		}
		//		else //if (target.cocks.Count > 1)
		//		{
		//			if (cockChangeCount == 1)
		//			{
		//				sb.Append("One of your " + target.genitals.AllCocksShortDescription() + " grows slightly longer.");
		//			}
		//			else if (cockChangeCount < target.cocks.Count)
		//			{
		//				sb.Append("Some of your " + target.genitals.AllCocksShortDescription() + " grow slightly longer.");
		//			}
		//			else //if (cockChangeCount == target.cocks.Count)
		//			{
		//				sb.Append("Your " + target.genitals.AllCocksShortDescription() + " seem to fill up... growing a little bit larger.");
		//			}
		//		}
		//	}
		//	else if (deltaAmount > 1 && deltaAmount < 3)
		//	{
		//		if (target.cocks.Count == 1)
		//		{
		//			sb.Append("A very pleasurable feeling spreads from your groin as your " + target.cocks[0].LongDescription() + " grows permanently longer - at least "
		//				+ (Measurement.UsesMetric ? "a few centimeters" : "an inch") + " - and leaks pre-cum from the pleasure of the change.");
		//		}

		//		else //if (target.cocks.Count > 1)
		//		{
		//			if (cockChangeCount == 1)
		//			{
		//				sb.Append("A very pleasurable feeling spreads from your groin as one of your " + target.genitals.AllCocksShortDescription() + " grows permanently longer, " +
		//					"by at least " + (Measurement.UsesMetric ? "a few centimeters" : "an inch") + ", and leaks plenty of pre-cum from the pleasure of the change.");
		//			}
		//			else if (cockChangeCount < target.cocks.Count)
		//			{
		//				sb.Append("A very pleasurable feeling spreads from your groin as " + Utils.NumberAsText(cockChangeCount) + " of your "
		//					+ target.genitals.AllCocksShortDescription() + " grow permanently longer, by at least " + (Measurement.UsesMetric ? "a few centimeters" : "an inch") +
		//					", and leak plenty of pre-cum from the pleasure of the change.");
		//			}
		//			else //if (cockChangeCount == target.cocks.Count)
		//			{
		//				sb.Append("A very pleasurable feeling spreads from your groin as your " + target.genitals.AllCocksShortDescription() + " grow permanently longer - at least "
		//					+ (Measurement.UsesMetric ? "a few centimeters" : "an inch") + " - and leak plenty of pre-cum from the pleasure of the change.");
		//			}
		//		}
		//	}
		//	else if (deltaAmount >= 3)
		//	{
		//		if (target.cocks.Count == 1)
		//		{
		//			sb.Append("Your " + target.cocks[0].LongDescription() + " feels incredibly tight as a few more " + (Measurement.UsesMetric ? "centimeters" : "inches") + " of length seem to pour out from your crotch.");
		//		}

		//		else //if (target.cocks.Count > 1)
		//		{
		//			if (cockChangeCount == 1)
		//			{
		//				sb.Append("Your " + target.genitals.AllCocksShortDescription() + " feel incredibly tight as one of their number begins to grow, "
		//					+ (Measurement.UsesMetric ? "centimeter after centimeter" : "inch after inch") + ".");
		//			}
		//			else if (cockChangeCount < target.cocks.Count)
		//			{
		//				sb.Append("Your " + target.genitals.AllCocksShortDescription() + " feel incredibly numb as " + Utils.NumberAsText(cockChangeCount) + " of them begin to grow, "
		//					+ (Measurement.UsesMetric ? "centimeter after centimeter" : "inch after inch") + " ");
		//			}
		//			else //if (cockChangeCount == target.cocks.Count)
		//			{
		//				sb.Append("Your " + target.genitals.AllCocksShortDescription() + " feel incredibly tight as they grow,"
		//					+ (Measurement.UsesMetric ? "centimeter after centimeter" : "inch after inch") + " of added length pouring from your groin.");
		//			}
		//		}
		//	}
		//	//Display LengthChange
		//	if (deltaAmount > 0)
		//	{
		//		//this now handles multiple cocks better - if we already had a cock at the current threshold, we don't mention what having a cock of that length entails.
		//		//we may, however, mention we have another cock at that threshold.

		//		//start with the largest and work our way down.
		//		if (largestUpdatedCock.length >= 20 && largestUpdatedCock.length - deltaAmount < 20)
		//		{
		//			bool describeAll = target.cocks.All((x, y) => x.length >= 20 && cocksChanged[y] && x.length - deltaAmount < 20);
		//			bool describeSeveral = target.cocks.Any((x, y) => x.length >= 20 && cocksChanged[y] && x != largestUpdatedCock && x.length - deltaAmount < 20);

		//			//if we didn't have any cocks above 20 before this, mention how it/they now obscure your lower vision.
		//			if (largestUnchangedCock is null || largestUnchangedCock.length < 20)
		//			{
		//				sb.Append("<b>As if the pulsing heat ");


		//				if (cockChangeCount == 1)
		//				{
		//					sb.Append("of your " + largestUpdatedCock.LongDescription());
		//				}
		//				else
		//				{
		//					sb.Append("of your " + target.genitals.AllCocksShortDescription());
		//				}
		//				sb.Append(" wasn't bad enough, ");

		//				if (describeAll || describeSeveral)
		//				{
		//					sb.Append("every time you get hard, the tips of ");

		//					if (describeSeveral)
		//					{
		//						sb.Append("several of ");
		//					}
		//					sb.Append("your " + target.genitals.AllCocksShortDescription() + "wave before you, obscuring the lower portions of your vision");
		//				}
		//				else
		//				{
		//					sb.Append("your " + largestUpdatedCock.ShortDescription() + "'s " + largestUpdatedCock.HeadDescription() + " keeps poking its way into your view " +
		//						"every time you get hard.");
		//				}

		//				sb.Append("</b>");

		//				//then, describe what this entails.
		//				if (target.corruption > 80)
		//				{
		//					sb.Append(" You find yourself fantasizing about impaling nubile young champions on your " + target.genitals.AllCocksLongDescription() + " in a year's time.");
		//				}
		//				else if (target.corruption > 60)
		//				{
		//					sb.Append(" You daydream about being attacked by a massive tentacle beast, its tentacles engulfing your " + target.genitals.AllCocksLongDescription() + " to the hilt, milking it of all your cum.\n\nYou smile at the pleasant thought.");
		//				}
		//				else if (target.corruption > 40)
		//				{
		//					if (target.cocks.Count == 1)
		//					{
		//						sb.Append(" You wonder if there is a demon or beast out there that could handle your full length.");
		//					}
		//					else
		//					{
		//						sb.Append(" You wonder - is a demon or beast out there that could take the full length of the largest of your " + target.genitals.AllCocksShortDescription() + "?");
		//					}
		//				}
		//			}
		//			//otherwise, just mention that you now have more that can obscure your vision
		//			else
		//			{
		//				if (describeAll || describeSeveral)
		//				{
		//					sb.Append((describeAll ? "They've each" : "Several have") + " grown long enough that you now have even more cocks starting to obscure your " +
		//						"vision whenever you have an erection.");
		//				}
		//				else
		//				{
		//					sb.Append("It's grown so long that you now have yet another cock that partially obscures your vision when erect.");
		//				}
		//			}
		//		}
		//		else if (largestUpdatedCock.length >= 16 && largestUpdatedCock.length - deltaAmount < 16)
		//		{

		//			bool describeAll = target.cocks.All((x, y) => x.length >= 16 && cocksChanged[y] && x.length - deltaAmount < 16);

		//			sb.Append(" <b>");
		//			if (describeAll)
		//			{
		//				sb.Append("Each one of your " + target.genitals.AllCocksShortDescription() + "now looks like it'd be more at home " +
		//					"on a large horse, let alone together on one body");
		//			}
		//			else if (cockChangeCount == 1)
		//			{
		//				sb.Append("Your " + largestUpdatedCock.LongDescription() + " would look more at home on a large horse than you");
		//			}
		//			else
		//			{
		//				sb.Append("The largest of them now looks like it'd be more at home on a large horse than you");
		//			}

		//			//if multiple cocks, and all the cocks that grew longer are still shorter than the longest, make note of that.
		//			if (!describeAll && !(largestUnchangedCock is null) && largestUpdatedCock.length < largestUnchangedCock.length)
		//			{
		//				sb.Append(", though it's still not as long as your " + largestUnchangedCock.LongDescription());
		//			}
		//			sb.Append(".</b>");

		//			//if it is the longest, that means we've just reached this threshold. mention that we can now tit-fuck ourselves.
		//			if (largestUnchangedCock is null || largestUnchangedCock.length < 16)
		//			{
		//				if (target.genitals.BiggestCupSize() >= CupSize.C)
		//				{
		//					//you only have one cock that grew larger or all the other ones are still below the threshold.
		//					if (cockChangeCount == 1 || target.cocks.Where((x, y) => cocksChanged[y] && x != largestUpdatedCock).All(x => x.length < 16))
		//					{
		//						sb.Append(" You could easily stuff your " + largestUpdatedCock.LongDescription() + " between your breasts and give yourself the titty-fuck of a lifetime.");
		//					}
		//					//some (but not all) are now this large.
		//					else if (!describeAll)
		//					{
		//						sb.Append(" Several of your " + target.genitals.AllCocksShortDescription() + " now reach so far up your chest it would be easy to stuff a few of them " +
		//							"between your breasts and give yourself the titty-fuck of a lifetime.");
		//					}
		//					else //if (target.cocks.Count > 1)
		//					{
		//						sb.Append(" They reach so far up your chest it would be easy to stuff a few cocks between your breasts and give yourself the titty-fuck of a lifetime.");
		//					}
		//				}
		//				else
		//				{
		//					if (cockChangeCount == 1 || target.cocks.Where((x, y) => cocksChanged[y] && x != largestUpdatedCock).All(x => x.length < 16))
		//					{
		//						sb.Append(" Your " + largestUpdatedCock.LongDescription() + " is so long it easily reaches your chest. " +
		//							"The possibility of autofellatio is now a foregone conclusion.");
		//					}
		//					else if (!describeAll)
		//					{
		//						sb.Append(" Several of your " + target.genitals.AllCocksShortDescription() + "are now long enough to easily reach your chest. " +
		//							"Autofellatio would be about as hard as looking down.");
		//					}
		//					else
		//					{
		//						sb.Append(" They are so long that they easily reach your chest; you'd be able to perform autofellatio on any of them with little effort.");
		//					}
		//				}
		//			}
		//		}
		//		else if (largestUpdatedCock.length >= 12 && largestUpdatedCock.length - deltaAmount < 12)
		//		{
		//			bool describeAll = target.cocks.All((x, y) => x.length >= 12 && cocksChanged[y] && x.length - deltaAmount < 12);
		//			sb.Append(" <b>");
		//			if (describeAll)
		//			{
		//				sb.Append("They are all so long now that they nearly reach your knees when at full length");
		//			}
		//			else if (cockChangeCount > 1)
		//			{
		//				sb.Append("The largest of them is now so long, it nearly reaches your knees");
		//			}
		//			else //if (cockChangeCount == 1)
		//			{
		//				sb.Append("Your " + target.cocks[0].LongDescription() + " is so long it nearly swings to your knee at its full length");
		//			}

		//			//if multiple cocks, and all the cocks that grew longer are still shorter than the longest, make note of that.
		//			if (!describeAll && !(largestUnchangedCock is null) && largestUpdatedCock.length < largestUnchangedCock.length)
		//			{
		//				sb.Append(", though it's still not as long as your " + largestUnchangedCock.LongDescription());
		//			}

		//			sb.Append(".</b>");
		//		}
		//		else if (largestUpdatedCock.length >= 8 && largestUpdatedCock.length - deltaAmount < 8)
		//		{
		//			bool describeAll = target.cocks.All((x, y) => x.length >= 8 && cocksChanged[y] && x.length - deltaAmount < 8);
		//			bool describeSeveral = target.cocks.Any((x, y) => x.length >= 20 && cocksChanged[y] && x != largestUpdatedCock && x.length - deltaAmount < 20);
		//			sb.Append("<b>");

		//			if (describeAll)
		//			{
		//				sb.Append("Most men would be overly proud to have a cock as long as your " + target.genitals.ShortestCock().LongDescription() +
		//					", and that's the shortest one you have!");
		//			}
		//			else if (describeSeveral)
		//			{
		//				sb.Append("Several have now reached lengths most men would be proud to match");
		//			}
		//			else if (target.cocks.Count != 1)
		//			{
		//				sb.Append("The largest is now long enough most men would be proud to match its length");
		//			}
		//			else //if (target.cocks.Count == 1)
		//			{
		//				sb.Append(" Most men would be overly proud to have a tool as long as yours");
		//			}



		//			//if multiple cocks, and all the cocks that grew longer are still shorter than the longest, make note of that.
		//			if (!describeAll && !(largestUnchangedCock is null) && largestUpdatedCock.length < largestUnchangedCock.length)
		//			{
		//				sb.Append(", and it's still not as long as your " + largestUnchangedCock.LongDescription() + "!");
		//			}
		//			else
		//			{
		//				sb.Append(".");
		//			}

		//			sb.Append("</b>");
		//		}
		//	}

		//	//Display the degree of length loss.
		//	else if (deltaAmount >= -1)
		//	{
		//		if (target.cocks.Count == 1)
		//		{
		//			sb.Append("Your " + target.genitals.AllCocksShortDescription() + " has shrunk to a slightly shorter length.");
		//		}
		//		else //if (target.cocks.Count > 1)
		//		{
		//			if (cockChangeCount == target.cocks.Count)
		//			{
		//				sb.Append("Your " + target.genitals.AllCocksShortDescription() + " have shrunk to a slightly shorter length.");
		//			}
		//			else if (cockChangeCount > 1)
		//			{
		//				sb.Append("You feel " + Utils.NumberAsText(cockChangeCount) + " of your " + target.genitals.AllCocksShortDescription() + " have shrunk to a slightly shorter length.");
		//			}
		//			else //if (cockChangeCount == 1)
		//			{
		//				sb.Append("You feel " + Utils.NumberAsText(cockChangeCount) + " of your " + target.genitals.AllCocksShortDescription() + " has shrunk to a slightly shorter length.");
		//			}
		//		}
		//	}
		//	else if (deltaAmount > -3)
		//	{
		//		if (target.cocks.Count == 1)
		//		{
		//			sb.Append("Your " + target.genitals.AllCocksShortDescription() + " shrinks smaller, flesh vanishing into your groin.");
		//		}
		//		else //if (target.cocks.Count > 1)
		//		{
		//			if (cockChangeCount == target.cocks.Count)
		//			{
		//				sb.Append("Your " + target.genitals.AllCocksShortDescription() + " shrink smaller, the flesh vanishing into your groin.");
		//			}
		//			else if (cockChangeCount > 1)
		//			{
		//				sb.Append("You feel " + Utils.NumberAsText(cockChangeCount) + " of your " + target.genitals.AllCocksShortDescription() + " shrink smaller, the flesh vanishing into your groin.");
		//			}
		//			else //if (cockChangeCount == 1)
		//			{
		//				sb.Append("You feel " + Utils.NumberAsText(cockChangeCount) + " of your " + target.genitals.AllCocksShortDescription() + " shrink smaller, the flesh vanishing into your groin.");
		//			}
		//		}
		//	}
		//	else //if (deltaAmount <= -3)
		//	{
		//		if (target.cocks.Count == 1)
		//		{
		//			sb.Append("A large portion of your " + target.genitals.AllCocksShortDescription() + "'s length shrinks and vanishes.");
		//		}
		//		else //if (target.cocks.Count > 1)
		//		{
		//			if (cockChangeCount == target.cocks.Count)
		//			{
		//				sb.Append("A large portion of your " + target.genitals.AllCocksShortDescription() + " recedes towards your groin, receding rapidly in length.");
		//			}
		//			else if (cockChangeCount > 1)
		//			{
		//				sb.Append("Your " + target.genitals.AllCocksShortDescription() + " tingles as " + Utils.NumberAsText(cockChangeCount) + " of your members vanish into your groin, receding rapidly in length.");
		//			}
		//			else //if (cockChangeCount == 1)
		//			{
		//				sb.Append("A single member of your " + target.genitals.AllCocksShortDescription() + " vanishes into your groin, receding rapidly in length.");
		//			}
		//		}
		//	}

		//	return sb.ToString();
		//}
	//}
}
