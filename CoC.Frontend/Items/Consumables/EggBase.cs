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
	public abstract partial class EggBase : ConsumableBase
	{


		private static readonly List<Func<bool, EggBase>> members;
		public static string[] EggColorChoices => members.Select(x => x(false).colorStr()).ToArray();

		static EggBase()
		{
			//add all members that implement egg base here, so the game can randomly choose one when needed. 
			//this will also be used for content that lets the player manually select an egg by color, by generating each possibility (size irrelevant), then simply pulling out their color text. 
			//This takes a function callback, which should/will probably just be the constructor, wrapped nicely.
			members = new List<Func<bool, EggBase>>()
			{

			};
		}


		protected readonly SimpleDescriptor colorStr;
		public bool isLarge { get; private set; }

		protected EggBase(SimpleDescriptor colorText, bool large, DescriptorWithArg<bool> shortText, DescriptorWithArg<bool> longText, DescriptorWithArg<bool> descText) 
			: base(checkValid(shortText, large, nameof(shortText)), checkValid(longText, large, nameof(longText)), checkValid(descText, large, nameof(descText)))
		{
			colorStr = colorText;
			isLarge = large;
		}

		private static SimpleDescriptor checkValid(DescriptorWithArg<bool> fn, bool value, string name)
		{
			if (fn is null) throw new ArgumentNullException(name);
			else return () => fn(value);
		}

		public virtual SimpleDescriptor shortDesc => ShortDescription;

		public static EggBase RandomEgg(bool large) => Utils.RandomChoice(members.ToArray()).Invoke(large);
	}
}
