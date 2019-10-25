using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Creatures.PlayerData;
using CoC.Frontend.Races;
using CoC.Frontend.UI;
using System;
using System.Collections.Generic;

namespace CoC.Frontend.Transformations
{
	/**
	 * Original Credits: 
	 * Based on the Imp transformative item
	 * fucking overhauled by Foxwells who was depressed by the sorry state of imp food
	 */
	class ImpTransformations : TransformationBase<ImpTransformResults>
	{
		StandardDisplay currentDisplay => DisplayManager.GetCurrentDisplay();
		private bool hyperHappy => SaveData.FrontendSessionSave.data?.HyperHappy ?? false;

		protected internal override ImpTransformResults DoTransformation(Creature target, byte strength = 1)
		{
			var results = new ImpTransformResults();
			if (strength == 0)
			{
				return results;
			}

			//currentDisplay.ClearOutput();
			//int results.changeCount = 0;

			int changeLimit = GenerateChanceCount(target, new int[] { 2, 2 }, strength, strength);

			if (target.hasMaleCock)
			{
				//currentDisplay.OutputText("The food tastes strange and corrupt - you can't really think of a better word for it, but it's unclean.");
				if (target.cocks[0].length < 12 )
				{
					float temp = target.cocks[0].LengthenCock(Utils.Rand(2) + 2);
					//currentDisplay.OutputText("" + Environment.NewLine + "" + Environment.NewLine + "");
					//output text for cocks changed, temp amount, and one cock.
					results.lengthenedCock = true;
					results.changeCount++;
				}
				//currentDisplay.OutputText("" + Environment.NewLine + "" + Environment.NewLine + "Inhuman vitality spreads through your body, invigorating you!" + Environment.NewLine + "");
				if (target is CombatCreature combat)
				{
					combat.AddHP((uint)(30 + combat.toughness / 3));
				}
				target.DeltaCreatureStats(lus: 3, corr: 1);

				if (results.changeCount >= changeLimit) return results;
			}
			else
			{
				//currentDisplay.OutputText("The food tastes... corrupt, for lack of a better word." + Environment.NewLine + "");
				if (target is CombatCreature combat)
				{
					combat.AddHP((uint)(20 + combat.toughness / 3));
				}
				target.DeltaCreatureStats(lus: 3, corr: 1);
			}

			//Red or orange skin!
			if (Utils.Rand(30) == 0 && !Array.Exists(Species.IMP.availableTones, x => x == target.body.primarySkin.tone))
			{
				//if (target.hasFur()) currentDisplay.OutputText("" + Environment.NewLine + "" + Environment.NewLine + "Underneath your fur, your skin ");
				//else currentDisplay.OutputText("" + Environment.NewLine + "" + Environment.NewLine + "Your " + target.skin.desc + " ");
				results.changedTone = true;
				results.oldSkinTone = target.body.primarySkin.tone;
				target.body.ChangeMainSkin(Utils.RandomChoice(Species.IMP.availableTones));
				//currentDisplay.OutputText("begins to lose its color, fading until you're as white as an albino. Then, starting at the crown of your head, a reddish hue rolls down your body in a wave, turning you completely " + target.skin.tone + ".");
				//kGAMECLASS.rathazul.addMixologyXP(20); //mixology the result of imbibing these, so move out to tf item level. abstract it out, too. 

				results.changeCount++;
				if (results.changeCount >= changeLimit) return results;
			}


			//Shrinkage!
			if (Utils.Rand(2) == 0 && target.build.heightInInches > 42)
			{
				//currentDisplay.OutputText("" + Environment.NewLine + "" + Environment.NewLine + "Your skin crawls, making you close your eyes and shiver. When you open them again the world seems... different. After a bit of investigation, you realize you've become shorter!");
				target.build.GetShorter((byte)(1 + Utils.Rand(3)));
				results.changeCount++;
				if (results.changeCount >= changeLimit) return results;
			}
			//Imp wings - I just kinda robbed this from demon results.changeCount ~Foxwells
			if (Utils.Rand(3) == 0 && ((target.wings.type != WingType.IMP && target.IsCorruptEnough(25)) || (target.wings.type == WingType.IMP && target.IsCorruptEnough(50))))
			{
				//grow smalls to large
				if (target.wings.type == WingType.IMP)
				{
					//currentDisplay.OutputText("" + Environment.NewLine + "" + Environment.NewLine + "");
					//currentDisplay.OutputText("Your small imp wings stretch and grow, tingling with the pleasure of being attached to such a tainted body. You stretch over your shoulder to stroke them as they unfurl, turning into large imp-wings. <b>Your imp wings have grown!</b>");
					target.wings.GrowLarge();
					results.wingsGrewToLarge = true;
				}
				else
				{
					//currentDisplay.OutputText("" + Environment.NewLine + "" + Environment.NewLine + "");
					//currentDisplay.OutputText("A knot of pain forms in your shoulders as they tense up. With a surprising force, a pair of small imp wings sprout from your back, ripping a pair of holes in the back of your " + target.armorName + ". <b>You now have imp wings.</b>");
					results.changedWings = true;
					results.oldWingType = target.wings.type;
					target.UpdateWings(WingType.IMP);
				}
				results.changeCount++;
				if (results.changeCount >= changeLimit) return results;
			}

			//Imp tail, because that's a unique thing from what I see?
			if (Utils.Rand(3) == 0 && target.tail.type != TailType.IMP)
			{
				//if (target.tail.type != TailType.NONE)
				//{
				//currentDisplay.OutputText("" + Environment.NewLine + "" + Environment.NewLine + "");
				//if (target.tail.type == TailType.SPIDER_ABDOMEN || target.tail.type == TailType.BEE_ABDOMEN) currentDisplay.OutputText("You feel a tingling in your insectile abdomen as it stretches, narrowing, the exoskeleton flaking off as it transforms into an imp's tail, complete with a round fluffed end. ");
				//else currentDisplay.OutputText("You feel a tingling in your tail. You are amazed to discover it has shifted into an imp tail, complete with a fluffy end. ");
				//currentDisplay.OutputText("<b>Your tail is an imp tail!</b>");
				//}
				//else
				//{
				//currentDisplay.OutputText("" + Environment.NewLine + "" + Environment.NewLine + "A pain builds in your backside, growing more and more pronounced. The pressure suddenly disappears with a loud ripping and tearing noise. <b>You realize you now have an imp tail</b>... complete with fluffed end.");
				//}
				target.IncreaseCorruption(2);
				//target.tail.type = TailType.IMP;

				results.changedTail = true;
				results.oldTailType = target.tail.type;

				target.UpdateTail(TailType.IMP);

				results.changeCount++;
				if (results.changeCount >= changeLimit) return results;
			}

			//Feets, needs red/orange skin and tail
			if (Array.Exists(Species.IMP.availableTones, x=>target.body.primarySkin.tone == x) && target.tail.type == TailType.IMP && target.lowerBody.type != LowerBodyType.IMP && Utils.Rand(3) == 0)
			{
				//currentDisplay.OutputText("" + Environment.NewLine + "" + Environment.NewLine + "Every muscle and sinew below your hip tingles and you begin to stagger. Seconds after you sit down, pain explodes in your " + target.feet() + ". Something hard breaks through your sole from the inside out as your " + target.feet() + " splinter and curve cruelly. The pain slowly diminishes and your eyes look along a skinny, human leg that splinters at the foot into three long claw with a smaller back one for balance. When you relax, your feet grip the ground easily. <b>Your lower body is now that of an imp.</b>");
				results.lowerBodyChanged = true;
				results.oldLowerBodyType = target.lowerBody.type;

				target.UpdateLowerBody(LowerBodyType.IMP);
				target.IncreaseCorruption(2);
				results.changeCount++;
				if (results.changeCount >= changeLimit) return results;
			}

			//Imp ears, needs red/orange skin and horns
			if (target.horns.type == HornType.IMP && Array.Exists(Species.IMP.availableTones, x=>target.body.primarySkin.tone == x) && target.ears.type != EarType.IMP && Utils.Rand(3) == 0 )
			{
				//currentDisplay.OutputText("" + Environment.NewLine + "" + Environment.NewLine + "Your head suddenly pulses in pain, causing you to double over and grip at it. You feel your ears elongate and curl in slightly, ending at points not much unlike elves. These, however, jut out of the side of your head and are coned, focusing on every sound around you. A realization strikes you. <b>Your ears are now that of an imp!</b>");
				results.changedEars = true;
				results.oldEarType = target.ears.type;

				target.UpdateEar(EarType.IMP);
				target.IncreaseCorruption(2);
				results.changeCount++;
				if (results.changeCount >= changeLimit) return results;
			}

			//Horns, because why not?
			if (target.horns.type != HornType.IMP && Utils.RandBool())
			{
				//if (target.horns.value == 0)
				//{
				//	currentDisplay.OutputText("" + Environment.NewLine + "" + Environment.NewLine + "A small pair of pointed imp horns erupt from your forehead. They look kind of cute. <b>You have horns!</b>");
				//}
				//else
				//{
				//	currentDisplay.OutputText("" + Environment.NewLine + "" + Environment.NewLine + "");
				//	currentDisplay.OutputText("Your horns shift, turning into two pointed imp horns.");
				//}

				results.changedHorns = true;
				results.oldHornType = target.horns.type;

				target.UpdateHorns(HornType.IMP);

				target.IncreaseCorruption(2);
				results.changeCount++;
				if (results.changeCount >= changeLimit) return results;
			}

			//Imp arms, needs orange/red skin. Also your hands turn human.
			if (Array.Exists(Species.IMP.availableTones, x=>target.body.primarySkin.tone == x) && target.arms.type != ArmType.IMP && Utils.Rand(3) == 0 )
			{
				//if (target.arms.type != ArmType.HUMAN)
				//{
				//	currentDisplay.OutputText("" + Environment.NewLine + "" + Environment.NewLine + "Your arms twist and mangle, warping back into human-like arms. But that, you realize, is just the beginning.");
				//}
				//if (target.arms.claws.type == Claws.NORMAL)
				//{
				//	currentDisplay.OutputText("" + Environment.NewLine + "" + Environment.NewLine + "Your hands suddenly ache in pain, and all you can do is curl them up to you. Against your body, you feel them form into three long claws, with a smaller one replacing your thumb but just as versatile. <b>You have imp claws!</b>");
				//}
				//else
				//{ //has claws
				//	currentDisplay.OutputText("" + Environment.NewLine + "" + Environment.NewLine + "Your claws suddenly begin to shift and change, starting to turn back into normal hands. But just before they do, they stretch out into three long claws, with a smaller one coming to form a pointed thumb. <b>You have imp claws!</b>");
				//}


				results.armsChanged = true;
				results.oldArmType = target.arms.type;

				target.UpdateArms(ArmType.IMP);

				target.IncreaseCorruption(2);
				results.changeCount++;
				if (results.changeCount >= changeLimit) return results;
			}

			//Changes hair to red/dark red, shortens it, sets it normal
			if (!Array.Exists(Species.IMP.availableHairColors, x=>x == target.hair.hairColor) && Utils.Rand(3) == 0 )
			{
				//currentDisplay.OutputText("" + Environment.NewLine + "" + Environment.NewLine + "Your hair suddenly begins to shed, rapidly falling down around you before it's all completely gone. Just when you think things are over, more hair sprouts from your head, slightly curled and color different.");
				//if (Utils.Rand(2) != 0)
				//{
				//	target.hair.color = "red";
				//}
				//else
				//{
				//	target.hair.color = "dark red";
				//}
				//currentDisplay.OutputText(" <b>You now have " + target.hair.color + "</b>");
				//if (target.hair.type != Hair.NORMAL)
				//{
				//	currentDisplay.OutputText("<b> human</b>");
				//}
				//currentDisplay.OutputText("<b> hair!</b>");
				//target.hair.type = Hair.NORMAL;
				//target.hair.length = 1;
				results.hairColorChanged = true;
				results.oldHairColor = target.hair.hairColor;

				if (target.hair.type != HairType.NORMAL)
				{
					target.UpdateHair(HairType.NORMAL, Utils.RandomChoice(Species.IMP.availableHairColors), newHairLength: 1);
					results.hairTypeChanged = true;
				}
				else
				{
					target.hair.SetAll(1, Utils.RandomChoice(Species.IMP.availableHairColors));
				}
				
				results.changeCount++;
				if (results.changeCount >= changeLimit) return results;

			}

			//Shrink titties
			if (target.genitals.BiggestCupSize() > CupSize.FLAT && Utils.Rand(3) == 0 && !SaveData.FrontendSessionSave.data.HyperHappy)
			{
				results.triedToShrinkBreasts = true;

				byte rowsModified = 0;
				results.breastsShrunkBy = new byte[target.breasts.Count];
				//temp3 stores how many rows are changed
				for (int k = 0; k < target.breasts.Count; k++)
				{
					//If this row is over threshhold
					if (target.breasts[k].cupSize > CupSize.FLAT)
					{
						CupSize oldSize = target.breasts[k].cupSize;
						//Big change
						if (target.breasts[k].cupSize > CupSize.EE_BIG)
						{
							target.breasts[k].ShrinkBreasts((byte)(2 + Utils.Rand(3)));
							results.breastsShrunkBy[k] = oldSize - target.breasts[k].cupSize;
							if (results.breastsShrunkBy[k] != 0)
							{
								rowsModified++;
							}
							//if (rowsModified == 0) currentDisplay.OutputText("" + Environment.NewLine + "" + Environment.NewLine + "The " + target.breastDescript(0) + " on your chest wobble for a second, then tighten up, losing several cup-sizes in the process!");
							//else currentDisplay.OutputText(" The change moves down to your " + num2Text2(k + 1) + " row of " + target.breastDescript(0) + ". They shrink greatly, losing a couple cup-sizes.");
						}
						//Small change
						else
						{
							target.breasts[k].ShrinkBreasts(1);
							results.breastsShrunkBy[k] = oldSize - target.breasts[k].cupSize;
							if (results.breastsShrunkBy[k] != 0)
							{
								rowsModified++;
							}
							//if (rowsModified == 0) currentDisplay.OutputText("" + Environment.NewLine + "" + Environment.NewLine + "All at once, your sense of gravity shifts. Your back feels a sense of relief, and it takes you a moment to realize your " + target.breastDescript(k) + " have shrunk!");
							//else currentDisplay.OutputText(" Your " + num2Text2(k + 1) + " row of " + target.breastDescript(k) + " gives a tiny jiggle as it shrinks, losing some off its mass.");
						}
						//Increment changed rows
					}
				}
				results.breastRowsModified = rowsModified;
				results.changeCount++;


			}

			//Remove spare titties
			if (target.breasts.Count > 1 && Utils.Rand(3) == 0 && !hyperHappy)
			{
				results.removedExtraBreasts = true;
				results.rowsRemoved = target.genitals.RemoveExtraBreastRows();
				results.changeCount++;
				if (results.changeCount >= changeLimit) return results;

			}

			//Free extra nipple removal service
			if (target.genitals.quadNipples && Utils.Rand(3) == 0 )
			{
				results.removedQuadNipples = true;
				//currentDisplay.OutputText("" + Environment.NewLine + "" + Environment.NewLine + "A strange burning sensation fills your breasts, and you look in your " + target.armorName + " to see your extra nipples are gone! <b>You've lost your extra nipples!</b>");
				target.DecreaseSensitivity(3);
				target.genitals.SetQuadNipples(false);
				results.changeCount++;
				if (results.changeCount >= changeLimit) return results;

			}

			//Neck restore
			if (target.neck.type != NeckType.defaultValue && Utils.Rand(4) == 0)
			{
				results.restoredNeck = true;

				results.oldNeckType = target.neck.type;
				target.RestoreNeck();

				results.changeCount++;
				if (results.changeCount >= changeLimit) return results;

			}
			//Rear body restore
			if (target.back.type != BackType.defaultValue && Utils.Rand(5) == 0)
			{
				results.restoredBack = true;
				results.oldBackType = target.neck.type;

				target.RestoreBack();

				results.changeCount++;
				if (results.changeCount >= changeLimit) return results;
			}
			//Ovi perk loss
			if (target is Player && Utils.Rand(5) == 0 )
			{
				results.removedOviposition = ((PlayerWomb)target.womb).ClearOviposition();
				if (results.removedOviposition)
				{
					results.changeCount++;
				}
				if (results.changeCount >= changeLimit) return results;
			}

			//You lotta imp? Time to turn male!
			//Unless you're one of the hyper happy assholes I guess
			//For real tho doesn't seem like female imps exist? Guess they're goblins
			if (target.ImpScore() >= 4 && !hyperHappy)
			{
				results.impified = true;

				if (target.breasts.Count > 1)
				{
					//currentDisplay.OutputText("" + Environment.NewLine + "" + Environment.NewLine + "You stumble back when your center of balance shifts, and though you adjust before you can fall over, you're left to watch in awe as your extra breasts shrink down, disappearing completely into your body. The nipples even fade away until they're gone completely. <b>You've lost your extra breasts due to being an imp!</b>");
					results.removedExtraBreasts = true;
					results.rowsRemoved = target.genitals.RemoveExtraBreastRows();
				}

				target.breasts[0].MakeMale(true);


				if (target.hasVagina)
				{
					results.vaginasRemoved = target.RemoveAllVaginas();

				}
				if (!target.hasCock)
				{
					results.grewCock = true;
					target.AddCock(CockType.HUMAN, 12, 2);
					//currentDisplay.OutputText("" + Environment.NewLine + "" + Environment.NewLine + "Pressure builds between your legs, and you barely get your armor off in time to watch a cock grow out of you. <b>You've grown a cock due to being an imp!</b>");
					
				}
				if (target.balls.count == 0)
				{
					results.grewBalls = true;
					//currentDisplay.OutputText("" + Environment.NewLine + "" + Environment.NewLine + "A strange, unpleasant pressure forms between your thighs. You take off your armor and see that you've grown balls. <b>You've grown balls due to being an imp!</b>");
					target.genitals.GrowBalls(2);
				}
				results.changeCount++;
				target.IncreaseCorruption(20);
			}
			if (target is IExtendedCreature extended)
			{
				extended.extendedData.TotalTransformCount += results.changeCount;
			}

			return results;
		}

	}

	class ImpTransformResults
	{
		internal int changeCount = 0;
		internal bool lengthenedCock = false;
		internal bool changedTone = false;
		internal Tones oldSkinTone = null;
		internal bool changedWings = false;
		internal WingType oldWingType = null;
		internal bool wingsGrewToLarge = false;
		internal bool changedTail = false;
		internal TailType oldTailType = null;
		internal bool lowerBodyChanged = false;
		internal LowerBodyType oldLowerBodyType = null;
		internal bool changedEars = false;
		internal EarType oldEarType = null;
		internal bool changedHorns = false;
		internal HornType oldHornType = null;
		internal bool armsChanged = false;
		internal ArmType oldArmType = null;
		internal bool hairTypeChanged = false;
		internal HairFurColors oldHairColor;
		internal bool hairColorChanged = false;
		internal bool triedToShrinkBreasts = false;
		internal byte breastRowsModified = 0;
		internal byte[] breastsShrunkBy = null;
		internal bool removedExtraBreasts = false;
		internal int rowsRemoved = 0;
		internal bool removedQuadNipples = false;
		internal bool restoredNeck = false;
		internal bool restoredBack = false;
		internal bool removedOviposition = false;
		internal bool impified = false;
		internal int vaginasRemoved = 0;
		internal bool grewCock = false;
		internal bool grewBalls = false;
		internal NeckType oldBackType = null;
		internal NeckType oldNeckType = null;
	}
}
