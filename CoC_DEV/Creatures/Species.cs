//Species.cs
//Description:
//Author: JustSomeGuy
//12/27/2018, 12:30 AM

using CoC.BodyParts;
using CoC.EpidermalColors;
using CoC.Tools;
using System.Collections.Generic;

namespace CoC.Creatures
{
	//in the event you want to randomly generate content, it may be helpful to have a default for each species, instead of requiring these to be generated
	//manually per monster group/whatever. i began to implement this, but decided it wasn't worth the effort, plus it's difficult for species that have different
	//rules for male/female (dragon/basilisk) or don't allow certain genders (goblins in this game are all female. imps are all male. anemone are all herms).
	//weirder still are demons - male : incubus, female: succubus, herm: omnibus
	internal abstract partial class Species
	{
		private static Dictionary<int, Species> theSpecies = new Dictionary<int, Species>();
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

		public static Species Dereference(int index)
		{
			if (theSpecies.ContainsKey(index))
			{
				return theSpecies[index];
			}
			throw new System.ArgumentException("the integer does not match any valid species");
		}

		//public GenericCreatureCreator GetCreator()
		//{
		//	if (speciesCreator.ContainsKey(this))
		//	{
		//		return speciesCreator[this];
		//	}
		//	throw new System.Exception("This species is missing from the list of registered species. Realistically, this should never occur.");
		//}

		//public static GenericCreatureCreator GetCreator(Species species)
		//{
		//	return species?.GetCreator();
		//}

		//public static readonly Species ANEMONE = new Species(AnemoneStr, ANEMONE_CREATOR);
		public static readonly Anemone ANEMONE = new Anemone();
		//public static readonly Species BASILISK = new Species(BasiliskStr, BASILISK_CREATOR);
		public static readonly Basilisk BASILISK = new Basilisk();
		public static readonly Species BEE = new Species(BeeStr, tone: Tones.BLACK, fur:, hair:HairFurColors.BLACK);
		public static readonly Species BEHEMOTH = new Species(BehemothStr, tone: Tones.RED, fur:FurColor.Generate(HairFurColors.RED), hair:HairFurColors.BLACK);
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
		//public static readonly Species WOLF = new Species(WolfStr, WOLF_CREATOR);
		public static readonly Species WOLF = new Species(WolfStr);

		//private static readonly GenericCreatureCreator WOLF_CREATOR = new GenericCreatureCreator(WOLF)
		//{
		//	arms = Arms.GenerateDefaultOfType(ArmType.WOLF),
		//	body = Body.GenerateFurredNoUnderbody(BodyType.SIMPLE_FUR, WOLF.DefaultFur),
		//	ears = Ears.GenerateDefaultOfType(EarType.WOLF),
		//	eyes = Eyes.GenerateNonHumanEyes(EyeType.WOLF, EYE_COLOR.BROWN, EYE_COLOR.BROWN),
		//	face = Face.GenerateNonStandardFace(FaceType.WOLF, true),
		//	lowerBody = LowerBody.GenerateDefaultOfType(LowerBodyType.WOLF),
		//	tail = Tail.GenerateDefaultOfType(TailType.WOLF),
		//	cocks = new Cock[] { Cock.Generate(CockType.WOLF, 10, 2) },
		//	balls = Balls.GenerateBalls(),
		//	femaleBreasts = new Breasts[] { Breasts.GenerateFemale(CupSize.C, 0.5f), Breasts.GenerateFemale(CupSize.B, 0.5f), Breasts.GenerateFemale(CupSize.A, 0.5f), Breasts.GenerateFemale(CupSize.A, 0.5f) },
		//};

		//private static readonly GenericCreatureCreator ANEMONE_CREATOR = new GenericCreatureCreator(ANEMONE)
		//{
		//	body = Body.GenerateFurredNoUnderbody(BodyType.HUMANOID, ANEMONE.DefaultTone),
		//	hair = Hair.Generate(HairType.ANEMONE, 10.0f, ANEMONE.DefaultHair),

		//	cocks = new Cock[] { Cock.Generate(CockType.ANEMONE, 7f, 1.25f) },
		//	balls = Balls.GenerateDefault(Gender.FEMALE),
		//	//default vag, breasts.
		//	supportedGenders = new bool[4] { false, false, false, true }
		//};

		//private static readonly GenericCreatureCreator BASILISK_CREATOR = new GenericCreatureCreator(BASILISK)
		//{
		//	arms = Arms.GenerateDefaultOfType(ArmType.LIZARD),
		//	body = Body.GenerateToneWithUnderbody(BodyType.REPTILIAN, BASILISK.DefaultPrimaryTone, BASILISK.DefaultUndertone),
		//	ears = Ears.GenerateDefaultOfType(EarType.LIZARD),
		//	eyes = Eyes.GenerateNonHumanEyes(EyeType.BASILISK, EYE_COLOR.BLUE, EYE_COLOR.BLUE),
		//	face = Face.GenerateNonStandardFace(FaceType.LIZARD, true),
		//	hair = Hair.GenerateNoHair(),
		//	lowerBody = LowerBody.GenerateDefaultOfType(LowerBodyType.LIZARD),
		//	tongue = Tongue.Generate(TongueType.LIZARD),
		//	tail = Tail.GenerateDefaultOfType(TailType.LIZARD),
		//	//default vag, breasts.
		//	cocks = new Cock[] { Cock.Generate(CockType.LIZARD) },
		//	balls = Balls.GenerateBalls()
		//};

		//private static readonly GenericCreatureCreator  
	}


}
