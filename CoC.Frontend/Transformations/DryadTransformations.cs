//DryadTransformations.cs
//Description:
//Author: JustSomeGuy
//2/10/2020 2:47:30 PM
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Creatures.PlayerData;
using CoC.Frontend.Races;
using CoC.Frontend.Settings.Gameplay;
using System;
using System.Text;

namespace CoC.Frontend.Transformations
{
	internal abstract class DryadTransformations : GenericTransformationBase
	{
		//a collection of simple effects. the callback will append its results (if applicable) to the stringbuilder, and return a boolean telling us if it did anything.
		protected static readonly Func<Creature, StringBuilder, bool>[] simpleEffects;
		//a collection of full effects. the callback will append its results (if applicable) to the stringbuilder, and return a boolean telling us if it did anything.
		protected static readonly Func<Creature, StringBuilder, bool>[] fullEffects;

		static DryadTransformations()
		{
			simpleEffects = new Func<Creature, StringBuilder, bool>[]
			{
				ChangeHips,

			};
		}

		private static bool ChangeHips(Creature target, StringBuilder content)
		{
			var hipData = target.hips.AsReadOnlyData();
			bool changed = false;
			if (target.hips.size < 5)
			{
				changed = target.hips.GrowHips(1) != 0;
			}
			else if (target.hips.size > 5)
			{
				changed = target.hips.ShrinkHips(1) != 0;
			}

			if (changed)
			{
				content.Append(HipChangeText(target, hipData));
				return true;
			}
			else
			{
				return false;
			}
		}

		private static bool ChangeButt(Creature target, StringBuilder sb)
		{
			var buttData = target.butt.AsReadOnlyData();
			bool changed = false;
			if (target.butt.size < 5)
			{
				changed = target.butt.GrowButt(1) != 0;
			}
			else if (target.butt.size > 5)
			{
				changed = target.butt.ShrinkButt(1) != 0;
			}

			if (changed)
			{
				sb.Append(ButtChangeText(target, buttData));
				return true;
			}
			else
			{
				return false;
			}
		}

		private static string ButtChangeText(Creature target, ButtData oldButt)
		{
			string verb = target.butt.size - oldButt.size > 0 ? "enlarge" : "shrink";

			return "You wiggle around in your gown, the pleasant feeling of flower petals rubbing against your skin washes over you." +
				" The feeling settles on your " + oldButt.ShortDescription() + "." + Environment.NewLine + "You feel it slowly " +
				verb + ". <b>You now have a " + target.build.ButtShortDescription() + ".</b>" + Environment.NewLine;
		}

		private static bool ChangeCock(Creature target, StringBuilder sb)
		{
			if (HyperHappySettings.isEnabled)
			{
				return false;
			}

			//find the largest cock. shrink it. if it gets small enough to remove it, do so. if it's the only cock the creature has, grow a vagina in its place.
			if (target.hasCock)
			{
				var oldGenitals = target.genitals.AsReadOnlyData();
				var largest = target.genitals.LongestCock();

				if (largest.DecreaseLengthAndCheckIfNeedsRemoval(Utils.Rand(3) + 1))
				{
					target.genitals.RemoveCock(largest);

					if (!target.hasCock && !target.hasVagina)
					{
						target.AddVagina(.25f);
						target.IncreaseCorruption();

						var oldBalls = target.balls.AsReadOnlyData();
						target.balls.RemoveAllBalls();
					}
				}

				sb.Append(CockChangedText(target, oldGenitals, largest.cockIndex));
				return true;
			}
			return false;
		}

		private static string CockChangedText(Creature target, GenitalsData oldGenitalData, int changedCock)
		{
			bool grewVagina = oldGenitalData.vaginas.Count == 0 && target.vaginas.Count > 0;
			bool lostBalls = oldGenitalData.balls.hasBalls && !target.balls.hasBalls;
			bool lostCock = oldGenitalData.cocks.Count > target.cocks.Count;

			CockData previousCockData = oldGenitalData.cocks[changedCock];

			return "Your " + previousCockData.LongDescription() + " feels strange as it brushes against the fabric of your gown." + Environment.NewLine +
				target.genitals.allCocks.GenericChangeOneCockLengthText(target, .


		}



		//private static string CockChangeText(Creature target, CockData )

		//				case "cock":
		//					sb.Append("Your [cock] feels strange as it brushes against the fabric of your gown." + Environment.NewLine);
		//					(new BimboProgression).shrinkCock();
		//changed = true;
		//					break;

		//				case "breasts":
		//					sb.Append("You feel like a beautful flower in your gown. Dawn approaches and you place your hands on your chest"
		//					   + " as if expecting your nipples to bloom to greet the rising sun." + Environment.NewLine);

		//					if (wearer.bRows() > 1)
		//					{
		//						sb.Append("Some of your breasts shrink back into your body leaving you with just two.");
		//						wearer.breasts.Count = 1;
		//					}

		//					if (wearer.breastRows[0].breastRating != CupSize.D)
		//					{
		//						if (wearer.breastRows[0].breastRating > CupSize.D)
		//						{
		//							sb.Append(Environment.NewLine + "A chill runs against your chest and your boobs become smaller.");
		//						}
		//						else
		//						{
		//							sb.Append(Environment.NewLine + "Heat builds in chest and your boobs become bigger.");
		//						}
		//						sb.Append(Environment.NewLine + "<b>You now have [breasts]</b>");
		//						wearer.breastRows[0].breastRating = CupSize.D;
		//						changed = true;
		//					}
		//					break;

		//				case "girlyness":
		//					text = wearer.modFem(70, 2);
		//					if (text == "")
		//					{
		//						break;
		//					}

		//					sb.Append("You run your [hands] across the fabric of your Gown, then against your face as it feels like"
		//				   + " there is something you need to wipe off." + Environment.NewLine);
		//					sb.Append(text);
		//					changed = true;
		//					break;

		//				default:
		//					// no error, intended
		//			}




		private static string HipChangeText(Creature target, HipData oldHips)
		{
			string verb = target.hips.size - oldHips.size > 0 ? "enlarge" : "shrink";

			return "You wiggle around in your gown, the pleasant feeling of flower petals rubbing against your skin washes over you." +
				" The feeling settles on your " + oldHips.ShortDescription() + "." + Environment.NewLine + "You feel them slowly " +
				verb + ". <b>You now have " + target.build.HipsShortDescription() + "</b>." + Environment.NewLine;

			;
		}


		protected readonly bool allowsMultipleTransformations;

		protected DryadTransformations() : this(true)
		{

		}

		protected DryadTransformations(bool allowMultipleTransformations)
		{
			this.allowsMultipleTransformations = allowMultipleTransformations;
		}



		//a helper that gets the currently set hyper happy flag for this game session. generally useful, but feel free to remove this if you don't need it.
		private bool hyperHappy => HyperHappySettings.isEnabled;


		protected internal override string DoTransformation(Creature target, out bool isBadEnd)
		{
			isBadEnd = false;
			StringBuilder sb = new StringBuilder();

			sb.Append(InitialTransformationText(target));

			//dryad is weird, because it's not written like literally every other tf out there: it may only let one change occur at a time,
			//and there are no requirements for a change to occur, unlike the stacking mechanic most other tfs have. additionally, the effects
			//are randomized instead of sequential. finally, it will not check to see if it can do a change before attempting it, but instead try
			//it, then handle any flavor text according to whether or not it did something.
			//So, i've come up with what i think is the best way to handle this:

			//we randomly select a number of simple changes to run. this starts at 0, but is affected by creature perks and two rolls.
			//if we are only allowing one change, this value is 1.
			int simpleChangeCount = this.allowsMultipleTransformations ? GenerateChangeCount(target, new int[] { 3, 4 }, 0, 1) : 1;
			int remainingChanges = simpleChangeCount;

			//then, we randomly select a simple change and do it, and decrease our remaining simple changes. we repeat this process until the remaining simple changes is 0.
			//any repeats that we hit are ignored, as are any simple changes that cannot occur because the creature already has the desired value.

			//if we have done any simple changes and we don't allow multiple tfs, immediately exit.
			if (!allowsMultipleTransformations && remainingChanges != simpleChangeCount)
			{
				return ApplyChangesAndReturn(target, sb, 0);
			}

			//otherwise, we go on to the complicated effects. we will carry over any simple changes that were ignored, up to 2, using the following rules:
			//if we don't allow multiple transformations or didn't ignore any simple tfs, don't carry anything over.
			//if we succeeded in doing any simple tf but have more we could have done, carry over 1.
			//if we failed to do any simple tfs, carry over 2 (or 1, if we only could have done 1)

			//the number of full changes to do. starts at 1, could go up to 3 if no simple changes occured.
			int fullChangeCount = 1;

			//if we've done one or no changes, and would have done more if possible.
			if (allowsMultipleTransformations && remainingChanges > 0 && remainingChanges + 1 >= simpleChangeCount)
			{
				//no simple changes, and we were attempting to do 2 or more.
				if (remainingChanges == simpleChangeCount && simpleChangeCount >= 2)
				{
					fullChangeCount += 2;
				}
				//we did at least one. only carryover 1.
				else
				{
					fullChangeCount += 1;
				}
			}

			//reset our change count remaining. remember, simple tfs are free.
			remainingChanges = fullChangeCount;

			//then repeat the process with RNG again, but with full tfs.

			//apply the changes and return.
			return ApplyChangesAndReturn(target, sb, simpleChangeCount - remainingChanges);




			//progress slowly to the ideal dryad build


			//Add any free changes here - these can occur even if the change count is 0. these include things such as change in stats (intelligence, etc)
			//change in height, hips, and/or butt, or other similar stats.

			//this will handle the edge case where the change count starts out as 0.
			if (remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, simpleChangeCount - remainingChanges);

			string tfChoice = Utils.RandomChoice("skin", "ears", "face", "lowerbody", "arms", "hair");
			sb.Append(GlobalStrings.NewParagraph());
			switch (tfChoice)
			{
				case "ears":
					if (wearer.ears.type != EarType.ELFIN)
					{
						sb.Append("There is a tingling on the sides of your head as your ears change to pointed elfin ears.");
						wearer.ears.type = EarType.ELFIN;
					}
					break;

				case "skin":
					if (wearer.skin.type != Skin.PLAIN)
					{
						sb.Append("A tingling runs up along your [skin] as it changes back to normal");
					}
					else if (wearer.skin.tone != "bark" && wearer.skin.type == Skin.PLAIN)
					{
						sb.Append("Your skin hardens and becomes the consistency of tree's bark.");
						wearer.skin.tone = "woodly brown";
						wearer.skin.type = Skin.BARK;
					}
					break;

				case "lowerbody":
					if (wearer.lowerBody.type != LowerBodyType.HUMAN)
					{
						sb.Append("There is a rumbling in your lower body as it returns to a human shape.");
						wearer.lowerBody.type = LowerBodyType.HUMAN;
					}
					break;

				case "arms":
					if (wearer.arms.type != ArmType.HUMAN || wearer.arms.claws.type != Claws.NORMAL)
					{
						sb.Append("Your hands shake and shudder as they slowly transform back into normal human hands.");
						wearer.arms.restore();
					}
					break;

				case "face":
					if (wearer.face.type != FaceType.HUMAN)
					{
						sb.Append("Your face twitches a few times and slowly morphs itself back to a normal human face.");
						wearer.face.type = FaceType.HUMAN;
					}
					break;

				case "hair":
					if (wearer.hair.type != Hair.LEAF)
					{
						sb.Append("Much to your shock, your hair begins falling out in tuffs onto the ground. "
						   + " Moments later, your scalp sprouts vines all about that extend down and bloom into leafy hair.");
						wearer.hair.type = Hair.LEAF;
					}
					break;

				default:
					sb.Append(Environment.NewLine + "ERROR: this forest gown TF choice shouldn't ever get called.");
			}

			//Any transformation related changes go here. these typically cost 1 change. these can be anything from body parts to gender (which technically also changes body parts,
			//but w/e). You are required to make sure you return as soon as you've applied changeCount changes, but a single line of code can be applied at the end of a change to do
			//this for you.

			//paste this line after any tf is applied, and it will: automatically decrement the remaining changes count. if it becomes 0 or less, apply the total number of changes
			//underwent to the target's change count (if applicable) and then return the StringBuilder content.
			//if (--remainingChanges <= 0) return ApplyChangesAndReturn(target, sb, changeCount - remainingChanges);




			//this is the fallthrough that occurs when a tf item goes through all the changes, but does not proc enough of them to exit early. it will apply however many changes
			//occurred, then return the contents of the stringbuilder.
		}

		//the abstract string calls that you create above should be declared here. they should be protected. if it is a body part change or a generic text that has already been
		//defined by the base class, feel free to make it virtual instead.
		protected abstract string InitialTransformationText(Creature target);
	}
}