﻿using CoC.Backend;
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Items.Wearables.Accessories.CockSocks;
using CoC.Backend.Tools;

namespace CoC.Frontend.Items.Wearables.Accessories.CockSocks
{
	public sealed class Gilded : CockSockBase
	{
		public Gilded() : base()
		{
		}

		public override double PhysicalDefensiveRating(Creature wearer) => 0;

		protected override int monetaryValue => throw new System.NotImplementedException();

		public override string LongDescription(CockData attachedCock)
		{
			throw new System.NotImplementedException();
		}

		public override string ShortDescription()
		{
			throw new System.NotImplementedException();
		}

		public override bool Equals(CockSockBase other)
		{
			throw new System.NotImplementedException();
		}

		protected override void OnEquip(Creature wearer)
		{
			base.OnEquip(wearer);
		}

		protected override string EquipText(Creature wearer)
		{
			throw new System.NotImplementedException();
		}

		protected override string PlayerText(PlayerBase player, CockData attachedCock)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		protected override CockSockBase OnRemove(Creature wearer)
		{
			return base.OnRemove(wearer);
		}

		public override string AbbreviatedName()
		{
			throw new System.NotImplementedException();
		}

		public override string ItemName()
		{
			throw new System.NotImplementedException();
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			throw new System.NotImplementedException();
		}

		public override string AboutItem()
		{
			throw new System.NotImplementedException();
		}

		public override string AboutItemWithStats(Creature wearer)
		{
			throw new System.NotImplementedException();
		}

		/*
		//./classes/PlayerAppearance.as
		public void sockDescript(int index)
		{
			sb.Append(" ");
			if (player.cocks[index].sock == "wool")
				sb.Append("It's covered by a wooly white cock-sock, keeping it snug and warm despite how cold it might get.");
			else if (player.cocks[index].sock == "alabaster")
				sb.Append("It's covered by a white, lacey cock-sock, snugly wrapping around it like a bridal dress around a bride.");
			else if (player.cocks[index].sock == "cockring")
				sb.Append("It's covered by a black latex cock-sock with two attached metal rings, keeping your cock just a little harder and [balls] aching for release.");
			else if (player.cocks[index].sock == "viridian")
				sb.Append("It's covered by a lacey dark green cock-sock accented with red rose-like patterns. Just wearing it makes your body, especially your cock, tingle.");
			else if (player.cocks[index].sock == "scarlet")
				sb.Append("It's covered by a lacey red cock-sock that clings tightly to your member. Just wearing it makes your cock throb, as if it yearns to be larger...");
			else if (player.cocks[index].sock == "cobalt")
				sb.Append("It's covered by a lacey blue cock-sock that clings tightly to your member... really tightly. It's so tight it's almost uncomfortable, and you wonder if any growth might be inhibited.");
			else if (player.cocks[index].sock == "gilded")
				sb.Append("It's covered by a metallic gold cock-sock that clings tightly to you, its surface covered in glittering gems. Despite the warmth of your body, the cock-sock remains cool.");
			else if (player.cocks[index].sock == "amaranthine")

			{
				sb.Append("It's covered by a lacey purple cock-sock");
				if (player.cocks[index].cockType != CockType.DISPLACER)
					sb.Append(" that fits somewhat awkwardly on your member");
				else
					sb.Append(" that fits your coeurl cock perfectly");
				sb.Append(". Just wearing it makes you feel stronger and more powerful.");
			}
			else if (player.cocks[index].sock == "red")
				sb.Append("It's covered by a red cock-sock that seems to glow. Just wearing it makes you feel a bit powerful.");
			else if (player.cocks[index].sock == "green")
				sb.Append("It's covered by a green cock-sock that seems to glow. Just wearing it makes you feel a bit healthier.");
			else if (player.cocks[index].sock == "blue"
								sb.Append("It's covered by a blue cock-sock that seems to glow. Just wearing it makes you feel like you can cast spells more effectively.");

			else sb.Append("<b>Yo, this is an error.</b>");
		}*/
	}
}
