/**
 * Created by aimozg on 16.01.14.
 */
namespace CoC.Frontend.Items.Wearables.Armor
{
	using System.Text;
	using CoC.Backend.Creatures;
	using CoC.Backend.Items.Wearables.Armor;
	using CoC.Backend.Strings;
	using CoC.Backend.Tools;
	using CoC.Frontend.Creatures.NPCs;

	public sealed class GooArmor : ArmorBase
	{

		public override string AbbreviatedName() => "GooArmr";

		public override string ItemName() => "Goo Armor";

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			if (count == 0)
			{
				return "non-existent, gooey armor";
			}
			else if (count == 1)
			{
				return "Valeria, the goo-girl armor";
			}
			else
			{
				return (displayCount ? Utils.NumberAsText(count) : "") + "cloned copies of valeria and her gooey armor. Congrats, you broke the game.";
			}
		}

		protected override int monetaryValue => 1;

		public override string AboutItem()
		{
			return "This shining suit of platemail is more than just platemail - it houses the goo-girl, Valeria! Together, they provide one tough defense, " +
				"but you had better be okay with having goo handling your junk while you fight if you wear this!";
		}

		public GooArmor() : base(ArmorType.HEAVY_ARMOR)
		{
		}

		protected override string EquipText(Creature wearer)
		{
			StringBuilder sb = new StringBuilder();
			//Produces any text seen when equipping the armor normally
			sb.Append("With an ecstatic smile, the goo-armor jumps to her feet and throws her arms around your shoulders. \"" +
				SafelyFormattedString.FormattedText("Oh, this is going to be so much fun! Thank you thank you thank you! I promise I'll keep you nice and snug and safe, " +
				"don't you worry. Oooh, a real adventure again! WHEEE!", StringFormats.ITALIC) + "\"" + GlobalStrings.NewParagraph());

			sb.Append("Before she can get too excited, you remind the goo that she's supposed to be your armor right about now. Clasping her hands over her mouth in embarrassment, " +
				"she utters a muted apology and urges you to just \"" + SafelyFormattedString.FormattedText("put me on!", StringFormats.ITALIC) + "\" Awkwardly, " +
				"you strip out of your gear and open up the platemail armor and clamber in. It's wet and squishy, making you shudder and squirm as you squash your new friend " +
				"flat against the metal armor." + GlobalStrings.NewParagraph());

			sb.Append("Eventually, the two of you get situated. The goo-girl slips around your body inside the heavy armor, maneuvering so that your face is unobstructed " +
				"and your joints, not protected by the armor, are soundly clad in squishy goo. She even forms a gooey beaver on your new helm, allowing you to open " +
				"and close her like a visor in battle. Eventually, her goo settles around your ");

			if (wearer.hasVagina)
			{
				sb.Append(wearer.genitals.AllVaginasShortDescription());
			}

			if (wearer.hasVagina && wearer.hasCock)
			{
				sb.Append(" and ");
			}

			if (wearer.hasCock)
			{
				sb.Append(wearer.genitals.AllCocksShortDescription());
			}

			if (wearer.gender == 0)
			{
				sb.Append("groin");
			}

			sb.Append(", encasing your loins in case you need a little mid-battle release, she says." + GlobalStrings.NewParagraph());
			sb.Append("After a few minutes, you and your armor-friend are settled and ready to go.");

			return sb.ToString();

		}

#warning handle initial velaria meeting text where it's called.

		protected override string OnRemoveText()
		{ //Produces any text seen when removing the armor normally
			return "Valeria picks herself up and huffs, \"" + SafelyFormattedString.FormattedText("Maybe we can adventure some more later on?", StringFormats.ITALIC) +
				"\" before undulating off towards your camp." + GlobalStrings.NewParagraph() + "(" + SafelyFormattedString.FormattedText("Valeria now available in the " +
					"followers tab!", StringFormats.BOLD) + ")";
		}

		protected override void OnEquip(Creature wearer)
		{
#warning unlock achievement for wearing goo armor if not unlocked.
			Valeria.SetCampState(false);
		}

		protected override void OnRemove(Creature wearer)
		{
			Valeria.SetCampState(true);
		}

		public override bool Equals(ArmorBase other)
		{
			return other is GooArmor;
		}

		protected override bool destroyOnRemoval => true;

		public override float DefensiveRating(Creature wearer)
		{
			//Bonus from spar victories! Max +4.
			int bonus = 0;
			if (Valeria.sparIntensity >= 10)
			{
				bonus++;
			}

			if (Valeria.sparIntensity >= 30)
			{
				bonus++;
			}

			if (Valeria.sparIntensity >= 60)
			{
				bonus++;
			}

			if (Valeria.sparIntensity >= 100)
			{
				bonus++;
			}
			//Valeria fluids enabled?
			if (Valeria.fluidsEnabled)
			{
				if (Valeria.totalFluids < 50)
				{
					return 15 + Valeria.totalFluids / 5 + bonus;
				}
				else
				{
					return 25 + bonus;
				}
			}
			else
			{
				return 22 + bonus;
			}
		}
	}
}
