using System;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.Frontend.Perks.Endowment;
using CoC.Frontend.Transformations;

namespace CoC.Frontend.Items.Consumables
{


	/**
	 * @since March 30, 2018
	 * @author Stadler76
	 */
	public class SuccubisDelight : StandardConsumable
	{
		private readonly TransformationType transformation;

		bool isPurified => transformation == TransformationType.PURIFIED;
		bool isEnhanced => transformation == TransformationType.ENHANCED;

		public SuccubisDelight(TransformationType transformationType)
		{
			if (!Enum.IsDefined(typeof(TransformationType), transformationType))
			{
				transformationType = TransformationType.STANDARD;
			}
			transformation = transformationType;
		}

		public override string AbbreviatedName()
		{
			if (isPurified)
			{
				return "PSDelit";
			}
			else if (isEnhanced)
			{
				return "S.Dream";
			}
			else
			{
				return "SDelite";
			}
		}

		public override string ItemName()
		{
			if (isPurified)
			{
				return "Purified Sucubi's Delite";

			}
			else if (isEnhanced)
			{
				return "Succubi's Dream";
			}
			else
			{
				return "Succubi's Delite";
			}
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			//"a bottle of 'Succubus' Dream'";

			string bottleText = (count != 1 ? "bottles" : "bottle");
			string taintedText = isPurified ? "untainted " : "";
			string itemText = isEnhanced ? "\"Succubi's Dream\"" : "\"Succubi's Delite\"";

			string countText = "";
			if (displayCount && count == 1)
			{
				countText = isPurified ? "an " : "a ";
			}
			else if (displayCount)
			{
				countText = Utils.NumberAsPlace(count);
			}

			return $"{count}{taintedText}{bottleText} of {itemText}";
		}

		public override string AboutItem()
		{
			string end;
			if (isPurified)
			{
				end = ". It has been partially purified by Rathazul to prevent corruption.";
			}
			else if (isEnhanced)
			{
				end = ", though this batch has been enhanced by Lumi to have even greater potency.";
			}
			else
			{
				end = ".";
			}

			return "This precious fluid is often given to men a succubus intends to play with for a long time" + end;
		}

		//apprently enhanced is only worth DEFAULT_VALUE. ok.
		protected override int monetaryValue => isPurified ? 20 : DEFAULT_VALUE;

		public override bool CanUse(Creature target, bool currentlyInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		public override bool countsAsLiquid => true;
		public override bool countsAsCum => false;
		public override byte sateHungerAmount => 10;

		protected override bool OnConsumeAttempt(Creature consumer, out string resultsOfUse, out bool isBadEnd)
		{
			var tf = new BallsTf(transformation);
			resultsOfUse = tf.DoTransformation(consumer, out isBadEnd);
			return true;
		}

		public override bool Equals(CapacityItem other)
		{
			return other is SuccubisDelight delight && delight.transformation == transformation;
		}


		private class BallsTf : GenericTransformationBase
		{
			private readonly TransformationType transformation;

			bool isPurified => transformation == TransformationType.PURIFIED;
			bool isEnhanced => transformation == TransformationType.ENHANCED;

			public BallsTf(TransformationType transformationType)
			{
				transformation = transformationType;
			}

			protected internal override string DoTransformation(Creature target, out bool isBadEnd)
			{
				isBadEnd = false;

				double crit = 1;
				//Determine crit multiplier (x2 or x3)
				if (Utils.Rand(4) == 0)
				{
					crit += Utils.Rand(2) + 1;
				}

				int totalChanges = base.GenerateChangeCount(target, new int[] { 2, 2 });
				int changesPerformed = 0;

				StringBuilder sb = new StringBuilder();

				bool CheckExit()
				{
					if (changesPerformed >= totalChanges)
					{
						if (target.balls.hasBalls && Utils.Rand(3) == 0)
						{
							var oldData = target.femininity.AsReadOnlyData();
							target.femininity.ChangeFemininityToward(12, 3);
							sb.Append(target.femininity.FemininityChangedText(oldData));
						}
						return true;
					}

					return false;
				}

				//Generic drinking text
				sb.Append(InitialUseText(target));

				//Corruption increase
				if ((target.corruption < 50 || Utils.Rand(2) == 0) && !isPurified)
				{
					double temp;
					if (target.corruption >= 90)
					{
						temp = 0.5;
					}
					else if (target.corruption < 30)
					{
						temp = 4;
					}
					else if (target.corruption < 40)
					{
						temp = 3;
					}
					else if (target.corruption < 50)
					{
						temp = 2;
					}
					else
					{
						temp = 1;
					}
					temp = target.IncreaseCorruption(temp);
					sb.Append(IncreasedCorruptionText(temp));
				}

				//Grow new balls!
				if ((!target.balls.hasBalls || (isEnhanced && target.balls.count < 4)) && Utils.Rand(4) == 0)
				{
					if (!target.balls.hasBalls)
					{
						target.balls.GrowBalls();
						sb.Append(GrowBallsText(target));
					}
					else
					{
						var oldBalls = target.balls.AsReadOnlyData();
						//if needed, silently correct uniball.
						target.balls.MakeStandard();
						//add 2 balls to give us 4.
						target.balls.AddBalls(2);
						sb.Append(GetFourBallsText(target, oldBalls));
					}
					if (CheckExit()) return ApplyChangesAndReturn(target, sb, totalChanges);
				}

				//Makes your balls biggah! (Or cummultiplier higher if futa!)
				//MOD: fixed this to actually work.
				if (Utils.Rand(3) < 2 && target.balls.hasBalls)
				{
					var oldData = target.balls.AsReadOnlyData();
					target.balls.EnlargeBalls(1);

					sb.Append(EnlargedBallsText(target, oldData));

					target.DeltaCreatureStats(lib: 1, lus: 3);
					if (CheckExit()) return ApplyChangesAndReturn(target, sb, totalChanges);
				}

				//Boost cum multiplier
				if (Utils.Rand(2) == 0 && target.cocks.Count > 0 && target.genitals.cumMultiplier < 6)
				{
					//Temp is the max it can be raised to
					double temp = 3;
					//Lots of cum raises cum multiplier cap to 6 instead of 3
					if (target.HasPerk<MessyOrgasms>())
					{
						temp = 6;
					}

					if (temp >= target.genitals.cumMultiplier + .4 * crit)
					{
						var delta = target.genitals.IncreaseCumMultiplier(.4 * crit);
						//Flavor text

						sb.Append(IncreasedCumText(target, delta, crit > 1));

						target.IncreaseLibido();
						if (CheckExit()) return ApplyChangesAndReturn(target, sb, totalChanges);
					}
				}
				//Fail-safe
				if (changesPerformed == 0)
				{
					target.genitals.AddPentUpTime(100);
					sb.Append(ExtraSimulatedTime());
				}

				if (target.balls.hasBalls && Utils.Rand(3) == 0)
				{
					var oldData = target.femininity.AsReadOnlyData();
					target.femininity.ChangeFemininityToward(12, 3);
					sb.Append(target.femininity.FemininityChangedText(oldData));
				}

				return ApplyChangesAndReturn(target, sb, totalChanges);
			}



			private string IncreasedCumText(Creature target, double delta, bool crit)
			{
				string critText = crit ? " A bit of milky pre dribbles from your " + target.genitals.AllCocksShortDescription() + ", pushed out by the change." : "";
				if (!target.balls.hasBalls)
				{
					return GlobalStrings.NewParagraph() + "You feel a churning inside your body as something inside you changes." + critText;
				}

				else //if (target.balls.hasBalls)
				{
					return GlobalStrings.NewParagraph() + "You feel a churning in your " + target.balls.ShortDescription() +
						". It quickly settles, leaving them feeling somewhat more dense." + critText;
				}
			}

			private string InitialUseText(Creature target)
			{
				string corruptionText = "";
				//low corruption thoughts
				if (target.corruption < 33)
				{
					corruptionText = " This stuff is gross, why are you drinking it?";
				}
				//high corruption
				else if (target.corruption >= 66)
				{
					corruptionText = " You lick your lips, marvelling at how thick and sticky it is.";
				}
				return "You uncork the bottle and drink down the strange substance, struggling to down the thick liquid." + corruptionText;

			}

			private string IncreasedCorruptionText(double amount)
			{
				return GlobalStrings.NewParagraph() + "The drink makes you feel... dirty.";
			}

			private string GrowBallsText(Creature target)
			{
				return GrewBallsCommon(target, false);
			}

			private string GetFourBallsText(Creature target, BallsData oldBalls)
			{
				return GrewBallsCommon(target, true);
			}

			private string GrewBallsCommon(Creature target, bool four)
			{
				string armorText = target.LowerBodyArmorShort(true);

				if (armorText is null)
				{
					armorText = "eyes sealed shut from the pain. When you can finally open them, you can barely comprehend";
				}
				else
				{
					armorText = "struggling to pull open your " + armorText + ". In shock, you can barely process";
				}

				return GlobalStrings.NewParagraph() + "Incredible pain scythes through your crotch, doubling you over. You stagger around, " + armorText +
					" the sight before your eyes: <b>You have" + (four ? " four" : "") + " balls!</b>";
			}

			private string EnlargedBallsText(Creature target, BallsData oldBalls)
			{
				//Texts
				if (oldBalls.uniBall && !target.balls.uniBall)
				{
					return GlobalStrings.NewParagraph() + "A flash of warmth passes through you and a sudden weight develops in your groin. " +
						"You pause to examine the changes and your roving fingers discover your " + target.balls.ShortDescription() + " have grown larger, and have "
						+ "split back into the standard two-ball format.";
				}
				else if (target.balls.size <= 2)
				{
					return GlobalStrings.NewParagraph() + "A flash of warmth passes through you and a sudden weight develops in your groin. " +
						"You pause to examine the changes and your roving fingers discover your " + target.balls.ShortDescription() + " have grown larger than a human's.";
				}
				else //if (target.balls.size > 2)
				{
					return GlobalStrings.NewParagraph() + "A sudden onset of heat envelops your groin, focusing on your " + target.balls.SackDescription() +
						". Walking becomes difficult as you discover your " + target.balls.ShortDescription() + " have enlarged again.";
				}
			}

			private string ExtraSimulatedTime()
			{
				return (GlobalStrings.NewParagraph() + "Your groin tingles, making it feel as if you haven't cum in a long time.");
			}
		}



	}
}
