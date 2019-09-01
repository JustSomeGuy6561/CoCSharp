using CoCWinDesktop.Helpers;
using CoCWinDesktop.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCWinDesktop.CustomControls
{
    public abstract class ExtraItemModelViewBase : NotifierBase
    {
		protected readonly ModelViewRunner runner;
		protected readonly ExtraMenuItemsModelView parent;

		public string ContentTitle
		{
			get => _contentTitle;
			private set => CheckPropertyChanged(ref _contentTitle, value);
		}
		private string _contentTitle = "";
		public string ContentHelper
		{
			get => _contentHelper;
			private set => CheckPropertyChanged(ref _contentHelper, value);
		}
		private string _contentHelper = "";

		public string Content
		{
			get => _content;
			private set => CheckPropertyChanged(ref _content, value);
		}
		private string _content = "";

		public string PostContent
		{
			get => postContent;
			private set => CheckPropertyChanged(ref postContent, value);
		}
		private string postContent = "";

		protected ExtraItemModelViewBase(ModelViewRunner modelViewRunner, ExtraMenuItemsModelView parentModelView)
		{
			runner = modelViewRunner ?? throw new ArgumentNullException(nameof(modelViewRunner));
			parent = parentModelView ?? throw new ArgumentNullException(nameof(parentModelView));
		}

		internal abstract void ParseDataForDisplay();
	}
}
