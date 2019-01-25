//PiercingFlags.cs
//Description: Flags class added to creature. all piercing classes store a reference to this.
//Author: JustSomeGuy
//1/1/2019, 8:41 AM

namespace  CoC.BodyParts
{
	//place this in creature. 
	//all piercing classes are passed a reference to this when initialized. they use it to determine if the creature supports them
	//it's simpler than having them all aware of an event and reacting to it that way.
	internal class PiercingFlags
	{
		//disables piercings globally for this creature. probably useless unless we implement a Turn Off Piercings a la worms/watersports.
		public bool enabled;
		//enables more "kinky" piercings. granted to player via Ceraph piercing, definitely during bad end, but also if she's a follower and 
		//you're into that kind of thing.
		public bool piercingFetishEnabled;

		protected PiercingFlags()
		{
			enabled = true;
			piercingFetishEnabled = false;
		}

		public static PiercingFlags Generate()
		{
			return new PiercingFlags();
		}
	}
}
