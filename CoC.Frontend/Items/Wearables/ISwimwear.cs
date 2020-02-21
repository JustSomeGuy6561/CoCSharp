using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Items.Wearables
{
	//allows you to extend an armor to be treated as swimwear for various scenes. as of current implementation, only slutty swimwear does so.
	interface ISwimwear
	{
		bool isSwimwear { get; }

	}
}
