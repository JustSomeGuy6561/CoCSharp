using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Items.Wearables.LowerGarment;
using CoC.Backend.Tools;

namespace CoC.Frontend.Items.Wearables.LowerGarment
{
	static class CommonGenericLowerGarments
	{
		public static Func<LowerGarmentBase> COMFORTABLE_LOINCLOTH => () => new GenericLowerGarment(Guid.Parse("bf5bfd93-f8e5-44d1-a275-d32f22161c8f"), COMFORTABLE_LOINCLOTHAbbr, COMFORTABLE_LOINCLOTHName, COMFORTABLE_LOINCLOTHDesc, COMFORTABLE_LOINCLOTHAbout, 0, 0, 6, true);
		public static Func<LowerGarmentBase> COMFORTABLE_PANTY => () => new GenericLowerGarment(Guid.Parse("80d24af3-4b7d-472b-a6db-7e46a2e97a15"), COMFORTABLE_PANTYAbbr, COMFORTABLE_PANTYName, COMFORTABLE_PANTYDesc, COMFORTABLE_PANTYAbout, 0, 0, 6, false);
		public static Func<LowerGarmentBase> COMFORTABLE_THONG => () => new GenericLowerGarment(Guid.Parse("bf5bfd93-f8e5-44d1-a275-d32f22161c8f"), COMFORTABLE_THONGAbbr, COMFORTABLE_THONGName, COMFORTABLE_THONGDesc, COMFORTABLE_THONGAbout, 0, 1, 6, false);
		public static Func<LowerGarmentBase> DRAGONSCALE_LOINCLOTH => () => new GenericLowerGarment(Guid.Parse("3071ab3d-8161-4aa9-9e24-1e09d611a9c4"), DRAGONSCALE_LOINCLOTHAbbr, DRAGONSCALE_LOINCLOTHName, DRAGONSCALE_LOINCLOTHDesc, DRAGONSCALE_LOINCLOTHAbout, 2, 1, 360, true);
		public static Func<LowerGarmentBase> DRAGONSCALE_THONG => () => new GenericLowerGarment(Guid.Parse("b7783fdb-c0a3-4266-a675-a7aec9660ef3"), DRAGONSCALE_THONGAbbr, DRAGONSCALE_THONGName, DRAGONSCALE_THONGDesc, DRAGONSCALE_THONGAbout, 2, 1, 360, false);
		public static Func<LowerGarmentBase> FUNDOSHI => () => new GenericLowerGarment(Guid.Parse("9ef4f36e-a882-4939-ac3d-bcac8b1522d3"), FUNDOSHIAbbr, FUNDOSHIName, FUNDOSHIDesc, FUNDOSHIAbout, 0, 2, 20, false);
		public static Func<LowerGarmentBase> FUR_LOINCLOTH => () => new GenericLowerGarment(Guid.Parse("52afbea5-9bfc-4d1d-8d53-4d24f0fc2caa"), FUR_LOINCLOTHAbbr, FUR_LOINCLOTHName, FUR_LOINCLOTHDesc, FUR_LOINCLOTHAbout, 0, 2, 6, true);
		public static Func<LowerGarmentBase> GARTERS => () => new GenericLowerGarment(Guid.Parse("cd3afcb9-9b1e-401b-b375-2e3c50a34c1a"), GARTERSAbbr, GARTERSName, GARTERSDesc, GARTERSAbout, 0, 3, 6, false);
		public static Func<LowerGarmentBase> LATEX_SHORTS => () => new GenericLowerGarment(Guid.Parse("48887392-85b1-4316-bbff-180e9a4f8ffd"), LATEX_SHORTSAbbr, LATEX_SHORTSName, LATEX_SHORTSDesc, LATEX_SHORTSAbout, 0, 3, 300, false);
		public static Func<LowerGarmentBase> LATEX_THONG => () => new GenericLowerGarment(Guid.Parse("8db387de-807a-4fd3-bbf1-8c9fa33f0d5f"), LATEX_THONGAbbr, LATEX_THONGName, LATEX_THONGDesc, LATEX_THONGAbout, 0, 3, 300, false);
		public static Func<LowerGarmentBase> SPIDERSILK_LOINCLOTH => () => new GenericLowerGarment(Guid.Parse("f5da0610-96f4-472a-b373-6bf77f98a537"), SPIDERSILK_LOINCLOTHAbbr, SPIDERSILK_LOINCLOTHName, SPIDERSILK_LOINCLOTHDesc, SPIDERSILK_LOINCLOTHAbout, 1, 1, 1000, true);
		public static Func<LowerGarmentBase> SPIDERSILK_PANTY => () => new GenericLowerGarment(Guid.Parse("10b06dfc-4664-4827-8e89-a093185c9209"), SPIDERSILK_PANTYAbbr, SPIDERSILK_PANTYName, SPIDERSILK_PANTYDesc, SPIDERSILK_PANTYAbout, 1, 1, 1000, false);
		public static Func<LowerGarmentBase> EBONWEAVE_JOCK => () => new GenericLowerGarment(Guid.Parse("f026f0be-b537-4f0e-94c3-45b3b17185a3"), EBONWEAVE_JOCKAbbr, EBONWEAVE_JOCKName, EBONWEAVE_JOCKDesc, EBONWEAVE_JOCKAbout, 3, 2, 900, false);
		public static Func<LowerGarmentBase> EBONWEAVE_THONG => () => new GenericLowerGarment(Guid.Parse("df182113-3220-4f36-a7f8-f60ae36bf4a7"), EBONWEAVE_THONGAbbr, EBONWEAVE_THONGName, EBONWEAVE_THONGDesc, EBONWEAVE_THONGAbout, 3, 2, 900, false);
		public static Func<LowerGarmentBase> EBONWEAVE_LOINCLOTH => () => new GenericLowerGarment(Guid.Parse("4a95f8e1-01a2-48d6-ab97-59c89b856808"), EBONWEAVE_LOINCLOTHAbbr, EBONWEAVE_LOINCLOTHName, EBONWEAVE_LOINCLOTHDesc, EBONWEAVE_LOINCLOTHAbout, 3, 2, 900, true);
		public static Func<LowerGarmentBase> EBONWEAVE_RUNIC_JOCKSTRAP => () => new GenericWellSpringAwareLowerGarment(Guid.Parse("bf1c3288-e175-4275-afd8-ba14fc0a70c8"), EBONWEAVE_RUNIC_JOCKSTRAPAbbr, EBONWEAVE_RUNIC_JOCKSTRAPName, EBONWEAVE_RUNIC_JOCKSTRAPDesc, EBONWEAVE_RUNIC_JOCKSTRAPAbout, 3, 3, 1200, false);
		public static Func<LowerGarmentBase> EBONWEAVE_RUNIC_THONG => () => new GenericWellSpringAwareLowerGarment(Guid.Parse("f502e504-255b-4064-ac14-19b1308a3860"), EBONWEAVE_RUNIC_THONGAbbr, EBONWEAVE_RUNIC_THONGName, EBONWEAVE_RUNIC_THONGDesc, EBONWEAVE_RUNIC_THONGAbout, 3, 3, 1200, false);
		public static Func<LowerGarmentBase> EBONWEAVE_RUNIC_LOINCLOTH => () => new GenericWellSpringAwareLowerGarment(Guid.Parse("8638d512-1c80-452e-b4a4-a99d987671a6"), EBONWEAVE_RUNIC_LOINCLOTHAbbr, EBONWEAVE_RUNIC_LOINCLOTHName, EBONWEAVE_RUNIC_LOINCLOTHDesc, EBONWEAVE_RUNIC_LOINCLOTHAbout, 3, 3, 1200, true);


		private static string COMFORTABLE_LOINCLOTHAbbr() => "C. Loin";
		private static string COMFORTABLE_LOINCLOTHName() => "comfortable loincloth";
		private static string COMFORTABLE_LOINCLOTHDesc(byte count, bool displayCount)
		{
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";

			if (count == 1)
			{

				return countText + "comfortable loincloth";
			}
			else
			{
				return countText + "pairs of comfortable loincloths";
			}
		}
		private static string COMFORTABLE_LOINCLOTHAbout() => "A generic loincloth.";



		private static string COMFORTABLE_PANTYAbbr() => "C.Panties";
		private static string COMFORTABLE_PANTYName() => "comfortable panties";
		private static string COMFORTABLE_PANTYDesc(byte count, bool displayCount)
		{
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";
			string pairText = count == 1 ? "pair" : "pairs";

			return $"{countText}{pairText} of comfortable panties";
		}
		private static string COMFORTABLE_PANTYAbout() => "A generic pair of panties.";

		private static string COMFORTABLE_THONGAbbr() => "C. Thong";
		private static string COMFORTABLE_THONGName() => "comfortable thong";
		private static string COMFORTABLE_THONGDesc(byte count, bool displayCount)
		{
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";

			if (count == 1)
			{

				return countText + "comfortable thong";
			}
			else
			{
				return countText + "pairs of comfortable thongs";
			}
		}
		private static string COMFORTABLE_THONGAbout() => "A generic thong.";



		private static string DRAGONSCALE_LOINCLOTHAbbr() => "D.Scale Loin";
		private static string DRAGONSCALE_LOINCLOTHName() => "dragonscale loincloth";
		private static string DRAGONSCALE_LOINCLOTHDesc(byte count, bool displayCount)
		{
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";

			if (count == 1)
			{

				return countText + "dragonscale loincloth";
			}
			else
			{
				return countText + "pairs of dragonscale loincloths";
			}
		}
		private static string DRAGONSCALE_LOINCLOTHAbout() => "This loincloth appears to be made of dragonscale and held together with a leather strap that goes around your waist. Great for those on the wild side!";


		private static string DRAGONSCALE_THONGAbbr() => "D.Scale Thong";
		private static string DRAGONSCALE_THONGName() => "dragonscale thong";
		private static string DRAGONSCALE_THONGDesc(byte count, bool displayCount)
		{
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";

			if (count == 1)
			{

				return countText + "dragonscale thong";
			}
			else
			{
				return countText + "pairs of dragonscale thongs";
			}
		}
		private static string DRAGONSCALE_THONGAbout() => "This thong appears to be made of dragonscale and held together with a leather strap that goes around your waist. Great for those on the wild side!";

		private static string FUNDOSHIAbbr() => "Fundoshi";
		private static string FUNDOSHIName() => "fundoshi";
		private static string FUNDOSHIDesc(byte count, bool displayCount)
		{
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";
			string pairText = count == 1 ? "fundoshi" : "fundoshis";

			return $"{countText}{pairText}";
		}
		private static string FUNDOSHIAbout() => "This Japanese-styled undergarment resembles a cross between a thong and a loincloth.";



		private static string FUR_LOINCLOTHAbbr() => "FurLoin";
		private static string FUR_LOINCLOTHName() => "fur loincloth";
		private static string FUR_LOINCLOTHDesc(byte count, bool displayCount)
		{
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";
			string setText = count == 1 ? "set" : "sets";

			return $"{countText}front and back {setText} of furry loincloths";
			//"a front and back set of loincloths"
		}
		private static string FUR_LOINCLOTHAbout() => "A pair of furry loincloths to cover your crotch and butt. Typically worn by people named 'Conan'. ";



		private static string GARTERSAbbr() => "Garters";
		private static string GARTERSName() => "stockings and garters";
		private static string GARTERSDesc(byte count, bool displayCount)
		{
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";
			string pairText = count == 1 ? "pair" : "pairs";

			return $"{countText}{pairText} of stockings and garters";
		}
		private static string GARTERSAbout() => "These pairs of stockings, garters and lingerie are perfect for seducing your partner!";



		private static string LATEX_SHORTSAbbr() => "LatexShorts";
		private static string LATEX_SHORTSName() => "latex shorts";
		private static string LATEX_SHORTSDesc(byte count, bool displayCount)
		{
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";
			string pairText = count == 1 ? "pair" : "pairs";

			return $"{countText}{pairText} of latex shorts";
		}
		private static string LATEX_SHORTSAbout() => "These shorts are black and shiny, obviously made of latex. It's designed to fit snugly against your form.";



		private static string LATEX_THONGAbbr() => "LatexThong";
		private static string LATEX_THONGName() => "latex thong";
		private static string LATEX_THONGDesc(byte count, bool displayCount)
		{
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";

			if (count == 1)
			{

				return countText + "latex thong";
			}
			else
			{
				return countText + "pairs of latex thongs";
			}
		}
		private static string LATEX_THONGAbout() => "This thong is black and shiny, obviously made of latex. It's designed to fit snugly against your form.";



		private static string SPIDERSILK_LOINCLOTHAbbr() => "S.Silk Loin";
		private static string SPIDERSILK_LOINCLOTHName() => "spider-silk loincloth";
		private static string SPIDERSILK_LOINCLOTHDesc(byte count, bool displayCount)
		{
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";

			if (count == 1)
			{

				return countText + "spider-silk loincloth";
			}
			else
			{
				return countText + "pairs of spider-silk loincloths";
			}
		}
		private static string SPIDERSILK_LOINCLOTHAbout() => "This loincloth looks incredibly comfortable. It's as white as snow and finely woven with hundreds of strands of spider silk.";



		private static string SPIDERSILK_PANTYAbbr() => "S.Silk Panty";
		private static string SPIDERSILK_PANTYName() => "spider-silk panties";
		private static string SPIDERSILK_PANTYDesc(byte count, bool displayCount)
		{
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";
			string pairText = count == 1 ? "pair" : "pairs";

			return $"{countText}{pairText} of spider-silk panties";
		}
		private static string SPIDERSILK_PANTYAbout() => "These panties look incredibly comfortable. It's as white as snow and finely woven with hundreds of strands of spider silk.";



		private static string EBONWEAVE_JOCKAbbr() => "Ebon Jock";
		private static string EBONWEAVE_JOCKName() => "Ebonweave jockstrap";
		private static string EBONWEAVE_JOCKDesc(byte count, bool displayCount)
		{
			string countText = displayCount ? (count == 1 ? "an " : Utils.NumberAsText(count) + " ") : "";

			if (count == 1)
			{

				return countText + "ebonweave jockstrap";
			}
			else
			{
				return countText + "pairs of ebonweave jockstraps";
			}
		}
		private static string EBONWEAVE_JOCKAbout() => "This jock is ebonweave, made of refined Ebonbloom petals. It's comfortable and elastic, providing support while containing assets of any size.";



		private static string EBONWEAVE_THONGAbbr() => "Ebon Thong";
		private static string EBONWEAVE_THONGName() => "Ebonweave thong";
		private static string EBONWEAVE_THONGDesc(byte count, bool displayCount)
		{
			string countText = displayCount ? (count == 1 ? "an " : Utils.NumberAsText(count) + " ") : "";

			if (count == 1)
			{

				return countText + "ebonweave thong";
			}
			else
			{
				return countText + "pairs of ebonweave thongs";
			}
		}
		private static string EBONWEAVE_THONGAbout() => "This thong is made of ebonweave, designed to fit snugly around your form. Thanks to alchemic treatments, it's elastic enough to hold assets of any size.";



		private static string EBONWEAVE_LOINCLOTHAbbr() => "Ebon Loin";
		private static string EBONWEAVE_LOINCLOTHName() => "Ebonweave loincloth";
		private static string EBONWEAVE_LOINCLOTHDesc(byte count, bool displayCount)
		{
			string countText = displayCount ? (count == 1 ? "an " : Utils.NumberAsText(count) + " ") : "";

			if (count == 1)
			{

				return countText + "ebonweave loincloth";
			}
			else
			{
				return countText + "pairs of ebonweave loincloths";
			}
		}
		private static string EBONWEAVE_LOINCLOTHAbout() => "This loincloth is made of ebonweave, designed to fit snugly around your form. Thanks to alchemic treatments, it's elastic enough to hold assets of any size.";




		private static string EBONWEAVE_RUNIC_JOCKSTRAPAbbr() => "Rune Jock";
		private static string EBONWEAVE_RUNIC_JOCKSTRAPName() => "runed Ebonweave jockstrap";
		private static string EBONWEAVE_RUNIC_JOCKSTRAPDesc(byte count, bool displayCount)
		{
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";

			if (count == 1)
			{

				return countText + "runed ebonweave jockstrap";
			}
			else
			{
				return countText + "pairs of runed ebonweave jockstraps";
			}
		}
		private static string EBONWEAVE_RUNIC_JOCKSTRAPAbout() => "This jock is ebonweave, made of refined Ebonbloom petals. Adorning the pouch is a rune of lust, glowing with dark magic.";



		private static string EBONWEAVE_RUNIC_THONGAbbr() => "Rune Thong";
		private static string EBONWEAVE_RUNIC_THONGName() => "runed Ebonweave thong";
		private static string EBONWEAVE_RUNIC_THONGDesc(byte count, bool displayCount)
		{
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";

			if (count == 1)
			{

				return countText + "runed ebonweave thong";
			}
			else
			{
				return countText + "pairs of runed ebonweave thongs";
			}
		}
		private static string EBONWEAVE_RUNIC_THONGAbout() => "This thong is made of ebonweave, designed to fit snugly around your form. Adorning the front is a rune of lust, glowing with dark magic.";


		private static string EBONWEAVE_RUNIC_LOINCLOTHAbbr() => "Rune Loin";
		private static string EBONWEAVE_RUNIC_LOINCLOTHName() => "runed Ebonweave loincloth";
		private static string EBONWEAVE_RUNIC_LOINCLOTHDesc(byte count, bool displayCount)
		{
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";

			if (count == 1)
			{

				return countText + "runed ebonweave loincloth";
			}
			else
			{
				return countText + "pairs of runed ebonweave loincloths";
			}
		}
		private static string EBONWEAVE_RUNIC_LOINCLOTHAbout() => "This loincloth is made of ebonweave, designed to fit snugly around your form. Adorning the front is a rune of lust, glowing with dark magic.";


	}
}



