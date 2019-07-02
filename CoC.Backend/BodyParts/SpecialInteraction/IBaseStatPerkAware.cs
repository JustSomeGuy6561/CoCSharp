using CoC.Backend.Perks;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts.SpecialInteraction
{
	internal delegate PassiveStatModifiers BasePerkDataGetter();
	internal interface IBaseStatPerkAware
	{
		void GetBasePerkStats(BasePerkDataGetter getter);
	}
}
