using CoC.Backend;
using CoC.Frontend.UI.ControllerData;
using CoC.WinDesktop.Helpers;
using CoC.WinDesktop.Strings;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace CoC.WinDesktop.ContentWrappers
{
	//the lifespan of these objects is actually the same as the base stat stored within it, so we don't need to worry about removing the event here. 
	//cool. That was shaping up to be a huge hassle. 
	public sealed class StatDisplayItemWrapper : NotifierBase
	{
		public Visibility visibility { get; private set; }

		private readonly SimpleDescriptor NameCallback;

		public bool IsNumeric
		{
			get => _isNumeric;
			private set => CheckPrimitivePropertyChanged(ref _isNumeric, value);
		}
		private bool _isNumeric;

		public bool HasMinMax
		{
			get => _HasMinMax;
			private set => CheckPrimitivePropertyChanged(ref _HasMinMax, value);
		}
		private bool _HasMinMax;

		public string Text
		{
			get => _text;
			private set => CheckPropertyChanged(ref _text, value);
		}// NameCallback();
		private string _text;

		public bool showValueOverMax
		{
			get => _showValueOverMax;
			private set => CheckPrimitivePropertyChanged(ref _showValueOverMax, value);
		}
		private bool _showValueOverMax;

		public bool displayStatChangeIcon
		{
			get => _displayStatChangeIcon;
			private set => CheckPrimitivePropertyChanged(ref _displayStatChangeIcon, value);
		}
		private bool _displayStatChangeIcon;

		private readonly bool silent;

		public Color? minColor
		{
			set
			{
				if (MinimumColor?.Color != value)
				{
					if (value is null)
					{
						MinimumColor = null;
					}
					else if (MinimumColor is null)
					{
						MinimumColor = new SolidColorBrush((Color)value);
					}
					else
					{
						MinimumColor.Color = (Color)value;
					}
				}
			}
		}

		public Color? regColorDefaultOrMax
		{
			set
			{
				if (value != _regularColorMax)
				{
					_regularColorMax = value;
					ChangeRegularColor();
				}
			}
		}
		private Color? _regularColorMax;

		public Color? regColorMin
		{
			set
			{
				if (value != _regularColorMin)
				{
					_regularColorMin = value;
					ChangeRegularColor();
				}
			}
		}

		private Color? _regularColorMin;

		//don't set this. Handled by minColor
		public SolidColorBrush MinimumColor
		{
			get => _MinimumColor;
			private set => CheckPropertyChanged(ref _MinimumColor, value);
		}
		private SolidColorBrush _MinimumColor = null;

		//don't set this. Handled by ChangeRegularColor();
		public SolidColorBrush RegularColor
		{
			get => _RegularColor;
			private set => CheckPropertyChanged(ref _RegularColor, value);
		}
		private SolidColorBrush _RegularColor = null;

		public string Value
		{
			get => _value;
			private set => CheckPropertyChanged(ref _value, value);
		}
		private string _value;

		public uint? Maximum
		{
			get => _maximum;
			private set => CheckPrimitivePropertyChanged(ref _maximum, value);
		}
		private uint? _maximum;

		public uint? Minimum
		{
			get => _minimum;
			private set => CheckPrimitivePropertyChanged(ref _minimum, value);
		}
		private uint? _minimum;

		public StatDisplayItemWrapper(CreatureStatBase creatureStat, SimpleDescriptor displayNameFunction, bool isSilent = false)
		{
			NameCallback = displayNameFunction ?? throw new ArgumentNullException(nameof(displayNameFunction));
			if (string.IsNullOrWhiteSpace(NameCallback()))
			{
				throw new ArgumentException("displayName must be a valid string");
			}
			_text = NameCallback();


			silent = isSilent;

			//updated externally via properties if not default values. 
			minColor = null;
			regColorDefaultOrMax = null;
			regColorMin = null;

			visibility = creatureStat.enabled ? Visibility.Visible : Visibility.Collapsed;
			IsNumeric = creatureStat is CreatureStatNumeric;
			HasMinMax = creatureStat is CreatureStatWithMinMax;
			showValueOverMax = (creatureStat as CreatureStatWithMinMax)?.isRatio ?? false;
			displayStatChangeIcon = !silent && ((creatureStat as CreatureStatNumeric)?.notifyPlayerOfChange ?? false);

			Maximum = (creatureStat as CreatureStatWithMinMax)?.maximum;
			Minimum = (creatureStat as CreatureStatWithMinMax)?.minimum;
			Value = creatureStat.value;

			ChangeRegularColor();
		}

		public Visibility ArrowVisibility
		{
			get => _ArrowVisibility;
			set => CheckEnumPropertyChanged(ref _ArrowVisibility, value);
		}
		private Visibility _ArrowVisibility = Visibility.Hidden;

		public void UpdateStats(CreatureStatBase statBase)
		{
			Text = NameCallback();

			visibility = statBase.enabled ? Visibility.Visible : Visibility.Collapsed;
			IsNumeric = statBase is CreatureStatNumeric;
			HasMinMax = statBase is CreatureStatWithMinMax;
			showValueOverMax = (statBase as CreatureStatWithMinMax)?.isRatio ?? false;
			displayStatChangeIcon = (statBase as CreatureStatNumeric)?.notifyPlayerOfChange ?? false;

			Maximum = (statBase as CreatureStatWithMinMax)?.maximum;
			Minimum = (statBase as CreatureStatWithMinMax)?.minimum;
			Value = statBase.value;

			ChangeRegularColor();
		}


		private void ChangeRegularColor()
		{
			if (!HasMinMax)
			{
				RegularColor = null;
			}
			else if (_regularColorMax == null)
			{
				RegularColor = null;
			}
			else if (_regularColorMin == null || Maximum == null)
			{
				if (RegularColor == null)
				{
					RegularColor = new SolidColorBrush((Color)_regularColorMax);
				}
				else
				{
					RegularColor.Color = (Color)_regularColorMax;
				}
			}
			else
			{
				double rd, gd, bd;
				Color minColor = (Color)_regularColorMin;
				Color maxColor = (Color)_regularColorMax;
				uint max = (uint)Maximum;
				uint value = uint.Parse(Value);

				double percent = 1.0 * value / max;
				rd = Math.Floor(CoC.Backend.Tools.Utils.Lerp(minColor.R, maxColor.R, percent));
				gd = Math.Floor(CoC.Backend.Tools.Utils.Lerp(minColor.G, maxColor.G, percent));
				bd = Math.Floor(CoC.Backend.Tools.Utils.Lerp(minColor.B, maxColor.B, percent));

				byte r, g, b;
				r = rd > byte.MaxValue ? byte.MaxValue : (byte)rd;
				g = gd > byte.MaxValue ? byte.MaxValue : (byte)gd;
				b = bd > byte.MaxValue ? byte.MaxValue : (byte)bd;

				if (RegularColor != null)
				{
					RegularColor.Color = Color.FromArgb(255, r, g, b);
				}
				else
				{
					RegularColor = new SolidColorBrush(Color.FromArgb(255, r, g, b));
				}
			}
		}
	}
}
