using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Items
{
	public abstract class KeyItem
	{


		//listen, i have no idea where this would display because the key items are just listed on a menu. but i guess you can have some credit?
		public virtual string Author() => string.Empty;
	}
}
