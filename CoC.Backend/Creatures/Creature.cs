using CoC.Backend.BodyParts;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;

namespace CoC.Backend.Creatures
{

	public abstract class Creature
	{
		public readonly string name;

		public readonly Antennae antennae;
		public readonly Arms arms;
		public readonly Ass ass;
		public readonly Back back;
		public readonly Body body;
		public readonly Ears ears;
		public readonly Eyes eyes;
		public readonly Face face;
		public readonly Frame frame;
		public readonly Genitals genitals;
		public readonly Gills gills;
		public readonly Hair hair;
		public readonly Hips hips;
		public readonly Horns horns;
		public readonly LowerBody lowerBody;
		public readonly Neck neck;
		public readonly Tail tail;
		public readonly Tongue tongue;
		public readonly Wings wings;
		public readonly Womb womb;

		//public readonly Beard beard;

		protected Creature(CreatureCreator creator)
		{
			name = creator?.name ?? "Unknown";
			//semantically, we Should do the things other parts can depend on first, but as long as we
			//dont actually require the data in the generate functions (which we generally shouldn't - that's why we're lazy)
			//it won't matter. Anything that needs this stuff for validation 


			//body
			if (creator?.bodyType == null)
			{
				body = Body.GenerateDefault();
			}
			else if (creator.bodyType is SimpleToneBodyType simpleToneBodyType)
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

			//antennae
			antennae = creator?.antennaeType != null ? Antennae.GenerateDefaultOfType(creator.antennaeType) : Antennae.GenerateDefault();
			//arms
			arms = creator?.armType != null ? Arms.GenerateDefaultOfType(creator.armType) : Arms.GenerateDefault();
			//back
			if (creator?.backType == null)
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
			ears = creator?.earType != null ? Ears.GenerateDefaultOfType(creator.earType) : Ears.GenerateDefault();
			//eyes
			if (creator?.eyeType == null)
			{
				eyes = Eyes.GenerateDefault();
			}
			else if (creator.leftEyeColor == null && creator.rightEyeColor == null)
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
			//gills
			gills = creator?.gillType != null ? Gills.GenerateDefaultOfType(creator.gillType) : Gills.GenerateDefault();

			//if (creator?.hairType == null)
			//{
			//	hair = Hair.GenerateDefault();
			//}
			//else if (creator.hairColor == null)
			//{
			//	if (creator.hairLength == null)
			//	{
			//		hair = Hair.GenerateDefaultOfType(creator.hairType);
			//	}
			//	else
			//	{
			//		hair = Hair.GenerateWithLength(creator.hairType, (float)creator.hairLength);
			//	}
			//}
			//else if (creator.hairHighlightColor == null)
			//{
			//	hair = Hair.GenerateWithColor(creator.hairType, creator.hairColor, creator.hairLength);
			//}
			//else
			//{
			//	hair = Hair.GenerateWithColorAndHighlight(creator.hairType, creator.hairColor, creator.hairHighlightColor, creator.hairLength);
			//}
			hair = Hair.GenerateDefault();

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
			//horns.ReactToChangeInFemininity(genitals.femininity);
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
			else if (creator.wingType is TonableWings && !Tones.IsNullOrEmpty(creator.wingTone))
			{
				if (creator.largeWings == null)
				{
					wings = Wings.GenerateColored((TonableWings)creator.wingType, creator.wingTone);
				}
				else
				{
					wings = Wings.GenerateColoredWithSize((TonableWings)creator.wingType, creator.wingTone, (bool)creator.largeWings);
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

			//genitals.InitializePiercings(creator?.nipplePiercings, creator?.clitPiercings, creator?.labiaPiercings, creator?.cockPiercings);
			//tail.InitializePiercings(creator?.tailPiercings);

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

			((IHairAware)body).GetHairData(hair.ToHairData);

			((IBodyAware)arms).GetBodyData(body.ToBodyData);
			((IBodyAware)ears).GetBodyData(body.ToBodyData);


		}



		public bool UpdateAntennae(AntennaeType antennaeType)
		{
			if (antennaeType == null) throw new System.ArgumentNullException();
			return antennae.UpdateAntennae(antennaeType);
		}

		public bool RestoreAntennae()
		{
			return antennae.Restore();
		}

		public bool UpdateArms(ArmType newType)
		{
			if (newType == null) throw new System.ArgumentNullException();
			return arms.UpdateArms(newType);
		}

		public bool RestoreArms()
		{
			return arms.Restore();
		}

		public bool UpdateEyes(EyeType newType)
		{
			if (newType == null) throw new System.ArgumentNullException();
			return eyes.UpdateEyeType(newType);
		}

		public bool RestoreEyes()
		{
			return eyes.Restore();
		}

		public void ResetEyes()
		{
			eyes.Reset();
		}

		public bool UpdateTongue(TongueType newType)
		{
			if (newType == null) throw new System.ArgumentNullException();
			return tongue.UpdateTongue(newType);
		}

		public bool RestoreTongue()
		{
			return tongue.Restore();
		}
	}
}
