using CoC.Backend.Attacks;
using CoC.Backend.BodyParts;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CoCBackendUnitTests
{
	//Quick aside: for awares, integration testing where they get valid data from another body part
	//will be done in creature. For now, any valid data will do, and therefore we provide it with fakes (i call them dummies)

	//HERE'S WHY YOU RUN TESTS: I've found over a dozen errors in my own code (and i think i'm good at this stuff) that would have slipped through otherwise. 


	/// <summary>
	/// Test class responsible for running unit tests on the various body parts. checks all Generate functions, the standard UpdateType(newType), and Restore.
	/// It also tests Reset if reset is available and does not require testing piercings. piercings are done in another test group. Piercing integration into
	/// a specific body part is considered an extra test and will be done there. 
	/// </summary>
	[TestClass]
	public class BehavioralSaveableBodyPartStandardTests
	{
		internal HairData HairDataGetterDummy()
		{
			return new HairData(HairType.NORMAL, HairFurColors.BROWN, HairFurColors.NO_HAIR_FUR, HairStyle.NO_STYLE, 5.25f, false, false);
		}

		internal BodyData BodyDataGetterDummy()
		{
			HairData hairData = HairDataGetterDummy();
			Epidermis primaryOne = new Epidermis(EpidermisType.FUR, new FurColor(HairFurColors.RED), FurTexture.NONDESCRIPT);
			Epidermis primaryTwo = new Epidermis(EpidermisType.SCALES, Tones.GREEN, SkinTexture.ROUGH);
			return new BodyData(primaryOne, primaryTwo, primaryOne, primaryTwo, hairData, BodyType.COCKATRICE);
		}

		//hair for tallness.
		internal BuildData BuildDataGetterDummy()
		{
			return new BuildData(67, 35, 50, 10, 7);
		}

		internal FemininityData FemininityDataGetterDummy()
		{
			return new FemininityData(25);
		}

		//build for descriptions
		internal LowerBodyData LowerBodyDataGetterDummy()
		{
			return new LowerBodyData(LowerBodyType.NAGA, new EpidermalData(EpidermisType.SCALES, Tones.SPRING_GREEN, SkinTexture.SHINY),
				new EpidermalData(EpidermisType.SCALES, Tones.AUBURN, SkinTexture.ROUGH));
		}


#warning ToDo: Do checks on non-behavior classes. Remember that Breasts and Genitals are technically integration tests too.

		[TestMethod]
		public void Antennae_GenerateUpdateRestore()
		{

			Antennae antennae = Antennae.GenerateDefault();
			Assert.IsTrue(antennae.isDefault);
			Assert.IsFalse(antennae.hasAntennae);

			Assert.ThrowsException<ArgumentNullException>(() => antennae = Antennae.GenerateDefaultOfType(null));

			antennae = Antennae.GenerateDefaultOfType(AntennaeType.BEE);
			Assert.AreEqual(antennae.type, AntennaeType.BEE);

			Assert.IsFalse(antennae.UpdateType(null));

			Assert.IsTrue(antennae.Restore());
			Assert.IsTrue(antennae.isDefault);
			Assert.IsFalse(antennae.UpdateType(Antennae.defaultType));
			Assert.IsFalse(antennae.hasAntennae);

			foreach (AntennaeType currtype in AntennaeType.availableTypes)
			{
				foreach (AntennaeType newType in AntennaeType.availableTypes)
				{
					//reset these to the current type we're checking.
					antennae.UpdateType(currtype);

					if (newType != antennae.type)
					{
						Assert.IsTrue(antennae.UpdateType(newType));
					}
					else
					{
						Assert.IsFalse(antennae.UpdateType(newType));
					}
				}
			}
		}

		[TestMethod]
		public void Arm_GenerateUpdateRestore()
		{
			Arms arms = Arms.GenerateDefault();
			Assert.AreEqual(arms.type, Arms.defaultType);

			Assert.ThrowsException<ArgumentNullException>(() => arms = Arms.GenerateDefaultOfType(null)); //didnt throw. caused null reference errors later. 

			//added null check for all further tests, and global find/replace to make sure this was the case. 

			arms = Arms.GenerateDefaultOfType(ArmType.BEE);
			Assert.AreEqual(arms.type, ArmType.BEE);

			Assert.IsFalse(arms.UpdateType(null));

			Assert.IsTrue(arms.Restore());
			Assert.IsTrue(arms.isDefault);
			Assert.IsFalse(arms.UpdateType(Arms.defaultType));

			//provide dummy data so the arms can correctly do parse epidermis data and work correctly. 
			((IBodyAware)arms).GetBodyData(BodyDataGetterDummy);

			foreach (var currType in ArmType.availableTypes)
			{
				//reset these to the current type we're checking.
				arms.UpdateType(currType);

				foreach (var newType in ArmType.availableTypes)
				{
					if (newType != arms.type)
					{
						Assert.IsTrue(arms.UpdateType(newType));
						EpidermalData epidermis = arms.epidermis;

						if (newType.hasPrimaryFur)
						{
							Assert.IsTrue(epidermis.usesFur);
						}
						else
						{
							Assert.IsTrue(epidermis.usesTone);
						}

						epidermis = arms.secondaryEpidermis;
						if (newType.hasSecondaryFur)
						{
							Assert.IsTrue(epidermis.usesFur);
						}
						else if (newType.hasSecondaryTone)
						{
							Assert.IsTrue(epidermis.usesTone);
						}
						else
						{
							Assert.IsTrue(epidermis.isEmpty);
						}

					}
					else
					{
						Assert.IsFalse(arms.UpdateType(newType));
					}
				}
			}
		}

		[TestMethod]
		public void Back_GenerateUpdateRestore()
		{
			Back back = Back.GenerateDefault();
			Assert.IsTrue(back.isDefault);

			Assert.ThrowsException<ArgumentNullException>(() => back = Back.GenerateDefaultOfType(null));

			//Attacks are integrated directly into back, so we don't need to worry about it. 
			back = Back.GenerateDefaultOfType(BackType.BEE_STINGER);
			Assert.AreEqual(back.type, BackType.BEE_STINGER);

			Assert.IsFalse(back.UpdateType(null));

			Assert.IsTrue(back.Restore());
			Assert.IsTrue(back.isDefault);
			Assert.IsFalse(back.UpdateType(Back.defaultType));
			foreach (BackType currtype in BackType.availableTypes)
			{
				foreach (BackType newType in BackType.availableTypes)
				{
					//reset these to the current type we're checking.
					back.UpdateType(currtype);

					if (newType != back.type)
					{
						Assert.IsTrue(back.UpdateType(newType));
						Assert.IsTrue((((ICanAttackWith)back).canAttackWith() && back.maxCharges != 0) || (!((ICanAttackWith)back).canAttackWith() && back.maxCharges == 0));
						Assert.IsTrue(back.type.hasSpecialEpidermis != back.backEpidermis.isEmpty); //if we have a special epidermis, epidermis isn't empty, and vice versa.
					}
					else
					{
						Assert.IsFalse(back.UpdateType(newType));
					}
				}
			}

			back.UpdateType(BackType.DRACONIC_MANE, HairFurColors.BROWN);
			Assert.IsTrue(back.backEpidermis.usesFur && back.backEpidermis.fur.IsIdenticalTo(HairFurColors.BROWN));
			Assert.IsFalse(back.UpdateType(BackType.DRACONIC_MANE, HairFurColors.WHITE));
			Assert.IsFalse(back.backEpidermis.fur.IsIdenticalTo(HairFurColors.WHITE));

			Assert.IsFalse(back.UpdateType(null, HairFurColors.WHITE));

			//back color cannot be changed unless dyed. old behavior was it didn't update to match hair color. it's possible to alter this behavior, and thus the results of this test.
			Assert.IsTrue(((IDyeable)back).attemptToDye(HairFurColors.WHITE));
			Assert.IsTrue(back.backEpidermis.fur.IsIdenticalTo(HairFurColors.WHITE));
		}

		[TestMethod]
		public void Body_GenerateUpdateRestore()
		{
			//Assert.Fail("Not implemented. should therefore fail");
			Body body = Body.GenerateDefault();
			Assert.AreEqual(body.type, Body.defaultType);

			Assert.ThrowsException<ArgumentNullException>(() => body = Body.GenerateDefaultOfType(null));

			body = Body.GenerateDefaultOfType(BodyType.CARAPACE);
			Assert.AreEqual(body.type, BodyType.CARAPACE);


			body = Body.GenerateDefaultOfType(BodyType.CARAPACE);
			Assert.AreEqual(body.type, BodyType.CARAPACE);

			Assert.ThrowsException<ArgumentNullException>(() => body = Body.GenerateFurredNoUnderbody(null, new FurColor(HairFurColors.GREEN)));
			Assert.ThrowsException<ArgumentNullException>(() => body = Body.GenerateFurredWithUnderbody(null, new FurColor(HairFurColors.GREEN), new FurColor(HairFurColors.INDIGO)));
			//Generates are the most confusing bit. We're gonna test them thoroughly here so they just work (TM) everywhere else. 
			body = Body.GenerateFurredNoUnderbody(BodyType.SIMPLE_FUR, new FurColor(HairFurColors.WHITE));
			Assert.IsTrue(body.mainEpidermis.usesFur);
			Assert.IsTrue(body.mainEpidermis.fur.IsIdenticalTo(HairFurColors.WHITE));

			body = Body.GenerateFurredWithUnderbody(BodyType.FEATHERED, new FurColor(HairFurColors.WHITE), new FurColor(HairFurColors.BLACK));
			Assert.IsTrue(body.mainEpidermis.usesFur);
			Assert.IsTrue(body.mainEpidermis.fur.IsIdenticalTo(HairFurColors.WHITE));
			Assert.IsTrue(body.supplementaryEpidermis.usesFur);
			Assert.IsTrue(body.supplementaryEpidermis.fur.IsIdenticalTo(HairFurColors.BLACK));

			body = Body.GenerateFurredWithUnderbody(BodyType.UNDERBODY_FUR, new FurColor(HairFurColors.BROWN, HairFurColors.GRAY, FurMulticolorPattern.NO_PATTERN), new FurColor(HairFurColors.ORANGE), FurTexture.SMOOTH);
			Assert.IsTrue(body.mainEpidermis.usesFur);
			Assert.AreEqual<FurColor>(body.mainEpidermis.fur, new FurColor(HairFurColors.BROWN, HairFurColors.GRAY, FurMulticolorPattern.NO_PATTERN));
			Assert.IsTrue(body.supplementaryEpidermis.usesFur);
			Assert.IsTrue(body.supplementaryEpidermis.fur.IsIdenticalTo(HairFurColors.ORANGE));
			Assert.AreEqual(FurTexture.SMOOTH, body.mainEpidermis.furTexture);
			Assert.AreEqual(FurTexture.NONDESCRIPT, body.supplementaryEpidermis.furTexture);

			body = Body.GenerateCockatrice(new FurColor(HairFurColors.GREY_GREEN), Tones.INDIGO, FurTexture.FLUFFY, SkinTexture.SHINY);
			Assert.AreEqual(body.type, BodyType.COCKATRICE);
			Assert.IsTrue(body.mainEpidermis.usesFur);
			Assert.IsTrue(body.supplementaryEpidermis.usesTone);
			Assert.AreEqual(body.mainEpidermis.fur, new FurColor(HairFurColors.GREY_GREEN));
			Assert.AreEqual(body.supplementaryEpidermis.tone, Tones.INDIGO);
			Assert.AreEqual(body.mainEpidermis.furTexture, FurTexture.FLUFFY);
			Assert.AreEqual(body.supplementaryEpidermis.skinTexture, SkinTexture.SHINY);

			body = Body.GenerateKitsune(Tones.EBONY, new FurColor(HairFurColors.AUBURN), SkinTexture.SEXY, FurTexture.SOFT);
			Assert.AreEqual(body.type, BodyType.KITSUNE);
			Assert.IsTrue(body.mainEpidermis.usesTone);
			Assert.IsTrue(body.supplementaryEpidermis.usesFur);
			Assert.AreEqual(body.mainEpidermis.tone, Tones.EBONY);
			Assert.AreEqual(body.supplementaryEpidermis.fur, new FurColor(HairFurColors.AUBURN));
			Assert.AreEqual(body.mainEpidermis.skinTexture, SkinTexture.SEXY);
			Assert.AreEqual(body.supplementaryEpidermis.furTexture, FurTexture.SOFT);

			body = Body.GenerateTonedNoUnderbody(BodyType.HUMANOID, Tones.FAIR);
			Assert.AreEqual(body.type, BodyType.HUMANOID);
			Assert.AreEqual(body.mainEpidermis.tone, Tones.FAIR);

			body = Body.GenerateTonedWithUnderbody(BodyType.NAGA, Tones.EMERALD, Tones.GOLD);
			Assert.AreEqual(body.type, BodyType.NAGA);
			Assert.IsTrue(body.mainEpidermis.usesTone);
			Assert.AreEqual(body.mainEpidermis.tone, Tones.EMERALD);
			Assert.IsTrue(body.supplementaryEpidermis.usesTone);
			Assert.AreEqual(body.supplementaryEpidermis.tone, Tones.GOLD);

			body = Body.GenerateTonedWithUnderbody(BodyType.REPTILIAN, Tones.SILVER, Tones.SANGUINE, SkinTexture.THICK, SkinTexture.ROUGH);
			Assert.AreEqual(body.type, BodyType.REPTILIAN);
			Assert.IsTrue(body.mainEpidermis.usesTone);
			Assert.AreEqual(body.mainEpidermis.tone, Tones.SILVER);
			Assert.IsTrue(body.supplementaryEpidermis.usesTone);
			Assert.AreEqual(body.supplementaryEpidermis.tone, Tones.SANGUINE);
			Assert.AreEqual(body.mainEpidermis.skinTexture, SkinTexture.THICK);
			Assert.AreEqual(body.supplementaryEpidermis.skinTexture, SkinTexture.ROUGH);

			((IHairAware)body).GetHairData(HairDataGetterDummy);

			Assert.IsFalse(body.UpdateType(null));

			Assert.IsTrue(body.Restore());
			Assert.IsTrue(body.isDefault);
			Assert.IsFalse(body.UpdateType(Body.defaultType));

			foreach (BodyType currtype in BodyType.availableTypes)
			{
				foreach (BodyType newType in BodyType.availableTypes)
				{
					//reset these to the current type we're checking.
					body.UpdateType(currtype);

					if (newType != body.type)
					{
						Assert.IsTrue(body.UpdateType(newType));
					}
					else
					{
						Assert.IsFalse(body.UpdateType(newType));
					}

				}
			}
		}

		[TestMethod]
		public void Cock_GenerateUpdateRestore()
		{
			Cock cock = Cock.GenerateDefault();
			Assert.IsTrue(cock.isDefault);

			Assert.ThrowsException<ArgumentNullException>(() => cock = Cock.GenerateDefaultOfType(null));

			cock = Cock.GenerateDefaultOfType(CockType.BEE);
			Assert.AreEqual(cock.type, CockType.BEE);

			Assert.IsFalse(cock.UpdateType(null));

			cock = Cock.GenerateFromGender(Gender.FEMALE);
			Assert.IsNull(cock);
			cock = Cock.GenerateFromGender(Gender.MALE);
			Assert.IsNotNull(cock);
			cock = Cock.GenerateFromGender(Gender.GENDERLESS);
			Assert.IsNull(cock);
			cock = Cock.GenerateFromGender(Gender.HERM);
			Assert.IsNotNull(cock);

			cock = Cock.GenerateWithKnot(CockType.HUMAN, 5.5f, 1.2f, 1.1f);
			Assert.IsFalse(cock.hasKnot);
			cock = Cock.GenerateWithKnot(CockType.DOG, 7.5f, 1.6f, 1.4f);
			Assert.IsTrue(cock.hasKnot);

			Assert.ThrowsException<ArgumentNullException>(() => cock = Cock.GenerateWithKnot(null, 5.5f, 1.2f, 1.1f));

			Clit clit = Clit.GenerateWithLength(1.0f);
			cock = Cock.GenerateClitCock(clit);
			Assert.IsTrue(cock.type == CockType.HUMAN && cock.cockLength == 1.0f + 5.0f);

			Assert.ThrowsException<ArgumentNullException>(() => cock = Cock.GenerateClitCock(null));

			Assert.ThrowsException<ArgumentNullException>(() => cock = Cock.Generate(null, 5.5f, 1.2f));

			cock = Cock.Generate(CockType.DRAGON, 10.1f, 2.0f);
			Assert.IsTrue(cock.hasKnot);


			cock = Cock.Generate(CockType.CAT, 8.0f, 1.5f);
			Assert.IsFalse(cock.hasKnot);


			Assert.IsTrue(cock.Restore());
			Assert.IsTrue(cock.isDefault);
			Assert.IsFalse(cock.UpdateType(Cock.defaultType));

			foreach (CockType currtype in CockType.availableTypes)
			{
				foreach (CockType newType in CockType.availableTypes)
				{
					//reset these to the current type we're checking.
					cock.UpdateType(currtype);

					if (newType != cock.type)
					{
						//Standard UpdateType()
						Assert.IsTrue(cock.UpdateType(newType));
						Assert.IsTrue(cock.type.hasKnot == cock.knotMultiplier > 1.0f);
						float girthToLengthRatio = cock.cockGirth / cock.cockLength;
						Assert.IsTrue(girthToLengthRatio >= cock.type.minGirthToLengthRatio && girthToLengthRatio <= cock.type.maxGirthToLengthRatio);
					}
					else
					{
						Assert.IsFalse(cock.UpdateType(newType));
					}

				}
			}

			//cock.Upda
		}

		//this is unit test, so fake or dummy data for body aware. integration test in creature will handle this.

		[TestMethod]
		public void Ear_GenerateUpdateRestore()
		{
			Ears ears = Ears.GenerateDefault();
			Assert.IsTrue(ears.isDefault);

			Assert.ThrowsException<ArgumentNullException>(() => ears = Ears.GenerateDefaultOfType(null));

			ears = Ears.GenerateDefaultOfType(EarType.ELFIN);
			Assert.AreEqual(ears.type, EarType.ELFIN);

			Assert.IsFalse(ears.UpdateType(null));

			Assert.IsTrue(ears.Restore());
			Assert.IsTrue(ears.isDefault);
			Assert.IsFalse(ears.UpdateType(Ears.defaultType));

			((IBodyAware)ears).GetBodyData(BodyDataGetterDummy);

			foreach (EarType currtype in EarType.availableTypes)
			{
				foreach (EarType newType in EarType.availableTypes)
				{
					//reset these to the current type we're checking.
					ears.UpdateType(currtype);

					if (newType != ears.type)
					{
						Assert.IsTrue(ears.UpdateType(newType));
						Assert.IsNotNull(ears.earFurColor); //just here to make sure it doesn't explode.
					}
					else
					{
						Assert.IsFalse(ears.UpdateType(newType));
					}
				}
			}
		}
		[TestMethod]
		public void Eye_GenerateUpdateRestore()
		{
			Eyes eyes = Eyes.GenerateDefault();
			Assert.IsTrue(eyes.isDefault);

			Assert.ThrowsException<ArgumentNullException>(() => eyes = Eyes.GenerateDefaultOfType(null));

			eyes = Eyes.GenerateDefaultOfType(EyeType.BASILISK);
			Assert.AreEqual(eyes.type, EyeType.BASILISK);
			Assert.AreEqual(eyes.leftIrisColor, eyes.rightIrisColor);

			eyes = Eyes.GenerateDefaultOfType(EyeType.CAT);
			Assert.AreEqual(eyes.type, EyeType.CAT);
			Assert.AreEqual(eyes.leftIrisColor, eyes.rightIrisColor);
			Assert.AreNotEqual(EyeColor.BLUE, eyes.leftIrisColor); //default is green. should therefore not be equal.

			Assert.ThrowsException<ArgumentNullException>(() => eyes = Eyes.GenerateWithColor(null, EyeColor.AMBER));
			Assert.ThrowsException<ArgumentNullException>(() => eyes = Eyes.GenerateWithHeterochromia(null, EyeColor.AMBER, EyeColor.GRAY));


			eyes = Eyes.GenerateWithColor(EyeType.CAT, EyeColor.BLUE);
			Assert.AreEqual(EyeType.CAT, eyes.type);
			Assert.AreEqual(eyes.leftIrisColor, eyes.rightIrisColor);
			Assert.AreEqual(EyeColor.BLUE, eyes.leftIrisColor);

			eyes = Eyes.GenerateWithHeterochromia(EyeType.DRAGON, EyeColor.BLUE, EyeColor.GREEN);
			Assert.AreEqual(EyeType.DRAGON, eyes.type);
			Assert.AreNotEqual(eyes.leftIrisColor, eyes.rightIrisColor);
			Assert.AreEqual(EyeColor.BLUE, eyes.leftIrisColor);
			Assert.AreEqual(EyeColor.GREEN, eyes.rightIrisColor);

			Assert.IsFalse(eyes.UpdateType(null));

			Assert.IsTrue(eyes.Restore());
			Assert.IsTrue(eyes.isDefault);
			Assert.AreEqual(EyeColor.BLUE, eyes.leftIrisColor);
			Assert.AreEqual(EyeColor.GREEN, eyes.rightIrisColor);
			Assert.IsFalse(eyes.UpdateType(Eyes.defaultType));

			eyes.Reset();
			Assert.AreEqual(Eyes.defaultType.defaultColor, eyes.leftIrisColor);
			Assert.AreEqual(Eyes.defaultType.defaultColor, eyes.rightIrisColor);

			foreach (EyeType currtype in EyeType.availableTypes)
			{
				foreach (EyeType newType in EyeType.availableTypes)
				{
					//reset these to the current type we're checking.
					eyes.UpdateType(currtype);

					if (newType != eyes.type)
					{
						EyeColor leftEye = eyes.leftIrisColor;
						EyeColor rightEye = eyes.rightIrisColor;
						Assert.IsTrue(eyes.UpdateType(newType));
						Assert.IsTrue(eyes.leftIrisColor == leftEye && eyes.rightIrisColor == rightEye);
					}
					else
					{
						Assert.IsFalse(eyes.UpdateType(newType));
					}
				}
			}
		}
		[TestMethod]
		public void Face_GenerateUpdateRestore()
		{
			Face face = Face.GenerateDefault();
			Assert.IsTrue(face.isDefault);

			Assert.ThrowsException<ArgumentNullException>(() => face = Face.GenerateDefaultOfType(null));
			Assert.ThrowsException<ArgumentNullException>(() => face = Face.GenerateWithComplexion(null, SkinTexture.SEXY));
			Assert.ThrowsException<ArgumentNullException>(() => face = Face.GenerateWithMorph(null, true));
			Assert.ThrowsException<ArgumentNullException>(() => face = Face.GenerateWithMorphAndComplexion(null, true, SkinTexture.SMOOTH));

			face = Face.GenerateDefaultOfType(FaceType.BEAK);
			Assert.AreEqual(face.type, FaceType.BEAK);

			Assert.IsFalse(face.UpdateType(null));

			face = Face.GenerateWithComplexion(FaceType.COCKATRICE, SkinTexture.FRECKLED);
			Assert.AreEqual(SkinTexture.FRECKLED, face.skinTexture);
			Assert.AreEqual(FaceType.COCKATRICE, face.type);

			face = Face.GenerateWithMorph(FaceType.HUMAN, true);
			Assert.IsFalse(face.isFullMorph);
			face = Face.GenerateWithMorph(FaceType.CAT, true);
			Assert.IsTrue(face.isFullMorph);

			face = Face.GenerateWithMorphAndComplexion(FaceType.HUMAN, true, SkinTexture.FRECKLED);
			Assert.AreEqual(SkinTexture.FRECKLED, face.skinTexture);
			Assert.IsFalse(face.isFullMorph);
			face = Face.GenerateWithMorphAndComplexion(FaceType.CAT, true, SkinTexture.SOFT);
			Assert.AreEqual(SkinTexture.SOFT, face.skinTexture);
			Assert.IsTrue(face.isFullMorph);


			Assert.IsTrue(face.Restore());
			Assert.IsTrue(face.isDefault);
			Assert.IsFalse(face.UpdateType(Face.defaultType));

			foreach (FaceType currtype in FaceType.availableTypes)
			{
				foreach (FaceType newType in FaceType.availableTypes)
				{
					//reset these to the current type we're checking.
					face.UpdateType(currtype);

					if (newType != face.type)
					{
						Assert.IsTrue(face.UpdateType(newType));
					}
					else
					{
						Assert.IsFalse(face.UpdateType(newType));
					}
				}
			}
		}

		[TestMethod]

		public void Gill_GenerateUpdateRestore()
		{
			Gills gills = Gills.GenerateDefault();
			Assert.IsTrue(gills.isDefault);

			Assert.ThrowsException<ArgumentNullException>(() => gills = Gills.GenerateDefaultOfType(null));

			gills = Gills.GenerateDefaultOfType(GillType.ANEMONE);
			Assert.AreEqual(gills.type, GillType.ANEMONE);

			Assert.IsFalse(gills.UpdateType(null));

			Assert.IsTrue(gills.Restore());
			Assert.IsTrue(gills.isDefault);
			Assert.IsFalse(gills.UpdateType(Gills.defaultType));

			foreach (GillType currtype in GillType.availableTypes)
			{
				foreach (GillType newType in GillType.availableTypes)
				{
					//reset these to the current type we're checking.
					gills.UpdateType(currtype);

					if (newType != gills.type)
					{
						Assert.IsTrue(gills.UpdateType(newType));
					}
					else
					{
						Assert.IsFalse(gills.UpdateType(newType));
					}
				}
			}
		}
		[TestMethod]
		public void Hair_GenerateUpdateRestore()
		{
			Hair hair = Hair.GenerateDefault();
			Assert.IsTrue(hair.isDefault);

			Assert.ThrowsException<ArgumentNullException>(() => hair = Hair.GenerateDefaultOfType(null));

			hair = Hair.GenerateDefaultOfType(HairType.ANEMONE);
			Assert.AreEqual(hair.type, HairType.ANEMONE);

			Assert.IsFalse(hair.UpdateType(null));

			Assert.ThrowsException<ArgumentNullException>(() => hair = Hair.GenerateWithLength(null, 4.5f));

			hair = Hair.GenerateWithLength(HairType.FEATHER, 10.5f);
			Assert.IsTrue(hair.type.isFixedLength != (hair.length == 10.5f));
			Assert.IsTrue(hair.length == 10.5f);

			hair = Hair.GenerateWithLength(HairType.BASILISK_SPINES, 10.5f);
			Assert.IsTrue(hair.type.isFixedLength != (hair.length == 10.5f));
			Assert.IsFalse(hair.length == 10.5f);

			Assert.ThrowsException<ArgumentNullException>(() => hair = Hair.GenerateWithColor(null, HairFurColors.BLONDE));

			hair = Hair.GenerateWithColor(HairType.BASILISK_SPINES, HairFurColors.CHARTREUSE);
			Assert.IsTrue(hair.type.canDye == (hair.hairColor == HairFurColors.CHARTREUSE));
			Assert.IsFalse(hair.hairColor == HairFurColors.CHARTREUSE);

			hair = Hair.GenerateWithColor(HairType.GOO, HairFurColors.CARAMEL, 15.0f);
			Assert.IsTrue(hair.type.canDye == (hair.hairColor == HairFurColors.CARAMEL));
			Assert.IsTrue(hair.hairColor == HairFurColors.CARAMEL);
			Assert.IsTrue(hair.type.isFixedLength != (hair.length == 15.0f));
			Assert.IsTrue(hair.length == 15.0f);

			Assert.ThrowsException<ArgumentNullException>(() => hair = Hair.GenerateWithColorAndHighlight(null, HairFurColors.BLACK, HairFurColors.PURPLE));

			hair = Hair.GenerateWithColorAndHighlight(HairType.LEAF, HairFurColors.BLACK, HairFurColors.PURPLE);
			Assert.IsTrue(hair.type.canDye == (hair.hairColor == HairFurColors.BLACK));
			Assert.IsTrue(hair.hairColor == HairFurColors.BLACK);
			Assert.IsTrue(hair.type.canDye == (hair.highlightColor == HairFurColors.PURPLE));
			Assert.IsTrue(hair.highlightColor == HairFurColors.PURPLE);

			hair = Hair.GenerateWithColorAndHighlight(HairType.WOOL, HairFurColors.BROWN, HairFurColors.BLONDE, 17.0f);
			Assert.IsTrue(hair.type.canDye == (hair.hairColor == HairFurColors.BROWN));
			Assert.IsTrue(hair.hairColor == HairFurColors.BROWN);
			Assert.IsTrue(hair.type.canDye == (hair.highlightColor == HairFurColors.BLONDE));
			Assert.IsTrue(hair.highlightColor == HairFurColors.BLONDE);
			Assert.IsTrue(hair.type.isFixedLength != (hair.length == 17.0f));
			Assert.IsTrue(hair.length == 17.0f);

			//due to the nature of how hair types change, we'll need to test them manually. but for now this is viable for the standard tests.

			Assert.IsTrue(hair.Restore());
			Assert.IsTrue(hair.isDefault); //caught an error where i didn't properly implement the set in type property.
			Assert.IsFalse(hair.UpdateType(Hair.defaultType));

			//normally part of extra tests, but this test broke in debug mode when viewing hair's values. this was added in to find out why. 
			foreach (HairType currtype in HairType.availableTypes) 
			{
				AttackBase attack = currtype.attack; //caught a StackOverflow with infinite recursion (attack => attack, not attack => _attack)
				Assert.IsNotNull(attack);
			}

			foreach (HairType currtype in HairType.availableTypes)
			{
				foreach (HairType newType in HairType.availableTypes)
				{
					//reset these to the current type we're checking.
					hair.UpdateType(currtype);

					if (newType != hair.type)
					{
						Assert.IsTrue(hair.UpdateType(newType));
					}
					else
					{
						Assert.IsFalse(hair.UpdateType(newType));
					}
				}
			}
		}

		//horn validation on this is weird. it'll need separate tests, and will be done in extra tests. 

		[TestMethod]
		public void Horn_GenerateUpdateRestore()
		{
			FemininityData dummyInitialData = FemininityDataGetterDummy();

			Horns horns;

			Assert.ThrowsException<ArgumentNullException>(() => horns = Horns.GenerateDefault(null));


			foreach (var data in HornType.availableTypes) //test below strangely failed. this proved that all defaults were 0. now fixed. 
			{
				Console.WriteLine(data.defaultHorns);
			}

			horns = Horns.GenerateDefault(dummyInitialData);
			Assert.IsTrue(horns.isDefault);

			Assert.ThrowsException<ArgumentNullException>(() => horns = Horns.GenerateDefaultOfType(null, dummyInitialData));
			Assert.ThrowsException<ArgumentNullException>(() => horns = Horns.GenerateDefaultOfType(HornType.BULL_LIKE, null));

			horns = Horns.GenerateDefaultOfType(HornType.BULL_LIKE, dummyInitialData);
			Assert.AreEqual(horns.type, HornType.BULL_LIKE);

			Assert.IsFalse(horns.UpdateType(null));

			Assert.ThrowsException<ArgumentNullException>(() => horns = Horns.GenerateWithExtraStrength(null, dummyInitialData, 2));
			Assert.ThrowsException<ArgumentNullException>(() => horns = Horns.GenerateWithExtraStrength(HornType.DRACONIC, null, 2));
			Assert.ThrowsException<ArgumentNullException>(() => horns = Horns.GenerateOverride(HornType.DEER_ANTLERS, null, 12, 4));

			//test one with variable size and count based on strength.
			horns = Horns.GenerateWithExtraStrength(HornType.DRACONIC, dummyInitialData, 4);
			Assert.AreEqual(HornType.DRACONIC, horns.type);
			Assert.AreEqual<byte>(12, horns.significantHornSize);
			Assert.AreEqual<byte>(2, horns.numHorns); //strange error with Clamp and these values. i converted to Clamp2 for cleanliness, and it somehow got fixed.

			//test one with single size and count.
			horns = Horns.GenerateDefaultOfType(HornType.IMP, dummyInitialData);
			byte hornCount = horns.numHorns;
			byte hornSize = horns.significantHornSize;
			horns = Horns.GenerateWithExtraStrength(HornType.IMP, dummyInitialData, 1);
			Assert.AreEqual(hornCount, horns.numHorns);
			Assert.AreEqual(hornSize, horns.significantHornSize);

			//test one with a valid override. Note that this is a very specific case as deer horn length = horn count + 4. Try it with 18 and it will fail.
			horns = Horns.GenerateOverride(HornType.DEER_ANTLERS, dummyInitialData, 16, 12);
			Assert.AreEqual(horns.type, HornType.DEER_ANTLERS);
			Assert.AreEqual<byte>(16, horns.significantHornSize);
			Assert.AreEqual<byte>(12, horns.numHorns);

			//test one with a partially invalid override. (12 is okay, 2 is not)
			horns = Horns.GenerateOverride(HornType.UNICORN, dummyInitialData, 12, 2);
			Assert.AreEqual(horns.type, HornType.UNICORN);
			Assert.AreEqual<byte>(12, horns.significantHornSize);
			Assert.AreNotEqual<byte>(2, horns.numHorns);

			//test one with a fully invalid override.
			horns = Horns.GenerateOverride(HornType.UNICORN, dummyInitialData, 15, 2);
			Assert.AreEqual(horns.type, HornType.UNICORN);
			Assert.AreNotEqual<byte>(15, horns.significantHornSize);
			Assert.AreNotEqual<byte>(2, horns.numHorns);


			Assert.IsTrue(horns.Restore());
			Assert.IsTrue(horns.isDefault);
			Assert.IsFalse(horns.UpdateType(Horns.defaultType));

			foreach (HornType currtype in HornType.availableTypes)
			{
				foreach (HornType newType in HornType.availableTypes)
				{
					//reset these to the current type we're checking.
					horns.UpdateType(currtype);

					if (newType != horns.type)
					{
						Assert.IsTrue(horns.UpdateType(newType));
					}
					else
					{
						Assert.IsFalse(horns.UpdateType(newType));
					}
				}
			}
		}
		[TestMethod]
		public void LowerBody_GenerateUpdateRestore()
		{
			LowerBody lowerBody = LowerBody.GenerateDefault();
			Assert.IsTrue(lowerBody.isDefault);

			Assert.ThrowsException<ArgumentNullException>(() => lowerBody = LowerBody.GenerateDefaultOfType(null));

			lowerBody = LowerBody.GenerateDefaultOfType(LowerBodyType.BEE);
			Assert.AreEqual(lowerBody.type, LowerBodyType.BEE);

			Assert.IsFalse(lowerBody.UpdateType(null));

			Assert.IsTrue(lowerBody.Restore());
			Assert.IsTrue(lowerBody.isDefault);
			Assert.IsFalse(lowerBody.UpdateType(LowerBody.defaultType));

			foreach (LowerBodyType currtype in LowerBodyType.availableTypes)
			{
				foreach (LowerBodyType newType in LowerBodyType.availableTypes)
				{
					//reset these to the current type we're checking.
					lowerBody.UpdateType(currtype);

					if (newType != lowerBody.type)
					{
						Assert.IsTrue(lowerBody.UpdateType(newType));
					}
					else
					{
						Assert.IsFalse(lowerBody.UpdateType(newType));
					}
				}
			}
		}
		[TestMethod]
		public void Neck_GenerateUpdateRestore()
		{
			Neck neck = Neck.GenerateDefault();
			Assert.IsTrue(neck.isDefault);

			Assert.ThrowsException<ArgumentNullException>(() => neck = Neck.GenerateDefaultOfType(null)); //now correctly throws argument null instead of null reference

			neck = Neck.GenerateDefaultOfType(NeckType.COCKATRICE);
			Assert.AreEqual(neck.type, NeckType.COCKATRICE);

			Assert.ThrowsException<ArgumentNullException>(() => neck = Neck.GenerateNonDefault(null, 3));
			//test neck with invalid length
			neck = Neck.GenerateNonDefault(NeckType.HUMANOID, 4);
			Assert.AreEqual(NeckType.HUMANOID, neck.type);
			Assert.AreNotEqual(4, neck.length);

			//test neck with valid length
			neck = Neck.GenerateNonDefault(NeckType.DRACONIC, 4);
			Assert.AreEqual(NeckType.DRACONIC, neck.type);
			Assert.AreEqual(4, neck.length);

			neck = Neck.GenerateDefaultOfType(NeckType.COCKATRICE);
			Assert.AreEqual(neck.type, NeckType.COCKATRICE);

			Assert.IsFalse(neck.UpdateType(null));

			Assert.IsTrue(neck.Restore());
			Assert.IsTrue(neck.isDefault);
			Assert.IsFalse(neck.UpdateType(Neck.defaultType));

			foreach (NeckType currtype in NeckType.availableTypes)
			{
				foreach (NeckType newType in NeckType.availableTypes)
				{
					//reset these to the current type we're checking.
					neck.UpdateType(currtype);

					if (newType != neck.type)
					{
						Assert.IsTrue(neck.UpdateType(newType));
					}
					else
					{
						Assert.IsFalse(neck.UpdateType(newType));
					}
				}
			}
		}
		[TestMethod]
		public void Tail_GenerateUpdateRestore()
		{
			Tail tail = Tail.GenerateDefault();
			Assert.IsTrue(tail.isDefault);

			Assert.ThrowsException<ArgumentNullException>(() => tail = Tail.GenerateDefaultOfType(null)); //now correctly throws argument null instead of null reference

			tail = Tail.GenerateDefaultOfType(TailType.COCKATRICE);
			Assert.AreEqual(tail.type, TailType.COCKATRICE);

			Assert.ThrowsException<ArgumentNullException>(() => tail = Tail.GenerateWithCount(null, 3));
			//test tail with invalid count
			tail = Tail.GenerateWithCount(TailType.DOG, 4);
			Assert.AreEqual(TailType.DOG, tail.type);
			Assert.AreNotEqual(4, tail.tailCount);

			//test tail with valid length
			tail = Tail.GenerateWithCount(TailType.FOX, 4);
			Assert.AreEqual(TailType.FOX, tail.type);
			Assert.AreEqual(4, tail.tailCount);

			tail = Tail.GenerateDefaultOfType(TailType.COCKATRICE);
			Assert.AreEqual(tail.type, TailType.COCKATRICE);

			Assert.IsFalse(tail.UpdateType(null));

			Assert.IsTrue(tail.Restore());
			Assert.IsTrue(tail.isDefault);
			Assert.IsFalse(tail.UpdateType(Tail.defaultType));

			foreach (TailType currtype in TailType.availableTypes)
			{
				foreach (TailType newType in TailType.availableTypes)
				{
					//reset these to the current type we're checking.
					tail.UpdateType(currtype);

					if (newType != tail.type)
					{
						Assert.IsTrue(tail.UpdateType(newType));
					}
					else
					{
						Assert.IsFalse(tail.UpdateType(newType));
					}
				}
			}
		}
		[TestMethod]
		public void Tongue_GenerateUpdateRestore()
		{
			Tongue tongue = Tongue.GenerateDefault();
			Assert.IsTrue(tongue.isDefault);

			Assert.ThrowsException<ArgumentNullException>(() => tongue = Tongue.GenerateDefaultOfType(null));

			tongue = Tongue.GenerateDefaultOfType(TongueType.DRACONIC);
			Assert.AreEqual(tongue.type, TongueType.DRACONIC);

			Assert.IsFalse(tongue.UpdateType(null));

			Assert.IsTrue(tongue.Restore());
			Assert.IsTrue(tongue.isDefault);
			Assert.IsFalse(tongue.UpdateType(Tongue.defaultType));

			foreach (TongueType currtype in TongueType.availableTypes)
			{
				foreach (TongueType newType in TongueType.availableTypes)
				{
					//reset these to the current type we're checking.
					tongue.UpdateType(currtype);

					if (newType != tongue.type)
					{
						Assert.IsTrue(tongue.UpdateType(newType));
					}
					else
					{
						Assert.IsFalse(tongue.UpdateType(newType));
					}
				}
			}
		}

		[TestMethod]
		public void Vagina_GenerateUpdateRestore()
		{
			Vagina vagina = Vagina.GenerateDefault();
			Assert.IsTrue(vagina.isDefault);

			Assert.ThrowsException<ArgumentNullException>(() => vagina = Vagina.GenerateDefaultOfType(null));

			vagina = Vagina.GenerateDefaultOfType(VaginaType.EQUINE);
			Assert.AreEqual(vagina.type, VaginaType.EQUINE);

			Assert.IsFalse(vagina.UpdateType(null));

			vagina = Vagina.GenerateFromGender(Gender.FEMALE);
			Assert.IsNotNull(vagina);
			vagina = Vagina.GenerateFromGender(Gender.MALE);
			Assert.IsNull(vagina);
			vagina = Vagina.GenerateFromGender(Gender.GENDERLESS);
			Assert.IsNull(vagina);
			vagina = Vagina.GenerateFromGender(Gender.HERM);
			Assert.IsNotNull(vagina);

			vagina = Vagina.Generate(VaginaType.HUMAN, 2.5f, VaginalLooseness.LOOSE, VaginalWetness.DROOLING);
			Assert.IsFalse(vagina.virgin);
			Assert.AreEqual(VaginalLooseness.LOOSE, vagina.looseness);
			Assert.AreEqual(VaginalWetness.DROOLING, vagina.wetness);

			vagina = Vagina.Generate(VaginaType.SAND_TRAP, 7.5f, VaginalLooseness.TIGHT, VaginalWetness.DRY, false);
			Assert.IsFalse(vagina.virgin);
			Assert.AreEqual(VaginalLooseness.TIGHT, vagina.looseness);
			Assert.AreEqual(VaginalWetness.DRY, vagina.wetness);

			Assert.ThrowsException<ArgumentNullException>(() => vagina = Vagina.Generate(null, 5.5f, VaginalLooseness.LOOSE, VaginalWetness.WET, true));

			//min clit length for omnibus is 2.
			vagina = Vagina.GenerateOmnibus(VaginaType.EQUINE, 1.5f, VaginalLooseness.ROOMY);
			Assert.IsTrue(vagina.clit.length >= 2f);
			Assert.IsTrue(vagina.clit.omnibusClit);
			Assert.IsTrue(vagina.clit.AsCock()?.cockLength >= 7f);

			Assert.ThrowsException<ArgumentNullException>(() => vagina = Vagina.GenerateOmnibus(null));

			Assert.IsTrue(vagina.Restore());
			Assert.IsTrue(vagina.isDefault);
			Assert.IsFalse(vagina.UpdateType(Vagina.defaultType));

			foreach (VaginaType currtype in VaginaType.availableTypes)
			{
				foreach (VaginaType newType in VaginaType.availableTypes)
				{
					//reset these to the current type we're checking.
					vagina.UpdateType(currtype);

					if (newType != vagina.type)
					{
						Assert.IsTrue(vagina.UpdateType(newType));
					}
					else
					{
						Assert.IsFalse(vagina.UpdateType(newType));
					}
				}
			}
		}

		[TestMethod]
		public void Wing_GenerateUpdateRestore()
		{
			//Assert.Fail("Not implemented. should therefore fail");
			Wings wings = Wings.GenerateDefault();
			Assert.AreEqual(wings.type, Wings.defaultType);

			Assert.ThrowsException<ArgumentNullException>(() => wings = Wings.GenerateDefaultOfType(null));

			wings = Wings.GenerateDefaultOfType(WingType.BEE_LIKE);
			Assert.AreEqual(wings.type, WingType.BEE_LIKE);

			Assert.ThrowsException<ArgumentNullException>(() => wings = Wings.GenerateDefaultWithSize(null, true));
			Assert.ThrowsException<ArgumentNullException>(() => wings = Wings.GenerateColored(null, HairFurColors.GREEN));
			Assert.ThrowsException<ArgumentNullException>(() => wings = Wings.GenerateColored(null, Tones.GREEN));
			Assert.ThrowsException<ArgumentNullException>(() => wings = Wings.GenerateColoredWithSize(null, HairFurColors.GREEN, false));
			Assert.ThrowsException<ArgumentNullException>(() => wings = Wings.GenerateColoredWithSize(null, Tones.GREEN, true));
			Assert.ThrowsException<ArgumentNullException>(() => wings = Wings.GenerateColoredWithSize(null, Tones.GREEN, Tones.INDIGO, false));

			wings = Wings.GenerateDefaultOfType(WingType.DRAGONFLY);
			Assert.AreEqual(WingType.DRAGONFLY, wings.type);
			//All dragonfly wings can fly, so this is true
			Assert.IsTrue(wings.canFly);

			wings = Wings.GenerateDefaultWithSize(WingType.DRAGONFLY, false);
			Assert.AreEqual(WingType.DRAGONFLY, wings.type);
			//All dragonfly wings can fly, so this is true, even though we tried to set it to false above.
			Assert.IsTrue(wings.canFly);

			wings = Wings.GenerateDefaultOfType(WingType.BAT_LIKE);
			Assert.AreEqual(WingType.BAT_LIKE, wings.type);
			//BatWings are size dependant, and default to current size. on generate, this is small, so this is false
			Assert.IsFalse(wings.canFly);

			wings = Wings.GenerateDefaultWithSize(WingType.BAT_LIKE, true);
			Assert.AreEqual(WingType.BAT_LIKE, wings.type);
			//Would be small, but we overrode it to false and it does have multiple sizes. should return true.
			Assert.IsTrue(wings.canFly);

			wings = Wings.GenerateColored(WingType.FEATHERED, HairFurColors.WHITE);
			Assert.IsTrue(wings.canFly); //converts to large, so can fly.
			Assert.AreEqual(HairFurColors.WHITE, wings.featherColor);
			Assert.AreEqual(Tones.NOT_APPLICABLE, wings.wingTone);
			Assert.AreEqual(Tones.NOT_APPLICABLE, wings.wingBoneTone);

			wings = Wings.GenerateColored(WingType.DRACONIC, Tones.WHITE, Tones.SILVER);
			Assert.IsFalse(wings.canFly); //converts to large, so can fly.
			Assert.AreEqual(Tones.WHITE, wings.wingTone);
			Assert.AreEqual(Tones.SILVER, wings.wingBoneTone);
			Assert.AreEqual(HairFurColors.NO_HAIR_FUR, wings.featherColor);

			wings = Wings.GenerateColoredWithSize(WingType.FEATHERED, HairFurColors.PINK, false);
			Assert.IsFalse(wings.canFly); //converts to large, so can fly.
			Assert.AreEqual(HairFurColors.PINK, wings.featherColor);
			Assert.AreEqual(Tones.NOT_APPLICABLE, wings.wingTone);
			Assert.AreEqual(Tones.NOT_APPLICABLE, wings.wingBoneTone);

			wings = Wings.GenerateColoredWithSize(WingType.DRACONIC, Tones.OLIVE, Tones.ORANGE, true);
			Assert.IsTrue(wings.canFly); //converts to large, so can fly.
			Assert.AreEqual(Tones.OLIVE, wings.wingTone);
			Assert.AreEqual(Tones.ORANGE, wings.wingBoneTone);
			Assert.AreEqual(HairFurColors.NO_HAIR_FUR, wings.featherColor);

			wings = Wings.GenerateColoredWithSize(WingType.DRACONIC, Tones.OLIVE, true);
			Assert.IsTrue(wings.canFly); //converts to large, so can fly.
			Assert.AreEqual(Tones.OLIVE, wings.wingTone);
			Assert.AreNotEqual(Tones.ORANGE, wings.wingBoneTone);
			Assert.AreEqual(Tones.OLIVE, wings.wingBoneTone);
			Assert.AreEqual(HairFurColors.NO_HAIR_FUR, wings.featherColor);

			wings = Wings.GenerateColored(WingType.DRACONIC, Tones.OLIVE);
			Assert.IsFalse(wings.canFly); //converts to large, so can fly.
			Assert.AreEqual(Tones.OLIVE, wings.wingTone);
			Assert.AreNotEqual(Tones.ORANGE, wings.wingBoneTone);
			Assert.AreEqual(Tones.OLIVE, wings.wingBoneTone);
			Assert.AreEqual(HairFurColors.NO_HAIR_FUR, wings.featherColor);

			Assert.IsFalse(wings.UpdateType(null));

			Assert.IsTrue(wings.Restore());
			Assert.IsTrue(wings.isDefault);
			Assert.IsFalse(wings.UpdateType(Wings.defaultType));

			foreach (WingType currtype in WingType.availableTypes)
			{
				foreach (WingType newType in WingType.availableTypes)
				{
					//reset these to the current type we're checking.
					wings.UpdateType(currtype);

					if (newType != wings.type)
					{
						Assert.IsTrue(wings.UpdateType(newType));
					}
					else
					{
						Assert.IsFalse(wings.UpdateType(newType));
					}

				}
			}
		}
	}
}
