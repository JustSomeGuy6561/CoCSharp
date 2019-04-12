using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CoC.Backend.Items.Wearables.Piercings
{
#if DEBUG
	
	public sealed class JewelryDummyForDebugging : PiercingJewelry
	{
		public JewelryDummyForDebugging() : base(JewelryType.STUD, null, false) {}
	}
#endif
}
