using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts.SpecialInteraction
{
	internal delegate BuildData BuildDataGetter();

	//Provides a way to get the body data ON-DEMAND. this is known as Lazy programming. call the getter passed in and viola.

	internal interface IBuildAware
	{
		void GetBuildData(BuildDataGetter getter);
	}
}
