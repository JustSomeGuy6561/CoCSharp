//GooTransformations.cs
//Description:
//Author: JustSomeGuy
//1/24/2020 9:38:40 PM
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Creatures.PlayerData;
using CoC.Frontend.Perks.SpeciesPerks;
using CoC.Frontend.Races;
using CoC.Frontend.Settings.Gameplay;
using CoC.Frontend.StatusEffect;

namespace CoC.Frontend.Transformations
{
	internal abstract class GooTransformations : GenericTransformationBase
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

			//NOTE: THIS DOESN'T CALL GENERIC CHANGE COUNT, IDK.

			//libido up to 80
			if (target.relativeLibido < 80)
			{
				target.DeltaCreatureStats(lib: .5f + (90 - target.libido) / 10, lus: target.libido / 2);
			}
			//sensitivity moves towards 50
			if (target.relativeSensitivity < 50)
			{
				target.ChangeSensitivity(1);
			}
			else if (target.relativeSensitivity > 50)
			{
				target.ChangeSensitivity(-1);
			}

			//Cosmetic changes based on 'goopyness'
			//Neck restore
			if (target.neck.type != NeckType.HUMANOID && Utils.Rand(4) == 0)
			{
				target.RestoreNeck();
			}
			//Rear body restore
			if (!target.back.isDefault && Utils.Rand(5) == 0)
			{
				target.RestoreBack();
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
			//Remove wings and shark fin
			if (target.wings.type != WingType.NONE || target.back.type == BackType.SHARK_FIN)
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

			if (target.hair.type != HairType.GOO)
			{

				target.UpdateHair(HairType.GOO);
				//if bald
				if (target.hair.length <= 0)
				{
					target.hair.SetHairLength(5);
				}

				if (!Species.GOO.availableHairColors.Contains(target.hair.hairColor))
				{
					target.hair.SetHairColor(Species.GOO.GetRandomHairColor(), true);
				}
				target.ChangeLust(10);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//1.Goopy skin
			if (target.hair.type == HairType.GOO && target.body.type != BodyType.GOO)
			{
				if (target.arms.type != ArmType.GOO)
				{
					target.UpdateArms(ArmType.GOO);
				}

				if (!Species.GOO.availableTones.Contains(target.body.primarySkin.tone))
				{
					target.UpdateBody(BodyType.GOO, Utils.RandomChoice(Species.GOO.availableTones));
				}
				else
				{
					target.UpdateBody(BodyType.GOO);
				}

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			////1a.Make alterations to dick/vaginal/nippular descriptors to match
			//DONE EXCEPT FOR TITS & MULTIDICKS (UNFINISHED KINDA)
			//2.Goo legs
			if (target.body.type == BodyType.GOO && target.lowerBody.type != LowerBodyType.GOO)
			{
				target.build.DecreaseHeight((byte)(3 + Utils.Rand(2)));
				if (target.build.heightInInches < 36)
				{
					target.build.SetHeight(36);
				}
				LowerBodyData oldData = target.lowerBody.AsReadOnlyData();
				target.UpdateLowerBody(LowerBodyType.GOO);
				sb.Append(UpdateLowerBodyText(target, oldData));

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//3a. Grow vagina if none
			if (!target.hasVagina)
			{
				target.genitals.AddVagina(VaginaType.defaultValue, 0.4f, VaginalLooseness.GAPING, VaginalWetness.DROOLING);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//3b.Infinite Vagina
			else if (!target.HasPerk<ElasticInnards>())
			{
				target.AddPerk<ElasticInnards>();
			}
			else if (target.build.heightInInches < 100 && Utils.Rand(3) <= 1)
			{
				target.build.IncreaseHeight(2);
				target.DeltaCreatureStats(str: 1, tou: 1);
			}
			//Big slime girl
			else if (!target.HasPerk<SlimeCraving>())
			{
				target.AddPerk<SlimeCraving>();
			}


			//this is the fallthrough that occurs when a tf item goes through all the changes, but does not proc enough of them to exit early. it will apply however many changes
			//occurred, then return the contents of the stringbuilder.
			return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
		}

		protected virtual string ClearOvipositionText(Creature target)
{
return RemovedOvipositionTextGeneric(target);
}

		protected virtual string UpdateLowerBodyText(Creature target, LowerBodyData oldData)
		{
			return target.lowerBody.TransformFromText(oldData);
		}

		protected virtual string RestoredWingsText(Creature target, WingData oldData)
		{
			return target.wings.RestoredText(oldData);
		}


		//the abstract string calls that you create above should be declared here. they should be protected. if it is a body part change or a generic text that has already been
		//defined by the base class, feel free to make it virtual instead.
		protected abstract string InitialTransformationText(Creature target);
	}
}