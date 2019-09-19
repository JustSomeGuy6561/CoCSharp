using CoC.WinDesktop.MSInternalCopy;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Automation.Peers;
// For typeconverter
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace CoC.WinDesktop.CustomControls
{
	/// <summary>
	/// CustomSlider control lets the user select from a range of values by moving a slider.
	/// CustomSlider is used to enable to user to gradually modify a value (range selection).
	/// CustomSlider is an easy and natural interface for users, because it provides good visual feedback.
	/// </summary>
	/// <seealso cref="RangeBase" />
	[Localizability(LocalizationCategory.Ignore)]
	[DefaultEvent("ValueChanged"), DefaultProperty("Value")]
	[TemplatePart(Name = "PART_Track", Type = typeof(Track))]
	[TemplatePart(Name = "PART_SelectionRange", Type = typeof(FrameworkElement))]
	public class CustomSlider : RangeBase
	{
		#region Constructors

		/// <summary>
		/// Instantiates a new instance of a CustomSlider with out Dispatcher.
		/// </summary>
		/// <ExternalAPI/>
		public CustomSlider() : base()
		{
			
		}

		/// <summary>
		/// This is the static constructor for the CustomSlider class.  It
		/// simply registers the appropriate class handlers for the input
		/// devices, and defines a default style sheet.
		/// </summary>
		static CustomSlider()
		{
			// Initialize CommandCollection & CommandLink(s)
			InitializeCommands();

			// Register all PropertyTypeMetadata
			MinimumProperty.OverrideMetadata(typeof(CustomSlider), new FrameworkPropertyMetadata(0.0d, FrameworkPropertyMetadataOptions.AffectsMeasure));
			MaximumProperty.OverrideMetadata(typeof(CustomSlider), new FrameworkPropertyMetadata(10.0d, FrameworkPropertyMetadataOptions.AffectsMeasure));
			ValueProperty.OverrideMetadata(typeof(CustomSlider), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsMeasure));

			// Register Event Handler for the Thumb
			EventManager.RegisterClassHandler(typeof(CustomSlider), Thumb.DragStartedEvent, new DragStartedEventHandler(CustomSlider.OnThumbDragStarted));
			EventManager.RegisterClassHandler(typeof(CustomSlider), Thumb.DragDeltaEvent, new DragDeltaEventHandler(CustomSlider.OnThumbDragDelta));
			EventManager.RegisterClassHandler(typeof(CustomSlider), Thumb.DragCompletedEvent, new DragCompletedEventHandler(CustomSlider.OnThumbDragCompleted));

			// Listen to MouseLeftButtonDown event to determine if slide should move focus to itself
			EventManager.RegisterClassHandler(typeof(CustomSlider), Mouse.MouseDownEvent, new MouseButtonEventHandler(CustomSlider._OnMouseLeftButtonDown), true);

			DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomSlider), new FrameworkPropertyMetadata(typeof(CustomSlider)));
		}

		#endregion Constructors
		#region Commands
		/// <summary>
		/// Increase CustomSlider value
		/// </summary>
		public static RoutedCommand IncreaseLarge { get; private set; } = null;
		/// <summary>
		/// Decrease CustomSlider value
		/// </summary>
		public static RoutedCommand DecreaseLarge { get; private set; } = null;
		/// <summary>
		/// Increase CustomSlider value
		/// </summary>
		public static RoutedCommand IncreaseSmall { get; private set; } = null;
		/// <summary>
		/// Decrease CustomSlider value
		/// </summary>
		public static RoutedCommand DecreaseSmall { get; private set; } = null;
		/// <summary>
		/// Set CustomSlider value to mininum
		/// </summary>
		public static RoutedCommand MinimizeValue { get; private set; } = null;
		/// <summary>
		/// Set CustomSlider value to maximum
		/// </summary>
		public static RoutedCommand MaximizeValue { get; private set; } = null;

		static void InitializeCommands()
		{
			IncreaseLarge = new RoutedCommand("IncreaseLarge", typeof(CustomSlider));
			DecreaseLarge = new RoutedCommand("DecreaseLarge", typeof(CustomSlider));
			IncreaseSmall = new RoutedCommand("IncreaseSmall", typeof(CustomSlider));
			DecreaseSmall = new RoutedCommand("DecreaseSmall", typeof(CustomSlider));
			MinimizeValue = new RoutedCommand("MinimizeValue", typeof(CustomSlider));
			MaximizeValue = new RoutedCommand("MaximizeValue", typeof(CustomSlider));

			RegisterCommandHandler(typeof(CustomSlider), IncreaseLarge, new ExecutedRoutedEventHandler(OnIncreaseLargeCommand),
												  new SliderGesture(Key.PageUp, Key.PageDown, false));

			RegisterCommandHandler(typeof(CustomSlider), DecreaseLarge, new ExecutedRoutedEventHandler(OnDecreaseLargeCommand),
												  new SliderGesture(Key.PageDown, Key.PageUp, false));

			RegisterCommandHandler(typeof(CustomSlider), IncreaseSmall, new ExecutedRoutedEventHandler(OnIncreaseSmallCommand),
												  new SliderGesture(Key.Up, Key.Down, false),
												  new SliderGesture(Key.Right, Key.Left, true));

			RegisterCommandHandler(typeof(CustomSlider), DecreaseSmall, new ExecutedRoutedEventHandler(OnDecreaseSmallCommand),
												  new SliderGesture(Key.Down, Key.Up, false),
												  new SliderGesture(Key.Left, Key.Right, true));

			RegisterCommandHandler(typeof(CustomSlider), MinimizeValue, new ExecutedRoutedEventHandler(OnMinimizeValueCommand),
												  Key.Home);

			RegisterCommandHandler(typeof(CustomSlider), MaximizeValue, new ExecutedRoutedEventHandler(OnMaximizeValueCommand),
												  Key.End);

		}

		private static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler,
			params InputGesture[] inputGestures)
		{
			CommandManager.RegisterClassCommandBinding(controlType, new CommandBinding(command, executedRoutedEventHandler, null));

			if (inputGestures != null)
			{
				for (int i = 0; i < inputGestures.Length; i++)
				{
					CommandManager.RegisterClassInputBinding(controlType, new InputBinding(command, inputGestures[i]));
				}
			}

		}

		private static void RegisterCommandHandler(Type type, RoutedCommand routedCommand, ExecutedRoutedEventHandler executedRoutedEventHandler,
			Key key)
		{
			RegisterCommandHandler(type, routedCommand, executedRoutedEventHandler, new KeyGesture(key));
		}


		private class SliderGesture : InputGesture
		{
			public SliderGesture(Key normal, Key inverted, bool forHorizontal)
			{
				_normal = normal;
				_inverted = inverted;
				_forHorizontal = forHorizontal;
			}

			/// <summary>
			/// Sees if the InputGesture matches the input associated with the inputEventArgs
			/// </summary>
			public override bool Matches(object targetElement, InputEventArgs inputEventArgs)
			{
				if (inputEventArgs is KeyEventArgs keyEventArgs && targetElement is CustomSlider slider && Keyboard.Modifiers == ModifierKeys.None)
				{
					if ((int)_normal == (int)GetRealKey(keyEventArgs))
					{
						return !IsInverted(slider);
					}
					if ((int)_inverted == (int)GetRealKey(keyEventArgs))
					{
						return IsInverted(slider);
					}
				}
				return false;
			}

			private static Key GetRealKey(KeyEventArgs keyEvent)
			{
				switch (keyEvent.Key)
				{
					case Key.System:
						return keyEvent.SystemKey;
					case Key.ImeProcessed:
						return keyEvent.ImeProcessedKey;
					case Key.DeadCharProcessed:
						return keyEvent.DeadCharProcessedKey;
					default:
						return keyEvent.Key;
				}
			}


			private bool IsInverted(CustomSlider slider)
			{
				if (_forHorizontal)
				{
					return slider.IsDirectionReversed != (slider.FlowDirection == FlowDirection.RightToLeft);
				}
				else
				{
					return slider.IsDirectionReversed;
				}
			}

			private readonly Key _normal;
			private readonly Key _inverted;
			private readonly bool _forHorizontal;
		}



		private static void OnIncreaseSmallCommand(object sender, ExecutedRoutedEventArgs e)
		{
			if (sender is CustomSlider slider)
			{
				slider.OnIncreaseSmall();
			}
		}

		private static void OnDecreaseSmallCommand(object sender, ExecutedRoutedEventArgs e)
		{
			if (sender is CustomSlider slider)
			{
				slider.OnDecreaseSmall();
			}
		}

		private static void OnMaximizeValueCommand(object sender, ExecutedRoutedEventArgs e)
		{
			if (sender is CustomSlider slider)
			{
				slider.OnMaximizeValue();
			}
		}

		private static void OnMinimizeValueCommand(object sender, ExecutedRoutedEventArgs e)
		{
			if (sender is CustomSlider slider)
			{
				slider.OnMinimizeValue();
			}
		}

		private static void OnIncreaseLargeCommand(object sender, ExecutedRoutedEventArgs e)
		{
			if (sender is CustomSlider slider)
			{
				slider.OnIncreaseLarge();
			}
		}

		private static void OnDecreaseLargeCommand(object sender, ExecutedRoutedEventArgs e)
		{
			if (sender is CustomSlider slider)
			{
				slider.OnDecreaseLarge();
			}
		}

		#endregion Commands

		#region Properties

		#region Orientation Property

		/// <summary>
		/// DependencyProperty for <see cref="Orientation" /> property.
		/// </summary>
		public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation),
			typeof(CustomSlider), new FrameworkPropertyMetadata(Orientation.Horizontal), new ValidateValueCallback(IsValidOrientation));

		internal static bool IsValidOrientation(object o)
		{
			Orientation value = (Orientation)o;
			return value == Orientation.Horizontal
				|| value == Orientation.Vertical;
		}

		/// <summary>
		/// Get/Set Orientation property
		/// </summary>
		public Orientation Orientation
		{
			get => (Orientation)GetValue(OrientationProperty);
			set => SetValue(OrientationProperty, value);
		}

		#endregion

		#region IsDirectionReversed Property
		/// <summary>
		/// CustomSlider ThumbProportion property
		/// </summary>
		public static readonly DependencyProperty IsDirectionReversedProperty
			= DependencyProperty.Register("IsDirectionReversed", typeof(bool), typeof(CustomSlider),
										  new FrameworkPropertyMetadata(false));

		/// <summary>
		/// Get/Set IsDirectionReversed property
		/// </summary>
		[Bindable(true), Category("Appearance")]
		public bool IsDirectionReversed
		{
			get => (bool)GetValue(IsDirectionReversedProperty);
			set => SetValue(IsDirectionReversedProperty, value);
		}
		#endregion

		#region Delay Property
		/// <summary>
		///     The Property for the Delay property.
		/// </summary>
		public static readonly DependencyProperty DelayProperty = RepeatButton.DelayProperty.AddOwner(typeof(CustomSlider),
			new FrameworkPropertyMetadata(GetKeyboardDelay()));

		internal static int GetKeyboardDelay()
		{
			int delay = SystemParameters.KeyboardDelay;
			// SPI_GETKEYBOARDDELAY 0,1,2,3 correspond to 250,500,750,1000ms
			if (delay < 0 || delay > 3)
				delay = 0;
			return (delay + 1) * 250;
		}


		/// <summary>
		///     Specifies the amount of time, in milliseconds, to wait before repeating begins.
		/// Must be non-negative.
		/// </summary>
		[Bindable(true), Category("Behavior")]
		public int Delay
		{
			get => (int)GetValue(DelayProperty);
			set => SetValue(DelayProperty, value);
		}

		#endregion Delay Property

		#region Interval Property
		/// <summary>
		///     The Property for the Interval property.
		/// </summary>
		public static readonly DependencyProperty IntervalProperty = RepeatButton.IntervalProperty.AddOwner(typeof(CustomSlider), new FrameworkPropertyMetadata(GetKeyboardSpeed()));

		internal static int GetKeyboardSpeed()
		{
			int speed = SystemParameters.KeyboardSpeed;
			// SPI_GETKEYBOARDSPEED 0,...,31 correspond to 1000/2.5=400,...,1000/30 ms
			if (speed < 0 || speed > 31)
				speed = 31;
			return (31 - speed) * (400 - 1000 / 30) / 31 + 1000 / 30;
		}

		/// <summary>
		///     Specifies the amount of time, in milliseconds, between repeats once repeating starts.
		/// Must be non-negative
		/// </summary>
		[Bindable(true), Category("Behavior")]
		public int Interval
		{
			get => (int)GetValue(IntervalProperty);
			set => SetValue(IntervalProperty, value);
		}

		#endregion Interval Property

		#region AutoToolTipPlacement Property
		/// <summary>
		///     The DependencyProperty for the AutoToolTipPlacement property.
		/// </summary>
		public static readonly DependencyProperty AutoToolTipPlacementProperty
			= DependencyProperty.Register("AutoToolTipPlacement", typeof(AutoToolTipPlacement), typeof(CustomSlider),
										  new FrameworkPropertyMetadata(AutoToolTipPlacement.None),
										  new ValidateValueCallback(IsValidAutoToolTipPlacement));

		/// <summary>
		///     AutoToolTipPlacement property specifies the placement of the AutoToolTip
		/// </summary>
		[Bindable(true), Category("Behavior")]
		public AutoToolTipPlacement AutoToolTipPlacement
		{
			get => (AutoToolTipPlacement)GetValue(AutoToolTipPlacementProperty);
			set => SetValue(AutoToolTipPlacementProperty, value);
		}

		private static bool IsValidAutoToolTipPlacement(object o)
		{
			AutoToolTipPlacement placement = (AutoToolTipPlacement)o;
			return placement == AutoToolTipPlacement.None ||
				   placement == AutoToolTipPlacement.TopLeft ||
				   placement == AutoToolTipPlacement.BottomRight;
		}

		#endregion

		#region AutoToolTipPrecision Property
		/// <summary>
		///     The DependencyProperty for the AutoToolTipPrecision property.
		///     Flags:              None
		///     Default Value:      0
		/// </summary>
		public static readonly DependencyProperty AutoToolTipPrecisionProperty
			= DependencyProperty.Register("AutoToolTipPrecision", typeof(int), typeof(CustomSlider),
			new FrameworkPropertyMetadata(0), new ValidateValueCallback(IsValidAutoToolTipPrecision));

		/// <summary>
		///     Get or set number of decimal digits of CustomSlider's Value shown in AutoToolTip
		/// </summary>
		[Bindable(true), Category("Appearance")]
		public int AutoToolTipPrecision
		{
			get => (int)GetValue(AutoToolTipPrecisionProperty);
			set => SetValue(AutoToolTipPrecisionProperty, value);
		}

		/// <summary>
		/// Validates AutoToolTipPrecision value
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		private static bool IsValidAutoToolTipPrecision(object o)
		{
			return (((int)o) >= 0);
		}

		#endregion

		/*
         * TickMark support
         *
         *   - double           TickFrequency
         *   - bool             IsSnapToTickEnabled
         *   - Enum             TickPlacement
         *   - DoubleCollection Ticks
         */
		#region TickMark support
		/// <summary>
		///     The DependencyProperty for the IsSnapToTickEnabled property.
		/// </summary>
		public static readonly DependencyProperty IsSnapToTickEnabledProperty
			= DependencyProperty.Register("IsSnapToTickEnabled", typeof(bool), typeof(CustomSlider),
			new FrameworkPropertyMetadata(false));

		/// <summary>
		///     When 'true', CustomSlider will automatically move the Thumb (and/or change current value) to the closest TickMark.
		/// </summary>
		[Bindable(true), Category("Behavior")]
		public bool IsSnapToTickEnabled
		{
			get => (bool)GetValue(IsSnapToTickEnabledProperty);
			set => SetValue(IsSnapToTickEnabledProperty, value);
		}

		private static readonly DependencyPropertyKey GetThumbWidthPropertyKey
			= DependencyProperty.RegisterReadOnly("GetThumbWidth", typeof(double), typeof(CustomSlider),
			new FrameworkPropertyMetadata(0.0));

		public static readonly DependencyProperty GetThumbWidthProperty = GetThumbWidthPropertyKey.DependencyProperty;
		/// <summary>
		///     When 'true', CustomSlider will automatically move the Thumb (and/or change current value) to the closest TickMark.
		/// </summary>
		[Bindable(false)]
		public double GetThumbWidth
		{
			get => (double)GetValue(GetThumbWidthProperty);
		}

		/// <summary>
		///     The DependencyProperty for the TickPlacement property.
		/// </summary>
		public static readonly DependencyProperty TickPlacementProperty
			= DependencyProperty.Register("TickPlacement", typeof(TickPlacement), typeof(CustomSlider),
				new FrameworkPropertyMetadata(TickPlacement.None), new ValidateValueCallback(IsValidTickPlacement));

		/// <summary>
		///     CustomSlider uses this value to determine where to show the Ticks.
		/// When Ticks is not 'null', CustomSlider will ignore 'TickFrequency', and draw only TickMarks
		/// that specified in Ticks collection.
		/// </summary>
		[Bindable(true), Category("Appearance")]
		public TickPlacement TickPlacement
		{
			get => (TickPlacement)GetValue(TickPlacementProperty);
			set => SetValue(TickPlacementProperty, value);
		}

		private static bool IsValidTickPlacement(object o)
		{
			TickPlacement value = (TickPlacement)o;
			bool retVal = value == TickPlacement.None ||
				   value == TickPlacement.TopLeft ||
				   value == TickPlacement.BottomRight ||
				   value == TickPlacement.Both;
			return retVal;
		}

		/// <summary>
		///     The DependencyProperty for the TickFrequency property.
		///     Default Value is 1.0
		/// </summary>
		public static readonly DependencyProperty TickFrequencyProperty
			= DependencyProperty.Register("TickFrequency", typeof(double), typeof(CustomSlider),
			new FrameworkPropertyMetadata(1.0),
			new ValidateValueCallback(IsValidDoubleValue));

		/// <summary>
		///     CustomSlider uses this value to determine where to show the Ticks.
		/// When Ticks is not 'null', CustomSlider will ignore 'TickFrequency', and draw only TickMarks
		/// that specified in Ticks collection.
		/// </summary>
		[Bindable(true), Category("Appearance")]
		public double TickFrequency
		{
			get => (double)GetValue(TickFrequencyProperty);
			set => SetValue(TickFrequencyProperty, value);
		}

		// 

		/// <summary>
		///     The DependencyProperty for the Ticks property.
		/// </summary>
		public static readonly DependencyProperty TicksProperty
			= DependencyProperty.Register("Ticks", typeof(DoubleCollection), typeof(CustomSlider),
			new FrameworkPropertyMetadata(new DoubleCollection().GetAsFrozen()));

		/// <summary>
		///     CustomSlider uses this value to determine where to show the Ticks.
		/// When Ticks is not 'null', CustomSlider will ignore 'TickFrequency', and draw only TickMarks
		/// that specified in Ticks collection.
		/// </summary>
		[Bindable(true), Category("Appearance")]
		public DoubleCollection Ticks
		{
			get => (DoubleCollection)GetValue(TicksProperty);
			set => SetValue(TicksProperty, value);
		}
		#endregion TickMark support

		/*
         * Selection support
         *
         *   - bool   IsSelectionRangeEnabled
         *   - double SelectionStart
         *   - double SelectionEnd
         */

		#region Selection supports

		/// <summary>
		///     The DependencyProperty for the IsSelectionRangeEnabled property.
		/// </summary>
		public static readonly DependencyProperty IsSelectionRangeEnabledProperty
			= DependencyProperty.Register("IsSelectionRangeEnabled", typeof(bool), typeof(CustomSlider),
			new FrameworkPropertyMetadata(false));

		/// <summary>
		///     Enable or disable selection support on CustomSlider
		/// </summary>
		[Bindable(true), Category("Appearance")]
		public bool IsSelectionRangeEnabled
		{
			get => (bool)GetValue(IsSelectionRangeEnabledProperty);
			set => SetValue(IsSelectionRangeEnabledProperty, value);
		}

		/// <summary>
		///     The DependencyProperty for the SelectionStart property.
		/// </summary>
		public static readonly DependencyProperty SelectionStartProperty
			= DependencyProperty.Register("SelectionStart", typeof(double), typeof(CustomSlider),
					new FrameworkPropertyMetadata(0.0d,
						FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
						new PropertyChangedCallback(OnSelectionStartChanged),
						new CoerceValueCallback(CoerceSelectionStart)),
					new ValidateValueCallback(IsValidDoubleValue));

		/// <summary>
		///     Get or set starting value of selection.
		/// </summary>
		[Bindable(true), Category("Appearance")]
		public double SelectionStart
		{
			get { return (double)GetValue(SelectionStartProperty); }
			set { SetValue(SelectionStartProperty, value); }
		}

		private static void OnSelectionStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			CustomSlider ctrl = (CustomSlider)d;
			double oldValue = (double)e.OldValue;
			double newValue = (double)e.NewValue;

			ctrl.CoerceValue(SelectionEndProperty);
			ctrl.UpdateSelectionRangeElementPositionAndSize();
		}

		private static object CoerceSelectionStart(DependencyObject d, object value)
		{
			CustomSlider slider = (CustomSlider)d;
			double selection = (double)value;

			double min = slider.Minimum;
			double max = slider.Maximum;

			if (selection < min)
			{
				return min;
			}
			if (selection > max)
			{
				return max;
			}
			return value;
		}

		/// <summary>
		///     The DependencyProperty for the SelectionEnd property.
		/// </summary>
		public static readonly DependencyProperty SelectionEndProperty
			= DependencyProperty.Register("SelectionEnd", typeof(double), typeof(CustomSlider),
					new FrameworkPropertyMetadata(0.0d,
						FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
						new PropertyChangedCallback(OnSelectionEndChanged),
						new CoerceValueCallback(CoerceSelectionEnd)),
					new ValidateValueCallback(IsValidDoubleValue));

		/// <summary>
		///     Get or set starting value of selection.
		/// </summary>
		[Bindable(true), Category("Appearance")]
		public double SelectionEnd
		{
			get { return (double)GetValue(SelectionEndProperty); }
			set { SetValue(SelectionEndProperty, value); }
		}

		private static void OnSelectionEndChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			CustomSlider ctrl = (CustomSlider)d;
			ctrl.UpdateSelectionRangeElementPositionAndSize();
		}

		private static object CoerceSelectionEnd(DependencyObject d, object value)
		{
			CustomSlider slider = (CustomSlider)d;
			double selection = (double)value;

			double min = slider.SelectionStart;
			double max = slider.Maximum;

			if (selection < min)
			{
				return min;
			}
			if (selection > max)
			{
				return max;
			}
			return value;
		}

		/// <summary>
		///     Called when the value of SelectionEnd is required by the property system.
		/// </summary>
		/// <param name="d">The object on which the property was queried.</param>
		/// <returns>The value of the SelectionEnd property on "d."</returns>
		private static object OnGetSelectionEnd(DependencyObject d)
		{
			return ((CustomSlider)d).SelectionEnd;
		}

		/// <summary>
		///     This method is invoked when the Minimum property changes.
		/// </summary>
		/// <param name="oldMinimum">The old value of the Minimum property.</param>
		/// <param name="newMinimum">The new value of the Minimum property.</param>
		protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
		{
			CoerceValue(SelectionStartProperty);
		}

		/// <summary>
		///     This method is invoked when the Maximum property changes.
		/// </summary>
		/// <param name="oldMaximum">The old value of the Maximum property.</param>
		/// <param name="newMaximum">The new value of the Maximum property.</param>
		protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
		{
			CoerceValue(SelectionStartProperty);
			CoerceValue(SelectionEndProperty);
		}

		#endregion Selection supports

		/*
         * Move-To-Point support
         *
         * Property
         *   - bool   IsMoveToPointEnabled
         *
         * Event Handlers
         *   - OnPreviewMouseLeftButtonDown
         *   - double SelectionEnd
         */
		#region Move-To-Point support

		/// <summary>
		///     The DependencyProperty for the IsMoveToPointEnabled property.
		/// </summary>
		public static readonly DependencyProperty IsMoveToPointEnabledProperty
			= DependencyProperty.Register("IsMoveToPointEnabled", typeof(bool), typeof(CustomSlider),
			new FrameworkPropertyMetadata(false));

		/// <summary>
		///     Enable or disable Move-To-Point support on CustomSlider.
		///     Move-To-Point feature, enables CustomSlider to immediately move the Thumb directly to the location where user
		/// clicked the Mouse.
		/// </summary>
		[Bindable(true), Category("Behavior")]
		public bool IsMoveToPointEnabled
		{
			get => (bool)GetValue(IsMoveToPointEnabledProperty);
			set => SetValue(IsMoveToPointEnabledProperty, value);
		}

		/// <summary>
		/// When IsMoveToPointEneabled is 'true', CustomSlider needs to preview MouseLeftButtonDown event, in order prevent its RepeatButtons
		/// from handle Left-Click.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
		{

			if (IsMoveToPointEnabled && Track != null && Track.Thumb != null && !Track.Thumb.IsMouseOver)
			{
				// Move Thumb to the Mouse location

				Point pt = e.MouseDevice.GetPosition(Track);

				double newValue = Track.ValueFromPoint(pt);
				if (IsDoubleFinite(newValue))
				{
					UpdateValue(newValue);
				}
				e.Handled = true;
			}

			base.OnPreviewMouseLeftButtonDown(e);
		}

		internal static bool IsDoubleFinite(object o)
		{
			double d = (double)o;
			return !(double.IsInfinity(d) || double.IsNaN(d));
		}


		#endregion Move-To-Point support

		#endregion // Properties


		#region Event Handlers
		/// <summary>
		/// Listen to Thumb DragStarted event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void OnThumbDragStarted(object sender, DragStartedEventArgs e)
		{
			CustomSlider slider = sender as CustomSlider;
			slider.OnThumbDragStarted(e);
		}

		/// <summary>
		/// Listen to Thumb DragDelta event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void OnThumbDragDelta(object sender, DragDeltaEventArgs e)
		{
			CustomSlider slider = sender as CustomSlider;

			slider.OnThumbDragDelta(e);
		}

		/// <summary>
		/// Listen to Thumb DragCompleted event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void OnThumbDragCompleted(object sender, DragCompletedEventArgs e)
		{
			CustomSlider slider = sender as CustomSlider;
			slider.OnThumbDragCompleted(e);
		}

		/// <summary>
		/// Called when user start dragging the Thumb.
		/// This function can be override to customize the way CustomSlider handles Thumb movement.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnThumbDragStarted(DragStartedEventArgs e)
		{
			// Show AutoToolTip if needed.

			if ((!(e.OriginalSource is Thumb thumb)) || (this.AutoToolTipPlacement == AutoToolTipPlacement.None))
			{
				return;
			}

			// Save original tooltip
			_thumbOriginalToolTip = thumb.ToolTip;

			if (_autoToolTip == null)
			{
				_autoToolTip = new ToolTip
				{
					Placement = PlacementMode.Custom,
					PlacementTarget = thumb,
					CustomPopupPlacementCallback = new CustomPopupPlacementCallback(this.AutoToolTipCustomPlacementCallback)
				};
			}

			thumb.ToolTip = _autoToolTip;
			_autoToolTip.Content = GetAutoToolTipNumber();
			_autoToolTip.IsOpen = true;
			RepositionPopup((Popup)_autoToolTip.Parent);
		}

		/// <summary>
		/// Called when user dragging the Thumb.
		/// This function can be override to customize the way CustomSlider handles Thumb movement.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnThumbDragDelta(DragDeltaEventArgs e)
		{
			Thumb thumb = e.OriginalSource as Thumb;
			// Convert to Track's co-ordinate
			if (Track != null && thumb == Track.Thumb)
			{

				double newValue = Value + Track.ValueFromDistance(e.HorizontalChange, e.VerticalChange);
				if (IsDoubleFinite(newValue))
				{
					UpdateValue(newValue);
				}

				// Show AutoToolTip if needed
				if (this.AutoToolTipPlacement != AutoToolTipPlacement.None)
				{
					if (_autoToolTip == null)
					{
						_autoToolTip = new ToolTip();
					}

					_autoToolTip.Content = GetAutoToolTipNumber();

					if (thumb.ToolTip != _autoToolTip)
					{
						thumb.ToolTip = _autoToolTip;
					}

					if (!_autoToolTip.IsOpen)
					{
						_autoToolTip.IsOpen = true;
					}
					RepositionPopup((Popup)_autoToolTip.Parent);
				}
			}
		}

		//only way i could find to trigger reposition without tearing my hear out. hacky af, but whatever. 
		private void RepositionPopup(Popup parent)
		{
			var offset = parent.HorizontalOffset;
			parent.SetCurrentValue(Popup.HorizontalOffsetProperty, offset + 1);
			parent.SetCurrentValue(Popup.HorizontalOffsetProperty, offset);
		}

		private string GetAutoToolTipNumber()
		{
			NumberFormatInfo format = (NumberFormatInfo)(NumberFormatInfo.CurrentInfo.Clone());
			format.NumberDecimalDigits = this.AutoToolTipPrecision;
			return this.Value.ToString("N", format);
		}

		/// <summary>
		/// Called when user stop dragging the Thumb.
		/// This function can be override to customize the way CustomSlider handles Thumb movement.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnThumbDragCompleted(DragCompletedEventArgs e)
		{
			// Show AutoToolTip if needed.

			if ((!(e.OriginalSource is Thumb thumb)) || (this.AutoToolTipPlacement == AutoToolTipPlacement.None))
			{
				return;
			}

			if (_autoToolTip != null)
			{
				_autoToolTip.IsOpen = false;
			}

			thumb.ToolTip = _thumbOriginalToolTip;
		}


		private CustomPopupPlacement[] AutoToolTipCustomPlacementCallback(Size popupSize, Size targetSize, Point offset)
		{
			switch (this.AutoToolTipPlacement)
			{
				case AutoToolTipPlacement.TopLeft:
					if (Orientation == Orientation.Horizontal)
					{
						// Place popup at top of thumb
						return new CustomPopupPlacement[]{new CustomPopupPlacement(
							new Point((targetSize.Width - popupSize.Width) * 0.5, -popupSize.Height),
							PopupPrimaryAxis.Horizontal)
						};
					}
					else
					{
						// Place popup at left of thumb
						return new CustomPopupPlacement[] {
							new CustomPopupPlacement(
							new Point(-popupSize.Width, (targetSize.Height - popupSize.Height) * 0.5),
							PopupPrimaryAxis.Vertical)
						};
					}

				case AutoToolTipPlacement.BottomRight:
					if (Orientation == Orientation.Horizontal)
					{
						// Place popup at bottom of thumb
						return new CustomPopupPlacement[] {
							new CustomPopupPlacement(
							new Point((targetSize.Width - popupSize.Width) * 0.5, targetSize.Height) ,
							PopupPrimaryAxis.Horizontal)
						};

					}
					else
					{
						// Place popup at right of thumb
						return new CustomPopupPlacement[] {
							new CustomPopupPlacement(
							new Point(targetSize.Width, (targetSize.Height - popupSize.Height) * 0.5),
							PopupPrimaryAxis.Vertical)
						};
					}

				default:
					return new CustomPopupPlacement[] { };
			}
		}


		/// <summary>
		/// Resize and resposition the SelectionRangeElement.
		/// </summary>
		private void UpdateSelectionRangeElementPositionAndSize()
		{
			Size trackSize = new Size(0d, 0d);
			Size thumbSize = new Size(0d, 0d);

			if (Track == null || DoubleUtil.LessThan(SelectionEnd, SelectionStart))
			{
				return;
			}

			trackSize = Track.RenderSize;
			thumbSize = (Track.Thumb != null) ? Track.Thumb.RenderSize : new Size(0d, 0d);

			double thumbWidth = Track?.Thumb.ActualWidth ?? 0;
			SetValue(GetThumbWidthPropertyKey, thumbWidth);

			double range = Maximum - Minimum;
			double valueToSize;


			if (!(this.SelectionRangeElement is FrameworkElement rangeElement))
			{
				return;
			}

			if (Orientation == Orientation.Horizontal)
			{
				// Calculate part size for HorizontalSlider
				if (DoubleUtil.AreClose(range, 0d) || (DoubleUtil.AreClose(trackSize.Width, thumbSize.Width)))
				{
					valueToSize = 0d;
				}
				else
				{
					valueToSize = Math.Max(0.0, (trackSize.Width - thumbSize.Width) / range);
				}

				rangeElement.Width = ((SelectionEnd - SelectionStart) * valueToSize);
				if (IsDirectionReversed)
				{
					Canvas.SetLeft(rangeElement, (thumbSize.Width * 0.5) + Math.Max(Maximum - SelectionEnd, 0) * valueToSize);
				}
				else
				{
					Canvas.SetLeft(rangeElement, (thumbSize.Width * 0.5) + Math.Max(SelectionStart - Minimum, 0) * valueToSize);
				}
			}
			else
			{
				// Calculate part size for VerticalSlider
				if (DoubleUtil.AreClose(range, 0d) || (DoubleUtil.AreClose(trackSize.Height, thumbSize.Height)))
				{
					valueToSize = 0d;
				}
				else
				{
					valueToSize = Math.Max(0.0, (trackSize.Height - thumbSize.Height) / range);
				}

				rangeElement.Height = ((SelectionEnd - SelectionStart) * valueToSize);
				if (IsDirectionReversed)
				{
					Canvas.SetTop(rangeElement, (thumbSize.Height * 0.5) + Math.Max(SelectionStart - Minimum, 0) * valueToSize);
				}
				else
				{
					Canvas.SetTop(rangeElement, (thumbSize.Height * 0.5) + Math.Max(Maximum - SelectionEnd, 0) * valueToSize);
				}


			}
		}


		/// <summary>
		/// Gets or sets reference to CustomSlider's Track element.
		/// </summary>
		internal Track Track
		{
			get => _track;
			set => _track = value;
		}



		/// <summary>
		/// Gets or sets reference to CustomSlider's SelectionRange element.
		/// </summary>
		internal FrameworkElement SelectionRangeElement
		{
			get => _selectionRangeElement;
			set => _selectionRangeElement = value;
		}

		private bool ticksValid
		{
			get
			{
				if (ReadLocalValue(TicksProperty) == DependencyProperty.UnsetValue)
				{
					return false;
				}
				else
				{
					return Ticks?.Count > 0;
				}

			}
		}
		//prevent tick snapping if we cannot snap to ticks - that is, if a tick collection is expressly set to null or empty, or 
		//is implicitely empty, and the tick frequency is not set. If both are true, it's impossible to snap to ticks. The WPF default behavior 
		//is to force a snap to min or max. IMO that's dumb. This 
		public bool CanSnapToTicks => IsSnapToTickEnabled ? ticksValid ? true : TickFrequency != 0 : false;

		/// <summary>
		/// Snap the input 'value' to the closest tick.
		/// If input value is exactly in the middle of 2 surrounding ticks, it will be snapped to the tick that has greater value.
		/// </summary>
		/// <param name="value">Value that want to snap to closest Tick.</param>
		/// <returns>Snapped value if IsSnapToTickEnabled is 'true'. Otherwise, returns un-snaped value.</returns>
		private double SnapToTick(double value)
		{
			if (CanSnapToTicks)
			{
				if (ticksValid)
				{
					DoubleCollection ticks = Ticks;
					if (ticks.Count == 1)
					{
						value = ticks[0];
					}
					else
					{
						double? min = null, max = null;
						for (int i = 0; i < ticks.Count; i++)
						{
							double tick = ticks[i];
							if (DoubleUtil.AreClose(tick, value))
							{
								return value;
							}

							if (tick < value && (min is null || tick > min))
							{
								min = tick;
							}
							else if (tick > value && (max is null || tick < max))
							{
								max = tick;
							}
						}
						//if min is null or diff between min and value > double.MaxValue, do max.
						if (min is null || (double)min - double.MinValue < value)
						{
							value = (double)max;
						}
						//otherwise, if max is null or diff between max and value > double.MaxValue, do min.
						else if (max is null || (double)max - double.MaxValue > value)
						{
							value = (double)min;
						}
						else
						{
							double minD = (double)min;
							double maxD = (double)max;
							//we're introducing slight rounding error here, but so too would be the add and multiply used by default.
							//This is also safer in the extremely rare case of 
							double minDiff = value - minD;
							double maxDiff = maxD - value;
							value = DoubleUtil.GreaterThanOrClose(minDiff, maxDiff) ? maxD : minD;
						}
					}
				}
				else
				{
					double previous, next;
					previous = Minimum + (Math.Round((value - Minimum) / TickFrequency) * TickFrequency);
					next = Math.Min(Maximum, previous + TickFrequency);
					value = DoubleUtil.GreaterThanOrClose(value, (previous + next) * 0.5) ? next : previous;
				}

				// Choose the closest value between previous and next. If tie, snap to 'next'.

			}

			return value;
		}

		// Sets Value = SnapToTick(value+direction), unless the result of SnapToTick is Value,
		// then it searches for the next tick greater(if direction is positive) than value
		// and sets Value to that tick
		private void MoveToNextTick(double direction)
		{
			if (direction != 0.0)
			{
				double value = this.Value;

				// Find the next value by snapping
				double next = SnapToTick(Math.Max(this.Minimum, Math.Min(this.Maximum, value + direction)));

				bool greaterThan = direction > 0; //search for the next tick greater than value?

				// If the snapping brought us back to value, find the next tick point
				if (next == value && CanSnapToTicks
					&& !(greaterThan && value == Maximum)  // Stop if searching up if already at Max
					&& !(!greaterThan && value == Minimum)) // Stop if searching down if already at Min
				{
					// This property is rarely set so let's try to avoid the GetValue
					// caching of the mutable default value
					if (ticksValid)
					{
						var ticks = Ticks;

						for (int i = 0; i < ticks.Count; i++)
						{
							double tick = ticks[i];

							// Find the smallest tick greater than value or the largest tick less than value
							if ((greaterThan && DoubleUtil.GreaterThan(tick, value) && (DoubleUtil.LessThan(tick, next) || next == value))
							 || (!greaterThan && DoubleUtil.LessThan(tick, value) && (DoubleUtil.GreaterThan(tick, next) || next == value)))
							{
								next = tick;
							}
						}
					}
					else
					{
						// Find the current tick we are at
						double tickNumber = Math.Round((value - Minimum) / TickFrequency);

						if (greaterThan)
							tickNumber += 1.0;
						else
							tickNumber -= 1.0;

						next = Minimum + tickNumber * TickFrequency;
					}
				}


				// Update if we've found a better value
				if (next != value)
				{
					this.SetCurrentValue(ValueProperty, next);
				}
			}
		}
		#endregion Event Handlers

		#region Override Functions

		/// <summary>
		/// Creates AutomationPeer (<see cref="UIElement.OnCreateAutomationPeer"/>)
		/// </summary>
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new CustomSliderAutomationPeer(this);
		}

		/// <summary>
		/// This is a class handler for MouseLeftButtonDown event.
		/// The purpose of this handle is to move input focus to CustomSlider when user pressed
		/// mouse left button on any part of slider that is not focusable.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void _OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton != MouseButton.Left) return;

			CustomSlider slider = (CustomSlider)sender;

			// When someone click on the CustomSlider's part, and it's not focusable
			// CustomSlider need to take the focus in order to process keyboard correctly
			if (!slider.IsKeyboardFocusWithin)
			{
				e.Handled = slider.Focus() || e.Handled;
			}
		}

		/// <summary>
		/// Perform arrangement of slider's children
		/// </summary>
		/// <param name="finalSize"></param>
		protected override Size ArrangeOverride(Size finalSize)
		{
			Size size = base.ArrangeOverride(finalSize);

			UpdateSelectionRangeElementPositionAndSize();

			return size;
		}

		/// <summary>
		/// Update SelectionRange Length.
		/// </summary>
		/// <param name="oldValue"></param>
		/// <param name="newValue"></param>
		protected override void OnValueChanged(double oldValue, double newValue)
		{
			base.OnValueChanged(oldValue, newValue);
			UpdateSelectionRangeElementPositionAndSize();
		}

		/// <summary>
		/// CustomSlider locates the SelectionRangeElement when its visual tree is created
		/// </summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			SelectionRangeElement = GetTemplateChild(SelectionRangeElementName) as FrameworkElement;
			Track = GetTemplateChild(TrackName) as Track;

			if (_autoToolTip != null)
			{
				_autoToolTip.PlacementTarget = Track?.Thumb;
			}
		}

		#endregion Override Functions

		#region Virtual Functions

		/// <summary>
		/// Call when CustomSlider.IncreaseLarge command is invoked.
		/// </summary>
		protected virtual void OnIncreaseLarge()
		{
			MoveToNextTick(this.LargeChange);
		}

		/// <summary>
		/// Call when CustomSlider.DecreaseLarge command is invoked.
		/// </summary>
		protected virtual void OnDecreaseLarge()
		{
			MoveToNextTick(-this.LargeChange);
		}

		/// <summary>
		/// Call when CustomSlider.IncreaseSmall command is invoked.
		/// </summary>
		protected virtual void OnIncreaseSmall()
		{
			MoveToNextTick(this.SmallChange);
		}

		/// <summary>
		/// Call when CustomSlider.DecreaseSmall command is invoked.
		/// </summary>
		protected virtual void OnDecreaseSmall()
		{
			MoveToNextTick(-this.SmallChange);
		}

		/// <summary>
		/// Call when CustomSlider.MaximizeValue command is invoked.
		/// </summary>
		protected virtual void OnMaximizeValue()
		{
			this.SetCurrentValue(ValueProperty, this.Maximum);
		}

		/// <summary>
		/// Call when CustomSlider.MinimizeValue command is invoked.
		/// </summary>
		protected virtual void OnMinimizeValue()
		{
			this.SetCurrentValue(ValueProperty, this.Minimum);
		}

		#endregion Virtual Functions

		#region Helper Functions
		/// <summary>
		/// Helper function for value update.
		/// This function will also snap the value to tick, if IsSnapToTickEnabled is true.
		/// </summary>
		/// <param name="value"></param>
		private void UpdateValue(double value)
		{
			double snappedValue = SnapToTick(value);

			if (snappedValue != Value)
			{
				SetCurrentValue(ValueProperty, Math.Max(Minimum, Math.Min(Maximum, snappedValue)));
			}
		}

		/// <summary>
		/// Validate input value in CustomSlider (LargeChange, SmallChange, SelectionStart, SelectionEnd, and TickFrequency).
		/// </summary>
		/// <param name="value"></param>
		/// <returns>Returns False if value is NaN or NegativeInfinity or PositiveInfinity. Otherwise, returns True.</returns>
		private static bool IsValidDoubleValue(object value)
		{
			double d = (double)value;

			return !(double.IsNaN(d) || double.IsInfinity(d));
		}


		#endregion Helper Functions


		#region Private Fields

		private const string TrackName = "PART_Track";
		private const string TopTickName = "TopTick";
		private const string BottomTickName = "BottomTick";
		private const string SelectionRangeElementName = "PART_SelectionRange";

		// CustomSlider required parts
		private FrameworkElement _selectionRangeElement;
		private Track _track;
		private ToolTip _autoToolTip = null;
		private object _thumbOriginalToolTip = null;

		#endregion Private Fields
	}
	///
	public class CustomSliderAutomationPeer : RangeBaseAutomationPeer
	{
		///
		public CustomSliderAutomationPeer(CustomSlider owner) : base(owner)
		{
		}

		///
		protected override string GetClassNameCore()
		{
			return "Slider";
		}

		///
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Slider;
		}

		///
		protected override Point GetClickablePointCore()
		{
			return new Point(double.NaN, double.NaN);
		}
	}
}

