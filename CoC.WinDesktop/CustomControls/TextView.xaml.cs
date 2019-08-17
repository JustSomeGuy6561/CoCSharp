using CoCWinDesktop.ModelView.Helpers;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CoCWinDesktop.CustomControls
{
	//-----------NEEDS
	//maybe an inputOffset, for displaying. probably could be hardcoded.
	//Input - prop, string, two way, onFocusLost - stores the input field text.
	//inputSelectVisible - prop, Visibility (or bool, same as above)
	//inputSelect - prop, custom object - wraps the content of a comboBox
	//selectedInput - the returnValue for inputSelect. needs to be two way, source needs to implement some catch for change logic. 



	/// <summary>
	/// Interaction logic for TextViewVersion2.xaml
	/// </summary>
	public partial class TextView : UserControl
	{
		//used
		public static readonly DependencyProperty MainTextProperty = DependencyProperty.Register("MainText", typeof(string), typeof(TextView),
			new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, OnCall));
		private static void OnCall(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextView st = d as TextView;

			st.UpdateFlowDocument(true);
			st.InvalidateVisual();
		}

		public string MainText
		{
			get => (string)GetValue(MainTextProperty);
			set => SetCurrentValue(MainTextProperty, value);
		}

		//used
		public static readonly DependencyProperty MainImageProperty = DependencyProperty.Register("MainImage", typeof(BitmapImage), typeof(TextView),
			new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender, OnMainImageChanged));
		private static void OnMainImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextView document = d as TextView;

			document.UpdateFlowDocument(true);
			document.InvalidateVisual();
		}

		public BitmapImage MainImage
		{
			get => (BitmapImage)GetValue(MainImageProperty);
			set => SetCurrentValue(MainImageProperty, value);
		}


		//used
		public static readonly DependencyProperty PostControlTextProperty = DependencyProperty.Register("PostControlText", typeof(string), typeof(TextView),
			new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsMeasure, OnPostControlTextChanged));
		private static void OnPostControlTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextView document = d as TextView;

			document.UpdateFlowDocument(false);
			document.InvalidateVisual();
		}

		public string PostControlText
		{
			get => (string)GetValue(PostControlTextProperty);
			set => SetCurrentValue(PostControlTextProperty, value);
		}

		//Used
		public static readonly DependencyProperty InputFieldVisibleProperty = DependencyProperty.Register("InputFieldVisible", typeof(bool), typeof(TextView),
			new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsArrange));

		public bool InputFieldVisible
		{
			get => (bool)GetValue(InputFieldVisibleProperty);
			set => SetCurrentValue(InputFieldVisibleProperty, value);
		}

		//Used
		public static readonly DependencyProperty InputFieldMaxLengthProperty = DependencyProperty.Register("InputFieldMaxLength", typeof(int), typeof(TextView),
			new FrameworkPropertyMetadata(0));

		public int InputFieldMaxLength
		{
			get => (int)GetValue(InputFieldMaxLengthProperty);
			set => SetCurrentValue(InputFieldMaxLengthProperty, value);
		}

		//Used
		public static readonly DependencyProperty InputFieldTextProperty = DependencyProperty.Register("InputFieldText", typeof(string), typeof(TextView),
			new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public string InputFieldText
		{
			get => (string)GetValue(InputFieldTextProperty);
			set => SetCurrentValue(InputFieldTextProperty, value);
		}

		//Used
		public static readonly DependencyProperty InputCalculatedWidthProperty = DependencyProperty.Register("InputCalculatedWidth", typeof(double), typeof(TextView),
			new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure));

		public double InputCalculatedWidth
		{
			get => (double)GetValue(InputCalculatedWidthProperty);
			set => SetCurrentValue(InputCalculatedWidthProperty, value);
		}

		//Used
		public static readonly DependencyProperty LimitCharactersRegexProperty = DependencyProperty.Register("LimitCharactersRegex", typeof(Regex), typeof(TextView),
			new FrameworkPropertyMetadata(defaultValue: null));

		public Regex LimitCharactersRegex
		{
			get => (Regex)GetValue(LimitCharactersRegexProperty);
			set => SetCurrentValue(LimitCharactersRegexProperty, value);
		}


		//NYI
		public static readonly DependencyProperty DropdownVisibleProperty = DependencyProperty.Register("DropdownVisible", typeof(bool), typeof(TextView),
			new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsArrange));

		public bool DropdownVisible
		{
			get => (bool)GetValue(DropdownVisibleProperty);
			set => SetCurrentValue(DropdownVisibleProperty, value);
		}

		////NYI
		//public static readonly DependencyProperty DropdownSelectedItemProperty = DependencyProperty.Register("DropdownSelectedItem", typeof(ComboBoxItemWrapper), typeof(TextViewVersion2),
		//	new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
		//public ComboBoxItemWrapper DropdownSelectedItem { get; set; }

		//NYI
		public static readonly DependencyProperty DropdownContainerProperty = DependencyProperty.Register("DropdownContainer", typeof(ComboBoxWrapper), typeof(TextView),
			new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
		public ComboBoxWrapper DropdownContainer
		{
			get => (ComboBoxWrapper)GetValue(DropdownContainerProperty);
			set => SetCurrentValue(DropdownContainerProperty, value);
		}

		public TextView()
		{
			InitializeComponent();

			//Eat your heart out MVVM purists. 
			InputField.IsVisibleChanged += InputField_IsVisibleChanged;
			DropDown.IsVisibleChanged += DropDown_IsVisibleChanged;
		}




		//InputField gets primary focus.
		private static void InputField_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			var view = sender as LimitedInputTextBox;
			if ((bool)e.NewValue == true)
			{
				var result = Keyboard.Focus(view);
			}
		}

		private void DropDown_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			var view = sender as ComboBox;
			Grid grid = view.Parent as Grid;

			bool otherVisible = false;
			for (int x = 0; x < grid.Children.Count; x++)
			{
				var child = VisualTreeHelper.GetChild(grid, x);
				if (child is LimitedInputTextBox textBox && textBox.IsVisible)
				{
					otherVisible = true;
				}
			}

			if ((bool)e.NewValue == true && !otherVisible)
			{
				var result = Keyboard.Focus(view);
			}
		}




		private void UpdateFlowDocument(bool isPrimaryText)
		{
			//FlowDocument document = PrimaryText.Document;
			FlowDocument document = isPrimaryText ? PrimaryText.Document : SecondaryText.Document;
			//string text = MainText;
			string text = isPrimaryText ? MainText : PostControlText;
			bool doImage = isPrimaryText;


			document.Blocks.Clear();

			if (!string.IsNullOrWhiteSpace(text))
			{
				TextRange textRange = new TextRange(document.ContentStart, document.ContentEnd);
				Console.WriteLine(text);
				using (MemoryStream ms = new MemoryStream(Encoding.Default.GetBytes(text)))
				{
					textRange.Load(ms, DataFormats.Rtf); //Html is not supported (yet?) so i've done this workaround. 
				}
			}


			if (doImage && MainImage != null)
			{
				Block block = document.Blocks.FirstBlock;
				while (block != null && !(block is Paragraph))
				{
					block = block.NextBlock;
				}
				if (block is null)
				{
					if (document.Blocks.FirstBlock == null)
					{
						document.Blocks.Add(new BlockUIContainer(new Image() { Source = MainImage }));
					}
					else
					{
						document.Blocks.InsertBefore(document.Blocks.FirstBlock, new BlockUIContainer(new Image() { Source = MainImage }));
					}
				}
				else if (block is Paragraph paragraph)
				{
					Figure figure = new Figure(new BlockUIContainer(new Image() { VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, MaxHeight = 50, MaxWidth = 75, Stretch = Stretch.Uniform, StretchDirection = StretchDirection.Both, Source = MainImage }))
					{
						HorizontalAnchor = FigureHorizontalAnchor.PageLeft,
						VerticalAnchor = FigureVerticalAnchor.ParagraphTop,
						Width = new FigureLength(0.0825, FigureUnitType.Page),
					};
					paragraph.Inlines.InsertBefore(paragraph.Inlines.FirstInline, figure);
				}
			}
		}
	}
}
