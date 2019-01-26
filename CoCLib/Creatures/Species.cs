//SpeciesName.cs
//Description:
//Author: JustSomeGuy
//12/27/2018, 12:30 AM

using CoC.Tools;

namespace CoC.Creatures
{
	internal partial class Species
	{
		public string race => raceDescriptor();
		private SimpleDescriptor raceDescriptor;
		private Species(SimpleDescriptor name)
		{
			raceDescriptor = name;
		}

		public readonly Species ANEMONE = new Species(AnemoneStr);
		public readonly Species BASILISK = new Species(BasiliskStr);
		public readonly Species BEE = new Species(BeeStr);
		public readonly Species BEHEMOTH = new Species(BehemothStr);
		public readonly Species BOAR = new Species(BoarStr);
		public readonly Species BUNNY = new Species(BunnyStr);
		public readonly Species CAT = new Species(CatStr);
		public readonly Species CENTAUR = new Species(CentaurStr);
		public readonly Species COCKATRICE = new Species(CockatriceStr);
		public readonly Species COW = new Species(CowStr);
		public readonly Species DEER = new Species(DeerStr);
		public readonly Species DEMON = new Species(DemonStr);
		public readonly Species DOG = new Species(DogStr);
		public readonly Species DRAGON = new Species(DragonStr);
		public readonly Species DRIDER = new Species(DriderStr);
		public readonly Species ECHIDNA = new Species(EchidnaStr);
		public readonly Species ELFIN = new Species(ElfinStr);
		public readonly Species FERRET = new Species(FerretStr);
		public readonly Species FOX = new Species(FoxStr);
		public readonly Species GAZELLE = new Species(GazelleStr);
		public readonly Species GHOST = new Species(GhostStr);
		public readonly Species GOAT = new Species(GoatStr);
		public readonly Species GOO = new Species(GooStr);
		public readonly Species HARPY = new Species(HarpyStr);
		public readonly Species HORSE = new Species(HorseStr);
		public readonly Species HUMAN = new Species(HumanStr);
		public readonly Species IMP = new Species(ImpStr);
		public readonly Species KANGAROO = new Species(KangarooStr);
		public readonly Species LIZARD = new Species(LizardStr);
		public readonly Species MANTIS = new Species(MantisStr);
		public readonly Species MOUSE = new Species(MouseStr);
		public readonly Species NAGA = new Species(NagaStr);
		public readonly Species PIG = new Species(PigStr);
		public readonly Species PONY = new Species(PonyStr);
		public readonly Species PREDATOR = new Species(PredatorStr);
		public readonly Species RABBIT = new Species(RabbitStr);
		public readonly Species RACCOON = new Species(RaccoonStr);
		public readonly Species RAM = new Species(RamStr);
		public readonly Species RED_PANDA = new Species(RedPandaStr);
		public readonly Species REPTILE = new Species(ReptileStr);
		public readonly Species RHINO = new Species(RhinoStr);
		public readonly Species SALAMANDER = new Species(SalamanderStr);
		public readonly Species SAND_TRAP = new Species(SandTrapStr);
		public readonly Species SCORPION = new Species(ScorpionStr);
		public readonly Species SHARK = new Species(SharkStr);
		public readonly Species SHEEP = new Species(SheepStr);
		public readonly Species SNAKE = new Species(SnakeStr);
		public readonly Species SPIDER = new Species(SpiderStr);
		public readonly Species UNICORN = new Species(UnicornStr);
		public readonly Species WOLF = new Species(WolfStr);
	}


}
