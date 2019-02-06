//Species.cs
//Description:
//Author: JustSomeGuy
//12/27/2018, 12:30 AM

using CoC.Tools;
using System.Collections.Generic;

namespace CoC.Creatures
{
	internal partial class Species
	{
		private static Dictionary<int, Species> theSpecies = new Dictionary<int,Species>();
		private static int indexMaker = 0;

		public readonly int index;
		public string race => raceDescriptor();
		private SimpleDescriptor raceDescriptor;
		private Species(SimpleDescriptor name, int defaultHeight)
		{
			index = indexMaker++;
			raceDescriptor = name;
			theSpecies.Add(index, this);
		}

		public static Species Dereference(int index)
		{
			if (theSpecies.ContainsKey(index))
			{
				return theSpecies[index];
			}
			throw new System.ArgumentException("the integer does not match any valid species");
		}

		public static readonly Species ANEMONE = new Species(AnemoneStr, );
		public static readonly Species BASILISK = new Species(BasiliskStr);
		public static readonly Species BEE = new Species(BeeStr);
		public static readonly Species BEHEMOTH = new Species(BehemothStr);
		public static readonly Species BOAR = new Species(BoarStr);
		public static readonly Species BUNNY = new Species(BunnyStr);
		public static readonly Species CAT = new Species(CatStr);
		public static readonly Species CENTAUR = new Species(CentaurStr);
		public static readonly Species COCKATRICE = new Species(CockatriceStr);
		public static readonly Species COW = new Species(CowStr);
		public static readonly Species DEER = new Species(DeerStr);
		public static readonly Species DEMON = new Species(DemonStr);
		public static readonly Species DOG = new Species(DogStr);
		public static readonly Species DRAGON = new Species(DragonStr);
		public static readonly Species DRIDER = new Species(DriderStr);
		public static readonly Species ECHIDNA = new Species(EchidnaStr);
		public static readonly Species ELFIN = new Species(ElfinStr);
		public static readonly Species FERRET = new Species(FerretStr);
		public static readonly Species FOX = new Species(FoxStr);
		public static readonly Species GAZELLE = new Species(GazelleStr);
		public static readonly Species GHOST = new Species(GhostStr);
		public static readonly Species GOAT = new Species(GoatStr);
		public static readonly Species GOO = new Species(GooStr);
		public static readonly Species HARPY = new Species(HarpyStr);
		public static readonly Species HORSE = new Species(HorseStr);
		public static readonly Species HUMAN = new Species(HumanStr);
		public static readonly Species IMP = new Species(ImpStr);
		public static readonly Species KANGAROO = new Species(KangarooStr);
		public static readonly Species LIZARD = new Species(LizardStr);
		public static readonly Species MANTIS = new Species(MantisStr);
		public static readonly Species MOUSE = new Species(MouseStr);
		public static readonly Species NAGA = new Species(NagaStr);
		public static readonly Species PIG = new Species(PigStr);
		public static readonly Species PONY = new Species(PonyStr);
		public static readonly Species PREDATOR = new Species(PredatorStr);
		public static readonly Species RABBIT = new Species(RabbitStr);
		public static readonly Species RACCOON = new Species(RaccoonStr);
		public static readonly Species RAM = new Species(RamStr);
		public static readonly Species RED_PANDA = new Species(RedPandaStr);
		public static readonly Species REPTILE = new Species(ReptileStr);
		public static readonly Species RHINO = new Species(RhinoStr);
		public static readonly Species SALAMANDER = new Species(SalamanderStr);
		public static readonly Species SAND_TRAP = new Species(SandTrapStr);
		public static readonly Species SCORPION = new Species(ScorpionStr);
		public static readonly Species SHARK = new Species(SharkStr);
		public static readonly Species SHEEP = new Species(SheepStr);
		public static readonly Species SNAKE = new Species(SnakeStr);
		public static readonly Species SPIDER = new Species(SpiderStr);
		public static readonly Species UNICORN = new Species(UnicornStr);
		public static readonly Species WOLF = new Species(WolfStr);
	}


}
