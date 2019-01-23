using CoC.Creatures;
using CoC.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CoC.UI
{

	//consider firing custom events instead of INotifyPropertyChanged. 
	public class Controller : INotifyPropertyChanged
	{

		public PlayerData playerData { get; }
		public List<ButtonData> uiData { get; }

		public CombatData combatData;
		private readonly Player player;

		private Controller(Player p)
		{
			uiData = new List<ButtonData>();
			playerData = new PlayerData(p);
			player = p;
		}

		private readonly ButtonData[] buttonData = new ButtonData[15];

		public event PropertyChangedEventHandler PropertyChanged;

		public void Init()
		{

		}
		private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		public bool AddButton(int index, string name, string hint, PlayerFunction callback)
		{
			if (buttonData[index] != null)
				return false;
			buttonData[index] = new ButtonData(index, name, hint, callback);
			return true;
		}

		public void Call(PlayerFunction callback)
		{
			//clear the old data.
			uiData.Clear();
			Array.Clear(buttonData, 0, buttonData.Length);
			playerData.ResetFrame();
			//call the function. wait for it to return.
			callback(player);
			//check if the callback changed anything. if it did, notify the UI.
			//update ui data. it's now changed.
			if (buttonData.Count(x=> x != null) > 0)
			{
				uiData.AddRange(buttonData);
				NotifyPropertyChanged(nameof(uiData));
			}
			//update the player data. 
			if (playerData.playerChangedThisFrame)
			{
				playerData.UpdateData();
				NotifyPropertyChanged(nameof(playerData));
			}
			//return to the view. 
		}
	}
}
