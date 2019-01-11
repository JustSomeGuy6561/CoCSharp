//Ass.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 5:21 PM
using CoC.Strings.BodyParts;
using CoC.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.BodyParts
{
	public enum AssLocation { BUTT, MOUTH, NOT_APPLICABLE }
	class Ass
	{
		public AssLocation assLocation { get; protected set; }

		protected Ass()
		{
			assLocation = AssLocation.BUTT;
		}

		public static Ass GenerateNormalAssHole()
		{
			return new Ass();
		}

		public static Ass GenerateAbnormalAssHole(AssLocation location)
		{
			return new Ass()
			{
				assLocation = location
			};
		}

		public bool Restore()
		{
			if (assLocation == AssLocation.BUTT)
			{
				return false;
			}
			assLocation = AssLocation.BUTT;
			return true;
		}

		public bool UpdateAssLocation(AssLocation location)
		{
			if (assLocation == location)
			{
				return false;
			}
			assLocation = location;
			return true;
		}

		public string fullDescription(Gender gender)
		{
			return AssButtHipStrings.assfullDescription(this, gender);
		}

		public string PlayerDescription(Player player)
		{
			return AssButtHipStrings.assPlayerDescription(this, player);
		}
	}
}
