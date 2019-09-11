using CoCWinDesktop.Helpers;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CoCWinDesktop.CustomControls
{

	public class ContentRichTextBox : RichTextBox
	{
		public static readonly DependencyProperty RTFContentProperty = DependencyProperty.Register("RTFContent", typeof(string), typeof(ContentRichTextBox),
			new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsMeasure, OnRTFContentChanged));

		private static void OnRTFContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs baseValue)
		{
			ContentRichTextBox contentBox = d as ContentRichTextBox;

			contentBox.UpdateFlowDocument();
			contentBox.InvalidateMeasure();
		}

		public string RTFContent
		{
			get => (string)GetValue(RTFContentProperty);
			internal set => SetValue(RTFContentProperty, value);
		}

		public static readonly DependencyProperty BitmapSourceProperty = DependencyProperty.Register("BitmapSource", typeof(BitmapImage), typeof(ContentRichTextBox),
			new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure, OnImageContentChanged));

		private static void OnImageContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs baseValue)
		{
			ContentRichTextBox contentBox = d as ContentRichTextBox;

			contentBox.UpdateFlowDocument();
			contentBox.InvalidateMeasure();
		}

		public BitmapImage BitmapSource
		{
			get => (BitmapImage)GetValue(BitmapSourceProperty);
			internal set => SetValue(BitmapSourceProperty, value);
		}

		public static readonly DependencyProperty ImageDimensionsProperty = DependencyProperty.Register("ImageDimensions", typeof(Size), typeof(ContentRichTextBox),
			new FrameworkPropertyMetadata(Size.Empty, FrameworkPropertyMetadataOptions.AffectsMeasure, OnImageSizeChanged));

		public ContentRichTextBox() : base()
		{
			//this.MouseDown += ContentRichTextBox_MouseDown;
			//this.PreviewMouseDown += ContentRichTextBox_PreviewMouseDown;
		}

		//private void ContentRichTextBox_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		//{
			

			

		//	TextRange textRange = new TextRange(Document.ContentStart, Document.ContentEnd);
		//	string result = null;
		//	using (MemoryStream ms = new MemoryStream())
		//	{
		//		textRange.Save(ms, DataFormats.Xaml); //Html is not supported (yet?) so i've done this workaround. 
		//		result = Encoding.Default.GetString(ms.ToArray());
		//	}
		//	Console.WriteLine(result);
		//}

		private void Link_Click(object sender, RoutedEventArgs e)
		{
			UrlHelper.OpenURL((sender as Hyperlink).NavigateUri.ToString());
		}

		//private void ContentRichTextBox_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		//{
			
		//}

		private static void OnImageSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs baseValue)
		{
			ContentRichTextBox contentBox = d as ContentRichTextBox;

			contentBox.UpdateFlowDocument();
			contentBox.InvalidateMeasure();
		}

		public Size ImageDimensions
		{
			get => (Size)GetValue(ImageDimensionsProperty);
			internal set => SetValue(ImageDimensionsProperty, value);
		}


		private void UpdateFlowDocument()
		{
			//string text = MainText;


			Document.Blocks.Clear();

			//Document.Style = new Style()
			//{
			//	TargetType = typeof(FlowDocument),
			//	BasedOn = Application.Current.Resources[]
			//};

			string text = RTFContent;

			if (!string.IsNullOrWhiteSpace(text))
			{
				TextRange textRange = new TextRange(Document.ContentStart, Document.ContentEnd);
				using (MemoryStream ms = new MemoryStream(Encoding.Default.GetBytes(text)))
				{
					textRange.Load(ms, DataFormats.Rtf); //Html is not supported (yet?) so i've done this workaround. 
				}
			}

			foreach (var block in Document.Blocks)
			{
				if (block is Paragraph paragraph && paragraph.Inlines.Count > 0)
				{
					foreach (var item in paragraph.Inlines)
					{
						if (item is Hyperlink link)
						{
							link.Click -= Link_Click;
							link.Click += Link_Click;
						}
					}
				}
			}

			if (BitmapSource != null)
			{
				Image image = new Image()
				{
					Source = BitmapSource,
					MaxHeight = ImageDimensions.Height,
					MaxWidth = ImageDimensions.Width,
					VerticalAlignment = VerticalAlignment.Center,
					HorizontalAlignment = HorizontalAlignment.Center,
					Stretch = Stretch.Uniform,
					StretchDirection = StretchDirection.Both,
				};

				Block block = Document.Blocks.FirstBlock;
				while (block != null && !(block is Paragraph))
				{
					block = block.NextBlock;
				}
				if (block is null)
				{
					if (Document.Blocks.FirstBlock == null)
					{
						Document.Blocks.Add(new BlockUIContainer(image));
					}
					else
					{
						Document.Blocks.InsertBefore(Document.Blocks.FirstBlock, new BlockUIContainer(image));
					}
				}
				else if (block is Paragraph paragraph)
				{
					Figure figure = new Figure(new BlockUIContainer(image))
					{
						HorizontalAnchor = FigureHorizontalAnchor.PageLeft,
						VerticalAnchor = FigureVerticalAnchor.ParagraphTop,
					};
					paragraph.Inlines.InsertBefore(paragraph.Inlines.FirstInline, figure);
				}
			}
		}

		public void Url_Click(object sender, RoutedEventArgs e)
		{
			if (sender is Hyperlink hyperlink)
			{
				UrlHelper.OpenURL(hyperlink.NavigateUri.ToString());
			}
		}
	}
}
