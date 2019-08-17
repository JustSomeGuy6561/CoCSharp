using CoC.Backend.Engine.Time;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.UI.ControllerData
{
	public abstract class StatDataCollectionBase
	{
		public readonly PlayerStatData playerStats = new PlayerStatData();
		public readonly CombatEnemyStatData enemyStats = new CombatEnemyStatData();
		public GameDateTime currentTime => GetTime();

		public StatDataCollectionBase() { }

		public abstract void ParseData();

		public abstract GameDateTime GetTime();
	}
}
