//FootStrings.cs
//Description:
//Author: JustSomeGuy
//1/26/2019, 6:30 AM

using CoC.Backend.Tools;

namespace CoC.Backend.BodyParts
{
	public partial class Feet
	{
		private static string Name()
		{
			return "Feet";
		}
	}

	public sealed partial class FootType
	{

		private static string HumanNoun(bool pluralIfApplicable, out bool isPlural)
		{
			isPlural = pluralIfApplicable;
			return pluralIfApplicable ? "feet" : "foot";
		}
		private static string HumanDesc(bool pluralIfApplicable, out bool isPlural)
		{
			return HumanNoun(pluralIfApplicable, out isPlural);
		}

		private static string HumanSingleDesc()
		{
			return "an ordinary foot";
		}

		private static string HumanLongDesc(FootData foot, bool alternateFormat, bool pluralIfApplicable, out bool isPlural)
		{
			if (!pluralIfApplicable && alternateFormat)
			{
				isPlural = false;
				return "a foot and toes";
			}
			else
			{
				return HumanDesc(pluralIfApplicable, out isPlural) + " and toes";
			}
		}

		private static string HoovesNoun(bool pluralIfApplicable, out bool isPlural)
		{
			isPlural = pluralIfApplicable;
			return pluralIfApplicable ? "hooves" : "hoof";
		}
		private static string HoovesDesc(bool pluralIfApplicable, out bool isPlural)
		{
			return HoovesNoun(pluralIfApplicable, out isPlural);
		}

		private static string HoovesSingleDesc()
		{
			return "a hoof";
		}

		private static string HoovesLongDesc(FootData foot, bool alternateFormat, bool pluralIfApplicable, out bool isPlural)
		{
			if (pluralIfApplicable && alternateFormat)
			{
				isPlural = false;
				return "a hoof";
			}
			else
			{
				return HoovesDesc(pluralIfApplicable, out isPlural);
			}
		}

		private static string PawNoun(bool pluralIfApplicable, out bool isPlural)
		{
			isPlural = pluralIfApplicable;
			return Utils.PluralizeIf("paw", pluralIfApplicable);
		}
		private static string PawDesc(bool pluralIfApplicable, out bool isPlural)
		{
			isPlural = pluralIfApplicable;
			return Utils.PluralizeIf("furry hindpaw", pluralIfApplicable);
		}
		private static string PawSingleDesc()
		{
			return "a furry hindpaw";
		}
		private static string PawLongDesc(FootData foot, bool alternateFormat, bool pluralIfApplicable, out bool isPlural)
		{
			if (alternateFormat && !pluralIfApplicable)
			{
				isPlural = false;
				return "a furry hindpaw with short claws";
			}
			else
			{
				return PawDesc(pluralIfApplicable, out isPlural);
			}
		}

		private static string GooNoun(bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GooDesc(bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GooSingleDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GooLongDesc(FootData foot, bool alternateFormat, bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string NagaNoun(bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NagaDesc(bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NagaSingleDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NagaLongDesc(FootData foot, bool alternateFormat, bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string DemonHeelNoun(bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DemonHeelDesc(bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DemonHeelSingleDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DemonHeelLongDesc(FootData foot, bool alternateFormat, bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string DemonClawNoun(bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DemonClawDesc(bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DemonClawSingleDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DemonClawLongDesc(FootData foot, bool alternateFormat, bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string InsectoidNoun(bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string InsectoidDesc(bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string InsectoidSingleDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string InsectoidLongDesc(FootData foot, bool alternateFormat, bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string LizardClawNoun(bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LizardClawDesc(bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LizardClawSingleDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LizardClawLongDesc(FootData foot, bool alternateFormat, bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string BronyNoun(bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BronyDesc(bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BronySingleDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BronyLongDesc(FootData foot, bool alternateFormat, bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string RabbitNoun(bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RabbitDesc(bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RabbitSingleDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RabbitLongDesc(FootData foot, bool alternateFormat, bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string HarpyTalonNoun(bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HarpyTalonDesc(bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HarpyTalonSingleDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HarpyTalonLongDesc(FootData foot, bool alternateFormat, bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string KangarooNoun(bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string KangarooDesc(bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string KangarooSingleDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string KangarooLongDesc(FootData foot, bool alternateFormat, bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string DragonClawNoun(bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DragonClawDesc(bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DragonClawSingleDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DragonClawLongDesc(FootData foot, bool alternateFormat, bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string ManderClawNoun(bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ManderClawDesc(bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ManderClawSingleDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ManderClawLongDesc(FootData foot, bool alternateFormat, bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string ImpClawNoun(bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ImpClawDesc(bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ImpClawSingleDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ImpClawLongDesc(FootData foot, bool alternateFormat, bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string TendrilNoun(bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string TendrilDesc(bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string TendrilSingleDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string TendrilLongDesc(FootData foot, bool alternateFormat, bool pluralIfApplicable, out bool isPlural)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string GenericClawedNoun(bool pluralIfApplicable, out bool isPlural)
		{
			isPlural = pluralIfApplicable;
			string footText = pluralIfApplicable ? "feet" : "foot";
			return "clawed " + footText;
		}

		private static string GenericPawedNoun(bool pluralIfApplicable, out bool isPlural)
		{
			isPlural = pluralIfApplicable;
			return Utils.PluralizeIf("hindpaw", pluralIfApplicable);
		}
	}
}
