//BlackRubberEgg.cs
//Description:
//Author: JustSomeGuy
//1/28/2020 1:24:07 AM

using System;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Perks;
using CoC.Frontend.Transformations; //use if this is an item that does a transformation. safe to remove if not.
using CoC.Frontend.UI; //used if the item has to deal with menus and such. safe to remove if not.

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

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			//if your text uses "an" as an article instead of "a", be sure to change that here.
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";
			string sizeText = isLarge ? "large " : "";
			string eggText = count == 1 ? "egg" : "eggs";

			return $"{count}{sizeText}rubbery black {eggText}";
		}

		public override string AboutItem()
		{
			return base.AboutItem() + (isLarge ? " For all you know, it could turn you into rubber!" : "");
		}

		private string RestoredBodyText(Creature target, BodyData oldBodyData)
		{
			return target.body.RestoredText(oldBodyData);
		}

		private string SkinBecameSmoothText(Creature target, BodyData oldBodyData)
		{
			return "Your skin tingles, though you can't see any major changes. Rubbing your " + target.hands.ShortDescription() + "along it, you notice it's become " +
				"really smooth - unnatturally smooth, even. Any blemishes or rough spots have been stripped away, as have any other noticable characteristics. Strange";
		}

		private string GainedRubberySkinPerk(Creature consumer, bool knewAboutBlackEggs)
		{
			if (!knewAboutBlackEggs)
			{
				string corruptionText;
				if (consumer.corruption < 66)
				{
					corruptionText = ("Looking like this makes you feel like some kind of freak, but fortunately you can change back whenever you want.");
				}
				else
				{
					corruptionText = "You feel like some kind of sexy rubber-skinned love-doll, though you can go back to normal if you really want.";
				}

				return GlobalStrings.NewParagraph() + "Your already flawless smooth skin begins to tingle as it changes again. It becomes shinier as its texture " +
					"changes subtly. You gasp as you touch yourself and realize your skin is suddenly covered in a strange substance. " +
					"The stuff is everywhere, obsuring your vision and making it difficult to breathe. Just as you're about to pass out, something clicks in your mind and " +
					"you gain control of the strange substance, as if it were another muscle in your body, and it receeds into your skin. Cautiously, you let it once again, " +
					"and this time you retain your ability to see and breathe normally. It seems you can now have a thin layer of a latex-like substance coat your body, " +
					"like a second skin. " + corruptionText + " You have little doubt you'll find uses for this later, be it in combat or, more kinky situations, if you so choose. "
					+ Environment.NewLine + SafelyFormattedString.FormattedText("Perk gained: Rubbery Skin. You can now choose to cover your body in a thin layer of latex " +
					"in certain scenes for unique sexual interactions.", StringFormats.BOLD) + GlobalStrings.NewParagraph() +
					"Looks like you now know what rubber eggs can do to you, you note with a smirk.";
			}
			else
			{
				return "A familiar sensation runs through your skin, and you are once again covered in a thin layer of latex. this time, you're prepared, and you gain control " +
					"over it with little effort. It looks like " + SafelyFormattedString.FormattedText("you've regained the ability to coat your body in latex!",
					StringFormats.BOLD) + " You knew what these things could do to you, so this isn't really that surprising.";
			}

		}

		private string NothingHappenedText(bool hasRubberySkin)
		{
			if (isLarge && hasRubberySkin)
			{
				return "Thick layers or rubber once again flare out of your body, though it's not anything more than you're already accoustomed to. It seems you can't " +
					"strengthen your rubbery second layer anymore. what a pity.";
			}
			else if (hasRubberySkin)
			{
				return "The rubbery substance your body can produce flares out again, but beyond this slight flare-up, it seems these small eggs don't really do anything else."
					+ " You suppose if you really wanted to see if they'd do anything to you, you should use a large one instead.";
			}
			else
			{
				return "Your skin tingles, but beyond removing any battle scare or blemishes, nothing happens, and your skin remains as smooth as ever.";
			}
		}

		private string StackedRubberySkinText(RubberySkin rubberySkin)
		{
			return "The rubbery substance that grants you an extra outer layer flows out of you, though this time it's thicker and tougher than before. " +
				"Once you get it back under control, you experiment with your newly strengthened ability. You're now able to produce "
				+ "a thicker substance, granting you a little more protection, and you can probably use your rubbery second skin for longer and in kinkier ways.";
		}

		public override bool EqualsIgnoreSize(EggBase other)
		{
			return other is BlackRubberEgg;
		}

		public override byte sateHungerAmount => (byte)(isLarge ? 60 : 20);


		protected override string OnConsumeAttempt(Creature consumer, out bool consumeItem, out bool isBadEnd)
		{
			isBadEnd = false;
			consumeItem = true;

			//gives you unnaturally smooth skin.
			if (consumer.body.type != BodyType.HUMANOID || (consumer.body.primarySkin.skinTexture != SkinTexture.NONDESCRIPT &&
				consumer.body.primarySkin.skinTexture != SkinTexture.SMOOTH))
			{
				BodyData oldBodyData = consumer.body.AsReadOnlyData();
				if (consumer.body.type != BodyType.HUMANOID)
				{
					consumer.UpdateBody(BodyType.HUMANOID, SkinTexture.SMOOTH);
					return RestoredBodyText(consumer, oldBodyData);
				}
				else
				{
					consumer.body.ChangeAllSkin(SkinTexture.SMOOTH);

					return SkinBecameSmoothText(consumer, oldBodyData);
				}
			}
			else if ((isLarge || Utils.Rand(3) == 0) && !consumer.HasPerk<RubberySkin>())
			{
				consumer.AddPerk<RubberySkin>();
				bool knewAboutBlackEggs = false;
				if (consumer is IExtendedCreature extended)
				{
					knewAboutBlackEggs = extended.extendedData.knowsAboutBlackEggs;
					extended.extendedData.knowsAboutBlackEggs = true;
				}
				consumer.DeltaCreatureStats(sens: 8, lus: 10, corr: 2);

				return GainedRubberySkinPerk(consumer, knewAboutBlackEggs);
			}
			else if (isLarge && consumer.StackPerk<RubberySkin>())
			{
				return StackedRubberySkinText(consumer.GetPerkData<RubberySkin>());
			}
			else
			{
				return NothingHappenedText(consumer.HasPerk<RubberySkin>());
			}
		}



		//combat consume is identical to regular consume. no need to override it.

		public override string Color()
		{
			return Tones.BLACK.AsString();
		}
	}
}
