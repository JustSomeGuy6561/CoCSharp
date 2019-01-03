//Class1.cs
//Description:
//Author: JustSomeGuy
//12/31/2018, 8:44 PM
using CoC.Items;
using CoC.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.Strings.BodyParts
{
	public static class EpidermisString
	{
		public static string ColoredStr(EpidermalColors color, GenericDescription descriptor)
		{
			return color.AsString();
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
			return "feathers";
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
	}
}
