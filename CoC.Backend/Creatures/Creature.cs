//Creature.cs
//Description:
//Author: JustSomeGuy
//2/20/2019, 4:13 PM
using CoC.Backend.BodyParts;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Engine;
using CoC.Backend.Engine.Time;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Backend.Perks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace CoC.Backend.Creatures
{

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
				creator.bodyType = Body.defaultType;
			}


			if (creator.bodyType is SimpleToneBodyType simpleToneBodyType)
			{
				SkinTexture skinTexture = creator.skinTexture ?? SkinTexture.NONDESCRIPT;

				body = Body.GenerateTonedNoUnderbody(simpleToneBodyType, creator.complexion, skinTexture);
			}
			else if (creator.bodyType is CompoundToneBodyType compoundToneBodyType)
			{
				SkinTexture primary = creator.skinTexture ?? SkinTexture.NONDESCRIPT;
				SkinTexture secondary = creator.underBodySkinTexture ?? SkinTexture.NONDESCRIPT;

				body = Body.GenerateToneWithUnderbody(compoundToneBodyType, creator.complexion, creator.underTone, primary, secondary);
			}
			else if (creator.bodyType is SimpleFurBodyType simpleFur)
			{
				FurTexture primary = creator.furTexture ?? FurTexture.NONDESCRIPT;

				body = Body.GenerateFurredNoUnderbody(simpleFur, creator.furColor, primary);
			}
			else if (creator.bodyType is CompoundFurBodyType compoundFur)
			{
				FurTexture primary = creator.furTexture ?? FurTexture.NONDESCRIPT;
				FurTexture secondary = creator.underBodyFurTexture ?? FurTexture.NONDESCRIPT;

				body = Body.GenerateFurredWithUnderbody(compoundFur, creator.furColor, creator.underFurColor, primary, secondary);
			}
			else if (creator.bodyType is KitsuneBodyType)
			{
				FurTexture fur = creator.furTexture ?? FurTexture.NONDESCRIPT;
				SkinTexture skin = creator.skinTexture ?? SkinTexture.NONDESCRIPT;

				body = Body.GenerateKitsune(creator.complexion, creator.furColor, skin, fur);
			}
			else if (creator.bodyType is CockatriceBodyType)
			{
				SkinTexture scales = creator.skinTexture ?? SkinTexture.NONDESCRIPT;
				FurTexture feather = creator.furTexture ?? FurTexture.NONDESCRIPT;

				body = Body.GenerateCockatrice(creator.furColor, creator.complexion, feather, scales);
			}
			else
			{
				body = Body.GenerateDefaultOfType(creator.bodyType);
			}
			//build
			if (creator.heightInInches == 0)
			{
				creator.heightInInches = Build.DEFAULT_HEIGHT;
			}
			build = Build.Generate(creator.heightInInches, creator.thickness, creator.tone, creator.hipSize, creator.buttSize);

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
				genitals = Genitals.GenerateDefault(gender);
			}

			Ass ass = Ass.Generate(creator.analWetness, creator.analLooseness, creator.assVirgin);
			Breasts[] breasts = creator.breasts?.Select(x => Breasts.Generate(x.cupSize, x.validNippleLength, x.nipplePiercings))?.ToArray();
			Balls balls;
			if (creator.numBalls != null && creator.ballSize != null)
			{
				balls = Balls.GenerateBalls((byte)creator.numBalls, (byte)creator.ballSize);
			}
			else
			{
				balls = Balls.GenerateFromGender(gender);
			}

			Fertility fertility;
			if (creator.fertility == null)
			{
				fertility = Fertility.GenerateDefault(gender, creator.artificiallyInfertile);
			}
			else
			{
				fertility = Fertility.Generate((byte)creator.fertility, creator.artificiallyInfertile);
			}

			//helper to make my select cleaner.
			Cock GenerateCock(CockType c, float l, float g, float k, ReadOnlyDictionary<CockPiercings, PiercingJewelry> p)
			{
				Cock retVal;
				if (c.hasKnot) retVal = Cock.GenerateWithKnot(c, l, g, k);
				else retVal = Cock.Generate(c, l, g);

				Dictionary<CockPiercings, PiercingJewelry> piercings = p is null ? null : new Dictionary<CockPiercings, PiercingJewelry>(p);
				retVal.InitializePiercings(piercings);
				return retVal;
			};

			Vagina GenerateVagina(VaginaType v, float c, VaginalLooseness l, VaginalWetness w, bool i,
				ReadOnlyDictionary<ClitPiercings, PiercingJewelry> cp, ReadOnlyDictionary<LabiaPiercings, PiercingJewelry> lp)
			{
				Vagina retVal;
				if (creator.hasOmnibusClit)
				{
					retVal = Vagina.GenerateOmnibus(v, c, l, w, i, creator.cockVirgin);
				}
				else retVal = Vagina.Generate(v, c, l, w, i);

				Dictionary<ClitPiercings, PiercingJewelry> clitPiercings = cp is null ? null : new Dictionary<ClitPiercings, PiercingJewelry>(cp);
				Dictionary<LabiaPiercings, PiercingJewelry> labiaPiercings = lp is null ? null : new Dictionary<LabiaPiercings, PiercingJewelry>(lp);
				retVal.InitializePiercings(clitPiercings, labiaPiercings);
				return retVal;
			};

			switch (gender)
			{
				case Gender.GENDERLESS:
					genitals = Genitals.Generate(ass, breasts, null, Balls.GenerateDefault(false), null, creator.femininity, fertility); break;
				case Gender.MALE:

					Cock[] cocks = creator.cocks?.Select(x => GenerateCock(x.type, x.validLength, x.validGirth, x.validKnot, x.piercings)).ToArray();
					if (cocks == null)
					{
						cocks = new Cock[] { Cock.GenerateDefault() };
					}
					genitals = Genitals.Generate(ass, breasts, cocks, balls, null, creator.femininity, fertility); break;
				case Gender.FEMALE:
					Vagina[] vaginas = creator.vaginas?.Select(x => GenerateVagina(x.type, x.validClitLength, x.looseness, x.wetness, x.virgin, x.clitPiercings, x.labiaPiercings)).ToArray();
					if (vaginas == null)
					{
						vaginas = new Vagina[] { Vagina.GenerateDefault() };
					}
					genitals = Genitals.Generate(ass, breasts, null, balls, vaginas, creator.femininity, fertility); break;
				case Gender.HERM:
				default:
					cocks = creator.cocks?.Select(x => GenerateCock(x.type, x.validLength, x.validGirth, x.validKnot, x.piercings)).ToArray();
					if (cocks == null)
					{
						cocks = new Cock[] { Cock.GenerateDefault() };
					}
					vaginas = creator.vaginas?.Select(x => GenerateVagina(x.type, x.validClitLength, x.looseness, x.wetness, x.virgin, x.clitPiercings, x.labiaPiercings)).ToArray();
					if (vaginas == null)
					{
						vaginas = new Vagina[] { Vagina.GenerateDefault() };
					}
					genitals = Genitals.Generate(ass, breasts, cocks, balls, vaginas, creator.femininity, fertility); break;
			}


			//antennae
			antennae = creator.antennaeType != null ? Antennae.GenerateDefaultOfType(creator.antennaeType) : Antennae.GenerateDefault();
			//arms
			arms = creator.armType != null ? Arms.GenerateDefaultOfType(creator.armType) : Arms.GenerateDefault();
			//back
			if (creator.backType == null)
			{
				back = Back.GenerateDefault();
			}
			else if (creator.backType == BackType.DRACONIC_MANE && !HairFurColors.IsNullOrEmpty(creator.backHairFur))
			{
				back = Back.GenerateDraconicMane(BackType.DRACONIC_MANE, creator.backHairFur);
			}
			else
			{
				back = Back.GenerateDefaultOfType(creator.backType);
			}

			//ears
			ears = creator.earType != null ? Ears.GenerateDefaultOfType(creator.earType) : Ears.GenerateDefault();
			//eyes
			if (creator.eyeType == null && creator.leftEyeColor == null && creator.rightEyeColor == null)
			{
				eyes = Eyes.GenerateDefault();
			}
			else
			{
				if (creator.eyeType == null)
				{
					creator.eyeType = Eyes.defaultType;
				}

				if (creator.leftEyeColor == null && creator.rightEyeColor == null)
				{
					eyes = Eyes.GenerateDefaultOfType(creator.eyeType);
				}
				else if (creator.leftEyeColor == null || creator.rightEyeColor == null)
				{
					EyeColor eyeColor = creator.leftEyeColor ?? (EyeColor)creator.rightEyeColor;
					eyes = Eyes.GenerateWithColor(creator.eyeType, eyeColor);
				}
				else
				{
					eyes = Eyes.GenerateWithHeterochromia(creator.eyeType, (EyeColor)creator.leftEyeColor, (EyeColor)creator.rightEyeColor);
				}
			}
			//face
			if (creator.faceType is null)
			{
				face = Face.GenerateDefault();
				if (creator.facialSkinTexture != null)
				{
					face.ChangeComplexion((SkinTexture)creator.facialSkinTexture);
				}
			}
			else if (creator.facialSkinTexture != null && creator.isFaceFullMorph != null)
			{
				face = Face.GenerateWithSizeAndComplexion(creator.faceType, (bool)creator.isFaceFullMorph, (SkinTexture)creator.facialSkinTexture);
			}
			else if (creator.facialSkinTexture != null)
			{
				face = Face.GenerateWithComplexion(creator.faceType, (SkinTexture)creator.facialSkinTexture);
			}
			else if (creator.isFaceFullMorph != null)
			{
				face = Face.GenerateWithMorph(creator.faceType, (bool)creator.isFaceFullMorph);
			}
			else
			{
				face = Face.GenerateDefaultOfType(creator.faceType);
			}


			//gills
			gills = creator.gillType != null ? Gills.GenerateDefaultOfType(creator.gillType) : Gills.GenerateDefault();


			if (creator.hairType is null && HairFurColors.IsNullOrEmpty(creator.hairColor) && creator.hairLength == null)
			{
				hair = Hair.GenerateDefault();
			}
			else
			{
				if (creator.hairType == null)
				{
					creator.hairType = Hair.defaultType;
				}


				if (HairFurColors.IsNullOrEmpty(creator.hairColor))
				{
					if (creator.hairLength == null)
					{
						hair = Hair.GenerateDefaultOfType(creator.hairType);
					}
					else
					{
						hair = Hair.GenerateWithLength(creator.hairType, (float)creator.hairLength);
					}
				}
				else if (HairFurColors.IsNullOrEmpty(creator.hairHighlightColor))
				{
					hair = Hair.GenerateWithColor(creator.hairType, creator.hairColor, creator.hairLength);
				}
				else
				{
					hair = Hair.GenerateWithColorAndHighlight(creator.hairType, creator.hairColor, creator.hairHighlightColor, creator.hairLength);
				}
			}

			if (creator.hairTransparent)
			{
				hair.SetTransparency(creator.hairTransparent);
			}
			if (creator.hairStyle != null)
			{
				hair.SetHairStyle((HairStyle)creator.hairStyle);
			}

			//horns
			if (creator?.hornType == null)
			{
				horns = Horns.GenerateDefault();
			}
			else if (creator.hornCount != null && creator.hornSize != null)
			{
				horns = Horns.GenerateOverride(creator.hornType, (byte)creator.hornSize, (byte)creator.hornCount);
			}
			else if (creator.additionalHornTransformStrength != 0)
			{
				horns = Horns.GenerateWithStrength(creator.hornType, creator.additionalHornTransformStrength, creator.forceUniformHornGrowthOnCreate);
			}
			else
			{
				horns = Horns.GenerateDefaultOfType(creator.hornType);
			}
			//Lower Body
			lowerBody = creator.lowerBodyType != null ? LowerBody.GenerateDefaultOfType(creator.lowerBodyType) : LowerBody.GenerateDefault();
			//Neck
			if (creator.neckType == null)
			{
				neck = Neck.GenerateDefault();
			}
			else if (creator.neckLength != default)
			{
				neck = Neck.GenerateNonDefault(creator.neckType, creator.neckLength);
			}
			else
			{
				neck = Neck.GenerateDefaultOfType(creator.neckType);
			}
			//Tail
			if (creator.tailType is null)
			{
				tail = Tail.GenerateDefault();
			}
			else if (creator.tailCount != null)
			{
				tail = Tail.GenerateWithCount(creator.tailType, (byte)creator.tailCount);
			}
			else
			{
				tail = Tail.GenerateDefaultOfType(creator.tailType);
			}
			//tongue
			tongue = creator?.tongueType == null ? Tongue.GenerateDefault() : Tongue.GenerateDefaultOfType(creator.tongueType);
			//wings
			if (creator?.wingType == null)
			{
				wings = Wings.GenerateDefault();
			}
			else if (creator.wingType is FeatheredWings && !HairFurColors.IsNullOrEmpty(creator.wingFeatherColor))
			{
				if (creator.largeWings == null)
				{
					wings = Wings.GenerateColored((FeatheredWings)creator.wingType, creator.wingFeatherColor);
				}
				else
				{
					wings = Wings.GenerateColoredWithSize((FeatheredWings)creator.wingType, creator.wingFeatherColor, (bool)creator.largeWings);
				}
			}
			else if (creator.wingType is TonableWings && !Tones.IsNullOrEmpty(creator.primaryWingTone) && !Tones.IsNullOrEmpty(creator.secondaryWingTone))
			{
				if (creator.largeWings == null)
				{
					wings = Wings.GenerateColored((TonableWings)creator.wingType, creator.primaryWingTone, creator.secondaryWingTone);
				}
				else
				{
					wings = Wings.GenerateColoredWithSize((TonableWings)creator.wingType, creator.primaryWingTone, creator.secondaryWingTone, (bool)creator.largeWings);
				}
			}
			else if (creator.wingType is TonableWings && !Tones.IsNullOrEmpty(creator.primaryWingTone))
			{
				if (creator.largeWings == null)
				{
					wings = Wings.GenerateColored((TonableWings)creator.wingType, creator.primaryWingTone);
				}
				else
				{
					wings = Wings.GenerateColoredWithSize((TonableWings)creator.wingType, creator.primaryWingTone, (bool)creator.largeWings);
				}
			}
			else if (creator.largeWings != null)
			{
				wings = Wings.GenerateDefaultWithSize(creator.wingType, (bool)creator.largeWings);
			}
			else
			{
				wings = Wings.GenerateDefaultOfType(creator.wingType);
			}

			//body.InializePiercings(creator?.navelPiercings);
			//ears.InitializePiercings(creator?.earPiercings);
			//face.InitializePiercings(creator?.eyebrowPiercings, creator?.lipPiercings, creator?.nosePiercings);
			//tongue.InitializePiercings(creator?.tonguePiercings);

			//tail.InitializePiercings(creator?.tailPiercings);

			perks = new PerkCollection(this, GameEngine.constructPerkModifier(), creator.perks?.ToArray());

			SetupBindings();
		}

		//internal Creature(CreatureSaveFormat format)
		//{
		//	//pull data from format
		//	SetupBindings();
		//	//ValidateData();
		//}
		private void SetupBindings()
		{
			body.SetupBodyAware(arms);
			body.SetupBodyAware(build);
			body.SetupBodyAware(ears);
			body.SetupBodyAware(face);
			body.SetupBodyAware(lowerBody);
			body.SetupBodyAware(tail);

			build.SetupBuildAware(hair);
			genitals.RegisterFemininityListener(horns);

			hair.SetupHairAware(body);
			lowerBody.SetupLowerBodyAware(build);



		}
		#endregion

		//everything that modifies data within a body part is supposed to be internal, and then called from here. this way, we can debug eaiser, while still making it somewhat intuitive for non-programmers.
		//we also do this so that we can correctly handle the data, without the frontend devs having to lookup functions and such. for example, we can create a single function for sexual intercourse
		//that calls the genitals and womb classes here, instead of forcing the dev to call multiple functions, and worry about sharing the data - we'll automate that process. It also lets us hook up events
		//and proc them here, so we don't have to worry about doing that in the body parts themselves. The downside is it's more work for us here in the backend and turns this class into a several thousand line
		//long monstrosity. Side note: I'm aware this is how code works on large projects, but dear God do I hate large code files.

		#region Body Part Update/Restore and related event handlers

		#region Antennae
		private event BodyPartChangedEventHandler<Antennae, AntennaeType> AntennaeTypeChanged;



		public void SubscribeToAntennaeChanged(BodyPartChangedEventHandler<Antennae, AntennaeType> antennaeSubscriber)
		{
			AntennaeTypeChanged -= antennaeSubscriber;
			AntennaeTypeChanged += antennaeSubscriber;
		}

		public void UnSubscribeToAntennaeChanged(BodyPartChangedEventHandler<Antennae, AntennaeType> antennaeSubscriber)
		{
			AntennaeTypeChanged -= antennaeSubscriber;
		}

		public bool UpdateAntennae(AntennaeType antennaeType)
		{
			if (antennaeType == null) throw new ArgumentNullException(nameof(antennaeType));
			AntennaeType oldType = antennae.type;
			bool changed = antennae.UpdateType(antennaeType);
			HandleTypeChange(AntennaeTypeChanged, oldType, antennae, changed);
			return changed;
		}

		public bool RestoreAntennae()
		{
			AntennaeType oldType = antennae.type;
			bool changed = antennae.Restore();
			HandleTypeChange(AntennaeTypeChanged, oldType, antennae, changed);
			return changed;
		}
		#endregion
		#region Arms
		//arms are weird b/c hands. technically it's own class, so we'll let you subscribe to it, but it may not change whenever armType does. 
		private event BodyPartChangedEventHandler<Arms, ArmType> ArmTypeChanged;
		private event BodyPartChangedEventHandler<Hands, HandType> HandTypeChanged;

		public void SubscribeToArmsChanged(BodyPartChangedEventHandler<Arms, ArmType> armsSubscriber)
		{
			ArmTypeChanged -= armsSubscriber;
			ArmTypeChanged += armsSubscriber;
		}

		public void UnSubscribeToArmsChanged(BodyPartChangedEventHandler<Arms, ArmType> armsSubscriber)
		{
			ArmTypeChanged -= armsSubscriber;
		}

		public void SubscribeToHandsChanged(BodyPartChangedEventHandler<Hands, HandType> handsSubscriber)
		{
			HandTypeChanged -= handsSubscriber;
			HandTypeChanged += handsSubscriber;
		}

		public void UnSubscribeToHandsChanged(BodyPartChangedEventHandler<Hands, HandType> handsSubscriber)
		{
			HandTypeChanged -= handsSubscriber;
		}

		public bool UpdateArms(ArmType armType)
		{
			if (armType == null) throw new ArgumentNullException(nameof(armType));
			ArmType oldArmType = arms.type;
			HandType oldHandType = hands.type;
			bool changed = arms.UpdateType(armType);
			HandleTypeChange(ArmTypeChanged, oldArmType, arms, changed);
			HandleTypeChange(HandTypeChanged, oldHandType, hands, oldHandType != hands.type);
			return changed;
		}

		public bool RestoreArms()
		{
			ArmType oldArmType = arms.type;
			HandType oldHandType = hands.type;
			bool changed = arms.Restore();
			HandleTypeChange(ArmTypeChanged, oldArmType, arms, changed);
			HandleTypeChange(HandTypeChanged, oldHandType, hands, oldHandType != hands.type);
			return changed;
		}
		#endregion
		#region Back
		private event BodyPartChangedEventHandler<Back, BackType> BackTypeChanged;

		public void SubscribeToBackChanged(BodyPartChangedEventHandler<Back, BackType> backSubscriber)
		{
			BackTypeChanged -= backSubscriber;
			BackTypeChanged += backSubscriber;
		}

		public void UnSubscribeToBackChanged(BodyPartChangedEventHandler<Back, BackType> backSubscriber)
		{
			BackTypeChanged -= backSubscriber;
		}

		public bool UpdateBack(BackType backType)
		{
			if (backType == null) throw new ArgumentNullException(nameof(backType));
			BackType oldType = back.type;
			bool changed;
			if (backType is DragonBackMane dragonBack)
			{
				changed = back.UpdateType(dragonBack, hair.hairColor);
			}
			else
			{
				changed = back.UpdateType(backType);
			}
			HandleTypeChange(BackTypeChanged, oldType, back, changed);
			return changed;
		}

		public bool UpdateBack(DragonBackMane dragonMane, HairFurColors maneColor)
		{
			if (dragonMane == null) throw new ArgumentNullException(nameof(dragonMane));
			//not sure if i should throw on a null. back will actually just ignore nulls, so we should be fine letting through.
			BackType oldType = back.type;
			bool changed = back.UpdateType(dragonMane, maneColor);
			HandleTypeChange(BackTypeChanged, oldType, back, changed);
			return changed;
		}

		public bool RestoreBack()
		{
			BackType oldType = back.type;
			bool changed = back.Restore();
			HandleTypeChange(BackTypeChanged, oldType, back, changed);
			return changed;
		}
		#endregion
		#region Body
		private event BodyPartChangedEventHandler<Body, BodyType> BodyTypeChanged;
		//private event BodyDataChangedEventHandler bodyDataChanged => body.dataChanged;
#warning Think of some way to check if epidermis data changed. likely will use the bodyData class for before and after. 
		public void SubscribeToBodyChanged(BodyPartChangedEventHandler<Body, BodyType> bodySubscriber)
		{
			BodyTypeChanged -= bodySubscriber;
			BodyTypeChanged += bodySubscriber;
		}

		public void UnSubscribeToBodyChanged(BodyPartChangedEventHandler<Body, BodyType> bodySubscriber)
		{
			BodyTypeChanged -= bodySubscriber;
		}

		public bool UpdateBody(BodyType bodyType)
		{
			if (bodyType == null) throw new ArgumentNullException(nameof(bodyType));
			BodyType oldType = body.type;
			bool changed = body.UpdateType(bodyType);
			HandleTypeChange(BodyTypeChanged, oldType, body, changed);
			return changed;
		}

		public bool UpdateBody(CockatriceBodyType cockatriceBodyType, FurTexture? featherTexture = null, SkinTexture? scaleTexture = null)
		{
			if (cockatriceBodyType == null) throw new ArgumentNullException(nameof(cockatriceBodyType));
			BodyType oldType = body.type;
			bool changed = body.UpdateBody(cockatriceBodyType, featherTexture, scaleTexture);
			HandleTypeChange(BodyTypeChanged, oldType, body, changed);
			return changed;
		}
		public bool UpdateBody(CockatriceBodyType cockatriceBodyType, FurColor featherColor, Tones scaleTone, FurTexture? featherTexture = null, SkinTexture? scaleTexture = null)
		{
			if (cockatriceBodyType == null) throw new ArgumentNullException(nameof(cockatriceBodyType));
			BodyType oldType = body.type;
			bool changed = body.UpdateBody(cockatriceBodyType, featherColor, scaleTone, featherTexture, scaleTexture);
			HandleTypeChange(BodyTypeChanged, oldType, body, changed);
			return changed;
		}

		public bool UpdateBody(KitsuneBodyType kitsuneBodyType, SkinTexture? skinTexture = null, FurTexture? furTexture = null)
		{
			if (kitsuneBodyType == null) throw new ArgumentNullException(nameof(kitsuneBodyType));
			BodyType oldType = body.type;
			bool changed = body.UpdateBody(kitsuneBodyType, skinTexture);
			HandleTypeChange(BodyTypeChanged, oldType, body, changed);
			return changed;
		}
		public bool UpdateBody(KitsuneBodyType kitsuneBodyType, Tones skinTone, FurColor furColor, SkinTexture? skinTexture = null, FurTexture? furTexture = null)
		{
			if (kitsuneBodyType == null) throw new ArgumentNullException(nameof(kitsuneBodyType));
			BodyType oldType = body.type;
			bool changed = body.UpdateBody(kitsuneBodyType, skinTone, furColor, skinTexture);
			HandleTypeChange(BodyTypeChanged, oldType, body, changed);
			return changed;
		}

		public bool UpdateBody(SimpleFurBodyType furryType, FurTexture? furTexture = null)
		{
			if (furryType == null) throw new ArgumentNullException(nameof(furryType));
			BodyType oldType = body.type;
			bool changed = body.UpdateBody(furryType, furTexture);
			HandleTypeChange(BodyTypeChanged, oldType, body, changed);
			return changed;
		}
		public bool UpdateBody(SimpleFurBodyType furryType, FurColor furColor, FurTexture? furTexture = null)
		{
			if (furryType == null) throw new ArgumentNullException(nameof(furryType));
			BodyType oldType = body.type;
			bool changed = body.UpdateBody(furryType, furColor, furTexture);
			HandleTypeChange(BodyTypeChanged, oldType, body, changed);
			return changed;
		}

		public bool UpdateBody(CompoundFurBodyType furryType, FurTexture? furTexture = null)
		{
			if (furryType == null) throw new ArgumentNullException(nameof(furryType));
			BodyType oldType = body.type;
			bool changed = body.UpdateBody(furryType, furTexture);
			HandleTypeChange(BodyTypeChanged, oldType, body, changed);
			return changed;
		}
		public bool UpdateBody(CompoundFurBodyType furryType, FurColor mainFurColor, FurTexture? furTexture = null)
		{
			if (furryType == null) throw new ArgumentNullException(nameof(furryType));
			BodyType oldType = body.type;
			bool changed = body.UpdateBody(furryType, mainFurColor, furTexture);
			HandleTypeChange(BodyTypeChanged, oldType, body, changed);
			return changed;
		}

		public bool UpdateBody(CompoundFurBodyType furryType, FurColor primaryColor, FurColor secondaryColor, FurTexture? primaryTexture = null, FurTexture? secondaryTexture = null)
		{
			if (furryType == null) throw new ArgumentNullException(nameof(furryType));
			BodyType oldType = body.type;
			bool changed = body.UpdateBody(furryType, primaryColor, secondaryColor, primaryTexture);
			HandleTypeChange(BodyTypeChanged, oldType, body, changed);
			return changed;
		}

		public bool UpdateBody(CompoundToneBodyType toneType, SkinTexture? toneTexture = null)
		{
			if (toneType == null) throw new ArgumentNullException(nameof(toneType));
			BodyType oldType = body.type;
			bool changed = body.UpdateBody(toneType, toneTexture);
			HandleTypeChange(BodyTypeChanged, oldType, body, changed);
			return changed;
		}
		public bool UpdateBody(CompoundToneBodyType toneType, Tones primaryColor, SkinTexture? toneTexture = null)
		{
			if (toneType == null) throw new ArgumentNullException(nameof(toneType));
			BodyType oldType = body.type;
			bool changed = body.UpdateBody(toneType, primaryColor, toneTexture);
			HandleTypeChange(BodyTypeChanged, oldType, body, changed);
			return changed;
		}
		public bool UpdateBody(CompoundToneBodyType toneType, Tones primaryColor, Tones secondaryColor, SkinTexture? primaryTexture = null, SkinTexture? secondaryTexture = null)
		{
			if (toneType == null) throw new ArgumentNullException(nameof(toneType));
			BodyType oldType = body.type;
			bool changed = body.UpdateBody(toneType, primaryColor, secondaryColor, primaryTexture);
			HandleTypeChange(BodyTypeChanged, oldType, body, changed);
			return changed;
		}

		public bool UpdateBody(SimpleToneBodyType toneType, SkinTexture? toneTexture = null)
		{
			if (toneType == null) throw new ArgumentNullException(nameof(toneType));
			BodyType oldType = body.type;
			bool changed = body.UpdateBody(toneType, toneTexture);
			HandleTypeChange(BodyTypeChanged, oldType, body, changed);
			return changed;
		}
		public bool UpdateBody(SimpleToneBodyType toneType, Tones color, SkinTexture? toneTexture = null)
		{
			if (toneType == null) throw new ArgumentNullException(nameof(toneType));
			BodyType oldType = body.type;
			bool changed = body.UpdateBody(toneType, color, toneTexture);
			HandleTypeChange(BodyTypeChanged, oldType, body, changed);
			return changed;
		}

		public bool RestoreBody()
		{
			BodyType oldType = body.type;
			bool changed = body.Restore();
			HandleTypeChange(BodyTypeChanged, oldType, body, changed);
			return changed;
		}
		#endregion
		#region Ears
		private event BodyPartChangedEventHandler<Ears, EarType> EarTypeChanged;

		public void SubscribeToEarChanged(BodyPartChangedEventHandler<Ears, EarType> earsSubscriber)
		{
			EarTypeChanged -= earsSubscriber;
			EarTypeChanged += earsSubscriber;
		}

		public void UnSubscribeToEarChanged(BodyPartChangedEventHandler<Ears, EarType> earsSubscriber)
		{
			EarTypeChanged -= earsSubscriber;
		}

		public bool UpdateEar(EarType earType)
		{
			if (earType == null) throw new ArgumentNullException(nameof(earType));
			EarType oldType = ears.type;
			bool changed = ears.UpdateType(earType);
			HandleTypeChange(EarTypeChanged, oldType, ears, changed);
			return changed;
		}

		public bool RestoreEar()
		{
			EarType oldType = ears.type;
			bool changed = ears.Restore();
			HandleTypeChange(EarTypeChanged, oldType, ears, changed);
			return changed;
		}
		#endregion
		#region Eyes
		private event BodyPartChangedEventHandler<Eyes, EyeType> EyeTypeChanged;

		public void SubscribeToEyesChanged(BodyPartChangedEventHandler<Eyes, EyeType> earsSubscriber)
		{
			EyeTypeChanged -= earsSubscriber;
			EyeTypeChanged += earsSubscriber;
		}

		public void UnSubscribeToEyesChanged(BodyPartChangedEventHandler<Eyes, EyeType> earsSubscriber)
		{
			EyeTypeChanged -= earsSubscriber;
		}

		public bool UpdateEyes(EyeType newType)
		{
			if (newType == null) throw new ArgumentNullException();
			EyeType oldType = eyes.type;
			bool changed = eyes.UpdateType(newType);
			HandleTypeChange(EyeTypeChanged, oldType, eyes, changed);
			return changed;
		}

		public bool RestoreEyes()
		{
			var oldType = eyes.type;
			bool changed = eyes.Restore();
			HandleTypeChange(EyeTypeChanged, oldType, eyes, changed);
			return changed;
		}

		public void ResetEyes()
		{
			EyeType oldType = eyes.type;
			eyes.Reset();
			HandleTypeChange(EyeTypeChanged, oldType, eyes, oldType != eyes.type);
			//handle change if eye color changed and we are keeping track of that. 

		}
		#endregion
		#region Face
		private event BodyPartChangedEventHandler<Face, FaceType> FaceTypeChanged;

		public void SubscribeToFaceChanged(BodyPartChangedEventHandler<Face, FaceType> faceSubscriber)
		{
			FaceTypeChanged -= faceSubscriber;
			FaceTypeChanged += faceSubscriber;
		}

		public void UnSubscribeToFaceChanged(BodyPartChangedEventHandler<Face, FaceType> faceSubscriber)
		{
			FaceTypeChanged -= faceSubscriber;
		}

		public bool UpdateFace(FaceType faceType)
		{
			if (faceType == null) throw new ArgumentNullException(nameof(faceType));
			FaceType oldType = face.type;
			bool changed = face.UpdateType(faceType);
			HandleTypeChange(FaceTypeChanged, oldType, face, changed);
			return changed;
		}

		public bool UpdateFaceWithMorph(FaceType faceType, bool fullMorph)
		{
			if (faceType == null) throw new ArgumentNullException(nameof(faceType));
			FaceType oldType = face.type;
			bool changed = face.UpdateFaceWithMorph(faceType, fullMorph);
			HandleTypeChange(FaceTypeChanged, oldType, face, changed);
			return changed;
		}

		public bool UpdateFaceWithComplexion(FaceType faceType, SkinTexture complexion)
		{
			if (faceType == null) throw new ArgumentNullException(nameof(faceType));
			FaceType oldType = face.type;
			bool changed = face.UpdateFaceWithComplexion(faceType, complexion);
			HandleTypeChange(FaceTypeChanged, oldType, face, changed);
			return changed;
		}

		public bool UpdateFaceWithMorphAndComplexion(FaceType faceType, bool fullMorph, SkinTexture complexion)
		{
			if (faceType == null) throw new ArgumentNullException(nameof(faceType));
			FaceType oldType = face.type;
			bool changed = face.UpdateFaceWithMorphAndComplexion(faceType, fullMorph, complexion);
			HandleTypeChange(FaceTypeChanged, oldType, face, changed);
			return changed;
		}

		public bool RestoreFace()
		{
			FaceType oldType = face.type;
			bool changed = face.Restore();
			HandleTypeChange(FaceTypeChanged, oldType, face, changed);
			return changed;
		}

		public void ResetFace()
		{
			FaceType oldType = face.type;
			face.Reset();
			HandleTypeChange(FaceTypeChanged, oldType, face, oldType != face.type);
		}

		#endregion
		#region Gills
		private event BodyPartChangedEventHandler<Gills, GillType> GillTypeChanged;

		public void SubscribeToGillsChanged(BodyPartChangedEventHandler<Gills, GillType> gillsSubscriber)
		{
			GillTypeChanged -= gillsSubscriber;
			GillTypeChanged += gillsSubscriber;
		}

		public void UnSubscribeToGillsChanged(BodyPartChangedEventHandler<Gills, GillType> gillsSubscriber)
		{
			GillTypeChanged -= gillsSubscriber;
		}

		public bool UpdateGills(GillType gillType)
		{
			if (gillType == null) throw new ArgumentNullException(nameof(gillType));
			GillType oldType = gills.type;
			bool changed = gills.UpdateType(gillType);
			HandleTypeChange(GillTypeChanged, oldType, gills, changed);
			return changed;
		}

		public bool RestoreGills()
		{
			GillType oldType = gills.type;
			bool changed = gills.Restore();
			HandleTypeChange(GillTypeChanged, oldType, gills, changed);
			return changed;
		}
		#endregion
		#region Hair
		private event BodyPartChangedEventHandler<Hair, HairType> HairTypeChanged;

		public void SubscribeToHairTypeChanged(BodyPartChangedEventHandler<Hair, HairType> hairSubscriber)
		{
			HairTypeChanged -= hairSubscriber;
			HairTypeChanged += hairSubscriber;
		}

		public void UnSubscribeToHairTypeChanged(BodyPartChangedEventHandler<Hair, HairType> hairSubscriber)
		{
			HairTypeChanged -= hairSubscriber;
		}

		public bool UpdateHair(HairType hairType)
		{
			if (hairType == null) throw new ArgumentNullException(nameof(hairType));
			HairType oldType = hair.type;
			bool changed = hair.UpdateType(hairType);
			HandleTypeChange(HairTypeChanged, oldType, hair, changed);
			return changed;
		}

		//may want to create nicer versions of this, idk. i was getting tired of dealing with that shit lol.
		public bool UpdateHair(HairType newType, HairFurColors newHairColor = null, HairFurColors newHighlightColor = null,
			float? newHairLength = null, HairStyle? newStyle = null, bool ignoreCanLengthenOrCut = false)
		{
			return hair.UpdateType(newType, newHairColor, newHighlightColor, newHairLength, newStyle, ignoreCanLengthenOrCut);
		}

		public bool RestoreHair()
		{
			HairType oldType = hair.type;
			bool changed = hair.Restore();
			HandleTypeChange(HairTypeChanged, oldType, hair, changed);
			return changed;
		}

		public void ResetHair()
		{
			HairType oldType = hair.type;
			hair.Reset();
			HandleTypeChange(HairTypeChanged, oldType, hair, oldType != hair.type);
		}

		#endregion
		#region Horns
		private event BodyPartChangedEventHandler<Horns, HornType> HornTypeChanged;

		public void SubscribeToHornsChanged(BodyPartChangedEventHandler<Horns, HornType> hornsSubscriber)
		{
			HornTypeChanged -= hornsSubscriber;
			HornTypeChanged += hornsSubscriber;
		}

		public void UnSubscribeToHornsChanged(BodyPartChangedEventHandler<Horns, HornType> hornsSubscriber)
		{
			HornTypeChanged -= hornsSubscriber;
		}

		public bool UpdateHorns(HornType hornType)
		{
			if (hornType == null) throw new ArgumentNullException(nameof(hornType));
			HornType oldType = horns.type;
			bool changed = horns.UpdateType(hornType);
			HandleTypeChange(HornTypeChanged, oldType, horns, changed);
			return changed;
		}

		public bool UpdateHornsAndStrengthenTransform(HornType newType, byte byAmount)
		{
			if (newType == null) throw new ArgumentNullException(nameof(newType));
			HornType oldType = horns.type;
			bool changed = horns.UpdateAndStrengthenHorns(newType, byAmount);
			HandleTypeChange(HornTypeChanged, oldType, horns, changed);
			return changed;
		}

		public bool RestoreHorns()
		{
			HornType oldType = horns.type;
			bool changed = horns.Restore();
			HandleTypeChange(HornTypeChanged, oldType, horns, changed);
			return changed;
		}
		#endregion
		#region LowerBody
		private event BodyPartChangedEventHandler<LowerBody, LowerBodyType> LowerBodyTypeChanged;
		private event BodyPartChangedEventHandler<Feet, FootType> FootTypeChanged;

		public void SubscribeToLowerBodyChanged(BodyPartChangedEventHandler<LowerBody, LowerBodyType> lowerBodySubscriber)
		{
			LowerBodyTypeChanged -= lowerBodySubscriber;
			LowerBodyTypeChanged += lowerBodySubscriber;
		}

		public void UnSubscribeToLowerBodyChanged(BodyPartChangedEventHandler<LowerBody, LowerBodyType> lowerBodySubscriber)
		{
			LowerBodyTypeChanged -= lowerBodySubscriber;
		}

		public bool UpdateLowerBody(LowerBodyType lowerBodyType)
		{
			if (lowerBodyType == null) throw new ArgumentNullException(nameof(lowerBodyType));
			LowerBodyType oldLowerBodyType = lowerBody.type;
			FootType oldFootType = feet.type;
			bool changed = lowerBody.UpdateType(lowerBodyType);
			HandleTypeChange(LowerBodyTypeChanged, oldLowerBodyType, lowerBody, changed);
			HandleTypeChange(FootTypeChanged, oldFootType, feet, oldFootType != feet.type);
			return changed;
		}

		public bool RestoreLowerBody()
		{
			LowerBodyType oldLowerBodyType = lowerBody.type;
			FootType oldFootType = feet.type;
			bool changed = lowerBody.Restore();
			HandleTypeChange(LowerBodyTypeChanged, oldLowerBodyType, lowerBody, changed);
			HandleTypeChange(FootTypeChanged, oldFootType, feet, oldFootType != feet.type);
			return changed;
		}
		#endregion
		#region Neck
		private event BodyPartChangedEventHandler<Neck, NeckType> NeckTypeChanged;

		public void SubscribeToNeckChanged(BodyPartChangedEventHandler<Neck, NeckType> neckSubscriber)
		{
			NeckTypeChanged -= neckSubscriber;
			NeckTypeChanged += neckSubscriber;
		}

		public void UnSubscribeToNeckChanged(BodyPartChangedEventHandler<Neck, NeckType> neckSubscriber)
		{
			NeckTypeChanged -= neckSubscriber;
		}

		public bool UpdateNeck(NeckType neckType)
		{
			if (neckType == null) throw new ArgumentNullException(nameof(neckType));
			NeckType oldType = neck.type;
			bool changed = neck.UpdateType(neckType);
			HandleTypeChange(NeckTypeChanged, oldType, neck, changed);
			return changed;
		}

		public bool RestoreNeck()
		{
			NeckType oldType = neck.type;
			bool changed = neck.Restore();
			HandleTypeChange(NeckTypeChanged, oldType, neck, changed);
			return changed;
		}
		#endregion
		#region Tail
		private event BodyPartChangedEventHandler<Tail, TailType> TailTypeChanged;

		public void SubscribeToTailChanged(BodyPartChangedEventHandler<Tail, TailType> tailSubscriber)
		{
			TailTypeChanged -= tailSubscriber;
			TailTypeChanged += tailSubscriber;
		}

		public void UnSubscribeToTailChanged(BodyPartChangedEventHandler<Tail, TailType> tailSubscriber)
		{
			TailTypeChanged -= tailSubscriber;
		}

		public bool UpdateTail(TailType tailType)
		{
			if (tailType == null) throw new ArgumentNullException(nameof(tailType));
			TailType oldType = tail.type;
			bool changed = tail.UpdateType(tailType);
			HandleTypeChange(TailTypeChanged, oldType, tail, changed);
			return changed;
		}

		public bool RestoreTail()
		{
			TailType oldType = tail.type;
			bool changed = tail.Restore();
			HandleTypeChange(TailTypeChanged, oldType, tail, changed);
			return changed;
		}
		#endregion
		#region Tongue
		private event BodyPartChangedEventHandler<Tongue, TongueType> TongueTypeChanged;

		public void SubscribeToTongueChanged(BodyPartChangedEventHandler<Tongue, TongueType> tongueSubscriber)
		{
			TongueTypeChanged -= tongueSubscriber;
			TongueTypeChanged += tongueSubscriber;
		}

		public void UnSubscribeToTongueChanged(BodyPartChangedEventHandler<Tongue, TongueType> tongueSubscriber)
		{
			TongueTypeChanged -= tongueSubscriber;
		}

		public bool UpdateTongue(TongueType tongueType)
		{
			if (tongueType == null) throw new ArgumentNullException(nameof(tongueType));
			TongueType oldType = tongue.type;
			bool changed = tongue.UpdateType(tongueType);
			HandleTypeChange(TongueTypeChanged, oldType, tongue, changed);
			return changed;
		}

		public bool RestoreTongue()
		{
			TongueType oldType = tongue.type;
			bool changed = tongue.Restore();
			HandleTypeChange(TongueTypeChanged, oldType, tongue, changed);
			return changed;
		}
		#endregion
		#region Wings
		private event BodyPartChangedEventHandler<Wings, WingType> WingTypeChanged;

		public void SubscribeToWingsChanged(BodyPartChangedEventHandler<Wings, WingType> wingsSubscriber)
		{
			WingTypeChanged -= wingsSubscriber;
			WingTypeChanged += wingsSubscriber;
		}

		public void UnSubscribeToWingsChanged(BodyPartChangedEventHandler<Wings, WingType> wingsSubscriber)
		{
			WingTypeChanged -= wingsSubscriber;
		}

		public bool UpdateWings(WingType wingType)
		{
			if (wingType == null) throw new ArgumentNullException(nameof(wingType));
			WingType oldType = wings.type;
			bool changed = wings.UpdateType(wingType);
			HandleTypeChange(WingTypeChanged, oldType, wings, changed);
			return changed;
		}

		public bool UpdateWingsAndChangeColor(FeatheredWings featheredWings, HairFurColors featherCol)
		{
			if (featheredWings == null) throw new ArgumentNullException(nameof(featheredWings));
			WingType oldType = wings.type;
			bool changed = wings.UpdateWingsAndChangeColor(featheredWings, featherCol);
			HandleTypeChange(WingTypeChanged, oldType, wings, changed);
			return changed;
		}

		public bool UpdateWingsAndChangeColor(TonableWings toneWings, Tones tone)
		{
			if (toneWings == null) throw new ArgumentNullException(nameof(toneWings));
			WingType oldType = wings.type;
			bool changed = wings.UpdateWingsAndChangeColor(toneWings, tone);
			HandleTypeChange(WingTypeChanged, oldType, wings, changed);
			return changed;
		}

		public bool UpdateWingsAndChangeColor(TonableWings toneWings, Tones tone, Tones boneTone)
		{
			if (toneWings == null) throw new ArgumentNullException(nameof(toneWings));
			WingType oldType = wings.type;
			bool changed = wings.UpdateWingsAndChangeColor(toneWings, tone, boneTone);
			HandleTypeChange(WingTypeChanged, oldType, wings, changed);
			return changed;
		}

		public bool UpdateWingsForceSize(WingType wingType, bool large)
		{
			if (wingType == null) throw new ArgumentNullException(nameof(wingType));
			WingType oldType = wings.type;
			bool changed = wings.UpdateWingsForceSize(wingType, large);
			HandleTypeChange(WingTypeChanged, oldType, wings, changed);
			return changed;
		}


		public bool UpdateWingsForceSizeChangeColor(FeatheredWings featheredWings, HairFurColors featherColor, bool large)
		{
			if (featheredWings == null) throw new ArgumentNullException(nameof(featheredWings));
			WingType oldType = wings.type;
			bool changed = wings.UpdateWingsForceSizeChangeColor(featheredWings, featherColor, large);
			HandleTypeChange(WingTypeChanged, oldType, wings, changed);
			return changed;
		}

		public bool UpdateWingsForceSizeChangeColor(TonableWings toneWings, Tones wingTone, bool large)
		{
			if (toneWings == null) throw new ArgumentNullException(nameof(toneWings));
			WingType oldType = wings.type;
			bool changed = wings.UpdateWingsForceSizeChangeColor(toneWings, wingTone, large);
			HandleTypeChange(WingTypeChanged, oldType, wings, changed);
			return changed;
		}

		public bool UpdateWingsForceSizeChangeColor(TonableWings toneWings, Tones wingTone, Tones wingBoneTone, bool large)
		{
			if (toneWings == null) throw new ArgumentNullException(nameof(toneWings));
			WingType oldType = wings.type;
			bool changed = wings.UpdateWingsForceSizeChangeColor(toneWings, wingTone, wingBoneTone, large);
			HandleTypeChange(WingTypeChanged, oldType, wings, changed);
			return changed;
		}


		public bool RestoreWings()
		{
			WingType oldType = wings.type;
			bool changed = wings.Restore();
			HandleTypeChange(WingTypeChanged, oldType, wings, changed);
			return changed;
		}
		#endregion


		private void HandleTypeChange<T, U>(BodyPartChangedEventHandler<T, U> handler, U oldType, T data, bool changed) where T : BehavioralPartBase<U> where U : BehaviorBase
		{
			if (changed)
			{
				handler?.Invoke(this, new BodyPartChangedEventArg<T, U>(data, oldType, data.type));
			}
		}


		#endregion
		#region Body Part Change Aliases

		//semantics: Set vs Change - Set will generally be void, as it'll just set it to the value given, though it can return a boolean if it may not be possible to set a value for this data based on other factors.
		//Change will always return a boolean, and it will only be true if the data actually changed. if the value given and the current data are the same, it will return false.

		#region Body
#warning May want to add some sort of check delta for events on body data changed. idk. see above warning. 
		public bool ChangePrimarySkinTexture(SkinTexture skinTexture)
		{
			return body.ChangePrimarySkinTexture(skinTexture);
		}

		public bool ChangeSecondarySkinTexture(SkinTexture skinTexture)
		{
			return body.ChangeSecondarySkinTexture(skinTexture);
		}

		public bool ChangePrimaryFurTexture(FurTexture furTexture)
		{
			return body.ChangePrimaryFurTexture(furTexture);
		}

		public bool ChangeSecondaryFurTexture(FurTexture furTexture)
		{
			return body.ChangeSecondaryFurTexture(furTexture);
		}

		public bool ChangePrimaryFurColor(FurColor furColor)
		{
			return body.ChangePrimaryFurColor(furColor);
		}

		public bool ChangePrimaryTone(Tones tone)
		{
			return body.ChangePrimaryTone(tone);
		}

		public bool ChangeSecondaryFurColor(FurColor furColor)
		{
			return body.ChangeSecondaryFurColor(furColor);
		}

		public bool ChangeSecondaryTone(Tones tone)
		{
			return body.ChangeSecondaryTone(tone);
		}
		#endregion
		#region Eyes
#warning may want to create an eye color change event, but imo that seems excessive.
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
			var oldType = face.type;
			bool retVal = face.WeakenFacialMorph(restoreIfAlreadyLevelOne);
			HandleTypeChange(FaceTypeChanged, oldType, face, oldType != face.type);
			return retVal;
		}

		public bool ChangeFacialComplexion(SkinTexture newTexture)
		{
			return face.ChangeComplexion(newTexture);
		}
		#endregion
		#region Hair
#warning consider a hair data changed event too - maybe people will care about length or style, idk. seems excessive.
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
			var oldType = horns.type;
			bool retVal = horns.WeakenTransform(numberOfTimes);
			HandleTypeChange(HornTypeChanged, oldType, horns, oldType != horns.type);
			return retVal;
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
		#endregion
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
