using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Strings;
using CoC.Backend.Tools;

namespace CoC.Backend.BodyParts
{
	//for now, this is all i need to describe all of the breasts and related text
	internal interface IBreastCollection<T> where T : IBreast
	{
		ReadOnlyCollection<T> breasts { get; }
		BreastData AverageBreasts();
	}

	partial class BreastCollection : IBreastCollection<Breasts>
	{
		ReadOnlyCollection<Breasts> IBreastCollection<Breasts>.breasts => breastRows;

		public static string Name()
		{
			return "All Breasts";
		}

		private string LactationSlowedDownDueToInactivity(bool becameOverFullThisPass, LactationStatus oldLevel)
		{
			int delta = oldLevel - lactationStatus;
			if (lactationStatus > LactationStatus.STRONG)
			{
				string hitLongDescing = becameOverFullThisPass ? ", though they're still very much in need of a milking" : "";
				return "\n<b>Your breasts feel somewhat lighter, though not much. It seems you aren't producing milk at such an ungodly rate anymore" + hitLongDescing + ".</b>\n";
			}
			else if (lactationStatus > LactationStatus.MODERATE)
			{
				string helperStr = delta > 1 ? "significantly " : "";
				string hitLongDescing = becameOverFullThisPass ? "Despite this, they're still very tender and probably should be milked soon." : "";

				return "\n<b>Your breasts feel " + helperStr + "lighter as your body's milk production starts to wind down." + hitLongDescing + "</b>\n";
			}
			else if (lactationStatus > LactationStatus.LIGHT)
			{
				string helperStr = delta > 1 ? "significantly " : "";
				string hitLongDescing = becameOverFullThisPass ? "Despite this, they're still very tender and probably should be milked soon." : "";

				return "\n<b>Your breasts feel " + helperStr + "lighter as your body's milk production winds down." + hitLongDescing + "</b>\n";
			}
			else if (lactationStatus > LactationStatus.NOT_LACTATING)
			{
				string helperStr = delta > 1 ? "significantly, " : "";
				string hitLongDescing = becameOverFullThisPass ? "Despite this, they're still tender and probably should be milked soon." : "";
				return "\n<b>Your body's milk output drops " + helperStr + "down to what would be considered 'normal' for a pregnant woman." + hitLongDescing + "</b>\n";
			}
			else
			{
				return "\n<b>Your body no longer produces any milk.</b>\n";
			}
		}
		private string LactationFullWarning()
		{
			return Environment.NewLine + "<b>Your " + breastRows[0].ShortNippleDescription() + "s feel swollen and bloated, needing to be milked.</b>" + Environment.NewLine;
		}

		private string AllBreastsPlayerText(PlayerBase player)
		{
			StringBuilder sb = new StringBuilder();

			if (breastRows.Count == 1)
			{
				sb.Append("You have " + Utils.NumberAsText(breastRows[0].numBreasts) + " " + breastRows[0].LongDescription() + ", each supporting ");
				sb.Append(hasQuadNipples ? "four" : "a"); //Number of nipples.
				sb.Append(" " + breastRows[0].ShortNippleDescription(hasQuadNipples, false));

				if (currentLactationAmount / currentLactationCapacity > 0.75)
				{
					sb.Append(" Your " + breastRows[0].LongDescription() + " are painful and sensitive from being so stuffed with milk.You should release the pressure soon.");
				}
				if (breastRows[0].cupSize >= CupSize.A)
				{
					sb.Append(" You could easily fill a " + breastRows[0].cupSize.AsText() + " bra.");
				}
				//Done with tits. Move on.
				sb.Append(Environment.NewLine);
			}
			//many rows
			else
			{
				sb.Append("You have " + Utils.NumberAsText(breastRows.Count) + " rows of breasts, the topmost pair starting at your chest.\n");
				for (int temp = 0; temp < breastRows.Count; temp++)
				{
					if (temp == 0)
					{
						sb.Append("--Your uppermost rack houses ");
					}
					else if (temp == 1)
					{
						sb.Append("\n--The second row holds ");
					}
					else if (temp == 2)
					{
						sb.Append("\n--Your third row of breasts contains ");
					}
					else //if (temp == 3)
					{
						sb.Append("\n--Your fourth and final set of tits cradles ");
					}

					sb.Append(Utils.NumberAsText(breastRows[temp].numBreasts) + " " + breastRows[temp].LongDescription() + " with ");
					sb.Append(hasQuadNipples ? "four" : "a"); //Number of nipples per breast
					sb.Append(" " + breastRows[0].ShortNippleDescription(hasQuadNipples, false));  // Length of nipples
					sb.Append(" each."); //Description and Plural

					if (breastRows[temp].cupSize >= CupSize.A)
					{
						sb.Append(" They could easily fill a " + breastRows[temp].cupSize.AsText() + " bra.");
					}
				}

				if (currentLactationAmount / currentLactationCapacity > 0.75)
				{
					sb.Append(" Your multiple rows of bountiful breasts are painful and sensitive from being so stuffed with milk. You should release the pressure soon.");
				}


			}

			return sb.ToString();
		}

		public string RemovedExtraBreastRowGenericText(BreastData removedBreastRow)
		{
			Creature target = CreatureStore.GetCreatureClean(creatureID);
			string skinText;
			bool plural;
			if (target is null)
			{
				skinText = new BodyData(Guid.Empty).ShortEpidermisDescription(out plural);
			}
			else
			{
				skinText = target.body.ShortEpidermisDescription(out plural);
			}

			return $"You stumble back when your center of balance shifts, and though you adjust before you can fall over, you're left to watch in awe as your bottom-most " +
				$"{removedBreastRow.ShortDescription()} shrink down, disappearing completely into your {(breastRows.Count > 2 ? "abdomen" : "chest")}. The " +
				$"{removedBreastRow.ShortNippleDescription()} even fade until nothing but {skinText} {(plural ? "remain" : "remains")}. " +
				SafelyFormattedString.FormattedText("You've lost a row of breasts!", StringFormats.BOLD);
		}

		public string GenericRemovedBlackNipples()
		{
			Creature creature = CreatureStore.GetCreatureClean(creatureID);

			string armorText = creature?.wearingArmor == true || creature?.wearingUpperGarment == true ? "Undoing your clothes" : "Looking down";

			return GlobalStrings.NewParagraph() + "Something invisible brushes against your " + CommonShortNippleDescription()
				+ ", making you twitch. " + armorText + ", you take a look at your"
				+ " chest and find that your nipples have turned back to their natural flesh color.";
		}

		public string GenericChangeOneRowCupSizeText(BreastData oldData, bool treatMissingAsRemoved = true, bool displayRemovedText = true)
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

			Breasts newValue = breastRows.FirstOrDefault(x => x.creatureID == oldData.creatureID && x.collectionID == oldData.collectionID);
			if (newValue is null && !treatMissingAsRemoved)
			{
				return "";
			}

			double deltaAmount = !(newValue is null) ? newValue.cupSize - oldData.cupSize : -(byte)oldData.cupSize;

			if (deltaAmount == 0)
			{
				return "";
			}
			else if (deltaAmount >= 4)
			{
				string row = "";
				if (breastRows.Count > 1 && oldData.currBreastRowIndex == 0)
				{
					row = "top-most row of";

				}
				else if (breastRows.Count > 1)
				{
					row = Utils.NumberAsPlace(oldData.currBreastRowIndex + 1) + "row of";
				}

				sb.Append("You drop to your knees from a massive change in your body's center of gravity. Your " + row + oldData.LongDescription() + " tingle strongly, " +
					"growing disturbingly large.");
			}
			else if (deltaAmount > 2)
			{
				string row = "";

				if (breastRows.Count > 1 && oldData.currBreastRowIndex == 0)
				{
					row = "top-most row of";

				}
				else if (breastRows.Count > 1)
				{
					row = Utils.NumberAsPlace(oldData.currBreastRowIndex + 1) + "row of";
				}

				sb.Append("You stagger as your chest gets much heavier. Looking down, you watch with curiosity as your " + row + oldData.LongDescription() + " expand significantly.");
			}
			else if (deltaAmount > 0)
			{
				string intro;
				if (breastRows.Count == 1)
				{
					intro = "Your " + oldData.LongDescription();
				}
				else //if (breastRows.Count > 1)
				{

					string row = oldData.currBreastRowIndex == 0 ? "top-most" : Utils.NumberAsPlace(oldData.currBreastRowIndex + 1);

					intro = "Your " + row + " of " + oldData.LongDescription();

					sb.Append("One of your " + AllBreastsShortDescription() + " grows slightly longer.");
				}

				sb.Append(intro + " jiggles with added weight as it expands, growing a bit larger.");
			}
			else if (!(newValue is null))
			{

			}
			//Display the degree of length loss.
			else if (deltaAmount >= -1)
			{
				string row = "";
				if ((newValue is null || breastRows.Count > 1) && oldData.currBreastRowIndex == 0)
				{
					row = "top-most row of ";

				}
				else if ((newValue is null || breastRows.Count > 1))
				{
					row = Utils.NumberAsPlace(oldData.currBreastRowIndex + 1) + "row of ";
				}

				if (newValue is null)
				{
					sb.Append("You feel a weight lifted from you, and realize your " + row + "breasts have shrunk down until they're flat against your chest. ");

					if (displayRemovedText)
					{
						sb.Append("They soon vanish from your chest completely.");

						if (breastRows.Count > oldData.currBreastRowIndex)
						{
							sb.Append("Your remaining " + (breastRows.Count > 1 ? "rows" : "row") + " shifts, replacing the empty space.");
						}
					}
				}
				else
				{
					sb.Append("You feel a weight lifted from you, and realize your " + row + "breasts have shrunk! With a quick measure, you determine they're now "
						+ newValue.cupSize.AsText() + "s.");
				}
			}
			else //if (deltaAmount < -1)
			{
				string row = "";
				if (breastRows.Count > 1 && oldData.currBreastRowIndex == 0)
				{
					row = "top-most row of ";

				}
				else if (breastRows.Count > 1)
				{
					row = Utils.NumberAsPlace(oldData.currBreastRowIndex + 1) + "row of ";
				}
				if (newValue is null)
				{
					sb.Append("You feel significantly lighter. Looking down, you realize your " + row + "breasts have shrunk down until they're flat against your chest. ");

					if (displayRemovedText)
					{
						sb.Append("They soon vanish from your chest completely.");

						if (breastRows.Count > oldData.currBreastRowIndex)
						{
							sb.Append("Your remaining " + (breastRows.Count > 1 ? "rows" : "row") + " shifts, replacing the empty space.");
						}
					}
				}
				else
				{
					sb.Append("You feel significantly lighter. Looking down, you realize your " + row + "breasts are much smaller! With a quick measure, " +
						"you determine they're now " + newValue.cupSize.AsText() + "s.");
				}
			}

			if (!(newValue is null) && oldData.nippleLength < newValue.nippleLength)
			{
				sb.Append(" A tender ache starts at your " + CommonLongNippleDescription() + " as they grow to match your burgeoning breast-flesh.");
			}

			return sb.ToString();
		}

		//unlike cocks, which are more or less unordered, this will go row-by-row. Therefore, this will also handle strange cases where you've reordered breast rows
		//though it's not exactly designed with that in mind. to be frank, the game isn't designed with that in mind, but if you pull it off, hey, we handle it.
		//It's still not really designed with add and remove in mind, but...
		//we will try to replace any removed rows with added rows. if the old collection has extra values, they will be treated as removed.
		//if the old collection is missing values, they will be treated as added. By default, we will handle the removed rows, though you can tell us to
		//conveniently ignore them. We conveniently ignore added rows because it's a pain in the ass to fix.

		//Note that if the old data and new data have somehow been reordered we'll describe this. That said, we cannot pull that off with replaced rows -
		//replace rows will be done in order, even if there's potentially a better size match configuration. we're not going out of our way to make your bad data work
		//pretty-like - next time, give us better data.
		public string GenericChangeCupSizeText(BreastCollectionData oldBreastRows, bool treatMissingRowsAsRemoved = true)
		{
			if (oldBreastRows == null)
			{
				throw new ArgumentNullException(nameof(oldBreastRows));
			}

			if (!CollectionChanged(oldBreastRows, true))
			{
				return "";
			}

			//All of this bullshit is to better handle missing rows, reordered rows, etc. If you know none of this will occur, the end result will be
			//the same as pairing breastRows[x] with oldBreastData.breasts[x] and comparing them directly. This just makes sure that's actually a valid
			//assumption, and corrects accordingly.
			Pair<Breasts, BreastData>[] comparisons = new Pair<Breasts, BreastData>[oldBreastRows.breasts.Count];

			LinkedList<Breasts> newValues = new LinkedList<Breasts>(breastRows);

			for (int x = 0; x < oldBreastRows.breasts.Count; x++)
			{
				var first = breastRows.FirstOrDefault(y => y.collectionID == oldBreastRows.breasts[x].collectionID);
				if (first != null)
				{
					comparisons[x] = new Pair<Breasts, BreastData>(first, oldBreastRows.breasts[x]);
					newValues.Remove(first);
				}
			}

			for (int x = 0; x < comparisons.Length; x++)
			{
				if (comparisons[x] != null)
				{
					continue;
				}

				Breasts breast = null;
				if (newValues.First != null)
				{
					breast = newValues.First.Value;
				}

				comparisons[x] = new Pair<Breasts, BreastData>(breast, oldBreastRows.breasts[x]);
			}

			var minVal = comparisons[0].first.rowIndex;
			bool outOfOrder = false;
			foreach (var item in comparisons.Skip(1))
			{
				if (item.first is null)
				{
					continue;
				}
				else if (minVal >= item.first.rowIndex)
				{
					outOfOrder = true;
					break;
				}
				else
				{
					minVal = item.first.rowIndex;
				}
			}

			var changeCount = 0;
			if (treatMissingRowsAsRemoved)
			{
				changeCount = comparisons.Count(x => x.first is null || x.first.cupSize != x.second.cupSize);
			}
			else
			{
				changeCount = comparisons.Count(x => !(x.first is null) && x.first.cupSize != x.second.cupSize);
			}

			if (changeCount == 0)
			{
				if (outOfOrder)
				{
					return "You feel the weight of your breasts shifting though the strain on your back suggests the weight hasn't really changed. " +
						"Once you're able to get a good look at your breasts, you notice the rows have rearrainged themselves. Odd, but this is Mareth, you" +
						"note, and stranger things have happened.";
				}
				else
				{
					return "";
				}

			}
			if (changeCount == 1)
			{
				BreastData oldChanged;
				if (treatMissingRowsAsRemoved)
				{
					oldChanged = comparisons.First(x => x.first is null || x.first.cupSize != x.second.cupSize).second;
				}
				else
				{
					oldChanged = comparisons.First(x => !(x.first is null) && x.first.cupSize != x.second.cupSize).second;
				}

				string orderText = "";
				if (outOfOrder)
				{
					orderText = "You feel the weight of your breasts shifting though the strain on your back doesn't suggest any immediate changes." +
						"Once you're able to get a good look at your breasts, you notice the rows have rearrainged themselves. Odd, " +
						"but this is Mareth, you note, and stranger things have happened. With that out of the way, however, more changes start to manifest. ";
				}
				return orderText + GenericChangeOneRowCupSizeText(oldChanged, treatMissingRowsAsRemoved, true);
			}

			StringBuilder sb = new StringBuilder();

			bool someGrew = comparisons.Any(x => x.second.cupSize < x.first.cupSize);
			bool someShrank = comparisons.Any(x => x.second.cupSize > x.first.cupSize || (x.first is null && treatMissingRowsAsRemoved));

			int netChange = comparisons.Sum(x=> x.second is null ? (treatMissingRowsAsRemoved ? -(byte)x.first.cupSize : 0) : (byte)x.second.cupSize - (byte)x.first.cupSize);

			if (someGrew && someShrank)
			{
				sb.Append("Your feel the weight of your multiple rows of breasts shifting, as if some are getting larger and other smaller, ");

				if (netChange > 0)
				{
					sb.Append("though the extra strain on your back suggests they've ultimately becoming heavier overall. ");
				}
				else if (netChange < 0)
				{
					sb.Append("though thankfully they seem to have gotten lighter as a result. ");
				}
				else
				{
					sb.Append("though the overall weight doesn't seem to have changed. ");
				}

				if (outOfOrder)
				{
					sb.Append("Additionally, they seem to have reordered themselves somehow. You'd scarcely believe it, but stranger things have happened. " +
						"After all the changes complete, you take stock of your newly adjusted breasts:" + Environment.NewLine);
				}
			}
			else if (outOfOrder)
			{
				sb.Append("You reordered themselves somehow. You'd scarcely believe it, but stranger things have happened. " +
						"After all the changes complete, you take stock of your newly adjusted breasts:" + Environment.NewLine);
			}
			else
			{
				sb.Append(Environment.NewLine);
			}

			var sortedNew = comparisons.OrderBy(x => x.first is null ? x.second.currBreastRowIndex * 1.0 : x.first.rowIndex + 0.1);

			int iteration = 0;
			foreach (var item in sortedNew)
			{
				var newData = item.first;
				var oldData = item.second;
				if (newData is null && !treatMissingRowsAsRemoved)
				{
					iteration++;
					continue;
				}

				var delta = (byte)(newData?.cupSize ?? CupSize.FLAT) - (byte)oldData.cupSize;
				if (iteration == 0)
				{
					sb.Append("Your uppermost row of " + oldData.LongDescription());

				}
				else
				{
					sb.Append("...your " + Utils.NumberAsPlace(iteration + 1) + "row of " + oldData.LongDescription());
				}

				if (newData.rowIndex != oldData.currBreastRowIndex)
				{
					sb.Append(" (which was previously your " + Utils.NumberAsPlace(oldData.currBreastRowIndex + 1) + ")");
				}

				if (delta < 0)
				{
					if (newData is null)
					{
						sb.Append(" shrink down until they are are flats, then disappear into your body. Your remaining breasts shift about, taking advantage of the now" +
							"vacated space.");
					}
					sb.Append(" shrink, dropping to " + newData.cupSize.AsText() + "s.");
				}
				else if (delta == 0)
				{
					sb.Append(" remain unchanged.");
				}
				else
				{
					if (delta < 2)
					{
						sb.Append(" grow a bit larger, reaching "+ newData.cupSize.AsText() + "s.");
					}
					else if (delta < 4)
					{
						sb.Append(" expand significantly, stopping at " + newData.cupSize.AsText() + "s.");
					}
					else
					{
						sb.Append(" grow disturbingly large, finally stopping at " + newData.cupSize.AsText() + "s.");
					}
				}
				sb.Append(Environment.NewLine);
				iteration++;
			}

			// Nipples
			if (oldBreastRows.nippleLength < nippleLength)
			{
				sb.Append(" A tender ache starts at your " + CommonLongNippleDescription() + " as they grow to match your burgeoning breast-flesh.");
			}

			return sb.ToString();
		}
	}

	partial class BreastCollectionData : IBreastCollection<BreastData>
{
	ReadOnlyCollection<BreastData> IBreastCollection<BreastData>.breasts => breasts;
}

internal static class BreastCollectionStrings
{
	private static int NumBreasts<T>(this IBreastCollection<T> collection) where T : IBreast
	{
		return collection.breasts.Count;
	}

	#region Breast Text

	internal static string AllBreastsShortDescription<T>(IBreastCollection<T> collection, bool alternateFormat = false) where T : IBreast
	{
		return BreastRowCountText(collection, alternateFormat, true) + collection.AverageBreasts().ShortDescription();
	}

	internal static string AllBreastsLongDescription<T>(IBreastCollection<T> collection, bool alternateFormat = false) where T : IBreast
	{
		return BreastRowCountText(collection, alternateFormat, true) + collection.AverageBreasts().LongDescription(false, true);
	}

	internal static string AllBreastsFullDescription<T>(IBreastCollection<T> collection, bool alternateFormat = false) where T : IBreast
	{
		return BreastRowCountText(collection, alternateFormat, true) + collection.AverageBreasts().FullDescription(false, false, true);
	}

	internal static string ChestOrAllBreastsShort<T>(IBreastCollection<T> collection, bool alternateFormat = false) where T : IBreast
	{
		if (collection.NumBreasts() == 1 && collection.breasts[0].isMaleBreasts)
		{
			return ChestShortDesc(collection, alternateFormat);
		}
		else
		{
			return AllBreastsShortDescription(collection, alternateFormat);
		}
	}

	internal static string ChestOrAllBreastsLong<T>(IBreastCollection<T> collection, bool alternateFormat = false) where T : IBreast
	{
		if (collection.NumBreasts() == 1 && collection.breasts[0].isMaleBreasts)
		{
			return ChestDesc(collection, alternateFormat, false);
		}
		else
		{
			return AllBreastsLongDescription(collection, alternateFormat);
		}
	}

	internal static string ChestOrAllBreastsFull<T>(IBreastCollection<T> collection, bool alternateFormat = false) where T : IBreast
	{
		if (collection.NumBreasts() == 1 && collection.breasts[0].isMaleBreasts)
		{
			return ChestDesc(collection, alternateFormat, true);
		}
		else
		{
			return AllBreastsFullDescription(collection, alternateFormat);
		}
	}

	#endregion

	private static string BreastRowCountText<T>(IBreastCollection<T> collection, bool alternateFormat, bool withEven) where T : IBreast
	{
		string evenStr = withEven ? (collection.breasts.Any(x => x.cupSize != collection.breasts[0].cupSize) ? "uneven " : "even ") : "";
		if (collection.breasts.Count <= 1)
		{
			if (alternateFormat)
			{
				return "a pair of ";
			}
			else
			{
				return "";
			}
		}
		else if (collection.breasts.Count == 2)
		{
			return "two " + evenStr + "rows of ";
		}
		else if (collection.breasts.Count == 3)
		{
			return Utils.RandBool() ? "three " + evenStr + "rows of " : (!string.IsNullOrEmpty(evenStr) ? evenStr + ", " : "") + "multi-layered ";
		}
		else //if (creature.collection.breasts.Count == 4)
		{
			return Utils.RandBool() ? "four " + evenStr + "rows of " : (!string.IsNullOrEmpty(evenStr) ? evenStr + ", " : "") + "four-tiered ";
		}
	}

	private static string ChestShortDesc<T>(IBreastCollection<T> collection, bool withArticle) where T : IBreast
	{
		return (withArticle ? "a " : "") + "chest";
	}

	private static string ChestDesc<T>(IBreastCollection<T> collection, bool withArticle, bool full) where T : IBreast
	{
		string chest = withArticle ? "a flat chest" : "flat chest";
		if (full)
		{
			return chest + " with " + NippleStrings.NippleSizeAdjective(collection.breasts[0].nippleLength) + " nipples";
		}
		else
		{
			return chest;
		}
	}
}
}
