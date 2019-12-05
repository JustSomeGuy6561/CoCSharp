//EpidermisStrings.cs
//Description:
//Author: JustSomeGuy
//12/31/2018, 8:44 PM

using CoC.Backend.CoC_Colors;
using CoC.Backend.Strings;

namespace CoC.Backend.BodyParts
{
	public static class EpidermisHelper
	{
		public static string AsString(this FurTexture furTexture, bool withArticle = false)
		{
			if (!withArticle)
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
			else
			{
				switch (furTexture)
				{
					case FurTexture.MANGEY: return "a mangey";
					case FurTexture.SHINY: return "a shiny";
					case FurTexture.SMOOTH: return "a smooth";
					case FurTexture.SOFT: return "a soft";
					case FurTexture.NONDESCRIPT:
					default: return "";
				}
			}
		}

		public static string AsString(this SkinTexture skinTexture, bool withArticle = false)
		{
			if (!withArticle)
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
			else
			{
				switch (skinTexture)
				{
					case SkinTexture.SEXY: return "a sexy";
					case SkinTexture.ROUGH: return "a rough";
					case SkinTexture.FRECKLED: return "a freckled";
					case SkinTexture.THICK: return "a thick";
					case SkinTexture.SMOOTH: return "a smooth";
					case SkinTexture.SHINY: return "a shiny";
					case SkinTexture.SOFT: return "a soft";
					case SkinTexture.NONDESCRIPT:
					default: return "";
				}
			}
		}
	}
	public partial class Epidermis
	{
		
	}

	public partial class EpidermisType
	{
		private static string ArticleText(bool withArticle, bool startsWitVowel)
		{
			if (!withArticle)
			{
				return "";
			}
			else
			{
				return GlobalStrings.Article(startsWitVowel);
			}
		}
		#region Common Epidermis Related Strings
		internal static string JustTexture(IEpidermis epidermis, bool withArticle = false)
		{
			if (epidermis.epidermisType == EpidermisType.EMPTY) return "";
			else if (epidermis.epidermisType.usesTone) return epidermis.skinTexture.AsString(withArticle);
			else return epidermis.furTexture.AsString(withArticle);
		}

		internal static string JustColor(IEpidermis epidermis, bool withArticle = false)
		{
			if (epidermis.epidermisType == EpidermisType.EMPTY) return "";
			else if (epidermis.epidermisType.usesTone) return epidermis.tone.AsString(withArticle);
			else return epidermis.furColor.AsString(withArticle);
		}

		internal static string DescriptionWithColor(IEpidermis epidermis)
		{
			if (epidermis.epidermisType == EpidermisType.EMPTY) return "";
			else if (epidermis.epidermisType.usesTone) return ColoredStr(epidermis.tone, epidermis.epidermisType.ShortDescription(), false);
			else return ColoredStr(epidermis.furColor, epidermis.epidermisType.ShortDescription(), false);
		}

		internal static string DescriptionWithTexture(IEpidermis epidermis)
		{
			if (epidermis.epidermisType == EpidermisType.EMPTY) return "";
			else if (epidermis.epidermisType.usesTone) return TexturedStr(epidermis.skinTexture, epidermis.epidermisType.ShortDescription(), false);
			else return TexturedStr(epidermis.furTexture, epidermis.epidermisType.ShortDescription(), false);
		}

		internal static string DescriptionWithoutType(IEpidermis epidermis, bool withArticle)
		{
			if (epidermis.epidermisType == EpidermisType.EMPTY) return "";
			else if (epidermis.epidermisType.usesTone) return epidermis.skinTexture.AsString(withArticle) + epidermis.tone.AsString();
			else return epidermis.furTexture.AsString(withArticle) + epidermis.furColor.AsString();
		}

		internal static string LongDescription(IEpidermis epidermis)
		{
			if (epidermis.epidermisType == EMPTY) return "";
			else if (epidermis.epidermisType.usesTone) return fullStr(epidermis.skinTexture.AsString(), epidermis.tone, epidermis.epidermisType.ShortDescription());
			else return fullStr(epidermis.furTexture.AsString(), epidermis.furColor, epidermis.epidermisType.ShortDescription());
		}

		internal static string AdjectiveWithColor(IEpidermis epidermis, bool withArticle)
		{

			if (epidermis.epidermisType == EpidermisType.EMPTY) return "";
			else if (epidermis.epidermisType.usesTone) return ColoredStr(epidermis.tone, epidermis.epidermisType.AdjectiveDescription(), withArticle, true);
			else return ColoredStr(epidermis.furColor, epidermis.epidermisType.AdjectiveDescription(), withArticle, true);
		}

		internal static string AdjectiveWithTexture(IEpidermis epidermis, bool withArticle)
		{
			if (epidermis.epidermisType == EpidermisType.EMPTY) return "";
			else if (epidermis.epidermisType.usesTone) return TexturedStr(epidermis.skinTexture, epidermis.epidermisType.AdjectiveDescription(), withArticle, true);
			else return TexturedStr(epidermis.furTexture, epidermis.epidermisType.AdjectiveDescription(), withArticle, true);
		}

		internal static string AdjectiveDescriptionWithoutType(IEpidermis epidermis, bool withArticle)
		{
			//identical, afaik.
			return DescriptionWithoutType(epidermis, withArticle);
		}

		internal static string LongAdjectiveDescription(IEpidermis epidermis, bool withArticle)
		{
			if (epidermis.epidermisType == EpidermisType.EMPTY) return "";
			else if (epidermis.epidermisType.usesTone) return fullStr(epidermis.skinTexture.AsString(withArticle), epidermis.tone, epidermis.epidermisType.AdjectiveDescription(false), true);
			else return fullStr(epidermis.furTexture.AsString(withArticle), epidermis.furColor, epidermis.epidermisType.AdjectiveDescription(false), true);
		}


		internal static string DescriptionWith(IEpidermis epidermis, bool noTexture = false, bool noColor = false)
		{
			if (epidermis.epidermisType == EpidermisType.EMPTY) return "";
			else if (noTexture && noColor)
			{
				return epidermis.epidermisType.ShortDescription();
			}
			else if (noTexture)
			{
				return DescriptionWithColor(epidermis);
			}
			else if (noColor)
			{
				return DescriptionWithTexture(epidermis);
			}
			else
			{
				return LongDescription(epidermis);
			}
		}

		internal static string AdjectiveWith(IEpidermis epidermis, bool noTexture = false, bool noColor = false, bool withArticle = false)
		{
			if (epidermis.epidermisType == EpidermisType.EMPTY) return "";
			else if (noTexture && noColor)
			{
				return epidermis.epidermisType.AdjectiveDescription(withArticle);
			}
			else if (noTexture)
			{
				return AdjectiveWithColor(epidermis, withArticle);
			}
			else if (noColor)
			{
				return AdjectiveWithTexture(epidermis, withArticle);
			}
			else
			{
				return LongAdjectiveDescription(epidermis, withArticle);
			}
		}
		#endregion
		#region Helpers
		private static string fullStr(string adj, Tones tone, string descriptor, bool commaBeforeDescriptor = false)
		{
			string preDesc = commaBeforeDescriptor ? ", " : " ";
			return adj + (string.IsNullOrWhiteSpace(adj) ? "" : " ") + tone.AsString() + preDesc + descriptor;
		}

		private static string fullStr(string adj, FurColor fur, string descriptor, bool commaBeforeDescriptor = false)
		{
			string preDesc = commaBeforeDescriptor ? ", " : " ";
			return adj + (string.IsNullOrWhiteSpace(adj) ? "" : " ") + fur.AsString() + preDesc + descriptor;
		}


		private static string ColoredStr(FurColor color, string descriptor, bool withArticle, bool commaBeforeDescriptor = false)
		{
			string preDesc = commaBeforeDescriptor ? ", " : " ";
			return color.AsString(withArticle) + " " + descriptor;
		}
		private static string ColoredStr(Tones color, string descriptor, bool withArticle, bool commaBeforeDescriptor = false)
		{
			string preDesc = commaBeforeDescriptor ? ", " : " ";
			return color.AsString(withArticle) + " " + descriptor;
		}

		private static string TexturedStr(FurTexture texture, string descriptor, bool withArticle, bool commaBeforeDescriptor = false)
		{
			string preDesc = commaBeforeDescriptor ? ", " : " ";
			return texture.AsString(withArticle) + " " + descriptor;
		}
		private static string TexturedStr(SkinTexture texture, string descriptor, bool withArticle, bool commaBeforeDescriptor = false)
		{
			string preDesc = commaBeforeDescriptor ? ", " : " ";
			return texture.AsString(withArticle) + " " + descriptor;
		}
		#endregion


		private static string SkinStr()
		{
			return "skin";
		}

		private static string SkinAdjectiveStr(bool withArticle)
		{
			return ArticleText(withArticle, false) + "standard";
		}


		private static string ScalesStr()
		{
			return "scales";
		}

		private static string ScalesAdjectiveStr(bool withArticle)
		{
			return ArticleText(withArticle, false) + "scaly";
		}


		private static string FeathersStr()
		{
			return "feathers";
		}

		private static string FeathersAdjectiveStr(bool withArticle)
		{
			return ArticleText(withArticle, false) + "feathery";
		}


		private static string FurStr()
		{
			return "fur";
		}

		private static string FurAdjectiveStr(bool withArticle)
		{
			return ArticleText(withArticle, false) + "furry";
		}


		//Hard exoskeleton for things like a turtle or spiders or whatever.
		private static string CarapaceStr()
		{
			return "carapace";
		}

		private static string CarapaceAdjectiveStr(bool withArticle)
		{
			return ArticleText(withArticle, false) + "carapace-covered";
		}


		private static string GooStr()
		{
			return "goo";
		}

		private static string GooAdjectiveStr(bool withArticle)
		{
			return "gooey";
		}

		private static string WoolStr()
		{
			return "wool";
		}

		private static string WoolAdjectiveStr(bool withArticle)
		{
			return ArticleText(withArticle, false) + "woolen";
		}

		private static string BarkStr()
		{
			return "bark";
		}

		private static string BarkAdjectiveStr(bool withArticle)
		{
			return ArticleText(withArticle, false) + "bark-covered";
		}

		private static string ExoskeletonStr()
		{
			return "exoskeleton";
		}

		private static string ExoskeletonAdjectiveStr(bool withArticle)
		{
			return ArticleText(withArticle, true) + "exoskeleton-covered";
		}


		private static string RubberStr()
		{
			return "rubber-skin";
		}

		private static string RubberAdjectiveStr(bool withArticle)
		{
			return ArticleText(withArticle, false) + "rubbery";
		}
	}

	public partial class EpidermalData
	{
	}
}
