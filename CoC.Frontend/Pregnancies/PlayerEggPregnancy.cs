//PlayerEggPregnancy.cs
//Description:
//Author: JustSomeGuy
//4/7/2019, 8:42 PM
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Reaction;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures.PlayerData;
using CoC.Frontend.Inventory;
using CoC.Frontend.Items.Consumables;
using CoC.Frontend.StatusEffect;
using System;
using System.Text;

namespace CoC.Frontend.Pregnancies
{
	//eggs are weird man.
	//need to account for fertalizing them.

	public sealed partial class PlayerEggPregnancy : EggPregnancy
	{
		private PlayerBase player => CreatureStore.activePlayer ?? throw new ArgumentException("Active Player is null in Creature Store. Should never happen");

		private const ushort BIRTH_TIME = 50;
		private const ushort BIRTH_SIZE = 20;

		public PlayerEggPregnancy(Guid creatureID, bool largeClutch = false, bool? isLarge = null, Func<bool, EggBase> color = null)
			: base(creatureID, EggDesc, EggSource, BIRTH_TIME, largeClutch, isLarge, color)
		{ }

		protected override DynamicTimeReaction HandleVaginalBirth(byte vaginalIndex)
		{
			//we know the creature is the player, so ignore the id.

			bool gainedOviMax = false;
			//check if we know the egg color. if we don't, we'll need to pick one
			if (knownEggType == null)
			{
				knownEggType = EggBase.RandomEgg;
			}

			if (!((PlayerWomb)player.womb).laysEggs && player.perks.HasTimedEffect<OverdosedOviMax>())
			{
				OverdosedOviMax oviOD = player.perks.GetTimedEffectData<OverdosedOviMax>();
				if (Utils.Rand(3) < oviOD.overdoseCount)
				{
					((PlayerWomb)player.womb).GrantOviposition();
					gainedOviMax = true;
				}

			}

			player.HaveGenericVaginalOrgasm(vaginalIndex, false, true);
			player.SetLust(player.maxLust);

			var egg = knownEggType(largeEggs);
			return GainItemHelper.GainItemEvent(player, egg, () => BirthStr(gainedOviMax, egg));
		}

		protected override string NotifyVaginalBirthingProgressed(byte vaginalIndex, float hoursToBirth, float previousHoursToBirth)
		{
			return "";
		}

		protected override string HandleOviElixir(ref ushort timeToBirth, byte strength = 1)
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

		public override ushort sizeOfCreatureAtBirth => BIRTH_SIZE;
	}

}
