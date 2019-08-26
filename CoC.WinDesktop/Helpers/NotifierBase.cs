using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CoCWinDesktop.Helpers
{
	public abstract class NotifierBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected bool CheckPropertyChanged<T>(ref T source, T newValue, [CallerMemberName] string propertyName = "") where T:class
		{
			if (source != newValue)
			{
				source = newValue;
				RaisePropertyChanged(propertyName);
				return true;
			}
			return false;
		}

		protected bool CheckPrimitivePropertyChanged<T>(ref T source, T newValue, [CallerMemberName] string propertyName = "") where T : struct, IEquatable<T>
		{
			if (!source.Equals(newValue))
			{
				source = newValue;
				RaisePropertyChanged(propertyName);
				return true;
			}
			return false;
		}
		protected bool CheckPrimitivePropertyChanged<T>(ref T? source, T? newValue, [CallerMemberName] string propertyName = "") where T : struct, IEquatable<T>
		{
			if (source is null != newValue is null || (!(source is null) && !source.Equals(newValue)))
			{
				source = newValue;
				RaisePropertyChanged(propertyName);
				return true;
			}
			return false;
		}

		protected bool CheckEnumPropertyChanged<T>(ref T source, T newValue, [CallerMemberName] string propertyName = "") where T : Enum
		{
			if (!source.Equals(newValue))
			{
				source = newValue;
				RaisePropertyChanged(propertyName);
				return true;
			}
			return false;
		}

		protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
