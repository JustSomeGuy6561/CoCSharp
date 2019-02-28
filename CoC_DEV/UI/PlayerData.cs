//PlayerData.cs
//Description:
//Author: JustSomeGuy
//1/22/2019, 1:00 AM
using CoC.Creatures;
using System.ComponentModel;

namespace CoC.UI
{
	public class PlayerData
	{
		private readonly Player player;
		public bool playerChangedThisFrame { get; private set; } = false;
		internal PlayerData(Player p)
		{
			player = p;
			player.PropertyChanged += Player_PropertyChanged;
		}

		private void Player_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			playerChangedThisFrame = true;
		}

		public float level { get; private set; }
		public float experience { get; private set; }
		public float strength { get; private set; }
		public float toughness { get; private set; }
		public float speed { get; private set; }
		public float intelligence { get; private set; }
		public float corruption { get; private set; }
		public float hp { get; private set; }
		public float lust { get; private set; }
		public float fatigue { get; private set; }
		public float satiety { get; private set; }

		internal void ResetFrame()
		{
			playerChangedThisFrame = false;
		}

		internal void UpdateData()
		{
			level = player.level;
			experience = player.experience;
			strength = player.strength;
			toughness = player.toughness;
			speed = player.speed;
			intelligence = player.intelligence;
			corruption = player.corruption;
			hp = player.hp;
			lust = player.lust;
			fatigue = player.fatigue;
			satiety = player.satiety;
		}
	}
}