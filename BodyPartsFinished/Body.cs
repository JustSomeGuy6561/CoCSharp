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

	/* Behavior of body: you always have some form of skin, even if you have feathers or fur. Thus, you always have a primary skin tone. 
	 * Underbody is optional, and some body types do not have it. underbody is sort of a vague term when dealing with anthropomorphic characters, but generally it's your chest and core that get it
	 * body parts can react to changes in tone, fur, or underbody, so things like red panda can use the underbody color on arms and legs (and possibly tail?)
	 *
	 * If you change to a fur type from a tone type and dont provide a fur color, it will use the hair color, or a default if the character is bald.
	 * if you change to a fur type from another fur type, the existing fur color will be used unless a new color is provided. 
	 * 
	 * If you cange to a tone type, the current skin tone will be used unless another is provided, regardless of original type.
	 * 
	 * Some body types allow an "underbody." underbodies are an alternate color for the anthropomorphic equivalent to the underside of an animal. this generally means the chest and core areas, though
	 * reptiles use it as an "underbelly" instead. some body parts, notably arms and legs, may wish to use this underbelly color; an interface exists for this. more on that later. 
	 * 
	 * Underbody is extremely difficult to provide a logical solution for, as it's sort of used for whatever anyone feels like. That said, here are the rules i have come up with:
	 * 1) Underbody only exists when needed. if a type does not use it, it is disabled. 
	 * 1-A) If a type does use it, but the old type does not, it will default to the primary color. you may provide an alternate color as you see fit.
	 * 
	 * 2) Underbody can only use a tone or a fur color. whichever one it uses, the other is always set to the empty or not applicable option. 
	 * 2-A) If you change from a furred underbody to a toned underbody, the old underbody fur color is lost. the tone is the primary tone unless an alternate is provided.
	 * 2-B) If you change from a toned underbody to a furred one, the old tone is lost. the fur is the primary fur color, unless an alternate is provided.
	 * 2-C) If the underbody remains toned (or remains furred), the old underbody color will remain in use, unless an alternate is provided. 
	 * 
	 * 3) Body parts (notably arms and legs) may wish to match the underbody. this class has a simple event system - any body part may implement a tone aware or fur aware interface, 
	 * by adding it to the lists here, it will be notified of any changes to hair or fur colors. it may simply update itself with the new color, or do something crazy such as lose its fur or whatever. 
	 * that's entirely up to you.
	 * 
	 * Note that this is one-way. The core can tell the body parts they may want to update, but the body parts do not report back.
	 * 3-A) It's important to note the order of events. when you update a type, it will change the type, activate or deactivate tone, fur, and/or underbody as needed, then notify all body parts that care.
	 * then, and only then, will the update function return.
	 * 3-B) It is recommended to provide custom underbelly or primary colors when you change the type, instead of separately. if you do this, it will only have to notify body parts once instead of twice.
	 * Additionally, it may prevent undesired behavior. for example: you are using Red Panda TFs, and wish to add an underbody. The PC already has black Red Panda arms. you wish to set the underbody to match the
	 * arms. Currently, however, the PC has blue fur. if you change the type to a furry type with underbody, but don't provide an underbody color, it will use the primary color (blue) as the underbody color. 
	 * Suddenly, your black panda arms are now blue. if you go to update the underbody to match your arms now, it will remain blue, not become black. 
	 * 
	 * TL;DR: you have control over body part colors, and you can automatically have them match the current fur or skin. if you change the body type, these will update automatically. if you wish to use their old color,
	 * you must get it before changing the type, as they may automatically change. 
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

		public bool isFurry => type.isFurry;

		protected Body(BodyType bodyType, PiercingFlags flags) : base(flags)
		{
			this.type = bodyType;
			this.primaryTone = Tones.LIGHT;
			this.hairColor = HairFurColors.BLACK;
			this.primaryFur = FurColor.GenerateEmpty();
			this.secondaryFur = FurColor.GenerateEmpty();
			this.secondaryTone = Tones.NOT_APPLICABLE;
			if (bodyType is FurBodyType)
			{
				FurBodyType furBody = (FurBodyType)type;
				this.hairColor = furBody.defaultFurColor.primaryColor;
				this.primaryFur.UpdateFurColor(furBody.defaultFurColor);
				if (furBody.hasUnderBody)
				{
					this.secondaryFur.UpdateFurColor(primaryFur);
				}
			}
			else
			{
				ToneBodyType toneBody = (ToneBodyType)type;
				this.primaryTone = toneBody.defaultTone;
				if (toneBody.hasUnderBody)
				{
					this.secondaryTone = primaryTone;
				}
			}
		}

		//Default initializer. all values are set to species default, if applicable. 
		public static Body GenerateDefault(PiercingFlags flags, BodyType bodyType)
		{
			return new Body(bodyType, flags);
		}

		public static Body GenerateHumanoid(PiercingFlags flags, Tones skinTone, HairFurColors hair)
		{
			return new Body(BodyType.HUMANOID, flags)
			{
				primaryTone = skinTone,
				hairColor = hair
			};
		}

		public static Body GenerateTonedNoUnderbody(PiercingFlags flags, ToneBodyType toneBody, Tones primaryTone, HairFurColors hairColor)
		{
			Body retVal = new Body(toneBody, flags);
			if (hairColor != HairFurColors.NO_HAIR_FUR)
			{
				retVal.hairColor = hairColor;
			}
			if (primaryTone != Tones.NOT_APPLICABLE)
			{
				retVal.primaryTone = primaryTone;
			}
			return retVal;
		}
		public static Body GenerateToneWithUnderbody(PiercingFlags flags, ToneBodyType toneBody, Tones primaryTone, Tones secondaryTone, HairFurColors hairColor)
		{
			Body retVal = new Body(toneBody, flags);
			if (hairColor != HairFurColors.NO_HAIR_FUR)
			{
				retVal.hairColor = hairColor;
			}
			if (primaryTone != Tones.NOT_APPLICABLE)
			{
				retVal.primaryTone = primaryTone;
			}
			if (secondaryTone != Tones.NOT_APPLICABLE && toneBody.hasUnderBody)
			{
				retVal.secondaryTone = secondaryTone;
			}
			return retVal;
		}

		public static Body GenerateFurredNoUnderbody(PiercingFlags flags, FurBodyType furryBody, FurColor primaryFur, HairFurColors hairColor)
		{
			Body retVal = new Body(furryBody, flags);
			if (hairColor != HairFurColors.NO_HAIR_FUR)
			{
				retVal.hairColor = hairColor;
			}
			if (!primaryFur.isNoFur())
			{
				retVal.primaryFur.UpdateFurColor(primaryFur);
			}
			return retVal;
		}
		public static Body GenerateFurredWithUnderbody(PiercingFlags flags, FurBodyType furryBody, FurColor primaryFur, FurColor secondaryFur, HairFurColors hairColor)
		{
			Body retVal = new Body(furryBody, flags);
			if (hairColor != HairFurColors.NO_HAIR_FUR)
			{
				retVal.hairColor = hairColor;
			}
			if (!primaryFur.isNoFur())
			{
				retVal.primaryFur.UpdateFurColor(primaryFur);
			}
			if (!secondaryFur.isNoFur())
			{
				retVal.secondaryFur.UpdateFurColor(secondaryFur);
			}
			return retVal;
		}

		public bool UpdateBody(FurBodyType furryType, FurColor primaryColor, FurColor secondaryColor)
		{
			//same type? quick exit.
			if (type == furryType)
			{
				return false;
			}
			//you gave us an empty fur? dick. fallback to default.
			else if (primaryColor.isNoFur() && secondaryColor.isNoFur())
			{
				return UpdateBody(furryType);
			}
			//primary fur is empty and secondary fur is not needed, fallback to default.
			else if (primaryFur.isNoFur() && !furryType.hasUnderBody)
			{
				return UpdateBody(furryType);
			}
			//primary is empty, secondary used. just assume secondary is the only one to update.
			else if (primaryFur.isNoFur())
			{
				Debug.WriteLine("You're a huge dick");
				if (furryType.hasUnderBody)
				{
					secondaryFur.UpdateFurColor(secondaryColor);
				}
				type = furryType;
				return true;
			}
			//slightly less of a dick - no secondary fur, or no underbody, but you called this anyway. nj.
			else if (secondaryColor.isNoFur() || !furryType.hasUnderBody)
			{
				return UpdateBody(furryType, primaryColor);
			}
			//different furry types. new type has an underbody, so old type underbody is irrelevant.
			else if (type.isFurry)
			{
				primaryFur.UpdateFurColor(primaryColor);
				secondaryFur.UpdateFurColor(secondaryColor);
				type = furryType;
				return true;
			}
			//old type is a tone type.
			else
			{
				primaryFur.UpdateFurColor(primaryColor);
				//initialize hair if empty.
				if (hairColor == HairFurColors.NO_HAIR_FUR) //!type.isFurry
				{
					hairColor = primaryFur.primaryColor;
				}
				secondaryFur.UpdateFurColor(secondaryFur);
				secondaryTone = Tones.NOT_APPLICABLE;
				type = furryType;
				return true;
			}
		}
		public bool UpdateBody(FurBodyType furryType, FurColor primaryColor)
		{
			//same type? quick exit.
			if (type == furryType)
			{
				return false;
			}
			//you gave us an empty fur? dick. fallback to default.
			else if (primaryColor.isNoFur())
			{
				return UpdateBody(furryType);
			}
			//different types, but use the same underbody and primary fur? change type, quick exit.
			else if (type.isFurry && type.hasUnderBody == furryType.hasUnderBody)
			{
				primaryFur.UpdateFurColor(primaryColor);
				type = furryType;
				return true;
			}
			//different types, the current type has an underbody, the new one doesn't.
			else if (type.isFurry && type.hasUnderBody)
			{
				primaryFur.UpdateFurColor(primaryColor);
				secondaryFur.Reset();
				type = furryType;
				return true;
			}
			//different types, current type doesnt have an underbody, the new one does.
			else if (type.isFurry && !type.hasUnderBody)
			{
				primaryFur.UpdateFurColor(primaryColor);
				secondaryFur.UpdateFurColor(primaryFur);
				type = furryType;
				return true;
			}
			//old type is a tone type.
			else
			{
				if (primaryFur != primaryColor)
				{
					primaryFur.UpdateFurColor(primaryColor);
				}
				//initialize hair if empty.
				if (hairColor == HairFurColors.NO_HAIR_FUR) //!type.isFurry
				{
					hairColor = primaryFur.primaryColor;
				}
				//if we have a underbody color, we need to update it.
				if (furryType.hasUnderBody)
				{
					secondaryFur.UpdateFurColor(primaryFur);
				}
				//disable secondary tones if applicable. 
				secondaryTone = Tones.NOT_APPLICABLE;
				type = furryType;
				return true;
			}
		}
		public bool UpdateBody(FurBodyType furryType)
		{
			//same type? quick exit.
			if (type == furryType)
			{
				return false;
			}
			//different types, but use the same underbody and primary fur? change type, quick exit.
			else if (type.isFurry && type.hasUnderBody == furryType.hasUnderBody)
			{
				type = furryType;
				return true;
			}
			//different types, the current type has an underbody, the new one doesn't.
			else if (type.isFurry && type.hasUnderBody)
			{
				secondaryFur.Reset();
				type = furryType;
				return true;
			}
			//different types, current type doesnt have an underbody, the new one does.
			else if (type.isFurry && !type.hasUnderBody)
			{
				secondaryFur.UpdateFurColor(primaryFur);
				type = furryType;
				return true;
			}
			//old type is a tone type.
			else
			{
				//initialize hair if empty.
				if (hairColor == HairFurColors.NO_HAIR_FUR) //!type.isFurry
				{
					hairColor = furryType.defaultFurColor.primaryColor;
				}
				//set fur to it if fur is empty.
				if (primaryFur.isNoFur())
				{
					primaryFur.UpdateFurColor(hairColor);
				}
				//if we have a underbody color, we need to update it.
				if (furryType.hasUnderBody)
				{
					secondaryFur.UpdateFurColor(primaryFur);
				}
				//disable secondary tones if applicable. 
				secondaryTone = Tones.NOT_APPLICABLE;
				type = furryType;
				return true;
			}
		}

		public bool UpdateBody(ToneBodyType toneType, Tones primaryColor, Tones secondaryColor)
		{
			if (type == toneType)
			{
				return false;
			}
			else if (primaryColor == Tones.NOT_APPLICABLE && secondaryColor == Tones.NOT_APPLICABLE)
			{
				return UpdateBody(toneType);
			}
			else if (primaryColor == Tones.NOT_APPLICABLE && !toneType.hasUnderBody)
			{
				return UpdateBody(toneType);
			}
			else if (primaryColor == Tones.NOT_APPLICABLE)
			{
				if (type.isFurry)
				{
					primaryFur.Reset();
					secondaryFur.Reset();
				}
				secondaryTone = secondaryColor;
				type = toneType;
				return true;
			}
			else if (secondaryColor == Tones.NOT_APPLICABLE || !toneType.hasUnderBody)
			{
				return UpdateBody(toneType, primaryColor);
			}
			else
			{
				if (type.isFurry)
				{
					primaryFur.Reset();
					secondaryFur.Reset();
				}
				secondaryTone = secondaryColor;
				primaryTone = primaryColor;
				type = toneType;
				return true;
			}
		}
		public bool UpdateBody(ToneBodyType toneType, Tones primaryColor)
		{
			if (type == toneType)
			{
				return false;
			}
			else if (primaryColor == Tones.NOT_APPLICABLE)
			{
				return UpdateBody(toneType);
			}
			else if (!type.isFurry && toneType.hasUnderBody == type.hasUnderBody)
			{
				primaryTone = primaryColor;
				type = toneType;
				return true;
			}
			else if (!type.isFurry && type.hasUnderBody)
			{
				primaryTone = primaryColor;
				secondaryTone = Tones.NOT_APPLICABLE;
				type = toneType;
				return true;
			}
			else if (!type.isFurry && toneType.hasUnderBody)
			{
				primaryTone = primaryColor;
				secondaryTone = primaryTone;
				type = toneType;
				return true;
			}
			else
			{
				primaryFur.Reset();
				secondaryFur.Reset();
				primaryTone = primaryColor;
				type = toneType;
				return true;
			}
		}
		public bool UpdateBody(ToneBodyType toneType)
		{
			if (type == toneType)
			{
				return false;
			}
			else if (!type.isFurry && toneType.hasUnderBody == type.hasUnderBody)
			{
				type = toneType;
				return true;
			}
			else if (!type.isFurry && type.hasUnderBody)
			{
				secondaryTone = Tones.NOT_APPLICABLE;
				type = toneType;
				return true;
			}
			else if (!type.isFurry && toneType.hasUnderBody)
			{
				secondaryTone = primaryTone;
				type = toneType;
				return true;
			}
			else
			{
				primaryFur.Reset();
				secondaryFur.Reset();
				type = toneType;
				return true;
			}
		}

		public bool UpdateBodyAndDisplayMessage(FurBodyType furryType, FurColor primaryColor, FurColor secondaryColor, Player player)
		{
			//same type? quick exit.
			if (type == furryType)
			{
				return false;
			}
			OutputText(transformInto(furryType, player));
			return UpdateBody(furryType, primaryColor, secondaryColor);
		}
		public bool UpdateBodyAndDisplayMessage(FurBodyType furryType, FurColor primaryColor, Player player)
		{
			//same type? quick exit.
			if (type == furryType)
			{
				return false;
			}
			OutputText(transformInto(furryType, player));
			return UpdateBody(furryType, primaryColor);
		}
		public bool UpdateBodyAndDisplayMessage(FurBodyType furryType, Player player)
		{
			if (type == furryType)
			{
				return false;
			}
			OutputText(transformInto(furryType, player));
			return UpdateBody(furryType);
		}

		public bool UpdateBodyAndDisplayMessage(ToneBodyType toneType, Tones primaryColor, Tones secondaryColor, Player player)
		{
			if (type == toneType)
			{
				return false;
			}
			OutputText(transformInto(toneType, player));
			return UpdateBody(toneType, primaryColor, secondaryColor);
		}
		public bool UpdateBodyAndDisplayMessage(ToneBodyType toneType, Tones primaryColor, Player player)
		{
			if (type == toneType)
			{
				return false;
			}
			OutputText(transformInto(toneType, player));
			return UpdateBody(toneType, primaryColor);
		}
		public bool UpdateBodyAndDisplayMessage(ToneBodyType toneType, Player player)
		{
			if (type == toneType)
			{
				return false;
			}
			OutputText(transformInto(toneType, player));
			return UpdateBody(toneType);
		}

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
				item.reactToChangeInSkinTone(primaryTone, type.epidermisType.usesTone, secondaryTone);
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
				toneAware.reactToChangeInSkinTone(primaryTone, type.epidermisType.usesTone, secondaryTone);
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

	public abstract class BodyType : PiercableBodyPartBehavior<BodyType, Body, NavelPiercings>
	{
		private static int indexMaker = 0;

		public readonly bool hasUnderBody;
		public readonly GenericDescription underBodyDescription;
		public readonly EpidermisType epidermisType;
		public override int index => _index;
		private readonly int _index;


		protected BodyType(EpidermisType type, GenericDescription underbodyDescript,
			GenericDescription shortDesc, FullDescription<Body> fullDesc, PlayerDescription<Body> playerDesc,
			ChangeType<Body> transform, ChangeType<Body> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			epidermisType = type;
			underBodyDescription = underbodyDescript;
			hasUnderBody = underbodyDescript != GlobalStrings.None;
		}

		protected BodyType(EpidermisType type,
			GenericDescription shortDesc, FullDescription<Body> fullDesc, PlayerDescription<Body> playerDesc,
			ChangeType<Body> transform, ChangeType<Body> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			epidermisType = type;
			underBodyDescription = GlobalStrings.None;
			hasUnderBody = false;
		}

		public static readonly ToneBodyType HUMANOID = new ToneBodyType(EpidermisType.SKIN, Tones.LIGHT, SkinDesc, SkinFullDesc, SkinPlayerStr, SkinTransformStr, SkinRestoreStr);
		public static readonly ToneBodyType REPTILIAN = new ToneBodyType(EpidermisType.SCALES, Tones.DARK_RED, ScalesUnderbodyDesc, ScalesDesc, ScalesFullDesc, ScalesPlayerStr, ScalesTransformStr, ScalesRestoreStr);
		public static readonly ToneBodyType NAGA = new ToneBodyType(EpidermisType.SCALES, Tones.DARK_RED, NagaUnderbodyDesc, NagaDesc, NagaFullDesc, NagaPlayerStr, NagaTransformStr, NagaRestoreStr);
		public static readonly FurBodyType FEATHERED = new FurBodyType(EpidermisType.FEATHERS, FurColor.Generate(HairFurColors.WHITE), FeatherUnderbodyDesc, FeatherDesc, FeatherFullDesc, FeatherPlayerStr, FeatherTransformStr, FeatherRestoreStr);
		public static readonly ToneBodyType WOODEN = new ToneBodyType(EpidermisType.BARK, Tones.WOODLY_BROWN, BarkDesc, BarkFullDesc, BarkPlayerStr, BarkTransformStr, BarkRestoreStr);
		//one color (or two in a pattern, like zebra stripes) over the entire body.
		public static readonly FurBodyType SIMPLE_FUR = new FurBodyType(EpidermisType.FUR, FurColor.Generate(HairFurColors.BLACK), FurDesc, FurFullDesc, FurPlayerStr, FurTransformStr, FurRestoreStr);

		//the anthropomorphic equivalent of underbody, at least. this means that most of the body is the first color (or pattern), while the chest is the other. note that this may also
		//effect the arms, legs, and face (and possibly others if implemented), as they may utilize both or just one of these colors, depending on the type. 
		public static readonly FurBodyType UNDERBODY_FUR = new FurBodyType(EpidermisType.FUR, FurColor.Generate(HairFurColors.BLACK), FurUnderbodyDesc, FurDesc, FurFullDesc, FurPlayerStr, FurTransformStr, FurRestoreStr);
		public static readonly FurBodyType WOOL = new FurBodyType(EpidermisType.WOOL, FurColor.Generate(HairFurColors.WHITE), WoolUnderbodyDesc, WoolDesc, WoolFullDesc, WoolPlayerStr, WoolTransformStr, WoolRestoreStr);
		//now, if you have gooey body, give the goo innards perk. simple.
		public static readonly ToneBodyType GOO = new ToneBodyType(EpidermisType.GOO, Tones.CERULEAN, GooDesc, GooFullDesc, GooPlayerStr, GooTransformStr, GooRestoreStr);
		//cleaner - we don't need umpteen checks to see if it's "rubbery"
		public static readonly ToneBodyType RUBBER = new ToneBodyType(EpidermisType.RUBBER, Tones.GRAY, RubberDesc, RubberFullDesc, RubberPlayerStr, RubberTransformStr, RubberRestoreStr);
		//like a turtle shell
		public static readonly ToneBodyType CARAPACE = new ToneBodyType(EpidermisType.CARAPACE, Tones.BLACK, CarapaceStr, CarapaceFullDesc, CarapacePlayerStr, CarapaceTransformStr, CarapaceRestoreStr);
		//or chitin, if you prefer.
		public static readonly ToneBodyType EXOSKELETON = new ToneBodyType(EpidermisType.EXOSKELETON, Tones.BLACK, ExoskeletonStr, ExoskeletonFullDesc, ExoskeletonPlayerStr, ExoskeletonTransformStr, ExoskeletonRestoreStr);

		public bool isFurry => this is FurBodyType;
	}

	public class FurBodyType : BodyType
	{
		public readonly FurColor defaultFurColor;
		public FurBodyType(EpidermisType type, FurColor defFur,
			GenericDescription shortDesc, FullDescription<Body> fullDesc, PlayerDescription<Body> playerDesc,
			ChangeType<Body> transform, ChangeType<Body> restore) : base(type, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			defaultFurColor = FurColor.GenerateFromOther(defFur);
		}

		public FurBodyType(EpidermisType type, FurColor defFur, GenericDescription underbodyDesc,
			GenericDescription shortDesc, FullDescription<Body> fullDesc, PlayerDescription<Body> playerDesc,
			ChangeType<Body> transform, ChangeType<Body> restore) : base(type, underbodyDesc, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			defaultFurColor = FurColor.GenerateFromOther(defFur);
		}
	}

	public class ToneBodyType : BodyType
	{
		public readonly Tones defaultTone;
		public ToneBodyType(EpidermisType type, Tones defTone,
			GenericDescription shortDesc, FullDescription<Body> fullDesc, PlayerDescription<Body> playerDesc,
			ChangeType<Body> transform, ChangeType<Body> restore) : base(type, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			defaultTone = defTone;
		}

		public ToneBodyType(EpidermisType type, Tones defTone, GenericDescription underbodyDesc,
			GenericDescription shortDesc, FullDescription<Body> fullDesc, PlayerDescription<Body> playerDesc,
			ChangeType<Body> transform, ChangeType<Body> restore) : base(type, underbodyDesc, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			defaultTone = defTone;
		}
	}
}
