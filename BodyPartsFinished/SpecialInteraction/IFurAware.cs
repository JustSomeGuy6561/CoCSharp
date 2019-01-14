//IFurAware.cs
//Description:
//Author: JustSomeGuy
//1/3/2019, 7:31 PM
using CoC.EpidermalColors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.BodyParts.SpecialInteraction
{
	public interface IFurAware
	{
		//note: this will be called if the body's fur or underfur is removed. take this into account when copying the data
		//you may want to ignore changes that remove fur from body if the part keeps the fur. you also can change the part
		//to something without fur if you so wish. need some way of dealing with messages to user, NYI.
		void reactToChangeInFurColor(FurColor primaryFur, FurColor secondaryFur);
	}
}
