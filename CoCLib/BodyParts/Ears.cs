//Ears.cs
//Description:
//Author: JustSomeGuy
//12/27/2018, 12:22 AM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoC.BodyParts.SpecialInteraction;
using CoC.EpidermalColors;
using CoC.Tools;
using static CoC.Strings.BodyParts.EarsStrings;
using static CoC.UI.TextOutput;
namespace CoC.BodyParts
{
	/*
	 * Here's to looking half of this shit up. Following your ear from bottom up around the edge:
	 * LOBE, AURICLE, HELIX, ANTI_HELIX.
	 * LOBE: Lowest part of ear. AURICLE: Thin part of the outside of the ear around the middle
	 * HELIX: the thin upper outside part of your ear. ANTI-HELIX: the thin upper inside part of your ear.
	 * I had more that i looked up but i can't realisticly make them part of anything not human.
	 * I am aware everyone's ears are different and you may have tiny lobes and long-ass helix bits
	 * (which i suppose is also true for elfin ears); i did what i could, sue me. 
	 */
	public enum EAR_PIERCING
	{    
		LEFT_LOBE_1, LEFT_LOBE_2, LEFT_LOBE_3, LEFT_UPPER_LOBE,
		LEFT_AURICAL_1, LEFT_AURICAL_2, LEFT_AURICAL_3, LEFT_AURICAL_4,
		LEFT_HELIX_1, LEFT_HELIX_2,	LEFT_HELIX_3, LEFT_HELIX_4,
		LEFT_ANTI_HELIX,
		RIGHT_LOBE_1, RIGHT_LOBE_2, RIGHT_LOBE_3, RIGHT_UPPER_LOBE,
		RIGHT_AURICAL_1, RIGHT_AURICAL_2, RIGHT_AURICAL_3, RIGHT_AURICAL_4,
		RIGHT_HELIX_1, RIGHT_HELIX_2, RIGHT_HELIX_3, RIGHT_HELIX_4,
		RIGHT_ANTI_HELIX
	}
	public class Ears : PiercableBodyPart<Ears, EarType, EAR_PIERCING>, IFurAware
	{
		public FurColor furColor { get; protected set; } = FurColor.Generate(HairFurColors.BLACK);
		protected Ears(PiercingFlags flags) : base(flags)
		{
			type = EarType.HUMAN;
		}

		public override EarType type { get; protected set; }

		public static Ears Generate(PiercingFlags flags)
		{
			return new Ears(flags);
		}
		public static Ears Generate(EarType earType, PiercingFlags flags)
		{
			return new Ears(flags) { type = earType };
		}

		protected override bool PiercingLocationUnlocked(EAR_PIERCING piercingLocation)
		{
			return true;
		}

		public override bool Restore()
		{
			if (type == EarType.HUMAN)
			{
				return false;
			}
			type = EarType.HUMAN;
			return true;
		}

		public override bool RestoreAndDisplayMessage(Player player)
		{
			if (type == EarType.HUMAN)
			{
				return false;
			}
			OutputText(restoreString(player));
			type = EarType.HUMAN;
			return true;
		}

		public bool UpdateEars(EarType earType)
		{
			if (type == earType)
			{
				return false;
			}
			type = earType;
			return true;
		}
		public bool UpdateEarsAndDisplayMessage(EarType newType, Player player)
		{
			if (type == newType)
			{
				return false;
			}
			OutputText(transformInto(newType, player));
			type = newType;
			return true;
		}

		public void reactToChangeInFurColor(object sender, FurAwareEventArg e)
		{
			if (!e.primaryColor.isNoFur())
			{
				furColor.UpdateFurColor(e.primaryColor);
			}
		}
	}
	public class EarType : PiercableBodyPartBehavior<EarType, Ears, EAR_PIERCING>
	{
		private static int indexMaker = 0;

		private readonly int _index;

		protected EarType(SimpleDescriptor shortDesc, DescriptorWithArg<Ears> fullDesc, TypeAndPlayerDelegate<Ears> playerDesc, 
			ChangeType<Ears> transform, RestoreType<Ears> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
		}

		public override int index => _index;

		public static readonly EarType HUMAN = new EarType(HumanDescStr, HumanFullDesc, HumanPlayerStr, HumanTransformStr, HumanRestoreStr);
		public static readonly EarType HORSE = new EarType(HorseDescStr, HorseFullDesc, HorsePlayerStr, HorseTransformStr, HorseRestoreStr);
		public static readonly EarType DOG = new EarType(DogDescStr, DogFullDesc, DogPlayerStr, DogTransformStr, DogRestoreStr);
		public static readonly EarType COW = new EarType(CowDescStr, CowFullDesc, CowPlayerStr, CowTransformStr, CowRestoreStr);
		public static readonly EarType ELFIN = new EarType(ElfinDescStr, ElfinFullDesc, ElfinPlayerStr, ElfinTransformStr, ElfinRestoreStr);
		public static readonly EarType CAT = new EarType(CatDescStr, CatFullDesc, CatPlayerStr, CatTransformStr, CatRestoreStr);
		public static readonly EarType LIZARD = new EarType(LizardDescStr, LizardFullDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr);
		public static readonly EarType BUNNY = new EarType(BunnyDescStr, BunnyFullDesc, BunnyPlayerStr, BunnyTransformStr, BunnyRestoreStr);
		public static readonly EarType KANGAROO = new EarType(KangarooDescStr, KangarooFullDesc, KangarooPlayerStr, KangarooTransformStr, KangarooRestoreStr);
		public static readonly EarType FOX = new EarType(FoxDescStr, FoxFullDesc, FoxPlayerStr, FoxTransformStr, FoxRestoreStr);
		public static readonly EarType DRAGON = new EarType(DragonDescStr, DragonFullDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr);
		public static readonly EarType RACCOON = new EarType(RaccoonDescStr, RaccoonFullDesc, RaccoonPlayerStr, RaccoonTransformStr, RaccoonRestoreStr);
		public static readonly EarType MOUSE = new EarType(MouseDescStr, MouseFullDesc, MousePlayerStr, MouseTransformStr, MouseRestoreStr);
		public static readonly EarType FERRET = new EarType(FerretDescStr, FerretFullDesc, FerretPlayerStr, FerretTransformStr, FerretRestoreStr);
		public static readonly EarType PIG = new EarType(PigDescStr, PigFullDesc, PigPlayerStr, PigTransformStr, PigRestoreStr);
		public static readonly EarType RHINO = new EarType(RhinoDescStr, RhinoFullDesc, RhinoPlayerStr, RhinoTransformStr, RhinoRestoreStr);
		public static readonly EarType ECHIDA = new EarType(EchidnaDescStr, EchidnaFullDesc, EchidnaPlayerStr, EchidnaTransformStr, EchidnaRestoreStr);
		public static readonly EarType DEER = new EarType(DeerDescStr, DeerFullDesc, DeerPlayerStr, DeerTransformStr, DeerRestoreStr);
		public static readonly EarType WOLF = new EarType(WolfDescStr, WolfFullDesc, WolfPlayerStr, WolfTransformStr, WolfRestoreStr);
		public static readonly EarType SHEEP = new EarType(SheepDescStr, SheepFullDesc, SheepPlayerStr, SheepTransformStr, SheepRestoreStr);
		public static readonly EarType IMP = new EarType(ImpDescStr, ImpFullDesc, ImpPlayerStr, ImpTransformStr, ImpRestoreStr);
		public static readonly EarType COCKATRICE = new EarType(CockatriceDescStr, CockatriceFullDesc, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr);
		public static readonly EarType RED_PANDA = new EarType(RedPandaDescStr, RedPandaFullDesc, RedPandaPlayerStr, RedPandaTransformStr, RedPandaRestoreStr);


	}
}
