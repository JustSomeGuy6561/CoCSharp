using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CoCWinDesktop.CustomControls
{
	
	public class LimitedInputTextBox : TextBox
	{
		public static DependencyProperty LimitCharactersRegexProperty = DependencyProperty.Register("LimitCharactersRegex", typeof(Regex), typeof(LimitedInputTextBox),
			new FrameworkPropertyMetadata(new Regex(".*")));

		public Regex LimitCharactersRegex { get; set; } = new Regex(".*"); //defaults to everything. but null also has the same effect. 

		protected override void OnPreviewTextInput(TextCompositionEventArgs e)
		{
			if (LimitCharactersRegex != null && !LimitCharactersRegex.IsMatch(e.Text))
			{
				e.Handled = true;
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
		//	e.CanExecute = false;
		//	e.Handled = true;
		//}
	}
}
