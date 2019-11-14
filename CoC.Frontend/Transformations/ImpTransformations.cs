using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Creatures.PlayerData;
using CoC.Frontend.Races;
using CoC.Frontend.UI;
using System;
using System.Text;

namespace CoC.Frontend.Transformations
{
	/**
	 * Original Credits: 
	 * Based on the Imp transformative item
	 * fucking overhauled by Foxwells who was depressed by the sorry state of imp food
	 */
	internal abstract class ImpTransformation : GenericTransformationBase
	{
		StandardDisplay currentDisplay => DisplayManager.GetCurrentDisplay();
		private bool hyperHappy => SaveData.FrontendSessionSave.data?.HyperHappyLocal ?? false;

		protected internal override string DoTransformation(Creature target, out bool isBadEnd)
		{
			isBadEnd = false;

			int changeCount = GenerateChanceCount(target, new int[] { 2, 2 });
			int remainingChanges = changeCount;

			StringBuilder sb = new StringBuilder();

			sb.Append(InitialTransformText(target));
			//if male	//currentDisplay.OutputText(target, "The food tastes strange and corrupt - you can't really think of a better word for it, but it's unclean.");
			//else //currentDisplay.OutputText(target, "The food tastes... corrupt, for lack of a better word." + Environment.NewLine + "");
			target.DeltaCreatureStats(lus: 3, corr: 1);

			uint hpDelta;
			if (target.hasCock)
			{
				if (target.cocks[0].length < 12)
				{
					float temp = target.cocks[0].LengthenCock(Utils.Rand(2) + 2);
					sb.Append(OneCockGrewLarger(target, target.cocks[0], temp));

				}
				hpDelta = 30;

			}
			else
			{
				hpDelta = 20;
			}

			if (target is CombatCreature healthCheck)
			{
				sb.Append(GainVitalityText(target));
				healthCheck.AddHP((uint)(hpDelta + healthCheck.toughness / 3));
			}
			if (remainingChanges <= 0) return ApplyAndReturn(target, sb, changeCount - remainingChanges);

			//Red or orange skin!
			if (Utils.Rand(30) == 0 && !Array.Exists(Species.IMP.availableTones, x => x == target.body.primarySkin.tone))
			{
				//if (target.hasFur()) currentDisplay.OutputText(target, "" + Environment.NewLine + "" + Environment.NewLine + "Underneath your fur, your skin ");
				//else currentDisplay.OutputText(target, "" + Environment.NewLine + "" + Environment.NewLine + "Your " + target.skin.desc + " ");

				var oldSkinTone = target.body.primarySkin.tone;
				target.body.ChangeMainSkin(Utils.RandomChoice(Species.IMP.availableTones));

				sb.Append(ChangeSkinColorText(target, oldSkinTone, target.body.primarySkin.tone));

				//currentDisplay.OutputText(target, "begins to lose its color, fading until you're as white as an albino. Then, starting at the crown of your head, a reddish hue rolls down your body in a wave, turning you completely " + target.skin.tone + ".");
				//kGAMECLASS.rathazul.addMixologyXP(20); //mixology the result of imbibing these, so move out to tf item level. abstract it out, too. 

				if (--remainingChanges <= 0) return ApplyAndReturn(target, sb, changeCount - remainingChanges);
			}


			//Shrinkage!
			if (Utils.Rand(2) == 0 && target.build.heightInInches > 42)
			{
				//currentDisplay.OutputText(target, "" + Environment.NewLine + "" + Environment.NewLine + "Your skin crawls, making you close your eyes and shiver. When you open them again the world seems... different. After a bit of investigation, you realize you've become shorter!");
				var heightDelta = target.build.GetShorter((byte)(1 + Utils.Rand(3)));
				sb.Append(GetShorterText(target, heightDelta));

				if (--remainingChanges <= 0) return ApplyAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Imp wings - I just kinda robbed this from demon changeCount ~Foxwells
			if (Utils.Rand(3) == 0 && ((target.wings.type != WingType.IMP && target.IsCorruptEnough(25)) || (target.wings.type == WingType.IMP && target.IsCorruptEnough(50))))
			{
				//grow smalls to large
				if (target.wings.type == WingType.IMP)
				{
					//currentDisplay.OutputText(target, "" + Environment.NewLine + "" + Environment.NewLine + "");
					//currentDisplay.OutputText(target, "Your small imp wings stretch and grow, tingling with the pleasure of being attached to such a tainted body. You stretch over your shoulder to stroke them as they unfurl, turning into large imp-wings. <b>Your imp wings have grown!</b>");
					target.wings.GrowLarge();
					sb.Append(EnlargenedImpWingsText(target));
				}
				else
				{
					var oldType = target.wings.type;
					//currentDisplay.OutputText(target, "" + Environment.NewLine + "" + Environment.NewLine + "");
					//currentDisplay.OutputText(target, "A knot of pain forms in your shoulders as they tense up. With a surprising force, a pair of small imp wings sprout from your back, ripping a pair of holes in the back of your " + target.armorName + ". <b>You now have imp wings.</b>");
					target.UpdateWings(WingType.IMP);
					sb.Append(GrowOrChangeWingsText(target, oldType));
				}

				if (--remainingChanges <= 0) return ApplyAndReturn(target, sb, changeCount - remainingChanges);
			}

			//Imp tail, because that's a unique thing from what I see?
			if (Utils.Rand(3) == 0 && target.tail.type != TailType.IMP)
			{
				var oldType = target.tail.type;
				sb.Append(GrowOrChangeTailText(target, oldType));
				//if (target.tail.type != TailType.NONE)
				//{
				//currentDisplay.OutputText(target, "" + Environment.NewLine + "" + Environment.NewLine + "");
				//if (target.tail.type == TailType.SPIDER_ABDOMEN || target.tail.type == TailType.BEE_ABDOMEN) currentDisplay.OutputText(target, "You feel a tingling in your insectile abdomen as it stretches, narrowing, the exoskeleton flaking off as it transforms into an imp's tail, complete with a round fluffed end. ");
				//else currentDisplay.OutputText(target, "You feel a tingling in your tail. You are amazed to discover it has shifted into an imp tail, complete with a fluffy end. ");
				//currentDisplay.OutputText(target, "<b>Your tail is an imp tail!</b>");
				//}
				//else
				//{
				//currentDisplay.OutputText(target, "" + Environment.NewLine + "" + Environment.NewLine + "A pain builds in your backside, growing more and more pronounced. The pressure suddenly disappears with a loud ripping and tearing noise. <b>You realize you now have an imp tail</b>... complete with fluffed end.");
				//}
				target.IncreaseCorruption(2);
				target.UpdateTail(TailType.IMP);

				if (--remainingChanges <= 0) return ApplyAndReturn(target, sb, changeCount - remainingChanges);
			}

			//Feets, needs red/orange skin and tail
			if (Species.IMP.availableTones.Contains(target.body.primarySkin.tone) && target.tail.type == TailType.IMP && target.lowerBody.type != LowerBodyType.IMP && Utils.Rand(3) == 0)
			{
				var oldType = target.lowerBody.type;
				sb.Append(ChangeLowerBodyText(target, oldType));
				//currentDisplay.OutputText(target, "" + Environment.NewLine + "" + Environment.NewLine + "Every muscle and sinew below your hip tingles and you begin to stagger. Seconds after you sit down, pain explodes in your " + target.feet() + ". Something hard breaks through your sole from the inside out as your " + target.feet() + " splinter and curve cruelly. The pain slowly diminishes and your eyes look along a skinny, human leg that splinters at the foot into three long claw with a smaller back one for balance. When you relax, your feet grip the ground easily. <b>Your lower body is now that of an imp.</b>");

				target.UpdateLowerBody(LowerBodyType.IMP);
				target.IncreaseCorruption(2);

				if (--remainingChanges <= 0) return ApplyAndReturn(target, sb, changeCount - remainingChanges);
			}

			//Imp ears, needs red/orange skin and horns
			if (target.horns.type == HornType.IMP && Array.Exists(Species.IMP.availableTones, x => target.body.primarySkin.tone == x) && target.ears.type != EarType.IMP && Utils.Rand(3) == 0)
			{
				//currentDisplay.OutputText(target, "" + Environment.NewLine + "" + Environment.NewLine + "Your head suddenly pulses in pain, causing you to double over and grip at it. You feel your ears elongate and curl in slightly, ending at points not much unlike elves. These, however, jut out of the side of your head and are coned, focusing on every sound around you. A realization strikes you. <b>Your ears are now that of an imp!</b>");
				var oldType = target.ears.type;
				sb.Append(ChangeEarsText(target, oldType));

				target.UpdateEar(EarType.IMP);
				target.IncreaseCorruption(2);
				if (--remainingChanges <= 0) return ApplyAndReturn(target, sb, changeCount - remainingChanges);
			}

			//Horns, because why not?
			if (target.horns.type != HornType.IMP && Utils.RandBool())
			{
				var oldType = target.ears.type;
				sb.Append(ChangeOrGrowHornsText(target, oldType));

				//if (target.horns.value == 0)
				//{
				//	currentDisplay.OutputText(target, "" + Environment.NewLine + "" + Environment.NewLine + "A small pair of pointed imp horns erupt from your forehead. They look kind of cute. <b>You have horns!</b>");
				//}
				//else
				//{
				//	currentDisplay.OutputText(target, "" + Environment.NewLine + "" + Environment.NewLine + "");
				//	currentDisplay.OutputText(target, "Your horns shift, turning into two pointed imp horns.");
				//}

				target.UpdateHorns(HornType.IMP);

				target.IncreaseCorruption(2);
				if (--remainingChanges <= 0) return ApplyAndReturn(target, sb, changeCount - remainingChanges);
			}

			//Imp arms, needs orange/red skin. Also your hands turn human.
			if (Species.IMP.availableTones.Contains(target.body.primarySkin.tone) && target.arms.type != ArmType.IMP && Utils.Rand(3) == 0)
			{
				var oldType = target.arms.type;
				sb.Append(ChangeArmText(target, oldType));
				//if (target.arms.type != ArmType.HUMAN)
				//{
				//	currentDisplay.OutputText(target, "" + Environment.NewLine + "" + Environment.NewLine + "Your arms twist and mangle, warping back into human-like arms. But that, you realize, is just the beginning.");
				//}
				//if (target.arms.claws.type == Claws.NORMAL)
				//{
				//	currentDisplay.OutputText(target, "" + Environment.NewLine + "" + Environment.NewLine + "Your hands suddenly ache in pain, and all you can do is curl them up to you. Against your body, you feel them form into three long claws, with a smaller one replacing your thumb but just as versatile. <b>You have imp claws!</b>");
				//}
				//else
				//{ //has claws
				//	currentDisplay.OutputText(target, "" + Environment.NewLine + "" + Environment.NewLine + "Your claws suddenly begin to shift and change, starting to turn back into normal hands. But just before they do, they stretch out into three long claws, with a smaller one coming to form a pointed thumb. <b>You have imp claws!</b>");
				//}


				target.UpdateArms(ArmType.IMP);

				target.IncreaseCorruption(2);
				if (--remainingChanges <= 0) return ApplyAndReturn(target, sb, changeCount - remainingChanges);
			}

			//Changes hair to red/dark red, shortens it, sets it normal
			if (!Species.IMP.availableHairColors.Contains(target.hair.hairColor) && Utils.Rand(3) == 0)
			{
				//currentDisplay.OutputText(target, "" + Environment.NewLine + "" + Environment.NewLine + "Your hair suddenly begins to shed, rapidly falling down around you before it's all completely gone. Just when you think things are over, more hair sprouts from your head, slightly curled and color different.");
				//if (Utils.Rand(2) != 0)
				//{
				//	target.hair.color = "red";
				//}
				//else
				//{
				//	target.hair.color = "dark red";
				//}
				//currentDisplay.OutputText(target, " <b>You now have " + target.hair.color + "</b>");
				//if (target.hair.type != Hair.NORMAL)
				//{
				//	currentDisplay.OutputText(target, "<b> human</b>");
				//}
				//currentDisplay.OutputText(target, "<b> hair!</b>");
				//target.hair.type = Hair.NORMAL;
				//target.hair.length = 1;

				var oldHairData = target.hair.AsReadOnlyReference();

				var hairColor = Utils.RandomChoice(Species.IMP.availableHairColors);
				var hairLength = 1f;
				sb.Append(HairChangedText(target, oldHairData, hairColor, hairLength));

				if (target.hair.type != HairType.NORMAL)
				{
					target.UpdateHair(HairType.NORMAL, hairColor, newHairLength: hairLength);
				}
				else
				{
					target.hair.SetAll(hairLength, hairColor);
				}

				if (--remainingChanges <= 0) return ApplyAndReturn(target, sb, changeCount - remainingChanges);

			}

			//Remove spare titties
			if (target.breasts.Count > 1 && Utils.Rand(3) == 0 && !hyperHappy)
			{
				var removeCount = target.genitals.RemoveExtraBreastRows();
				sb.Append(RemovedAdditionalBreasts(target, removeCount));
				if (--remainingChanges <= 0) return ApplyAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Shrink titties
			if (target.genitals.BiggestCupSize() > CupSize.FLAT && Utils.Rand(3) == 0 && !hyperHappy)
			{
				byte rowsModified = 0;
				//temp3 stores how many rows are changed
				foreach (Breasts breast in target.breasts)
				{
					//If this row is over threshhold
					if (breast.cupSize > CupSize.FLAT)
					{
						CupSize oldSize = breast.cupSize;
						//Big change
						if (breast.cupSize > CupSize.EE_BIG)
						{
							var delta = breast.ShrinkBreasts((byte)(2 + Utils.Rand(3)));
							if (delta != 0)
							{
								rowsModified++;
								sb.Append(CurrentBreastRowChanged(target, breast, delta, rowsModified, true));
							}


							//if (rowsModified == 0) currentDisplay.OutputText(target, "" + Environment.NewLine + "" + Environment.NewLine + "The " + target.breastDescript(0) + " on your chest wobble for a second, then tighten up, losing several cup-sizes in the process!");
							//else currentDisplay.OutputText(target, " The change moves down to your " + num2Text2(k + 1) + " row of " + target.breastDescript(0) + ". They shrink greatly, losing a couple cup-sizes.");
						}
						//Small change
						else
						{
							var delta = breast.ShrinkBreasts(1);
							if (delta != 0)
							{
								rowsModified++;
								sb.Append(CurrentBreastRowChanged(target, breast, delta, rowsModified, false));
							}

							//if (rowsModified == 0) currentDisplay.OutputText(target, "" + Environment.NewLine + "" + Environment.NewLine + "All at once, your sense of gravity shifts. Your back feels a sense of relief, and it takes you a moment to realize your " + target.breastDescript(k) + " have shrunk!");
							//else currentDisplay.OutputText(target, " Your " + num2Text2(k + 1) + " row of " + target.breastDescript(k) + " gives a tiny jiggle as it shrinks, losing some off its mass.");
						}
						//Increment changed rows
					}
				}

				if (--remainingChanges <= 0) return ApplyAndReturn(target, sb, changeCount - remainingChanges);
			}



			//Free extra nipple removal service
			if (target.genitals.quadNipples && Utils.Rand(3) == 0)
			{
				sb.Append(RemovedQuadNippleText(target));
				//currentDisplay.OutputText(target, "" + Environment.NewLine + "" + Environment.NewLine + "A strange burning sensation fills your breasts, and you look in your " + target.armorName + " to see your extra nipples are gone! <b>You've lost your extra nipples!</b>");
				target.DecreaseSensitivity(3);
				target.genitals.SetQuadNipples(false);
				if (--remainingChanges <= 0) return ApplyAndReturn(target, sb, changeCount - remainingChanges);

			}

			//Neck restore
			if (target.neck.type != NeckType.defaultValue && Utils.Rand(4) == 0)
			{

				var oldType = target.neck.type;
				sb.Append(RestoredNeckText(target, oldType));
				target.RestoreNeck();

				if (--remainingChanges <= 0) return ApplyAndReturn(target, sb, changeCount - remainingChanges);

			}
			//Rear body restore
			if (target.back.type != BackType.defaultValue && Utils.Rand(5) == 0)
			{
				var oldType = target.neck.type;
				sb.Append(RestoredBackText(target, oldType));
				target.RestoreBack();

				if (--remainingChanges <= 0) return ApplyAndReturn(target, sb, changeCount - remainingChanges);
			}
			//Ovi perk loss
			if (target is Player && Utils.Rand(5) == 0)
			{
				if (((PlayerWomb)target.womb).ClearOviposition())
				{
					sb.Append(RemovedOvipositionText(target));
					if (--remainingChanges <= 0) return ApplyAndReturn(target, sb, changeCount - remainingChanges);
				}
			}

			//You lotta imp? Time to turn male!
			//Unless you're one of the hyper happy assholes I guess
			//For real tho doesn't seem like female imps exist? Guess they're goblins
			if (target.ImpScore() >= 4 && !hyperHappy)
			{
				int rowsRemoved = 0;
				bool madeFinalRowMale = false;
				int vaginasRemoved = 0;
				bool grewCock = false;
				bool grewBalls = false;

				if (target.breasts.Count > 1)
				{
					//currentDisplay.OutputText(target, "" + Environment.NewLine + "" + Environment.NewLine + "You stumble back when your center of balance shifts, and though you adjust before you can fall over, you're left to watch in awe as your extra breasts shrink down, disappearing completely into your body. The nipples even fade away until they're gone completely. <b>You've lost your extra breasts due to being an imp!</b>");
					rowsRemoved = target.genitals.RemoveExtraBreastRows();
				}

				madeFinalRowMale = target.breasts[0].MakeMale(true);


				if (target.hasVagina)
				{
					vaginasRemoved = target.RemoveAllVaginas();

				}
				if (!target.hasCockOrClitCock)
				{
					grewCock = true;
					target.AddCock(CockType.HUMAN, 12, 2);
					//currentDisplay.OutputText(target, "" + Environment.NewLine + "" + Environment.NewLine + "Pressure builds between your legs, and you barely get your armor off in time to watch a cock grow out of you. <b>You've grown a cock due to being an imp!</b>");

				}
				if (target.balls.count == 0)
				{
					grewBalls = true;
					//currentDisplay.OutputText(target, "" + Environment.NewLine + "" + Environment.NewLine + "A strange, unpleasant pressure forms between your thighs. You take off your armor and see that you've grown balls. <b>You've grown balls due to being an imp!</b>");
					target.genitals.GrowBalls(2);
				}

				sb.Append(ImpifiedText(target, rowsRemoved, madeFinalRowMale, grewCock, grewBalls));
				remainingChanges--;
				target.IncreaseCorruption(20);


			}
			return ApplyAndReturn(target, sb, changeCount - remainingChanges);
		}

		protected abstract string ImpifiedText(Creature target, int breastRowsRemoved, bool madeFinalBreastRowMale, bool grewCock, bool grewBalls);
		protected abstract string RemovedOvipositionText(Creature target);
		protected abstract string RestoredBackText(Creature target, NeckType oldType);
		protected abstract string RestoredNeckText(Creature target, NeckType oldType);
		protected abstract string RemovedQuadNippleText(Creature target);
		protected abstract string CurrentBreastRowChanged(Creature target, Breasts breast, byte delta, byte rowsModified, bool wasLargeChange);
		protected abstract string RemovedAdditionalBreasts(Creature target, int removeCount);
		protected abstract string HairChangedText(Creature target, HairData oldHairData, HairFurColors hairColor, float hairLength);
		protected abstract string ChangeArmText(Creature target, ArmType oldType);
		protected abstract string ChangeOrGrowHornsText(Creature target, EarType oldType);
		protected abstract string ChangeEarsText(Creature target, EarType oldType);
		protected abstract string ChangeLowerBodyText(Creature target, LowerBodyType oldType);
		protected abstract string InitialTransformText(Creature target);
		protected abstract string OneCockGrewLarger(Creature target, Cock cock, float temp);
		protected abstract string GainVitalityText(Creature target);
		protected abstract string ChangeSkinColorText(Creature target, Tones oldSkinTone, Tones tone);
		protected abstract string GetShorterText(Creature target, byte heightDelta);
		protected abstract string EnlargenedImpWingsText(Creature target);
		protected abstract string GrowOrChangeWingsText(Creature target, WingType oldType);
		protected abstract string GrowOrChangeTailText(Creature target, TailType oldType);
	}
}