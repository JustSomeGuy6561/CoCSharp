using System;
using System.Collections.Generic;
using System.Text;

namespace TestingClassLibrary.SaveTest
{
	public interface ISaveableBase
	{
		Type currentSaveType { get; }
		Type[] saveVersionTypes { get; }

		object ToCurrentSaveVersion();
	}
}
