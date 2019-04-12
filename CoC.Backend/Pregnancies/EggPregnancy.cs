using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Pregnancies
{
	//eggs are weird man.
	//need to account for fertalizing them. 

	public sealed partial class EggPregnancy : SpawnType
	{
		private int eggCount;
		private bool largeEgg;
		//private EggItem eggColor = null;

		public bool colorKnown = false;


		public EggPregnancy() : base(null, 9001, null)
		{

		}

		public override void HandleBirth()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
			//if (eggColor == null)
			//{
			//	eggColor = Utils.RandomChoice<EggItem>(EggItem.AvailableColors);
			//	eggColor.isLarge = largeEgg;
			//}
			//add eggColor to inventory.

		}

		public override void HandleOviElixir(int strength = 1)
		{
			if (!largeEgg && Utils.Rand(3) < strength)
			{
				largeEgg = true;
			}
			eggCount += Utils.Rand(4) + strength;
		}

		public override SimpleDescriptor OviElixirText => EggOviText;

	}
}
