using System;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Strings;
using CoC.Backend.Tools;

namespace CoC.Frontend.Items.Consumables
{


	/**
	 * @since March 31, 2018
	 * @author Stadler76
	 */
	public class BrownEgg : StandardConsumable
	{
		public const int SMALL = 0;
		public const int LARGE = 1;

		private bool large;

		public BrownEgg(int type)
		{
			string id;
			string shortName;
			string longName;
			string description;
			int value;

			large = type == LARGE;

			switch (type)
			{
				case SMALL:
					id = "BrownEg";
					shortName = "BrownEg";
					longName = "a brown and white mottled egg";
					description = "This is an oblong egg, not much different from a chicken egg in appearance (save for the color)."
						   + " Something tells you it's more than just food.";
					value = DEFAULT_VALUE;
					break;

				case LARGE:
					id = "L.BrnEg";
					shortName = "L.BrnEg";
					longName = "a large brown and white mottled egg";
					description = "This is an oblong egg, not much different from an ostrich egg in appearance (save for the color)."
						   + " Something tells you it's more than just food.";
					value = DEFAULT_VALUE;
					break;

				default: // Remove this if someone manages to get SonarQQbe to not whine about a missing default ... ~Stadler76
			}

			super(id, shortName, longName, value, description);
		}

		public override bool CanUse(Creature target, bool currentlyInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		protected override bool OnConsumeAttempt(Creature consumer, out string resultsOfUse, out bool isBadEnd)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("You devour the egg, momentarily sating your hunger." + GlobalStrings.NewParagraph());
			if (!large)
			{
				sb.Append("You feel a bit of additional weight on your backside as your [butt] gains a bit more padding.");
				consumer.butt.size++;
);
			}
			else
			{
				sb.Append("Your [butt] wobbles, nearly throwing you off balance as it grows much bigger!");
				consumer.butt.size += 2 + Utils.Rand(3);

			}
			if (Utils.Rand(3) == 0)
			{
				if (large)
				{
					sb.Append(consumer.modThickness(100, 8));
				}
				else
				{
					sb.Append(consumer.modThickness(95, 3));
				}
			}

			return false;
		}
		public override byte sateHungerAmount => 60;
		public override byte sateHungerAmount => 20;
	}
}
