//NagaTransformations.cs
//Description:
//Author: JustSomeGuy
//2/7/2020 9:31:55 AM
using System.Linq;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Creatures.PlayerData;
using CoC.Frontend.Races;
using CoC.Frontend.Settings.Gameplay;

namespace CoC.Frontend.Transformations
{
	//OG NOTE:
	/*Effects:
	 Boosts Speed stat
	 Ass reduction
	 Testicles return inside your body (could be reverted by the use of succubi delight)
	 Can change penis into reptilian form (since there's a lot of commentary here not knowing where to go, let me lay it out.)
	 the change will select one cock (Utils.Randomly if you have multiple)
	 said cock will become two reptilian cocks
	 these can then be affected separately, so if someone wants to go through the effort of removing one and leaving themselves with one reptile penis, they have the ability to do that
	 This also means that someone who's already reached the maximum numbers of dicks cannot get a reptilian penis unless they remove one first
	 "Your reptilian penis is X.X inches long and X.X inches thick. The sheath extends halfway up the shaft, thick and veiny, while the smooth shaft extends out of the sheath coming to a pointed tip at the head. "
	 Grow poisonous fangs (grants Poison Bite ability to target, incompatible with the sting ability, as it uses the same poison-meter)
	 Causes your tongue to fork
	 Legs fuse together and dissolve into snake tail (grants Constrict ability to target, said tail can only be covered in scales, independently from the rest of the body)
	 If snake tail exists:
		Make it longer, possibly larger (tail length is considered independently of your height, so it doesn't enable you to use the axe, for instance.
		Change tail's color according to location
		 [Smooth] Beige and Tan (Desert), [Rough] Brown and Rust (Mountains), [Lush] Forest Green and Yellow (Forest), [Cold] Blue and White (ice land?), [Fresh] Meadow Green [#57D53B - #7FFF00] and Dark Teal [#008080] (lake) , [Menacing] Black and Red (Demon realm, outside encounters), [Distinguished] Ivory (#FFFFF0) and Royal Purple/Amethyst (#702963) (Factory), [Mossy] Emerald and Chestnut (Swamp), [Arid] Orange and Olive pattern (Tel' Adre)

	 9a) Item Description
	 "A vial the size of your fist made of dark brown glass. It contains what appears to be an oily, yellowish liquid. The odor is abominable."
	 */
	//MOD NOTE: At some point, much of this was adapted or removed. i'm going to add a few things that are new to this version (notably, reptilian body), but that's about it.
	//sorry whoever wrote that og design doc - that's some work. Note that this will have text unique to in combat or not, which makes it different from nearly everything else and
	//forced me to handle that nearly everywhere (not a fan, btw. but necessary, so i guess i'm not mad).

	internal abstract class NagaTransformations : GenericTransformationBase
	{
		//a helper that gets the currently set hyper happy flag for this game session. generally useful, but feel free to remove this if you don't need it.
		private bool hyperHappy => HyperHappySettings.isEnabled;


		protected internal override string DoTransformation(Creature target, out bool isBadEnd)
		{
			return DoTransformationCommon(target, false, out isBadEnd);
		}

		protected internal override string DoTransformationFromCombat(CombatCreature target, CombatCreature opponent, out bool isBadEnd)
		{
			return DoTransformationCommon(target, true, out isBadEnd);
		}

		private string DoTransformationCommon(Creature target, bool currentlyInCombat, out bool isBadEnd)
		{
			isBadEnd = false;

			//by default, this is 2 rolls at 50%, so a 25% chance of 0 additional tfs, 50% chance of 1 additional tf, 25% chance of 2 additional tfs.
			//also takes into consideration any perks that increase or decrease tf effectiveness. if you need to roll out your own, feel free to do so.
			int changeCount = GenerateChangeCount(target, new int[] { 2, 2 });
			int remainingChanges = changeCount;

			bool statsChanged = false;

			StringBuilder sb = new StringBuilder();

			//For all of these, any text regarding the transformation should be instead abstracted out as an abstract string function. append the result of this abstract function
			//to the string builder declared above (aka sb.Append(FunctionCall(variables));) string builder is just a fancy way of telling the compiler that you'll be creating a
			//long string, piece by piece, so don't do any crazy optimizations first.

			//the initial text for starting the transformation. feel free to add additional variables to this if needed.
			sb.Append(InitialTransformationText(target, currentlyInCombat));

			//Add any free changes here - these can occur even if the change count is 0. these include things such as change in stats (intelligence, etc)
			//change in height, hips, and/or butt, or other similar stats.

			if (target.relativeSpeed < 70 && Utils.Rand(2) == 0)
			{
				target.IncreaseSpeed(2 - (target.relativeSpeed / 50));
				statsChanged = true;
			}

			//this will handle the edge case where the change count starts out as 0.
			if (remainingChanges <= 0)
			{
				return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			//Any transformation related changes go here. these typically cost 1 change. these can be anything from body parts to gender (which technically also changes body parts,
			//but w/e). You are required to make sure you return as soon as you've applied changeCount changes, but a single line of code can be applied at the end of a change to do
			//this for you.

			//paste this line after any tf is applied, and it will: automatically decrement the remaining changes count. if it becomes 0 or less, apply the total number of changes
			//underwent to the target's change count (if applicable) and then return the StringBuilder content.
			//if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);



			//Neck restore
			if (target.neck.type != NeckType.HUMANOID && Utils.Rand(4) == 0)
			{
				NeckData oldData = target.neck.AsReadOnlyData();
				target.RestoreNeck();
				sb.Append(RestoredNeckText(target, oldData));
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Rear body restore
			if (target.back.type != BackType.SHARK_FIN && !target.back.isDefault && Utils.Rand(5) == 0)
			{
				BackData oldData = target.back.AsReadOnlyData();
				target.RestoreBack();
				sb.Append(RestoredBackText(target, oldData));
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Ovi perk loss
			if (target.womb.canRemoveOviposition && Utils.Rand(5) == 0)
			{
				target.womb.ClearOviposition();
				sb.Append(ClearOvipositionText(target));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//Removes wings and shark fin
			if ((target.wings.type != WingType.NONE || target.back.type == BackType.SHARK_FIN) && Utils.Rand(3) == 0)
			{
				if (target.back.type == BackType.SHARK_FIN)
				{
					target.RestoreBack();
				}
				WingData oldData = target.wings.AsReadOnlyData();
				target.RestoreWings();
				sb.Append(RestoredWingsText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Removes antennae
			if (target.antennae.type != AntennaeType.NONE && Utils.Rand(3) == 0)
			{
				AntennaeData oldData = target.antennae.AsReadOnlyData();
				target.RestoreAntennae();
				sb.Append(RestoredAntennaeText(target, oldData));
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//Sexual changes

			//-Remove extra breast rows
			if (target.breasts.Count > 1 && Utils.Rand(3) == 0 && !HyperHappySettings.isEnabled)
			{
				target.genitals.RemoveExtraBreastRows();

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//adjust cocks until the target has 2 reptilian cocks. only occurs for males or herms, or genderless if the hyper happy flag is off and a 1/2 roll procs true.
			if ((target.gender.HasFlag(Gender.MALE) || (target.gender == Gender.GENDERLESS && !HyperHappySettings.isEnabled && Utils.Rand(2) == 0))
				&& (target.cocks.Count != 2 || target.genitals.CountCocksOfType(CockType.LIZARD) != 2) && Utils.Rand(3) == 0)
			{
				int lizardCocks = target.genitals.CountCocksOfType(CockType.LIZARD);
				//grant up to 2 lizard cocks.
				if (lizardCocks < 2)
				{
					//if we have two or less cocks, convert all that we do have to lizard.
					if (target.cocks.Count < 3)
					{
						foreach (Cock cock in target.cocks)
						{
							target.genitals.UpdateCock(cock, CockType.LIZARD);
						}

						//then, add cock(s) until we have 2 lizard cocks
						while (target.cocks.Count < 2)
						{
							target.AddCock(CockType.LIZARD);
						}
					}
					else if (lizardCocks == 1)
					{
						int toChange = target.cocks[0].type == CockType.LIZARD ? 0 : 1;
						target.genitals.UpdateCock(toChange, CockType.LIZARD);
					}
				}
				//otherwise, we already have 2 lizard cocks (or more). we're only going to keep 2 lizard cocks, so we need to remove extra ones.
				else
				{
					//any non-lizard cock gets removed first.
					Cock toRemove = target.cocks.FirstOrDefault(x => x.type != CockType.LIZARD);
					//if we can't find one, it means we only have lizard cocks. remove the last one.
					if (toRemove is null)
					{
						toRemove = target.cocks[target.cocks.Count - 1];
					}

				}
			}

			//9c) II The tongue (sensitivity bonus, stored as a perk?)
			if (changeCount == remainingChanges && Utils.Rand(3) == 0)
			{
				TongueData oldData = target.tongue.AsReadOnlyData();
				target.UpdateTongue(TongueType.SNAKE);
				sb.Append(UpdateTongueText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//9c) III The fangs
			if (changeCount == remainingChanges && target.tongue.type == TongueType.SNAKE && target.face.type != FaceType.SNAKE && Utils.Rand(3) == 0)
			{
				FaceData oldData = target.face.AsReadOnlyData();
				target.UpdateFace(FaceType.SNAKE);
				sb.Append(UpdateFaceText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//9c) I The tail ( http://tvtropes.org/pmwiki/pmwiki.php/Main/TransformationIsAFreeAction ) (Shouldn't we try to avert this? -Ace)
			//Should the enemy "kill" you during the transformation, it skips the scene and immediately goes to tthe rape scene.
			//(Now that I'm thinking about it, we should add some sort of appendix where the target realizes how much he's/she's changed. -Ace)
			//MOD NOTE: this also silently grants a reptilian body. there's also a check for this if you somehow lose the reptilian body and have a naga lower body later.
			if (changeCount == remainingChanges && target.face.type == FaceType.SNAKE && target.lowerBody.type != LowerBodyType.NAGA && Utils.Rand(4) == 0)
			{
				target.UpdateLowerBody(LowerBodyType.NAGA);

				// Naga lower body plus a tail may look awkward, so silently discard it (Stadler76)
				target.RestoreTail();
				//convert body to match lower body.
				if (target.body.type != BodyType.REPTILE)
				{
					target.UpdateBody(BodyType.REPTILE, Tones.GREEN, Tones.LIGHT_GREEN);
				}

				if (--remainingChanges <= 0 || currentlyInCombat)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			if (target.body.type != BodyType.REPTILE && target.lowerBody.type == LowerBodyType.NAGA)
			{
				BodyData oldData = target.body.AsReadOnlyData();
				target.UpdateBody(BodyType.REPTILE, Tones.GREEN, Tones.LIGHT_GREEN);
				sb.Append(UpdateBodyText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			// Remove gills
			if (Utils.Rand(4) == 0 && !target.gills.isDefault)
			{
				GillData oldData = target.gills.AsReadOnlyData();
				target.RestoreGills();
				sb.Append(RestoredGillsText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//Default change - blah
			if (changeCount == remainingChanges && !statsChanged)
			{
			}

			//this is the fallthrough that occurs when a tf item goes through all the changes, but does not proc enough of them to exit early. it will apply however many changes
			//occurred, then return the contents of the stringbuilder.
			return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
		}

		protected virtual string ClearOvipositionText(Creature target)
{
return RemovedOvipositionTextGeneric(target);
}

		protected virtual string UpdateTongueText(Creature target, TongueData oldData)
		{
			return target.tongue.TransformFromText(oldData);
		}

		protected virtual string UpdateFaceText(Creature target, FaceData oldData)
		{
			return target.face.TransformFromText(oldData);
		}

		protected virtual string UpdateBodyText(Creature target, BodyData oldData)
		{
			return target.body.TransformFromText(oldData);
		}

		protected virtual string RestoredNeckText(Creature target, NeckData oldData)
		{
			return target.neck.RestoredText(oldData);
		}

		protected virtual string RestoredBackText(Creature target, BackData oldData)
		{
			return target.back.RestoredText(oldData);
		}

		protected virtual string RestoredWingsText(Creature target, WingData oldData)
		{
			return target.wings.RestoredText(oldData);
		}

		protected virtual string RestoredAntennaeText(Creature target, AntennaeData oldData)
		{
			return target.antennae.RestoredText(oldData);
		}

		protected virtual string RestoredGillsText(Creature target, GillData oldData)
		{
			return target.gills.RestoredText(oldData);
		}



		//the abstract string calls that you create above should be declared here. they should be protected. if it is a body part change or a generic text that has already been
		//defined by the base class, feel free to make it virtual instead.
		protected abstract string InitialTransformationText(Creature target, bool currentlyInCombat);
	}
}