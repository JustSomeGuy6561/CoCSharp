using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CoC.Backend.Wearables
{
#if DEBUG
	[DataContract]
	public sealed class JewelryDummyForDebugging : PiercingJewelry
	{
		public JewelryDummyForDebugging() : base(JewelryType.STUD, null, false) {}
	}
#endif
}
