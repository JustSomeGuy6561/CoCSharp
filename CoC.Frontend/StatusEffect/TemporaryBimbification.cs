//TemporaryBimbification.cs
//Description:
//Author: JustSomeGuy
//2/22/2020 8:03:16 PM

using System;
using System.Linq;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Perks;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.Frontend.Perks;

namespace CoC.Frontend.StatusEffect
{
	//a timed status effect making the user a bimbo temporarily.
	//forces the creature to have DD breasts or larger (first row only), a big butt, big hips, and a vagina.
	//note that these requirements persist as long as this effect is available.

	//since this is temporary, we will return the respective sizes to their lowest values obtained before or during this status effect
	//However, these are stored as relative values, so that any other tfs that increase the size will be respected. for example,
	//if the cup size goes from A to DD, we will store 4 (5-1 = 4). if the pc further increases cupsize via a tf, to E, we will subtract 4 from
	//E cup when this effect wears off, making the net result a B cup. Similarly, starting at A cup, if the pc undergoes a tf that gives them flat
	//breasts while under the effects of this, we will update the value stored with 5 (5-0 = 5), because Flat cup is smaller than A cup.

	//In addition, if the creature had a vagina before this effect took hold, but loses it from other means while this is still active, it will
	//be removed when this effect wears off.

	//This effect is automatically removed if the Bimbo perk is obtained while this is active.

	public sealed partial class TemporaryBimbification : TimedPerk
	{

		private const byte DEFAULT_TIMER = 2 * STACK_VALUE;
		private const byte STACK_VALUE = 4;

		private short cupSizeDelta = 0;
		private uint? vaginaCollectionID = null;
		private bool grewVagina => vaginaCollectionID is uint;
		private short buttSizeDelta = 0;
		private short hipsSizeDelta = 0;

		//used to prevent onRemove from doing extra work - if this is true, we do a quick exit.
		private bool clearedData = false;

		public TemporaryBimbification() : this(DEFAULT_TIMER)
		{ }

		public TemporaryBimbification(ushort initialTimeout) : base(initialTimeout)
		{ }

		protected override void OnActivation()
		{
			ClearData();
			MakeBimbo();
		}
		//called immediately after onactivation, if desired.
		public override string ObtainText()
		{
			StringBuilder sb = new StringBuilder();

			if (cupSizeDelta > 0)
			{
				string armorText = sourceCreature.UpperBodyArmorShort(false);
				if (armorText is null)
				{
					armorText = ", but when you grab your titties they start getting bigger! Wow";
				}
				else
				{
					armorText = "... And then your " + armorText + "get,s like, tighter; wow";
				}

				sb.Append(GlobalStrings.NewParagraph() + "You feel this, like, totally sweet tingling in your boobies" + armorText
					+ ", it seems like Niamh's booze is making your boobies grow! That's so awesome!"
					+ " You giggle and gulp down as much as you can... Aw; your boobies are <b>kinda</b> big now, but, like,"
					+ " you wanted great big bouncy sloshy boobies like Niamh has. That'd be so hot!");
			}
			if (grewVagina)
			{
				sb.Append(GlobalStrings.NewParagraph() + "You can feel ");
				if (sourceCreature.hasCock)
				{
					sb.Append("the flesh under your cock" + (sourceCreature.balls.hasBalls ? "and behind your " + sourceCreature.balls.ShortDescription() : ""));
				}
				else
				{
					sb.Append("the blank expanse of flesh that is your crotch");
				}
				sb.Append(" start to tingle and squirm... mmm... that feels nice! There's a sensation you, like, can't describe,"
					 + " and then your crotch feels all wet... but in a good, sticky sorta way. Oh, wow!"
					 + " <b>You've, like, just grown a new virgin pussy!</b> Awesome!");
			}

			if (buttSizeDelta > 0)
			{
				sb.Append(GlobalStrings.NewParagraph() + "Your butt jiggles deliciously - it feels like the bubbles from the drink are pushing out your plump rump,"
					+ " filling it like bagged sparkling wine! Your bubbly booty swells and inflates until it feels as airy as your head."
					+ " Like, this is soooo plush!");
			}

			return sb.ToString();
		}

		internal bool IncreaseEffect(byte stack = 1)
		{
			if (!(sourceCreature is null))
			{
				//increase duration for 4 hours per stack.
				timeWearsOff = timeWearsOff.Delta(STACK_VALUE * stack);
				sourceCreature.DeltaCreatureStats(spe: -2, lib: 1, lus: 10);
				return true;
			}
			else
			{
				return false;
			}
		}


		//called when time passes, and the effect is still active.
		protected override string OnStatusEffectTimePassing(byte hoursPassedSinceLastUpdate, out bool removeEffect)
		{
			if (sourceCreature.perks.HasPerk<BimBro>() && sourceCreature.GetPerkData<BimBro>().hasBimboEffect)
			{
				removeEffect = true;
				ClearData();
				return null;
			}

			removeEffect = false;

			StringBuilder sb = new StringBuilder();
			if (sourceCreature.breasts[0].cupSize < CupSize.DD)
			{ //Tits!
				sb.Append(Environment.NewLine + "<b>Your boobies like, get all big an' wobbly again! You'll have lots of fun now that your tits are back to being big, yummy knockers!</b>" + Environment.NewLine);
				sourceCreature.DeltaCreatureStats(inte: -1, lus: 15);
			}
			if (!sourceCreature.hasVagina)
			{ //Vagoo
				sb.Append(Environment.NewLine + "<b>Your crotch is like, all itchy an' stuff. Omigawsh! There's a wet little slit opening up, and it's all tingly! It feels so good, maybe like, someone could put something inside there!</b>" + Environment.NewLine);
			}
			if (sourceCreature.hips.size < 12)
			{
				sb.Append(Environment.NewLine + "Whoah! As you move, your " + sourceCreature.build.HipsLongDescription() + " sway farther and farther to each side, expanding with every step, soft new flesh filling in as your hips spread into something more appropriate on a tittering bimbo. You giggle when you realize you can't walk any other way. At least it makes you look, like, super sexy!" + Environment.NewLine);
				sourceCreature.DeltaCreatureStats(inte: -1);
			}
			if (sourceCreature.butt.size < 12)
			{
				sb.Append(Environment.NewLine + "Gradually warming, you find that your " + sourceCreature.build.ButtLongDescription() + " is practically sizzling with erotic energy. You smile to yourself, imagining how much you wish you had a nice, plump, bimbo-butt again, your hands finding their way to the flesh on their own. Like, how did they get down there? You bite your lip when you realize how good your tush feels in your hands, particularly when it starts to get bigger. Are butts supposed to do that? Happy pink thoughts wash that concern away - it feels good, and you want a big, sexy butt! The growth stops eventually, and you pout disconsolately when the lusty warmth's last lingering touches dissipate. Still, you smile when you move and feel your new booty jiggling along behind you. This will be fun!" + Environment.NewLine);
				sourceCreature.DeltaCreatureStats(inte: -1, lus: 10);
			}
			MakeBimbo();
			return sb.ToString();
		}

		protected override string OnStatusEffectWoreOff(byte hoursPassedSinceLastUpdate)
		{
			return RemoveEffect(true);
		}

		protected override void OnRemoval()
		{
			RemoveEffect(false);
		}

		private string RemoveEffect(bool doText)
		{
			sourceCreature.DeltaCreatureStats(spe: 10, lib: -1);

			//if we've already cleared the data
			if (clearedData)
			{
				return null;
			}
			//if they got bimbo body before this is called, silently wear off.
			else if (sourceCreature.GetPerkData<BimBro>()?.hasBimboEffect == true)
			{
				ClearData();
				return null;
			}

			StringBuilder sb = new StringBuilder();

			if (doText)
			{
				sb.Append(Environment.NewLine + "<b>Whoah! Your head is clearing up, and you feel like you can think clearly for the first time in forever."
					 + " Niamh sure is packing some potent stuff! You shake the cobwebs out of your head, glad to once again be less dense than"
					 + " a goblin with a basilisk boyfriend.</b>");
			}

			if (cupSizeDelta > 0)
			{
				sourceCreature.breasts[0].ShrinkBreasts((byte)cupSizeDelta, true);

				if (doText)
				{
					sb.Append(" As the treacherous brew fades, your " + sourceCreature.breasts[0].LongDescription() + " loses some of its... bimboliciousness."
						 + " Your back feels so much lighter without the extra weight dragging down on it.");
				}
			}
			if (grewVagina && sourceCreature.hasVagina)
			{
				var id = (uint)vaginaCollectionID;
				var vag = sourceCreature.vaginas.FirstOrDefault(x => x.collectionID == id) ?? sourceCreature.vaginas[sourceCreature.vaginas.Count - 1];

				sourceCreature.genitals.RemoveVagina(vag);

				if (doText)
				{
					sb.Append(" At the same time, " + sourceCreature.genitals.OneVaginaOrVaginasShort() + " slowly seals itself up, disappearing as quickly as it came. " +
						(sourceCreature.vaginas.Count > 1 ? "goodbye extra vagina." : "Goodbye womanhood."));
				}
			}
			if (buttSizeDelta > 0)
			{
				sourceCreature.butt.ShrinkButt((byte)buttSizeDelta);
				if (doText)
				{
					sb.Append(" Of course, the added junk in your trunk fades too, leaving you back to having a [butt].");
				}
			}
			if (hipsSizeDelta > 0)
			{
				sourceCreature.hips.ShrinkHips((byte)hipsSizeDelta);
			}
			if (doText)
			{
				sb.Append(Environment.NewLine);
			}
			return sb.ToString();
		}

		private void ClearData()
		{
			buttSizeDelta = 0;
			cupSizeDelta = 0;
			hipsSizeDelta = 0;
			vaginaCollectionID = null;

			clearedData = true;
		}

		//this will be enforced both on activation and when time passes. hence the breakout.
		//note that these effects are 'permanent' in that you cannot go below these values.
		private void MakeBimbo()
		{
			clearedData = false;
			if (sourceCreature.breasts[0].cupSize < CupSize.DD)
			{
				short delta = sourceCreature.breasts[0].SetCupSize(CupSize.DD);

				if (delta > cupSizeDelta)
				{
					cupSizeDelta = delta;
				}
			}

			//grow a vagina if doesn't have one. store the collection id for the created vag in case the player gets a second one before this wears off.
			if (!sourceCreature.hasVagina)
			{
				sourceCreature.AddVagina();
				vaginaCollectionID = sourceCreature.vaginas[0].collectionID;
			}
			//if they do have one, and some strange edge case caused the player to remove or replace the vagina we marked for removal, update it so we remove
			//the last vagina in the collection instead. This should only occur with weird multi-vagina edge cases or an even strager case where they lost and
			//regained a vagina before this could auto-correct.
			else if (grewVagina && sourceCreature.vaginas.All(x => x.collectionID != vaginaCollectionID))
			{
				vaginaCollectionID = sourceCreature.vaginas[sourceCreature.vaginas.Count - 1].collectionID;
			}

			//butt size. must be 12.
			if (sourceCreature.butt.size < 12)
			{
				short temp = sourceCreature.butt.SetButtSize(12);
				if (temp > buttSizeDelta)
				{
					buttSizeDelta = temp;
				}
			}

			if (sourceCreature.hips.size < 12)
			{
				byte temp = sourceCreature.hips.SetHipSize(12);
				if (temp > hipsSizeDelta)
				{
					hipsSizeDelta = temp;
				}
			}
		}





		public override string Name()
		{
			return "Bimbo Body (Temporary)";
		}

		public override string HasPerkText()
		{
			return "Drinking Bimbo Champagne has made you, like, a super sexy bimbo! Sadly, the stuff wears off after a while - You should totally " +
				"get your hands on some Bimbo Liqueur and make it permanent!";
		}
	}
}
