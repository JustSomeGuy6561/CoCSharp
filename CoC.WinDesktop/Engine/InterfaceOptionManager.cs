using CoC.WinDesktop.InterfaceSettings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoC.WinDesktop.Engine
{
	public static class InterfaceOptionManager
	{
		private static readonly List<InterfaceOptions> _interfaces;
		private static readonly HashSet<InterfaceOptions> _interfaceLookup;
		public static readonly ReadOnlyCollection<InterfaceOptions> interfaceOptions;
		static InterfaceOptionManager()
		{
			_interfaces = new List<InterfaceOptions>();
			_interfaceLookup = new HashSet<InterfaceOptions>();
			interfaceOptions = new ReadOnlyCollection<InterfaceOptions>(_interfaces);
		}

		public static void IncludeOption(InterfaceOptions option)
		{
			if (!_interfaceLookup.Contains(option))
			{
				_interfaceLookup.Add(option);
				_interfaces.Add(option);
			}
		}

		public static T GetOptionOfType<T>() where T : InterfaceOptions
		{
			return _interfaces.Find(x => x.GetType() == typeof(T)) as T;
		}
	}
}
