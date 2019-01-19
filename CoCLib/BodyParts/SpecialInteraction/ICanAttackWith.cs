//ICanAttackWith.cs
//Description:
//Author: JustSomeGuy
//1/7/2019, 2:58 AM
using CoC.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.BodyParts.SpecialInteraction
{
	interface ICanAttackWith<T>
	{
#warning TODO: Change this to use a CombatAttack class. just assign it to the body part, and then use it.
		//Standard attack for this type - be it kick/punch/whatever
		bool canAttackWith();
		bool hasStandardAttackForThisPart();
		GenericDescription attackName { get; }
		TypeAndPlayerDelegate<T> attackHint { get; }

		//additional special attack this type also has.
		//could make it a list if we want multiple specials per type, but that seems op
		bool hasSpecialAttack();
		GenericDescription specialAttackName { get; }
		TypeAndPlayerDelegate<T> specialAttackHint { get; }
	}
}
