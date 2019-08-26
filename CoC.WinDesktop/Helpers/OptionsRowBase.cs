using CoC.Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCWinDesktop.Helpers
{
	//public enum OptionRowDisplayMode { GAME, SESSION, GLOBAL }

	public abstract class OptionsRowBase : NotifierBase
	{
		//public string activeItemDescription
		//{
		//	get => _selectedItemDescription;
		//	protected set => CheckPropertyChanged(ref _selectedItemDescription, value);
		//}
		//private string _selectedItemDescription;

		public string OptionName => nameDesc();
		private readonly SimpleDescriptor nameDesc;


		protected OptionsRowBase(SimpleDescriptor optionName)//OptionRowDisplayMode defaultMode = OptionRowDisplayMode.GAME)
		{
			nameDesc = optionName ?? throw new ArgumentNullException(nameof(optionName));
			_isGlobal = false;
			//_displayMode = defaultMode;
		}

		public bool isGlobal
		{
			get => _isGlobal;
			private set
			{
				if (CheckPrimitivePropertyChanged(ref _isGlobal, value))
				{
					OnStatusChanged();
				}
			}
		}
		private bool _isGlobal;

		//public OptionRowDisplayMode displayMode
		//{
		//	get => _displayMode;
		//	private set
		//	{
		//		OptionRowDisplayMode oldMode = _displayMode;
		//		if (CheckEnumPropertyChanged(ref _displayMode, value))
		//		{
		//			OnOptionStatusChanged(oldMode);
		//		}
		//	}
		//}
		//private OptionRowDisplayMode _displayMode;

		//public void ChangeOptionStatus(OptionRowDisplayMode newMode)
		//{
		//	displayMode = newMode;
		//}

		public void ChangeState(bool nowGlobal)
		{
			isGlobal = nowGlobal;
		}

		//protected abstract void OnOptionStatusChanged(OptionRowDisplayMode oldMode);
		protected abstract void OnStatusChanged();
	}
}
