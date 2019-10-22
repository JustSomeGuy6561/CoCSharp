//TestItem.cs
//Description:
//Author: JustSomeGuy
//Note: date follows MMDDYYYY format.
//10/11/2019 10:18:06 PM

using CoC.Backend;
using CoC.Backend.BodyParts;
using CoC.Backend.BodyParts.BaseClasses;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.Backend.UI;
using CoC.Frontend.Engine;
using CoC.Frontend.UI;
using System;
using System.Text;

namespace CoC.Frontend.Items.Consumables
{
	//god, i hope this works lol. 
	//it's been reworked to work with as many different body parts as support it. 
	//undoubtedly, there will need to be some efforts to clean it up, but for now
	//the logic is all in place. unfortunately, anything that requires events
	//becomes a clusterfuck really quickly. 

	public sealed class BodyLotion : ConsumableWithMenuBase
	{
		private readonly SkinTexture lotionType;
		private string lotionStrLower => lotionType.AsString().ToLower();
		private string lotionStr => lotionType.AsString();

		public readonly SimpleDescriptor liquidDesc;
		public readonly DescriptorWithArg<string> applyDesc;

		public BodyLotion(SkinTexture texture, SimpleDescriptor liquidShortDescription, SimpleDescriptor liquidFullDescription,
			DescriptorWithArg<string> ApplyTextGeneric): base(() => Short(texture),
			() => Full(texture), GetDesc(texture, liquidFullDescription))
		{
			lotionType = texture;
			liquidDesc = liquidShortDescription ?? throw new ArgumentNullException(nameof(liquidShortDescription));
			applyDesc = ApplyTextGeneric ?? throw new ArgumentNullException(nameof(ApplyTextGeneric));
		}

		private static string Short(SkinTexture texture)
		{
			return texture.AsString() + "Ltn";
		}

		private static string Full(SkinTexture texture)
		{
			return "a flask of " + texture.AsString().ToLower() + " lotion";
		}

		private static SimpleDescriptor GetDesc(SkinTexture texture, SimpleDescriptor fullDesc)
		{
			if (fullDesc is null) throw new ArgumentNullException(nameof(fullDesc));
			return () => "A small wooden flask filled with a " + fullDesc() + " . A label across the front says, \"" + texture.AsString() + " Lotion.\"";
		}

		//does this consumable count as liquid for slimes?
		public override bool countsAsSlimeLiquid => true;
		//does this consumable count as cum for succubi?
		public override bool countsAsCum => false;
		//how much hunger does consuming this sate?
		public override byte sateHungerAmount => 0;

		protected override int monetaryValue => DEFAULT_VALUE;


		private UseItemCallback resumeFromThisMadness;
		private StandardDisplay display => DisplayManager.GetCurrentDisplay();

		private string QueryLocationText()
		{
			return "Where do you want to apply this lotion?";
		}

		private string CancelLotionContext()
		{
			return "You decided not to apply the " + base.fullName() + " after all";
		}

		private void TryMultiLotionable(Creature target, IMultiLotionable bodyPart, ButtonListMaker buttonMaker)
		{
			display.ClearOutput();

			display.OutputText("Try to lotion this shit idk fix me later");

			for (byte x = 0; x < bodyPart.numLotionableMembers; x++)
			{
				
				if (bodyPart.canLotion(x))
				{
					string overrideText = null;
					if (bodyPart is IMultiLotionableCustomText customText)
					{
						overrideText = customText.ApplySingleLotion(lotionType, x);
					}
					display.AddButton(x, bodyPart.buttonText(x), () => DoApplyLotion(target, q => bodyPart.attemptToLotion(q, x), bodyPart.locationDesc(x), overrideText));
				}
				else
				{
					display.AddButtonDisabled(x, bodyPart.buttonText(x));
				}
			}
		}

		private void ApplyLotion(Creature target, ILotionable bodyPart)
		{
			string overrideText = null;
			if (bodyPart is ILotionableCustomText customText)
			{
				overrideText = customText.ApplyLotion(this.lotionType);
			}
			DoApplyLotion(target, bodyPart.attemptToLotion, bodyPart.locationDesc(), overrideText);
		}

		private void DoApplyLotion(Creature target, Func<SkinTexture,bool> applyCallback, string locationDesc, string overrideText)
		{
			bool retVal = applyCallback(this.lotionType);

			display.ClearOutput();

			string result = null;
			if (!string.IsNullOrEmpty(overrideText))
			{
				result = overrideText;
			}
			else if (applyCallback(this.lotionType))
			{
				result = SuccessTextGeneric(target, locationDesc);
			}
			else
			{
				result = FailTextGeneric(target, locationDesc);
			}
			BodyLotion retItem = retVal ? null : this;

			resumeFromThisMadness(true, result, null);
		}

		private string SuccessTextGeneric(Creature target, string locationDesc)
		{
			StringBuilder sb = StartText(target, locationDesc);
			sb.Append(" Soon, " + applyDesc(locationDesc));
			return sb.ToString();
		}

		private string FailTextGeneric(Creature target, string locationDesc)
		{
			StringBuilder sb = StartText(target, locationDesc);
			sb.Append(" Strangely, nothing seems to happen. What a waste!");
			return sb.ToString();
		}

		private StringBuilder StartText(Creature target, string locationDesc)
		{
			StringBuilder sb = new StringBuilder();
			if (!target.wearingAnything)
			{
				sb.Append("You uncork the flask of lotion and rub ");
			}
			else
			{
				sb.Append("You take a second to disrobe before uncorking the flask of lotion and rubbing ");
			}
			sb.Append(" the " + liquidDesc() + " on your " + locationDesc + ".");
			if (target.body.primarySkin.skinTexture == lotionType)
			{
				 sb.Append(" Once you've finished you feel reinvigorated. ");
			}
			else
			{
				sb.Append(" As you rub the mixture into your " + locationDesc +", which begins to tingle pleasantly. ");
			}

			return sb;
		}

		public override bool CanUse(Creature target, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		//since we override the attempt to use, this is irrelevant. 
		protected override bool OnConsumeAttempt(Creature consumer, out string resultsOfUse)
		{
			resultsOfUse = null;
			return true;
		}

		protected override void BuildMenu(Creature target, UseItemCallback postItemUseCallback)
		{
			StandardDisplay display = DisplayManager.GetCurrentDisplay();
			resumeFromThisMadness = postItemUseCallback;

			display.ClearButtons();
			display.OutputText(QueryLocationText());

			ButtonListMaker buttonMaker = new ButtonListMaker(display);

			foreach (var bodyPart in target.bodyParts)
			{
				if (bodyPart is ILotionable lotionable)
				{
					buttonMaker.AddButtonToList(bodyPart.BodyPartName(), lotionable.canLotion(), () => ApplyLotion(target, lotionable));
				}
				else if (bodyPart is IMultiLotionable multiLotionable)
				{
					buttonMaker.AddButtonToList(bodyPart.BodyPartName() + "...", true, () => TryMultiLotionable(target, multiLotionable, buttonMaker));
				}
			}

			buttonMaker.CreateButtons(GlobalStrings.CANCEL(), true, () => resumeFromThisMadness(false, CancelLotionContext(), this));
		}
	}
}
