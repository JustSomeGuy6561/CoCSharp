//IBodyAware.cs
//Description:
//Author: JustSomeGuy
//3/26/2019, 2:04 AM
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts.SpecialInteraction
{
	internal delegate BodyData BodyDataGetter();

	//Provides a way to get the body data ON-DEMAND. this is known as Lazy programming. call the getter passed in and viola.

	internal interface IBodyAware
	{
		void GetBodyData(BodyDataGetter getter);
	}
}
