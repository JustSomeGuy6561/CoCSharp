//AnemoneTransformations.cs
//Description:
//Author: JustSomeGuy
//1/24/2020 9:43:57 PM
using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Creatures.PlayerData;
using CoC.Frontend.Races;
using CoC.Frontend.Settings.Gameplay;
using System.Text;

namespace CoC.Frontend.Transformations
{
	internal abstract class AnemoneTransformations : GenericTransformationBase
	{
		//a helper that gets the currently set hyper happy flag for this game session. generally useful, but feel free to remove this if you don't need it.
		private bool hyperHappy => HyperHappySettings.isEnabled;


		protected internal override string DoTransformation(Creature target, out bool isBadEnd)
		{
			isBadEnd = false;

			//by default, this is 2 rolls at 50%, so a 25% chance of 0 additional tfs, 50% chance of 1 additional tf, 25% chance of 2 additional tfs.
			//also takes into consideration any perks that increase or decrease tf effectiveness. if you need to roll out your own, feel free to do so.
			int changeCount = GenerateChangeCount(target, new int[] { 2, 3 });
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
			if (remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);

			//Any transformation related changes go here. these typically cost 1 change. these can be anything from body parts to gender (which technically also changes body parts,
			//but w/e). You are required to make sure you return as soon as you've applied changeCount changes, but a single line of code can be applied at the end of a change to do
			//this for you.

			//paste this line after any tf is applied, and it will: automatically decrement the remaining changes count. if it becomes 0 or less, apply the total number of changes
			//underwent to the target's change count (if applicable) and then return the StringBuilder content.
			//if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);

			//possible use effects:
			//- corruption increases by 1 up to low threshold (~20)
			if (Utils.Rand(3) == 0 && target.corruption < 20)
			{
				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
				target.DeltaCreatureStats(corr: 1);
			}
			if (target is CombatCreature cc)
			{
				//- toughess up, sensitivity down
				if (Utils.Rand(3) == 0 && cc.relativeToughness < 50)
				{

					cc.DeltaCombatCreatureStats(tou: 1, sens: -1);
				}
				//- speed down
				if (Utils.Rand(3) == 0 && cc.relativeSpeed > 40)
				{
					cc.DeltaCombatCreatureStats(spe: -1);
				}
			}//-always increases lust by a function of sensitivity

			//"The tingling of the tentacle

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

			//physical changes:
			//- may randomly remove bee abdomen, if present; always checks and does so when any changes to hair might happen
			if (Utils.Rand(4) == 0 && target.tail.type == TailType.BEE_STINGER)
			{

				target.UpdateTail(TailType.NONE);
				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//-may randomly remove bee wings:
			if (Utils.Rand(4) == 0 && target.wings.type == WingType.BEE_LIKE)
			{
				target.RestoreWings();

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//-hair morphs to anemone tentacles, retains color, hair shrinks back to med-short('shaggy') and stops growing, lengthening treatments don't work and goblins won't cut it, but more anemone items can lengthen it one level at a time
			if (target.gills.type == GillType.ANEMONE && target.hair.type != HairType.ANEMONE && Utils.Rand(5) == 0)
			{
				target.UpdateHair(HairType.ANEMONE, newStyle: HairStyle.WAVY);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			//-feathery gills sprout from chest and drape sensually over nipples (cumulative swimming power boost with fin, if swimming is implemented)
			if (Utils.Rand(5) == 0 && target.gills.type != GillType.ANEMONE && target.body.primarySkin.tone == Tones.BLUE_BLACK)
			{
				target.UpdateGills(GillType.ANEMONE);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}

			//-[aphotic] skin tone (blue-black)
			if (Utils.Rand(5) == 0 && target.body.primarySkin.tone != Tones.BLUE_BLACK)
			{

				target.body.ChangeAllSkin(Tones.BLUE_BLACK);

				//kGAMECLASS.rathazul.addMixologyXP(20);
				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
			}
			//-eat more, grow more 'hair':
			if (target.hair.type == HairType.ANEMONE && target.hair.length < 36 && Utils.Rand(2) == 0)
			{
				float temp = 5 + Utils.Rand(3);

				//grow it, and force growth. ignore the natural growth because that's how anemone hair works - it lengthens with tf item.
				target.hair.GrowHair(temp, true);

				if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);
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