using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Items.Wearables.Piercings;

namespace CoC.Backend.Creatures
{
	//everything in here must have a default value of something that does force an unintended behavior.
	//for example, large or small wings can be set with a boolean, however, we don't want to accidently force all wings to small
	//if the content creator doesn't set it (the default for a bool is false). for this, we must use a Nullable<boolean> (aka bool?)
	//so that the default value is null. in this way, if the value is set to false, it is expected to be small, as null is our default.
	//with a null check, we can then use default behavior if null, and override that behavior if it is set to small (false) or large (true).

	//Note that if we can use a default and not force unintended behavior (like setting an enum property to it's default value, that we'd use for default anyway)
	//we are able to set that here without requiring nullable.

	public abstract class CreatureCreator
	{
		//Antennae
		public AntennaeType antennaeType;
		//Arm
		public ArmType armType;
		//Back
		public BackType backType;
		public HairFurColors backHairFur;
		//Body
		public BodyType bodyType;
		public Tones tone;
		public FurColor furColor;
		public Tones underTone;
		public FurColor underFurColor;
		public FurTexture furTexture = FurTexture.NONDESCRIPT;
		public SkinTexture skinTexture = SkinTexture.NONDESCRIPT;
		public FurTexture underBodyFurTexture = FurTexture.NONDESCRIPT;
		public SkinTexture underBodySkinTexture = SkinTexture.NONDESCRIPT;
		public PiercingJewelry[] navelPiercings = null;
		//Ears
		public EarType earType;
		public PiercingJewelry[] earPiercings = null;
		//Eyebrow
		//public bool[] eyebrowPiercings = null;
		//Eyes
		public EyeType eyeType;
		public EyeColor? leftEyeColor;
		public EyeColor? rightEyeColor;
		//Gills
		public GillType gillType;
		//Hair
		public HairType hairType;
		public HairFurColors hairColor;
		public HairFurColors hairHighlightColor;
		public float? hairLength = null;
		//Horns
		//NOTE: horns can be defined in one of two ways: transformation level or explicit horn count and length. While it's recommended to use the tf level and 
		//let the game logic run its course, you may have NPCs with specific lengths and horns that may be difficult or impossible to get naturally, so you have that option too.

		//If both length and count are specified, the game will prioritize them for creation. if neither, or only one is specified, the game will use the transformation level.
		//if using the transformation level, you have the option to ignore random growth values. This will only apply during creation, randoms will be used in normal gameplay.
		public HornType hornType;

		//Horn option1: TransformLevel
		public int additionalHornTransformStrength = 0;
		public bool forceUniformHornGrowthOnCreate = false;

		//Horn option2: Explicit values.
		public int? hornCount = null;
		public int? hornSize = null;

		//Tongue
		public TongueType tongueType;
		public PiercingJewelry[] tonguePiercings = null;
		//Wings
		public WingType wingType;
		public HairFurColors wingFeatherColor;
		public Tones wingTone;
		public bool? largeWings;

	}
}