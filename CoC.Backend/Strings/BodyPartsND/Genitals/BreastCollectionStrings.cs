using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

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
					sb.Append("  Your " + breastRows[0].LongDescription() + " are painful and sensitive from being so stuffed with milk.You should release the pressure soon.");
				}
				if (breastRows[0].cupSize >= CupSize.A)
				{
					sb.Append("  You could easily fill a " + breastRows[0].cupSize.AsText() + " bra.");
				}
				//Done with tits.  Move on.
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
					sb.Append(" " + breastRows[0].ShortNippleDescription(hasQuadNipples, false));       // Length of nipples
					sb.Append(" each."); //Description and Plural

					if (breastRows[temp].cupSize >= CupSize.A)
					{
						sb.Append("  They could easily fill a " + breastRows[temp].cupSize.AsText() + " bra.");
					}
				}

				if (currentLactationAmount / currentLactationCapacity > 0.75)
				{
					sb.Append("  Your multiple rows of bountiful breasts are painful and sensitive from being so stuffed with milk. You should release the pressure soon.");
				}


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
			else return AllBreastsShortDescription(collection, alternateFormat);
		}

		internal static string ChestOrAllBreastsLong<T>(IBreastCollection<T> collection, bool alternateFormat = false) where T : IBreast
		{
			if (collection.NumBreasts() == 1 && collection.breasts[0].isMaleBreasts)
			{
				return ChestDesc(collection, alternateFormat, false);
			}
			else return AllBreastsLongDescription(collection, alternateFormat);
		}

		internal static string ChestOrAllBreastsFull<T>(IBreastCollection<T> collection, bool alternateFormat = false) where T : IBreast
		{
			if (collection.NumBreasts() == 1 && collection.breasts[0].isMaleBreasts)
			{
				return ChestDesc(collection, alternateFormat, true);
			}
			else return AllBreastsFullDescription(collection, alternateFormat);
		}

		#endregion

		private static string BreastRowCountText<T>(IBreastCollection<T> collection, bool alternateFormat, bool withEven) where T : IBreast
		{
			string evenStr = withEven ? (collection.breasts.Any(x => x.cupSize != collection.breasts[0].cupSize) ? "uneven " : "even ") : "";
			if (collection.breasts.Count <= 1)
			{
				if (alternateFormat) return "a pair of ";
				else return "";
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
