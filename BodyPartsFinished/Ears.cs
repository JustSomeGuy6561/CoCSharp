﻿//Ears.cs
//Description:
//Author: JustSomeGuy
//12/27/2018, 12:22 AM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
	class Ears : PiercableBodyPart<Ears, EarType, EAR_PIERCING>
	{
		protected Ears(PiercingFlags flags) : base(flags)
		{
			type = EarType.HUMAN;
		}

		public override EarType type { get; protected set; }

		public Ears Generate(PiercingFlags flags)
		{
			return new Ears(flags);
		}
		public Ears Generate(EarType earType, PiercingFlags flags)
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
			OutputText(restoreString(this, player));
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
		public bool UpdateEarsAndDisplayMessage(EarType earType, Player player)
		{
			if (type == earType)
			{
				return false;
			}
			OutputText(transformFrom(this, player));
			type = earType;
			return true;
		}
	}
	class EarType : PiercableBodyPartBehavior<EarType, Ears, EAR_PIERCING>
	{
		private static int indexMaker = 0;

		private readonly int _index;

		protected EarType(GenericDescription shortDesc, CreatureDescription<Ears> creatureDesc, PlayerDescription<Ears> playerDesc, 
			ChangeType<Ears> transform, ChangeType<Ears> restore) : base(shortDesc, creatureDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
		}

		public override int index => _index;

		public static readonly EarType HUMAN = new EarType(HumanDescStr, HumanCreatureStr, HumanPlayerStr, HumanTransformStr, HumanRestoreStr);
		public static readonly EarType HORSE = new EarType(HorseDescStr, HorseCreatureStr, HorsePlayerStr, HorseTransformStr, HorseRestoreStr);
		public static readonly EarType DOG = new EarType(DogDescStr, DogCreatureStr, DogPlayerStr, DogTransformStr, DogRestoreStr);
		public static readonly EarType COW = new EarType(CowDescStr, CowCreatureStr, CowPlayerStr, CowTransformStr, CowRestoreStr);
		public static readonly EarType ELFIN = new EarType(ElfinDescStr, ElfinCreatureStr, ElfinPlayerStr, ElfinTransformStr, ElfinRestoreStr);
		public static readonly EarType CAT = new EarType(CatDescStr, CatCreatureStr, CatPlayerStr, CatTransformStr, CatRestoreStr);
		public static readonly EarType LIZARD = new EarType(LizardDescStr, LizardCreatureStr, LizardPlayerStr, LizardTransformStr, LizardRestoreStr);
		public static readonly EarType BUNNY = new EarType(BunnyDescStr, BunnyCreatureStr, BunnyPlayerStr, BunnyTransformStr, BunnyRestoreStr);
		public static readonly EarType KANGAROO = new EarType(KangarooDescStr, KangarooCreatureStr, KangarooPlayerStr, KangarooTransformStr, KangarooRestoreStr);
		public static readonly EarType FOX = new EarType(FoxDescStr, FoxCreatureStr, FoxPlayerStr, FoxTransformStr, FoxRestoreStr);
		public static readonly EarType DRAGON = new EarType(DragonDescStr, DragonCreatureStr, DragonPlayerStr, DragonTransformStr, DragonRestoreStr);
		public static readonly EarType RACCOON = new EarType(RaccoonDescStr, RaccoonCreatureStr, RaccoonPlayerStr, RaccoonTransformStr, RaccoonRestoreStr);
		public static readonly EarType MOUSE = new EarType(MouseDescStr, MouseCreatureStr, MousePlayerStr, MouseTransformStr, MouseRestoreStr);
		public static readonly EarType FERRET = new EarType(FerretDescStr, FerretCreatureStr, FerretPlayerStr, FerretTransformStr, FerretRestoreStr);
		public static readonly EarType PIG = new EarType(PigDescStr, PigCreatureStr, PigPlayerStr, PigTransformStr, PigRestoreStr);
		public static readonly EarType RHINO = new EarType(RhinoDescStr, RhinoCreatureStr, RhinoPlayerStr, RhinoTransformStr, RhinoRestoreStr);
		public static readonly EarType ECHIDA = new EarType(EchidnaDescStr, EchidnaCreatureStr, EchidnaPlayerStr, EchidnaTransformStr, EchidnaRestoreStr);
		public static readonly EarType DEER = new EarType(DeerDescStr, DeerCreatureStr, DeerPlayerStr, DeerTransformStr, DeerRestoreStr);
		public static readonly EarType WOLF = new EarType(WolfDescStr, WolfCreatureStr, WolfPlayerStr, WolfTransformStr, WolfRestoreStr);
		public static readonly EarType SHEEP = new EarType(SheepDescStr, SheepCreatureStr, SheepPlayerStr, SheepTransformStr, SheepRestoreStr);
		public static readonly EarType IMP = new EarType(ImpDescStr, ImpCreatureStr, ImpPlayerStr, ImpTransformStr, ImpRestoreStr);
		public static readonly EarType COCKATRICE = new EarType(CockatriceDescStr, CockatriceCreatureStr, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr);
		public static readonly EarType RED_PANDA = new EarType(RedPandaDescStr, RedPandaCreatureStr, RedPandaPlayerStr, RedPandaTransformStr, RedPandaRestoreStr);


	}
}