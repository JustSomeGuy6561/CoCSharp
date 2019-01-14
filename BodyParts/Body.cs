using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using CoC.BodyParts.SpecialInteraction;
using CoC.EpidermalColors;
using CoC.Strings;
using CoC.Tools;
using static CoC.Strings.BodyParts.BodyStrings;
using static CoC.UI.TextOutput;
namespace CoC.BodyParts
{

	/*
	 * Body: Much like genitals, it will store all data related to it. in this case, skin tone/ fur color.
	 * for primary and things like underbody. it will also store the hair color if the pc is completely devoid of hair
	 * similarly, it will generate the fur color from hair if no hair is available. body parts that react to changes in
	 * primary and/or secondary colors can implement an interface granting them access to primary and secondary colors as they
	 * change. as of now, this is arms (by extension, hands), legs, and face. 
	 * 
	 * vanilla behavior is that all parts that set a tone or fur color and chage a type restore underbody to none. i'd like to automate that
	 * idk how w/0 making every race implement core type. 
	 * 
	 * need to update epidermis. it currently only supports either furcolor or tone. that, or store them separately here, which is the only place
	 * that behavior is necessary. i could then set one in here internally for others to read and use.
	 * 
	 * problem is that asking implementation of that for every race is nuts. 
	 * also, player appearance could says something like: the rest of your body is covered in (scales/fur/normal skin) (if secondary color: with exception to your stomach, which has (whatever color))
	 * 
	 * solution: if your fur or scales do something special, give it a type. so, goblin/demon/anemone (etc.) skin, which is basically normal skin with a different tone, can be skin. but if your or skin
	 * has stripes or some incarnate tattoo or something (yw, kitsunes), make it a new type. same goes for fur - if the creature _CAN_ use a secondary color, make them require it. this allows stripes, spots
	 * etc - whatever pattern you desire. you can have a creature with normal fur/scales everywhere, but using the secondary color as short hairs around their reproductive bits or whatever, if you so desire
	 *NOTES:
	 * 
	 * underbody can be dyed separately if furry. can be toned regardless. just changes adjective, but is more or less useless for fur.

USE OF UNDERBODY:

black rubber egg - restores type. i assume it also removes fur from core?

canine - not directly used. used to determine arm color.
clovis (sheep) - same as core. counts as fur.
ember - same as core. counts as scales. used as the underbelly.
equinium - fun fact: despite horses having an underbody, it actually restores it.
ferret fruit: picks a secondary color from a list of ferret fur colors. called underside. arms and legs both use it. fur
fox berry: used directly with appearance, indirectly used for arms. fur.
fox jewel: removed, unless fur color combo is a specific group. not directly used. fur or none
golden rind (deer): set to white when given brown fur, like a deer. fur
red river root (panda): set with fur color, to black. probably used for arms and legs in future. fur
reptilium: same as core. scales. used as underbelly.
snake oil: from what i can tell, it's actually bugged. it's supposed to be a naga underbody type, but it's allows to be reptilian. also, counts as scales. color is green.
tonotrice: counts as feathered. set to random colors from cockatrice pool. different from core.
cat: used for arms. not set directly.

PLAYER_APPEARANCE: (if applicable)
used on fox face as fur color under jaw. 
used on cat face as fur color under jaw.
used on lizard face. if scaled, makes lower/under jaw.
used on dragon face. used as under jaw color
ditto, for deer.
cockatrice just checks for it. if applicable, changes a string.

used for ferret arms
used in describing core, used as underbelly.
used in lizard tail, if different from regular epidermis.
ditto, for dragon.
used in naga lower body as underbelly scales.
used in red panda for legs. 

From what i can tell of underbody, it takes the animal and applies colors seen on the underbody. since most are quadrupeds, this includes arms and legs. if they have scales, it's 
		 -----------------------------------
things that set fur color or tone:

black rubber egg. silently removes fur. making that less silent. restoring underbelly. consider making rubber a new skin type. 
ectoplasm - only skin tone

canine pepper - restored
echidna - restored
ferret - set
goldenrind (deer) -set
kanga fruit - restored
labova - restored
mouse cocoa - restored
red river root (red panda) - set
ringtail fig (coon)  - restored
kitsune scene - N/A

gown (dryad) - restores skin type, actually changes to bark.
update behavior to restore.
fox jewel (kitsune) - restored
goblin ale - just skin color. no effect on skintype.
golden seed (harpy) - just skin color. no effect on skintype.
imp food - may change type to plain, but does so silently. fix this. otherwise, let it go.
pig truffle - just changes color. no effect on type.
reptilium - set
rhino steak - restored
snake oil - set
succubus milk - changes color. no effect on type. 
sweet gossamer - restored
wet cloth - restored

	 */
	public enum NavelPiercings { TOP, BOTTOM }
	/*
	public class Core : PiercableBodyPart<Core, CoreType, NavelPiercings>, IToneable, IDyeable
	{
		protected Core(CoreType type, Tones currentTone, FurColor currentFur, PiercingFlags flags) : base(type.epidermisType, currentTone, currentFur, flags) { }

		#region Base Body Part
		public override CoreType type { get; protected set; }

		public override bool Restore()
		{
			if (type == CoreType.SKIN)
			{
				return false;
			}
			type = CoreType.SKIN;

			return true;
		}

		public override bool RestoreAndDisplayMessage(Player player)
		{
			if (type == CoreType.SKIN)
			{
				return false;
			}
			OutputText(restoreString(this, player));
			type = CoreType.SKIN;
			return true;
		}
		#endregion

		#region Generate/Update
		public Core Generate(CoreType coreType, Tones currentTone, FurColor fur, PiercingFlags flags)
		{
			return new Core(coreType, currentTone, fur, flags);
		}

		public bool UpdateCore(CoreType coreType, Tones currentTone, FurColor furColor)
		{
			if (type == coreType)
			{
				return false;
			}
			type = coreType;
			epidermis.UpdateEpidermis(type.epidermisType, currentTone, furColor);
			return true;
		}

		public bool UpdateCoreAndDisplayMessage(CoreType newType, Tones currentTone, FurColor furColor, Player player)
		{
			if (type == newType)
			{
				return false;
			}
			OutputText(transformInto(newType, player));
			return UpdateCore(newType, currentTone, furColor);
		}
		#endregion

		#region piercings
		protected override bool PiercingLocationUnlocked(NavelPiercings piercingLocation)
		{
			return true;
		}
		#endregion

		#region ToneAndDye
		public bool canToneLotion()
		{
			return epidermis.canToneLotion();
		}

		public bool attemptToUseLotion(Tones tone)
		{
			return epidermis.attemptToUseLotion(tone);
		}

		public bool canDye()
		{
			return epidermis.canDye();
		}

		public bool attemptToDye(HairFurColors dye)
		{
			return epidermis.attemptToDye(dye);
		}
		#endregion
	}

	public class CoreType : PiercableBodyPartBehavior<CoreType, Core, NavelPiercings>
	{
		public override int index => epidermisType.index;
		public readonly EpidermisType epidermisType;
		protected CoreType(EpidermisType epidermis, GenericDescription shortDesc, FullDescription<Core> fullDesc, PlayerDescription<Core> playerDesc,
			ChangeType<Core> transform, ChangeType<Core> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore) { }

		public static CoreType SKIN = new CoreType(EpidermisType.SKIN, SkinStr, SkinFullDesc, SkinPlayerStr, SkinTransformStr, SkinRestoreStr);
		public static CoreType FUR = new CoreType(EpidermisType.FUR, FurStr, FurFullDesc, FurPlayerStr, FurTransformStr, FurRestoreStr);
		public static CoreType SCALES = new CoreType(EpidermisType.SCALES, ScalesStr, ScalesFullDesc, ScalesPlayerStr, ScalesTransformStr, ScalesRestoreStr);
		public static CoreType GOO = new CoreType(EpidermisType.GOO, GooStr, GooFullDesc, GooPlayerStr, GooTransformStr, GooRestoreStr);
		public static CoreType WOOL = new CoreType(EpidermisType.WOOL, WoolStr, WoolFullDesc, WoolPlayerStr, WoolTransformStr, WoolRestoreStr);
		public static CoreType FEATHERS = new CoreType(EpidermisType.FEATHERS, FeatherStr, FeatherFullDesc, FeatherPlayerStr, FeatherTransformStr, FeatherRestoreStr);
		public static CoreType BARK = new CoreType(EpidermisType.BARK, BarkStr, BarkFullDesc, BarkPlayerStr, BarkTransformStr, BarkRestoreStr);
		public static CoreType CARAPACE = new CoreType(EpidermisType.CARAPACE, CarapaceStr, CarapaceFullDesc, CarapacePlayerStr, CarapaceTransformStr, CarapaceRestoreStr);
		public static CoreType EXOSKELETON = new CoreType(EpidermisType.EXOSKELETON, ExoskeletonStr, ExoskeletonFullDesc, ExoskeletonPlayerStr, ExoskeletonTransformStr, ExoskeletonRestoreStr);
	}
	*/
	public class Body : PiercableBodyPart<Body, BodyType, NavelPiercings>
	{
		HashSet<IToneAware> toneAwares = new HashSet<IToneAware>();
		HashSet<IFurAware> furAwares = new HashSet<IFurAware>();

		private HairFurColors hairColor;
		private FurColor primaryFur;
		private FurColor secondaryFur;

		private Tones primaryTone;
		private Tones secondaryTone;

		public EpidermalData primaryEpidermis => new EpidermalData(type.epidermisType, primaryFur, primaryTone);

		public EpidermalData secondaryEpidermis => (type.hasUnderBody) ? new EpidermalData(type.epidermisType, secondaryFur, secondaryTone) : null;

		public override BodyType type { get; protected set; }


		protected Body(BodyType bodyType, PiercingFlags flags) : base(flags)
		{
			this.type = bodyType;
			this.hairColor = type.defaultFur.primaryColor;
			this.primaryFur = FurColor.GenerateFromOther(type.defaultFur);
			this.secondaryFur = FurColor.GenerateFromOther(type.defaultFur);
			this.primaryTone = type.defaultTone;
			this.secondaryTone = type.defaultTone;

		}

		public static Body GenerateHumanoid(PiercingFlags flags, Tones skinTone)
		{
			return new Body(BodyType.HUMANOID, flags)
			{
				primaryTone = skinTone
			};
		}

		//Default initializer. all values are set to species default, if applicable. 
		public static Body GenerateDefault(PiercingFlags flags, BodyType bodyType)
		{
			return new Body(bodyType, flags);
		}

		//Standard initializer. assumes the creature may have hair and/or some skin. will set the corresponding values, even if the body type does not currently use them
		//they may be used elsewhere, or have to be used as the result of a TF. underbody, if applicable, will default to primary colors.
		public static Body GenerateStandard(PiercingFlags flags, BodyType bodyType, HairFurColors primaryHairColor, Tones primaryTone)
		{
			Body retVal = new Body(bodyType, flags)
			{
				hairColor = primaryHairColor,
				primaryTone = primaryTone,
				secondaryTone = primaryTone
			};
			retVal.primaryFur.UpdateFurColor(primaryHairColor);
			retVal.secondaryFur.UpdateFurColor(primaryHairColor);
			return retVal;
		}

		//Standard initializer, but allows multicolored fur. again, assumes creatures may have/use skin and/or fur.
		//they may be used elsewhere, or have to be used as the result of a TF. underbody, if applicable, will default to primary colors.
		public static Body GenerateStandardWithFur(PiercingFlags flags, BodyType bodyType, FurColor primaryFur, Tones primaryTone)
		{
			Body retVal = new Body(bodyType, flags)
			{
				hairColor = primaryFur.primaryColor,
				primaryTone = primaryTone,
				secondaryTone = primaryTone
			};
			retVal.primaryFur.UpdateFurColor(primaryFur);
			retVal.secondaryFur.UpdateFurColor(primaryFur);
			return retVal;
		}

		//initializer with specific values for underbody. again, assumes creature may have skin and/or hair/fur. in the event they become necessary, they are available.
		//if underbody is not applicable, the secondary tone and fur values will be ignored. 
		public static Body GenerateWithUnderbody(PiercingFlags flags, BodyType bodyType, FurColor primaryFur, Tones primaryTone, FurColor secondaryFur, Tones secondaryTone)
		{
			Body retVal = new Body(bodyType, flags)
			{
				hairColor = primaryFur.primaryColor,
				primaryTone = primaryTone,
				secondaryTone = secondaryTone
			};
			retVal.primaryFur.UpdateFurColor(primaryFur);
			retVal.secondaryFur.UpdateFurColor(secondaryFur);
			return retVal;
		}

		public bool isFurry => type == BodyType.SIMPLE_FUR || type == BodyType.UNDERBODY_FUR;


		public override bool Restore()
		{
			if (type == BodyType.HUMANOID)
			{
				return false;
			}
			type = BodyType.HUMANOID;

			ToneChanged();
			FurChanged();
			return true;
		}

		public override bool RestoreAndDisplayMessage(Player player)
		{
			if (type == BodyType.HUMANOID)
			{
				return false;
			}
			OutputText(restoreString(this, player));
			return Restore();
		}

		protected override bool PiercingLocationUnlocked(NavelPiercings piercingLocation)
		{
			return true;
		}

		public bool AttachIToneAware(IToneAware item, bool runNow = false)
		{
			bool retVal = toneAwares.Add(item);
			if (retVal && runNow)
			{
				item.reactToChangeInSkinTone(primaryTone, secondaryTone);
			}
			return retVal;
		}

		public bool DetachIToneAware(IToneAware item)
		{
			return toneAwares.Remove(item);
		}

		public bool AttachIFurAware(IFurAware item, bool runNow = false)
		{
			bool retVal = furAwares.Add(item);
			if (retVal && runNow)
			{
				item.reactToChangeInFurColor(primaryFur, secondaryFur);
			}
			return retVal;
		}

		public bool DetachIToneAware(IFurAware item)
		{
			return furAwares.Remove(item);
		}

		private void ToneChanged()
		{
			foreach (IToneAware toneAware in toneAwares)
			{
				toneAware.reactToChangeInSkinTone(primaryTone, secondaryTone);
			}
		}

		private void FurChanged()
		{
			foreach (IFurAware furAware in furAwares)
			{
				furAware.reactToChangeInFurColor(primaryFur, secondaryFur);
			}
		}

		private bool setupRan = false;
		//call this when the initial creature setup is done. it wil cause every body part that subscribed to fur or tone aware to
		//get the new data. should only be called once. 
		public void Setup()
		{
			if (setupRan)
			{
				Debug.WriteLine("Body Setup was already run. ignoring.");
			}
			else
			{
				setupRan = true;
				ToneChanged();
				FurChanged();
			}
		}
	}

	public class BodyType : PiercableBodyPartBehavior<BodyType, Body, NavelPiercings>
	{
		private static int indexMaker = 0;

		public readonly bool hasUnderBody;
		public readonly GenericDescription underBodyDescription;
		public readonly EpidermisType epidermisType;
		public override int index => _index;
		private readonly int _index;
		public readonly Tones defaultTone;
		public readonly FurColor defaultFur;

		protected BodyType(EpidermisType type, Tones defTone,
			GenericDescription shortDesc, FullDescription<Body> fullDesc, PlayerDescription<Body> playerDesc, 
			ChangeType<Body> transform, ChangeType<Body> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			epidermisType = type;
			defaultTone = defTone;
			defaultFur = FurColor.GenerateEmpty();
			hasUnderBody = false;
			underBodyDescription = GlobalStrings.None;
		}

		protected BodyType(EpidermisType type, FurColor defFur,
			GenericDescription shortDesc, FullDescription<Body> fullDesc, PlayerDescription<Body> playerDesc,
			ChangeType<Body> transform, ChangeType<Body> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			epidermisType = type;
			defaultTone = Tones.NOT_APPLICABLE;
			defaultFur = FurColor.GenerateFromOther(defFur);
			hasUnderBody = false;
			underBodyDescription = GlobalStrings.None;
		}

		protected BodyType(EpidermisType type, Tones defTone, GenericDescription underBodyDescript, 
			GenericDescription shortDesc, FullDescription<Body> fullDesc, PlayerDescription<Body> playerDesc,
			ChangeType<Body> transform, ChangeType<Body> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			epidermisType = type;
			defaultTone = defTone;
			defaultFur = FurColor.GenerateEmpty();
			hasUnderBody = (underBodyDescript != GlobalStrings.None);
			underBodyDescription = underBodyDescript;
		}

		protected BodyType(EpidermisType type, FurColor defFur, GenericDescription underBodyDescript,
			GenericDescription shortDesc, FullDescription<Body> fullDesc, PlayerDescription<Body> playerDesc,
			ChangeType<Body> transform, ChangeType<Body> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			epidermisType = type;
			defaultTone = Tones.NOT_APPLICABLE;
			defaultFur = FurColor.GenerateFromOther(defFur);
			hasUnderBody = (underBodyDescript != GlobalStrings.None);
			underBodyDescription = underBodyDescript;
		}

		public static readonly BodyType HUMANOID;
		public static readonly BodyType REPTILIAN;
		public static readonly BodyType FEATHERED;
		public static readonly BodyType WOODEN;
		public static readonly BodyType SIMPLE_FUR; //one color (or two in a pattern, like zebra stripes) over the entire body.

		//the anthropomorphic equivalent of underbody, at least. this means that most of the body is the first color (or pattern), while the chest is the other. note that this may also
		//effect the arms, legs, and face (and possibly others if implemented), as they may utilize both or just one of these colors, depending on the type. 
		public static readonly BodyType UNDERBODY_FUR;
		public static readonly BodyType GOO; //now, if you have gooey body, give the goo innards perk. simple.
		public static readonly BodyType RUBBER; //cleaner - we don't need umpteen checks to see if it's "rubbery"
		public static readonly BodyType CARAPACE; //like a turtle shell
		public static readonly BodyType EXOSKELETON; //or chitin, if you prefer.
	}
}
