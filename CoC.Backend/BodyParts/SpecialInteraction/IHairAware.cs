//IHairAware.cs
//Description:
//Author: JustSomeGuy
//3/26/2019, 2:04 AM
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
