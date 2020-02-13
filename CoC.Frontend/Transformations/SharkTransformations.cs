//SharkTransformations.cs
//Description:
//Author: JustSomeGuy
//1/24/2020 9:46:17 PM
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
	internal abstract class SharkTransformations : GenericTransformationBase
	{
		protected readonly bool isTigerShark;

		protected SharkTransformations(bool tigerShark)
		{
			isTigerShark = tigerShark;
		}

		//a helper that gets the currently set hyper happy flag for this game session. generally useful, but feel free to remove this if you don't need it.
		private bool hyperHappy => HyperHappySettings.isEnabled;


		protected internal override string DoTransformation(Creature target, out bool isBadEnd)
		{
			isBadEnd = false;

			//by default, this is 2 rolls at 50%, so a 25% chance of 0 additional tfs, 50% chance of 1 additional tf, 25% chance of 2 additional tfs.
			//also takes into consideration any perks that increase or decrease tf effectiveness. if you need to roll out your own, feel free to do so.
			int changeCount = GenerateChangeCount(target, new int[] { 2, 2 }, 2);
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


			//STATS
			if (target.relativeSensitivity > 25 && Utils.Rand(3) < 2)
			{
				target.DeltaCreatureStats(sens: (-1 - Utils.Rand(3)));
			}
			if (target is CombatCreature cc)
			{
				//Increase strength 1-2 points (Up to 50) (60 for tiger)
				if (((cc.relativeStrength < 60 && isTigerShark) || cc.relativeStrength < 50) && Utils.Rand(3) == 0)
				{
					cc.DeltaCombatCreatureStats(str: 1 + Utils.Rand(2));
				}
				//Increase Speed 1-3 points (Up to 75) (100 for tigers)
				if (((cc.relativeSpeed < 100 && isTigerShark) || cc.relativeSpeed < 75) && Utils.Rand(3) == 0)
				{
					cc.DeltaCombatCreatureStats(spe: 1 + Utils.Rand(3));
				}
				//Reduce sensitivity 1-3 Points (Down to 25 points)

				//Increase Libido 2-4 points (Up to 75 points) (100 for tigers)
				if (((cc.relativeLibido < 100 && isTigerShark) || cc.relativeLibido < 75) && Utils.Rand(3) == 0)
				{
					cc.DeltaCreatureStats(lib: (1 + Utils.Rand(3)));
				}
				//Decrease intellect 1-3 points (Down to 40 points)
				if (cc.relativeIntelligence > 40 && Utils.Rand(3) == 0)
				{
					cc.DeltaCombatCreatureStats(inte: -(1 + Utils.Rand(3)));
				}
			}
			//Smexual stuff!
			//-TIGGERSHARK ONLY: Grow a cunt (guaranteed if no gender)
			if (isTigerShark && (target.gender == 0 || (!target.hasVagina && Utils.Rand(3) == 0)))
			{
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
				//(balls)
				//(dick)
				//(neither)
				target.genitals.AddVagina();
				target.DeltaCreatureStats(sens: 10);
			}
			//WANG GROWTH - TIGGERSHARK ONLY
			if (isTigerShark && (!target.hasCock) && Utils.Rand(3) == 0)
			{
				//Genderless:
				//Female:
				if (target.balls.count == 0)
				{
					target.balls.GrowBalls();
				}
				target.genitals.AddCock(CockType.defaultValue, 7, 1.4f);

				target.DeltaCreatureStats(lib: 4, sens: 5, lus: 20);
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//(Requires the target having two testicles)
			if (isTigerShark && (target.balls.count == 0 || target.balls.count == 2) && target.hasCock && Utils.Rand(3) == 0)
			{
				if (target.balls.count == 2)
				{
					target.balls.AddBalls(2);
				}
				else if (target.balls.count == 0)
				{
					target.balls.GrowBalls();
				}
				target.DeltaCreatureStats(lib: 2, sens: 3, lus: 10);
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//-Remove extra breast rows
			if (target.breasts.Count > 1 && Utils.Rand(3) == 0 && !hyperHappy)
			{
				target.RemoveExtraBreastRows();
			}
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

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Transformations:
			//Mouth TF
			if (target.face.type != FaceType.SHARK && Utils.Rand(3) == 0)
			{
				target.UpdateFace(FaceType.SHARK);
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Remove odd eyes
			if (Utils.Rand(5) == 0 && target.eyes.type != EyeType.HUMAN)
			{
				target.RestoreEyes();

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Tail TF
			if (target.tail.type != TailType.SHARK && Utils.Rand(3) == 0)
			{
				target.UpdateTail(TailType.SHARK);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Gills TF
			if (target.gills.type != GillType.FISH && target.tail.type == TailType.SHARK && target.face.type == FaceType.SHARK && Utils.Rand(3) == 0)
			{
				target.UpdateGills(GillType.FISH);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Hair
			if (target.hair.hairColor != HairFurColors.SILVER && Utils.Rand(4) == 0)
			{
				target.hair.SetHairColor(HairFurColors.SILVER);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Skin
			//MOD NOTE: FUCK. A TWO TONED SKIN TONE. FUCK FUCK FUCK.
			//MOD NOTE (continued): tigershark skin tone is hacked in. it's a hack. it's an ugly as fuck hack and i need a shower now. but it'll work. i really, really, really
			//dont want to add a new body type for tigersharks, and i am not adding a fur color equivalent for tones for literally just tigersharks.
			if (((target.body.primarySkin.tone != Tones.GRAY && target.body.primarySkin.tone != Tones.TIGERSHARK) || target.body.type != BodyType.HUMANOID) && Utils.Rand(7) == 0)
			{
				Tones targetTone = isTigerShark ? Tones.TIGERSHARK : Tones.GRAY;
				SkinTexture targetTexture = SkinTexture.ROUGH;
				//getGame().rathazul.addMixologyXP(20);

				target.UpdateBody(BodyType.HUMANOID, targetTone, targetTexture);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//FINZ R WINGS
			if ((target.wings.type != WingType.NONE || target.back.type != BackType.SHARK_FIN) && Utils.Rand(3) == 0)
			{
				target.UpdateBack(BackType.SHARK_FIN);
				target.RestoreWings();

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//this is the fallthrough that occurs when a tf item goes through all the changes, but does not proc enough of them to exit early. it will apply however many changes
			//occurred, then return the contents of the stringbuilder.
			return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
		}

		//the abstract string calls that you create above should be declared here. they should be protected. if it is a body part change or a generic text that has already been
		//defined by the base class, feel free to make it virtual instead.
		protected abstract string InitialTransformationText(Creature target);
	}
}