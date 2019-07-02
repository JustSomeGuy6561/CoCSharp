//PiercingData.cs
//Description:
//Author: JustSomeGuy
//6/18/2019, 11:55 PM
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Items.Wearables.Piercings
{
	public sealed class PiercingData<T> : Dictionary<T, PiercingJewelry> where T: Enum {}
}
