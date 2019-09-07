using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CoCWinDesktop.CustomControls
{

	public class LimitedInputTextBox : TextBox
	{
		public static readonly DependencyProperty LimitCharactersRegexProperty = DependencyProperty.Register("LimitCharactersRegex", typeof(Regex), typeof(LimitedInputTextBox),
			new FrameworkPropertyMetadata(new Regex(".*")));

		public Regex LimitCharactersRegex
		{
			get => (Regex)GetValue(LimitCharactersRegexProperty);
			set => SetCurrentValue(LimitCharactersRegexProperty, value);
		}

		public static readonly DependencyProperty ValidStringRegexProperty = DependencyProperty.Register("ValidStringRegex", typeof(Regex), typeof(LimitedInputTextBox),
			new FrameworkPropertyMetadata(new Regex(".*"), FrameworkPropertyMetadataOptions.AffectsRender, OnValidStringRegexChanged));

		private static void OnValidStringRegexChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			(o as LimitedInputTextBox).CheckText();
		}

		public Regex ValidStringRegex
		{
			get => (Regex)GetValue(ValidStringRegexProperty);
			set => SetCurrentValue(ValidStringRegexProperty, value);
		}

		private static readonly DependencyPropertyKey IsTextValidPropertyKey = DependencyProperty.RegisterReadOnly("IsTextValid", typeof(bool), typeof(LimitedInputTextBox),
			new FrameworkPropertyMetadata(true));

		public static readonly DependencyProperty IsTextValidProperty = IsTextValidPropertyKey.DependencyProperty;
		public bool IsTextValid => (bool)GetValue(IsTextValidProperty);

		public static readonly DependencyProperty InvalidTextBorderBrushProperty = DependencyProperty.Register("InvalidTextBorderBrush", typeof(Brush),
			typeof(LimitedInputTextBox), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Red), FrameworkPropertyMetadataOptions.AffectsRender));

		public Brush InvalidTextBorderBrush
		{
			get => (Brush)GetValue(InvalidTextBorderBrushProperty);
			set => SetCurrentValue(InvalidTextBorderBrushProperty, value);
		}

		public LimitedInputTextBox()
		{
			ContextMenu = null;
			//CommandBinding binding = new CommandBinding(ApplicationCommands.Paste);
			//binding.CanExecute += CanPaste;
			//CommandBindings.Add(binding);
			DataObject.AddPastingHandler(this, CanPaste);
			this.TextChanged += LimitedInputTextBox_TextChanged;

			if (ValidStringRegex != null)
			{
				SetValue(IsTextValidPropertyKey, ValidStringRegex.IsMatch(Text));
			}
		}

		private void LimitedInputTextBox_TextChanged(object _, TextChangedEventArgs __)
		{
			CheckText();
		}

		private void CheckText()
		{
			if (ValidStringRegex != null)
			{
				SetValue(IsTextValidPropertyKey, ValidStringRegex.IsMatch(Text));
			}
		}

		protected override void OnPreviewTextInput(TextCompositionEventArgs e)
		{
			if (LimitCharactersRegex != null)
			{
				string full = Text.Insert(CaretIndex, e.Text);
				if (!LimitCharactersRegex.IsMatch(full))
				{
					e.Handled = true;
				}
			}
			base.OnPreviewTextInput(e);
		}

		private void CanPaste(object sender, DataObjectPastingEventArgs e)
		{
			bool isText = e.SourceDataObject.GetDataPresent(DataFormats.UnicodeText, true);
			if (!isText)
			{
				e.CancelCommand();
			}
			string text = e.SourceDataObject.GetData(DataFormats.UnicodeText) as string;
			if (LimitCharactersRegex != null && !LimitCharactersRegex.IsMatch(text))
			{
				e.CancelCommand();
			}
		}

		//private void CanPaste(object sender, CanExecuteRoutedEventArgs e)
		//{
		//	e.Parameter
		//	if ()
		//		e.CanExecute = false;
		//	e.Handled = true;
		//}
	}
}
