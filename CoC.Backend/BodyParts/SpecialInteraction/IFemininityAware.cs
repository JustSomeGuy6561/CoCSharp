using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts.SpecialInteraction
{
	internal delegate FemininityData FemininityDataGetter();
	internal interface IFemininityAware
	{
		void GetFemininityData(FemininityDataGetter getter);
	}
}
