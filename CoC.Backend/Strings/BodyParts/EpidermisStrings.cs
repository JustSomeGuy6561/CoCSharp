//EpidermisStrings.cs
//Description:
//Author: JustSomeGuy
//12/31/2018, 8:44 PM

using CoC.Backend.CoC_Colors;

namespace CoC.Backend.BodyParts
{
	public static class EpidermisHelper
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
	public partial class Epidermis
	{
		private static string fullStr(string adj, Tones tone, SimpleDescriptor descriptor, bool commaBeforeDescriptor = false)
		{
			string preDesc = commaBeforeDescriptor ? ", " : " ";
			return adj + (string.IsNullOrWhiteSpace(adj) ? "" : " ") + tone.AsString() + preDesc + descriptor();
		}

		private static string fullStr(string adj, FurColor fur, SimpleDescriptor descriptor, bool commaBeforeDescriptor = false)
		{
			string preDesc = commaBeforeDescriptor ? ", " : " ";
			return adj + (string.IsNullOrWhiteSpace(adj) ? "" : " ") + fur.AsString() + preDesc + descriptor();
		}


		private static string ColoredStr(FurColor color, SimpleDescriptor descriptor, bool commaBeforeDescriptor = false)
		{
			string preDesc = commaBeforeDescriptor ? ", " : " ";
			return color.AsString() + " " + descriptor();
		}
		private static string ColoredStr(Tones color, SimpleDescriptor descriptor, bool commaBeforeDescriptor = false)
		{
			string preDesc = commaBeforeDescriptor ? ", " : " ";
			return color.AsString() + " " + descriptor();
		}
	}

	public partial class EpidermisType
	{
		private static string SkinStr()
		{
			return "skin";
		}

		private static string SkinAdjectiveStr()
		{
			return "standard";
		}


		private static string ScalesStr()
		{
			return "scales";
		}

		private static string ScalesAdjectiveStr()
		{
			return "scaly";
		}


		private static string FeathersStr()
		{
			return "feathers";
		}

		private static string FeathersAdjectiveStr()
		{
			return "feathery";
		}


		private static string FurStr()
		{
			return "fur";
		}

		private static string FurAdjectiveStr()
		{
			return "furry";
		}


		//Hard exoskeleton for things like a turtle or spiders or whatever.
		private static string CarapaceStr()
		{
			return "carapace";
		}

		private static string CarapaceAdjectiveStr()
		{
			return "carapace-covered";
		}


		private static string GooStr()
		{
			return "goo";
		}

		private static string GooAdjectiveStr()
		{
			return "gooey";
		}

		private static string WoolStr()
		{
			return "wool";
		}

		private static string WoolAdjectiveStr()
		{
			return "woolen";
		}

		private static string BarkStr()
		{
			return "bark";
		}

		private static string BarkAdjectiveStr()
		{
			return "bark-covered";
		}

		private static string ExoskeletonStr()
		{
			return "exoskeleton";
		}

		private static string ExoskeletonAdjectiveStr()
		{
			return "exoskeleton-covering";
		}


		private static string RubberStr()
		{
			return "rubber-skin";
		}

		private static string RubberAdjectiveStr()
		{
			return "rubbery";
		}
	}
}
