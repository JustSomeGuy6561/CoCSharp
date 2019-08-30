using CoC.Backend;
using CoCWinDesktop.Helpers;
using CoCWinDesktop.ModelView;
using System;

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

		protected abstract SimpleDescriptor TitleText { get; }
		protected abstract SimpleDescriptor TitleHelperText { get; }

		public abstract SimpleDescriptor ButtonText { get; }

		//public abstract string HeaderText { get; protected set; }
		//public abstract string HeaderDescription { get; protected set; }
	}
}
