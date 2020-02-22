using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CoC.Backend.BodyParts
{
	public partial class GenitalTattooLocation
	{
		private static string LeftChestButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LeftChestLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LeftBreastButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LeftBreastLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LeftUnderBreastButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LeftUnderBreastLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LeftNippleButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LeftNippleLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightChestButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightChestLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightBreastButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightBreastLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightUnderBreastButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightUnderBreastLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightNippleButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightNippleLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ChestButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ChestLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GroinButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GroinLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CockButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CockLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VulvaButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VulvaLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string AssButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string AssLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FullButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FullLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
	}

	internal static class CommonGenitalStrings
	{
		internal static readonly string[] matchedPairOptions = new string[] { "pair of ", "two ", "brace of ", "matching ", "twin " };
		internal static readonly string[] mismatchedPairOptions = new string[] { "pair of ", "two ", "brace of " };
		internal static readonly string[] matchedTripleOptions = new string[]
		{
			"three ",
			"group of ",
			SafelyFormattedString.FormattedText("ménage à trois", StringFormats.ITALIC) + " of ",
			"triad of ",
			"triumvirate of "
		};
		internal static readonly string[] mismatchedTripleOptions = new string[] { "three ", "group of " };


	}

	partial class Genitals
	{

		public static string Name()
		{
			return "Genitals";
		}

		private string AllTattoosShort(Creature creature)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private string AllTattoosLong(Creature creature)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}




	}
}
