//Main.cs
//Description:
//Author: JustSomeGuy
//1/18/2019, 3:09 AM
using CoC.Creatures;
using CoC.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.Engine
{
	public static class Program
	{
		public static Controller controller;

		internal static Player currPlayer;

		static Program()
		{
			currPlayer = Player.NO_PLAYER;
			controller = new Controller(currPlayer);
		}
		public static void Init()
		{

		}
		//static 
	}
}
