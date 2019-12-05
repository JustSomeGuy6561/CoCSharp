using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoC.Backend;

namespace CoC.WinDesktop.ContentWrappers.ButtonWrappers
{
	public sealed class LanguageAwareButtonWrapper : AutomaticButtonWrapper
	{
		public LanguageAwareButtonWrapper(SimpleDescriptor TitleStrCallback, Action onClick, DescriptorWithArg<bool> unlockedLockedTipCallback,
			SimpleDescriptor tipTitleCallback, bool enabled = true, bool defaultButton = false) : base(TitleStrCallback, onClick,
			unlockedLockedTipCallback, tipTitleCallback, enabled, defaultButton)
		{
		}

		private ToolTipWrapper toolTipSetter
		{
			set => CheckPropertyChanged(ref _ToolTip, value, nameof(ToolTip));
		}

		public void OnLanguageChanged()
		{
			toolTipSetter = GenerateToolTipWrapper();
		}
	}
}
