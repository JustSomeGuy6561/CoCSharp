//Body.cs
//Description:
//Author: JustSomeGuy
//1/18/2019, 9:56 PM

using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Strings;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace CoC.Backend.BodyParts
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
	 * fur only applies if the body has it.
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

	/*
	 * To make things easier, everything that is updated by changes in the body is stored in the body. so, anything that uses the fur color or skin tone. at this point that's pretty much everything. 
	 * 
	 * Any part that uses these now expects to be correctly set when it is updated - there's no magic involved. note that these may implement some custom logic to parse it before setting it if they want,
	 * but other than that, it just works (TM).
	 * 
	 * Anything that has its own fur/hair/skin 
	 */

	[DataContract]
	public class Body : PiercableBodyPart<Body, BodyType, NavelPiercings>, ISerializable
	{

		//Hair, Fur, Tone
		//[Save]
		public HairFurColors hairColor { get; private set; }

		public EpidermalData primaryEpidermis => _primaryEpidermis.GetEpidermalData();
		public EpidermalData secondaryEpidermis => type.hasUnderBody ? _secondaryEpidermis.GetEpidermalData() : null;

		//types are always correct, as the body updates them when it changes.
		//[Save]
		internal Epidermis _primaryEpidermis;
		//[Save]
		internal Epidermis _secondaryEpidermis;

		private FurColor primaryFur => _primaryEpidermis.fur;
		private Tones primaryTone => _primaryEpidermis.tone;
		private FurColor secondaryFur => _secondaryEpidermis.fur;
		private Tones secondaryTone => _secondaryEpidermis.tone;
		//End Hair/Fur/Tone

		public override BodyType type
		{
			get => _type;
			protected set
			{
				if (_type != value)
				{
					value.UpdateEpidermisTypes(_primaryEpidermis, _secondaryEpidermis);
				}
				_type = value;
			}
		}
		//[Save]
		private BodyType _type;

		public override bool isDefault => type == BodyType.HUMANOID;

		public bool isFurry => type.isFurry;
		public bool isTone => type.isTone;
		public bool isCockatrice => type.isCockatrice;

		internal Body(BodyType bodyType)
		{
			if (hairColor == null || hairColor == HairFurColors.NO_HAIR_FUR)
			{
				hairColor = HairFurColors.BLACK;
			}
			this.hairColor = hairColor;
			type = bodyType;

			_primaryEpidermis = Epidermis.GenerateDefault(bodyType.epidermisType);
			_secondaryEpidermis = Epidermis.GenerateDefault(bodyType.epidermisType);
			if (bodyType.isFurry)
			{
				FurBodyType furBody = (FurBodyType)type;
				this.hairColor = furBody.defaultFurColor.primaryColor;
				_primaryEpidermis.UpdateEpidermis((FurBasedEpidermisType)type.epidermisType, furBody.defaultFurColor);
				if (furBody.hasUnderBody)
				{
					_secondaryEpidermis.UpdateEpidermis((FurBasedEpidermisType)type.epidermisType, furBody.defaultFurColor);
				}
			}
			else if (bodyType.isTone)
			{
				ToneBodyType toneBody = (ToneBodyType)type;
				_primaryEpidermis.UpdateEpidermis((ToneBasedEpidermisType)type.epidermisType, toneBody.defaultTone, true);
				if (toneBody.hasUnderBody)
				{
					_secondaryEpidermis.UpdateEpidermis((ToneBasedEpidermisType)type.epidermisType, toneBody.defaultTone);

				}
			}
			else //bodyType.isCockatrice
			{
				CockatriceBodyType cockatriceBody = (CockatriceBodyType)type;
				_primaryEpidermis.UpdateEpidermis((FurBasedEpidermisType)cockatriceBody.epidermisType, cockatriceBody.defaultFur);
				_secondaryEpidermis.UpdateEpidermis((ToneBasedEpidermisType)cockatriceBody.secondaryEpidermisType, cockatriceBody.defaultScales, true);
			}

			SetupAndValidateData();
		}

		#region Generate
		public static Body GenerateDefault()
		{
			return new Body(BodyType.HUMANOID);
		}
		public static Body GenerateDefaultOfType(BodyType bodyType)
		{
			return new Body(bodyType);
		}

		public static Body GenerateHumanoid(Tones skinTone)
		{
			Body retVal = new Body(BodyType.HUMANOID);
			retVal.updatePrimaryEpidermis(skinTone);
			return retVal;
		}

		//i hate you so much.
		public static Body GenerateCockatrice(FurColor featherColor, Tones scaleColor)
		{
			Body retVal = new Body(BodyType.COCKATRICE);
			if (!featherColor.isNoFur())
			{
				retVal.updatePrimaryEpidermis(featherColor);
			}
			if (scaleColor != Tones.NOT_APPLICABLE)
			{
				retVal.updateSecondaryEpidermis(scaleColor);
			}
			return retVal;
		}

		public static Body GenerateTonedNoUnderbody(ToneBodyType toneBody, Tones primaryTone)
		{
			Body retVal = new Body(toneBody);
			if (primaryTone != Tones.NOT_APPLICABLE)
			{
				retVal.updatePrimaryEpidermis(primaryTone);
			}
			return retVal;
		}
		public static Body GenerateToneWithUnderbody(ToneBodyType toneBody, Tones primaryTone, Tones secondaryTone)
		{
			Body retVal = new Body(toneBody);
			if (primaryTone != Tones.NOT_APPLICABLE)
			{
				retVal.updatePrimaryEpidermis(primaryTone);
			}
			if (secondaryTone != Tones.NOT_APPLICABLE && toneBody.hasUnderBody)
			{
				retVal.updateSecondaryEpidermis(secondaryTone);
			}
			return retVal;
		}

		public static Body GenerateFurredNoUnderbody(FurBodyType furryBody, FurColor primaryFur)
		{
			Body retVal = new Body(furryBody);
			if (!primaryFur.isNoFur())
			{
				retVal.updatePrimaryEpidermis(primaryFur);
			}
			return retVal;
		}
		public static Body GenerateFurredWithUnderbody(FurBodyType furryBody, FurColor primaryFur, FurColor secondaryFur)
		{
			Body retVal = new Body(furryBody);
			if (!primaryFur.isNoFur())
			{
				retVal.updatePrimaryEpidermis(primaryFur);
			}
			if (!secondaryFur.isNoFur())
			{
				retVal.updateSecondaryEpidermis(secondaryFur);
			}
			return retVal;
		}
		#endregion
		#region Updates

		public bool UpdateBody(CockatriceBodyType cockatriceBodyType, FurColor featherColor, Tones scaleTone)
		{
			if (type == cockatriceBodyType)
			{
				return false;
			}
			//if both null, make new one default.
			if (scaleTone == Tones.NOT_APPLICABLE && _secondaryEpidermis.tone == Tones.NOT_APPLICABLE)
			{
				scaleTone = cockatriceBodyType.defaultScales;
			}
			if (featherColor.isNoFur() && _primaryEpidermis.fur.isNoFur())
			{
				featherColor = cockatriceBodyType.defaultFur;
			}
			//otherwise, at least one is good.

			bool furChanged = _primaryEpidermis.usesTone || (type.hasUnderBody && _secondaryEpidermis.usesFur) || featherColor != _primaryEpidermis.fur;
			bool toneChanged = _primaryEpidermis.usesTone || (type.hasUnderBody && _secondaryEpidermis.usesFur) || scaleTone != _secondaryEpidermis.tone;

			//if only one fur is good, and it's the new color, replace the null color.
			if (!featherColor.isNoFur())
			{
				this.updatePrimaryEpidermis(featherColor);
			}
			//if it's the old color, do nothing.

			//do the same for tones.
			if (scaleTone != Tones.NOT_APPLICABLE)
			{
				updateSecondaryEpidermis(scaleTone);
			}

			if (furChanged)
			{
				FurChanged();
			}
			if (toneChanged)
			{
				ToneChanged();
			}
			type = cockatriceBodyType;
			return true;
		}
		public bool UpdateBody(CockatriceBodyType cockatriceBodyType)
		{
			bool bothChanged = type.epidermisType.usesTone || (type.hasUnderBody && _secondaryEpidermis.usesFur);
			if (type == cockatriceBodyType)
			{
				return false;
			}
			if (_primaryEpidermis.fur.isNoFur())
			{

				if (hairColor != HairFurColors.NO_HAIR_FUR)
				{
					updatePrimaryEpidermis(new FurColor(hairColor));
				}
				else
				{
					updatePrimaryEpidermis(cockatriceBodyType.defaultFur);
				}
			}
			if (_secondaryEpidermis.tone == Tones.NOT_APPLICABLE)
			{
				updateSecondaryEpidermis(cockatriceBodyType.defaultScales);
			}
			type = cockatriceBodyType;
			if (bothChanged)
			{
				FurChanged();
				ToneChanged();
			}
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

			FurColor firstFur = new FurColor(primaryFur);
			FurColor secondFur = new FurColor(secondaryFur);
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
				updatePrimaryEpidermis(primaryColor);
			}
			//if not, and the fur is not currently set, use the hair value.
			else if (primaryFur.isNoFur())
			{
				updatePrimaryEpidermis(new FurColor(hairColor));
			}

			//set the secondary fur.
			if (furryType.hasUnderBody)
			{
				updateSecondaryEpidermis(secondaryColor);
			}
			else
			{
				_secondaryEpidermis.Reset();
			}

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
			FurColor firstFur = new FurColor(primaryFur);
			FurColor secondFur = new FurColor(secondaryFur);
			Tones firstTone = primaryTone;
			Tones secondTone = secondaryTone;


			if (hairColor == HairFurColors.NO_HAIR_FUR)
			{
				hairColor = primaryColor.primaryColor;
			}
			updatePrimaryEpidermis(primaryColor);
			//set the secondary fur.
			if (furryType.hasUnderBody && secondaryFur.isNoFur())
			{
				updateSecondaryEpidermis(primaryColor);
			}
			//or clear it
			else if (!furryType.hasUnderBody)
			{
				_secondaryEpidermis.Reset();
			}

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
			FurColor firstFur = new FurColor(primaryFur);
			FurColor secondFur = new FurColor(secondaryFur);
			Tones firstTone = primaryTone;
			Tones secondTone = secondaryTone;


			//set the primary fur (and hair, if needed)
			if (primaryFur.isNoFur() && hairColor == HairFurColors.NO_HAIR_FUR)
			{
				hairColor = furryType.defaultFurColor.primaryColor;
				updatePrimaryEpidermis(furryType.defaultFurColor);
			}
			else if (primaryFur.isNoFur())
			{
				updatePrimaryEpidermis(new FurColor(hairColor));
			}
			//set the secondary fur.
			if (furryType.hasUnderBody && secondaryFur.isNoFur())
			{
				updateSecondaryEpidermis(primaryFur);
			}
			else if (!furryType.hasUnderBody)
			{
				_secondaryEpidermis.Reset();
			}

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
			FurColor firstFur = new FurColor(primaryFur);
			FurColor secondFur = new FurColor(secondaryFur);
			Tones firstTone = primaryTone;
			Tones secondTone = secondaryTone;


			if (primaryColor != Tones.NOT_APPLICABLE)
			{
				updatePrimaryEpidermis(primaryColor);
			}
			if (toneType.hasUnderBody)
			{
				updateSecondaryEpidermis(secondaryColor);
			}
			else
			{
				_secondaryEpidermis.Reset();
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
			FurColor firstFur = new FurColor(primaryFur);
			FurColor secondFur = new FurColor(secondaryFur);
			Tones firstTone = primaryTone;
			Tones secondTone = secondaryTone;

			updatePrimaryEpidermis(primaryColor);
			if (toneType.hasUnderBody && secondaryTone == Tones.NOT_APPLICABLE)
			{
				updateSecondaryEpidermis(primaryColor);
			}
			else
			{
				_secondaryEpidermis.Reset();
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
			FurColor firstFur = new FurColor(primaryFur);
			FurColor secondFur = new FurColor(secondaryFur);
			Tones firstTone = primaryTone;
			Tones secondTone = secondaryTone;

			if (!toneType.hasUnderBody)
			{
				_secondaryEpidermis.Reset();
			}
			else if (secondaryTone == Tones.NOT_APPLICABLE)
			{
				updateSecondaryEpidermis(primaryTone);
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
		#endregion
		#region Restore
		internal override bool Restore()
		{
			if (type == BodyType.HUMANOID)
			{
				return false;
			}
			return UpdateBody(BodyType.HUMANOID);
		}
		#endregion
		#region Helpers
		protected override bool PiercingLocationUnlocked(NavelPiercings piercingLocation)
		{
			return true;
		}

		private void UpdateEpidermis(Epidermis epidermis, FurBasedEpidermisType furType, FurColor fur)
		{

		}

		private void UpdateEpidermis(Epidermis epidermis, ToneBasedEpidermisType toneType, Tones tone)
		{

		}

		private void ChangeEpidermis(Epidermis epidermis, Tones tone)
		{
			epidermis.ChangeTone(tone, true);
		}

		private void ChangeEpidermis(Epidermis epidermis, FurColor fur)
		{
			epidermis.ChangeFur(fur, true);
		}

		#endregion
		#region Serialization
		protected Body(SerializationInfo info, StreamingContext context)
		{
			type = BodyType.Deserialize(info.GetInt32(nameof(type)));
			_primaryEpidermis = (Epidermis)info.GetValue(nameof(_primaryEpidermis), typeof(Epidermis));
			_secondaryEpidermis = (Epidermis)info.GetValue(nameof(_secondaryEpidermis), typeof(Epidermis));
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(nameof(type), type.index);
			info.AddValue(nameof(_primaryEpidermis), _primaryEpidermis, typeof(Epidermis));
			info.AddValue(nameof(_secondaryEpidermis), _secondaryEpidermis, typeof(Epidermis));
		}
		#endregion
	}

	public abstract partial class BodyType : PiercableBodyPartBehavior<BodyType, Body, NavelPiercings>
	{
		private static int indexMaker = 0;

		private static List<BodyType> bodyTypes = new List<BodyType>();

		public readonly bool hasUnderBody;
		public readonly SimpleDescriptor underBodyDescription;
		public readonly EpidermisType epidermisType;
		public override int index => _index;
		private readonly int _index;

		public virtual void UpdateEpidermisTypes(Epidermis primary, Epidermis secondary)
		{
			primary.UpdateEpidermis(epidermisType);
			secondary.UpdateEpidermis(secondaryEpidermisType);
		}

		public virtual EpidermisType secondaryEpidermisType => epidermisType;

		protected BodyType(EpidermisType type, SimpleDescriptor underbodyDescript,
			SimpleDescriptor shortDesc, DescriptorWithArg<Body> fullDesc, TypeAndPlayerDelegate<Body> playerDesc,
			ChangeType<Body> transform, RestoreType<Body> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			epidermisType = type;
			underBodyDescription = underbodyDescript;
			hasUnderBody = underbodyDescript != GlobalStrings.None;

			_index = indexMaker++;
			bodyTypes[_index] = this;
		}

		protected BodyType(EpidermisType type,
			SimpleDescriptor shortDesc, DescriptorWithArg<Body> fullDesc, TypeAndPlayerDelegate<Body> playerDesc,
			ChangeType<Body> transform, RestoreType<Body> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			epidermisType = type;
			underBodyDescription = GlobalStrings.None;
			hasUnderBody = false;

			_index = indexMaker++;
			bodyTypes[_index] = this;
		}

		public static BodyType Deserialize(int index)
		{
			if (index < 0 || index >= bodyTypes.Count)
			{
				throw new System.ArgumentException("index for body type deserialize out of range");
			}
			else
			{
				BodyType body = bodyTypes[index];
				if (body != null)
				{
					return body;
				}
				else
				{
					throw new System.ArgumentException("index for arm type points to an object that does not exist. this may be due to obsolete code");
				}
			}
		}

		//apparently cat, fox, wolf, horse, and dog use fur underbody, kindof. 

		public static readonly ToneBodyType HUMANOID = new ToneBodyType(EpidermisType.SKIN, Tones.LIGHT, SkinDesc, SkinFullDesc, SkinPlayerStr, SkinTransformStr, SkinRestoreStr);
		public static readonly ToneBodyType REPTILIAN = new ToneBodyType(EpidermisType.SCALES, Tones.DARK_RED, ScalesUnderbodyDesc, ScalesDesc, ScalesFullDesc, ScalesPlayerStr, ScalesTransformStr, ScalesRestoreStr);
		public static readonly ToneBodyType NAGA = new ToneBodyType(EpidermisType.SCALES, Tones.DARK_RED, NagaUnderbodyDesc, NagaDesc, NagaFullDesc, NagaPlayerStr, NagaTransformStr, NagaRestoreStr);
		public static readonly CockatriceBodyType COCKATRICE = new CockatriceBodyType(new FurColor(HairFurColors.WHITE), Tones.TAN);
		public static readonly ToneBodyType WOODEN = new ToneBodyType(EpidermisType.BARK, Tones.WOODLY_BROWN, BarkDesc, BarkFullDesc, BarkPlayerStr, BarkTransformStr, BarkRestoreStr);
		//one color (or two in a pattern, like zebra stripes) over the entire body.
		public static readonly FurBodyType SIMPLE_FUR = new FurBodyType(EpidermisType.FUR, new FurColor(HairFurColors.BLACK), FurDesc, FurFullDesc, FurPlayerStr, FurTransformStr, FurRestoreStr);

		//the anthropomorphic equivalent of underbody, at least. this means that most of the body is the first color (or pattern), while the chest is the other. note that this may also
		//effect the arms, legs, and face (and possibly others if implemented), as they may utilize both or just one of these colors, depending on the type. 
		public static readonly FurBodyType UNDERBODY_FUR = new FurBodyType(EpidermisType.FUR, new FurColor(HairFurColors.BLACK), FurUnderbodyDesc, FurDesc, FurFullDesc, FurPlayerStr, FurTransformStr, FurRestoreStr);
		public static readonly FurBodyType WOOL = new FurBodyType(EpidermisType.WOOL, new FurColor(HairFurColors.WHITE), WoolUnderbodyDesc, WoolDesc, WoolFullDesc, WoolPlayerStr, WoolTransformStr, WoolRestoreStr);
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
		internal FurBodyType(EpidermisType type, FurColor defFur,
			SimpleDescriptor shortDesc, DescriptorWithArg<Body> fullDesc, TypeAndPlayerDelegate<Body> playerDesc,
			ChangeType<Body> transform, RestoreType<Body> restore) : base(type, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			defaultFurColor = new FurColor(defFur);
		}

		internal FurBodyType(EpidermisType type, FurColor defFur, SimpleDescriptor underbodyDesc,
			SimpleDescriptor shortDesc, DescriptorWithArg<Body> fullDesc, TypeAndPlayerDelegate<Body> playerDesc,
			ChangeType<Body> transform, RestoreType<Body> restore) : base(type, underbodyDesc, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			defaultFurColor = new FurColor(defFur);
		}
	}

	public class ToneBodyType : BodyType
	{
		public readonly Tones defaultTone;
		internal ToneBodyType(EpidermisType type, Tones defTone,
			SimpleDescriptor shortDesc, DescriptorWithArg<Body> fullDesc, TypeAndPlayerDelegate<Body> playerDesc,
			ChangeType<Body> transform, RestoreType<Body> restore) : base(type, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			defaultTone = defTone;
		}

		internal ToneBodyType(EpidermisType type, Tones defTone, SimpleDescriptor underbodyDesc,
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
		internal CockatriceBodyType(FurColor feathers, Tones underbodyScales) : base(EpidermisType.FEATHERS, FeatherUnderbodyDesc, FeatherDesc, FeatherFullDesc, FeatherPlayerStr, FeatherTransformStr, FeatherRestoreStr)
		{
			defaultFur = feathers;
			defaultScales = underbodyScales;
		}

		public override EpidermisType secondaryEpidermisType => EpidermisType.SCALES;
	}
}
