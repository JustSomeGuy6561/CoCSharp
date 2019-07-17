using CoC.Backend.BodyParts;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CoCBackendUnitTests
{
	//Quick aside: for awares, integration testing where they get valid data from another body part
	//will be done in creature. For now, any valid data will do, and therefore we provide it with fakes (i call them dummies)

	[TestClass]
	public class BodyPartStandardTests
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

		internal BuildData BuildDataGetterDummy()
		{
			
		}

		internal FemininityData FemininityDataGetterDummy()
		{

		}

		internal LowerBodyData LowerBodyDataGetterDummy()
		{

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

			Assert.ThrowsException<ArgumentNullException>(() => arms = Arms.GenerateDefaultOfType(null));

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
			Assert.Fail("Not implemented. should therefore fail");
			Body body = Body.GenerateDefault();
			Assert.AreEqual(body.type, Body.defaultType);

			Assert.ThrowsException<ArgumentNullException>(() => body = Body.GenerateDefaultOfType(null));

			body = Body.GenerateDefaultOfType(BodyType.CARAPACE);
			Assert.AreEqual(body.type, BodyType.CARAPACE);

			Assert.IsFalse(body.UpdateType(null));


			Assert.IsTrue(body.Restore());
			Assert.IsTrue(body.isDefault);
			Assert.IsFalse(body.UpdateType(Body.defaultType));
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
			Assert.AreEqual(eyes.type, EyeType.CAT);
			Assert.AreEqual(eyes.leftIrisColor, eyes.rightIrisColor);
			Assert.AreEqual(EyeColor.BLUE, eyes.leftIrisColor);

			eyes = Eyes.GenerateWithHeterochromia(EyeType.DRAGON, EyeColor.BLUE, EyeColor.GREEN);
			Assert.AreEqual(eyes.type, EyeType.DRAGON);
			Assert.AreNotEqual(eyes.leftIrisColor, eyes.rightIrisColor);
			Assert.AreEqual(EyeColor.BLUE, eyes.leftIrisColor);
			Assert.AreEqual(EyeColor.GREEN, eyes.rightIrisColor);

			Assert.IsFalse(eyes.UpdateType(null));

			Assert.IsTrue(eyes.Restore());
			Assert.IsTrue(eyes.isDefault);
			Assert.IsFalse(eyes.UpdateType(Eyes.defaultType));

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
			Assert.Fail("Not implemented. should therefore fail");

			Antennae antennae = Antennae.GenerateDefault();
			Assert.IsTrue(antennae.isDefault);

			Assert.ThrowsException<ArgumentNullException>(() => antennae = Antennae.GenerateDefaultOfType(null));

			antennae = Antennae.GenerateDefaultOfType(AntennaeType.BEE);
			Assert.AreEqual(antennae.type, AntennaeType.BEE);

			Assert.IsFalse(antennae.UpdateType(null));

			Assert.IsTrue(antennae.Restore());
			Assert.IsTrue(antennae.isDefault);
			Assert.IsFalse(antennae.UpdateType(Antennae.defaultType));

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
			Assert.Fail("Not implemented. should therefore fail");

			Antennae antennae = Antennae.GenerateDefault();
			Assert.IsTrue(antennae.isDefault);

			Assert.ThrowsException<ArgumentNullException>(() => antennae = Antennae.GenerateDefaultOfType(null));

			antennae = Antennae.GenerateDefaultOfType(AntennaeType.BEE);
			Assert.AreEqual(antennae.type, AntennaeType.BEE);

			Assert.IsFalse(antennae.UpdateType(null));

			Assert.IsTrue(antennae.Restore());
			Assert.IsTrue(antennae.isDefault);
			Assert.IsFalse(antennae.UpdateType(Antennae.defaultType));

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
		public void Horn_GenerateUpdateRestore()
		{
			Assert.Fail("Not implemented. should therefore fail");

			Antennae antennae = Antennae.GenerateDefault();
			Assert.IsTrue(antennae.isDefault);

			Assert.ThrowsException<ArgumentNullException>(() => antennae = Antennae.GenerateDefaultOfType(null));

			antennae = Antennae.GenerateDefaultOfType(AntennaeType.BEE);
			Assert.AreEqual(antennae.type, AntennaeType.BEE);

			Assert.IsFalse(antennae.UpdateType(null));

			Assert.IsTrue(antennae.Restore());
			Assert.IsTrue(antennae.isDefault);
			Assert.IsFalse(antennae.UpdateType(Antennae.defaultType));

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
		public void LowerBody_GenerateUpdateRestore()
		{
			Assert.Fail("Not implemented. should therefore fail");

			Antennae antennae = Antennae.GenerateDefault();
			Assert.IsTrue(antennae.isDefault);

			Assert.ThrowsException<ArgumentNullException>(() => antennae = Antennae.GenerateDefaultOfType(null));

			antennae = Antennae.GenerateDefaultOfType(AntennaeType.BEE);
			Assert.AreEqual(antennae.type, AntennaeType.BEE);

			Assert.IsFalse(antennae.UpdateType(null));

			Assert.IsTrue(antennae.Restore());
			Assert.IsTrue(antennae.isDefault);
			Assert.IsFalse(antennae.UpdateType(Antennae.defaultType));

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
		public void Neck_GenerateUpdateRestore()
		{
			Assert.Fail("Not implemented. should therefore fail");

			Antennae antennae = Antennae.GenerateDefault();
			Assert.IsTrue(antennae.isDefault);

			Assert.ThrowsException<ArgumentNullException>(() => antennae = Antennae.GenerateDefaultOfType(null));

			antennae = Antennae.GenerateDefaultOfType(AntennaeType.BEE);
			Assert.AreEqual(antennae.type, AntennaeType.BEE);

			Assert.IsFalse(antennae.UpdateType(null));

			Assert.IsTrue(antennae.Restore());
			Assert.IsTrue(antennae.isDefault);
			Assert.IsFalse(antennae.UpdateType(Antennae.defaultType));

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
		public void Tail_GenerateUpdateRestore()
		{
			Assert.Fail("Not implemented. should therefore fail");

			Antennae antennae = Antennae.GenerateDefault();
			Assert.IsTrue(antennae.isDefault);

			Assert.ThrowsException<ArgumentNullException>(() => antennae = Antennae.GenerateDefaultOfType(null));

			antennae = Antennae.GenerateDefaultOfType(AntennaeType.BEE);
			Assert.AreEqual(antennae.type, AntennaeType.BEE);

			Assert.IsFalse(antennae.UpdateType(null));

			Assert.IsTrue(antennae.Restore());
			Assert.IsTrue(antennae.isDefault);
			Assert.IsFalse(antennae.UpdateType(Antennae.defaultType));

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
		public void Tongue_GenerateUpdateRestore()
		{
			Assert.Fail("Not implemented. should therefore fail");

			Antennae antennae = Antennae.GenerateDefault();
			Assert.IsTrue(antennae.isDefault);

			Assert.ThrowsException<ArgumentNullException>(() => antennae = Antennae.GenerateDefaultOfType(null));

			antennae = Antennae.GenerateDefaultOfType(AntennaeType.BEE);
			Assert.AreEqual(antennae.type, AntennaeType.BEE);

			Assert.IsFalse(antennae.UpdateType(null));

			Assert.IsTrue(antennae.Restore());
			Assert.IsTrue(antennae.isDefault);
			Assert.IsFalse(antennae.UpdateType(Antennae.defaultType));

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
		public void Vagina_GenerateUpdateRestore()
		{
			Assert.Fail("Not implemented. should therefore fail");

			Antennae antennae = Antennae.GenerateDefault();
			Assert.IsTrue(antennae.isDefault);

			Assert.ThrowsException<ArgumentNullException>(() => antennae = Antennae.GenerateDefaultOfType(null));

			antennae = Antennae.GenerateDefaultOfType(AntennaeType.BEE);
			Assert.AreEqual(antennae.type, AntennaeType.BEE);

			Assert.IsFalse(antennae.UpdateType(null));

			Assert.IsTrue(antennae.Restore());
			Assert.IsTrue(antennae.isDefault);
			Assert.IsFalse(antennae.UpdateType(Antennae.defaultType));

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
		public void Wing_GenerateUpdateRestore()
		{
			Assert.Fail("Not implemented. should therefore fail");

			Antennae antennae = Antennae.GenerateDefault();
			Assert.IsTrue(antennae.isDefault);

			Assert.ThrowsException<ArgumentNullException>(() => antennae = Antennae.GenerateDefaultOfType(null));

			antennae = Antennae.GenerateDefaultOfType(AntennaeType.BEE);
			Assert.AreEqual(antennae.type, AntennaeType.BEE);

			Assert.IsFalse(antennae.UpdateType(null));

			Assert.IsTrue(antennae.Restore());
			Assert.IsTrue(antennae.isDefault);
			Assert.IsFalse(antennae.UpdateType(Antennae.defaultType));

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
	}
}
