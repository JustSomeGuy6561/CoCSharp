//KangaFruit.cs
//Description:
//Author: JustSomeGuy
//1/24/2020 9:23:31 PM

using System;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Tools;
using CoC.Frontend.Transformations; //use if this is an item that does a transformation. safe to remove if not.
using CoC.Frontend.UI; //used if the item has to deal with menus and such. safe to remove if not.

namespace CoC.Frontend.Items.Consumables
{
	public sealed partial class KangaFruit : StandardConsumable
	{
		private bool isEnhanced;

		public KangaFruit(bool enhanced) : base()
		{
			isEnhanced = enhanced;
		}

		//move these to a dedicated file withing the strings folder group. they're here to make initial development easier.



		public override string AbbreviatedName()
		{
			return isEnhanced ? "MghtVg" : "KangaFt";
		}

		public override string ItemName()
		{
			return isEnhanced ? "Mighty Veggie" : "Kanga Fruit";
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{

			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";
			string enhancedText = isEnhanced ? "mightily enhanced " : "";

			string pieceText = count != 1 ? "pieces" : "piece";
			return $"{count}{enhancedText}{pieceText} of kanga fruit";
		}

		public override string AboutItem()
		{
			return "A yellow, fibrous, tubular pod. A split in the end reveals many lumpy, small seeds inside. The smell of mild fermentation wafts from them." +
				(isEnhanced ? " It glows slightly from Lumi's enhancements." : "");
		}

		public override bool Equals(CapacityItem other)
		{
			return other is KangaFruit kangaFruit && this.isEnhanced == kangaFruit.isEnhanced;
		}



		public override bool countsAsLiquid => false;
		public override bool countsAsCum => false;
		public override byte sateHungerAmount => 20;
		protected override int monetaryValue => DEFAULT_VALUE;

		public override bool CanUse(Creature target, bool isInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		protected override string OnConsumeAttempt(Creature consumer, out bool consumeItem, out bool isBadEnd)
		{
			KangarooTFs tf = new KangarooTFs(isEnhanced);
			consumeItem = true;
			return tf.DoTransformation(consumer, out isBadEnd);
		}
		protected override string OnCombatConsumeAttempt(CombatCreature consumer, CombatCreature opponent, out bool consumeItem, out bool isBadEnd)
		{
			KangarooTFs tf = new KangarooTFs(isEnhanced);
			consumeItem = true;
			return tf.DoTransformationFromCombat(consumer, opponent, out isBadEnd);
		}

		private class KangarooTFs : KangarooTransformations
		{
			public KangarooTFs(bool enhanced) : base(enhanced)
			{
			}

			protected override string InitialTransformationText(Creature target)
			{
				throw new NotImplementedException();
			}
		}


	}
}
