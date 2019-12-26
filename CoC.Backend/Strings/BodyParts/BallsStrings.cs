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
		int hoursSinceLastCum { get; }
		float relativeLust { get; }
		BodyType bodyType { get; }
	}

	public partial class Balls : IBalls
	{
		int IBalls.hoursSinceLastCum => hoursSinceCum;

		float IBalls.relativeLust => relativeLust;

		BodyType IBalls.bodyType => bodyType;

		public static string Name()
		{
			return "Balls";
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
							return "Your " + SackDescription() + " clings tightly to your groin, dripping and holding your" + LongDescription(false, false) + " snugly against you." + fNale;
						else
							return "Your " + SackDescription() + " clings tightly to your groin, holding your" + LongDescription(false, false) + " snugly against you." + fNale;
					}
					else if (!player.hasCock)
					{
						if (bodyType.primaryIsFur)
						{
							return "A fuzzy " + SackDescription() + " filled with " + LongDescription(false, true) + " swings low under where a penis would normally grow." + fNale;
						}
						else if (bodyType.epidermisType == EpidermisType.SCALES)
						{
							return "A scaley " + SackDescription() + " hugs your " + LongDescription(false, false) + " tightly against your body." + fNale;
						}
						else if (bodyType == BodyType.GOO)
						{
							return "An oozing, semi-solid sack with " + LongDescription(false, true) + " swings heavily under where a penis would normally grow." + fNale;
						}
						else //humanoid or skin. more cases for carapace or rubber could be added, idk.
						{
							return ("A " + SackDescription() + " with " + LongDescription(false, true) + " swings heavily under where a penis would normally grow.") + fNale;
						}
					}
					else
					{
						if (bodyType.primaryIsFur)
						{
							return "A fuzzy " + SackDescription() + " filled with " + LongDescription(false, false) + " swings low under your " + player.genitals.AllCocksLongDescription() + "." + fNale;
						}
						else if (bodyType.epidermisType == EpidermisType.SCALES)
						{
							return "A scaley " + SackDescription() + " hugs your " + LongDescription(false, false) + " tightly against your body." + fNale;
						}
						else if (bodyType == BodyType.GOO)
						{
							return "An oozing, semi-solid sack with " + LongDescription(false, false) + " swings heavily beneath your " + player.genitals.AllCocksLongDescription() + "." + fNale;
						}
						else // if (bodyType == BodyType.HUMANOID)
						{
							return "A " + SackDescription() + " with " + LongDescription(false, false) + " swings heavily beneath your " + player.genitals.AllCocksLongDescription() + "." + fNale;
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

		int IBalls.hoursSinceLastCum => hoursSinceLastCum;

		float IBalls.relativeLust => relativeLust;

		BodyType IBalls.bodyType => bodyType;

	}

	public static class BallsStrings
	{
		internal static string SackDescription(IBalls balls)
		{
			if (balls.count == 0) return "prostate";

			return Utils.RandomChoice("scrotum", "sack", "nutsack", "ballsack", "beanbag", "pouch");
		}

		public static string BallsNoun(bool plural)
		{
			if (plural)
			{
				return Utils.RandomChoice("nuts", "gonads", "testes", "testicles", "testicles", "balls", "balls", "balls");
			}
			else
			{
				return Utils.RandomChoice("nut", "gonad", "testicle");
			}
		}

		public static string SingularBallNoun()
		{
			return Utils.RandomChoice("a nut", "a gonad", "a testicle");
		}

		internal static string ShortDesc(byte numBalls, bool prostateIfNoBalls, out bool isPlural)
		{
			if (numBalls == 0 && prostateIfNoBalls)
			{
				isPlural = false;
				return FallbackProstateText(false);
			}
			else if (numBalls == 0)
			{
				isPlural = true;
				return "non-existent balls";
			}
			else if (numBalls == 1)
			{
				isPlural = false;
				return " uniball";
			}

			else
			{
				isPlural = true;
				return BallsNoun(true);
			}
		}

		//numballs is so we know if it's a uniball
		internal static string SingleBallDesc(byte numBalls, bool prostateIfNoBalls)
		{
			if (numBalls == 0 && prostateIfNoBalls)
			{
				return FallbackProstateText(true);
			}
			else if (numBalls == 0)
			{
				return "a non-existant testicle";
			}
			else if (numBalls == 1)
			{
				return "a uniball";
			}
			else
			{
				return SingularBallNoun();
			}
		}

		internal static string FallbackProstateText(bool alternateFormat)
		{
			return alternateFormat ? "a prostate" : "prostate";
		}

		internal static string CountDescription(IBalls balls, bool simplifiedCount, bool alternateFormat, bool prostateIfNoBalls, out bool isPlural)
		{
			if (balls.count == 0 && prostateIfNoBalls)
			{
				isPlural = false;
				return alternateFormat ? "a prostate" : "prostate";
			}
			else if (balls.count == 0)
			{
				isPlural = true;
				return alternateFormat ? "a distinct lack of balls" : "lack of balls";
			}
			else
			{
				isPlural = balls.count != 1;
				return CountText(balls, simplifiedCount, alternateFormat) + BallsNoun(balls.count != 1);
			}
		}

		internal static string SizeDescription(IBalls balls, bool preciseSize, out bool isPlural)
		{
			if (balls.count == 0)
			{
				isPlural = true;
				return "non-existent balls";
			}
			else
			{
				isPlural = balls.count != 1;
				return SizeText(balls, preciseSize) + BallsNoun(balls.count != 1);
			}

		}

		internal static string LongDescription(IBalls balls, bool simplified, bool alternateFormat, bool prostateIfNoBalls, out bool isPlural)
		{
			if (balls.count == 0)
			{
				isPlural = false;
				return alternateFormat ? "a prostate" : "prostate";
			}
			else
			{
				isPlural = balls.count != 1;
				return CountText(balls, simplified, alternateFormat) + SizeText(balls, simplified) + BallsNoun(balls.count != 1);
			}
		}

		internal static string FullDescription(IBalls balls, bool preciseMeasurements, bool alternateFormat, bool prostateIfNoBalls, out bool isPlural)
		{

			if (balls.count == 0 && prostateIfNoBalls)
			{
				isPlural = false;
				return FallbackProstateText(alternateFormat);
			}
			else if (balls.count == 0)
			{
				isPlural = true;
				return alternateFormat ? "a distinct lack of balls" : "lack of balls";
			}

			bool uniBall = balls.count == 1;

			string description = "";

			//count
			description += CountText(balls, preciseMeasurements, alternateFormat);
			//balls.ballSize (normal)
			if (Utils.Rand(3) <= 1)
			{
				if (balls.size > 1 && !uniBall)
				{
					description += SizeText(balls, preciseMeasurements);
				}
				//balls.ballSize (UNIBALL)
				else if (uniBall)
				{
					description += Utils.RandomChoice("tightly-compressed ", "snug ", "cute ", "pleasantly squeezed ", "compressed-together ");
				}
			}

			//adjective:

			//Descriptive
			if (balls.hoursSinceLastCum >= 48 && Utils.RandBool())
			{
				description += Utils.RandomChoice("overflowing", "swollen", "cum-engorged");

			}
			//lusty
			if (balls.relativeLust > 90 && (description == "") && Utils.RandBool())
			{
				description += Utils.RandomChoice("eager", "full", "needy", "desperate", "throbbing", "heated", "trembling", "quivering", "quaking");

			}
			//Slimy skin
			if (balls.bodyType == BodyType.GOO)
			{
				if (description.Length != 0) description += " ";
				description += Utils.RandomChoice("goopey", "gooey", "slimy");

			}
			if (description.Length != 0) description += " ";



			description += BallsStrings.BallsNoun(balls.count != 1);

			if (uniBall && Utils.RandBool())
			{
				if (Utils.Rand(3) == 0)
					description += " merged into a cute, spherical package";
				else if (Utils.RandBool())
					description += " combined into a round, girlish shape";
				else
					description += " squeezed together into a perky, rounded form";
			}
			isPlural = !uniBall;
			return description;
		}

		//count related
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



		//size strings
		internal static string SizeText(IBalls balls, bool precise)
		{
			if (precise) return SimpleSizeStr(balls);
			else return SizeStr(balls);
		}

		private static string SizeStr(IBalls balls)
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

		private static string SimpleSizeStr(IBalls balls)
		{
			return Measurement.ToNearestSmallUnit(balls.size, true, true, false);
		}
	}
}
