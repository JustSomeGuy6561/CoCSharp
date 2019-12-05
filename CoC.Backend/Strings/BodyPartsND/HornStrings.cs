//HornStrings.cs
//Description:
//Author: JustSomeGuy
//1/12/2019, 3:34 AM
using CoC.Backend.Creatures;
using CoC.Backend.Tools;

namespace CoC.Backend.BodyParts
{
	public partial class Horns
	{
		public static string Name()
		{
			return "Horns";
		}
	}

	public partial class HornType
	{
		private static string NoHornsShortDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NoHornsLongDesc(HornData horns)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NoHornsPlayerStr(Horns horns, PlayerBase player)
		{
			return "";
		}
		private static string NoHornsTransformStr(HornData previousHornData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NoHornsRestoreStr(HornData previousHornData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DemonShortDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DemonLongDesc(HornData horns)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DemonPlayerStr(Horns horns, PlayerBase player)
		{
			if (horns.numHorns <= 2)
				return " A small pair of pointed horns has broken through the " + player.face.facialSkin.LongDescription() + " on your forehead, " +
					"proclaiming some demonic taint to any who see them.";
			else if (horns.numHorns <= 4)
			{
				return " A quartet of prominent horns has broken through your " + player.face.facialSkin.LongDescription() + ". The back pair are longer, " +
					"and curve back along your head. The front pair protrude forward demonically.";
			}
			else if (horns.numHorns <= 6)
			{
				return " Six horns have sprouted through your " + player.face.facialSkin.LongDescription() + ", the back two pairs curve backwards over your head " +
					"and down towards your neck, while the front two horns stand almost " + Measurement.ToNearestSmallUnit(horns.significantHornSize, false, true) + " " +
					"long upwards and a little forward.";
			}
			else //if (horns.numHorns >= 8)
			{
				return " A large number of thick demonic horns sprout through your " + player.face.facialSkin.LongDescription() + ", each pair sprouting behind the ones before. " +
					"The front jut forwards nearly " + Measurement.ToNearestSmallUnit(horns.significantHornSize, false, true) + " while the rest curve back over your head, " +
					"some of the points ending just below your ears. You estimate you have a total of " + Utils.NumberAsText(horns.numHorns) + " horns.";
			}
		}
		private static string DemonTransformStr(HornData previousHornData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DemonRestoreStr(HornData previousHornData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BullShortDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BullLongDesc(HornData horns)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BullPlayerStr(Horns horns, PlayerBase player)
		{
			if (horns.significantHornSize < 3)
			{
				return " Two tiny horn-like nubs protrude from your forehead, resembling the horns of the young livestock kept by your village.";
			}
			else if (horns.significantHornSize < 6)
			{
				return " Two moderately sized horns grow from your forehead, similar in size to those on a young bovine.";
			}
			else if (horns.significantHornSize < 12)
			{
				return " Two large horns sprout from your forehead, curving forwards like those of a bull.";
			}
			else if (horns.significantHornSize < 20)
			{
				return " Two very large and dangerous looking horns sprout from your head, curving forward and over a foot long. They have dangerous looking points.";
			}
			else //if (horns.significantHornSize >= 20)
			{
				return " Two huge horns erupt from your forehead, curving outward at first, then forwards. The weight of them is heavy, and they end in dangerous looking points.";
			}
		}
		private static string BullTransformStr(HornData previousHornData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BullRestoreStr(HornData previousHornData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private string BullHornsReactToFemDeltaStr(byte oldLength, in byte newLength, byte oldMasculinity, in FemininityData femininity)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}


		private static string DragonShortDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DragonLongDesc(HornData horns)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DragonPlayerStr(Horns horns, PlayerBase player)
		{
			if (horns.numHorns < 4)
			{
				return " A pair of " + Measurement.ToNearestSmallUnit(horns.significantHornSize, false, true) + " horns grow from the sides of your head, " +
					"sweeping backwards and adding to your imposing visage.";
			}
			else
			{
				return " Two pairs of horns, roughly " + (Measurement.UsesMetric ? Measurement.ToNearestSmallUnit(horns.significantHornSize, false, true) : "a foot") +
					" long, sprout from the sides of your head. They sweep back and give you a fearsome look, almost like the dragons from your village's legends.";
			}
		}
		private static string DragonTransformStr(HornData previousHornData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DragonRestoreStr(HornData previousHornData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DeerShortDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DeerLongDesc(HornData horns)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DeerPlayerStr(Horns horns, PlayerBase player)
		{
			return horns.numHorns > 0 ? " Two antlers, forking into " + Utils.NumberAsText(horns.numHorns) + " points, have sprouted from the top of your head, " +
					"forming a spiky, regal crown of bone." : "";
		}
		private static string DeerTransformStr(HornData previousHornData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DeerRestoreStr(HornData previousHornData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ReindeerShortDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ReindeerLongDesc(HornData horns)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ReindeerPlayerStr(Horns horns, PlayerBase player)
		{
			return horns.numHorns > 0 ? " Two antlers, forking into " + Utils.NumberAsText(horns.numHorns) + " points, have sprouted from the top of your head, " +
					"forming a spiky, regal crown of bone. They are a bit wider and more pronounced than a standard deer's." : "";
		}
		private static string ReindeerTransformStr(HornData previousHornData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ReindeerRestoreStr(HornData previousHornData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GoatShortDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GoatLongDesc(HornData horns)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		//1-6 inches. 1-3: small. 4-6: large.
		private static string GoatPlayerStr(Horns horns, PlayerBase player)
		{
			if (horns.significantHornSize < 4)
			{
				return " A pair of stubby goat horns sprout from the sides of your head.";
			}
			else
			{
				return " A pair of tall-standing goat horns sprout from the sides of your head.  They are curved and patterned with ridges.";
			}
		}
		private static string GoatTransformStr(HornData previousHornData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GoatRestoreStr(HornData previousHornData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string UniHornShortDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string UniHornLongDesc(HornData horns)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string UniHornPlayerStr(Horns horns, PlayerBase player)
		{
			string intro = " A single sharp nub of a horn sprouts from the center of your forehead.";
			if (horns.significantHornSize < horns.type.maxHornLength)
			{
				return intro + " You estimate it to be about " + Measurement.ToNearestSmallUnit(horns.significantHornSize, false, true) + " long.";
			}
			else
			{
				return intro + " It has developed its own cute little spiral. You estimate it to be about " + Measurement.ToNearestSmallUnit(horns.significantHornSize, false, false) + 
					" long, " + Measurement.ToNearestSmallUnit(2, false, false) + " thick and very sturdy. A very useful natural weapon.";
			}
		}
		private static string UniHornTransformStr(HornData previousHornData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string UniHornRestoreStr(HornData previousHornData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RhinoShortDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RhinoLongDesc(HornData horns)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RhinoPlayerStr(Horns horns, PlayerBase player)
		{
			if (horns.numHorns >= horns.type.maxHorns)
			{
				return " You have a pair of large, sturdy horns, each conical in shape and distinctly rhino-like. The first reaches nearly " +
					Measurement.ToNearestSmallUnit(horns.significantHornSize, false, false) + "and sprouts from just above your nose, while second is higher up and roughly half that " +
					"length, roughly " + Measurement.ToNearestSmallUnit((horns.significantHornSize + 1) / 2, false, false) + ".";
			}
			else
			{
				return " A single horn sprouts from your forehead. It is conical and resembles a rhino's horn. You estimate it to be about " + 
					Measurement.ToNearestSmallUnit(horns.significantHornSize, false, true) + " long.";
			}
		}
		private static string RhinoTransformStr(HornData previousHornData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RhinoRestoreStr(HornData previousHornData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SheepShortDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SheepLongDesc(HornData horns)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SheepPlayerStr(Horns horns, PlayerBase player)
		{
			if (horns.significantHornSize < 4)
			{
				return " A pair of small sheep horns sit atop your head. They curl out and upwards in a slight crescent shape.";
			}
			else if (horns.significantHornSize < SheepHorns.maxFeminineLength)
			{
				return " A pair of large sheep horns sit atop your head. They curl out and upwards in a crescent shape.";
			}
			else if (horns.significantHornSize < 17) //arbitrary number, feel free to chang it.
			{
				return " A set of " + Measurement.ToNearestSmallUnit(horns.significantHornSize, false, true, false) + " ram horns sit atop your head, curling around " +
					"in a tight spiral at the side of your head before coming to an upwards hook around your ears.";
			}
			else
			{
				return " A large set of ram's horns sit atop your head, measuring at least " + Measurement.ToNearestSmallUnit(horns.significantHornSize, false, true) + " in length. " +
					"They curl around the side of your head in a tight spiral before coming to an end with an upwards hook around your ears. ";

			}
		}
		private static string SheepTransformStr(HornData previousHornData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SheepRestoreStr(HornData previousHornData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ImpShortDesc()
		{
			return "short, imp-like horns";
		}
		private static string ImpLongDesc(HornData horns)
		{
			return "a pair of short, imp-like horns";
		}
		private static string ImpPlayerStr(Horns horns, PlayerBase player)
		{
			return " A set of pointed imp horns rest atop your head.";
		}
		private static string ImpTransformStr(HornData previousHornData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ImpRestoreStr(HornData previousHornData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
	}
}
