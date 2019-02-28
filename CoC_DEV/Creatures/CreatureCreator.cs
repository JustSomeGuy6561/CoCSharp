using CoC.BodyParts;
using CoC.Tools;

namespace CoC.Creatures
{	
	/// <summary>
	/// Generic creature creator, generally used for species. it is a base class and can be extended for more specific examples (like the PC, for example). it provides a template for generating
	/// members of a given species or generic format. Note that this requires a default for male and female parts, so that these respective genders can be created.
	/// </summary>
	public class GenericCreatureCreator
	{
		public GenericCreatureCreator(Species race)
		{
			species = race;
		}

		//Body parts. set these up to be the defaults for the given species or character or whatever.
		public Antennae antennae;
		public Arms arms;
		public Back back;
		public Body body;
		public Ears ears;
		public Eyes eyes;
		public Gills gills;
		public Hair hair;
		public Horns horns;
		public LowerBody lowerBody;
		public Neck neck;
		public Tail tail;
		public Tongue tongue;
		public Wings wings;
		public FacialHair facialHair;
		public Face face;

		//genitals. These are used to define male/female/herm versions of the species
		public Breasts[] maleBreasts;
		public Cock[] cocks;
		public Balls balls;

		public Breasts[] femaleBreasts;
		public Vagina[] vaginas;

		public bool[] supportedGenders = new bool[4] { true, true, true, true };

		public readonly Species species;
	}
}
