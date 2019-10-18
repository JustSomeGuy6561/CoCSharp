using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts
{
	partial class Genitals
	{
		public static string Name()
		{
			return "Genitals";
		}

		private string AllCocksShort()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		private string AllCocksFull()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		private string AllVaginasShort()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		private string AllVaginasFull()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		private string LactationSlowedDownDueToInactivity(bool becameFullThisPass, LactationStatus oldLevel)
		{
			int delta = oldLevel - lactationStatus;
			if (lactationStatus > LactationStatus.STRONG)
			{
				string hitFullString = becameFullThisPass ? ", though they're still very much in need of a milking" : ""; 
				return "\n<b>Your breasts feel somewhat lighter, though not much. It seems you aren't producing milk at such an ungodly rate anymore" + hitFullString + ".</b>\n";
			}
			else if (lactationStatus > LactationStatus.MODERATE)
			{
				string helperStr = delta > 1 ? "significantly " : "";
				string hitFullString = becameFullThisPass ? "Despite this, they're still very tender and probably should be milked soon." : "";

				return "\n<b>Your breasts feel " + helperStr + "lighter as your body's milk production starts to wind down."+ hitFullString + "</b>\n";
			}
			else if (lactationStatus > LactationStatus.LIGHT)
			{
				string helperStr = delta > 1 ? "significantly " : "";
				string hitFullString = becameFullThisPass ? "Despite this, they're still very tender and probably should be milked soon." : "";

				return "\n<b>Your breasts feel " + helperStr + "lighter as your body's milk production winds down." + hitFullString + "</b>\n";
			}
			else if (lactationStatus > LactationStatus.NOT_LACTATING)
			{
				string helperStr = delta > 1 ? "significantly, " : "";
				string hitFullString = becameFullThisPass ? "Despite this, they're still tender and probably should be milked soon." : "";
				return "\n<b>Your body's milk output drops " + helperStr + "down to what would be considered 'normal' for a pregnant woman."+ hitFullString +"</b>\n";
			}
			else
			{
				return "\n<b>Your body no longer produces any milk.</b>\n";
			}
		}
		private string LactationFullWarning()
		{
			return Environment.NewLine + "<b>Your " + breastRows[0].nipples.ShortDescription() + "s feel swollen and bloated, needing to be milked.</b>" + Environment.NewLine;
		}
	}
}
