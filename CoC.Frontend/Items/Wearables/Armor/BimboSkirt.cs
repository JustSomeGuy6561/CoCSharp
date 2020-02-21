
using System;
using System.Text;
using CoC.Backend;
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Items.Wearables.Armor;
using CoC.Backend.Items.Wearables.LowerGarment;
using CoC.Backend.Items.Wearables.UpperGarment;
using CoC.Backend.Settings.Gameplay;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.Backend.UI;
using CoC.Frontend.Creatures;
using CoC.Frontend.UI;
/**
* Created by aimozg on 18.01.14.
*/
namespace CoC.Frontend.Items.Wearables.Armor
{
	public sealed class BimboSkirt : ArmorBase
	{
		//min lust adjusted here too.

#warning keep track of orgasm counts while worn. fire a reaction if corruption gets too high that removes undergarments.
		//by default, its effects are active. you can disable them when you equip it. you can also do it through the inventory menu.
		private bool active = true;
		private bool userPrompted = false;

		public override string AbbreviatedName() => "BimboSk";

		public override string ItemName()
		{
			return "Bimbo Skirt";
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";
			string skirtText = count != 1 ? "skirts" : "skirt";

			return $"{countText}{skirtText} that looks like it belongs on a bimbo";
		}
		public override double PhysicalDefensiveRating(Creature wearer) => 1;

		//slutty seduction bonus depends on undergarments.
		public override double BonusTeaseDamage(Creature wearer)
		{
			if (wearer.wearingLowerGarment && wearer.wearingUpperGarment)
			{
				return 0;
			}
			else if (wearer.wearingLowerGarment)
			{
				return 1;
			}
			else if (wearer.wearingUpperGarment)
			{
				return 3;
			}
			else
			{
				return 10;
			}
		}

		protected override int monetaryValue => 50;

		public override string AboutItem()
		{
			return "A tight, cleavage-inducing halter top and an extremely short miniskirt. The sexual allure of this item is undoubtable. " +
				(active ? "It's currently enchanted to affect the wearer, but you could probably disable it if you wanted."
				: "Its body-altering enchanment is currently disabled, though you could reactivate it with little effort.");
		}

		public BimboSkirt() : base(ArmorType.LIGHT_ARMOR)
		{ }

		protected override string EquipText(Creature wearer)
		{ //Produces any text seen when equipping the armor normally

			bool wornUpper = wearer.wearingUpperGarment;
			bool wornLower = wearer.wearingLowerGarment;

			string intro;
			if (userPrompted && active)
			{
				intro = "You decide to ignore the warnings about any body changing effects and change into your " + ItemName() +
					". As you slip it on, you realize just how good it feels and begin to wonder why anyone would want to prevent that.";
			}
			else if (userPrompted)
			{
				intro = "You decide it'd be best to heed the warnings about any body changing effects and utter the deactivation incantion before " +
					"equipping your " + ItemName() + ". It doesn't really feel any different from wearing anything else, though it is a little slutty. ";
			}
			else
			{
				intro = "You slip into your " + ItemName() + " without so much as a second thought. You can't help but notice just how good it feels on you. ";
			}

			//reset the user prompted flag so it will work next time.
			userPrompted = false;

			if (wornUpper && wornLower)
			{
				return intro + "You have some trouble getting it all the way on, and it quickly becomes obvious that it wasn't designed to be worn with a bra. Kinky. " +
					"After some minor adjustments, you're able to get it to fit, but you wouldn't mind removing your " + wearer.upperGarment.ItemName() +
					", either. You have similar trouble with your " + wearer.lowerGarment.ItemName() + ", though for an entirely different reason - " +
					"you probably won't be able to seduce many of your foes with your " + wearer.lowerGarment.ItemName() + " getting in the way. " +
					"After all, what's the point of wearing such a ludicrous outfit, if you can't take advantage of it? For a moment you consider taking your "
					+ wearer.lowerGarment.ItemName() + " off, but then decide against it. You can always change your mind later, if you really want to.";
			}

			StringBuilder sb = new StringBuilder(intro);

			if (!wornUpper)
			{
				CupSize largestCup = wearer.genitals.BiggestCupSize();

				if (largestCup >= CupSize.E_BIG)
				{
					sb.Append("The halter top clings tightly to your bustline, sending a shiver of pleasure through your body. You feel how your erect "
						+ wearer.breasts[0].ShortNippleDescription() + " protrude from the soft fabric of your beautiful dress, and the sensation makes you feel slightly dizzy. ");
					if (wearer.genitals.isLactating)
					{
						sb.Append("Droplets of milk leak from your aroused " + wearer.breasts[0].ShortNippleDescription() + "wettening the top of your dress.");
					}
				}
				else if (largestCup >= CupSize.DD)
				{
					sb.Append("The halter top clings to your bustline, sending a shiver of pleasure through your body. ");
					if (wearer.genitals.isLactating)
					{
						sb.Append("Droplets of milk leak from your aroused " + wearer.breasts[0].ShortNippleDescription() + "wettening the top of your dress.");
					}
				}
				else if (largestCup >= CupSize.B)
				{
					sb.Append("The halter top of your sluttish outfit snugly embraces your " + wearer.genitals.AllBreastsLongDescription() +
						". The sensation of the soft fabric on your bare " + wearer.breasts[0].ShortNippleDescription() + " makes you feel warm and sexy. ");
					if (wearer.genitals.isLactating)
					{
						sb.Append("The sensation causes milk to leak from your breasts, wetting the top of your dress.");
					}
				}
				else if (largestCup >= CupSize.A)
				{
					sb.Append("You feel how the soft fabric of your dress caresses your " + wearer.genitals.AllBreastsLongDescription() +
						". The sensation is very erotic and you touch your sensitive " + wearer.breasts[0].ShortNippleDescription() + ", feeling the spread of arousal. " +
						"You idly notice that the halter top of your whorish dress is somewhat loose, and it would feel much better if your breasts were bigger and suppler. ");
					if (wearer.genitals.isLactating)
					{
						sb.Append("Your nipples respond to your stimulation, leaking milk into the top of your dress. ");
					}
				}
				else
				{
					sb.Append("You feel rather stupid putting the top part on like this, but you're willing to bear with it. As you put it on, " +
						"you feel how the soft fabric of your dress touches your " + wearer.breasts[0].LongNippleDescription() + ", making them erect.");
				}
				sb.Append(GlobalStrings.NewParagraph());
			}

			if (!wornLower)
			{
				if (wearer.butt.size < 8)
				{
					sb.Append("The sensation of tender fabric clinging to your " + wearer.build.ButtLongDescription() + " arouses you immensely, as you smooth your skirt. ");
				}
				else
				{
					sb.Append("You can feel how the fine fabric of your sluttish skirt doesn't quite cover your [ass]");
					if (wearer.hips.size > 8)
					{
						sb.Append(", and how the smooth skirt is stretched by your [hips]. ");
					}
					else
					{
						sb.Append(". ");
					}
				}
				if (wearer.hasCock)
				{
					sb.Append("Your [cock] becomes erect under your obscene skirt, bulging unnaturally. ");
				}
				else if (wearer.hasVagina)
				{
					Vagina wettest = wearer.genitals.LargestVaginalByWetness();
					switch (wettest.wetness)
					{
						case VaginalWetness.SLAVERING:
							sb.Append("Your juice constantly escapes your " + wettest.LongDescription() + " and spoils your sexy skirt. ");
							break;
						case VaginalWetness.DROOLING:
							sb.Append("A thin stream of your girl-cum escapes your " + wettest.LongDescription() + " and spoils your skirt. ");
							break;
						case VaginalWetness.SLICK:
							sb.Append("Your " + wettest.LongDescription() + " becomes all tingly and wet under your slutty skirt. ");
							break;
					}
				}
				if (wearer.gender == 0)
				{
					sb.Append("Despite your lack of features, you indeed feel arousal all over your body. ");
				}
				sb.Append(GlobalStrings.NewParagraph());
			}

			return sb.ToString();
		}


		//we're going way tf buck to give this a menu when you equip it.

		protected override DisplayBase AttemptToUse(Creature target, UseItemCallbackSafe<ArmorBase> postItemUseCallbackSafe)
		{
			if (!CanUse(target, false, out string whyNot))
			{
				postItemUseCallbackSafe(false, whyNot, Author(), this);
				return null;
			}
			else
			{
				if (target.intelligence > 20)
				{
					return BuildMenu(target, postItemUseCallbackSafe);
				}
				else
				{
					active = true;
					ArmorBase retVal = ChangeEquipment(target, out string resultsOfUse);
					postItemUseCallbackSafe(true, resultsOfUse, Author(), retVal);
					return null;
				}
			}
		}

		private StandardDisplay BuildMenu(Creature wearer, UseItemCallbackSafe<ArmorBase> postItemCallback)
		{
			var display = new StandardDisplay();

			display.OutputText(WearBimboSkirt(wearer));

			display.AddButtonWithToolTip(0, GlobalStrings.ENABLE(), () => DoEquip(wearer, postItemCallback, true), BimboText(true), BimboTitle(true));
			display.AddButtonWithToolTip(1, GlobalStrings.DISABLE(), () => DoEquip(wearer, postItemCallback, false), BimboText(false), BimboTitle(false));

			return display;
		}

		private string WearBimboSkirt(Creature wearer)
		{
			StringBuilder sb = new StringBuilder();


			sb.Append("As you go to put on your " + ItemName() + ", a strange sensation makes you pause for a moment. ");

			if (wearer.intelligence < 40)
			{
				sb.Append("You get the feeling wearing this won't just make you look like a bimbo, it'll make you LOOK like a bimbo, too.");
			}
			else if (wearer.intelligence < 65)
			{
				sb.Append("You get the impression wearing this won't just make you look like a bimbo, it'll make you more bimbo-like, too.");
			}
			else
			{
				sb.Append("You're fairly certain this is an enchanted item, and will undoubtedly alter your body and mind until you fully embody what it is to be a bimbo." +
					"You're not really sure if that's something you want.");
			}
			sb.Append("Checking the tag confirms your suspicions. ");
			if (SillyModeSettings.isEnabled)
			{
				sb.Append("It reads: " + SafelyFormattedString.FormattedText("Made in Mareth. Machine wash cold. ", StringFormats.ITALIC));
			}
			else
			{
				sb.Append(" It also details a means to disable the effect. " + GlobalStrings.NewParagraph() + "Do you leave the bimbo-inducing effects " +
					"enabled, or do you disable them?");
			}

			return sb.ToString();
		}

		private string BimboTitle(bool isEnable)
		{
			return isEnable ? "Leave Enabled " : "Disable Bimbification";
		}

		private string BimboText(bool isEnable)
		{
			return (isEnable ? "Enable " : "Disable ") + "bimbo progression from wearing the " + ItemName() + ".";
		}

		private void DoEquip(Creature wearer, UseItemCallbackSafe<ArmorBase> postItemCallback, bool enabled)
		{
			active = enabled;
			this.userPrompted = true;

			ArmorBase retVal = ChangeEquipment(wearer, out string resultsOfUse);
			postItemCallback(true, resultsOfUse, Author(), retVal);
		}

		public override bool CanWearWithLowerGarment(Creature wearer, LowerGarmentBase lowerGarment, out string whyNot)
		{
			//means you're pure enough to pull it off, or you're not wearing any lower garments.
			if (SupportsUndergarment(wearer) || LowerGarmentBase.IsNullOrNothing(lowerGarment))
			{
				whyNot = null;
				return true;
			}
			else
			{
				whyNot = NotPureEnough(wearer, lowerGarment.ItemName());
				return false;
			}
		}

		public override bool CanWearWithUpperGarment(Creature wearer, UpperGarmentBase upperGarment, out string whyNot)
		{
			//means you're pure enough to pull it off, or you're not wearing any lower garments.
			if (SupportsUndergarment(wearer) || UpperGarmentBase.IsNullOrNothing(upperGarment))
			{
				whyNot = null;
				return true;
			}
			else
			{
				whyNot = NotPureEnough(wearer, upperGarment.ItemName());
				return false;
			}
		}

		private string NotPureEnough(Creature wearer, string garmentName)
		{
			string corruptionText = wearer.IsPureEnough(25)
				? " Wait a minute, did you really think that? This place must really be getting to you." : "";

			return "You could put your " + garmentName + " on, but... well, it'd be uncomfortable. Besides, why would you want to hide any more of your sexy body?" +
				"After all, you're not a prude." + corruptionText;
		}

		private bool SupportsUndergarment(Creature wearer) => wearer.IsPureEnough(10);

		protected override void OnEquip(Creature wearer)
		{
			wearer.IncreaseLust(5);

			CupSize largestCup = wearer.genitals.BiggestCupSize();

			if (!wearer.wearingUpperGarment)
			{
				wearer.HaveGenericTitOrgasm(0, false, false);

				if (largestCup < CupSize.E_BIG && largestCup >= CupSize.DD)
				{
					wearer.IncreaseLust(2);
				}
				else if (largestCup >= CupSize.B)
				{
					wearer.IncreaseLust(5);
				}
				else if (largestCup >= CupSize.A)
				{
					wearer.IncreaseLust(10);
				}
				else
				{
					wearer.IncreaseLust(15);
				}
			}
			if (!wearer.wearingLowerGarment)
			{
				var wettest = wearer.genitals.LargestVaginalByWetness();
				if (wearer.hasVagina && !wearer.hasCock && wettest.wetness > VaginalWetness.WET)
				{
					wearer.IncreaseLust(5);
				}

				wearer.HaveGenericAnalOrgasm(false, false);

				if (wearer.hasVagina)
				{
					wearer.HaveGenericVaginalOrgasm(wettest.vaginaIndex, false, false);
				}
			}
			wearer.HaveGenericOralOrgasm(true, false);
		}

		protected override ArmorBase OnRemove(Creature wearer)
		{
			active = true;

			return this;
		}

		public override bool CanWearWithUndergarments(Creature wearer, UpperGarmentBase upperGarment, LowerGarmentBase lowerGarment, out string whyNot)
		{
			bool wornUpper = !UpperGarmentBase.IsNullOrNothing(upperGarment);
			bool wornLower = !LowerGarmentBase.IsNullOrNothing(lowerGarment);

			if (!wearer.IsPureEnough(10) && (wornUpper || wornLower))
			{
				string output = "";
				output = "You could put on your " + ItemName() + " when you're currently wearing your ";
				if (wornUpper)
				{
					output += wearer.upperGarment.ItemName();
					wornUpper = true;
				}
				if (wornLower)
				{
					if (wornUpper)
					{
						output += " and ";
					}
					output += wearer.lowerGarment.ItemName();
				}
				output += ", but you get the feeling it'd be like, super uncomfortable. What's the point of wearing such a sexy, " +
					"revealing outfit if you're just gonna cover it all up? You're not a prude!";

				if (wearer.IsPureEnough(25))
				{
					output += " Wait a minute, did you really think that? This place must really be getting to you.";
				}

				whyNot = output;
				return false;
			}
			else
			{
				whyNot = null;
				return true;
			}
		}

		public override bool Equals(ArmorBase other)
		{
			return other is BimboSkirt;
		}

	}


}
