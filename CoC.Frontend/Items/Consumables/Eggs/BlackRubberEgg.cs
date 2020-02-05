//BlackRubberEgg.cs
//Description:
//Author: JustSomeGuy
//1/28/2020 1:24:07 AM

using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Perks;
using CoC.Frontend.Transformations; //use if this is an item that does a transformation. safe to remove if not.
using CoC.Frontend.UI; //used if the item has to deal with menus and such. safe to remove if not.
using System;
using System.Text;

namespace CoC.Frontend.Items.Consumables.Eggs
{
	public sealed partial class BlackRubberEgg : EggBase
	{
		public BlackRubberEgg(bool large) : base(large)
		{
		}

		//move these to a dedicated file withing the strings folder group. they're here to make initial development easier.
		public override string AbbreviatedName()
		{
			return isLarge ? "L.BlkEgg" : "BlackEgg";
		}

		public override string ItemName()
		{
			return (isLarge ? "Large " : "") + "Black Egg";
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			//if your text uses "an" as an article instead of "a", be sure to change that here.
			string countText = displayCount ? (count == 1 ? "a" : Utils.NumberAsText(count)) : "";
			string sizeText = isLarge ? "large " : "";
			string eggText = count == 1 ? "egg" : "eggs";

			return $"{count} {sizeText}black {eggText}";
		}

		public override string Appearance()
		{
			return "This is an oblong egg, not much different from a" + (isLarge ? "n ostrich" : " chicken") + " egg in appearance (save for the color)." +
				" Something tells you it's more than just food." + (isLarge ? " For all you know, it could turn you into rubber!" : "");
		}

		private string RestoredBodyText(BodyData oldBodyData)
		{
			throw new NotImplementedException();
		}

		private string GainedRubberySkinPerk(bool knewAboutBlackEggs)
		{
			throw new NotImplementedException();
		}

		private string NothingHappenedText()
		{
			throw new NotImplementedException();
		}

		private string StackedRubberySkinText()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		public override bool Equals(EggBase other)
		{
			return other is BlackRubberEgg && other.isLarge == this.isLarge;
		}

		public override bool EqualsIgnoreSize(EggBase other)
		{
			return other is BlackRubberEgg;
		}



		public override byte sateHungerAmount => (byte)(isLarge ? 60 : 20);


		protected override bool OnConsumeAttempt(Creature consumer, out string resultsOfUse, out bool isBadEnd)
		{
			//gives you unnaturally smooth skin.
			if (consumer.body.type != BodyType.HUMANOID || (consumer.body.primarySkin.skinTexture != SkinTexture.NONDESCRIPT &&
				consumer.body.primarySkin.skinTexture != SkinTexture.SMOOTH))
			{
				var oldBodyData = consumer.body.AsReadOnlyData();
				consumer.UpdateBody(BodyType.HUMANOID, SkinTexture.SMOOTH);

				resultsOfUse = RestoredBodyText(oldBodyData);
			}
			else if ((isLarge || Utils.Rand(3) == 0) && !consumer.HasPerk<RubberySkin>())
			{
				consumer.AddPerk<RubberySkin>();
				var knewAboutBlackEggs = false;
				if (consumer is IExtendedCreature extended)
				{
					knewAboutBlackEggs = extended.extendedData.knowsAboutBlackEggs;
					extended.extendedData.knowsAboutBlackEggs = true;
				}
				consumer.DeltaCreatureStats(sens: 8, lus: 10, corr: 2);

				resultsOfUse = GainedRubberySkinPerk(knewAboutBlackEggs);
			}
			else if (isLarge)
			{
				consumer.StackPerk<RubberySkin>();

				resultsOfUse = StackedRubberySkinText();
			}
			else
			{
				resultsOfUse = NothingHappenedText();
			}

			isBadEnd = false;
			return true;
		}

		public override string Color()
		{
			return Tones.BLACK.AsString();
		}
	}
}
