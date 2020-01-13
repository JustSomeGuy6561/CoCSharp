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
		public static string AsString(this FurTexture furTexture, bool singularFormat = false)
		{
			if (!singularFormat)
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

		public static string AsString(this SkinTexture skinTexture, bool singularFormat = false)
		{
			if (!singularFormat)
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
		private static string ArticleText(bool singularFormat, bool startsWitVowel)
		{
			if (!singularFormat)
			{
				return "";
			}
			else
			{
				return GlobalStrings.Article(startsWitVowel);
			}
		}

		#region Common Epidermis Related Strings
		internal static string JustTexture(IEpidermis epidermis, bool singularFormat = false)
		{
			if (epidermis.epidermisType == EpidermisType.EMPTY) return "";
			else if (epidermis.epidermisType.usesTone) return epidermis.skinTexture.AsString(singularFormat);
			else return epidermis.furTexture.AsString(singularFormat);
		}

		internal static string JustColor(IEpidermis epidermis, bool singularFormat = false)
		{
			if (epidermis.epidermisType == EpidermisType.EMPTY) return "";
			else if (epidermis.epidermisType.usesTone) return epidermis.tone.AsString(singularFormat);
			else return epidermis.furColor.AsString(singularFormat);
		}

		internal static string DescriptionWithColor(IEpidermis epidermis, out bool isPlural)
		{
			return ColoredStr(epidermis, false, false, out isPlural);
		}

		internal static string DescriptionWithTexture(IEpidermis epidermis, out bool isPlural)
		{
			return TexturedStr(epidermis, false, false, out isPlural);
		}

		internal static string DescriptionWithoutType(IEpidermis epidermis, bool singularFormat)
		{
			if (epidermis.epidermisType == EpidermisType.EMPTY) return "";
			else if (epidermis.epidermisType.usesTone) return epidermis.skinTexture.AsString(singularFormat) + epidermis.tone.AsString();
			else return epidermis.furTexture.AsString(singularFormat) + epidermis.furColor.AsString();
		}

		internal static string LongDescription(IEpidermis epidermis, out bool isPlural)
		{
			return fullStr(epidermis, false, false, out isPlural);
		}

		internal static string AdjectiveWithColor(IEpidermis epidermis, bool singularFormat)
		{
			return ColoredStr(epidermis, singularFormat, true, out bool _);
		}

		internal static string AdjectiveWithTexture(IEpidermis epidermis, bool singularFormat)
		{
			return TexturedStr(epidermis, singularFormat, true, out bool _);
		}

		internal static string AdjectiveDescriptionWithoutType(IEpidermis epidermis, bool singularFormat)
		{
			//identical, afaik.
			return DescriptionWithoutType(epidermis, singularFormat);
		}

		internal static string LongAdjectiveDescription(IEpidermis epidermis, bool singularFormat)
		{
			return fullStr(epidermis, singularFormat, true, out bool _);
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
				return DescriptionWithColor(epidermis, out bool _);
			}
			else if (noColor)
			{
				return DescriptionWithTexture(epidermis, out bool _);
			}
			else
			{
				return LongDescription(epidermis, out bool _);
			}
		}

		internal static string DescriptionWith(IEpidermis epidermis, bool noTexture, bool noColor, out bool isPlural)
		{
			if (epidermis.epidermisType == EpidermisType.EMPTY)
			{
				isPlural = false;
				return "";
			}
			else if (noTexture && noColor)
			{
				return epidermis.epidermisType.ShortDescription(out isPlural);
			}
			else if (noTexture)
			{
				return DescriptionWithColor(epidermis, out isPlural);
			}
			else if (noColor)
			{
				return DescriptionWithTexture(epidermis, out isPlural);
			}
			else
			{
				return LongDescription(epidermis, out isPlural);
			}
		}

		internal static string AdjectiveWith(IEpidermis epidermis, bool noTexture = false, bool noColor = false, bool singularFormat = false)
		{
			if (epidermis.epidermisType == EpidermisType.EMPTY) return "";
			else if (noTexture && noColor)
			{
				return epidermis.epidermisType.AdjectiveDescription(singularFormat);
			}
			else if (noTexture)
			{
				return AdjectiveWithColor(epidermis, singularFormat);
			}
			else if (noColor)
			{
				return AdjectiveWithTexture(epidermis, singularFormat);
			}
			else
			{
				return LongAdjectiveDescription(epidermis, singularFormat);
			}
		}
		#endregion
		#region Helpers

		private static string fullStr(IEpidermis epidermis, bool singularFormat, bool adjectiveDesc, out bool isPlural)
		{
			string preDesc = adjectiveDesc ? ", " : " ";

			if (epidermis.epidermisType == EpidermisType.EMPTY)
			{
				isPlural = false;
				return "";
			}
			else if (epidermis.epidermisType.usesFurColor)
			{
				string adj = epidermis.furTexture.AsString(singularFormat);
				return adj + (string.IsNullOrWhiteSpace(adj) ? "" : preDesc) + epidermis.furColor.AsString() + preDesc + epidermis.epidermisType.ShortDescription(out isPlural);
			}
			else
			{
				string adj = epidermis.skinTexture.AsString(singularFormat);
				return adj + (string.IsNullOrWhiteSpace(adj) ? "" : preDesc) + epidermis.tone.AsString() + preDesc + epidermis.epidermisType.ShortDescription(out isPlural);
			}
		}

		private static string ColoredStr(IEpidermis epidermis, bool singularFormat, bool adjectiveDesc, out bool isPlural)
		{
			string preDesc = adjectiveDesc ? ", " : " ";

			if (epidermis.epidermisType == EpidermisType.EMPTY)
			{
				isPlural = false;
				return "";
			}
			else if (epidermis.epidermisType.usesFurColor)
			{
				return epidermis.furColor.AsString(singularFormat) + preDesc + epidermis.epidermisType.ShortDescription(out isPlural);
			}
			else
			{
				return epidermis.tone.AsString(singularFormat) + preDesc + epidermis.epidermisType.ShortDescription(out isPlural);
			}
		}

		private static string TexturedStr(IEpidermis epidermis, bool singularFormat, bool adjectiveDesc, out bool isPlural)
		{
			string preDesc = adjectiveDesc ? ", " : " ";

			if (epidermis.epidermisType == EpidermisType.EMPTY)
			{
				isPlural = false;
				return "";
			}
			else if (epidermis.epidermisType.usesFurColor)
			{
				string adj = epidermis.furTexture.AsString(singularFormat);
				return adj + (string.IsNullOrWhiteSpace(adj) ? "" : preDesc) + epidermis.epidermisType.ShortDescription(out isPlural);
			}
			else
			{
				string adj = epidermis.skinTexture.AsString(singularFormat);
				return adj + (string.IsNullOrWhiteSpace(adj) ? "" : preDesc) + epidermis.epidermisType.ShortDescription(out isPlural);
			}
		}
		#endregion

		protected static string NothingStr(out bool isPlural)
		{
			isPlural = false;
			return "";
		}

		protected static string BitOfNothingness()
		{
			return "";
		}

		protected static string NothingAdjectiveStr(bool singularFormat)
		{
			return "";
		}

		private static string SkinStr(out bool isPlural)
		{
			isPlural = false;
			return "skin";
		}

		private static string PieceOfSkin()
		{
			return "a bit of skin";
		}

		private static string SkinAdjectiveStr(bool singularFormat)
		{
			return ArticleText(singularFormat, false) + "standard";
		}


		private static string ScalesStr(out bool isPlural)
		{
			isPlural = true;
			return "scales";
		}

		private static string PieceOfScales()
		{
			return "a few scales";
		}

		private static string ScalesAdjectiveStr(bool singularFormat)
		{
			return ArticleText(singularFormat, false) + "scaly";
		}


		private static string FeathersStr(out bool isPlural)
		{
			isPlural = true;
			return "feathers";
		}

		private static string PieceOfFeathers()
		{
			return "a few feathers";
		}

		private static string FeathersAdjectiveStr(bool singularFormat)
		{
			return ArticleText(singularFormat, false) + "feathery";
		}


		private static string FurStr(out bool isPlural)
		{
			isPlural = false;
			return "fur";
		}

		private static string PieceOfFur()
		{
			return "a tuft of fur";
		}

		private static string FurAdjectiveStr(bool singularFormat)
		{
			return ArticleText(singularFormat, false) + "furry";
		}


		//Hard exoskeleton for things like a turtle or spiders or whatever.
		private static string CarapaceStr(out bool isPlural)
		{
			isPlural = false;
			return "carapace";
		}

		private static string PieceOfCarapace()
		{
			return "a pierce of carapace";
		}

		private static string CarapaceAdjectiveStr(bool singularFormat)
		{
			return ArticleText(singularFormat, false) + "carapace-covered";
		}


		private static string GooStr(out bool isPlural)
		{
			isPlural = false;
			return "goo";
		}

		private static string PieceOfGoo()
		{
			return "a bit of goo";
		}

		private static string GooAdjectiveStr(bool singularFormat)
		{
			return "gooey";
		}

		private static string WoolStr(out bool isPlural)
		{
			isPlural = false;
			return "wool";
		}

		private static string PieceOfWool()
		{
			return "a bit of wool";
		}

		private static string WoolAdjectiveStr(bool singularFormat)
		{
			return ArticleText(singularFormat, false) + "woolen";
		}

		private static string BarkStr(out bool isPlural)
		{
			isPlural = false;
			return "bark";
		}

		private static string PieceOfBark()
		{
			return "some bark";
		}

		private static string BarkAdjectiveStr(bool singularFormat)
		{
			return ArticleText(singularFormat, false) + "bark-covered";
		}
	}

	public partial class EpidermalData
	{
	}
}
