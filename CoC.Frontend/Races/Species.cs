//Species.cs
//Description:
//Author: JustSomeGuy
//12/27/2018, 12:30 AM

using CoC.Backend;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using System.Collections.Generic;
using System.Linq;

namespace CoC.Frontend.Races
{
	//in the event you want to randomly generate content, it may be helpful to have a means to generate a creature creator from a given species.
	//to do so, uncomment the abstract function, and do so for all species defined.
	public abstract partial class Species
	{

		//private static Dictionary<Species, GenericCreatureCreator> speciesCreator = new Dictionary<Species, GenericCreatureCreator>();
		private static int indexMaker = 0;

		public readonly int index;
		public string race => raceDescriptor();
		private SimpleDescriptor raceDescriptor;

		//if null, sets them to default values.
		protected Species(SimpleDescriptor name/*, GenericCreatureCreator creator*/)
		{
			index = indexMaker++;
			raceDescriptor = name;
			theSpecies.Add(index, this);
			//speciesCreator.Add(this, creator);
		}

		public static Species CurrentSpecies(Creature creature)
		{
			return theSpecies.Values.MaxItem(x => x.Score(creature));
		}

		//Returns an array of key-value pairs, which stores a species and the score for the given creature in that species, respectively. the results are stored from highest score
		//to lowest score. any species with a score of 0 (or less) is excluded.
		public static KeyValuePair<Species, int>[] AllSpeciesWithScores(Creature creature)
		{
			return theSpecies.Values.Select(x => new KeyValuePair<Species, int>(x, x.Score(creature))).Where(x => x.Value > 0).OrderByDescending(x => x.Value).ToArray();
		}

		private static Dictionary<int, Species> theSpecies = new Dictionary<int, Species>();

		public static Species Dereference(int index)
		{
			if (theSpecies.ContainsKey(index))
			{
				return theSpecies[index];
			}
			throw new System.ArgumentException("the integer does not match any valid species");
		}

		public abstract byte Score(Creature source);

		//generates a random CreatureCreator of the given species, using the gender parameter as the suggested gender.
		//it will respect that unless the species prohibits that gender (anemone are herms, goblins female, etc).
		//updates the gender value passed in to the actual gender of the creator. if useDefault is set, does not do any RNG
		//and instead generates a creator for the default of that species (i.e. fur color, genital size)
		//regardless, it will return the new creator.
		//internal abstract CharacterCreator GenerateRandomCreator(ref Gender gender, bool useDefault = false);

		public static readonly Anemone ANEMONE = new Anemone();
		public static readonly Basilisk BASILISK = new Basilisk();
		public static readonly Bee BEE = new Bee();
		public static readonly Behemoth BEHEMOTH = new Behemoth();
		public static readonly Bunny BUNNY = new Bunny();
		public static readonly Cat CAT = new Cat();
		public static readonly Satyr CENTAUR = new Satyr();
		public static readonly Cockatrice COCKATRICE = new Cockatrice();
		public static readonly Cow COW = new Cow();
		public static readonly Deer DEER = new Deer();
		public static readonly Demon DEMON = new Demon();
		public static readonly Dog DOG = new Dog();
		public static readonly Dragon DRAGON = new Dragon();
		public static readonly Dryad DRYAD = new Dryad();
		public static readonly Echidna ECHIDNA = new Echidna();
		public static readonly Ferret FERRET = new Ferret();
		public static readonly Fox FOX = new Fox();
		//public static readonly Gazelle GAZELLE = new Gazelle();
		public static readonly Ghost GHOST = new Ghost();
		public static readonly Goblin GOBLIN = new Goblin();
		public static readonly Goo GOO = new Goo();
		public static readonly Harpy HARPY = new Harpy();
		public static readonly Horse HORSE = new Horse();
		public static readonly Human HUMAN = new Human();
		public static readonly Imp IMP = new Imp();
		public static readonly Kangaroo KANGAROO = new Kangaroo();
		public static readonly Kitsune KITSUNE = new Kitsune();
		public static readonly Lizard LIZARD = new Lizard();
		public static readonly Manticore MANTICORE = new Manticore();
		public static readonly Mouse MOUSE = new Mouse();
		public static readonly Mutant MUTANT = new Mutant();
		public static readonly Naga NAGA = new Naga();
		public static readonly Pig PIG = new Pig();
		public static readonly Pony PONY = new Pony();
		public static readonly Raccoon RACCOON = new Raccoon();
		public static readonly RedPanda RED_PANDA = new RedPanda();
		public static readonly Rhino RHINO = new Rhino();
		public static readonly Salamander SALAMANDER = new Salamander();
		public static readonly SandTrap SAND_TRAP = new SandTrap();
		public static readonly Shark SHARK = new Shark();
		public static readonly Sheep SHEEP = new Sheep();
		public static readonly Siren SIREN = new Siren();
		public static readonly Spider SPIDER = new Spider();
		public static readonly Unicorn UNICORN = new Unicorn();
		public static readonly Wolf WOLF = new Wolf();
	}
}
