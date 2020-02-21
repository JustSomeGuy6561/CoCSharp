using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Items.Wearables.UpperGarment;
using CoC.Backend.Tools;

namespace CoC.Frontend.Items.Wearables.UpperGarment
{
	static class CommonGenericUpperGarments
	{
		public static Func<UpperGarmentBase> COMFORTABLE_BRA => () => new GenericUpperGarment(Guid.Parse("238884fa-fa9a-4f85-9a27-a272b528a9bf"), COMFORTABLE_BRAAbbr, COMFORTABLE_BRAName, COMFORTABLE_BRADesc, COMFORTABLE_BRAAbout, 0, 0, 6);
		public static Func<UpperGarmentBase> DRAGONSCALE_BRA => () => new GenericUpperGarment(Guid.Parse("365d45a3-0a0a-45c0-99eb-d9fbc882ca3a"), DRAGONSCALE_BRAAbbr, DRAGONSCALE_BRAName, DRAGONSCALE_BRADesc, DRAGONSCALE_BRAAbout, 2, 1, 360);
		public static Func<UpperGarmentBase> LATEX_BRA => () => new GenericUpperGarment(Guid.Parse("f422e9e7-a902-4f13-8608-0c5dddea5992"), LATEX_BRAAbbr, LATEX_BRAName, LATEX_BRADesc, LATEX_BRAAbout, 0, 3, 250);
		public static Func<UpperGarmentBase> SPIDER_SILK_BRA => () => new GenericUpperGarment(Guid.Parse("d0f270e6-c469-46d9-8b70-3c144b87b884"), SPIDER_SILK_BRAAbbr, SPIDER_SILK_BRAName, SPIDER_SILK_BRADesc, SPIDER_SILK_BRAAbout, 1, 1, 1000);
		public static Func<UpperGarmentBase> EBONWEAVE_VEST => () => new GenericUpperGarment(Guid.Parse("fc73fbdf-97b2-44db-bd9d-d648227202b0"), EBONWEAVE_VESTAbbr, EBONWEAVE_VESTName, EBONWEAVE_VESTDesc, EBONWEAVE_VESTAbout, 3, 2, 900);
		public static Func<UpperGarmentBase> EBONWEAVE_CORSET => () => new GenericUpperGarment(Guid.Parse("30cc8e4b-c478-4f5c-b85a-67a994b53ff9"), EBONWEAVE_CORSETAbbr, EBONWEAVE_CORSETName, EBONWEAVE_CORSETDesc, EBONWEAVE_CORSETAbout, 3, 2, 900);

		private static string COMFORTABLE_BRAAbbr() => "C. Bra";
		private static string COMFORTABLE_BRAName() => "comfortable bra";
		private static string COMFORTABLE_BRADesc(byte count, bool displayCount)
		{
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";

			if (count == 1)
			{

				return countText + "comfortable bra";
			}
			else
			{
				return countText + "pairs of comfortable bras";
			}
		}
		private static string COMFORTABLE_BRAAbout() => "A generic pair of bra.";

		private static string DRAGONSCALE_BRAAbbr() => "D.Scale Bra";
		private static string DRAGONSCALE_BRAName() => "dragonscale bra";
		private static string DRAGONSCALE_BRADesc(byte count, bool displayCount)
		{
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";

			if (count == 1)
			{

				return countText + "dragonscale bra";
			}
			else
			{
				return countText + "pairs of dragonscale bras";
			}
		}
		private static string DRAGONSCALE_BRAAbout() => "This bra appears to be made of dragon scale. It's held together with leather straps for flexibility. Great for those on the primal side!";

		private static string LATEX_BRAAbbr() => "Latex Bra";
		private static string LATEX_BRAName() => "latex bra";
		private static string LATEX_BRADesc(byte count, bool displayCount)
		{
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";

			if (count == 1)
			{

				return countText + "latex bra";
			}
			else
			{
				return countText + "pairs of latex bras";
			}
		}
		private static string LATEX_BRAAbout() => "This bra is black and shiny, obviously made of latex. It's designed to fit snugly against your breasts.";

		private static string SPIDER_SILK_BRAAbbr() => "S.Silk Bra";
		private static string SPIDER_SILK_BRAName() => "spider-silk bra";
		private static string SPIDER_SILK_BRADesc(byte count, bool displayCount)
		{
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";

			if (count == 1)
			{

				return countText + "spider-silk bra";
			}
			else
			{
				return countText + "pairs of spider-silk bras";
			}
		}
		private static string SPIDER_SILK_BRAAbout() => "This bra looks incredibly comfortable. It's as white as snow and finely woven with hundreds of strands of spider silk.";

		private static string EBONWEAVE_VESTAbbr() => "EW Vest";
		private static string EBONWEAVE_VESTName() => "Ebonweave vest";
		private static string EBONWEAVE_VESTDesc(byte count, bool displayCount)
		{
			string countText = displayCount ? (count == 1 ? "an " : Utils.NumberAsText(count) + " ") : "";
			string vestText = count == 1 ? "vest" : "vests";

			return $"{countText}Ebonweave {vestText}";
		}
		private static string EBONWEAVE_VESTAbout() => "This vest is made of ebonweave, created using refined Ebonbloom petals. Elastic, form-fitting, and somewhat transparent, this comfortable vest will display your curves, masculine or feminine.";

		private static string EBONWEAVE_CORSETAbbr() => "EW Corset";
		private static string EBONWEAVE_CORSETName() => "Ebonweave corset";
		private static string EBONWEAVE_CORSETDesc(byte count, bool displayCount)
		{
			string countText = displayCount ? (count == 1 ? "an " : Utils.NumberAsText(count) + " ") : "";
			string corsetText = count == 1 ? "corset" : "corsets";

			return $"{countText}Ebonweave {corsetText}";
		}
		private static string EBONWEAVE_CORSETAbout() => "This corset is made of ebonweave, created using refined Ebonbloom petals. The ebonweave is elastic, making the corset surprisingly comfortable to wear, while displaying your bust down to the most subtle curves.";

	}
}
