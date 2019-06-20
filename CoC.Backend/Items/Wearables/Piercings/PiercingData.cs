using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Items.Wearables.Piercings
{
	public sealed class PiercingData<T> : Dictionary<T, PiercingJewelry> where T: Enum {}
}
