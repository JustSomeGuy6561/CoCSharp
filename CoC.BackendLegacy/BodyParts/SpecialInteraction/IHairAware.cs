using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts.SpecialInteraction
{
	internal delegate HairData HairDataGetter();
	internal interface IHairAware
	{
		void GetHairData(HairDataGetter hairData);
	}
}
