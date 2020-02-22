using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Items
{
	//common between items and transformations. This lets us use an enum (which is essentially an int, but cleaner) for items/tfs that support purified/ehanced at once.
	public enum TransformationType { STANDARD, PURIFIED, ENHANCED }
}
