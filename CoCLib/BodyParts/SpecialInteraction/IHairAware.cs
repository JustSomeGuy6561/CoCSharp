using CoC.BodyParts;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.BodyParts.SpecialInteraction
{
	public interface IHairAware
	{
		void reactToChangeInHairColor(object sender, HairColorEventArg e);
	}
}
