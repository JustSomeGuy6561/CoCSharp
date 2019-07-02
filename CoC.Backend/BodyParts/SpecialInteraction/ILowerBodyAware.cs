//ILowerBodyAware.cs
//Description:
//Author: JustSomeGuy
//6/25/2019, 1:03 AM

namespace CoC.Backend.BodyParts.SpecialInteraction
{
	internal delegate LowerBodyData LowerBodyDataGetter();

	//Provides a way to get the body data ON-DEMAND. this is known as Lazy programming. call the getter passed in and viola.

	internal interface ILowerBodyAware
	{
		void GetLowerBodyData(LowerBodyDataGetter getter);
	}
}
