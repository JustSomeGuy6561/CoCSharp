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

		private LowerBodyWrapper lowerBodyData => CreatureStore.TryGetCreature(creatureID, out Creature creature)? creature.lowerBody.AsReadOnlyReference() : new LowerBodyWrapper(creatureID);
		private BodyWrapper bodyData => CreatureStore.TryGetCreature(creatureID, out Creature creature) ? creature.body.AsReadOnlyReference() : new BodyWrapper(creatureID);

		private string ButtLongDesc()
		{
			StringBuilder sb = new StringBuilder();
			string[] options;
			if (butt.size < Butt.TIGHT)
			{
				if (muscleTone >= TONE_SLIGHTLY_VISIBLE)
				{
					options = new string[] { "incredibly tight, perky " };
				}
				else
				{
					sb.Append(Utils.RandomChoice("tiny", "very small", "dainty"));
					options = new string[1];
					options[0] = " ";
					//Soft PC's buns!
					if (muscleTone <= TONE_SOFT && Utils.Rand(3) == 0) options[0] = ", yet soft ";
				}
			}
			else if (butt.size < Butt.AVERAGE)
			{
				if (muscleTone >= TONE_SLIGHTLY_VISIBLE)
				{
					options = new string[] { "perky, muscular ", "tight, toned ", "compact, muscular ", "tight ", "muscular, toned " };
				}
				//Nondescript
				else if (muscleTone >= TONE_SOFT)
				{
					options = new string[] { "tight ", "firm ", "compact ", "petite " };
				}
				//FLABBAH
				else
				{
					options = new string[] { "small, heart-shaped ", "soft, compact ", "soft, heart-shaped ", "small, cushy ", "small ", "petite ", "snug ", };
				}
			}
			else if (butt.size < Butt.NOTICEABLE)
			{
				//TOIGHT LIKE A TIGER
				if (muscleTone >= TONE_SLIGHTLY_VISIBLE)
				{
					options = new string[] { "nicely muscled ", "nice, toned ", "muscly ", "nice toned ", "toned ", "fair " };
				}
				//Nondescript
				else if (muscleTone >= TONE_SOFT)
				{
					options = new string[] { "nice ", "fair " };
				}
				//FLABBAH
				else
				{
					options = new string[] { "nice, cushiony ", "soft ", "nicely-rounded, heart-shaped ", "cushy ", "soft, squeezable " };
				}
			}
			else if (butt.size < Butt.LARGE)
			{
				//TOIGHT LIKE A TIGER
				if (muscleTone >= TONE_SLIGHTLY_VISIBLE)
				{
					options = new string[] { "full, toned ", "muscly handful of ", "shapely, toned ", "muscular, hand-filling ", "shapely, chiseled ", "full ", "chiseled " };
				}
				//Nondescript
				else if (muscleTone >= TONE_SOFT)
				{
					options = new string[] { "handful of ", "full ", "shapely ", "hand-filling " };
				}
				//FLABBAH
				else
				{
					if (Utils.Rand(8) == 0) return "supple, handful of ass";
					options = new string[] { "somewhat jiggly ", "soft, hand-filling ", "cushiony, full ", "plush, shapely ", "full ", "soft, shapely ", "rounded, spongy " };
				}
			}
			else if (butt.size < Butt.JIGGLY)
			{
				//TOIGHT LIKE A TIGER
				if (muscleTone >= TONE_SLIGHTLY_VISIBLE)
				{
					options = new string[] { "large, muscular ", "substantial, toned ", "big-but-tight ", "squeezable, toned ", "large, brawny ", "big-but-fit ", "powerful, squeezable ", "large " };
				}
				//Nondescript
				else if (muscleTone >= TONE_SOFT)
				{
					options = new string[] { "squeezable ", "large ", "substantial " };
				}
				//FLABBAH
				else
				{
					options = new string[] { "large, bouncy ", "soft, eye-catching ", "big, slappable ", "soft, pinchable ", "large, plush ", "squeezable ", "cushiony ", "plush ", "pleasantly plump " };
				}
			}
			else if (butt.size < Butt.EXPANSIVE)
			{
				//TOIGHT LIKE A TIGER
				if (muscleTone >= TONE_SLIGHTLY_VISIBLE)
				{
					options = new string[] { "thick, muscular ", "big, burly ", "heavy, powerful ", "spacious, muscular ", "toned, cloth-straining ", "thick ", "thick, strong " };
				}
				//Nondescript
				else if (muscleTone >= TONE_SOFT)
				{
					options = new string[] { "jiggling ", "spacious ", "heavy ", "cloth-straining " };
				}
				//FLABBAH
				else
				{
					options = new string[] { "super-soft, jiggling ", "spacious, cushy ", "plush, cloth-straining ", "squeezable, over-sized ",
						"spacious ", "heavy, cushiony ", "slappable, thick ", "jiggling ", "spacious ", "soft, plump " };
				}
			}
			else if (butt.size < Butt.HUGE)
			{
				//TOIGHT LIKE A TIGER
				if (muscleTone >= TONE_SLIGHTLY_VISIBLE)
				{
					options = new string[] { "expansive, muscled ", "voluminous, rippling ", "generous, powerful ",
						"big, burly ", "well-built, voluminous ", "powerful ", "muscular ", "powerful, expansive " };
				}
				//Nondescript
				else if (muscleTone >= TONE_SOFT)
				{
					options = new string[] { "expansive ", "generous ", "voluminous ", "wide " };
				}
				//FLABBAH
				else
				{
					options = new string[] { "pillow-like ", "generous, cushiony ", "wide, plush ", "soft, generous ", "expansive, squeezable ",
						"slappable ", "thickly-padded ", "wide, jiggling ", "wide ", "voluminous ", "soft, padded " };
				}
			}
			else if (butt.size < Butt.INCONCEIVABLY_BIG)
			{
				if (muscleTone >= TONE_SLIGHTLY_VISIBLE)
				{
					options = new string[] { "huge, toned ", "vast, muscular ", "vast, well-built ", "huge, muscular ", "strong, immense ", "muscle-bound " };
				}
				//Nondescript
				else if (muscleTone >= TONE_SOFT)
				{
					if (Utils.Rand(5) == 0) return "jiggling expanse of ass";
					else if (Utils.Rand(4) == 0) return "copious ass-flesh";
					options = new string[] { "huge ", "vast ", "giant " };
				}
				//FLABBAH
				else
				{
					options = new string[] { "vast, cushiony ", "huge, plump ", "expansive, jiggling ", "huge, cushiony ", "huge, slappable ",
						"seam-bursting ", "plush, vast ", "giant, slappable ", "giant ", "huge ", "swollen, pillow-like " };
				}
			}
			else //if (butt.size >= INCONCEIVABLY_BIG)
			{
				if (muscleTone >= TONE_SLIGHTLY_VISIBLE)
				{
					if (Utils.Rand(7) == 0) return "colossal, muscly ass";
					options = new string[] { "ginormous, muscle-bound ", "colossal yet toned ", "strong, tremendously large ", "tremendous, muscled ", "ginormous, toned ", "colossal, well-defined " };
				}
				//Nondescript
				else if (muscleTone >= TONE_SOFT)
				{
					options = new string[] { "ginormous ", "colossal ", "tremendous ", "gigantic " };
				}
				//FLABBAH
				else
				{
					options = new string[] {"ginormous, jiggly ", "plush, ginormous ", "seam-destroying ", "tremendous, rounded ",
						"bouncy, colossal ", "thong-devouring ", "tremendous, thickly padded ", "ginormous, slappable ",
						"gigantic, rippling ", "gigantic ", "ginormous ", "colossal ", "tremendous "};
				}
			}
			sb.Append(Utils.RandomChoice(options));
			sb.Append(new Lottery<string>("butt", "butt", "butt", "butt", "ass", "ass", "ass", "ass", "backside", "backside", "derriere", "rump", "bottom").Select());
			return sb.ToString();
		}

		private string HipsLongDesc()
		{
			LowerBodyType lowerBody = lowerBodyData.type;
			BodyType bodyType = bodyData.type;

			StringBuilder sb = new StringBuilder();
			if (hips.size < Hips.SLENDER)
			{
				sb.Append(Utils.RandomChoice("tiny ", "narrow ", "boyish "));
			}
			else if (hips.size < Hips.AVERAGE)
			{
				sb.Append(Utils.RandomChoice("slender ", "narrow ", "thin "));
				if (thickness < 30)
				{
					sb.Append(Utils.RandomChoice("slightly-flared ", "curved "));
				}
			}
			else if (hips.size < Hips.AMPLE)
			{
				sb.Append(Utils.RandomChoice("well-formed ", "pleasant "));
				if (thickness < 30)
				{
					sb.Append(Utils.RandomChoice("flared ", "curvy "));
				}
			}
			else if (hips.size < Hips.CURVY)
			{
				sb.Append(Utils.RandomChoice("ample ", "noticeable ", "girly "));
				if (thickness < 30)
				{
					sb.Append(Utils.RandomChoice("flared ", "waspish "));
				}
			}
			//we just skip pudgy. ok.
			else if (hips.size < Hips.FERTILE)
			{
				sb.Append(Utils.RandomChoice("flared ", "curvy ", "wide "));
				if (thickness < 30)
				{
					sb.Append(Utils.RandomChoice("flared ", "waspish "));
				}
			}
			else if (hips.size < Hips.INHUMANLY_WIDE)
			{
				if (thickness < 40)
				{
					sb.Append(Utils.RandomChoice("flared, ", "waspish, "));
				}
				sb.Append(Utils.RandomChoice("fertile ", "child-bearing ", "voluptuous "));
			}
			else //if (hips.hipSize >= INHUMANLY_WIDE)
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

	public partial class Hips
	{
		public static string Name()
		{
			return "Hips";
		}

		public string AsText()
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
			return AsText() + " hips";
		}
	}

	public partial class Butt
	{
		public string AsText()
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
			StringBuilder sb = new StringBuilder();
			string[] options;
			if (size < TIGHT)
			{
				options = new string[] { "insignificant ", "very small " };
			}
			else if (size < AVERAGE)
			{
				options = new string[] { "tight ", "firm ", "compact " };
			}
			else if (size < NOTICEABLE)
			{
				options = new string[] { "regular ", "unremarkable " };
			}
			else if (size < LARGE)
			{
				if (Utils.Rand(3) == 0) return "handful of ass";
				options = new string[] { "full ", "shapely " };
			}
			else if (size < JIGGLY)
			{
				options = new string[] { "squeezable ", "large ", "substantial " };
			}
			else if (size < EXPANSIVE)
			{
				options = new string[] { "jiggling ", "spacious ", "heavy " };
			}
			else if (size < HUGE)
			{
				if (Utils.Rand(3) == 0) return "generous amount of ass";
				options = new string[] { "expansive ", "voluminous " };
			}
			else if (size < INCONCEIVABLY_BIG)
			{
				if (Utils.Rand(3) == 0) return "jiggling expanse of ass";
				options = new string[] { "huge ", "vast " };
			}
			else //if (buttSize >= INCONCEIVABLY_BIG)
			{
				options = new string[] { "ginormous ", "colossal ", "tremendous " };
			}

			sb.Append(Utils.RandomChoice(options));
			options = new string[] { "butt ", "ass " };
			sb.Append(Utils.RandomChoice(options));
			if (Utils.RandBool()) sb.Append("cheeks");
			return sb.ToString();
		}
	}
}
