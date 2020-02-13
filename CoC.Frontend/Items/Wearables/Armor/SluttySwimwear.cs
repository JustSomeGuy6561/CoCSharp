/**
 * Created by aimozg on 18.01.14.
 */
namespace CoC.Frontend.Items.Wearables.Armor
{
	using System.Text;
	using CoC.Backend.BodyParts;
	using CoC.Backend.Creatures;
	using CoC.Backend.Items.Wearables.Armor;
	using CoC.Backend.Items.Wearables.LowerGarment;
	using CoC.Backend.Items.Wearables.UpperGarment;
	using CoC.Backend.Strings;
	using CoC.Backend.Tools;

	public sealed class SluttySwimwear : ArmorBase, IBulgeArmor, ISluttySeductionItem
	{
		private bool bulged;

		public override string AbbreviatedName() => "S.Swmwr";

		public override string ItemName() => "slutty swimwear";

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string bikiniText = count != 1 ? "bikinis" : "bikini";

			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";

			return $"{count}skimpy black {bikiniText}";
		}
		public override float DefensiveRating(Creature wearer) => 0;

		protected override int monetaryValue => 6;

		public override string AboutItem() => "An impossibly skimpy black bikini. You feel dirty just looking at it... and a little aroused, actually.";

		public bool supportsBulgeArmor => true;

		public SluttySwimwear() : base(ArmorType.LIGHT_ARMOR)
		{ }

		public byte SluttySeductionModifier(Creature wearer) => 6;

		public override bool CanWearWithUpperGarment(UpperGarmentBase upperGarment, out string whyNot)
		{
			if (!(upperGarment is null))
			{
				whyNot = GenericArmorIncompatText(upperGarment);
				return false;
			}
			else
			{
				whyNot = null;
				return true;
			}
		}

		public override bool CanWearWithLowerGarment(LowerGarmentBase lowerGarment, out string whyNot)
		{
			if (!(lowerGarment is null))
			{
				whyNot = GenericArmorIncompatText(lowerGarment);
				return false;
			}
			else
			{
				whyNot = null;
				return true;
			}
		}

		protected override void OnEquip(Creature wearer)
		{
			wearer.IncreaseLust(5);
			if (wearer.hasCock)
			{
				wearer.IncreaseLust(5);
			}

			if (wearer.genitals.BiggestCupSize() > CupSize.FLAT)
			{
				wearer.IncreaseLust(5);
			}
		}

		protected override string EquipText(Creature wearer)
		{ //Produces any text seen when equipping the armor normally
			StringBuilder sb = new StringBuilder();

			if (wearer.genitals.BiggestCupSize() <= CupSize.FLAT)
			{
				sb.Append("You feel rather stupid putting the top part on like this, but you're willing to bear with it. It could certainly be good for distracting. ");
			}
			else
			{
				sb.Append("The bikini top clings tightly to your bustline, sending a shiver of pleasure through your body. It serves to turn you on quite nicely. ");
				wearer.DeltaCreatureStats(lus: 5);
			}
			if (wearer.cocks.Count == 0)
			{
				sb.Append("The thong moves over your smooth groin, clinging onto your buttocks nicely. ");
				if (wearer.balls.hasBalls)
				{
					if (wearer.balls.size > 5)
					{
						sb.Append("You do your best to put the thong on, and while the material is very stretchy, it simply can't even begin to cover everything, and your " +
							wearer.balls.ShortDescription() + " hang out the sides, exposed. Maybe if you shrunk your male parts down a little...");
					}
					else
					{
						sb.Append("However, your testicles do serve as an area of discomfort, stretching the material and bulging out the sides slightly. ");
					}
				}
			}
			else
			{
				sb.Append("You grunt in discomfort, your " + wearer.genitals.AllCocksLongDescription() + " flopping free from the thong's confines. " +
					"The tight material rubbing against your " + (wearer.cocks.Count > 1 ? "dicks" : "dick") + " does manage to turn you on slightly. ");

				wearer.DeltaCreatureStats(lus: 5);
				var biggest = wearer.genitals.BiggestCockByArea();
				if (biggest.area>= 20 || wearer.balls.size > 5)
				{
					string exposedText = biggest.area >= 20
						? biggest.LongDescription() + " has popped out of the top, completely"
						: wearer.balls.ShortDescription() + " hang on the sides,";

					sb.Append("You do your best to put the thong on, and while the material is very stretchy, it simply can't even begin to cover everything, and your " +
						exposedText + " exposed. Maybe if you shrunk your male parts down a little...");
				}
			}

			sb.Append(GlobalStrings.NewParagraph());

			return sb.ToString();
		}

		public override bool Equals(ArmorBase other)
		{
			return other is SluttySwimwear;
		}

		protected override string OnRemoveText()
		{
			return this.GenericBulgeAwareRemoveText(bulged);
		}

		bool IBulgeArmor.isBulged => bulged;

		string IBulgeArmor.SetBulgeState(Creature wearer, bool bulgified)
		{
			if (bulged == bulgified)
			{
				return "";
			}
			bulged = bulgified;
			if (bulgified)
			{
				return "The miniscule piece of swimwear that doubles as a tent to your " + wearer.genitals.AllCocksLongDescription(out bool isPlural) + "begins to grow and " +
					"encapsulate " + (isPlural ? "them" : "it") + ", molding itself perfectly to your manhood.";
			}
			else
			{
				return "The swimwear begins to revert to it's normal form, once again struggling to even remotely contain your manhood";
			}
		}
	}
}
