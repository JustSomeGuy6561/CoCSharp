//DrakesHeart.cs
//Description:
//Author: JustSomeGuy
//1/18/2020 5:50:32 PM

using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Tools;
using CoC.Frontend.Transformations;
using CoC.Frontend.UI;
using System;

namespace CoC.Frontend.Items.Consumables
{
	/**
	 * Moved out of classes.Scenes.NPCs.EmberScene
	 * @since December 11, 2016
	 * @author Stadler76
	 */
	 //MOD NOTE: removed ember's blood as a hidden item, instead implementing the transform in EmberScene. so the above comment is partially incorrect now.
	public sealed class DrakesHeart : StandardConsumable
	{
		public DrakesHeart() : base()
		{
		}

		public override string AbbreviatedName()
		{
			return "DrakeHart";
		}

		public override string ItemName()
		{
			return "Drake's Heart";
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string flowerText = count != 1 ? "flowers" : "flower";

			if (count == 1)
			{
				return $"a drake's flower flower";
			}
			else
			{
				return $"a bundle of {Utils.NumberAsText(count)} drake's heart {flowerText}";
			}
		}

		public override string AboutItem()
		{
			return "A rare, beautiful flower. It could make an exquisite perfume. According to a legend, dragons give this flower to the ones they intend to court.";
		}

		//does this consumable count as liquid for slimes and (kangaroo) diapause?
		public override bool countsAsLiquid => false;
		//does this consumable count as cum (i.e. for succubi)?
		public override bool countsAsCum => false;
		//how much hunger does consuming this sate?
		public override byte sateHungerAmount => 1;

		protected override int monetaryValue => 50;

		public override bool CanUse(Creature target, bool isInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		public override bool Equals(CapacityItem other)
		{
			return other is DrakesHeart;
		}
		protected override bool OnConsumeAttempt(Creature consumer, out string resultsOfUse, out bool isBadEnd)
		{
#warning when Ember implemented, set these to the correct values.
			var tf = new DragonTF(false, true);

			resultsOfUse = tf.DoTransformation(consumer, out isBadEnd);
			return true;
		}

		protected override bool OnCombatConsumeAttempt(CombatCreature consumer, CombatCreature opponent, out string resultsOfUse, out bool isBadEnd)
		{
#warning when Ember implemented, set these to the correct values.
			var tf = new DragonTF(false, true);

			resultsOfUse = tf.DoTransformationFromCombat(consumer, opponent, out isBadEnd);
			return true;
		}

		private sealed class DragonTF : DragonTransformations
		{
			public DragonTF(bool allowsDraconicFace, bool backUsesMane) : base(false, allowsDraconicFace, backUsesMane)
			{
			}

			protected override string InitialTransformationText(Creature target)
			{
				throw new NotImplementedException();
			}
		}

	}
}
