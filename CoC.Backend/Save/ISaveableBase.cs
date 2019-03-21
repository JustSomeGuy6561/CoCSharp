using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Save
{
	public interface ISaveableBase
	{
		Type currentSaveType { get; }
		Type[] saveVersionTypes { get; }

		object ToCurrentSaveVersion();
	}
}
