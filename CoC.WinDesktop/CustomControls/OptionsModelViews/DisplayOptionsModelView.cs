using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoCWinDesktop.ModelView;

namespace CoCWinDesktop.CustomControls.OptionsModelViews
{
	public sealed class DisplayOptionsModelView : OptionModelViewDataBase
	{
		public string DisplayOptionsText
		{
			get => _DisplayOptionsText;
			private set => CheckPropertyChanged(ref _DisplayOptionsText, value);
		}
		private string _DisplayOptionsText;

		public string DisplayOptionsHelper
		{
			get => _DisplayOptionsHelper;
			private set => CheckPropertyChanged(ref _DisplayOptionsHelper, value);
		}
		private string _DisplayOptionsHelper;

		public int[] availableFontSizes { get; }

		public int selectedFontIndex
		{
			get => _selectedFontIndex;
			set
			{
				if (CheckPrimitivePropertyChanged(ref _selectedFontIndex, value))
				{
					runner.FontSize = availableFontSizes[value];
				}
			}
		}
		private int _selectedFontIndex;
		private readonly Dictionary<string, int> bgIndexMapper = new Dictionary<string, int>();
		public string[] availableBackgrounds { get; }

		public int backgroundsMaxValue => availableBackgrounds.Length - 1;

		public int selectedBackgroundIndex
		{
			get => _selectedBackgroundIndex;
			set
			{
				if (CheckPrimitivePropertyChanged(ref _selectedBackgroundIndex, value))
				{
					//get the string associated with this index. then get the index that the runner uses with this string and set it accordingly.
					runner.BackgroundIndex = bgIndexMapper[availableBackgrounds[_selectedBackgroundIndex]];
				}
			}
		}
		private int _selectedBackgroundIndex;



		private readonly Dictionary<string, int> textIndexMapper = new Dictionary<string, int>();
		public string[] availableTextBackgrounds { get; }

		public int textBackgroundsMaxValue => availableTextBackgrounds.Length - 1;

		public int selectedTextBackground
		{
			get => _selectedTextBackground;
			set
			{
				if (CheckPrimitivePropertyChanged(ref _selectedTextBackground, value))
				{
					//get the string associated with this index. then get the index that the runner uses with this string and set it accordingly.
					runner.TextBackgroundIndex = textIndexMapper[availableTextBackgrounds[_selectedTextBackground]];
				}
			}
		}
		private int _selectedTextBackground;


		public DisplayOptionsModelView(ModelViewRunner modelViewRunner, OptionsModelView optionsModelView) : base(modelViewRunner, optionsModelView)
		{
			availableFontSizes = Enumerable.Range(ModelViewRunner.MinFontSize, ModelViewRunner.MaxFontSize).ToArray();
			bgIndexMapper = ModelViewRunner.backgroundDescriptors.Select((x, y) => new KeyValuePair<string, int>(x?.Invoke(), y)).Where(x => x.Key != null).ToDictionary(x => x.Key, y => y.Value);

			availableBackgrounds = bgIndexMapper.Keys.ToArray();

			textIndexMapper = ModelViewRunner.textBackgroundDescriptors.Select((x, y) => new KeyValuePair<string, int>(x?.Invoke(), y)).Where(x => x.Key != null).ToDictionary(x => x.Key, y => y.Value);

			availableTextBackgrounds = textIndexMapper.Keys.ToArray();
		}

		//Top: header, helper
		//ContentRTB in the middle. Full of lorem ipsum or language check text. 

		//bottom: 3 sliders - for font size, text bg color and bg image. 

		//confirm on bottom icons. 
		public override void ParseDataForDisplay()
		{

		}

		public override Action OnConfirmation => ConfirmControls;


		private void ConfirmControls()
		{
			Console.WriteLine("Controls written? NYI, but code works!");
		}
	}
}
