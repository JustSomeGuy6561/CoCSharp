using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CoC.Backend.Items.Consumables
{
	public abstract partial class EggBase : ConsumableBase
	{
		private static readonly List<EggBase> members = new List<EggBase>();
		public static readonly ReadOnlyCollection<EggBase> eggChoices = new ReadOnlyCollection<EggBase>(members);

		protected SimpleDescriptor colorStr;

		protected EggBase(SimpleDescriptor color)
		{
			members.Add(this);
			colorStr = color;
		}

		public virtual SimpleDescriptor shortDesc => ShortDescription;


		public static EggBase RandomEgg() => Utils.RandomChoice(members.ToArray());
	}
}
