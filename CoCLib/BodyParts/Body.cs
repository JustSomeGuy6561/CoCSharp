using System;
using System.Collections.Generic;
using System.Diagnostics;
using CoC.Creatures;
using CoC.EpidermalColors;
using CoC.Strings;
using CoC.Tools;
using  CoC.BodyParts.SpecialInteraction;

using static   CoC.BodyParts.BodyStrings;
using static CoC.UI.TextOutput;
namespace  CoC.BodyParts
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
	 * TL;DR: when you call any update or restore function, this will automatically update body parts that implement itoneaware or ifuraware before returning. if you want to get data from these body parts 
	 * before they are altered, you must do so before calling an update or restore. 
	 */
	public class Body : PiercableBodyPart<Body, BodyType, NavelPiercings>, IHairAware
	{
		HashSet<IToneAware> toneAwares = new HashSet<IToneAware>();
		HashSet<IFurAware> furAwares = new HashSet<IFurAware>();


		private HairFurColors hairColor = HairFurColors.BLACK;
		private readonly FurColor primaryFur;
		private readonly FurColor secondaryFur;

		private Tones primaryTone;
		private Tones secondaryTone;


		public EpidermalData primaryEpidermis => new EpidermalData(type.epidermisType, primaryFur, primaryTone);

		public EpidermalData secondaryEpidermis => (type.hasUnderBody) ? new EpidermalData(type.secondaryEpidermisType, secondaryFur, secondaryTone) : null;

		public override BodyType type { get; protected set; }

		public bool isFurry => type.isFurry;
		public bool isTone => type.isTone;
		public bool isCockatrice => type.isCockatrice;

		protected Body(BodyType bodyType, PiercingFlags flags) : base(flags)
		{
			this.type = bodyType;
			this.primaryTone = Tones.LIGHT;
			this.hairColor = HairFurColors.BLACK;
			this.primaryFur = FurColor.GenerateEmpty();
			this.secondaryFur = FurColor.GenerateEmpty();
			this.secondaryTone = Tones.NOT_APPLICABLE;
			if (bodyType.isFurry)
			{
				FurBodyType furBody = (FurBodyType)type;
				this.hairColor = furBody.defaultFurColor.primaryColor;
				this.primaryFur.UpdateFurColor(furBody.defaultFurColor);
				if (furBody.hasUnderBody)
				{
					this.secondaryFur.UpdateFurColor(primaryFur);
				}
			}
			else if (bodyType.isTone)
			{
				ToneBodyType toneBody = (ToneBodyType)type;
				this.primaryTone = toneBody.defaultTone;
				if (toneBody.hasUnderBody)
				{
					this.secondaryTone = primaryTone;
				}
			}
			else //bodyType.isCockatrice
			{
				CockatriceBodyType cockatriceBody = (CockatriceBodyType)type;
				this.primaryFur = cockatriceBody.defaultFur;
				this.secondaryTone = cockatriceBody.defaultScales;
			}
		}

		//Default initializer. all values are set to species default, if applicable. 
		public static Body GenerateDefault(PiercingFlags flags, BodyType bodyType)
		{
			return new Body(bodyType, flags);
		}

		public static Body GenerateHumanoid(PiercingFlags flags, Tones skinTone)
		{
			return new Body(BodyType.HUMANOID, flags)
			{
				primaryTone = skinTone,
			};
		}

		//i hate you so much.
		public static Body GenerateCockatrice(PiercingFlags flags, FurColor featherColor, Tones scaleColor)
		{
			Body retVal = new Body(BodyType.COCKATRICE, flags);
			if (!featherColor.isNoFur())
			{
				retVal.primaryFur.UpdateFurColor(featherColor);
			}
			if (scaleColor != Tones.NOT_APPLICABLE)
			{
				retVal.secondaryTone = scaleColor;
			}
			return retVal;
		}

		public static Body GenerateTonedNoUnderbody(PiercingFlags flags, ToneBodyType toneBody, Tones primaryTone)
		{
			Body retVal = new Body(toneBody, flags);
			if (primaryTone != Tones.NOT_APPLICABLE)
			{
				retVal.primaryTone = primaryTone;
			}
			return retVal;
		}
		public static Body GenerateToneWithUnderbody(PiercingFlags flags, ToneBodyType toneBody, Tones primaryTone, Tones secondaryTone)
		{
			Body retVal = new Body(toneBody, flags);
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

		public static Body GenerateFurredNoUnderbody(PiercingFlags flags, FurBodyType furryBody, FurColor primaryFur)
		{
			Body retVal = new Body(furryBody, flags);
			if (!primaryFur.isNoFur())
			{
				retVal.primaryFur.UpdateFurColor(primaryFur);
			}
			return retVal;
		}
		public static Body GenerateFurredWithUnderbody(PiercingFlags flags, FurBodyType furryBody, FurColor primaryFur, FurColor secondaryFur)
		{
			Body retVal = new Body(furryBody, flags);
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
		
		public bool UpdateBody(CockatriceBodyType cockatriceBodyType, FurColor featherColor, Tones scaleTone)
		{
			if (type == cockatriceBodyType)
			{
				return false;
			}
			if (!featherColor.isNoFur())
			{
				this.primaryFur.UpdateFurColor(featherColor);
			}
			else if (primaryFur.isNoFur())
			{
				primaryFur.UpdateFurColor(cockatriceBodyType.defaultFur);
			}
			if (scaleTone != Tones.NOT_APPLICABLE)
			{
				this.secondaryTone = scaleTone;
			}
			else if (secondaryTone == Tones.NOT_APPLICABLE)
			{
				this.secondaryTone = primaryTone;
			}
			this.secondaryFur.Reset();
			type = cockatriceBodyType;
			return true;
		}
		public bool UpdateBody(CockatriceBodyType cockatriceBodyType)
		{
			if (type == cockatriceBodyType)
			{
				return false;
			}
			if (primaryFur.isNoFur())
			{
				if (hairColor != HairFurColors.NO_HAIR_FUR)
				{
					primaryFur.UpdateFurColor(hairColor);
				}
				else
				{
					primaryFur.UpdateFurColor(cockatriceBodyType.defaultFur);
				}
			}
			secondaryFur.Reset();
			if (this.secondaryTone == Tones.NOT_APPLICABLE)
			{
				secondaryTone = primaryTone;
			}
			type = cockatriceBodyType;
			return true;
		}

		public bool UpdateBody(FurBodyType furryType, FurColor primaryColor, FurColor secondaryColor)
		{
			//same type? quick exit.
			if (type == furryType)
			{
				return false;
			}
			else if (primaryColor.isNoFur() && secondaryColor.isNoFur())
			{
				return UpdateBody(furryType);
			}
			else if (!furryType.hasUnderBody || secondaryColor.isNoFur())
			{
				return UpdateBody(furryType, primaryColor);
			}

			//Check vals for if stuff changed. it's simpler this way.
			bool previouslyUsedTone = type.epidermisType.usesTone;
			FurColor firstFur = FurColor.GenerateFromOther(primaryFur);
			FurColor secondFur = FurColor.GenerateFromOther(secondaryFur);
			Tones firstTone = primaryTone;
			Tones secondTone = secondaryTone;

			//set the hair color.
			if (hairColor == HairFurColors.NO_HAIR_FUR && primaryColor.isNoFur())
			{
				hairColor = furryType.defaultFurColor.primaryColor;
			}
			else if (hairColor == HairFurColors.NO_HAIR_FUR)
			{
				hairColor = primaryColor.primaryColor;
			}
			//set the primary fur. if we can use the passed value, do so.
			if (!primaryColor.isNoFur())
			{
				primaryFur.UpdateFurColor(primaryColor);
			}
			//if not, and the fur is not currently set, use the hair value.
			else if (primaryFur.isNoFur())
			{
				primaryFur.UpdateFurColor(hairColor);
			}

			//set the secondary fur.
			if (furryType.hasUnderBody && secondaryFur.isNoFur())
			{
				secondaryFur.UpdateFurColor(primaryFur);
			}
			else if (!furryType.hasUnderBody)
			{
				secondaryFur.Reset();
			}

			secondaryTone = Tones.NOT_APPLICABLE;
			type = furryType;

			if (previouslyUsedTone || primaryTone != firstTone || secondaryTone != secondTone)
			{
				ToneChanged();
			}
			if (previouslyUsedTone || primaryFur != firstFur || secondaryFur != secondFur)
			{
				FurChanged();
			}
			return true;
		}
		public bool UpdateBody(FurBodyType furryType, FurColor primaryColor)
		{
			//same type? quick exit.
			if (type == furryType)
			{
				return false;
			}
			else if (primaryColor.isNoFur())
			{
				return UpdateBody(furryType);
			}
			//Check vals for if stuff changed. it's simpler this way.
			bool previouslyUsedTone = type.epidermisType.usesTone;
			FurColor firstFur = FurColor.GenerateFromOther(primaryFur);
			FurColor secondFur = FurColor.GenerateFromOther(secondaryFur);
			Tones firstTone = primaryTone;
			Tones secondTone = secondaryTone;


			if (hairColor == HairFurColors.NO_HAIR_FUR)
			{
				hairColor = primaryColor.primaryColor;
			}
			primaryFur.UpdateFurColor(primaryColor);
			//set the secondary fur.
			if (furryType.hasUnderBody && secondaryFur.isNoFur())
			{
				secondaryFur.UpdateFurColor(primaryFur);
			}
			else if (!furryType.hasUnderBody)
			{
				secondaryFur.Reset();
			}

			secondaryTone = Tones.NOT_APPLICABLE;
			type = furryType;

			if (previouslyUsedTone || primaryTone != firstTone || secondaryTone != secondTone)
			{
				ToneChanged();
			}
			if (previouslyUsedTone || primaryFur != firstFur || secondaryFur != secondFur)
			{
				FurChanged();
			}
			return true;
		}
		public bool UpdateBody(FurBodyType furryType)
		{
			//same type? quick exit.
			if (type == furryType)
			{
				return false;
			}
			//Check vals for if stuff changed. it's simpler this way.
			bool previouslyUsedTone = type.epidermisType.usesTone;
			FurColor firstFur = FurColor.GenerateFromOther(primaryFur);
			FurColor secondFur = FurColor.GenerateFromOther(secondaryFur);
			Tones firstTone = primaryTone;
			Tones secondTone = secondaryTone;


			//set the primary fur (and hair, if needed)
			if (primaryFur.isNoFur() && hairColor == HairFurColors.NO_HAIR_FUR)
			{
				hairColor = furryType.defaultFurColor.primaryColor;
				primaryFur.UpdateFurColor(furryType.defaultFurColor);
			}
			else if (primaryFur.isNoFur())
			{
				primaryFur.UpdateFurColor(hairColor);
			}
			//set the secondary fur.
			if (furryType.hasUnderBody && secondaryFur.isNoFur())
			{
				secondaryFur.UpdateFurColor(primaryFur);
			}
			else if (!furryType.hasUnderBody)
			{
				secondaryFur.Reset();
			}

			secondaryTone = Tones.NOT_APPLICABLE;
			type = furryType;

			if (previouslyUsedTone || primaryTone != firstTone || secondaryTone != secondTone)
			{
				ToneChanged();
			}
			if (previouslyUsedTone || primaryFur != firstFur || secondaryFur != secondFur)
			{
				FurChanged();
			}
			return true;
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
			else if (secondaryColor == Tones.NOT_APPLICABLE || !toneType.hasUnderBody)
			{
				return UpdateBody(toneType, primaryColor);
			}
			bool previouslyUsedFur = type.epidermisType.usesFur;
			FurColor firstFur = FurColor.GenerateFromOther(primaryFur);
			FurColor secondFur = FurColor.GenerateFromOther(secondaryFur);
			Tones firstTone = primaryTone;
			Tones secondTone = secondaryTone;

			primaryFur.Reset();
			secondaryFur.Reset();
			if (primaryColor != Tones.NOT_APPLICABLE)
			{
				primaryTone = primaryColor;
			}
			if (!toneType.hasUnderBody)
			{
				secondaryTone = Tones.NOT_APPLICABLE;
			}
			else
			{
				secondaryTone = secondaryColor;
			}
			type = toneType;
			if (previouslyUsedFur || primaryTone != firstTone || secondaryTone != secondTone)
			{
				ToneChanged();
			}
			if (previouslyUsedFur || primaryFur != firstFur || secondaryFur != secondFur)
			{
				FurChanged();
			}
			return true;
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

			bool previouslyUsedFur = type.epidermisType.usesFur;
			FurColor firstFur = FurColor.GenerateFromOther(primaryFur);
			FurColor secondFur = FurColor.GenerateFromOther(secondaryFur);
			Tones firstTone = primaryTone;
			Tones secondTone = secondaryTone;

			primaryFur.Reset();
			secondaryFur.Reset();
			primaryTone = primaryColor;
			if (!toneType.hasUnderBody)
			{
				secondaryTone = Tones.NOT_APPLICABLE;
			}
			else if (secondTone == Tones.NOT_APPLICABLE)
			{
				secondaryTone = primaryTone;
			}
			type = toneType;
			if (previouslyUsedFur || primaryTone != firstTone || secondaryTone != secondTone)
			{
				ToneChanged();
			}
			if (previouslyUsedFur || primaryFur != firstFur || secondaryFur != secondFur)
			{
				FurChanged();
			}
			return true;
		}
		public bool UpdateBody(ToneBodyType toneType)
		{
			if (type == toneType)
			{
				return false;
			}

			bool previouslyUsedFur = type.epidermisType.usesFur;
			FurColor firstFur = FurColor.GenerateFromOther(primaryFur);
			FurColor secondFur = FurColor.GenerateFromOther(secondaryFur);
			Tones firstTone = primaryTone;
			Tones secondTone = secondaryTone;

			primaryFur.Reset();
			secondaryFur.Reset();
			if (!toneType.hasUnderBody)
			{
				secondaryTone = Tones.NOT_APPLICABLE;
			}
			else if (secondTone == Tones.NOT_APPLICABLE)
			{
				secondaryTone = primaryTone;
			}
			type = toneType;
			if (previouslyUsedFur || primaryTone != firstTone || secondaryTone != secondTone)
			{
				ToneChanged();
			}
			if (previouslyUsedFur || primaryFur != firstFur || secondaryFur != secondFur)
			{
				FurChanged();
			}
			return true;
		}

		public bool UpdateBodyAndDisplayMessage(CockatriceBodyType cockatriceBodyType, Player player)
		{
			if (type == cockatriceBodyType)
			{
				return false;
			}
			OutputText(transformInto(cockatriceBodyType, player));
			return UpdateBody(cockatriceBodyType);
		}
		public bool UpdateBodyAndDisplayMessage(CockatriceBodyType cockatriceBodyType, FurColor featherColor, Tones scaleTone, Player player)
		{
			if (type == cockatriceBodyType)
			{
				return false;
			}
			OutputText(transformInto(cockatriceBodyType, player));
			return UpdateBody(cockatriceBodyType, featherColor, scaleTone);
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
			return UpdateBody(BodyType.HUMANOID);
		}

		public override bool RestoreAndDisplayMessage(Player player)
		{
			if (type == BodyType.HUMANOID)
			{
				return false;
			}
			OutputText(restoreString(player));
			return Restore();
		}

		protected override bool PiercingLocationUnlocked(NavelPiercings piercingLocation)
		{
			return true;
		}

		public bool AttachIToneAware(IToneAware item)
		{

			bool retVal = toneAwares.Add(item);
			if (retVal)
			{
				ToneAware += item.reactToChangeInSkinTone;
			}
			return retVal;
		}

		public bool DetachIToneAware(IToneAware item)
		{
			bool retVal = toneAwares.Remove(item);
			if (retVal)
			{
				ToneAware -= item.reactToChangeInSkinTone;
			}
			return retVal;
		}

		public bool AttachIFurAware(IFurAware item)
		{
			bool added = furAwares.Add(item);
			if (added)
			{
				FurAware += item.reactToChangeInFurColor;
			}
			return added;
		}

		public bool DetachIFurAware(IFurAware item)
		{
			bool removed = furAwares.Remove(item);
			if (removed)
			{
				FurAware -= item.reactToChangeInFurColor;
			}
			return removed;
		}

		private void ToneChanged()
		{
			toneEventArg.primaryTone = primaryTone;
			toneEventArg.secondaryTone = secondaryTone;
			toneEventArg.primaryToneActive = type.epidermisType.usesTone;
			OnToneAware(toneEventArg);
		}

		private void FurChanged()
		{
			furEventArg.primaryColor.UpdateFurColor(primaryFur);
			furEventArg.secondaryColor.UpdateFurColor(secondaryFur);
			OnFurAware(furEventArg);
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

		public void reactToChangeInHairColor(object sender, HairColorEventArg e)
		{
			if (e.hairColor != HairFurColors.RAINBOW && e.hairColor != HairFurColors.NO_HAIR_FUR)
			{
				hairColor = e.hairColor;
			}
		}

		//none of this is exposed to public. this is done to control adds and removes. 
		protected virtual void OnFurAware(FurAwareEventArg e)
		{
			FurAware?.Invoke(this, e);
		}
		
		private event EventHandler<FurAwareEventArg> FurAware;
		private readonly FurAwareEventArg furEventArg = new FurAwareEventArg(FurColor.GenerateEmpty(), FurColor.GenerateEmpty());

		protected virtual void OnToneAware(ToneAwareEventArg e)
		{
			ToneAware?.Invoke(this, e);
		}

		private event EventHandler<ToneAwareEventArg> ToneAware;
		private readonly ToneAwareEventArg toneEventArg = new ToneAwareEventArg();
	}

	public abstract class BodyType : PiercableBodyPartBehavior<BodyType, Body, NavelPiercings>
	{
		private static int indexMaker = 0;

		public readonly bool hasUnderBody;
		public readonly SimpleDescriptor underBodyDescription;
		public readonly EpidermisType epidermisType;
		public override int index => _index;
		private readonly int _index;

		public virtual EpidermisType secondaryEpidermisType => epidermisType;

		protected BodyType(EpidermisType type, SimpleDescriptor underbodyDescript,
			SimpleDescriptor shortDesc, DescriptorWithArg<Body> fullDesc, TypeAndPlayerDelegate<Body> playerDesc,
			ChangeType<Body> transform, RestoreType<Body> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			epidermisType = type;
			underBodyDescription = underbodyDescript;
			hasUnderBody = underbodyDescript != GlobalStrings.None;
		}

		protected BodyType(EpidermisType type,
			SimpleDescriptor shortDesc, DescriptorWithArg<Body> fullDesc, TypeAndPlayerDelegate<Body> playerDesc,
			ChangeType<Body> transform, RestoreType<Body> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			epidermisType = type;
			underBodyDescription = GlobalStrings.None;
			hasUnderBody = false;
		}

		public static readonly ToneBodyType HUMANOID = new ToneBodyType(EpidermisType.SKIN, Tones.LIGHT, SkinDesc, SkinFullDesc, SkinPlayerStr, SkinTransformStr, SkinRestoreStr);
		public static readonly ToneBodyType REPTILIAN = new ToneBodyType(EpidermisType.SCALES, Tones.DARK_RED, ScalesUnderbodyDesc, ScalesDesc, ScalesFullDesc, ScalesPlayerStr, ScalesTransformStr, ScalesRestoreStr);
		public static readonly ToneBodyType NAGA = new ToneBodyType(EpidermisType.SCALES, Tones.DARK_RED, NagaUnderbodyDesc, NagaDesc, NagaFullDesc, NagaPlayerStr, NagaTransformStr, NagaRestoreStr);
		public static readonly CockatriceBodyType COCKATRICE = new CockatriceBodyType(FurColor.Generate(HairFurColors.WHITE), Tones.TAN);
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
		//like a turtle shell or bee exoskeleton.
		public static readonly ToneBodyType CARAPACE = new ToneBodyType(EpidermisType.CARAPACE, Tones.BLACK, CarapaceStr, CarapaceFullDesc, CarapacePlayerStr, CarapaceTransformStr, CarapaceRestoreStr);

		public bool isFurry => this is FurBodyType;
		public bool isTone => this is ToneBodyType;
		public bool isCockatrice => this is CockatriceBodyType;
	}

	public class FurBodyType : BodyType
	{
		public readonly FurColor defaultFurColor;
		public FurBodyType(EpidermisType type, FurColor defFur,
			SimpleDescriptor shortDesc, DescriptorWithArg<Body> fullDesc, TypeAndPlayerDelegate<Body> playerDesc,
			ChangeType<Body> transform, RestoreType<Body> restore) : base(type, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			defaultFurColor = FurColor.GenerateFromOther(defFur);
		}

		public FurBodyType(EpidermisType type, FurColor defFur, SimpleDescriptor underbodyDesc,
			SimpleDescriptor shortDesc, DescriptorWithArg<Body> fullDesc, TypeAndPlayerDelegate<Body> playerDesc,
			ChangeType<Body> transform, RestoreType<Body> restore) : base(type, underbodyDesc, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			defaultFurColor = FurColor.GenerateFromOther(defFur);
		}
	}

	public class ToneBodyType : BodyType
	{
		public readonly Tones defaultTone;
		public ToneBodyType(EpidermisType type, Tones defTone,
			SimpleDescriptor shortDesc, DescriptorWithArg<Body> fullDesc, TypeAndPlayerDelegate<Body> playerDesc,
			ChangeType<Body> transform, RestoreType<Body> restore) : base(type, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			defaultTone = defTone;
		}

		public ToneBodyType(EpidermisType type, Tones defTone, SimpleDescriptor underbodyDesc,
			SimpleDescriptor shortDesc, DescriptorWithArg<Body> fullDesc, TypeAndPlayerDelegate<Body> playerDesc,
			ChangeType<Body> transform, RestoreType<Body> restore) : base(type, underbodyDesc, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			defaultTone = defTone;
		}
	}

	public class CockatriceBodyType : BodyType
	{
		public readonly FurColor defaultFur;
		public readonly Tones defaultScales;
		public CockatriceBodyType(FurColor feathers, Tones underbodyScales) : base(EpidermisType.FEATHERS, FeatherUnderbodyDesc, FeatherDesc, FeatherFullDesc, FeatherPlayerStr, FeatherTransformStr, FeatherRestoreStr)
		{
			defaultFur = feathers;
			defaultScales = underbodyScales;
		}

		public override EpidermisType secondaryEpidermisType => EpidermisType.SCALES;
	}

	public class FurAwareEventArg : EventArgs
	{
		public readonly FurColor primaryColor;
		public readonly FurColor secondaryColor;

		public FurAwareEventArg(FurColor primary, FurColor secondary)
		{
			primaryColor = FurColor.GenerateFromOther(primary);
			secondaryColor = FurColor.GenerateFromOther(secondary);
		}
	}

	public class ToneAwareEventArg : EventArgs
	{
		public Tones primaryTone, secondaryTone;
		public bool primaryToneActive;
	}

}
