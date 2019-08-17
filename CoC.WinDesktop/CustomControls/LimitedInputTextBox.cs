using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

		public LimitedInputTextBox()
		{
			ContextMenu = null;
			//CommandBinding binding = new CommandBinding(ApplicationCommands.Paste);
			//binding.CanExecute += CanPaste;
			//CommandBindings.Add(binding);
			DataObject.AddPastingHandler(this, CanPaste);
			//this.KeyDown += LimitedInputTextBox_KeyDown;
			//InputManager.Current.PostProcessInput += Current_PostProcessInput;
		}

		//private void LimitedInputTextBox_KeyDown(object sender, KeyEventArgs e)
		//{
		//	//if (e.OriginalSource == this)
		//	//{
		//	//	RaiseEvent(new TextCompositionEventArgs(e.KeyboardDevice,
		//	//		new TextComposition(InputManager.Current, this,))
		//	//	{
		//	//		RoutedEvent = TextCompositionManager.TextInputEvent
		//	//	});
		//	//}
		//}

		//private void Current_PostProcessInput(object sender, ProcessInputEventArgs e)
		//{
		//	//if (e.StagingItem.Input.Handled) return;

		//	if (e.StagingItem.Input.RoutedEvent == Keyboard.KeyDownEvent)
		//	{
		//		OnKeyDown((KeyEventArgs)e.StagingItem.Input);
		//	}

		//}

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
		//	e.CanExecute = false;
		//	e.Handled = true;
		//}
	}
}
