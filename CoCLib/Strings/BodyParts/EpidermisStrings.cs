//Class1.cs
//Description:
//Author: JustSomeGuy
//12/31/2018, 8:44 PM
using CoC.EpidermalColors;
using CoC.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace   CoC.BodyParts
{
	public static class EpidermisString
	{

		public static string fullStr(string adj, Tones tone, SimpleDescriptor descriptor)
		{
			return adj + (String.IsNullOrWhiteSpace(adj) ? "" : " ") + tone.AsString() + " " + descriptor();
		}

		public static string fullStr(string adj, FurColor fur, SimpleDescriptor descriptor)
		{
			return adj + (String.IsNullOrWhiteSpace(adj) ? "" : " ") + fur.AsString() + " " + descriptor();
		}


		public static string ColoredStr(FurColor color, SimpleDescriptor descriptor)
		{
			return color.AsString() + " " + descriptor();
		}
		public static string ColoredStr(Tones color, SimpleDescriptor descriptor)
		{
			return color.AsString() + " " + descriptor();
		}
		public static string SkinStr()
		{
			return "skin";
		}

		public static string ScalesStr()
		{
			return "scales";
		}

		public static string FeathersStr()
		{
			return "feather";
		}

		public static string FurStr()
		{
			return "fur";
		}

		//Hard exoskeleton for things like a turtle or spiders or whatever.
		public static string CarapaceStr()
		{
			return "carapace";
		}

		public static string GooStr()
		{
			return "goo";
		}
		public static string WoolStr()
		{
			return "wool";
		}
		public static string BarkStr()
		{
			return "bark";
		}
		public static string ExoskeletonStr()
		{
			return "exoskeleton";
		}

		public static string RubberStr()
		{
			return "rubber-skin";
		}
	}
}
