using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts.SpecialInteraction
{
	internal delegate GenderData GenderDataGetter();
	internal interface IGenderAware
	{
		void GetFemininityData(GenderDataGetter getter);
	}
}
