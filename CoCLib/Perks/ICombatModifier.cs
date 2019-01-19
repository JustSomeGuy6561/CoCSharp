//ICombatModifier.cs
//Description:
//Author: JustSomeGuy
//1/8/2019, 1:26 AM
using CoC.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.Perks
{
	interface ICombatModifier
	{
		//called when calculating combat related stuff.
		double spellCostMultiplier { get; }
		double fatigueCostMultiplier { get; }
		double damageMultiplier { get; }
		double strengthMultiplier { get; }
		double toughnessMultiplier { get; }
		double speedMultiplier { get; }
		double intelligenceMultiplier { get; }
		double lustGainMultiplier { get; }
		//called before a round of combat
		CombatDelegate preCombatEffect { get; }
		//called after the player attacks
		CombatDelegate postAttackEffect { get; }
	}
}
