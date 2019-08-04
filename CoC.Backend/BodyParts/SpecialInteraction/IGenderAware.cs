using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts.SpecialInteraction
{
	public delegate Gender GenderGetter();

	public interface IGenderAware
	{
		void GetGenderData(GenderGetter getter);
	}
}
