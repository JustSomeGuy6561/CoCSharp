//CatTransformations.cs
//Description:
//Author: JustSomeGuy
//1/24/2020 9:37:34 PM
using System.Linq;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Creatures.PlayerData;
using CoC.Frontend.Races;
using CoC.Frontend.Settings.Gameplay;
using CoC.Frontend.StatusEffect;

namespace CoC.Frontend.Transformations
{
	internal abstract class CatTransformations : GenericTransformationBase
	{
		//a helper that gets the currently set hyper happy flag for this game session. generally useful, but feel free to remove this if you don't need it.
		private bool hyperHappy => HyperHappySettings.isEnabled;


		protected internal override string DoTransformation(Creature target, out bool isBadEnd)
		{
			isBadEnd = false;

			//by default, this is 2 rolls at 50%, so a 25% chance of 0 additional tfs, 50% chance of 1 additional tf, 25% chance of 2 additional tfs.
			//also takes into consideration any perks that increase or decrease tf effectiveness. if you need to roll out your own, feel free to do so.
			int changeCount = GenerateChangeCount(target, new int[] { 2, 2, 3 });
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


			//Text go!
			if (target is CombatCreature cc)
			{
				//Speed raises up to 75
				if (cc.relativeSpeed < 75 && Utils.Rand(3) == 0)
				{
					//low speed
					if (cc.relativeSpeed <= 30)
					{
						cc.DeltaCombatCreatureStats(spe: 2);
					}
					//medium speed
					else if (cc.relativeSpeed <= 60)
					{
						cc.DeltaCombatCreatureStats(spe: 1);
					}
					//high speed
					else
					{
						cc.DeltaCombatCreatureStats(spe: .5f);
					}
				}
				//Strength raises to 40
				if (cc.relativeStrength < 40 && Utils.Rand(3) == 0)
				{
					cc.DeltaCombatCreatureStats(str: 1);
				}
				//Strength ALWAYS drops if over 60
				//Does not add to change total
				else if (cc.relativeStrength > 60 && Utils.Rand(2) == 0)
				{
					cc.DeltaCombatCreatureStats(str: -2);
				}
				//Toughness drops if over 50
				//Does not add to change total
				if (cc.relativeToughness > 50 && Utils.Rand(2) == 0)
				{
					cc.DeltaCombatCreatureStats(tou: -2);
				}
				//Intelliloss
				if (Utils.Rand(4) == 0)
				{
					cc.DeltaCombatCreatureStats(inte: -1);
				}
			}
			//Libido gain
			if (target.relativeLibido < 80 && Utils.Rand(4) == 0)
			{
				target.DeltaCreatureStats(lib: 1, sens: .25f);
			}

			//Sexual changes would go here if I wasn't a tard.
			//Heat
			if (Utils.Rand(4) == 0)
			{
				bool intensified = target.HasTimedEffect<Heat>();

				if (target.GoIntoHeat())
				{
					if (--remainingChanges <= 0)
					{
						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
			}

			//Shrink the boobalies down to A for men or C for girls.
			if (Utils.Rand(4) == 0 && !hyperHappy)
			{
				CupSize targetSize;
				//female or herm. targeting C cup, but will respect smallest possible cup if it's somehow above a c cup.
				if (target.gender.HasFlag(Gender.FEMALE))
				{
					targetSize = EnumHelper.Max(CupSize.C, target.genitals.smallestPossibleFemaleCupSize);
				}
				//male or genderless. targeting A cup, but will respect smallest possible cup if somehow above an a cup.
				else
				{
					targetSize = EnumHelper.Max(CupSize.A, target.genitals.smallestPossibleMaleCupSize);
				}
				//IT IS!
				int rowsShrunk = 0;
				foreach (Breasts row in target.breasts)
				{
					//If this row is over threshhold
					if (row.cupSize > targetSize)
					{
						//Big change
						if (row.cupSize > CupSize.EE_BIG)
						{
							row.ShrinkBreasts((byte)(2 + Utils.Rand(3)));
						}
						//Small change
						else
						{
							row.ShrinkBreasts(1);
						}
						rowsShrunk++;
					}
				}
				//Count that tits were shrunk
				if (rowsShrunk > 0)
				{
					remainingChanges--;
					if (remainingChanges <= 0)
					{

						return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
					}
				}
			}
			//Cat dangly-doo.
			if (target.cocks.Count > 0 && target.cocks.Any(x => x.type != CockType.CAT) && (target.ears.type == EarType.CAT || Utils.Rand(3) > 0) &&
				(target.tail.type == TailType.CAT || Utils.Rand(3) > 0) && Utils.Rand(4) == 0)
			{
				Cock nonCat = target.cocks.First(x => x.type != CockType.CAT);

				target.genitals.UpdateCock(nonCat, CockType.CAT);
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Cat penorz shrink
			if (target.genitals.HasAnyCocksOfType(CockType.CAT) && Utils.Rand(3) == 0 && !hyperHappy)
			{
				//loop through and find a cat wang.

				foreach (Cock catCock in target.cocks.Where(x => x.type == CockType.CAT))
				{
					//lose 33% size until under 10, then lose 2" at a time
					if (catCock.length > 16)
					{
						catCock.SetLength(catCock.length * .66f);
					}
					else if (catCock.length > 6)
					{
						catCock.DecreaseLength(2);
					}
					if (catCock.length / 5 < catCock.girth && catCock.girth > 1.25)
					{
						catCock.SetGirth(catCock.length / 6);
					}
				}

				//(big sensitivity boost)
				target.DeltaCreatureStats(sens: 5);
				//Make note of other dicks changing
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Neck restore
			if (target.neck.type != NeckType.HUMANOID && Utils.Rand(4) == 0)
			{
				target.RestoreNeck();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Rear body restore
			if (!target.back.isDefault && Utils.Rand(5) == 0)
			{
				target.RestoreBack();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Ovi perk loss
			if (target.womb.canRemoveOviposition && Utils.Rand(5) == 0)
			{
				target.womb.ClearOviposition();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Body type changes.  Teh rarest of the rare.
			// Catgirl-face -> cat-morph-face
			if (target.face.type == FaceType.CAT && !target.face.isFullMorph &&
				target.tongue.type == TongueType.CAT &&
				target.ears.type == EarType.CAT &&
				target.tail.type == TailType.CAT &&
				target.lowerBody.type == LowerBodyType.CAT &&
				target.arms.type == ArmType.CAT &&
				(target.body.IsFurBodyType() /*|| (target.body.type == BodyType.REPTILE && Species.SPHINX.Score(target) >= 4)*/) &&
				Utils.Rand(5) == 0
			)
			{
				target.face.StrengthenFacialMorph();
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//DA EARZ
			if (target.ears.type != EarType.CAT && Utils.Rand(5) == 0)
			{
				//human to cat:
				if (target.ears.type == EarType.HUMAN)
				{
				}
				//non human to cat:
				else
				{
				}
				target.UpdateEars(EarType.CAT);
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//DA TAIL (IF ALREADY HAZ URZ)
			if (target.tail.type != TailType.CAT && target.ears.type == EarType.CAT && Utils.Rand(5) == 0)
			{
				target.UpdateTail(TailType.CAT);
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Da paws (if already haz ears & tail)
			if (target.tail.type == TailType.CAT && target.ears.type == EarType.CAT && Utils.Rand(5) == 0 && target.lowerBody.type != LowerBodyType.CAT)
			{
				//hoof to cat:
				if (target.lowerBody.type == LowerBodyType.HOOVED)
				{
				}
				//Goo to cat
				else if (target.lowerBody.type == LowerBodyType.GOO)
				{
				}
				//non hoof to cat:
				target.UpdateLowerBody(LowerBodyType.CAT);
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//TURN INTO A FURRAH!  OH SHIT
			if (target.tail.type == TailType.CAT && target.ears.type == EarType.CAT && Utils.Rand(5) == 0 && target.lowerBody.type == LowerBodyType.CAT && !target.body.IsFurBodyType())
			{

				target.UpdateBody(BodyType.UNDERBODY_FUR);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			// Fix old cat faces without cat-eyes.
			if (target.face.type == FaceType.CAT && target.eyes.type != EyeType.CAT && Utils.Rand(3) == 0)
			{
				target.UpdateEyes(EyeType.CAT);
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//CAT-FACE!  FULL ON FURRY!  RAGE AWAY NEKOZ
			if (target.tail.type == TailType.CAT && target.ears.type == EarType.CAT && target.lowerBody.type == LowerBodyType.CAT && target.face.type != FaceType.CAT && Utils.Rand(5) == 0)
			{
				//Gain cat face, replace old face
				//MOD NOTE: but not a full-morph cat face. just level 1.
				target.UpdateFace(FaceType.CAT);

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			// Cat-tongue
			if (target.face.type == FaceType.CAT && target.tongue.type != TongueType.CAT && Utils.Rand(5) == 0)
			{
				target.UpdateTongue(TongueType.CAT);
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//Arms
			if (target.arms.type != ArmType.CAT && target.body.IsFurBodyType() && target.tail.type == TailType.CAT && target.lowerBody.type == LowerBodyType.CAT && Utils.Rand(4) == 0)
			{
				target.UpdateArms(ArmType.CAT);
				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			// Remove gills
			if (Utils.Rand(4) == 0 && !target.gills.isDefault)
			{
				target.RestoreGills();

				if (--remainingChanges <= 0)
				{
					return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				}
			}
			//FAILSAFE CHANGE
			if (remainingChanges == changeCount)
			{
				(target as CombatCreature)?.AddHP(50);
				target.DeltaCreatureStats(lus: 3);
			}

			//this is the fallthrough that occurs when a tf item goes through all the changes, but does not proc enough of them to exit early. it will apply however many changes
			//occurred, then return the contents of the stringbuilder.
			return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
		}

		//the abstract string calls that you create above should be declared here. they should be protected. if it is a body part change or a generic text that has already been
		//defined by the base class, feel free to make it virtual instead.
		protected abstract bool InitialTransformationText(Creature target);
	}
}