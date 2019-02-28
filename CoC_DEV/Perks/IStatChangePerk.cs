//IStatChangePerk.cs
//Description:
//Author: JustSomeGuy
//1/8/2019, 1:20 AM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.Perks
{
	interface IStatChangePerk
	{
		double libidoGainMultiplier { get; }
		double lustGainMultiplier { get; }
		double corruptionGainMultiplier { get; }

		//how fast do you lose your fatigue?
		double fatigueChangeMultiplier { get; }

		double incomeMultiplier { get; }
		double experienceMultiplier { get; }

		double deltaFertilityAmount { get; }
		double deltaLustAmount { get; }
		double deltaCorruptionAmount { get; }
		double deltaMaxStrength { get; }
		double deltaMaxToughness { get; }
		double deltaMaxSpeed { get; }
		double deltaMaxIntelligence { get; }
		double deltaMaxLibido { get; }

		double deltaMinLust { get; }
		double deltaMaxLust { get; }

		double deltaMinCorruption { get; }

		double deltaMaxCorruption { get; }

		double deltaMaxHunger { get; }
	}
}
