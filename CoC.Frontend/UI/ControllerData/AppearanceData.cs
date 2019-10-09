using CoC.Backend;
using CoC.Backend.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.UI.ControllerData
{
	/// <summary>
	/// Provides a read-only version of all appearance related data that can safely be passed to the UI level.
	/// </summary>
	public sealed class AppearanceData
	{
		public SimpleDescriptor appearanceString;

		internal AppearanceData()
		{
			appearanceString = GameEngine.currentlyControlledCharacter.Appearance;
		}
	}
}
