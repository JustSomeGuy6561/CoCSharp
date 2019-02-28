//Program.cs
//Description:
//Author: JustSomeGuy
//1/18/2019, 3:09 AM
using CoC.Creatures;
using CoC.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CoC.Engine
{
	public static class Program
	{
		public static ModelView controller;

		//Current Session. Changes on Load.
		internal static Player currPlayer;
		internal static SessionSettings sessionSettings;

		//global settings is static. 


		//where are we?
		//all places and areas, though they only are used once, are loaded dynamically, instead of static,
		//we store the active one here. may keep the camp stored all the time, as it'll be accessed frequently enough
		//to incur the memory cost over the time and memory cost of intializing and collecting it. 
		internal static Location location;

		static Program()
		{
			DataContractSerializer
			currPlayer = Player.NO_PLAYER;
			controller = new ModelView(currPlayer);
		}
		public static void Init()
		{

		}

		internal static bool Save()
		{
			throw new NotImplementedException();
		}
		//static 

		internal static GameSerializer serializer { get; } = new GameSerializer();
	}
}
