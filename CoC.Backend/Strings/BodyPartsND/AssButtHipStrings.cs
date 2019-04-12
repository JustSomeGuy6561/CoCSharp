//ButtHipStrings.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 3:05 AM


using CoC.Backend.Tools;
using System.Text;

namespace CoC.Backend.BodyParts
{
	//public sealed partial class Ass
	//{
	//	private string assFullDescription()
	//	{

	//	}
	//	private string assPlayerStr(Player player)
	//	{

	//	}
	//}
	public sealed partial class Butt
	{

		private string toText()
		{

			if (buttSize == Butt.BUTTLESS)
			{
				return "buttless";
			}
			else if (buttSize < Butt.TIGHT)
			{
				return "very tight";
			}
			else if (buttSize < Butt.AVERAGE)
			{
				return "tight";
			}
			else if (buttSize < Butt.NOTICEABLE)
			{
				return "average";
			}
			else if (buttSize < Butt.LARGE)
			{
				return "noticable";
			}
			else if (buttSize < Butt.JIGGLY)
			{
				return "large";
			}
			else if (buttSize < Butt.EXPANSIVE)
			{
				return "jiggly";
			}
			else if (buttSize < Butt.HUGE)
			{
				return "expansive";
			}
			else if (buttSize < Butt.INCONCEIVABLY_BIG)
			{
				return "huge";
			}
			else
			{
				return "inconceivably big";
			}
		}

		private string shortDesc()
		{
			StringBuilder sb = new StringBuilder();
			string[] options;
			if (buttSize < TIGHT)
			{
				options = new string[] { "insignificant ", "very small " };
			}
			else if (buttSize < AVERAGE)
			{
				options = new string[] { "tight ", "firm ", "compact " };
			}
			else if (buttSize < NOTICEABLE)
			{
				options = new string[] { "regular ", "unremarkable " };
			}
			else if (buttSize < LARGE)
			{
				if (Utils.Rand(3) == 0) return "handful of ass";
				options = new string[] { "full ", "shapely " };
			}
			else if (buttSize < JIGGLY)
			{
				options = new string[] { "squeezable ", "large ", "substantial " };
			}
			else if (buttSize < EXPANSIVE)
			{
				options = new string[] { "jiggling ", "spacious ", "heavy " };
			}
			else if (buttSize < HUGE)
			{
				if (Utils.Rand(3) == 0) return "generous amount of ass";
				options = new string[] { "expansive ", "voluminous " };
			}
			else if (buttSize < INCONCEIVABLY_BIG)
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


		private string fullDesc(Frame frame)
		{
			StringBuilder sb = new StringBuilder();
			string[] options;
			if (buttSize < TIGHT)
			{
				if (frame.muscleTone >= Frame.TONE_SLIGHTLY_VISIBLE)
				{
					options = new string[] { "incredibly tight, perky " };
				}
				else
				{
					sb.Append(Utils.RandomChoice("tiny", "very small", "dainty"));
					options = new string[1];
					options[0] = " ";
					//Soft PC's buns!
					if (frame.muscleTone <= Frame.TONE_SOFT && Utils.Rand(3) == 0) options[0] = ", yet soft ";
				}
			}
			else if (buttSize < AVERAGE)
			{
				if (frame.muscleTone >= Frame.TONE_SLIGHTLY_VISIBLE)
				{
					options = new string[] { "perky, muscular ", "tight, toned ", "compact, muscular ", "tight ", "muscular, toned " };
				}
				//Nondescript
				else if (frame.muscleTone >= Frame.TONE_SOFT)
				{
					options = new string[] { "tight ", "firm ", "compact ", "petite " };
				}
				//FLABBAH
				else
				{
					options = new string[] { "small, heart-shaped ", "soft, compact ", "soft, heart-shaped ", "small, cushy ", "small ", "petite ", "snug ", };
				}
			}
			else if (buttSize < NOTICEABLE)
			{
				//TOIGHT LIKE A TIGER
				if (frame.muscleTone >= Frame.TONE_SLIGHTLY_VISIBLE)
				{
					options = new string[] { "nicely muscled ", "nice, toned ", "muscly ", "nice toned ", "toned ", "fair " };
				}
				//Nondescript
				else if (frame.muscleTone >= Frame.TONE_SOFT)
				{
					options = new string[] { "nice ", "fair " };
				}
				//FLABBAH
				else
				{
					options = new string[] { "nice, cushiony ", "soft ", "nicely-rounded, heart-shaped ", "cushy ", "soft, squeezable " };
				}
			}
			else if (buttSize < LARGE)
			{
				//TOIGHT LIKE A TIGER
				if (frame.muscleTone >= Frame.TONE_SLIGHTLY_VISIBLE)
				{
					options = new string[] { "full, toned ", "muscly handful of ", "shapely, toned ", "muscular, hand-filling ", "shapely, chiseled ", "full ", "chiseled " };
				}
				//Nondescript
				else if (frame.muscleTone >= Frame.TONE_SOFT)
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
			else if (buttSize < JIGGLY)
			{
				//TOIGHT LIKE A TIGER
				if (frame.muscleTone >= Frame.TONE_SLIGHTLY_VISIBLE)
				{
					options = new string[] { "large, muscular ", "substantial, toned ", "big-but-tight ", "squeezable, toned ", "large, brawny ", "big-but-fit ", "powerful, squeezable ", "large " };
				}
				//Nondescript
				else if (frame.muscleTone >= Frame.TONE_SOFT)
				{
					options = new string[] { "squeezable ", "large ", "substantial " };
				}
				//FLABBAH
				else
				{
					options = new string[] { "large, bouncy ", "soft, eye-catching ", "big, slappable ", "soft, pinchable ", "large, plush ", "squeezable ", "cushiony ", "plush ", "pleasantly plump " };
				}
			}
			else if (buttSize < EXPANSIVE)
			{
				//TOIGHT LIKE A TIGER
				if (frame.muscleTone >= Frame.TONE_SLIGHTLY_VISIBLE)
				{
					options = new string[] { "thick, muscular ", "big, burly ", "heavy, powerful ", "spacious, muscular ", "toned, cloth-straining ", "thick ", "thick, strong " };
				}
				//Nondescript
				else if (frame.muscleTone >= Frame.TONE_SOFT)
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
			else if (buttSize < HUGE)
			{
				//TOIGHT LIKE A TIGER
				if (frame.muscleTone >= Frame.TONE_SLIGHTLY_VISIBLE)
				{
					options = new string[] { "expansive, muscled ", "voluminous, rippling ", "generous, powerful ",
						"big, burly ", "well-built, voluminous ", "powerful ", "muscular ", "powerful, expansive " };
				}
				//Nondescript
				else if (frame.muscleTone >= Frame.TONE_SOFT)
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
			else if (buttSize < INCONCEIVABLY_BIG)
			{
				if (frame.muscleTone >= Frame.TONE_SLIGHTLY_VISIBLE)
				{
					options = new string[] { "huge, toned ", "vast, muscular ", "vast, well-built ", "huge, muscular ", "strong, immense ", "muscle-bound " };
				}
				//Nondescript
				else if (frame.muscleTone >= Frame.TONE_SOFT)
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
			else //if (buttSize >= INCONCEIVABLY_BIG)
			{
				if (frame.muscleTone >= Frame.TONE_SLIGHTLY_VISIBLE)
				{
					if (Utils.Rand(7) == 0) return "colossal, muscly ass";
					options = new string[] { "ginormous, muscle-bound ", "colossal yet toned ", "strong, tremendously large ", "tremendous, muscled ", "ginormous, toned ", "colossal, well-defined " };
				}
				//Nondescript
				else if (frame.muscleTone >= Frame.TONE_SOFT)
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
	}
	public sealed partial class Hips
	{
		private string AsText()
		{
			if (hipSize <= Hips.BOYISH)
			{
				return "slim, masculine";
			}
			else if (hipSize < Hips.SLENDER)
			{
				return "boyish";
			}
			else if (hipSize < Hips.AVERAGE)
			{
				return "slender";
			}
			else if (hipSize < Hips.AMPLE)
			{
				return "average";
			}
			else if (hipSize < Hips.CURVY)
			{
				return "ample";
			}
			else if (hipSize < Hips.PUDGY)
			{
				return "curvy";
			}
			else if (hipSize < Hips.FERTILE)
			{
				return "very curvy, amost pudgy";
			}
			else if (hipSize < Hips.INHUMANLY_WIDE)
			{
				return "child-bearing";
			}
			else
			{
				return "broodmother level";
			}
		}

		private string Description(LowerBodyType lowerBody, Frame frame)
		{
			BodyType bodyType = bodyData().bodyType;

			StringBuilder sb = new StringBuilder();
			if (hipSize < SLENDER)
			{
				sb.Append(Utils.RandomChoice("tiny ", "narrow ", "boyish "));
			}
			else if (hipSize < AVERAGE)
			{
				sb.Append(Utils.RandomChoice("slender ", "narrow ", "thin "));
				if (frame.thickness < 30)
				{
					sb.Append(Utils.RandomChoice("slightly-flared ", "curved "));
				}
			}
			else if (hipSize < AMPLE)
			{
				sb.Append(Utils.RandomChoice("well-formed ", "pleasant "));
				if (frame.thickness < 30)
				{
					sb.Append(Utils.RandomChoice("flared ", "curvy "));
				}
			}
			else if (hipSize < CURVY)
			{
				sb.Append(Utils.RandomChoice("ample ", "noticeable ", "girly "));
				if (frame.thickness < 30)
				{
					sb.Append(Utils.RandomChoice("flared ", "waspish "));
				}
			}
			//we just skip pudgy. ok.
			else if (hipSize < FERTILE)
			{
				sb.Append(Utils.RandomChoice("flared ", "curvy ", "wide "));
				if (frame.thickness < 30)
				{
					sb.Append(Utils.RandomChoice("flared ", "waspish "));
				}
			}
			else if (hipSize < INHUMANLY_WIDE)
			{
				if (frame.thickness < 40)
				{
					sb.Append(Utils.RandomChoice("flared, ", "waspish, "));
				}
				sb.Append(Utils.RandomChoice("fertile ", "child-bearing ", "voluptuous "));
			}
			else //if (hipSize >= INHUMANLY_WIDE)
			{
				if (frame.thickness < 40)
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
