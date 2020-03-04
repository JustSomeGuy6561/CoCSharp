//using System;
//using System.Text;
//using CoC.Backend.BodyParts;
//using CoC.Backend.Creatures;
//using CoC.Backend.Items.Consumables;
//using CoC.Backend.Strings;
//using CoC.Backend.Tools;

//namespace CoC.Frontend.Items.Consumables
//{


//	/**
//	 * A refreshing icicle.
//	 */
//	public class IceShard : StandardConsumable
//	{
//		public IceShard() : base()
//		{ }

//		public override string AbbreviatedName() => "Icicle ";
//		public override string ItemName() => "Icicle";
//		public override string ItemDescription(byte count, bool displayCount = false)
//		{
//			throw;
//			//"an ice shard";
//		}
//		public override string AboutItem() => "An icicle that seems to be incapable of melting. It numbs your hands as you hold it. ";
//		protected override int monetaryValue => DEFAULT_VALUE;

//		public override bool countsAsLiquid => true;
//		public override bool countsAsCum => false;

//		public override bool CanUse(Creature target, bool currentlyInCombat, out string whyNot)
//		{
//			whyNot = null;
//			return true;
//		}

//		protected override string OnConsumeAttempt(Creature consumer, out bool consumeItem, out bool isBadEnd)
//		{
//			StringBuilder sb = new StringBuilder();
//			sb.Append("You give the icicle a tentative lick, careful not to stick your tongue to it. It tastes refreshing, like cool, clear glacier water. The ice readily dissolves against the heat of your mouth as you continue to lick away at it. Before long, the icicle has dwindled into a sliver small enough to pop into your mouth. As the pinprick of ice melts you slide your chilled tongue around your mouth, savoring the crisp feeling." + GlobalStrings.NewParagraph())
//					if (Utils.Rand(2) == 0 && (consumer.relativeStrength < 75 || consumer.relativeToughness < 75))
//			{
//				sb.Append("The rush of cold tenses your muscles and fortifies your body, making you feel hardier than ever. ");
//				if (consumer.relativeStrength < 75)
//				{
//					consumer.DeltaCreatureStats(str: ((1 + Utils.Rand(5)) / 5))
//}

//				if (consumer.relativeToughness < 75)
//				{
//					consumer.DeltaCreatureStats(tou: ((1 + Utils.Rand(5)) / 5))
//}
//			}
//			if (Utils.Rand(2) == 0 && (consumer.relativeSpeed > 25))
//			{
//				sb.Append("You feel a chill spread through you; when it passes, you feel more slothful and sluggish. ");
//				if (consumer.relativeSpeed > 25)
//				{
//					consumer.DeltaCreatureStats(spe: -((1 + Utils.Rand(5)) / 5))
//}
//			}
//			if (Utils.Rand(2) == 0)
//			{
//				sb.Append("You also feel a little numb all over, in more ways than one... ")
//				consumer.DeltaCreatureStats(lib: -((1 + Utils.Rand(2)) / 2))
//				consumer.DeltaCreatureStats(sens: -((1 + Utils.Rand(2)) / 2))
//			}


//			return false;
//		}
//		public override byte sateHungerAmount => 5;
//	}
//}
