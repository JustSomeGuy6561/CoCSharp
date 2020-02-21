//BunnyTransformations.cs
//Description:
//Author: JustSomeGuy
//1/24/2020 9:53:24 PM
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Creatures.PlayerData;
using CoC.Frontend.Races;
using CoC.Frontend.Settings.Gameplay;

namespace CoC.Frontend.Transformations
{
	internal abstract class BunnyTransformations : GenericTransformationBase
	{
		//a helper that gets the currently set hyper happy flag for this game session. generally useful, but feel free to remove this if you don't need it.
		private bool hyperHappy => HyperHappySettings.isEnabled;


		protected internal override string DoTransformation(Creature target, out bool isBadEnd)
		{
			isBadEnd = false;

			//by default, this is 2 rolls at 50%, so a 25% chance of 0 additional tfs, 50% chance of 1 additional tf, 25% chance of 2 additional tfs.
			//also takes into consideration any perks that increase or decrease tf effectiveness. if you need to roll out your own, feel free to do so.
			int changeCount = GenerateChangeCount(target, new int[] { 2, 2 });
			int remainingChanges = changeCount;

			StringBuilder sb = new StringBuilder();

			//For all of these, any text regarding the transformation should be instead abstracted out as an abstract string function. append the result of this abstract function
			//to the string builder declared above (aka sb.Append(FunctionCall(variables));) string builder is just a fancy way of telling the compiler that you'll be creating a
			//long string, piece by piece, so don't do any crazy optimizations first.

			//the initial text for starting the transformation. feel free to add additional variables to this if needed.
			sb.Append(InitialTransformationText(target));

			//Add any free changes here - these can occur even if the change count is 0. these include things such as change in stats (intelligence, etc)
			//change in height, hips, and/or butt, or other similar stats.


			//STATS CHANGURYUUUUU
			//Boost speed (max 80!)
			if (Utils.Rand(3) == 0 && target.relativeSpeed < 80)
			{
				target.DeltaCreatureStats(spe: target.relativeSpeed < 35 ? 2 : 1);
			}
			//Boost libido
			if (Utils.Rand(5) == 0)
			{
				target.DeltaCreatureStats(lib: 1, lus: (5 + target.libido / 7));
				if (target.relativeLibido < 30)
				{
					target.ChangeLibido(1);
				}

				if (target.relativeLibido < 40)
				{
					target.ChangeLibido(1);
				}

				if (target.relativeLibido < 60)
				{
					target.ChangeLibido(1);
				}
				//Lower ones are gender specific for some reason
			}

			//BIG sensitivity gains to 60.
			if (target.relativeSensitivity < 60 && Utils.Rand(3) == 0)
			{
				//(low)
				if (Utils.Rand(3) != 2)
				{
					target.ChangeSensitivity(5);
				}
				//(BIG boost 1/3 chance)
				else
				{
					target.ChangeSensitivity(15);

				}
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


			//Makes girls very girl(90), guys somewhat girly (61).
			if (Utils.Rand(2) == 0)
			{
				short res = target.femininity.ChangeFemininityToward((byte)(target.gender.HasFlag(Gender.FEMALE) ? 90 : 61), 4);
				if (res != 0)
				{
					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
			}

			//De-wettification of cunt (down to 3?)!
			VaginalWetness targetWetness = EnumHelper.Max(target.genitals.minVaginalWetness, VaginalWetness.SLICK);
			if (target.hasVagina && target.genitals.SmallestVaginalWetness() > targetWetness && Utils.Rand(3) == 0)
			{
				//Just to be safe
				foreach (Vagina vag in target.vaginas)
				{
					vag.DecreaseWetness();
				}

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Fertility boost!
			if (Utils.Rand(4) == 0 && target.fertility.totalFertility < 50 && target.hasVagina)
			{
				target.fertility.IncreaseFertility((byte)(2 + Utils.Rand(5)));
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//-VAGs
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
			if (!target.back.isDefault && Utils.Rand(5) == 0)
			{
				BackData oldData = target.back.AsReadOnlyData();
				target.RestoreBack();
				sb.Append(RestoredBackText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//MOD: Bunny eggs and oviposition are now one and the same. the idea of removing oviposition and adding bunny eggs - that's just dumb.
			if (target.womb.canObtainOviposition && Utils.Rand(4) == 0)
			{
				target.womb.GrantOviposition();
				sb.Append(GrantOvipositionText(target));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Shrink Balls!
			if (target.balls.count > 0 && target.balls.size > 5 && Utils.Rand(3) == 0)
			{
				if (target.balls.size < 10)
				{
					target.balls.ShrinkBalls();
				}
				else if (target.balls.size < 25)
				{
					target.balls.ShrinkBalls((byte)(2 + Utils.Rand(3)));
				}
				else
				{
					target.balls.ShrinkBalls((byte)(6 + Utils.Rand(3)));
				}
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Get rid of extra balls
			if (target.balls.count > 2 && Utils.Rand(3) == 0)
			{
				target.balls.RemoveExtraBalls();

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Boost cum production
			if ((target.balls.count > 0 || target.hasCock) && target.genitals.totalCum < 3000 && Utils.Rand(3) == 0)
			{
				target.genitals.IncreaseCumMultiplier(3 + Utils.Rand(7));

				if (target.genitals.totalCum >= 250)
				{
					target.ChangeLust(3);
				}
				else if (target.genitals.totalCum >= 750)
				{
					target.ChangeLust(4);
				}
				else if (target.genitals.totalCum >= 2000)
				{
					target.ChangeLust(5);
				}

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Bunny feet! - requirez earz
			if (target.lowerBody.type != LowerBodyType.BUNNY && Utils.Rand(5) == 0 && target.ears.type == EarType.BUNNY)
			{
				LowerBodyData oldData = target.lowerBody.AsReadOnlyData();
				target.UpdateLowerBody(LowerBodyType.BUNNY);
				sb.Append(UpdateLowerBodyText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//BUN FACE! REQUIREZ EARZ
			if (target.ears.type == EarType.BUNNY && target.face.type != FaceType.BUNNY && Utils.Rand(3) == 0)
			{
				FaceData oldData = target.face.AsReadOnlyData();
				target.UpdateFace(FaceType.BUNNY);
				sb.Append(UpdateFaceText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			else if (target.ears.type == EarType.BUNNY && target.face.type == FaceType.BUNNY && !target.face.isFullMorph && Utils.Rand(3) == 0)
			{
				target.face.StrengthenFacialMorph();

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//DAH BUNBUN EARZ - requires poofbutt!
			if (target.ears.type != EarType.BUNNY && Utils.Rand(3) == 0 && target.tail.type == TailType.RABBIT)
			{
				EarData oldData = target.ears.AsReadOnlyData();
				target.UpdateEars(EarType.BUNNY);
				sb.Append(UpdateEarsText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//DAH BUNBUNTAILZ
			if (target.tail.type != TailType.RABBIT && Utils.Rand(2) == 0)
			{
				TailData oldData = target.tail.AsReadOnlyData();
				target.UpdateTail(TailType.RABBIT);
				sb.Append(UpdateTailText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			// Remove gills
			if (Utils.Rand(4) == 0 && !target.gills.isDefault)
			{
				target.RestoreGills();
			}
			// Remove antennae
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
			// Remove wings
			if ((target.wings.type != WingType.NONE || target.back.type == BackType.SHARK_FIN) && Utils.Rand(4) == 0)
			{
				target.RestoreWings();
				BackData oldData = target.back.AsReadOnlyData();
				target.RestoreBack();
				sb.Append(RestoredBackText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//Bunny Breeder Perk?
			//FAILSAAAAFE
			if (remainingChanges == changeCount)
			{
				if (target.relativeLibido < 100)
				{
					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}

				target.DeltaCreatureStats(lib: 1, lus: (5 + target.libido / 7));
				if (target.relativeLibido < 30)
				{
					target.ChangeLibido(1);
				}

				if (target.relativeLibido < 40)
				{
					target.ChangeLibido(1);
				}

				if (target.relativeLibido < 60)
				{
					target.ChangeLibido(1);
				}
			}


			//this is the fallthrough that occurs when a tf item goes through all the changes, but does not proc enough of them to exit early. it will apply however many changes
			//occurred, then return the contents of the stringbuilder.
			return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
		}

		protected virtual string GrantOvipositionText(Creature target)
		{
			return GainedOvipositionTextGeneric(target);
		}

		protected virtual string UpdateEarsText(Creature target, EarData oldData)
		{
			return target.ears.TransformFromText(oldData);
		}
		protected virtual string UpdateLowerBodyText(Creature target, LowerBodyData oldData)
		{
			return target.lowerBody.TransformFromText(oldData);
		}

		protected virtual string UpdateFaceText(Creature target, FaceData oldData)
		{
			return target.face.TransformFromText(oldData);
		}

		protected virtual string UpdateTailText(Creature target, TailData oldTail)
		{
			return target.tail.TransformFromText(oldTail);
		}
		protected virtual string RestoredNeckText(Creature target, NeckData oldData)
		{
			return target.neck.RestoredText(oldData);
		}

		protected virtual string RestoredAntennaeText(Creature target, AntennaeData oldData)
		{
			return target.antennae.RestoredText(oldData);
		}

		protected virtual string RestoredBackText(Creature target, BackData oldData)
		{
			return target.back.RestoredText(oldData);
		}


		//the abstract string calls that you create above should be declared here. they should be protected. if it is a body part change or a generic text that has already been
		//defined by the base class, feel free to make it virtual instead.
		protected abstract string InitialTransformationText(Creature target);
	}
}