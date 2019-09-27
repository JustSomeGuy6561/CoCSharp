//PlayerEggPregnancy.cs
//Description:
//Author: JustSomeGuy
//4/7/2019, 8:42 PM
using CoC.Backend.Engine.Time;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Tools;
using System.Text;

namespace CoC.Backend.Pregnancies
{
	//eggs are weird man.
	//need to account for fertalizing them. 

	public sealed partial class PlayerEggPregnancy : PlayerSpawnType
	{
		private int eggCount;
		private bool largeEggs;
		private EggBase knownColor;
		private const ushort BIRTH_TIME = 50;

		public PlayerEggPregnancy(bool largeClutch = false, bool? isLarge = null, EggBase color = null) : base(EggSource, BIRTH_TIME)
		{
			largeEggs = isLarge ?? Utils.RandBool();
			knownColor = color;
			eggCount = largeClutch ? Utils.Rand(4) + 6 : Utils.Rand(3) + 5;
		}

		//currently only allows you to do this once.
		public bool setEggColor(EggBase newColor)
		{
			if (knownColor != null)
			{
				return false;
			}
			knownColor = newColor;
			return true;
		}

		protected internal override EventWrapper HandleBirth(bool isVaginal)
		{
			bool gainedOviMax = false;
			//check if we know the egg color. if we don't, we'll need to pick one
			if (knownColor == null)
			{
				knownColor = EggBase.RandomEgg();
			}

			//if (!mother.hasPerk(PerkLib.Oviposition) && kGAMECLASS.flags[kFLAGS.OVIMAX_OVERDOSE] > 0 && Utils.rand(3) < kGAMECLASS.flags[kFLAGS.OVIMAX_OVERDOSE])
			//{
			//mother.createPerk(PerkLib.Oviposition, 0, 0, 0, 0);
			//gainedOviMax = true;
			//}
			//mother.orgasm('Vaginal');

			//mother.SetLust(mother.maxLust);
			//mother.cuntChange(20, true);
			//mother.AddItemToInventory(knownColor);

			return (EventWrapper)BirthStr(gainedOviMax);
		}

		protected internal override string NotifyTimePassed(bool isVaginal, float hoursToBirth, float previousHoursToBirth)
		{
			return "";
		}

		protected internal override string HandleOviElixir(ref ushort timeToBirth, byte strength = 1)
		{
			StringBuilder oviBuilder = new StringBuilder();

			if (strength == 0)
			{
				return null;
			}

			byte largeEggChance = 3;
			if (strength != 1)
			{
				largeEggChance = 2;
			}

			if (!largeEggs && Utils.Rand(largeEggChance) == 0)
			{
				largeEggs = true;
				oviBuilder.Append(EggBiggerStr());
			}

			if (Utils.RandBool())
			{
				eggCount += Utils.Rand(4) + 1;
				oviBuilder.Append(EggMoreStr());
			}
			//if we haven't done anything
			if (oviBuilder.Length == 0)
			{
				//do the base case.
				return base.HandleOviElixir(ref timeToBirth, strength);
			}
			//otherwise return it.
			else
			{
				return oviBuilder.ToString();
			}
		}
	}

}
