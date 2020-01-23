//CreatureCreator.cs
//Description:
//Author: JustSomeGuy
//3/22/2019, 6:12 PM
using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Backend.Perks;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CoC.Backend.Creatures
{
	/* DESIGNER'S NOTE (if not relevant to you, feel free to ignore)
	 * everything in here must have a default value of something that does not force an unintended behavior.
	 * for example, large or small wings can be set with a boolean, however, we don't want to accidently force all wings to small
	 * if the content creator doesn't set it (the default for a bool is false). for this, we must use a Nullable<boolean> (aka bool?)
	 * so that the default value is null. in this way, if the value is set to false, it is expected to be small, as null is our default.
	 * with a null check, we can then use default behavior if null, and override that behavior if it is set to small (false) or large (true).
	 *
	 * Note that if we can use a default and not force unintended behavior (like setting an enum property to it's default value, that we'd use for default anyway)
	 * we are able to set that here without requiring nullable.
	 */

	//The following is designed to be easily created using C#'s object initializer paradigm, something i find immensely useful.
	//As such, i've created helpers for arrays, which would otherwise mess up this idea. You may find this confusing and thus choose not to use them and instead
	//initialize the object normally (or a hybrid between object initializer and standard). Do whatever you're comfortable with.

	public abstract class CreatureCreator
	{
		protected CreatureCreator(string creatureName)
		{
			name = creatureName;
		}
		public string name;

		//stats.
		public byte? sensitivity;
		public byte? libido;
		public byte? corruption;

		//The following can be left alone, as they will default to human values. You only need to define these if they aren't human values.
		public byte heightInInches = 0; //min height is like 3 feet something, idk. so if it's less than that, we'll treat it as min height, however, 0 is treated as not set.

		//Antennae
		public AntennaeType antennaeType;
		//Arm
		public ArmType armType;
		//Back
		public BackType backType;
		public HairFurColors backHairFur;
		//Body
		public BodyType bodyType;
		public Tones complexion; //NOTE: This can be different between humans. It does have a default, but feel free to change it anyway.
		public FurColor furColor;
		public Tones underTone;
		public FurColor underFurColor;
		public FurTexture? furTexture = null;
		public FurTexture? underBodyFurTexture = null;
		public SkinTexture? underBodySkinTexture = null;

		//Ears
		public EarType earType;
		//Face
		public FaceType faceType;
		public bool? isFaceFullMorph = null;
		//Eyes
		public EyeType eyeType;
		//Gills
		public GillType gillType;
		//Hair
		public HairType hairType;
		public bool hairTransparent = false;
		//Horns
		//NOTE: horns can be defined in one of two ways: transformation level or explicit horn count and length. While it's recommended to use the tf level and
		//let the game logic run its course, you may have NPCs with specific lengths and horns that may be difficult or impossible to get naturally, so you have that option too.

		//If both length and count are specified, the game will prioritize them for creation. if neither, or only one is specified, the game will use the transformation level.
		//if using the transformation level, you have the option to ignore random growth values. This will only apply during creation, randoms will be used in normal gameplay.
		public HornType hornType;

		//Horn option1: TransformLevel
		public byte additionalHornTransformStrength = 0;
		public bool forceUniformHornGrowthOnCreate = false;

		//Horn option2: Explicit values.
		public byte? hornCount = null;
		public byte? hornSize = null;
		//Lower Body
		public LowerBodyType lowerBodyType;
		//Neck
		public NeckType neckType;
		public byte neckLength = NeckType.MIN_NECK_LENGTH; //ignored if type does not support neck length.

		//Tail
		public byte? tailCount = null;
		public TailType tailType;
		public JewelryType[] tailPiercings = null;
		//Ovipositor - subset of tail, just like hands or feet of arms and legs, respectively. but you can have the tail w/o the ovipositor.
		//by default, you do not have an ovipositor. if the type does not support an ovipositor and this flag is set to true, it will be ignored.
		public bool hasOvipositorIfApplicable = false;

		//Tongue
		public TongueType tongueType;
		//Wings
		public WingType wingType;
		public HairFurColors wingFeatherColor;
		public Tones primaryWingTone;
		public Tones secondaryWingTone;
		public bool? largeWings;


		//NOTE: The following have defaults, but you may wish to change these, even for human pre-defined characters. Some of these can be changed during creation, if not defined here.
		//Body
		public SkinTexture? skinTexture = null;
		public Dictionary<NavelPiercingLocation, PiercingJewelry> navelPiercings = null;
		//may add hip piercings because that's a thing (more common in art than reality in my experience but whatever) Edit: added.
		public Dictionary<HipPiercingLocation, PiercingJewelry> hipPiercings = null;
		//Ears
		public Dictionary<EarPiercingLocation, PiercingJewelry> earPiercings = null;
		//Face
		public Dictionary<EyebrowPiercingLocation, PiercingJewelry> eyebrowPiercings = null; //Eyebrow
		public Dictionary<LipPiercingLocation, PiercingJewelry> lipPiercings = null; //Lip
		public Dictionary<NosePiercingLocation, PiercingJewelry> nosePiercings = null; //Nose
		public SkinTexture? facialSkinTexture = null; //lets you have freckles i guess.

		//Eyes
		//Note: If one eye color is defined, both eyes are given that color. I'd prefer you use left eye for that purpose, but either works.
		public EyeColor? leftEyeColor = null;
		public EyeColor? rightEyeColor = null;
		//Hair
		public HairFurColors hairColor;
		public HairFurColors hairHighlightColor;
		public float? hairLength = null;
		public HairStyle hairStyle = HairStyle.NO_STYLE;
		//Tongue
		public Dictionary<TonguePiercingLocation, PiercingJewelry> tonguePiercings = null;

		//Ass
		public AnalLooseness analLooseness = AnalLooseness.NORMAL;
		public AnalWetness analWetness = AnalWetness.NORMAL;
		public bool assVirgin = true;
		public bool? hasAnalPractice = null;

		//Note: The following have no default values, and therefore cannot be determined. The player's decisions during character creation determine these values.
		//HOWEVER, You may choose to define these yourself, in which case the game will use these instead.
		//If possible, the game will use these defined values to determine any of the remaining undefined values, but if this is not possible, the game will prompt the player
		//to choose an option. It is thus recommended to familiarize yourself with how the characters are created, or err on the side of caution and define extra values.

		//This is done to clean up the old code, especially with partially-defined PCs with no history. Often, their attributes would be ignored or overridden,
		//or worse still, compounded into strange results. (extra breasts, herms when male/female was selected, etc, due to preset values)
		//Hopefully this new design will yield more consistent, and yet more flexible, behavior.

		//GENDER:

		//You have the option to set a default gender. Players have the ability to override this, however. You can prevent this by setting the forceDefaultGender to true.
		//If defaultGender is null, force default is ignored. Note that this is the only means for Neuter characters being created, as it's not an option in game.
		//Note that for non-player characters, we will default to Male, though this is really not ideal imo.
		public Gender? defaultGender = null;
		public bool forceDefaultGender = false;

		//you can further supplement the above data by providing custom cock(s) and/or vagina(s), instead of just letting them be the default values.
		//These values will be used wherever possible. For example, if the player can choose their own gender, the cock data will be used (if any available) if they choose male.
		//the same applies for vagina data for females, and both for herms. Similarly, if the gender is locked, the corresponding gender data will be used for that gender.
		//Note that any options set that do not apply to the selected or locked gender will be ignored.

		//For now, breasts will be used regardless of gender. I could (and probably should) create a means for male/female/herm breast options.

		public CockCreator[] cocks = null;
		public bool cockVirgin = true;
		public VaginaCreator[] vaginas = null;
		public float? cumMultiplier = null; //if have cocks, defaults to 1, otherwise 0.

		//i suppose this could be a perk (and may make more sense that way), but w/e.
		public bool hasOmnibusClit = false;

		//Balls
		public byte? numBalls;
		public byte? ballSize;

		//Womb

		//womb is not sealed; any creature that needs their own specific version can derive it and do what they need to. Player creature, for example, does this.
		//to accomodate this, you have an optional override womb constructor callback, where you can provide your own function that returns a womb.
		public Func<Guid, Womb> overrideWombConstructor;
		//if this is null, it will fallback to the boolean values below.

		//can this creature get pregnant at all? if false, the remaining booleans will be ignored.
		public bool allowsAnyPregnancies = false;
		//can this creature get vaginally pregnant? this boolean applies to all vaginas, and thus, all vaginal pregnancies.
		public bool allowsVaginalPregnancies = false;
		//can this creature get anally pregnant, assuming the conditions are met?
		//if true, anal pregnancies will only occur when the spawn type allows anal pregnancies and forces it. if this is set to false, the creature will never get anally pregnant.
		public bool allowsDefaultAnalPregnancies;

		//in all cases, the creature can only get pregnant if the have the required endowments. Note that these cannot be changed after creation.
		//a creature can also be rendered infertile through other means; this does not affect the womb itself.

		public virtual Womb GetWomb(Guid creatureID)
		{
			if (!(overrideWombConstructor is null))
			{
				var womb = overrideWombConstructor(creatureID);
				if (!(womb is null))
				{
					return womb;
				}
				else
				{
#if DEBUG
					Debug.WriteLine("Womb constructor override returned null. falling back to default.");
#endif
				}
			}
			else if (allowsAnyPregnancies)
			{
				return new Womb(creatureID, allowsVaginalPregnancies, allowsDefaultAnalPregnancies);
			}

			//fall through, default case.

			return new Womb(creatureID, false, false);
		}

		//BUILD:

		//Using the above gender information, the build is then determined.

		////if set to true, will ignore the fact that the values in this group dont match the gender.
		//public bool androgyny = false;

		//To prevent players from choosing this themselves, thickness must be set. If you choose to do this, it's also recommended you at least set the femininity
		//especially for herms or genderless characters. For now, values that don't match the chosen gender are allowed, but this may change if the androgyny bool is implemented
		//Tone, buttSize, and hipSize can be left alone if you don't mind the default values, which can be determined from the thickness (and femininity, if applicable)
		//The same goes for the breasts, which will create a single pair based on gender, thickness, and femininity. Again, you may choose to override this.

		//Dev's note: May want to provide another means for breast row customization - perhaps they're fine with matching the build the player chooses, but wants multiple rows for some animal morph.

		//Femininity
		public byte? femininity;
		//Fertility
		public byte? fertility;
		public bool artificiallyInfertile = false; //taking contraceptives or something? ignored for player characters.

		//Build data
		public byte? thickness;
		public byte? tone;
		public byte? buttSize;
		public byte? hipSize;


		//Breasts
		public BreastCreator[] breasts = null;
		public NippleStatus? nippleStatus = null; //allows you to make nipples fuckable or whatever.
		public bool blackNipples = false;
		public bool quadNipples = false;
		public float? nippleLength = null; //if unset, will use the default value for the largest given cup size. if set, use the max between this value and the default for largest cup size.
		public bool unlockedDickNipples = false;
		public float lactationMultiplier = 0;

		//perks

		public List<PerkBase> perks;
	}
}
