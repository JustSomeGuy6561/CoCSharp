using System;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Strings;
using CoC.Backend.Tools;

namespace CoC.Frontend.Items.Consumables.Eggs
{
	/**
	 * @since March 31, 2018
	 * @author Stadler76
	 */
	//MOD: JSG - they now take into account dick-nipples. Dick-nipples aren't widely supported (yet), but could in theory be implemented further.
	//this now handles dick nipples.
	public class WhiteEgg : EggBase
	{
		public WhiteEgg(bool large) : base(large)
		{ }
		public override string AbbreviatedName()
		{
			return isLarge ? "L.WhtEgg" : "WhiteEgg";
		}

		public override string ItemName()
		{
			return isLarge ? "large white egg" : "white egg";
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string itemText = count == 1 ? "egg" : "eggs";
			string adjective = isLarge ? "large white " : "milky-white ";
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";

			return countText + adjective + itemText;
		}



		public override string Color()
		{
			return Tones.WHITE.AsString(false);
		}

		public override bool CanUse(Creature target, bool currentlyInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		protected override int monetaryValue => DEFAULT_VALUE;

		public override bool EqualsIgnoreSize(EggBase other)
		{
			return other is WhiteEgg;
		}

		private string GrewNipplesText(Creature source, double amountGrown)
		{
			string armorText = source.UpperBodyArmorShort(false);
			if (armorText is null)
			{
				armorText = "Your exposed nipples harden, suddenly sensitive to the slightest wind";
			}
			else
			{
				armorText = "Your nipples engorge, prodding hard against the inside of your " + armorText;
			}

			string measure;
			if (amountGrown < 0.25)
			{
				measure = Measurement.UsesMetric ? "a little over half a centemeter" : "almost a quarter-inch";
			}
			else
			{
				measure = Measurement.UsesMetric ? "almost a full centimeter" : "over a quarter-inch";
			}


			return armorText + ". Abrubtly, you realize they've grown " + measure + ".";

		}

		protected override string OnConsumeAttempt(Creature consumer, out bool consumeItem, out bool isBadEnd)
		{
			isBadEnd = false;
			StringBuilder sb = new StringBuilder();
			// kGAMECLASS.temp was a great idea ... *cough, cough* ~Stadler76
			//MOD NOTE: i mean... technically it conserves cycles because the memory isn't being allocated all the time. at the cost of being stored all the time.
			//and being dynamic because actionscript is a bitch. ummmm... yeah, i got nothing. jokes aside, that's terrible. i need a shower now. -JSG
			sb.Append("You devour the egg, momentarily sating your hunger.");



			double sizeDelta = isLarge ? (Utils.Rand(2) + 3) / 10.0 : 0.2;
			//MOD: Clear DickNipples. become fuckable if long enough.
			//if not, grow them slightly (but not enough to force them to be fuckable)
			//and let their current size determine their state.
			if (consumer.genitals.dickNipplesEnabled)
			{
				NippleStatus oldStatus = consumer.genitals.nippleType;
				consumer.genitals.SetDickNippleFlag(false);
				//grow them slightly so the flavor text makes sense.
				if (consumer.genitals.nippleLength + sizeDelta < 3)
				{
					consumer.genitals.GrowNipples(sizeDelta, true);
				}
				else if (consumer.genitals.nippleLength < 3)
				{
					consumer.genitals.SetNippleLength(3);
				}
				sb.Append(ClearedDickNipplesFlag(consumer, oldStatus));
			}
			else
			{
				//Grow nipples
				if (consumer.genitals.nippleLength < BreastCollection.FUCKABLE_NIPPLE_THRESHOLD && consumer.genitals.BiggestCupSize() > CupSize.FLAT)
				{
					double grownAmount = consumer.genitals.GrowNipples(sizeDelta);
					consumer.DeltaCreatureStats(lus: 15);
					sb.Append(GrewNipplesText(consumer, grownAmount));

				}
				//if large and at fuckable threshold (or above and somehow not fuckable) make fuckable.
				if (isLarge && consumer.genitals.nippleLength >= BreastCollection.FUCKABLE_NIPPLE_THRESHOLD && consumer.genitals.nippleType != NippleStatus.FUCKABLE)
				{
					if (consumer.genitals.SetNippleStatus(NippleStatus.FUCKABLE))
					{
						//Talk about if anything was changed.
						sb.Append(GlobalStrings.NewParagraph() + "Your " + consumer.genitals.AllBreastsLongDescription() + " tingle with warmth " +
							"that slowly migrates to your nipples, filling them with warmth. You pant and moan, rubbing them with your fingers."
							 + " A trickle of wetness suddenly coats your finger as it slips inside the nipple. Shocked, you pull the finger free."
							 + " <b>You now have fuckable nipples!</b>");
					}
				}
				//otherwise, make them inverted if not already
				else if (isLarge &&
					(consumer.genitals.nippleLength >= BreastCollection.FULLY_INVERTED_THRESHOLD && consumer.genitals.nippleType != NippleStatus.SLIGHTLY_INVERTED)
					|| (consumer.genitals.nippleLength < BreastCollection.FULLY_INVERTED_THRESHOLD && consumer.genitals.nippleType != NippleStatus.FULLY_INVERTED))
				{
					NippleStatus target = consumer.genitals.nippleLength < BreastCollection.FULLY_INVERTED_THRESHOLD ? NippleStatus.FULLY_INVERTED : NippleStatus.SLIGHTLY_INVERTED;
					if (consumer.genitals.SetNippleStatus(target))
					{
						sb.Append(GlobalStrings.NewParagraph() + "Your " + consumer.genitals.AllBreastsLongDescription() + " tingle with warmth " +
							"that slowly migrates to your nipples, filling them with warmth. Surprisingly, they've drawn inward, so that they are " +
							(target == NippleStatus.FULLY_INVERTED ? "fully" : "partially") + "hidden within your breasts. The whole thing turns you on immensely, and" +
							"You can't help but rub and prod <b>your newly inverted nipples</b>. Your continued stimulation causes them to pop free, so you'll still be able to " +
							"do whatever you'd like with them, which is certainly a bonus.");
					}
				}

			}
			consumeItem = true;
			return sb.ToString();
		}

		private string ClearedDickNipplesFlag(Creature consumer, NippleStatus oldStatus)
		{
			//new status is fuckable.
			if (oldStatus == NippleStatus.DICK_NIPPLE)
			{
				//Talk about if anything was changed.
				return GlobalStrings.NewParagraph() + "Your massive dick-nipples tingle with sudden need, causing you to stroke them vigorously. As you do, " +
					"they begin to flatten, trading width for length. Undeterred, you switch to rubbing them, and occasionally prod their rapdidly widening opening. " +
					"Your efforts are rewarded as your nipples become wet, but it's not exactly what you were expecting. It's not until your fingers begin to slip " +
					"into your nipples that you realize the full extent of the change: <b>your dick-like nipples have shifted to become fuckable instead!</b>" +
					"It seems the egg has made it so your nipples prefer to be penetrated, instead of doing the penetrating.";
			}
			//old was not dick nipple (likely normal), and are now longer than fuckable threshold, so when we flipped the flag, they updated and became fuckable.W
			else if (consumer.genitals.nippleType == NippleStatus.FUCKABLE)
			{
				//Talk about if anything was changed.
				return GlobalStrings.NewParagraph() + "Your " + consumer.genitals.AllBreastsLongDescription() + " tingle with warmth " +
					"that slowly migrates to your nipples, filling them with warmth. You pant and moan, rubbing them with your fingers."
					 + " A trickle of wetness suddenly coats your finger as it slips inside the nipple. Shocked, you pull the finger free."
					 + " <b>You now have fuckable nipples!</b>. It seems the egg has made it so your nipples prefer to be penetrated, instead of doing the penetrating.";
			}
			//else, they weren't long enough to become fuckable, so we grew them slightly. hint that dick nipples are no longer available.
			else
			{
				return GlobalStrings.NewParagraph() + "Your " + consumer.genitals.AllBreastsLongDescription() + " tingle with warmth " +
					"that slowly migrates to your nipples, filling them with warmth. You pant and moan, rubbing them with your fingers." +
					" Your continued stimulation is eventually rewarded, as your nipples harden and enlongate slightly. You're certain something else changed, but you're " +
					"not entirely sure what. Perhaps if they were longer, they might act differently?";
			}
		}

		public override byte sateHungerAmount => isLarge ? (byte)60 : (byte)20;
	}
}
