//ICanAttackWith.cs
//Description:
//Author: JustSomeGuy
//1/7/2019, 2:58 AM
using CoC.Engine.Combat.Attacks;
using CoC.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.BodyParts.SpecialInteraction
{
	public interface ICanAttackWith
	{
		AttackBase attack { get; }
		bool canAttackWith();
	}
}
