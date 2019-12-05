//PlayerEggPregnancy.cs
//Description:
//Author: JustSomeGuy
//4/7/2019, 8:42 PM
using CoC.Backend;
using CoC.Backend.Pregnancies;
using CoC.Backend.Tools;
using CoC.Frontend.Items.Consumables;
using System;

namespace CoC.Frontend.Pregnancies
{
	//eggs are weird man.
	//need to account for fertalizing them.

	public abstract class EggPregnancy : StandardSpawnType
	{
		protected internal int eggCount { get; protected set; }
		protected internal bool largeEggs { get; protected set; }
		protected internal Func<bool, EggBase> knownEggType { get; protected set; }

		public EggPregnancy(Guid creatureID, SimpleDescriptor eggText, SimpleDescriptor eggSourceText, ushort spawnTimer, bool largeClutch = false, bool? isLarge = null,
			Func<bool, EggBase> color = null) : base(creatureID, eggText, eggSourceText, spawnTimer)
		{
			largeEggs = isLarge ?? Utils.RandBool();
			knownEggType = color;
			eggCount = largeClutch ? Utils.Rand(4) + 6 : Utils.Rand(3) + 5;
		}

		protected virtual bool AllowEggFertilization(StandardSpawnType fertilizeType)
		{
			return fertilizeType.canFertilizeAnEggPregnancy;
		}

		protected override bool HandleNewKnockupAttempt(StandardSpawnType type, out StandardSpawnType newType)
		{
			if (type.canFertilizeAnEggPregnancy && AllowEggFertilization(type))
			{
				newType = type;
				return true;
			}

			return base.HandleNewKnockupAttempt(type, out newType);
		}

		public override StandardSpawnData AsReadOnlyData()
		{
			return new EggSpawnData(this);
		}

		//currently only allows you to do this once.
		public bool setEggColor(Func<bool, EggBase> newColorType)
		{
			if (knownEggType != null)
			{
				return false;
			}
			knownEggType = newColorType;
			return true;
		}

		protected override bool AdditionalValidation(bool currentlyValid, bool correctInvalidData)
		{
			//silently correct these.
			if (eggCount < 4)
			{
				eggCount = 4;
			}

			if (knownEggType != null && (knownEggType(true) is null || knownEggType(false) is null))
			{
				knownEggType = null;
			}

			return currentlyValid;
		}
	}

	public class EggSpawnData : StandardSpawnData
	{
		public readonly int currentEggCount;
		public readonly EggBase currentEggColor;
		public readonly bool eggsCurrentlyLarge;

		internal EggSpawnData(EggPregnancy source) : base(source)
		{
			currentEggCount = source.eggCount;
			eggsCurrentlyLarge = source.largeEggs;
			currentEggColor = source.knownEggType?.Invoke(source.largeEggs);
		}
	}
}