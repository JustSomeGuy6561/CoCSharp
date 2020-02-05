//ArmStrings.cs
//Description: Implements the strings for the arm and armtype. separation of concerns.
//Author: JustSomeGuy
//1/18/2019, 9:30 PM
using CoC.Backend.Creatures;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System.Text;

namespace CoC.Backend.BodyParts
{
	public partial class Build
	{
		public static string Name()
		{
			return "Build";
		}
	}

	public partial class Hips
	{
		public static string Name()
		{
			return "Hips";
		}
	}

	public partial class ButtTattooLocation
	{
		private static string LeftCheekButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LeftCheekLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightCheekButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightCheekLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
	}

	public partial class Butt
	{
		public static string Name()
		{
			return "Butt";
		}

		private string AllTattoosShort(Creature creature, Conjugate conjugate)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private string AllTattoosLong(Creature creature, Conjugate conjugate)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
	}

	internal static class BuildStrings
	{
		public static string ButtAdjective(byte size)
		{

			if (size == Butt.BUTTLESS)
			{
				return "buttless";
			}
			else if (size < Butt.TIGHT)
			{
				return "very tight";
			}
			else if (size < Butt.AVERAGE)
			{
				return "tight";
			}
			else if (size < Butt.NOTICEABLE)
			{
				return "average";
			}
			else if (size < Butt.LARGE)
			{
				return "noticable";
			}
			else if (size < Butt.JIGGLY)
			{
				return "large";
			}
			else if (size < Butt.EXPANSIVE)
			{
				return "jiggly";
			}
			else if (size < Butt.HUGE)
			{
				return "expansive";
			}
			else if (size < Butt.INCONCEIVABLY_BIG)
			{
				return "huge";
			}
			else
			{
				return "inconceivably big";
			}
		}

		public static string ButtShortDescription(byte size, bool singleMemberFormat)
		{
			string noun = Utils.RandomChoice("butt ", "ass ");
			if (size < Butt.TIGHT)
			{
				if (singleMemberFormat)
				{
					return Utils.RandomChoice("an insignificant ", "a very small ") + noun;
				}
				else
				{
					return Utils.RandomChoice("insignificant ", "very small ") + noun;
				}
			}
			else if (size < Butt.AVERAGE)
			{
				string article = singleMemberFormat ? "a " : "";
				return article + Utils.RandomChoice("tight ", "firm ", "compact ") + noun;
			}
			else if (size < Butt.NOTICEABLE)
			{
				if (singleMemberFormat) return Utils.RandomChoice("a regular ", "an unremarkable ") + noun;
				else return Utils.RandomChoice("regular ", "unremarkable ") + noun;
			}
			else if (size < Butt.LARGE)
			{
				if (Utils.Rand(3) == 0) return (singleMemberFormat ? "a " : "") + "handful of ass";
				return (singleMemberFormat ? "a " : "") + Utils.RandomChoice("full ", "shapely ") + noun;
			}
			else if (size < Butt.JIGGLY)
			{
				return (singleMemberFormat ? "a " : "") + Utils.RandomChoice("squeezable ", "large ", "substantial ") + noun;
			}
			else if (size < Butt.EXPANSIVE)
			{
				return (singleMemberFormat ? "a " : "") + Utils.RandomChoice("jiggling ", "spacious ", "heavy ") + noun;
			}
			else if (size < Butt.HUGE)
			{
				if (Utils.Rand(3) == 0) return (singleMemberFormat ? "a " : "") + "generous amount of ass";
				else if (singleMemberFormat) return Utils.RandomChoice("an expansive ", "a voluminous ") + noun;
				else return Utils.RandomChoice("expansive ", "voluminous ") + noun;
			}
			else if (size < Butt.INCONCEIVABLY_BIG)
			{
				if (Utils.Rand(3) == 0) return (singleMemberFormat ? "a " : "") + "jiggling expanse of ass";
				return Utils.RandomChoice("huge ", "vast ") + noun;
			}
			else //if (buttSize >= Butt.INCONCEIVABLY_BIG)
			{
				return (singleMemberFormat ? "a " : "") + Utils.RandomChoice("ginormous ", "colossal ", "tremendous ") + noun;
			}
		}


		public static string ButtLongDescription(byte size, byte muscleTone, bool alternateFormat)
		{
			string noun = Utils.RandomChoice("butt", "butt", "butt", "butt", "ass", "ass", "ass", "ass", "backside", "backside", "derriere", "rump", "bottom");

			if (size <= 1)
			{
				if (muscleTone >= 60)
					return (alternateFormat ? "an " : "") + "incredibly tight, perky " + noun;
				else
				{
					string softness = (muscleTone <= 30 && Utils.Rand(3) == 0) ? "yet soft " : "";
					return (alternateFormat ? "a " : "") + Utils.RandomChoice("tiny ", "very small ", "dainty ") + softness + noun;
				}
			}
			else if (size < 4)
			{
				if (muscleTone >= 65)
				{
					return (alternateFormat ? "a " : "") + Utils.RandomChoice("perky, muscular ", "tight, toned ", "compact, muscular ", "tight ", "muscular, toned ") + noun;
				}
				//Nondescript
				else if (muscleTone >= 30)
				{
					return (alternateFormat ? "a " : "") + Utils.RandomChoice("tight ", "firm ", "compact ", "petite ") + noun;
				}
				//FLABBAH
				else
				{
					return (alternateFormat ? "a " : "") + Utils.RandomChoice("small, heart-shaped ", "soft, compact ", "soft, heart-shaped ", "small, cushy ", "small ", "petite ", "snug ") + noun;
				}
			}
			else if (size < 6)
			{
				//TOIGHT LIKE A TIGER
				if (muscleTone >= 65)
				{
					return (alternateFormat ? "a " : "") + Utils.RandomChoice("nicely muscled ", "nice, toned ", "muscly ", "nice toned ", "toned ", "fair ") + noun;
				}
				//Nondescript
				else if (muscleTone >= 30)
				{
					return (alternateFormat ? "a " : "") + Utils.RandomChoice("nice ", "fair ") + noun;
				}
				//FLABBAH
				else
				{
					return (alternateFormat ? "a " : "") + Utils.RandomChoice("nice, cushiony ", "soft ", "nicely-rounded, heart-shaped ", "cushy ", "soft, squeezable ") + noun;
				}
			}
			else if (size < 8)
			{
				//TOIGHT LIKE A TIGER
				if (muscleTone >= 65)
				{
					return (alternateFormat ? "a " : "") + Utils.RandomChoice("full, toned ", "muscly handful of ", "shapely, toned ", "muscular, hand-filling ", "shapely, chiseled ", "full ", "chiseled ") + noun;
				}
				//Nondescript
				else if (muscleTone >= 30)
				{
					return (alternateFormat ? "a " : "") + Utils.RandomChoice("handful of ", "full ", "shapely ", "hand-filling ") + noun;
				}
				//FLABBAH
				else
				{
					if (Utils.Rand(8) == 0) return (alternateFormat ? "a " : "") + "supple, handful of ass";
					return (alternateFormat ? "a " : "") + Utils.RandomChoice("somewhat jiggly ", "soft, hand-filling ", "cushiony, full ", "plush, shapely ", "full ", "soft, shapely ", "rounded, spongy ") + noun;
				}
			}
			else if (size < 10)
			{
				//TOIGHT LIKE A TIGER
				if (muscleTone >= 65)
				{
					return (alternateFormat ? "a " : "") + Utils.RandomChoice("large, muscular ", "substantial, toned ", "big-but-tight ", "squeezable, toned ", "large, brawny ",
						"big-but-fit ", "powerful, squeezable ", "large ") + noun;
				}
				//Nondescript
				else if (muscleTone >= 30)
				{
					return (alternateFormat ? "a " : "") + Utils.RandomChoice("squeezable ", "large ", "substantial ") + noun;
				}
				//FLABBAH
				else
				{
					return (alternateFormat ? "a " : "") + Utils.RandomChoice("large, bouncy ", "soft, eye-catching ", "big, slappable ", "soft, pinchable ", "large, plush ", "squeezable ",
						"cushiony ", "plush ", "pleasantly plump ") + noun;
				}
			}
			else if (size < 13)
			{
				//TOIGHT LIKE A TIGER
				if (muscleTone >= 65)
				{
					return (alternateFormat ? "a " : "") + Utils.RandomChoice("thick, muscular ", "big, burly ", "heavy, powerful ", "spacious, muscular ", "toned, cloth-straining ",
						"thick ", "thick, strong ") + noun;
				}
				//Nondescript
				else if (muscleTone >= 30)
				{
					return (alternateFormat ? "a " : "") + Utils.RandomChoice("jiggling ", "spacious ", "heavy ", "cloth-straining ") + noun;
				}
				//FLABBAH
				else
				{
					return (alternateFormat ? "a " : "") + Utils.RandomChoice("super-soft, jiggling ", "spacious, cushy ", "plush, cloth-straining ", "squeezable, over-sized ",
						"spacious ", "heavy, cushiony ", "slappable, thick ", "jiggling ", "spacious ", "soft, plump ") + noun;
				}
			}
			else if (size < 16)
			{
				//TOIGHT LIKE A TIGER
				if (muscleTone >= 65)
				{
					if (Utils.Rand(8) == 0)
					{
						return (alternateFormat ? "an " : "") + "expansive, muscled " + noun;
					}
					return (alternateFormat ? "a " : "") + Utils.RandomChoice("voluminous, rippling ", "generous, powerful ", "big, burly ", "well-built, voluminous ",
						"powerful ", "muscular ", "powerful, expansive ") + noun;
				}
				//Nondescript
				else if (muscleTone >= 30)
				{
					if (Utils.Rand(4) == 0) return (alternateFormat ? "an " : "") + "expansive " + noun;
					return (alternateFormat ? "a " : "") + Utils.RandomChoice("generous ", "voluminous ", "wide ") + noun;
				}
				//FLABBAH
				else
				{
					if (Utils.Rand(11) == 4) return (alternateFormat ? "an " : "") + "expansive, squeezable " + noun;
					return (alternateFormat ? "a " : "") + Utils.RandomChoice("pillow-like ", "generous, cushiony ", "wide, plush ", "soft, generous ",
						"slappable ", "thickly-padded ", "wide, jiggling ", "wide ", "voluminous ", "soft, padded ") + noun;
				}
			}
			else if (size < 20)
			{
				if (muscleTone >= 65)
				{
					return (alternateFormat ? "a " : "") + Utils.RandomChoice("huge, toned ", "vast, muscular ", "vast, well-built ", "huge, muscular ", "strong, immense ", "muscle-bound ") + noun;
				}
				//Nondescript
				else if (muscleTone >= 30)
				{

					if (Utils.Rand(5) == 0) return (alternateFormat ? "a " : "") + "jiggling expanse of ass";
					if (Utils.Rand(4) == 0) return (alternateFormat ? "a " : "") + "copious ass-flesh";
					return (alternateFormat ? "a " : "") + Utils.RandomChoice("huge ", "vast ", "giant ") + noun;
				}
				//FLABBAH
				else
				{
					if (Utils.Rand(11) == 2) return (alternateFormat ? "an " : "") + "expansive, jiggling " + noun;
					return (alternateFormat ? "a " : "") + Utils.RandomChoice("vast, cushiony ", "huge, plump ", "huge, cushiony ", "huge, slappable ",
						"seam-bursting ", "plush, vast ", "giant, slappable ", "giant ", "huge ", "swollen, pillow-like ") + noun;
				}
			}
			else //if (size >= 20)
			{
				if (muscleTone >= 65)
				{
					if (Utils.Rand(7) == 0) return (alternateFormat ? "a " : "") + "colossal, muscly ass";
					return (alternateFormat ? "a " : "") + Utils.RandomChoice("ginormous, muscle-bound ", "colossal yet toned ", "strong, tremendously large ", "tremendous, muscled ",
						"ginormous, toned ", "colossal, well-defined ") + noun;
				}
				//Nondescript
				else if (muscleTone >= 30)
				{
					return (alternateFormat ? "a " : "") + Utils.RandomChoice("ginormous ", "colossal ", "tremendous ", "gigantic ") + noun;
				}
				//FLABBAH
				else
				{
					return (alternateFormat ? "a " : "") + Utils.RandomChoice("ginormous, jiggly ", "plush, ginormous ", "seam-destroying ", "tremendous, rounded ", "bouncy, colossal ",
						"thong-devouring ", "tremendous, thickly padded ", "ginormous, slappable ", "gigantic, rippling ", "gigantic ", "ginormous ", "colossal ", "tremendous ") + noun;
				}
			}
		}

		public static string HipAdjective(byte size, bool alternateFormat = false)
		{
			if (size <= Hips.BOYISH)
			{
				return (alternateFormat ? "a " : "") + "slim, masculine";
			}
			else if (size < Hips.SLENDER)
			{
				return (alternateFormat ? "a " : "") + "boyish";
			}
			else if (size < Hips.AVERAGE)
			{
				return (alternateFormat ? "a " : "") + "slender";
			}
			else if (size < Hips.AMPLE)
			{
				return (alternateFormat ? "an " : "") + "average";
			}
			else if (size < Hips.CURVY)
			{
				return (alternateFormat ? "an " : "") + "ample";
			}
			else if (size < Hips.PUDGY)
			{
				return (alternateFormat ? "a " : "") + "curvy";
			}
			else if (size < Hips.FERTILE)
			{
				return (alternateFormat ? "a " : "") + "very curvy, amost pudgy";
			}
			else if (size < Hips.INHUMANLY_WIDE)
			{
				return (alternateFormat ? "a " : "") + "child-bearing";
			}
			else
			{
				return (alternateFormat ? "a " : "") + "broodmother level";
			}
		}


		public static string HipShortDescription(byte size, bool plural)
		{
			return HipAdjective(size) + " hip" + (plural ? "s" : "");
		}
		public static string HipSingleDescription(byte size)
		{
			return HipAdjective(size, true) + " hip";
		}
		public static string HipLongDescription(byte size, LowerBodyType lowerBody, BodyType bodyType, byte thickness)
		{

			StringBuilder sb = new StringBuilder();
			if (size < Hips.SLENDER)
			{
				sb.Append(Utils.RandomChoice("tiny ", "narrow ", "boyish "));
			}
			else if (size < Hips.AVERAGE)
			{
				sb.Append(Utils.RandomChoice("slender ", "narrow ", "thin "));
				if (thickness < 30)
				{
					sb.Append(Utils.RandomChoice("slightly-flared ", "curved "));
				}
			}
			else if (size < Hips.AMPLE)
			{
				sb.Append(Utils.RandomChoice("well-formed ", "pleasant "));
				if (thickness < 30)
				{
					sb.Append(Utils.RandomChoice("flared ", "curvy "));
				}
			}
			else if (size < Hips.CURVY)
			{
				sb.Append(Utils.RandomChoice("ample ", "noticeable ", "girly "));
				if (thickness < 30)
				{
					sb.Append(Utils.RandomChoice("flared ", "waspish "));
				}
			}
			//we just skip pudgy. ok.
			else if (size < Hips.FERTILE)
			{
				sb.Append(Utils.RandomChoice("flared ", "curvy ", "wide "));
				if (thickness < 30)
				{
					sb.Append(Utils.RandomChoice("flared ", "waspish "));
				}
			}
			else if (size < Hips.INHUMANLY_WIDE)
			{
				if (thickness < 40)
				{
					sb.Append(Utils.RandomChoice("flared, ", "waspish, "));
				}
				sb.Append(Utils.RandomChoice("fertile ", "child-bearing ", "voluptuous "));
			}
			else //if (hipSize >= Hips.INHUMANLY_WIDE)
			{
				if (thickness < 40)
				{
					sb.Append(Utils.RandomChoice("flaring, ", "incredibly waspish, "));
				}
				sb.Append(Utils.RandomChoice("broodmother-sized ", "cow-like ", "inhumanly-wide "));
			}
			//Taurs
			if (lowerBody.isQuadruped && Utils.Rand(3) == 0) sb.Append("flanks");
			//Nagas have sides, right?
			else if (lowerBody.isMonoped && Utils.Rand(3) == 0) sb.Append("sides");
			//Non taurs or taurs who didn't roll flanks
			else
			{
				sb.Append(Utils.RandomChoice("hips", "thighs"));
			}
			return sb.ToString();
		}
	}
}
