//EggBase.cs
//Description:
//Author: JustSomeGuy
//6/27/2019, 6:30 PM
using CoC.Backend;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Tools;
using CoC.Frontend.Items.Consumables.Eggs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CoC.Frontend.Items.Consumables
{
#warning Make sure white eggs disable dick nipple flag. Include a new egg (i'm thinking yellow, but color irrelevant) that enables dick nipples, and large causes the creature to get them

	public abstract partial class EggBase : StandardConsumable, IEquatable<EggBase>
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
				(x) => new BlackRubberEgg(x),

			};
		}


		public bool isLarge { get; private set; }

		protected EggBase(bool large) : base()
		{
			isLarge = large;
		}

		public override string ItemName()
		{
			return (isLarge ? "Large " : "") + Color() + " egg";
		}

		public override string AboutItem()
		{
			return "This is an oblong egg, not much different from " + (isLarge ? "an ostrich" : "a chicken") + " egg in appearance (save for the color)."
				+ " Something tells you it's more than just food.";
		}

		public abstract string Color();

		public override bool Equals(CapacityItem other)
		{
			return other is EggBase egg && this.Equals(egg);
		}

		public virtual bool Equals(EggBase other)
		{
			return EqualsIgnoreSize(other) && other.isLarge == isLarge;
		}

		public abstract bool EqualsIgnoreSize(EggBase other);

		public override bool countsAsCum => false;

		public override bool countsAsLiquid => false;

		public override bool CanUse(Creature target, bool isInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		protected override int monetaryValue => DEFAULT_VALUE;
	}
}
