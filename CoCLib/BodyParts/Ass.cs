//Ass.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 5:21 PM
using CoC.Creatures;
using CoC.Tools;

namespace CoC.BodyParts
{
	public enum AssLocation { BUTT, MOUTH, NOT_APPLICABLE }
	internal partial class Ass
	{
#warning Implement analwetness/looseness. add descriptors.
		public AssLocation assLocation { get; protected set; }

		protected Ass()
		{
			assLocation = AssLocation.BUTT;
		}

		public static Ass GenerateDefault()
		{
			return new Ass();
		}

		public static Ass Generate(AssLocation location)
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

		public string fullDescription()
		{
			return assFullDescription();
		}

		public string TypeAndPlayerDelegate(Player player)
		{
			return assPlayerStr(player);
		}
	}
}
