using CoCWinDesktop.Helpers;
using CoCWinDesktop.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCWinDesktop.CustomControls
{
	/// <summary>
	/// Provides a base class for the model view desribing the content section of the options page. 
	/// </summary>
	public abstract class OptionModelViewDataBase : NotifierBase
	{
		protected readonly ModelViewRunner runner;
		protected readonly OptionsModelView parent;

		protected OptionModelViewDataBase(ModelViewRunner modelViewRunner, OptionsModelView optionsModelView)
		{
			runner = modelViewRunner ?? throw new ArgumentNullException(nameof(modelViewRunner));
			parent = optionsModelView ?? throw new ArgumentNullException(nameof(optionsModelView));
		}

		public abstract void ParseDataForDisplay();

		public virtual void OnSwitchTo() { }

		public bool requiresConfirmation => OnConfirmation != null;

		public virtual Action OnConfirmation { get; } = null;
	}
}
