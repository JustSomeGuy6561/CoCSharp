using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CoCWinDesktop.CustomControls
{
	public class TextView : RichTextBox
	{

		public static DependencyProperty MainTextProperty = DependencyProperty.Register("MainText", typeof(string), typeof(TextView),
			new FrameworkPropertyMetadata("", OnCall));

		public static DependencyProperty ScrollbarWidthProperty = DependencyProperty.Register("ScrollbarWidth", typeof(double), typeof(TextView),
			new FrameworkPropertyMetadata(SystemParameters.VerticalScrollBarWidth));
		public double ScrollbarWidth { get; set; } = SystemParameters.VerticalScrollBarWidth;

		public static DependencyProperty ScrollbarHeightProperty = DependencyProperty.Register("ScrollbarHeight", typeof(double), typeof(TextView),
			new FrameworkPropertyMetadata(SystemParameters.HorizontalScrollBarHeight));
		public double ScrollbarHeight { get; set; } = SystemParameters.HorizontalScrollBarHeight;

		private static void OnCall(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextView st = d as TextView;
			st.MainText = (string)e.NewValue;

			st.InvalidateVisual();
			//st.Document.Blocks.Add(new Paragraph(new Run((string)e.NewValue)));
		}

		public string MainText { get; set; } = "";

		public static DependencyProperty MainImageProperty = DependencyProperty.Register("MainImage", typeof(BitmapImage), typeof(TextView),
			new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender, OnMainImageChanged));

		private static void OnMainImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextView document = d as TextView;
			document.InvalidateMeasure();
			document.InvalidateVisual();
			document.MainImage = (BitmapImage)e.NewValue;

		}
		public BitmapImage MainImage { get; set; } = null;

		public static DependencyProperty ExtraUIControlsProperty = DependencyProperty.Register("ExtraUIControls", typeof(ObservableCollection<Control>), typeof(TextView),
			new FrameworkPropertyMetadata(new ObservableCollection<Control>(), OnExtraContentChanged));

		private static void OnExtraContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextView document = d as TextView;

			document.ExtraUIControls = (ObservableCollection<Control>)e.NewValue;

			if (e.OldValue != null)
			{
				((INotifyCollectionChanged)e.OldValue).CollectionChanged -= document.OnExtraContentCollectionChanged;
			}
			if (e.NewValue != null)
			{
				((INotifyCollectionChanged)e.NewValue).CollectionChanged += document.OnExtraContentCollectionChanged;
			}

			document.InvalidateVisual();
		}

		private void OnExtraContentCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			InvalidateVisual();
		}

		public ObservableCollection<Control> ExtraUIControls { get; set; }

		public static DependencyProperty PostControlTextProperty = DependencyProperty.Register("PostControlText", typeof(string), typeof(TextView),
			new FrameworkPropertyMetadata("", OnPostControlTextChanged));

		private static void OnPostControlTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextView document = d as TextView;
			string data = e.NewValue as string;

			document.PostControlText = data;

			document.InvalidateVisual();
		}

		public string PostControlText { get; set; }


		private void UpdateFlowDocument()
		{
			if (Document is null)
			{
				Document = new FlowDocument();
			}

			Document.Blocks.Clear();
			Document.Style = (Style)Application.Current.FindResource("MainOutputDoc");
			Console.WriteLine(Document.Style);

			if (ExtraUIControls != null)
			{
				foreach (var obj in ExtraUIControls)
				{
					if (obj.Parent != null)
					{
						(obj.Parent as BlockUIContainer).Child = null;
					}
				}
			}

			if (!string.IsNullOrWhiteSpace(MainText))
			{
				TextRange textRange = new TextRange(Document.ContentStart, Document.ContentEnd);
				Console.WriteLine(MainText);
				using (MemoryStream ms = new MemoryStream(Encoding.Default.GetBytes(MainText)))
				{
					textRange.Load(ms, DataFormats.Rtf); //Html is not supported (yet?) so i've done this workaround. 
				}
			}

			if (MainImage != null)
			{
				//Blocks.InsertBefore(Blocks.FirstBlock, new Paragraph(new InlineUIContainer(new Image() { Source = BonusImage })));
				Block block = Document.Blocks.FirstBlock;
				while (block != null && !(block is Paragraph))
				{
					block = block.NextBlock;
				}
				if (block is null)
				{
					if (Document.Blocks.FirstBlock == null)
					{
						Document.Blocks.Add(new BlockUIContainer(new Image() { Source = MainImage }));
					}
					else
					{
						Document.Blocks.InsertBefore(Document.Blocks.FirstBlock, new BlockUIContainer(new Image() { Source = MainImage }));
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

			if (ExtraUIControls != null && ExtraUIControls.Count != 0)
			{
				foreach (var data in ExtraUIControls)
				{
					Document.Blocks.Add(new BlockUIContainer(data));
				}
			}

			if (!string.IsNullOrWhiteSpace(PostControlText))
			{
				FlowDocument appendDocument = new FlowDocument();
				TextRange textRange = new TextRange(appendDocument.ContentStart, appendDocument.ContentEnd);
				using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(PostControlText)))
				{
					textRange.Load(ms, DataFormats.Rtf); //Html is not supported (yet?) so i've done this workaround. 
				}
				Block block = appendDocument.Blocks.FirstBlock;
				while (block != null)
				{
					Document.Blocks.Add(block);
					block = block.NextBlock;
				}
			}
		}

		public TextView() : base()
		{
			DefaultStyleKey = typeof(TextView);
		}

		protected override void OnRender(DrawingContext drawingContext)
		{
			UpdateFlowDocument();
			base.OnRender(drawingContext);
		}
	}
}
