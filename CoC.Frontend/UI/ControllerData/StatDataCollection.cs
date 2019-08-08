using CoC.Backend.Engine.Time;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.UI.ControllerData
{
	public sealed class StatDataCollection
	{
		public readonly PlayerStatData playerStats = new PlayerStatData();
		public readonly CombatEnemyStatData enemyStats = new CombatEnemyStatData();
		public GameDateTime currentTime => getGameTime();
		private readonly Func<GameDateTime> getGameTime;

		public StatDataCollection(Func<GameDateTime> getTime)
		{
			getGameTime = getTime ?? throw new ArgumentNullException(nameof(getTime));
		}
	}
}
