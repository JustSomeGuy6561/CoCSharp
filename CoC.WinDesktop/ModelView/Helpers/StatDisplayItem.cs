using CoC.Frontend.UI.ControllerData;
using CoCWinDesktop.Strings;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace CoCWinDesktop.ModelView.Helpers
{
	//the lifespan of these objects is actually the same as the base stat stored within it, so we don't need to worry about removing the event here. 
	//cool. That was shaping up to be a huge hassle. 
	public sealed class StatDisplayItem : INotifyPropertyChanged
	{
		private readonly CreatureStat baseStat;

		public event PropertyChangedEventHandler PropertyChanged;

		public Visibility visibility => baseStat.enabled ? Visibility.Visible : Visibility.Collapsed;

		internal string Name => baseStat.statName;

		public string Text
		{
			get => _Text;
			private set => IHateYouBoat(ref _Text, value);
		}
		private string _Text = "";

		public void CheckText()
		{
			Text = LanguageLookup.Lookup(baseStat.statName);
		}

		public bool needsGauge => baseStat is CreatureStatWithMinMax;
		public bool showMinOverMax => (baseStat as CreatureStatWithMinMax)?.isRatio ?? false;

		public bool displayStatChangeIcon => baseStat.notifyPlayerOfChange;

		public Color? minColor
		{
			set
			{
				if (_minColor != value)
				{
					_minColor = value;
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
		private Color? _minColor = null;

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

		public SolidColorBrush MinimumColor
		{
			get => _MinimumColor;
			private set => IHateYouBoat(ref _MinimumColor, value);
		}
		private SolidColorBrush _MinimumColor = null;


		public SolidColorBrush RegularColor
		{
			get => _RegularColor;
			private set => IHateYouBoat(ref _RegularColor, value);
		}
		private SolidColorBrush _RegularColor = null;


		public uint Value => baseStat.current;
		private void OnValueChanged()
		{
			OnPropertyChanged(nameof(Value));
			ChangeRegularColor();
		}

		public uint? Maximum => (baseStat as CreatureStatWithMinMax)?.maximum;
		private void OnMaximumChanged()
		{
			OnPropertyChanged(nameof(Maximum));
			ChangeRegularColor();
		}
		public uint? Minimum => (baseStat as CreatureStatWithMinMax)?.minimum;
		private void OnMinimumChanged()
		{
			OnPropertyChanged(nameof(Minimum));
			ChangeRegularColor();
		}


		public StatDisplayItem(CreatureStat creatureStat)
		{
			baseStat = creatureStat ?? throw new ArgumentNullException(nameof(creatureStat));

			baseStat.PropertyChanged += BasePropertyChanged;

			minColor = null;
			regColorDefaultOrMax = null;
			regColorMin = null;

			OnPropertyChanged(null);
		}

		public Visibility ArrowVisibility
		{
			get => _ArrowVisibility;
			set
			{
				if (_ArrowVisibility != value)
				{
					_ArrowVisibility = value;
					OnPropertyChanged(nameof(ArrowVisibility));
				}
			}
		}
		private Visibility _ArrowVisibility = Visibility.Hidden;


		private void BasePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (string.IsNullOrEmpty(e.PropertyName))
			{
				OnPropertyChanged(nameof(Value));
				OnPropertyChanged(nameof(visibility));

				if (baseStat is CreatureStatWithMinMax)
				{
					OnPropertyChanged(nameof(showMinOverMax));
					OnPropertyChanged(nameof(Maximum));
					OnPropertyChanged(nameof(Minimum));
					ChangeRegularColor();
				}
			}
			else if (e.PropertyName == nameof(baseStat.current))
			{
				OnValueChanged();
			}
			else if (e.PropertyName == nameof(baseStat.enabled))
			{
				OnPropertyChanged(nameof(visibility));
			}

			else if (e.PropertyName == nameof(baseStat.notifyPlayerOfChange))
			{
				OnPropertyChanged(nameof(displayStatChangeIcon));
			}

			else if (baseStat is CreatureStatWithMinMax minMax)
			{
				if (e.PropertyName == nameof(minMax.isRatio))
				{
					OnPropertyChanged(nameof(showMinOverMax));
				}
				else if (e.PropertyName == nameof(minMax.maximum))
				{
					OnMaximumChanged();
				}
				else if (e.PropertyName == nameof(minMax.minimum))
				{
					OnMinimumChanged();
				}
			}

		}

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private void IHateYouBoat(ref SolidColorBrush data, SolidColorBrush newValue, [CallerMemberName] string propertyName = "")
		{
			if (data != newValue)
			{
				data = newValue;
				OnPropertyChanged(propertyName);
			}
		}

		private void IHateYouBoat<T>(ref T data, T newValue, [CallerMemberName] string propertyName = "") where T : IEquatable<T>
		{
			if ((data == null) != (newValue == null) || (data != null && !data.Equals(newValue)))
			{
				data = newValue;
				OnPropertyChanged(propertyName);
			}
		}

		private void ChangeRegularColor()
		{
			if (_regularColorMax == null)
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
				uint value = Value;

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

		/*
		<CC:StatBar Padding="0,0,0,1" Text="{Binding Path=text}" HasGauge="{Binding Path=needsGauge}" ShowMinOverMax="{Binding Path=showMinOverMax}"
		MinColor="{Binding Path=minColorOverride, Converter={StaticResource ColorConverter}, TargetNullValue={StaticResource MinDefault}}" 
		RegularColor="{Binding Path=regularColorOverride, Converter={StaticResource ColorConverter}, TargetNullValue={StaticResource RegDefault}}" 
		ColorRangeMinimum="{Binding Path=regularColorRangeMinValue, Converter={StaticResource ColorConverter}, TargetNullValue=Transparent}"
		MinimumValue="{Binding Path=minimum, TargetNullValue=0}" MaximumValue="{Binding Path=maximum}" Value="{Binding Path=current}" />
		 */

	}
}
