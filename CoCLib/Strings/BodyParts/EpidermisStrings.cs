//Class1.cs
//Description:
//Author: JustSomeGuy
//12/31/2018, 8:44 PM
using CoC.EpidermalColors;
using CoC.Tools;
using System;

namespace CoC.BodyParts
{
	internal static class EpidermisHelper
	{
		public static string AsString(this FurTexture furTexture)
		{
			switch (furTexture)
			{
				case FurTexture.MANGEY: return "mangey";
				case FurTexture.SHINY: return "shiny";
				case FurTexture.SMOOTH: return "smooth";
				case FurTexture.SOFT: return "soft";
				case FurTexture.NONDESCRIPT:
				default: return "";
			}
		}

		public static string AsString(this SkinTexture skinTexture)
		{
			switch (skinTexture)
			{
				case SkinTexture.SEXY: return "sexy";
				case SkinTexture.ROUGH: return "rough";
				case SkinTexture.FRECKLED: return "freckled";
				case SkinTexture.THICK: return "thick";
				case SkinTexture.SMOOTH: return "smooth";
				case SkinTexture.SHINY: return "shiny";
				case SkinTexture.SOFT: return "soft";
				case SkinTexture.NONDESCRIPT:
				default: return "";
			}
		}
	}
	internal partial class Epidermis
	{
		private static string fullStr(string adj, Tones tone, SimpleDescriptor descriptor)
		{
			return adj + (string.IsNullOrWhiteSpace(adj) ? "" : " ") + tone.AsString() + " " + descriptor();
		}

		private static string fullStr(string adj, FurColor fur, SimpleDescriptor descriptor)
		{
			return adj + (string.IsNullOrWhiteSpace(adj) ? "" : " ") + fur.AsString() + " " + descriptor();
		}


		private static string ColoredStr(FurColor color, SimpleDescriptor descriptor)
		{
			return color.AsString() + " " + descriptor();
		}
		private static string ColoredStr(Tones color, SimpleDescriptor descriptor)
		{
			return color.AsString() + " " + descriptor();
		}
	}

	internal partial class EpidermisType
	{
		private static string SkinStr()
		{
			return "skin";
		}

		private static string ScalesStr()
		{
			return "scales";
		}

		private static string FeathersStr()
		{
			return "feather";
		}

		private static string FurStr()
		{
			return "fur";
		}

		//Hard exoskeleton for things like a turtle or spiders or whatever.
		private static string CarapaceStr()
		{
			return "carapace";
		}

		private static string GooStr()
		{
			return "goo";
		}
		private static string WoolStr()
		{
			return "wool";
		}
		private static string BarkStr()
		{
			return "bark";
		}
		private static string ExoskeletonStr()
		{
			return "exoskeleton";
		}

		private static string RubberStr()
		{
			return "rubber-skin";
		}
	}
}
