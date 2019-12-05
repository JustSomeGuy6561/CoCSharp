//ArmStrings.cs
//Description: Implements the strings for the arm and armtype. separation of concerns.
//Author: JustSomeGuy
//1/18/2019, 9:30 PM
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
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

		private LowerBodyData lowerBodyData => CreatureStore.TryGetCreature(creatureID, out Creature creature) ? creature.lowerBody.AsReadOnlyData() : new LowerBodyData(creatureID);
		private BodyData bodyData => CreatureStore.TryGetCreature(creatureID, out Creature creature) ? creature.body.AsReadOnlyData() : new BodyData(creatureID);

		public string HipAdjective()
		{
			if (size <= BOYISH)
			{
				return "slim, masculine";
			}
			else if (size < SLENDER)
			{
				return "boyish";
			}
			else if (size < AVERAGE)
			{
				return "slender";
			}
			else if (size < AMPLE)
			{
				return "average";
			}
			else if (size < CURVY)
			{
				return "ample";
			}
			else if (size < PUDGY)
			{
				return "curvy";
			}
			else if (size < FERTILE)
			{
				return "very curvy, amost pudgy";
			}
			else if (size < INHUMANLY_WIDE)
			{
				return "child-bearing";
			}
			else
			{
				return "broodmother level";
			}
		}

		public string ShortDescription()
		{
			return HipAdjective() + " hips";
		}

		public string LongDescription(byte thickness)
		{
			LowerBodyType lowerBody = lowerBodyData.type;
			BodyType bodyType = bodyData.type;

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
			else //if (hipSize >= INHUMANLY_WIDE)
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

	public partial class Butt
	{
		public string ButtAdjective()
		{

			if (size == BUTTLESS)
			{
				return "buttless";
			}
			else if (size < TIGHT)
			{
				return "very tight";
			}
			else if (size < AVERAGE)
			{
				return "tight";
			}
			else if (size < NOTICEABLE)
			{
				return "average";
			}
			else if (size < LARGE)
			{
				return "noticable";
			}
			else if (size < JIGGLY)
			{
				return "large";
			}
			else if (size < EXPANSIVE)
			{
				return "jiggly";
			}
			else if (size < HUGE)
			{
				return "expansive";
			}
			else if (size < INCONCEIVABLY_BIG)
			{
				return "huge";
			}
			else
			{
				return "inconceivably big";
			}
		}

		public string ShortDescription()
		{
			string noun = Utils.RandomChoice("butt ", "ass ");
			if (size < TIGHT)
			{
				return Utils.RandomChoice("insignificant ", "very small " ) + noun;
			}
			else if (size < AVERAGE)
			{
				return Utils.RandomChoice("tight ", "firm ", "compact " ) + noun;
			}
			else if (size < NOTICEABLE)
			{
				return Utils.RandomChoice("regular ", "unremarkable " ) + noun;
			}
			else if (size < LARGE)
			{
				if (Utils.Rand(3) == 0) return "handful of ass";
				return Utils.RandomChoice("full ", "shapely " ) + noun;
			}
			else if (size < JIGGLY)
			{
				return Utils.RandomChoice("squeezable ", "large ", "substantial " ) + noun;
			}
			else if (size < EXPANSIVE)
			{
				return Utils.RandomChoice("jiggling ", "spacious ", "heavy " ) + noun;
			}
			else if (size < HUGE)
			{
				if (Utils.Rand(3) == 0) return "generous amount of ass";
				return Utils.RandomChoice("expansive ", "voluminous " ) + noun;
			}
			else if (size < INCONCEIVABLY_BIG)
			{
				if (Utils.Rand(3) == 0) return "jiggling expanse of ass";
				return Utils.RandomChoice("huge ", "vast " ) + noun;
			}
			else //if (buttSize >= INCONCEIVABLY_BIG)
			{
				return Utils.RandomChoice("ginormous ", "colossal ", "tremendous " ) + noun;
			}
		}

		public string LongDescription(byte muscleTone)
		{
			string noun = Utils.RandomChoice("butt", "butt", "butt", "butt", "ass", "ass", "ass", "ass", "backside", "backside", "derriere", "rump", "bottom");

			if (size <= 1)
			{
				if (muscleTone >= 60)
					return "incredibly tight, perky " + noun;
				else
				{
					string softness = (muscleTone <= 30 && Utils.Rand(3) == 0) ? "yet soft " : ""; 
					return Utils.RandomChoice("tiny ", "very small ", "dainty ") + softness + noun;
				}
			}
			else if (size < 4)
			{
				if (muscleTone >= 65)
				{
					return Utils.RandomChoice("perky, muscular ", "tight, toned ", "compact, muscular ", "tight ", "muscular, toned ") + noun;
				}
				//Nondescript
				else if (muscleTone >= 30)
				{
					return Utils.RandomChoice("tight ", "firm ", "compact ", "petite ") + noun;
				}
				//FLABBAH
				else
				{
					return Utils.RandomChoice("small, heart-shaped ", "soft, compact ", "soft, heart-shaped ", "small, cushy ", "small ", "petite ", "snug ") + noun;
				}
			}
			else if (size < 6)
			{
				//TOIGHT LIKE A TIGER
				if (muscleTone >= 65)
				{
					return Utils.RandomChoice("nicely muscled ", "nice, toned ", "muscly ", "nice toned ", "toned ", "fair ") + noun;
				}
				//Nondescript
				else if (muscleTone >= 30)
				{
					return Utils.RandomChoice("nice ", "fair ") + noun;
				}
				//FLABBAH
				else
				{
					return Utils.RandomChoice("nice, cushiony ", "soft ", "nicely-rounded, heart-shaped ", "cushy ", "soft, squeezable ") + noun;
				}
			}
			else if (size < 8)
			{
				//TOIGHT LIKE A TIGER
				if (muscleTone >= 65)
				{
					return Utils.RandomChoice("full, toned ", "muscly handful of ", "shapely, toned ", "muscular, hand-filling ", "shapely, chiseled ", "full ", "chiseled ") + noun;
				}
				//Nondescript
				else if (muscleTone >= 30)
				{
					return Utils.RandomChoice("handful of ", "full ", "shapely ", "hand-filling ") + noun;
				}
				//FLABBAH
				else
				{
					if (Utils.Rand(8) == 0) return "supple, handful of ass";
					return Utils.RandomChoice("somewhat jiggly ", "soft, hand-filling ", "cushiony, full ", "plush, shapely ", "full ", "soft, shapely ", "rounded, spongy ") + noun;
				}
			}
			else if (size < 10)
			{
				//TOIGHT LIKE A TIGER
				if (muscleTone >= 65)
				{
					return Utils.RandomChoice("large, muscular ", "substantial, toned ", "big-but-tight ", "squeezable, toned ", "large, brawny ",
						"big-but-fit ", "powerful, squeezable ", "large ") + noun;
				}
				//Nondescript
				else if (muscleTone >= 30)
				{
					return Utils.RandomChoice("squeezable ", "large ", "substantial ") + noun;
				}
				//FLABBAH
				else
				{
					return Utils.RandomChoice("large, bouncy ", "soft, eye-catching ", "big, slappable ", "soft, pinchable ", "large, plush ", "squeezable ",
						"cushiony ", "plush ", "pleasantly plump ") + noun;
				}
			}
			else if (size < 13)
			{
				//TOIGHT LIKE A TIGER
				if (muscleTone >= 65)
				{
					return Utils.RandomChoice("thick, muscular ", "big, burly ", "heavy, powerful ", "spacious, muscular ", "toned, cloth-straining ", "thick ", "thick, strong ") + noun;
				}
				//Nondescript
				else if (muscleTone >= 30)
				{
					return Utils.RandomChoice("jiggling ", "spacious ", "heavy ", "cloth-straining ") + noun;
				}
				//FLABBAH
				else
				{
					return Utils.RandomChoice("super-soft, jiggling ", "spacious, cushy ", "plush, cloth-straining ", "squeezable, over-sized ", "spacious ",
						"heavy, cushiony ", "slappable, thick ", "jiggling ", "spacious ", "soft, plump ") + noun;
				}
			}
			else if (size < 16)
			{
				//TOIGHT LIKE A TIGER
				if (muscleTone >= 65)
				{
					return Utils.RandomChoice("expansive, muscled ", "voluminous, rippling ", "generous, powerful ", "big, burly ", "well-built, voluminous ",
						"powerful ", "muscular ", "powerful, expansive ") + noun;
				}
				//Nondescript
				else if (muscleTone >= 30)
				{
					return Utils.RandomChoice("expansive ", "generous ", "voluminous ", "wide ") + noun;
				}
				//FLABBAH
				else
				{
					return Utils.RandomChoice("pillow-like ", "generous, cushiony ", "wide, plush ", "soft, generous ", "expansive, squeezable ",
						"slappable ", "thickly-padded ", "wide, jiggling ", "wide ", "voluminous ", "soft, padded ") + noun;
				}
			}
			else if (size < 20)
			{
				if (muscleTone >= 65)
				{
					return Utils.RandomChoice("huge, toned ", "vast, muscular ", "vast, well-built ", "huge, muscular ", "strong, immense ", "muscle-bound ") + noun;
				}
				//Nondescript
				else if (muscleTone >= 30)
				{
					if (Utils.Rand(5) == 0) return "jiggling expanse of ass";
					if (Utils.Rand(5) == 0) return "copious ass-flesh";
					return Utils.RandomChoice("huge ", "vast ", "giant ") + noun;
				}
				//FLABBAH
				else
				{
					return Utils.RandomChoice("vast, cushiony ", "huge, plump ", "expansive, jiggling ", "huge, cushiony ", "huge, slappable ",
						"seam-bursting ", "plush, vast ", "giant, slappable ", "giant ", "huge ", "swollen, pillow-like ") + noun;
				}
			}
			else //if (size >= 20)
			{
				if (muscleTone >= 65)
				{
					if (Utils.Rand(7) == 0) return "colossal, muscly ass";
					return Utils.RandomChoice("ginormous, muscle-bound ", "colossal yet toned ", "strong, tremendously large ", "tremendous, muscled ",
						"ginormous, toned ", "colossal, well-defined ") + noun;
				}
				//Nondescript
				else if (muscleTone >= 30)
				{
					return Utils.RandomChoice("ginormous ", "colossal ", "tremendous ", "gigantic ") + noun;
				}
				//FLABBAH
				else
				{
					return Utils.RandomChoice("ginormous, jiggly ", "plush, ginormous ", "seam-destroying ", "tremendous, rounded ", "bouncy, colossal ",
						"thong-devouring ", "tremendous, thickly padded ", "ginormous, slappable ", "gigantic, rippling ", "gigantic ", "ginormous ", "colossal ", "tremendous ") + noun;
				}
			}
		}
	}
}
