//ButtHipStrings.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 3:05 AM
using CoC.BodyParts;
using CoC.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace CoC.Strings.BodyParts
{
	public static class AssButtHipStrings
	{
		public static string assfullDescription(Ass ass)
		{

		}
		public static string assTypeAndPlayerDelegate(Ass ass, Player player)
		{

		}

		public static string buttDescription(int buttSize)
		{

			if (buttSize == Butt.BUTTLESS)
			{
				return "buttless";
			}
			else if (buttSize < Butt.TIGHT)
			{
				return "very tight";
			}
			else if (buttSize < Butt.AVERAGE)
			{
				return "tight";
			}
			else if (buttSize < Butt.NOTICEABLE)
			{
				return "average";
			}
			else if (buttSize < Butt.LARGE)
			{
				return "noticable";
			}
			else if (buttSize < Butt.JIGGLY)
			{
				return "large";
			}
			else if (buttSize < Butt.EXPANSIVE)
			{
				return "jiggly";
			}
			else if (buttSize < Butt.HUGE)
			{
				return "expansive";
			}
			else if (buttSize < Butt.INCONCEIVABLY_BIG)
			{
				return "huge";
			}
			else
			{
				return "inconceivably big";
			}
		}

		public static string hipDescription(int hipSize)
		{
			if (hipSize <= Hips.BOYISH)
			{
				return "slim, masculine";
			}
			else if (hipSize < Hips.SLENDER)
			{
				return "boyish";
			}
			else if (hipSize < Hips.AVERAGE)
			{
				return "slender";
			}
			else if (hipSize < Hips.AMPLE)
			{
				return "average";
			}
			else if (hipSize < Hips.CURVY)
			{
				return "ample";
			}
			else if (hipSize < Hips.PUDGY)
			{
				return "curvy";
			}
			else if (hipSize < Hips.FERTILE)
			{
				return "very curvy, amost pudgy";
			}
			else if (hipSize < Hips.INHUMANLY_WIDE)
			{
				return "child-bearing";
			}
			else
			{
				return "broodmother level";
			}
		}
	}
}
