using CoC.Backend.BodyParts;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Save;
using CoC.Backend.Save.Internals;
using System.Runtime.Serialization;

namespace CoC.Backend.Creatures
{
	[DataContract]
	public abstract class Creature
	{

		public readonly Antennae antennae;
		public readonly Arms arms;
		public readonly Back back;
		public readonly Body body;
		public readonly Ears ears;
		public readonly Eyes eyes;
		public readonly Gills gills;
		public readonly Hair hair;
		public readonly Horns horns;
		public readonly Tongue tongue;
		public readonly Wings wings;
		//piercing extras

		protected Creature(CreatureCreator creator)
		{
			//Do parts that others depend on first, namely, body and genitals.
			//body. Needs to go before anything that uses hair, fur, or skin tone. 
			if (creator?.bodyType == null)
			{
				body = Body.GenerateDefault();
			}
			else if (creator.bodyType.isFurry)
			{
				//invalid primary color
				if (FurColor.isNullOrEmpty(creator.furColor))
				{
					body = Body.GenerateDefaultOfType(creator.bodyType);
					if (creator.furTexture != FurTexture.NONDESCRIPT)
					{
						body.ChangePrimaryFurTexture(creator.furTexture);
					}
				}
				//valid primary color from here out
				//no underbody
				else if (!creator.bodyType.hasUnderBody)
				{
					body = Body.GenerateFurredNoUnderbody((FurBodyType)creator.bodyType, creator.furColor, creator.furTexture);
				}
				//underbody from here out
				else
				{
					//check for invalid color.
					FurColor underColor = FurColor.isNullOrEmpty(creator.underFurColor) ? creator.furColor : creator.underFurColor;
					body = Body.GenerateFurredWithUnderbody((FurBodyType)creator.bodyType, creator.furColor, underColor, creator.furTexture, creator.underBodyFurTexture);
				}
			}
			else if (creator.bodyType.isTone)
			{
				if (Tones.isNullOrEmpty(creator.tone))
				{
					body = Body.GenerateDefaultOfType(creator.bodyType);
					if (creator.skinTexture != SkinTexture.NONDESCRIPT)
					{
						body.ChangePrimarySkinTexture(creator.skinTexture);
					}
				}
				else if (!creator.bodyType.hasUnderBody)
				{
					body = Body.GenerateTonedNoUnderbody((ToneBodyType)creator.bodyType, creator.tone, creator.skinTexture);
				}
				else
				{
					Tones underTone = Tones.isNullOrEmpty(creator.underTone) ? creator.tone : creator.underTone;
					body = Body.GenerateToneWithUnderbody((ToneBodyType)creator.bodyType, creator.tone, underTone, creator.skinTexture, creator.underBodySkinTexture);
				}
			}
			else if (creator.bodyType.isCockatrice)
			{
				if (Tones.isNullOrEmpty(creator.underTone) && FurColor.isNullOrEmpty(creator.furColor))
				{
					body = Body.GenerateDefaultOfType(creator.bodyType);
				}
				else
				{
					Tones scales = Tones.isNullOrEmpty(creator.underTone) ? BodyType.COCKATRICE.defaultScales : creator.underTone;
					FurColor feathers = FurColor.isNullOrEmpty(creator.furColor) ? BodyType.COCKATRICE.defaultFur : creator.furColor;

					body = Body.GenerateCockatrice(feathers, scales);
				}
			}
			else if (creator.bodyType.isKitsune)
			{
				if (FurColor.isNullOrEmpty(creator.underFurColor) && Tones.isNullOrEmpty(creator.tone))
				{
					body = Body.GenerateDefaultOfType(creator.bodyType);
				}
				else
				{
					Tones skin = Tones.isNullOrEmpty(creator.tone) ? BodyType.KITSUNE.defaultSkin : creator.tone;
					FurColor fur = FurColor.isNullOrEmpty(creator.underFurColor) ? BodyType.KITSUNE.defaultFur : creator.underFurColor;

					body = Body.GenerateKitsune(skin, fur);
				}
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
			else if (creator.backType == BackType.DRACONIC_MANE && !HairFurColors.isNullOrEmpty(creator.backHairFur))
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
				horns = Horns.GenerateOverride(creator.hornType, (int)creator.hornSize, (int)creator.hornCount);
			}
			else if (creator.additionalHornTransformStrength != 0)
			{
				horns = Horns.GenerateWithStrength(creator.hornType, creator.additionalHornTransformStrength, creator.forceUniformHornGrowthOnCreate);
			}
			else
			{
				horns = Horns.GenerateDefaultOfType(creator.hornType);
			}
			//horns.ReactToChangeInFemininity(genitals.feminity);
			//tongue
			tongue = creator?.tongueType == null ? Tongue.GenerateDefault() : Tongue.GenerateDefaultOfType(creator.tongueType);
			//wings
			if (creator?.wingType == null)
			{
				wings = Wings.GenerateDefault();
			}
			else if (creator.wingType is FeatheredWings && !HairFurColors.isNullOrEmpty(creator.wingFeatherColor))
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
			else if (creator.wingType is TonableWings && !Tones.isNullOrEmpty(creator.wingTone))
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
			SetupBindings();
		}

		internal Creature(SurrogateCreatureCreator surrogateCreator)
		{
			antennae = (Antennae)surrogateCreator?.antennae ?? Antennae.GenerateDefault();
			arms = surrogateCreator?.arms ?? Arms.GenerateDefault();
			back = surrogateCreator?.back ?? Back.GenerateDefault();
			body = surrogateCreator?.body ?? Body.GenerateDefault();
			ears = surrogateCreator?.ears ?? Ears.GenerateDefault();
			eyes = surrogateCreator?.eyes ?? Eyes.GenerateDefault();
			gills = surrogateCreator?.gills ?? Gills.GenerateDefault();
			hair = surrogateCreator?.hair ?? Hair.GenerateDefault();
			horns = surrogateCreator?.horns ?? Horns.GenerateDefault();
			tongue = surrogateCreator?.tongue ?? Tongue.GenerateDefault();
			wings = surrogateCreator?.wings ?? Wings.GenerateDefault();
			//Piercings
			//lip = surrogateCreator?.lip ?? Lip.Generate();
			//nose = surrogateCreator?.nose ?? Nose.Generate();
			//eyebrow = surrogateCreator?.eyebrow ?? Eyebrow.Generate();

			//piercingTest = surrogateCreator.piercingTest ?? new CockTest();


			SetupBindings();
		}

		private void SetupBindings()
		{

			((IBodyAware)arms).GetBodyData(body.ToBodyData);
			//((IBodyAware)ears).GetBodyData(body.ToBodyData);
			((IHairAware)body).GetHairData(hair.ToHairData);
		}



		public bool UpdateAntennae(AntennaeType antennaeType)
		{
			return antennae.UpdateAntennae(antennaeType);
		}

		public bool RestoreAntennae()
		{
			return antennae.Restore();
		}

		public bool UpdateArms(ArmType newType)
		{
			return arms.UpdateArms(newType);
		}

		public bool RestoreArms()
		{
			return arms.Restore();
		}

		public bool UpdateEyes(EyeType newType)
		{
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
			return tongue.UpdateTongue(newType);
		}

		public bool RestoreTongue()
		{
			return tongue.Restore();
		}
	}
}
