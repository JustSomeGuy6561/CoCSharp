//EggBase.cs
//Description:
//Author: JustSomeGuy
//6/27/2019, 6:30 PM
using CoC.Backend;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CoC.Frontend.Items.Consumables
{
#warning Make sure white eggs disable dick nipple flag. Include a new egg (i'm thinking yellow, but color irrelevant) that enables dick nipples, and large causes the creature to get them

	public abstract partial class EggBase : ConsumableBase, IEquatable<EggBase>
	{
		private static readonly List<Func<bool, EggBase>> members;
		public static string[] EggColorChoices => members.Select(x => x(false).Color()).ToArray();

		public static EggBase RandomEgg(bool large) => Utils.RandomChoice(members.ToArray()).Invoke(large);

		static EggBase()
		{
			//add all members that implement egg base here, so the game can randomly choose one when needed.
			//this will also be used for content that lets the player manually select an egg by color, by generating each possibility (size irrelevant), then simply pulling out their color text.
			//This takes a function callback, which should/will probably just be the constructor, wrapped nicely.
			members = new List<Func<bool, EggBase>>()
			{

			};
		}


		public bool isLarge { get; private set; }

		protected EggBase(bool large) : base()
		{
			isLarge = large;
		}

		public abstract string Color();

		public abstract bool Equals(EggBase other);

		public abstract bool EqualsIgnoreSize(EggBase other);
	}
}
