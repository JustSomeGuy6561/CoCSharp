using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.LinqHelpers;

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

		internal static string OneCockOrCocksNoun<T>(ICockCollection<T> collection) where T : ICock => OneCockOrCocksNoun(collection, Conjugate.YOU);
		internal static string OneCockOrCocksNoun<T>(ICockCollection<T> collection, Conjugate conjugate) where T : ICock
		{
			if (collection.cocks.Count == 0)
			{
				return "";
			}

			return CommonBodyPartStrings.OneOfDescription(collection.cocks.Count > 1, conjugate, CockType.GenericCockNoun(collection.cocks.Count > 1));
		}

		internal static string OneCockOrCocksShort<T>(ICockCollection<T> collection) where T : ICock => OneCockOrCocksShort(collection, Conjugate.YOU);
		internal static string OneCockOrCocksShort<T>(ICockCollection<T> collection, Conjugate conjugate) where T : ICock
		{
			if (collection.cocks.Count == 0)
			{
				return "";
			}

			return CommonBodyPartStrings.OneOfDescription(collection.cocks.Count > 1, conjugate, AllCocksShortDescription(collection, out bool _));
		}

		internal static string EachCockOrCocksNoun<T>(ICockCollection<T> collection) where T : ICock => EachCockOrCocksNoun(collection, Conjugate.YOU);
		internal static string EachCockOrCocksNoun<T>(ICockCollection<T> collection, Conjugate conjugate) where T : ICock
		{
			return EachCockOrCocksNoun(collection, conjugate, out bool _);
		}

		internal static string EachCockOrCocksShort<T>(ICockCollection<T> collection) where T : ICock => EachCockOrCocksShort(collection, Conjugate.YOU);
		internal static string EachCockOrCocksShort<T>(ICockCollection<T> collection, Conjugate conjugate) where T : ICock
		{
			return EachCockOrCocksShort(collection, conjugate, out bool _);
		}

		internal static string EachCockOrCocksNoun<T>(ICockCollection<T> collection, Conjugate conjugate, out bool isPlural) where T : ICock
		{
			isPlural = collection.cocks.Count != 1;
			if (collection.cocks.Count == 0)
			{
				return "";
			}

			return CommonBodyPartStrings.EachOfDescription(collection.cocks.Count > 1, conjugate, CockType.GenericCockNoun(collection.cocks.Count > 1));
		}

		internal static string EachCockOrCocksShort<T>(ICockCollection<T> collection, Conjugate conjugate, out bool isPlural) where T : ICock
		{
			if (collection.cocks.Count == 0)
			{
				isPlural = true;
				return "";
			}

			return CommonBodyPartStrings.EachOfDescription(collection.cocks.Count > 1, conjugate, AllCocksShortDescription(collection, out isPlural));
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

		public string GenericChangeOneCockLengthText(CockData oldData, bool treatMissingAsRemoved = true, bool displayRemovedText = true)
		{
			if (oldData == null)
			{
				throw new ArgumentNullException(nameof(oldData));
			}

			if (creatureID != oldData.creatureID)
			{
				return "";
			}

			StringBuilder sb = new StringBuilder();

			Cock newValue = cocks.FirstOrDefault(x => x.creatureID == oldData.creatureID && x.collectionID == oldData.collectionID);
			if (newValue is null && !treatMissingAsRemoved)
			{
				return "";
			}

			double deltaAmount = !(newValue is null) ? newValue.length - oldData.length : -oldData.length;

			if (deltaAmount <= 1 && deltaAmount > 0)
			{
				if (cocks.Count == 1)
				{
					sb.Append("Your " + oldData.LongDescription() + " has grown slightly longer.");
				}
				else //if (cocks.Count > 1)
				{
					sb.Append("One of your " + AllCocksShortDescription() + " grows slightly longer.");
				}
			}
			else if (deltaAmount > 1 && deltaAmount < 3)
			{
				if (cocks.Count == 1)
				{
					sb.Append("A very pleasurable feeling spreads from your groin as your " + oldData.LongDescription() + " grows permanently longer - at least "
						+ (Measurement.UsesMetric ? "a few centimeters" : "an inch") + " - and leaks pre-cum from the pleasure of the change.");
				}

				else //if (cocks.Count > 1)
				{
					sb.Append("A very pleasurable feeling spreads from your groin as one of your " + AllCocksShortDescription() + " grows permanently longer, " +
						"by at least " + (Measurement.UsesMetric ? "a few centimeters" : "an inch") + ", and leaks plenty of pre-cum from the pleasure of the change.");
				}
			}
			else if (deltaAmount >= 3)
			{
				if (cocks.Count == 1)
				{
					sb.Append("Your " + oldData.LongDescription() + " feels incredibly tight as a few more " + (Measurement.UsesMetric ? "centimeters" : "inches") +
						" of length seem to pour out from your crotch.");
				}

				else //if (cocks.Count > 1)
				{
					sb.Append("Your " + AllCocksShortDescription() + " feel incredibly tight as one of their number begins to grow, "
						+ (Measurement.UsesMetric ? "centimeter after centimeter" : "inch after inch") + ".");
				}
			}


			//Display the degree of length loss.
			else if (deltaAmount >= -1)
			{
				if (cocks.Count == 1)
				{
					sb.Append("Your " + oldData.ShortDescription() + " has shrunk to a slightly shorter length.");
				}
				else //if (cocks.Count > 1)
				{
					sb.Append("You feel one of your " + AllCocksShortDescription() + " shrink to a slightly shorter length.");
				}
			}
			else if (deltaAmount > -3)
			{
				if (cocks.Count == 1)
				{
					sb.Append("Your " + oldData.ShortDescription() + " shrinks smaller, flesh vanishing into your groin.");
				}
				else //if (cocks.Count > 1)
				{
					sb.Append("You feel one of your " + AllCocksShortDescription() + " shrink smaller, the flesh vanishing into your groin.");
				}
			}
			else //if (deltaAmount <= -3)
			{
				if (cocks.Count == 1)
				{
					sb.Append("A large portion of your " + oldData.ShortDescription() + "'s length shrinks and vanishes.");
				}
				else //if (cocks.Count > 1)
				{
					sb.Append("A single member of your " + AllCocksShortDescription() + " vanishes into your groin, receding rapidly in length.");
				}
			}

			Cock largestUnchangedCock;

			if (newValue is null)
			{
				largestUnchangedCock = LongestCock();
			}
			else
			{
				largestUnchangedCock = cocks.Where(x=> x!=newValue).MaxItem(x => x.length);
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
						if (creature?.corruption > 80)
						{
							sb.Append(" You find yourself fantasizing about impaling nubile young champions on your " + AllCocksLongDescription() + " in a year's time.");
						}
						else if (creature?.corruption > 60)
						{
							sb.Append(" You daydream about being attacked by a massive tentacle beast, its tentacles engulfing your " + AllCocksLongDescription() + " to the hilt, milking it of all your cum.\n\nYou smile at the pleasant thought.");
						}
						else if (creature?.corruption > 40)
						{
							if (cocks.Count == 1)
							{
								sb.Append(" You wonder if there is a demon or beast out there that could handle your full length.");
							}
							else
							{
								sb.Append(" You wonder - is a demon or beast out there that could take the full length of the largest of your " + AllCocksShortDescription() + "?");
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
						if (source.BiggestCupSize() >= CupSize.C)
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

					sb.Append("Your " + cocks[0].LongDescription() + " is so long it nearly swings to your knee at its full length");

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
			else if (newValue is null && displayRemovedText)
			{
				if (cocks.Count == 0)
				{
					sb.Append("<b>Your manhood shrinks into your body, disappearing completely.</b>");
				}
				else if (cocks.Count == 1)
				{
					sb.Append("<b>Your " + oldData.LongDescription() + " disappears, shrinking into your body and leaving you with just one "
						+ cocks[0].ShortDescription() + ".</b>");
				}
				else
				{
					sb.Append("<b>Your smallest penis disappears forever, leaving you with just your " + AllCocksShortDescription() + ".</b>");
				}
			}

			return sb.ToString();
		}

		public string GenericChangeCockLengthText(CockCollectionData oldCockData, bool treatMissingCocksAsRemoved = true, bool displayRemovedText = true)
		{

			if (oldCockData == null)
			{
				throw new ArgumentNullException(nameof(oldCockData));
			}

			if (!CollectionChanged(oldCockData, true))
			{
				return "";
			}

			IEnumerable<ValueDifference<CockData>> temp = ChangedCocks(oldCockData, true).Where(x => x.oldValue.length != x.newValue.length);
			int removedCockCount = 0;
			ValueDifference<CockData>[] changed;
			if (treatMissingCocksAsRemoved)
			{
				CockData[] removed = RemovedCocks(oldCockData).ToArray();
				changed = temp.Concat(removed.Select(x => new ValueDifference<CockData>(x, null))).ToArray();

				if (displayRemovedText)
				{
					removedCockCount = removed.Length;
				}
			}
			else
			{
				changed = temp.ToArray();
			}

			if (changed.Length == 0)
			{
				return "";
			}
			if (changed.Length == 1)
			{
				return GenericChangeOneCockLengthText(changed[0].oldValue, treatMissingCocksAsRemoved, displayRemovedText);
			}

			double[] deltas = new double[changed.Length];

			for (int x = 0; x < changed.Length; x++)
			{
				if (changed[x].newValue is null)
				{
					deltas[x] = -changed[x].oldValue.length;
				}
				else
				{
					deltas[x] = changed[x].newValue.length - changed[x].oldValue.length;
				}
			}

			StringBuilder sb = new StringBuilder();

			//int grewCount = deltas.Length;
			int grewCount = deltas.Count(x => x > 0);
			int shrunkCount = deltas.Count(x => x < 0);

			//describe cocks getting shorter
			if (shrunkCount > 0)
			{
				double largestNegativeDelta = deltas.Min();
				double smallestNegativeDelta = deltas.Where(x => x < 0).Max();

				if (largestNegativeDelta >= -1)
				{
					if (shrunkCount < deltas.Length)
					{
						if (grewCount > 0 && shrunkCount + grewCount == deltas.Length)
						{
							sb.Append("Meanwhile, your remaining " + (shrunkCount > 1 ? "cocks have " : "cock has") + " shrunk to a slightly shorter length.");
						}
						else
						{
							string intro = grewCount > 0 ? "Meanwhile, you" : "You";

							sb.Append(intro + " feel " + Utils.NumberAsText(shrunkCount) + " of your " + AllCocksShortDescription()
								+ " shrink to a slightly shorter length.");
						}
					}
					else //if (shrinkCount == cocks.Count)
					{
						sb.Append("Your " + AllCocksShortDescription() + " have shrunk to a slightly shorter length.");
					}

				}
				else if (largestNegativeDelta >= -3)
				{
					bool varyingLengths = smallestNegativeDelta >= -1;

					if (shrunkCount < deltas.Length)
					{
						if (grewCount > 0 && shrunkCount + grewCount == deltas.Length)
						{
							sb.Append("Meanwhile, your remaining" + (shrunkCount > 1 ? "cocks begin " : "cock begins") + " to shrink,");
						}
						else
						{
							string intro = grewCount > 0 ? "Meanwhile, you" : "You";

							sb.Append(intro + " feel " + Utils.NumberAsText(shrunkCount) + " of your " + AllCocksShortDescription() + " shrink smaller,");
						}

						sb.Append("the flesh vanishing into your groin" + (varyingLengths ? " in varying lengths." : "."));
					}
					else //shrinkCount == deltas.Length
					{
						sb.Append("Your " + AllCocksShortDescription() + " shrink smaller, the flesh vanishing into your groin"
							+ (varyingLengths ? " in varying lengths." : "."));
					}
				}
				else //if (deltaAmount <= -3)
				{
					bool varyingLengths = smallestNegativeDelta >= -3;
					if (shrunkCount < deltas.Length)
					{
						if (grewCount > 0 && shrunkCount + grewCount == deltas.Length)
						{
							sb.Append("Meanwhile, your remaining " + (shrunkCount > 1 ? "cocks vanish " : "cock vanishes") + " into your groin instead" +
								(varyingLengths ? ", receeding in length significantly, with some moreso than others." : ", receding rapidly in length."));
						}
						else if (shrunkCount == 1)
						{
							string intro = grewCount > 0 ? "Meanwhile, another of your " : "A single member of your";

							sb.Append(intro + AllCocksShortDescription() + " vanishes into your groin" + (grewCount > 0 ? " instead" : "") +
								", receding rapidly in length.");
						}
						else //if (shrunkCount > 1)
						{
							string intro = grewCount > 0
								? "Meanwhile, several other of your members begin to"
								: "Your " + AllCocksShortDescription() + " tingles as " + Utils.NumberAsText(shrunkCount) + " of your members";
							string outro = varyingLengths
								? "receeding in length, some moreso than others"
								: "receding rapidly in length.";

							sb.Append(" vanish into your groin,");
						}
					}
					else //(shrunkCount == cocks.Count)
					{
						sb.Append("A large portion of your " + AllCocksShortDescription() + " recedes towards your groin, receding rapidly in length.");
					}
				}
			}
			//describe any removed cocks.
			if (removedCockCount > 0)
			{
				string shrinkPronoun = shrunkCount > 1 ? "they" : "it";

				string removeCountText = removedCockCount > 1 ? "several of them are" : "one of them is";
				string removePronoun = removedCockCount > 1 ? "them " : "it";
				string removePronoun2 = removedCockCount > 1 ? "they " : "it";
				int remainingCocks = deltas.Length - removedCockCount;
				string remainingCockText;

				if (remainingCocks == 0)
				{
					remainingCockText = "without any cocks.";
				}
				else if (remainingCocks == 1)
				{
					remainingCockText = "with just one cock.";
				}
				else
				{
					remainingCockText = "with just " + Utils.NumberAsText(remainingCocks) + " cocks.";
				}

				sb.Append("As " + shrinkPronoun + " shrink, you notice " + removeCountText + " are becoming incredibly small. As if on cue, <b>you watch " +
					removePronoun + "continue to shrink until " + removePronoun2 + " disappear into your groin, leaving you " + remainingCockText + "</b>");
			}


			//describe cocks getting longer.
			if (grewCount > 0)
			{
				double largestPositiveDelta = deltas.Max();
				double smallestPositiveDelta = deltas.Where(x => x > 0).Min();

				bool differentLengths = largestPositiveDelta != smallestPositiveDelta;

				bool capitalize = true;

				if (shrunkCount > 0)
				{
					sb.Append("Meanwhile, ");
					capitalize = false;
				}

				if (largestPositiveDelta < 1)
				{
					string GetIntro(string standardIntro)
					{
						string text;
						if (shrunkCount > 0 && removedCockCount > 0)
						{
							text = $"{standardIntro} remaining ";
						}
						else if (shrunkCount > 0)
						{
							text = $"{standardIntro} other ";
						}
						else
						{
							text = $"{standardIntro} ".CapitalizeFirstLetter();
						}

						return text;
					}

					string intro;
					string outro = ".";

					if (shrunkCount > 0 && removedCockCount > 0)
					{
						outro = ", as if compensating for your lost cocks.";
					}
					else
					{
						outro = ", countering your previous losses.";
					}

					if (grewCount == 1 && grewCount + shrunkCount == changed.Length)
					{
						intro = capitalize ? "Your" : "your";
						intro += removedCockCount > 0 ? " remaining" : " other";

						sb.Append(intro + " cock grows slightly longer" + outro);
					}
					else if (grewCount == 1)
					{
						intro = GetIntro("one of your");
						sb.Append(intro + AllCocksShortDescription() + " grows slightly longer" + outro);
					}
					else if (grewCount < cocks.Count - shrunkCount)
					{
						intro = GetIntro("some of your");
						sb.Append(intro + AllCocksShortDescription() + " grow slightly longer" + outro);
					}
					else //if (grewCount == cocks.Count)
					{
						intro = GetIntro("your");
						sb.Append(intro + AllCocksShortDescription() + " seem to fill up... growing a little bit larger" + outro);
					}
				}
				else if (largestPositiveDelta < 3)
				{
					string describeCocks;

					string shrinkText = "";
					if (removedCockCount > 0)
					{
						shrinkText = "remaining ";
					}
					else if (shrunkCount > 0)
					{
						shrinkText = "other";
					}

					if (grewCount == 1 && grewCount + shrunkCount == changed.Length)
					{
						describeCocks = "your " + (removedCockCount > 0 ? "remaining " : "final ") + "cock grows";
					}
					else if (grewCount == 1)
					{
						describeCocks = "one of your " + shrinkText + AllCocksShortDescription() + " grows";
					}
					else if (grewCount < cocks.Count - shrunkCount)
					{
						describeCocks = Utils.NumberAsText(grewCount) + " of your " + shrinkText + AllCocksShortDescription() + " grow";
					}
					else
					{
						describeCocks = "your " + shrinkText + AllCocksShortDescription() + " each grow";
					}

					string intro = capitalize ? "A" : "a";

					sb.Append(intro + " very pleasurable feeling spreads from your groin as " + describeCocks + " permanently longer, ");

					if (smallestPositiveDelta < 1)
					{
						sb.Append(" by varying lengths. The largest change is at least " + (Measurement.UsesMetric ? "a few centimeters" : "an inch") +
							"and the pleasure has you leaking plenty of pre-cum from all of your recently grown cocks");
					}
					else
					{
						sb.Append("by at least " + (Measurement.UsesMetric ? "a few centimeters" : "an inch") + ", " + (grewCount > 1 ? "each leaking" : "and leaks") +
							" plenty of pre-cum from the pleasure of the change.");
					}
				}
				else //if (largestPositiveDelta >= 3)
				{
					sb.Append(capitalize ? "Your " : "your ");

					if (grewCount == 1 && grewCount + shrunkCount == changed.Length)
					{
						sb.Append("remainings cock feels incredibly tight as it begins to grow, countering your previously lost dick-flesh by growing " + (Measurement.UsesMetric ? "several centimeters" : "several inches") + ".");
					}
					else if (grewCount == 1)
					{
						sb.Append(AllCocksShortDescription() + " feel incredibly tight as one of their number begins to grow, "
							+ (Measurement.UsesMetric ? "centimeter after centimeter" : "inch after inch") + ".");
					}
					else if (grewCount < cocks.Count)
					{
						sb.Append(AllCocksShortDescription() + " feel incredibly numb as " + Utils.NumberAsText(grewCount) + " of them begin to grow");

						if (smallestPositiveDelta < 3)
						{
							sb.Append(", albeit at different rates. The largest continues to grow, not stopping until it has put on " +
								(Measurement.UsesMetric ? "centimeter after centimeter" : "inch after inch") + " of added length.");
						}
						else
						{
							sb.Append((Measurement.UsesMetric ? "centimeter after centimeter" : "inch after inch") + " of added length pouring from your groin.");
						}
					}
					else //if (grewCount == cocks.Count)
					{
						sb.Append(AllCocksShortDescription() + " feel incredibly tight as they grow, ");

						if (smallestPositiveDelta < 3)
						{
							sb.Append("albeit at different rates. The largest continues to grow, not stopping until it has put on " +
								(Measurement.UsesMetric ? "centimeter after centimeter" : "inch after inch") + " of added length.");
						}
						else
						{
							sb.Append((Measurement.UsesMetric ? "centimeter after centimeter" : "inch after inch") + " of added length pouring from your groin.");
						}
					}
				}

				//Display LengthChange

				//if we did some shrinking or removing, we only count a threshold if we weren't previously there.
				//if we were previously there, we're just going to exit right away.

				CockData largestNotGrown = UnchangedCocks(oldCockData, true).Select(x => x.AsReadOnlyData()).Union(changed.Where(x =>
					!(x.newValue is null) && x.newValue.length < x.oldValue.length).Select(x => x.newValue)).MaxItem(x => x.length);

				var tempPair = changed.Where(x => !(x.newValue is null) && x.newValue.length > x.oldValue.length).MaxItem(x => x.newValue.length);

				var largestGrown = tempPair.newValue;
				var deltaAmount = tempPair.newValue.length - tempPair.oldValue.length;


				if (shrunkCount > 0 && (largestNotGrown.length >= largestGrown.length || largestNotGrown.length >= 20 || largestGrown.length < 8
					|| (largestNotGrown.length >= 16 && largestGrown.length < 20) || (largestNotGrown.length >= 12 && largestGrown.length < 16)
					|| (largestNotGrown.length >= 8 && largestGrown.length < 12)))
				{
					return sb.ToString();
				}



				//this now handles multiple cocks better - if we already had a cock at the current threshold, we don't mention what having a cock of that length entails.
				//we may, however, mention we have another cock at that threshold.

				//start with the largest and work our way down.
				if (largestGrown.length >= 20 && largestGrown.length - deltaAmount < 20)
				{
					bool describeAll = cocks.All(x => x.length >= 20);
					bool describeSeveral = cocks.Count(x => x.length >= 20) > 1;

					//if we didn't have any cocks above 20 before this, mention how it/they now obscure your lower vision.
					if (largestNotGrown is null || largestNotGrown.length < 20)
					{
						sb.Append("<b>As if the pulsing heat ");


						if (cocks.Count == 1)
						{
							sb.Append("of your " + largestGrown.LongDescription());
						}
						else
						{
							sb.Append("of your " + AllCocksShortDescription());
						}
						sb.Append(" wasn't bad enough, ");

						if (describeAll || describeSeveral)
						{
							sb.Append("every time you get hard, the tips of ");

							if (describeSeveral)
							{
								sb.Append("several of ");
							}
							sb.Append("your " + AllCocksShortDescription() + "wave before you, obscuring the lower portions of your vision");
						}
						else
						{
							sb.Append("your " + largestGrown.ShortDescription() + "'s " + largestGrown.HeadDescription() + " keeps poking its way into your view " +
								"every time you get hard.");
						}

						sb.Append("</b>");

						//then, describe what this entails.
						if (creature?.corruption > 80)
						{
							sb.Append(" You find yourself fantasizing about impaling nubile young champions on your " + AllCocksLongDescription() + " in a year's time.");
						}
						else if (creature?.corruption > 60)
						{
							sb.Append(" You daydream about being attacked by a massive tentacle beast, its tentacles engulfing your " + AllCocksLongDescription() + " to the hilt, milking it of all your cum.\n\nYou smile at the pleasant thought.");
						}
						else if (creature?.corruption > 40)
						{
							if (cocks.Count == 1)
							{
								sb.Append(" You wonder if there is a demon or beast out there that could handle your full length.");
							}
							else
							{
								sb.Append(" You wonder - is a demon or beast out there that could take the full length of the largest of your " + AllCocksShortDescription() + "?");
							}
						}
					}
					//otherwise, just mention that you now have more that can obscure your vision
					else
					{
						if (describeAll || describeSeveral)
						{
							sb.Append((describeAll ? "They've each" : "Several have") + " grown long enough that you now have even more cocks starting to obscure your " +
								"vision whenever you have an erection.");
						}
						else
						{
							sb.Append("It's grown so long that you now have yet another cock that partially obscures your vision when erect.");
						}
					}
				}
				else if (largestGrown.length >= 16 && largestGrown.length - deltaAmount < 16)
				{

					bool describeAll = cocks.All(x => x.length >= 16);

					sb.Append(" <b>");
					if (describeAll)
					{
						sb.Append("Each one of your " + AllCocksShortDescription() + "now looks like it'd be more at home " +
							"on a large horse, let alone together on one body");
					}
					else if (grewCount == 1)
					{
						sb.Append("Your " + largestGrown.LongDescription() + " would look more at home on a large horse than you");
					}
					else
					{
						sb.Append("The largest of them now looks like it'd be more at home on a large horse than you");
					}

					//if multiple cocks, and all the cocks that grew longer are still shorter than the longest, make note of that.
					if (!describeAll && !(largestNotGrown is null) && largestGrown.length < largestNotGrown.length)
					{
						sb.Append(", though it's still not as long as your " + largestNotGrown.LongDescription());
					}
					sb.Append(".</b>");

					bool onlyOneLarger = grewCount == 1 || changed.All(x => x != tempPair && (x.newValue is null || x.newValue.length < 16));

					//if it is the longest, that means we've just reached this threshold. mention that we can now tit-fuck ourselves.
					if (largestNotGrown is null || largestNotGrown.length < 16)
					{
						if (source.BiggestCupSize() >= CupSize.C)
						{
							//you only have one cock that grew larger or all the other ones are still below the threshold.
							if (onlyOneLarger)
							{
								sb.Append(" You could easily stuff your " + largestGrown.LongDescription() + " between your breasts and give yourself the titty-fuck of a lifetime.");
							}
							//some (but not all) are now this large.
							else if (!describeAll)
							{
								sb.Append(" Several of your " + AllCocksShortDescription() + " now reach so far up your chest it would be easy to stuff a few of them " +
									"between your breasts and give yourself the titty-fuck of a lifetime.");
							}
							else //if (cocks.Count > 1)
							{
								sb.Append(" They reach so far up your chest it would be easy to stuff a few cocks between your breasts and give yourself the titty-fuck of a lifetime.");
							}
						}
						else
						{
							if (onlyOneLarger)
							{
								sb.Append(" Your " + largestGrown.LongDescription() + " is so long it easily reaches your chest. " +
									"The possibility of autofellatio is now a foregone conclusion.");
							}
							else if (!describeAll)
							{
								sb.Append(" Several of your " + AllCocksShortDescription() + "are now long enough to easily reach your chest. " +
									"Autofellatio would be about as hard as looking down.");
							}
							else
							{
								sb.Append(" They are so long that they easily reach your chest; you'd be able to perform autofellatio on any of them with little effort.");
							}
						}
					}
				}
				else if (largestGrown.length >= 12 && largestGrown.length - deltaAmount < 12)
				{
					bool describeAll = cocks.All((x, y) => x.length >= 12);
					sb.Append(" <b>");
					if (describeAll)
					{
						sb.Append("They are all so long now that they nearly reach your knees when at full length");
					}
					else if (grewCount > 1)
					{
						sb.Append("The largest of them is now so long, it nearly reaches your knees");
					}
					else
					{
						sb.Append("Your " + cocks[0].LongDescription() + " is so long it nearly swings to your knee at its full length");
					}

					//if multiple cocks, and all the cocks that grew longer are still shorter than the longest, make note of that.
					if (!describeAll && !(largestNotGrown is null) && largestGrown.length < largestNotGrown.length)
					{
						sb.Append(", though it's still not as long as your " + largestNotGrown.LongDescription());
					}

					sb.Append(".</b>");
				}
				else if (largestGrown.length >= 8 && largestGrown.length - deltaAmount < 8)
				{
					bool describeAll = cocks.All(x => x.length >= 8);
					bool describeSeveral = cocks.Count(x => x.length >= 8) > 1;
					sb.Append("<b>");

					if (describeAll)
					{
						sb.Append("Most men would be overly proud to have a cock as long as your " + ShortestCock().LongDescription() +
							", and that's the shortest one you have!");
					}
					else if (describeSeveral)
					{
						sb.Append("Several have now reached lengths most men would be proud to match");
					}
					else if (cocks.Count != 1)
					{
						sb.Append("The largest is now long enough most men would be proud to match its length");
					}
					else //if (cocks.Count == 1)
					{
						sb.Append(" Most men would be overly proud to have a tool as long as yours");
					}

					//if multiple cocks, and all the cocks that grew longer are still shorter than the longest, make note of that.
					if (!describeAll && !(largestNotGrown is null) && largestGrown.length < largestNotGrown.length)
					{
						sb.Append(", and it's still not as long as your " + largestNotGrown.LongDescription() + "!");
					}
					else
					{
						sb.Append(".");
					}

					sb.Append("</b>");
				}
			}

			return sb.ToString();
		}

		private string ReductoCocks()
		{
			StringBuilder sb = new StringBuilder();
			if (cocks[0].type == CockType.BEE)
			{
				sb.Append("The gel produces an odd effect when you rub it into your " + cocks[0].LongDescription() +
					". It actually seems to calm the need that usually fills you. In fact, as your " + cocks[0].LongDescription() +
					" shrinks, its skin tone changes to be more in line with yours and the bee hair that covered it falls out. <b>You now have a human cock!</b>");
				cocks[0].Restore();
			}
			else
			{
				sb.Append("You smear the repulsive smelling paste over your " + AllCocksShortDescription() + ". It immediately begins to grow warm, " +
					"almost uncomfortably so, as your " + AllCocksShortDescription() + " begins to shrink." + GlobalStrings.NewParagraph());
				if (cocks.Count == 1)
				{
					sb.Append("Your " + cocks[0].LongDescription() + " twitches as it shrinks, disappearing steadily into your " + (hasSheath ? "sheath" : "crotch")
						+ " until it has lost about a third of its old size.");
				}
				else
				{ //MULTI
					sb.Append("Your " + AllCocksShortDescription() + " twitch and shrink, each member steadily disappearing into your "
						+ (hasSheath ? "sheath" : "crotch") + " until they've lost about a third of their old size.");
				}
			}
			return sb.ToString();
		}

		private string GroPlusCocks()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("You sink the needle into the base of your " + AllCocksShortDescription() + ". It hurts like hell, but as you depress the plunger, " +
				"the pain vanishes, replaced by a tingling pleasure as the chemicals take effect." + GlobalStrings.NewParagraph());
			if (cocks.Count == 1)
			{
				sb.Append("Your " + cocks[0].LongDescription() + " twitches and thickens, pouring " + (Measurement.UsesMetric ? "several centimeters" : "more than an inch") +
					" of thick new length from your ");
			}
			//MULTI
			else
			{
				sb.Append("Your " + AllCocksShortDescription() + " twitch and thicken, each member pouring out "
					+ (Measurement.UsesMetric ? "several centimeters" : "more than an inch") + " of new length from your ");
			}
			if (hasSheath)
			{
				sb.Append("sheath.");
			}
			else
			{
				sb.Append("crotch.");
			}

			return sb.ToString();
		}
	}
}
