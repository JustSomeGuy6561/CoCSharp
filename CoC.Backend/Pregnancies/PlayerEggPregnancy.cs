using CoC.Backend.Items.Consumables;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
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

		//protected bool timeOutputFlag; //inherited from parent. 
		//protected StringBuilder timeOutputBuilder; //inherited from parent.

		private bool gainedOviMax = false;

		protected internal override SimpleDescriptor BirthText => () => BirthStr(gainedOviMax);

		protected internal override SimpleDescriptor TimePassedText => throw new NotImplementedException();

		protected internal override void HandleBirth(bool isVaginal)
		{
			gainedOviMax = false;
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
		}

		protected internal override void NotifyTimePassed(bool isVaginal, ushort hoursToBirth, ushort previousHoursToBirth)
		{
			//do nothing;
		}

		private readonly StringBuilder oviBuilder = new StringBuilder();
		private bool oviDidSomething = false;
		protected internal override float HandleOviElixir(ref ushort timeToBirth, byte strength = 1)
		{
			oviDidSomething = false;
			oviBuilder.Clear();

			byte largeEggChance = 3;
			if (strength == 0)
			{
				return 0;
			}
			else if (strength != 1)
			{
				largeEggChance = 2;
			}

			if (!largeEggs && Utils.Rand(largeEggChance) == 0)
			{
				largeEggs = true;
				oviDidSomething = true;
				timeOutputBuilder.Append(EggBiggerStr());
			}

			if (Utils.RandBool())
			{
				eggCount += Utils.Rand(4) + 1;
				oviDidSomething = true;
			}

			if (!oviDidSomething)
			{
				return base.HandleOviElixir(ref timeToBirth, strength);
			}
			return 0;
		}

		protected internal override SimpleDescriptor OviElixirText(float advancedBy, byte strength)
		{
			if (!oviDidSomething)
			{
				return base.OviElixirText(advancedBy, strength);
			}
			else
			{
				return () => oviBuilder.ToString();
			}
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
	}

}
