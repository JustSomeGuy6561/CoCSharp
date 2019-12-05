using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts
{
	internal interface IBalls
	{
		byte count { get; }
		byte size { get; }
	}

	public partial class Balls : IBalls
	{
		public static string Name()
		{
			return "Balls";
		}

		public string SackDescription()
		{
			if (!hasBalls) return "prostate";

			return Utils.RandomChoice("scrotum", "sack", "nutsack", "ballsack", "beanbag", "pouch");
		}


		public string CreatureDescription(bool preciseMeasurements, bool withArticle)
		{
			return CountStr(preciseMeasurements, withArticle) + SizeStr(preciseMeasurements) + BallsData.BallsNoun(count != 1);
		}

		public string LongCreatureDescription(bool preciseMeasurements, bool withArticle)
		{
			Creature creature = CreatureStore.GetCreatureClean(creatureID);

			if (count == 0) return "prostate";

			string description = "";

			//count
			description += CountStr(preciseMeasurements, withArticle);
			//balls.ballSize (normal)
			if (Utils.Rand(3) <= 1)
			{
				if (size > 1 && !uniBall)
				{
					description += SizeStr(preciseMeasurements);
				}
				//balls.ballSize (UNIBALL)
				else if (uniBall)
				{
					description += Utils.RandomChoice("tightly-compressed ", "snug ", "cute ", "pleasantly squeezed ", "compressed-together ");
				}
			}

			//adjective:

			//Descriptive
			if (creature?.genitals.hoursSinceLastCum >= 48 && Utils.RandBool())
			{
				description += Utils.RandomChoice("overflowing", "swollen", "cum-engorged");

			}
			//lusty
			if (creature?.relativeLust > 90 && (description == "") && Utils.RandBool())
			{
				description += Utils.RandomChoice("eager", "full", "needy", "desperate", "throbbing", "heated", "trembling", "quivering", "quaking");

			}
			//Slimy skin
			if (creature?.body.type == BodyType.GOO)
			{
				if (description.Length != 0) description += " ";
				description += Utils.RandomChoice("goopey", "gooey", "slimy");

			}
			if (description.Length != 0) description += " ";



			description += BallsData.BallsNoun(count != 1);

			if (uniBall && Utils.RandBool())
			{
				if (Utils.Rand(3) == 0)
					description += " merged into a cute, spherical package";
				else if (Utils.RandBool())
					description += " combined into a round, girlish shape";
				else
					description += " squeezed together into a perky, rounded form";
			}
			return description;
		}

		private string SizeStr(bool preciseSize)
		{
			return BallsData.SizeText(this, preciseSize);
		}

		private string CountStr(bool simpleCount, bool withArticle)
		{
			return BallsData.CountDescription(this, simpleCount, withArticle);
		}


		public string PlayerDescription()
		{
			if (CreatureStore.TryGetCreature(creatureID, out Creature creature) && creature is PlayerBase player)
			{
				string fNale = " You estimate each of them to be about " + Measurement.ToNearestSmallUnit(size, false, true) + " across" + Environment.NewLine;

				BodyType bodyType = player.body.type;
				if (count > 0)
				{
					if (count == 1)
					{
						if (bodyType == BodyType.GOO)
							return "Your " + SackDescription() + " clings tightly to your groin, dripping and holding your" + CreatureDescription(false, false) + " snugly against you." + fNale;
						else
							return "Your " + SackDescription() + " clings tightly to your groin, holding your" + CreatureDescription(false, false) + " snugly against you." + fNale;
					}
					else if (!player.hasCock)
					{
						if (bodyType.primaryIsFur)
						{
							return "A fuzzy " + SackDescription() + " filled with " + CreatureDescription(false, true) + " swings low under where a penis would normally grow." + fNale;
						}
						else if (bodyType.epidermisType == EpidermisType.SCALES)
						{
							return "A scaley " + SackDescription() + " hugs your " + CreatureDescription(false, false) + " tightly against your body." + fNale;
						}
						else if (bodyType == BodyType.GOO)
						{
							return "An oozing, semi-solid sack with " + CreatureDescription(false, true) + " swings heavily under where a penis would normally grow." + fNale;
						}
						else //humanoid or skin. more cases for carapace or rubber could be added, idk.
						{
							return ("A " + SackDescription() + " with " + CreatureDescription(false, true) + " swings heavily under where a penis would normally grow.") + fNale;
						}
					}
					else
					{
						if (bodyType.primaryIsFur)
						{
							return "A fuzzy " + SackDescription() + " filled with " + CreatureDescription(false, false) + " swings low under your " + player.genitals.AllCocksLongDescription() + "." + fNale;
						}
						else if (bodyType.epidermisType == EpidermisType.SCALES)
						{
							return "A scaley " + SackDescription() + " hugs your " + CreatureDescription(false, false) + " tightly against your body." + fNale;
						}
						else if (bodyType == BodyType.GOO)
						{
							return "An oozing, semi-solid sack with " + CreatureDescription(false, false) + " swings heavily beneath your " + player.genitals.AllCocksLongDescription() + "." + fNale;
						}
						else // if (bodyType == BodyType.HUMANOID)
						{
							return "A " + SackDescription() + " with " + CreatureDescription(false, false) + " swings heavily beneath your " + player.genitals.AllCocksLongDescription() + "." + fNale;
						}
					}
				}
				else return "";
			}
			else return "";
		}
	}

	public partial class BallsData : IBalls
	{
		byte IBalls.count => count;

		byte IBalls.size => size;

		public static string BallsNoun(bool plural)
		{
			return Utils.RandomChoice("nut", "gonad", "teste", "testicle", "testicle", "ball", "ball", "ball") + (plural ? "s" : "");
		}


		internal static string BallsWithCountOrProstate(IBalls balls, bool simplifiedCount, bool withArticle)
		{
			if (balls.count == 0)
			{
				return withArticle ? "a prostate" : "prostate";
			}
			else
			{
				return CountDescription(balls, simplifiedCount, withArticle);
			}
		}
		internal static string BallsWithSize(IBalls balls, bool preciseSize)
		{
			if (balls.count == 0)
			{
				return "non-existent balls";
			}
			else
			{
				return SizeDescription(balls, preciseSize);
			}
			
		}

		internal static string BallsLongOrProstate(IBalls balls, bool simplified, bool withArticle)
		{
			if (balls.count == 0)
			{
				return withArticle ? "a prostate" : "prostate";
			}
			else
			{
				return CountText(balls, simplified, withArticle) + SizeText(balls, simplified) + BallsNoun(balls.count != 1);
			}
		}


		internal static string ShortDesc(bool hasBalls)
		{
			return hasBalls ? "balls" : "prostate";
		}

		

		internal static string CountDescription(IBalls balls, bool preciseCount, bool withArticle)
		{
			return CountText(balls, preciseCount, withArticle) + BallsNoun(balls.count != 1);
		}

		internal static string CountText(IBalls balls, bool preciseCount, bool withArticle)
		{
			if (preciseCount)
			{
				return SimpleCountStr(balls, withArticle);
			}
			else
			{
				return CountStr(balls, withArticle);
			}
		}

		private static string CountStr(IBalls balls, bool withArticle)
		{
			if (balls.count == 0)
			{
				return withArticle ? "a distinct lack of" : "lack of";
			}
			else if (balls.count == 1)
			{
				if (withArticle)
				{
					return Utils.RandomChoice("a single ", "a solitary ", "a lone ", "an individual ");
				}
				else
				{
					return Utils.RandomChoice("single ", "solitary ", "lone ", "individual ");
				}
			}
			else if (balls.count == 2)
			{
				if (withArticle)
				{
					return Utils.RandomChoice("a pair of ", "two ", "a duo of ");
				}
				else
				{
					return Utils.RandomChoice("pair of ", "two ", "duo of ");
				}
			}
			else if (balls.count == 4)
			{
				return Utils.RandomChoice("four ", "quadruple ", "two pairs of ", (withArticle ? "a quartette of" : "quartette of "));
			}
			else
			{
				if (withArticle)
				{
					return Utils.RandomChoice("a multitude of ", "many ", "a large handful of ", Utils.NumberAsText(balls.count / 2) + "pairs of ");
				}
				else
				{
					return Utils.RandomChoice("multitude of ", "many ", "large handful of ", Utils.NumberAsText(balls.count / 2) + "pairs of ");
				}
			}
		}

		private static string SimpleCountStr(IBalls balls, bool withArticle)
		{
			if (balls.count == 0)
			{
				return withArticle ? "a distinct lack of " : "lack of ";
			}
			else if (balls.count == 1)
			{
				return withArticle ? "a single " : "sole ";
			}
			else if (balls.count < 4)
			{
				return withArticle ? "a pair of " : "pair of ";
			}
			else
			{
				return Utils.NumberAsText(balls.count / 2) + " pairs of ";
			}
		}

		internal static string SizeDescription(IBalls balls, bool preciseSize)
		{
			return SizeText(balls, preciseSize) + BallsNoun(balls.count != 1);
		}

		internal static string SizeText(IBalls balls, bool precise)
		{
			if (precise) return SimpleSizeStr(balls);
			else return SizeStr(balls);
		}

		internal static string SizeStr(IBalls balls)
		{
			if (balls.size >= 18)
				return "hideously swollen and oversized ";
			else if (balls.size >= 15)
				return "beachball-sized ";
			else if (balls.size >= 12)
				return "watermelon-sized ";
			else if (balls.size >= 9)
				return "basketball-sized ";
			else if (balls.size >= 7)
				return "soccerball-sized ";
			else if (balls.size >= 5)
				return "cantaloupe-sized ";
			else if (balls.size >= 4)
				return "grapefruit-sized ";
			else if (balls.size >= 3)
				return "apple-sized ";
			else if (balls.size >= 2)
				return "baseball-sized ";
			else if (balls.size > 1)
				return "large ";
			else return "";
		}

		internal static string SimpleSizeStr(IBalls balls)
		{
			return Measurement.ToNearestSmallUnit(balls.size, true, true, false);
		}

	}
}
