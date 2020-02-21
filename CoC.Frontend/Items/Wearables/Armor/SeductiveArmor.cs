
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Items.Wearables.Armor;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Creatures.NPCs;
using CoC.Frontend.Items.Wearables.Piercings;
using CoC.Frontend.Perks;
/**
* Created by aimozg on 15.01.14.
*/
namespace CoC.Frontend.Items.Wearables.Armor
{

	public sealed class SeductiveArmor : ArmorBase
	{

		public override string AbbreviatedName() => "SeductA";

		public override string ItemName()
		{
			return "Scandelously Seductive Armor";
		}
		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string setText = count != 1 ? "sets" : "set";

			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";

			return $"{count}{setText} of scandalously seductive armor";
		}


		public override double PhysicalDefensiveRating(Creature wearer) => 0;

		protected override int monetaryValue => 1;

		public override string AboutItem() => "A complete suit of scalemail shaped to hug tightly against every curve, it has a solid steel chest-plate with obscenely " +
			"large nipples molded into it. The armor does nothing to cover the backside, exposing the wearer's cheeks to the world.";

		public SeductiveArmor() : base(ArmorType.HEAVY_ARMOR)
		{
		}

		protected override string EquipText(Creature wearer)
		{
			StringBuilder sb = new StringBuilder();
			if (!Ceraph.isSlave)
			{
				bool previouslyPierced = oldPiercings.IsPiercedAt(NipplePiercingLocation.LEFT_HORIZONTAL) || oldPiercings.IsPiercedAt(NipplePiercingLocation.RIGHT_HORIZONTAL);
				bool wasWearingJewelry = oldPiercings.WearingJewelryAt(NipplePiercingLocation.LEFT_HORIZONTAL) || oldPiercings.WearingJewelryAt(NipplePiercingLocation.RIGHT_HORIZONTAL);

				string nippleText = wearer.breasts.Count > 1 ? "uppermost set of nipples" : "nipples";
				sb.Append("After struggling to get it on, you feel a sudden shift in your scandalous new armor. To your horror, it begins folding into itself, revealing more " +
					"and more of your " + wearer.body.LongEpidermisDescription() + " and the comfortable underclothes you had on underneath it. The transforming armor " +
					"gradually covers less and less of you until it's little more than a pair of huge nipple-coverings and a silver chain. A loud KA-CHUNK startles you, " +
					"and then you're screaming as you feel something stabbing through your "+ nippleText +". Goosebumps cover your flesh as you twist in unexpected agony." +
					GlobalStrings.NewParagraph() + "After you've had a chance to recover, you quickly notice your armor is completely gone");

				if (wearer.wearingUpperGarment && wearer.wearingLowerGarment)
				{
					sb.Append(", though thankfully your undergarments remain, so you're not completely naked. ");
				}
				else if (wearer.wearingUpperGarment || wearer.wearingLowerGarment)
				{
					string item = wearer.wearingUpperGarment ? wearer.upperGarment.ItemName() : wearer.lowerGarment.ItemName();
					sb.Append("; your " + item + " is now the only thing keeping your from being completely nude. ");
				}
				else
				{
					sb.Append(", exposing your body for all the world to see. ");
				}

				if (wearer.wearingUpperGarment)
				{
					sb.Append("As you move your " + wearer.upperGarment.ItemName() + "aside to check your ");
				}
				else
				{
					sb.Append("As you inspect your ");
				}

				sb.Append("abused nipples, you realize you were wrong - instead of completely disappearing, it appears your armor has condensed down into " +
					"a set of seamless black nipple-studs running through your " + nippleText + ".");

				if (previouslyPierced)
				{
					sb.Append("Even though you aleady had your nipples pierced, it quickly dawns on you that this is something on an entirely different level - " +
						"it's almost as if your sensitive nubs have fused to the metal. No wonder it hurt so much!");

					if (wasWearingJewelry)
					{
						//YARE YARE DAZE!
						sb.Append("You're also not entirely sure what happened to your previous nipple jewelry, but you suppose that can't be helped.");
					}
				}
				else
				{
					sb.Append("You try to remove your brand-new piercings, to little avail. You can't even get them to budge - it's almost as if your nipples are now " +
						"fused to your new nipple-studs, and you have little doubt magic is involved.");
				}


				if (wearer.wearingUpperGarment)
				{
					sb.Append("Strangely, your " + wearer.upperGarment.ItemName() + "seems to be unaffected, as if your new piercings simply phased through it. " +
						"At least that's something, you note, as you rubs your still tender nipples gently. ");
				}
				else if (wearer.wearingLowerGarment)
				{
					sb.Append("You suddenly have the urge to take advantage of your sudden topless state, eager to show off your " + (previouslyPierced ? "brand-new piercings. "
						: "new nipple jewelry. "));
				}
				else
				{
					sb.Append("Your sudden nudity sends a thrill through your body, and you have the urge to show off " + (previouslyPierced ? "your brand-new piercings."
						: "your new nipple jewelry. "));
				}

				sb.Append("The source of your newfound predicament quickly dawns on you, and you mentally curse Ceraph for what she's done to you." + GlobalStrings.NewParagraph());
				sb.Append("As if summoned by your thoughts, you can hear her voice on the wind, taunting you again, \"<i>Enjoy your new bondage fetish, pet! " +
					"One more piercing and you'll be ready. Don't have too much fun being tied down and fucked, ok?</i>" + GlobalStrings.NewParagraph());
			}
			else
			{
				sb.Append("As you're trying to put on the armor, Ceraph appears from nowhere, apologizing profusely and stopping you before you can slide the last strap into place. \"<i>Please don't put that on, " + (wearer.genitals.AppearsMoreMaleThanFemale() ? "Master" : "Mistress") + ". I trapped that armor to pierce new fetishes the unwary so that I could add them to my harem. I'd hate to garner your anger.</i>\" She wrings her hands nervously. \"<i>If you'll hand it here, I'll get rid of it for you. Noone would buy it anyway.</i>\"");
				sb.Append(GlobalStrings.NewParagraph() + "You shrug and toss her the armor, disappointed that you're down a potentially sexy outfit.");
				sb.Append(GlobalStrings.NewParagraph() + "Ceraph bows gratefully and swiftly backpedals, offering, \"<i>And if you ever want me to stuff you full of magic fetishes, just ask, okay?</i>\"");
				sb.Append(GlobalStrings.NewParagraph() + "She's gone before you can reply. Sometimes she's more trouble than she's worth.");
			}

			return sb.ToString();
		}

		private ReadOnlyPiercing<NipplePiercingLocation> oldPiercings;

		protected override void OnEquip(Creature wearer)
		{
			if (!Ceraph.isSlave)
			{
				if (wearer.HasPerk<CeraphFetishesPerk>())
				{
					CeraphFetishesPerk ceraph = wearer.GetPerkData<CeraphFetishesPerk>();
					if (!ceraph.hasBondageFetish)
					{
						ceraph.SetStacks(2);
					}
				}
				else
				{
					wearer.AddPerk(new CeraphFetishesPerk(2));
				}

				oldPiercings = wearer.breasts[0].nipplePiercings.AsReadOnlyData();

				//give them horizontal nipple piercings on the first row (both sides), piercing them first if needed. Replace any existing piercings.
				wearer.breasts[0].nipplePiercings.EquipOrPierceAndEquip(NipplePiercingLocation.LEFT_HORIZONTAL, new CeraphNipplePiercings(), true);
				wearer.breasts[0].nipplePiercings.EquipOrPierceAndEquip(NipplePiercingLocation.RIGHT_HORIZONTAL, new CeraphNipplePiercings(), true);
			}

			//then silently remove the armor.
			wearer.RemoveArmorManual(out _);
		}

		//if you somehow procc this, it should destroy itself on removal.
		protected override ArmorBase OnRemove(Creature wearer)
		{
			return null;
		}

		public override bool Equals(ArmorBase other)
		{
			return other is SeductiveArmor;
		}
	}
}
