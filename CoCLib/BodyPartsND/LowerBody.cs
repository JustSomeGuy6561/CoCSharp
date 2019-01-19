//LowerBody.cs
//Description:
//Author: JustSomeGuy
//12/28/2018, 10:09 PM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoC.Tools;
using static CoC.UI.TextOutput;
using static CoC.Strings.BodyParts.LowerBodyStrings;
using CoC.Strings;
using CoC.BodyParts.SpecialInteraction;

namespace CoC.BodyParts
{
	//FUCK THIS. I just finished horns. Can't i get something easy? TBD.

	/*
	 * Butt and hips have been added here. this is due to the fact that if you dont have a lower body (read: monsters like vines), you dont have a butt or hips.
	 * it also lets you specify defaults per lower body type, so naturally slender characters, like cats, for example, can default to thin forms. pigs, on the other hand,
	 * may start out slightly wider. so, too, may more "sexy" and/or "thicc" creatures
	 * 
	 * 96.7% of body parts do not change leg count. the only one not that i've seen is hooves. it's possible more 'taur forms were planned, idk.
	 * for now, that's removed. if you need it, just add a supportsTaur? boolean to the type constructor and a readonly field, and an isTaur to the lower body. 
	 * you'll also need to define behavior to determine when changing to or from taur compatible forms. but that's on you, i ain't adding it - too much validation.
	 * In the event you do, you can remain fully compatible with existing code by adding additional update calls with a specific subclass of lowerbodytype. see the wings 
	 * for an example of how to do this. 
	 */


	public class LowerBody : BodyPartBase<LowerBody, LowerBodyType>, IFurAware
	{
		//No magic constants. Woo!
		//not even remotely necessary, but it makes it a hell of a lot easier to debug
		//when numbers aren't magic constants. (running grep with a string is much easier
		//than a regular expression looking for legs.count = [A-Za-z0-9]+ or something worse
		public const byte MONOPED_LEG_COUNT = 1;
		public const byte BIPED_LEG_COUNT = 2;
		public const byte QUADRUPED_LEG_COUNT = 4;
		public const byte OCTOPED_LEG_COUNT = 8;


		public readonly Butt butt;
		public readonly Hips hips;

		//you'll need to change this if taurs are suddenly part of everything that supports it. 
		public int legCount => type.legCount;

		public override LowerBodyType type { get; protected set; }

		protected LowerBody()
		{
			butt = Butt.Generate_NoButt();
			hips = Hips.GenerateHips(0);
		}

		protected LowerBody(LowerBodyType lowerBody, AssLocation assLocation, int buttSize, int hipSize)
		{
			butt = Butt.GenerateButt(assLocation, buttSize);
			hips = Hips.GenerateHips(hipSize);
		}

		public static LowerBody GenerateNoLowerBody()
		{
			return new LowerBody();
		}

		public static LowerBody Generate(LowerBodyType lowerBody, AssLocation assLocation = AssLocation.BUTT, int ButtSize = Butt.AVERAGE, int HipSize = Hips.AVERAGE)
		{
			return new LowerBody(lowerBody, assLocation, ButtSize, HipSize);
		}

		public override bool Restore()
		{
			if (type == LowerBodyType.HUMAN)
			{
				return false;
			}
			type = LowerBodyType.HUMAN;
			return true;
		}

		public override bool RestoreAndDisplayMessage(Player player)
		{
			if (type == LowerBodyType.HUMAN)
			{
				return false;
			}
			OutputText(restoreString(this, player));
			return Restore();
		}

		public bool UpdateType(LowerBodyType newType)
		{
			if (type == newType)
			{
				return false;
			}
			type = newType;
			return true;
		}

		public bool UpdateTypeAndDisplayMessage(LowerBodyType newType, Player player)
		{
			if (type == newType)
			{
				return false;
			}
			OutputText(transformInto(newType, player));
			return UpdateType(newType);
		}
	}

	public class LowerBodyType : BodyPartBehavior<LowerBodyType, LowerBody>
	{
		private const int NOLEGS = 1;
		private const int MONOPED = 1;
		private const int BIPED = 2;
		private const int QUADRUPED = 4;
		private const int OCTOPED = 8;

		private static int indexMaker = 0;

		private readonly int _index;

		public readonly int legCount;
		protected LowerBodyType(int numLegs,
			GenericDescription shortDesc, FullDescription<LowerBody> fullDesc, PlayerDescription<LowerBody> playerDesc,
			ChangeType<LowerBody> transform, ChangeType<LowerBody> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			legCount = numLegs;
		}

		public override int index => _index;

		public static readonly LowerBodyType HUMAN = new LowerBodyType(BIPED, HumanDesc, HumanFullDesc, HumanPlayerStr, HumanTransformStr, HumanRestoreStr);
		public static readonly LowerBodyType HOOVED = new LowerBodyType(BIPED, HoovedDesc, HoovedFullDesc, HoovedPlayerStr, HoovedTransformStr, HoovedRestoreStr);
		public static readonly LowerBodyType DOG = new LowerBodyType(BIPED, DogDesc, DogFullDesc, DogPlayerStr, DogTransformStr, DogRestoreStr);
		public static readonly LowerBodyType NAGA = new LowerBodyType(MONOPED, NagaDesc, NagaFullDesc, NagaPlayerStr, NagaTransformStr, NagaRestoreStr);
		public static readonly LowerBodyType CENTAUR = new LowerBodyType(QUADRUPED, HoovedDesc, HoovedFullDesc, HoovedPlayerStr, HoovedTransformStr, HoovedRestoreStr);
		public static readonly LowerBodyType DEMONIC_HIGH_HEELS = new LowerBodyType(BIPED, DemonHiHeelsDesc, DemonHiHeelsFullDesc, DemonHiHeelsPlayerStr, DemonHiHeelsTransformStr, DemonHiHeelsRestoreStr);
		public static readonly LowerBodyType DEMONIC_CLAWS = new LowerBodyType(BIPED, DemonClawDesc, DemonClawFullDesc, DemonClawPlayerStr, DemonClawTransformStr, DemonClawRestoreStr);
		public static readonly LowerBodyType BEE = new LowerBodyType(BIPED, BeeDesc, BeeFullDesc, BeePlayerStr, BeeTransformStr, BeeRestoreStr);
		public static readonly LowerBodyType GOO = new LowerBodyType(MONOPED, GooDesc, GooFullDesc, GooPlayerStr, GooTransformStr, GooRestoreStr);
		public static readonly LowerBodyType CAT = new LowerBodyType(BIPED, CatDesc, CatFullDesc, CatPlayerStr, CatTransformStr, CatRestoreStr);
		public static readonly LowerBodyType LIZARD = new LowerBodyType(BIPED, LizardDesc, LizardFullDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr);
		public static readonly LowerBodyType PONY = new LowerBodyType(QUADRUPED, PonyDesc, PonyFullDesc, PonyPlayerStr, PonyTransformStr, PonyRestoreStr);
		public static readonly LowerBodyType BUNNY = new LowerBodyType(BIPED, BunnyDesc, BunnyFullDesc, BunnyPlayerStr, BunnyTransformStr, BunnyRestoreStr);
		public static readonly LowerBodyType HARPY = new LowerBodyType(BIPED, HarpyDesc, HarpyFullDesc, HarpyPlayerStr, HarpyTransformStr, HarpyRestoreStr);
		public static readonly LowerBodyType KANGAROO = new LowerBodyType(BIPED, KangarooDesc, KangarooFullDesc, KangarooPlayerStr, KangarooTransformStr, KangarooRestoreStr);
		public static readonly LowerBodyType CHITINOUS_SPIDER = new LowerBodyType(BIPED, SpiderDesc, SpiderFullDesc, SpiderPlayerStr, SpiderTransformStr, SpiderRestoreStr);
		public static readonly LowerBodyType DRIDER = new LowerBodyType(OCTOPED, DriderDesc, DriderFullDesc, DriderPlayerStr, DriderTransformStr, DriderRestoreStr);
		public static readonly LowerBodyType FOX = new LowerBodyType(BIPED, FoxDesc, FoxFullDesc, FoxPlayerStr, FoxTransformStr, FoxRestoreStr);
		public static readonly LowerBodyType DRAGON = new LowerBodyType(BIPED, DragonDesc, DragonFullDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr);
		public static readonly LowerBodyType RACCOON = new LowerBodyType(BIPED, RaccoonDesc, RaccoonFullDesc, RaccoonPlayerStr, RaccoonTransformStr, RaccoonRestoreStr);
		public static readonly LowerBodyType FERRET = new LowerBodyType(BIPED, FerretDesc, FerretFullDesc, FerretPlayerStr, FerretTransformStr, FerretRestoreStr);
		public static readonly LowerBodyType CLOVEN_HOOVED = new LowerBodyType(BIPED, ClovenHoofDesc, ClovenHoofFullDesc, ClovenHoofPlayerStr, ClovenHoofTransformStr, ClovenHoofRestoreStr);//?
		public static readonly LowerBodyType ECHIDNA = new LowerBodyType(BIPED, EchidnaDesc, EchidnaFullDesc, EchidnaPlayerStr, EchidnaTransformStr, EchidnaRestoreStr);
		public static readonly LowerBodyType SALAMANDER = new LowerBodyType(BIPED, SalamanderDesc, SalamanderFullDesc, SalamanderPlayerStr, SalamanderTransformStr, SalamanderRestoreStr);
		public static readonly LowerBodyType WOLF = new LowerBodyType(BIPED, WolfDesc, WolfFullDesc, WolfPlayerStr, WolfTransformStr, WolfRestoreStr);
		public static readonly LowerBodyType IMP = new LowerBodyType(BIPED, ImpDesc, ImpFullDesc, ImpPlayerStr, ImpTransformStr, ImpRestoreStr);
		public static readonly LowerBodyType COCKATRICE = new LowerBodyType(BIPED, CockatriceDesc, CockatriceFullDesc, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr);
		public static readonly LowerBodyType RED_PANDA = new LowerBodyType(BIPED, RedPandaDesc, RedPandaFullDesc, RedPandaPlayerStr, RedPandaTransformStr, RedPandaRestoreStr);
		//monsters that don't have feet, like an aquatic creature or something. vines also apply, unless you go all piranna plant or something.
		public static readonly LowerBodyType NO_LEG_MONSTERS = new LowerBodyType(NOLEGS, GlobalStrings.None, (x) => GlobalStrings.None(), (x,y) => GlobalStrings.None(), (x, y) => GlobalStrings.None(), (x, y) => GlobalStrings.None());
	}
}
