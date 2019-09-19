//Creature.cs
//Description:
//Author: JustSomeGuy
//2/20/2019, 4:13 PM
using CoC.Backend.BodyParts;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Engine;
using CoC.Backend.Engine.Time;
using CoC.Backend.Perks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace CoC.Backend.Creatures
{
	//creature class breaks with non-standard creatures - it's generall 
	public enum CreatureType { STANDARD, PRIMITIVE, ARTIFICIAL }

	public abstract class Creature
	{

		public readonly string name;

		public readonly Antennae antennae;
		public readonly Arms arms;
		public readonly Back back;
		//public readonly Beard beard;
		public readonly Body body;
		public readonly Build build;
		public readonly Ears ears;
		public readonly Eyes eyes;
		public readonly Face face;

		public readonly Genitals genitals;
		public readonly Gills gills;
		public readonly Hair hair;
		public readonly Horns horns;
		public readonly LowerBody lowerBody;
		public readonly Neck neck;
		public readonly Tail tail;
		public readonly Tongue tongue;
		public readonly Wings wings;

		//aliases for build.
		public Butt butt => build.butt;
		public Hips hips => build.hips;

		//aliases for genitals.
		public Ass ass => genitals.ass;
		public ReadOnlyCollection<Breasts> breasts => genitals.breasts;
		public ReadOnlyCollection<Cock> cocks => genitals.cocks;
		public ReadOnlyCollection<Vagina> vaginas => genitals.vaginas;
		public Balls balls => genitals.balls;

		public ReadOnlyCollection<Clit> clits => genitals.clits;
		public ReadOnlyCollection<Nipples> nipples => genitals.nipples;

		//aliases for arms/legs
		public Hands hands => arms.hands;
		public Feet feet => lowerBody.feet;

		public readonly PerkCollection perks;

		protected BasePerkModifiers modifiers => perks.baseModifiers;

		//NOTE: WE HAVE A DESTRUCTOR/FINALIZER. IT'S AT THE BOTTOM!
		#region Constructors
		protected Creature(CreatureCreator creator)
		{
			if (creator == null) throw new ArgumentNullException();

			name = creator.name;
			//semantically, we Should do the things other parts can depend on first, but as long as we
			//dont actually require the data in the generate functions (which we generally shouldn't - that's why we're lazy)
			//it won't matter. Anything that needs this stuff for validation

			if (creator.bodyType is null)
			{
				body = new Body(this);
			}
			else
			{
				body = new Body(this, creator.bodyType, creator.furColor, creator.furTexture, creator.complexion, creator.skinTexture, creator.underFurColor,
					creator.underBodyFurTexture, creator.underTone, creator.underBodySkinTexture);
			}

			//build
			if (creator.heightInInches == 0)
			{
				creator.heightInInches = Build.DEFAULT_HEIGHT;
			}
			build = new Build(this, creator.heightInInches, creator.thickness, creator.tone, creator.hipSize, creator.buttSize);

			//genitals

			Gender gender;
			if (creator.defaultGender != null)
			{
				gender = (Gender)creator.defaultGender;
			}
			else if (creator.cocks?.Length > 0 || creator.vaginas?.Length > 0)
			{
				gender = Gender.GENDERLESS;
				if (creator.cocks?.Any(x => x != null) == true)
				{
					gender |= Gender.MALE;
				}
				if (creator.vaginas?.Any(x => x != null) == true)
				{
					gender |= Gender.FEMALE;
				}
			}
			else
			{
#if DEBUG
				Debug.WriteLine("No gender data available. Falling back to default gender of Male");
#endif
				gender = Gender.MALE;
			}


			Ass ass = new Ass(this, creator.analWetness, creator.analLooseness, creator.assVirgin, creator.hasAnalPractice);

			Balls balls;
			if (creator.numBalls != null && creator.ballSize != null)
			{
				balls = new Balls(this, (byte)creator.numBalls, (byte)creator.ballSize);
			}
			else
			{
				balls = new Balls(this, gender);
			}

			Fertility fertility;
			if (creator.fertility == null)
			{
				fertility = new Fertility(this, gender, creator.artificiallyInfertile);
			}
			else
			{
				fertility = new Fertility(this, (byte)creator.fertility, creator.artificiallyInfertile);
			}

			var cup = gender.HasFlag(Gender.FEMALE) ? Breasts.DEFAULT_FEMALE_SIZE : Breasts.DEFAULT_MALE_SIZE;

			var breasts = creator.breasts ?? new BreastCreator[] { new BreastCreator(cup) };

			switch (gender)
			{
				case Gender.GENDERLESS:
					genitals = new Genitals(this, ass, creator.breasts, null, new Balls(this, false), null, creator.femininity, fertility); break;
				case Gender.MALE:

					var cocks = creator.cocks;
					if (cocks == null || cocks.Length == 0)
					{
						cocks = new CockCreator[] { new CockCreator() };
					}
					genitals = new Genitals(this, ass, breasts, cocks, balls, null, creator.femininity, fertility); break;
				case Gender.FEMALE:
					VaginaCreator[] vaginas = creator.vaginas;
					if (vaginas == null || vaginas.Length == 0)
					{
						vaginas = new VaginaCreator[] { new VaginaCreator() };
					}
					genitals = new Genitals(this, ass, breasts, null, balls, vaginas, creator.femininity, fertility); break;
				case Gender.HERM:
				default:
					cocks = creator.cocks;
					if (cocks == null)
					{
						cocks = new CockCreator[] { new CockCreator() };
					}
					vaginas = creator.vaginas;
					if (vaginas == null)
					{
						vaginas = new VaginaCreator[] { new VaginaCreator() };
					}
					genitals = new Genitals(this, ass, breasts, cocks, balls, vaginas, creator.femininity, fertility); break;
			}

			//antennae
			antennae = creator.antennaeType != null ? new Antennae(this, creator.antennaeType) : new Antennae(this);
			//arms
			arms = creator.armType != null ? new Arms(this, creator.armType) : new Arms(this);
			//back
			if (creator.backType == null)
			{
				back = new Back(this);
			}
			else if (creator.backType == BackType.DRACONIC_MANE && !HairFurColors.IsNullOrEmpty(creator.backHairFur))
			{
				back = new Back(this, BackType.DRACONIC_MANE, creator.backHairFur);
			}
			else
			{
				back = new Back(this, creator.backType);
			}

			//ears
			ears = creator.earType != null ? new Ears(this, creator.earType) : new Ears(this);
			//eyes
			if (creator.eyeType == null && creator.leftEyeColor == null && creator.rightEyeColor == null)
			{
				eyes = new Eyes(this);
			}
			else
			{
				if (creator.eyeType == null)
				{
					creator.eyeType = EyeType.defaultValue;
				}

				if (creator.leftEyeColor == null && creator.rightEyeColor == null)
				{
					eyes = new Eyes(this, creator.eyeType);
				}
				else if (creator.leftEyeColor == null || creator.rightEyeColor == null)
				{
					EyeColor eyeColor = creator.leftEyeColor ?? (EyeColor)creator.rightEyeColor;
					eyes = new Eyes(this, creator.eyeType, eyeColor);
				}
				else
				{
					eyes = new Eyes(this, creator.eyeType, (EyeColor)creator.leftEyeColor, (EyeColor)creator.rightEyeColor);
				}
			}
			//face
			if (creator.faceType is null)
			{
				creator.faceType = FaceType.defaultValue;
			}

			if (creator.facialSkinTexture != null)
			{
				face = new Face(this, creator.faceType, creator.isFaceFullMorph, (SkinTexture)creator.facialSkinTexture);
			}
			else if (creator.isFaceFullMorph != null)
			{
				face = new Face(this, creator.faceType, creator.isFaceFullMorph);
			}
			else
			{
				face = new Face(this, creator.faceType);
			}

			//gills
			gills = creator.gillType != null ? new Gills(this, creator.gillType) : new Gills(this);

			if (creator.hairType is null && HairFurColors.IsNullOrEmpty(creator.hairColor) && creator.hairLength == null
				&& creator.hairHighlightColor is null && creator.hairStyle == HairStyle.NO_STYLE)
			{
				hair = new Hair(this);
			}
			else
			{
				if (creator.hairType == null)
				{
					creator.hairType = HairType.defaultValue;
				}
				hair = new Hair(this, creator.hairType, creator.hairColor, creator.hairHighlightColor, creator.hairLength, creator.hairStyle, creator.hairTransparent);
			}

			//FemininityData femininityData = new FemininityData(genitals.femininity);

			//horns
			if (creator?.hornType == null)
			{
				horns = new Horns(this);
			}
			else if (creator.hornCount != null && creator.hornSize != null)
			{
				horns = new Horns(this, creator.hornType, (byte)creator.hornSize, (byte)creator.hornCount);
			}
			else if (creator.additionalHornTransformStrength != 0)
			{
				horns = new Horns(this, creator.hornType, creator.additionalHornTransformStrength, creator.forceUniformHornGrowthOnCreate);
			}
			else
			{
				horns = new Horns(this, creator.hornType);
			}
			//Lower Body
			lowerBody = creator.lowerBodyType != null ? new LowerBody(this, creator.lowerBodyType) : new LowerBody(this);
			//Neck
			if (creator.neckType == null)
			{
				neck = new Neck(this);
			}
			else if (creator.neckLength != 0)
			{
				neck = new Neck(this, creator.neckType, creator.hairColor, creator.neckLength);
			}
			else
			{
				neck = new Neck(this, creator.neckType);
			}
			//Tail

			if (creator.tailType is null)
			{
				tail = new Tail(this);
			}
			else if (creator.tailCount != null)
			{
				tail = new Tail(this, creator.tailType, (byte)creator.tailCount);
			}
			else
			{
				tail = new Tail(this, creator.tailType);
			}
			//tongue
			tongue = creator.tongueType != null ? new Tongue(this, creator.tongueType) : new Tongue(this);

			if (creator.wingType is null)
			{
				wings = new Wings(this);
			}
			else if (creator.wingType is TonableWings tonableWings)
			{
				wings = new Wings(this, tonableWings, creator.primaryWingTone, creator.secondaryWingTone);
			}
			else if (creator.wingType is FeatheredWings featheredWings)
			{
				wings = new Wings(this, featheredWings, creator.wingFeatherColor);
			}
			else
			{
				wings = new Wings(this, creator.wingType);
			}


			//body.InializePiercings(creator?.navelPiercings);
			//ears.InitializePiercings(creator?.earPiercings);
			//face.InitializePiercings(creator?.eyebrowPiercings, creator?.lipPiercings, creator?.nosePiercings);
			//tongue.InitializePiercings(creator?.tonguePiercings);

			//tail.InitializePiercings(creator?.tailPiercings);

			perks = new PerkCollection(this);
			perks.InitPerks(creator.perks?.ToArray());

			DoPostPerkInit();
			DoLateInit();
		}

		//internal Creature(CreatureSaveFormat format)
		//{
		//	//pull data from format
		//	SetupBindings();
		//	//ValidateData();
		//}
		private void DoPostPerkInit()
		{
			antennae.PostPerkInit();
			arms.PostPerkInit();
			back.PostPerkInit();
			//beard.PostPerkInit();
			body.PostPerkInit();
			build.PostPerkInit();
			ears.PostPerkInit();
			eyes.PostPerkInit();
			face.PostPerkInit();
			genitals.PostPerkInit();
			gills.PostPerkInit();
			hair.PostPerkInit();
			horns.PostPerkInit();
			lowerBody.PostPerkInit();
			neck.PostPerkInit();
			tail.PostPerkInit();
			tongue.PostPerkInit();
			wings.PostPerkInit();
		}

		private void DoLateInit()
		{
			antennae.LateInit();
			arms.LateInit();
			back.LateInit();
			//beard.LateInit();
			body.LateInit();
			build.LateInit();
			ears.LateInit();
			eyes.LateInit();
			face.LateInit();
			genitals.LateInit();
			gills.LateInit();
			hair.LateInit();
			horns.LateInit();
			lowerBody.LateInit();
			neck.LateInit();
			tail.LateInit();
			tongue.LateInit();
			wings.LateInit();
		}
		#endregion

		//everything that modifies data within a body part is supposed to be internal, and then called from here. this way, we can debug eaiser, while still making it somewhat intuitive for non-programmers.
		//we also do this so that we can correctly handle the data, without the frontend devs having to lookup functions and such. for example, we can create a single function for sexual intercourse
		//that calls the genitals and womb classes here, instead of forcing the dev to call multiple functions, and worry about sharing the data - we'll automate that process. It also lets us hook up events
		//and proc them here, so we don't have to worry about doing that in the body parts themselves. The downside is it's more work for us here in the backend and turns this class into a several thousand line
		//long monstrosity. Side note: I'm aware this is how code works on large projects, but dear God do I hate large code files.

		#region Body Part Update/Restore and related event handlers

		#region Antennae
		public bool UpdateAntennae(AntennaeType antennaeType)
		{
			if (antennaeType == null) throw new ArgumentNullException(nameof(antennaeType));
			return antennae.UpdateType(antennaeType);
		}

		public bool RestoreAntennae()
		{
			return antennae.Restore();
		}
		#endregion
		#region Arms
		//arms are weird b/c hands. technically it's own class, so we'll let you subscribe to it, but it may not change whenever armType does. 
		public bool UpdateArms(ArmType armType)
		{
			if (armType == null) throw new ArgumentNullException(nameof(armType));
			return arms.UpdateType(armType);
		}

		public bool RestoreArms()
		{
			return arms.Restore();
		}
		#endregion
		#region Back


		public bool UpdateBack(BackType backType)
		{
			if (backType == null) throw new ArgumentNullException(nameof(backType));
			if (backType is DragonBackMane dragonBack)
			{
				return back.UpdateType(dragonBack, hair.hairColor);
			}
			else
			{
				return back.UpdateType(backType);
			}
		}

		public bool UpdateBack(DragonBackMane dragonMane, HairFurColors maneColor)
		{
			return back.UpdateType(dragonMane, maneColor);
		}

		public bool RestoreBack()
		{
			return back.Restore();
		}
		#endregion
		#region Body

		public bool UpdateBody(BodyType bodyType)
		{
			return body.UpdateType(bodyType);
		}

		public bool UpdateBody(CockatriceBodyType cockatriceBodyType, FurTexture? featherTexture = null, SkinTexture? scaleTexture = null)
		{
			return body.UpdateBody(cockatriceBodyType, featherTexture, scaleTexture);
		}
		public bool UpdateBody(CockatriceBodyType cockatriceBodyType, FurColor featherColor, Tones scaleTone, FurTexture? featherTexture = null, SkinTexture? scaleTexture = null)
		{
			return body.UpdateBody(cockatriceBodyType, featherColor, scaleTone, featherTexture, scaleTexture);
		}

		public bool UpdateBody(KitsuneBodyType kitsuneBodyType, SkinTexture? skinTexture = null, FurTexture? furTexture = null)
		{
			return body.UpdateBody(kitsuneBodyType, skinTexture);
		}
		public bool UpdateBody(KitsuneBodyType kitsuneBodyType, Tones skinTone, FurColor furColor, SkinTexture? skinTexture = null, FurTexture? furTexture = null)
		{
			return body.UpdateBody(kitsuneBodyType, skinTone, furColor, skinTexture);
		}

		public bool UpdateBody(SimpleFurBodyType furryType, FurTexture? furTexture = null)
		{
			return body.UpdateBody(furryType, furTexture);
		}
		public bool UpdateBody(SimpleFurBodyType furryType, FurColor furColor, FurTexture? furTexture = null)
		{
			return body.UpdateBody(furryType, furColor, furTexture);
		}

		public bool UpdateBody(CompoundFurBodyType furryType, FurTexture? furTexture = null)
		{
			return body.UpdateBody(furryType, furTexture);
		}
		public bool UpdateBody(CompoundFurBodyType furryType, FurColor mainFurColor, FurTexture? furTexture = null)
		{
			return body.UpdateBody(furryType, mainFurColor, furTexture);
		}

		public bool UpdateBody(CompoundFurBodyType furryType, FurColor primaryColor, FurColor secondaryColor, FurTexture? primaryTexture = null, FurTexture? secondaryTexture = null)
		{
			return body.UpdateBody(furryType, primaryColor, secondaryColor, primaryTexture);
		}

		public bool UpdateBody(CompoundToneBodyType toneType, SkinTexture? toneTexture = null)
		{
			return body.UpdateBody(toneType, toneTexture);
		}
		public bool UpdateBody(CompoundToneBodyType toneType, Tones primaryColor, SkinTexture? toneTexture = null)
		{
			return body.UpdateBody(toneType, primaryColor, toneTexture);
		}
		public bool UpdateBody(CompoundToneBodyType toneType, Tones primaryColor, Tones secondaryColor, SkinTexture? primaryTexture = null, SkinTexture? secondaryTexture = null)
		{
			return body.UpdateBody(toneType, primaryColor, secondaryColor, primaryTexture);
		}

		public bool UpdateBody(SimpleToneBodyType toneType, SkinTexture? toneTexture = null)
		{
			return body.UpdateBody(toneType, toneTexture);
		}
		public bool UpdateBody(SimpleToneBodyType toneType, Tones color, SkinTexture? toneTexture = null)
		{
			return body.UpdateBody(toneType, color, toneTexture);
		}

		public bool RestoreBody()
		{
			return body.Restore();
		}
		#endregion
		#region Ears


		public bool UpdateEar(EarType earType)
		{
			return ears.UpdateType(earType);
		}

		public bool RestoreEar()
		{
			return ears.Restore();
		}
		#endregion
		#region Eyes


		public bool UpdateEyes(EyeType newType)
		{
			return eyes.UpdateType(newType);
		}

		public bool RestoreEyes()
		{
			return eyes.Restore();
		}

		public void ResetEyes()
		{
			eyes.Reset();
		}
		#endregion
		#region Face

		public bool UpdateFace(FaceType faceType)
		{
			return face.UpdateType(faceType);
		}

		public bool UpdateFaceWithMorph(FaceType faceType, bool fullMorph)
		{
			return face.UpdateFaceWithMorph(faceType, fullMorph);
		}

		public bool UpdateFaceWithComplexion(FaceType faceType, SkinTexture complexion)
		{
			return face.UpdateFaceWithComplexion(faceType, complexion);
		}

		public bool UpdateFaceWithMorphAndComplexion(FaceType faceType, bool fullMorph, SkinTexture complexion)
		{
			return face.UpdateFaceWithMorphAndComplexion(faceType, fullMorph, complexion);
		}

		public bool RestoreFace()
		{
			return face.Restore();
		}

		public void ResetFace()
		{
			face.Reset();
		}

		#endregion
		#region Gills


		public bool UpdateGills(GillType gillType)
		{
			return gills.UpdateType(gillType);
		}

		public bool RestoreGills()
		{
			return gills.Restore();
		}
		#endregion
		#region Hair


		public bool UpdateHair(HairType hairType)
		{
			return hair.UpdateType(hairType);
		}

		//may want to create nicer versions of this, idk. i was getting tired of dealing with that shit lol.
		public bool UpdateHair(HairType newType, HairFurColors newHairColor = null, HairFurColors newHighlightColor = null,
			float? newHairLength = null, HairStyle? newStyle = null, bool ignoreCanLengthenOrCut = false)
		{
			return hair.UpdateType(newType, newHairColor, newHighlightColor, newHairLength, newStyle, ignoreCanLengthenOrCut);
		}

		public bool RestoreHair()
		{
			return hair.Restore();
		}

		public void ResetHair()
		{
			hair.Reset();
		}

		#endregion
		#region Horns


		public bool UpdateHorns(HornType hornType)
		{
			return horns.UpdateType(hornType);
		}

		public bool UpdateHornsAndStrengthenTransform(HornType newType, byte byAmount)
		{
			return horns.UpdateAndStrengthenHorns(newType, byAmount);
		}

		public bool RestoreHorns()
		{
			return horns.Restore();
		}
		#endregion
		#region LowerBody



		public bool UpdateLowerBody(LowerBodyType lowerBodyType)
		{
			return lowerBody.UpdateType(lowerBodyType);
		}

		public bool RestoreLowerBody()
		{
			return lowerBody.Restore();
		}
		#endregion
		#region Neck


		public bool UpdateNeck(NeckType neckType)
		{
			return neck.UpdateType(neckType);
		}

		public bool RestoreNeck()
		{
			return neck.Restore();
		}
		#endregion
		#region Tail


		public bool UpdateTail(TailType tailType)
		{
			return tail.UpdateType(tailType);
		}

		public bool RestoreTail()
		{
			return tail.Restore();
		}
		#endregion
		#region Tongue


		public bool UpdateTongue(TongueType tongueType)
		{
			return tongue.UpdateType(tongueType);
		}

		public bool RestoreTongue()
		{
			return tongue.Restore();
		}
		#endregion
		#region Wings


		public bool UpdateWings(WingType wingType)
		{
			return wings.UpdateType(wingType);
		}

		public bool UpdateWingsAndChangeColor(FeatheredWings featheredWings, HairFurColors featherCol)
		{
			return wings.UpdateWingsAndChangeColor(featheredWings, featherCol);
		}

		public bool UpdateWingsAndChangeColor(TonableWings toneWings, Tones tone)
		{
			return wings.UpdateWingsAndChangeColor(toneWings, tone);
		}

		public bool UpdateWingsAndChangeColor(TonableWings toneWings, Tones tone, Tones boneTone)
		{
			return wings.UpdateWingsAndChangeColor(toneWings, tone, boneTone);
		}

		public bool UpdateWingsForceSize(WingType wingType, bool large)
		{
			return wings.UpdateWingsForceSize(wingType, large);
		}

		public bool UpdateWingsForceSizeChangeColor(FeatheredWings featheredWings, HairFurColors featherColor, bool large)
		{
			return wings.UpdateWingsForceSizeChangeColor(featheredWings, featherColor, large);
		}

		public bool UpdateWingsForceSizeChangeColor(TonableWings toneWings, Tones wingTone, bool large)
		{
			return wings.UpdateWingsForceSizeChangeColor(toneWings, wingTone, large);
		}

		public bool UpdateWingsForceSizeChangeColor(TonableWings toneWings, Tones wingTone, Tones wingBoneTone, bool large)
		{
			return wings.UpdateWingsForceSizeChangeColor(toneWings, wingTone, wingBoneTone, large);
		}

		public bool RestoreWings()
		{
			return wings.Restore();
		}
		#endregion

		#endregion
		/*#region Body Part Change Aliases

		//semantics: Set vs Change - Set will generally be void, as it'll just set it to the value given, though it can return a boolean if it may not be possible to set a value for this data based on other factors.
		//Change will always return a boolean, and it will only be true if the data actually changed. if the value given and the current data are the same, it will return false.

		#region Body

		#endregion
		#region Eyes
		public bool ChangeEyeColor(EyeColor color)
		{
			return eyes.ChangeEyeColor(color);
		}
		public bool ChangeEyeColor(EyeColor leftEye, EyeColor rightEye)
		{
			return eyes.ChangeEyeColor(leftEye, rightEye);
		}
		#endregion
		#region Face
		public bool StrengthenFacialMorph()
		{
			return face.StrengthenFacialMorph();
		}

		//can call restore. so we need to fire an event if it does. 
		public bool WeakenFacialMorph(bool restoreIfAlreadyLevelOne = true)
		{
			return face.WeakenFacialMorph(restoreIfAlreadyLevelOne);
		}

		public bool ChangeFacialComplexion(SkinTexture newTexture)
		{
			return face.ChangeComplexion(newTexture);
		}
		#endregion
		#region Hair
		public bool SetHairColor(HairFurColors newHairColor, bool clearHighlights = false)
		{
			return hair.SetHairColor(newHairColor, clearHighlights);
		}

		public bool SetHairHighlightColor(HairFurColors newHighlightColor)
		{
			return hair.SetHighlightColor(newHighlightColor);
		}

		public bool SetBothHairColors(HairFurColors hairColor, HairFurColors highlightColor)
		{
			return hair.SetBothHairColors(hairColor, highlightColor);
		}

		public bool SetHairStyle(HairStyle newStyle)
		{
			return hair.SetHairStyle(newStyle);
		}

		public bool SetHairLength(float newLength)
		{
			return hair.SetHairLength(newLength);
		}

		public float GrowHair(float byAmount, bool ignoreCanLengthen = false)
		{
			return hair.GrowHair(byAmount, ignoreCanLengthen);
		}

		public float ShortenHair(float byAmount, bool ignoreCanCut = false)
		{
			return hair.ShortenHair(byAmount, ignoreCanCut);
		}
		#endregion
		#region Horns
		public bool StrengthenHornTransformation(byte numberOfTimes = 1)
		{
			return horns.StrengthenTransform(numberOfTimes);
		}

		//NOTE: this may cause you to lose your horns!. hence we do the event shit. 
		public bool WeakenHornTransformation(byte numberOfTimes = 1)
		{
			return horns.WeakenTransform(numberOfTimes);
		}
		#endregion
		#region Neck
		internal byte GrowNeck(byte amount)
		{
			return neck.GrowNeck(amount
);
		}
		#endregion
		#region Tail
		public bool GrowAdditionalTail()
		{
			return tail.GrowAdditionalTail();
		}

		public byte GrowMultipleAdditionalTails(byte amount = 1)
		{
			return tail.GrowMultipleAdditionalTails(amount);
		}
		#endregion
		#region Wings
		internal bool GrowWingsToLarge()
		{
			return wings.GrowLarge();
		}

		internal bool ShrinkWingsToSmall()
		{
			return wings.ShrinkToSmall();
		}

		#endregion
		#endregion*/

		#region Body Part Piercing Aliases and Event Helpers

		#endregion
		#region Time Listeners

		private bool isPlayer => this is Player;

		#region "Anonymous" classes
		//C# isn't java, so we don't have anonymous classes. This is the closest we can achieve. YMMV on which is better. 
		//basically, we need to wrap all the body part events into something the game engine can handle. i'd do it all in creature, but 
		//there's the whole multipage mess to deal with. 
		private sealed class LazyWrapper : ITimeLazyListener
		{
			public readonly IBodyPartTimeLazy listener;
			private readonly bool isPlayer;

			public EventWrapper reactToTimePassing(byte hoursPassed)
			{
				return new EventWrapper(listener.reactToTimePassing(isPlayer, hoursPassed));
			}

			public LazyWrapper(bool player, IBodyPartTimeLazy lazyMember)
			{
				isPlayer = player;
				listener = lazyMember;
			}
		}

		private sealed class ActiveWrapper : ITimeActiveListener
		{
			public readonly IBodyPartTimeActive listener;
			private readonly bool isPlayer;

			public EventWrapper reactToHourPassing()
			{
				return new EventWrapper(listener.reactToHourPassing(isPlayer));
			}

			public ActiveWrapper(bool player, IBodyPartTimeActive activeMember)
			{
				isPlayer = player;
				listener = activeMember;
			}
		}

		private sealed class DailyWrapper : ITimeDailyListener
		{
			public readonly IBodyPartTimeDaily listener;
			private readonly bool isPlayer;

			public byte hourToTrigger => listener.hourToTrigger;

			public EventWrapper reactToDailyTrigger()
			{
				return new EventWrapper(listener.reactToDailyTrigger(isPlayer));
			}

			public DailyWrapper(bool player, IBodyPartTimeDaily activeMember)
			{
				isPlayer = player;
				listener = activeMember;
			}
		}

		private sealed class DayMultiWrapper : ITimeDayMultiListener
		{
			public readonly IBodyPartTimeDayMulti listener;
			private readonly bool isPlayer;

			public byte[] triggerHours => listener.triggerHours;

			public EventWrapper reactToTrigger(byte currHour)
			{
				return new EventWrapper(listener.reactToTrigger(isPlayer, currHour));
			}

			public DayMultiWrapper(bool player, IBodyPartTimeDayMulti activeMember)
			{
				isPlayer = player;
				listener = activeMember;
			}
		}
		#endregion
		//we store a reference to all the listeners, bot the ones we use and the ones the game engine uses, so when we create or destroy this class, we don't "leak" events.

		private protected bool listenersActive { get; private set; } = false;
		private readonly Dictionary<IBodyPartTimeLazy, LazyWrapper> lazyListeners = new Dictionary<IBodyPartTimeLazy, LazyWrapper>();
		private readonly Dictionary<IBodyPartTimeActive, ActiveWrapper> activeListeners = new Dictionary<IBodyPartTimeActive, ActiveWrapper>();
		private readonly Dictionary<IBodyPartTimeDaily, DailyWrapper> dailyListeners = new Dictionary<IBodyPartTimeDaily, DailyWrapper>();
		private readonly Dictionary<IBodyPartTimeDayMulti, DayMultiWrapper> dayMultiListeners = new Dictionary<IBodyPartTimeDayMulti, DayMultiWrapper>();

		#region Register/Deregister
		private protected bool AddTimeListener(IBodyPartTimeLazy listener)
		{
			if (lazyListeners.ContainsKey(listener))
			{
				return false;
			}
			else
			{
				LazyWrapper wrapper = new LazyWrapper(isPlayer, listener);
				lazyListeners.Add(listener, wrapper);
				if (listenersActive)
				{
					GameEngine.RegisterLazyListener(wrapper);
				}
				return true;
			}
		}

		private protected bool RemoveTimeListener(IBodyPartTimeLazy listener)
		{
			if (!lazyListeners.ContainsKey(listener))
			{
				return false;
			}
			else
			{
				LazyWrapper wrapper = lazyListeners[listener];
				lazyListeners.Remove(listener);
				if (listenersActive)
				{
					GameEngine.DeregisterLazyListener(wrapper);
				}
				return true;
			}
		}

		private protected bool AddTimeListener(IBodyPartTimeActive listener)
		{
			if (activeListeners.ContainsKey(listener))
			{
				return false;
			}
			else
			{
				ActiveWrapper wrapper = new ActiveWrapper(isPlayer, listener);
				activeListeners.Add(listener, wrapper);
				if (listenersActive)
				{
					GameEngine.RegisterActiveListener(wrapper);
				}
				return true;
			}
		}

		private protected bool RemoveTimeListener(IBodyPartTimeActive listener)
		{
			if (!activeListeners.ContainsKey(listener))
			{
				return false;
			}
			else
			{
				ActiveWrapper wrapper = activeListeners[listener];
				activeListeners.Remove(listener);
				if (listenersActive)
				{
					GameEngine.DeregisterActiveListener(wrapper);
				}
				return true;
			}
		}

		private protected bool AddTimeListener(IBodyPartTimeDaily listener)
		{
			if (dailyListeners.ContainsKey(listener))
			{
				return false;
			}
			else
			{
				DailyWrapper wrapper = new DailyWrapper(isPlayer, listener);
				dailyListeners.Add(listener, wrapper);
				if (listenersActive)
				{
					GameEngine.RegisterDailyListener(wrapper);
				}
				return true;
			}
		}

		private protected bool RemoveTimeListener(IBodyPartTimeDaily listener)
		{
			if (!dailyListeners.ContainsKey(listener))
			{
				return false;
			}
			else
			{
				DailyWrapper wrapper = dailyListeners[listener];
				dailyListeners.Remove(listener);
				if (listenersActive)
				{
					GameEngine.DeregisterDailyListener(wrapper);
				}
				return true;
			}
		}

		private protected bool AddTimeListener(IBodyPartTimeDayMulti listener)
		{
			if (dayMultiListeners.ContainsKey(listener))
			{
				return false;
			}
			else
			{
				DayMultiWrapper wrapper = new DayMultiWrapper(isPlayer, listener);
				dayMultiListeners.Add(listener, wrapper);
				if (listenersActive)
				{
					GameEngine.RegisterDayMultiListener(wrapper);
				}
				return true;
			}
		}

		private protected bool RemoveTimeListener(IBodyPartTimeDayMulti listener)
		{
			if (!dayMultiListeners.ContainsKey(listener))
			{
				return false;
			}
			else
			{
				DayMultiWrapper wrapper = dayMultiListeners[listener];
				dayMultiListeners.Remove(listener);
				if (listenersActive)
				{
					GameEngine.DeregisterDayMultiListener(wrapper);
				}
				return true;
			}
		}
		#endregion
		protected void ActivateTimeListeners()
		{
			if (!listenersActive)
			{
				listenersActive = true;
				foreach (var listener in lazyListeners.Values)
				{
					GameEngine.RegisterLazyListener(listener);
				}
				foreach (var listener in activeListeners.Values)
				{
					GameEngine.RegisterActiveListener(listener);
				}
				foreach (var listener in dailyListeners.Values)
				{
					GameEngine.RegisterDailyListener(listener);
				}
				foreach (var listener in dayMultiListeners.Values)
				{
					GameEngine.RegisterDayMultiListener(listener);
				}
			}
		}

		protected void DeactivateTimeListeners()
		{
			if (listenersActive)
			{
				listenersActive = false;
				foreach (var listener in lazyListeners.Values)
				{
					GameEngine.DeregisterLazyListener(listener);
				}
				foreach (var listener in activeListeners.Values)
				{
					GameEngine.DeregisterActiveListener(listener);
				}
				foreach (var listener in dailyListeners.Values)
				{
					GameEngine.DeregisterDailyListener(listener);
				}
				foreach (var listener in dayMultiListeners.Values)
				{
					GameEngine.DeregisterDayMultiListener(listener);
				}
			}
		}

		#endregion

		#region DESTRUCTOR/FINALIZER
		~Creature()
		{
			CleanupCreatureForDeletion();
		}

		internal void CleanupCreatureForDeletion()
		{
			if (listenersActive)
			{
				//remove all events if any exist. 
				DeactivateTimeListeners();

				lazyListeners.Clear();
				activeListeners.Clear();
				dailyListeners.Clear();
				dayMultiListeners.Clear();
			}
		}
		#endregion

	}
}
