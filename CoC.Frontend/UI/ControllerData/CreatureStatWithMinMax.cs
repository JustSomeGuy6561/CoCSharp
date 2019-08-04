using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend;

namespace CoC.Frontend.UI.ControllerData
{
	public sealed class CreatureStatWithMinMax : CreatureStat
	{
		public uint maximum
		{
			get => _maximum;
			internal set
			{
				if (_maximum != value)
				{
					_maximum = value;
					NotifyPropertyChanged();
				}
			}
		}
		private uint _maximum = 100;

		public uint? minimum
		{
			get => _minimum;
			internal set
			{
				if (_minimum != value)
				{
					_minimum = value;
					NotifyPropertyChanged();
				}
			}
		}
		private uint? _minimum = null;

		public bool isRatio
		{
			get => _isRatio;
			internal set
			{
				if (isRatio != value)
				{
					_isRatio = value;
					NotifyPropertyChanged();
				}
			}
		}
		private bool _isRatio = false;

		internal CreatureStatWithMinMax(string name, CreatureStatCategory statCategory) : base(name, statCategory)
		{

		}
	}
}
