//IBaseStatPerkAware.cs
//Description:
//Author: JustSomeGuy
//6/30/2019, 8:07 PM
using CoC.Backend.Perks;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts.SpecialInteraction
{
	public delegate BasePerkModifiers PerkStatBonusGetter();
	internal interface IBaseStatPerkAware
	{
		void GetBasePerkStats(PerkStatBonusGetter getter);
	}
}
