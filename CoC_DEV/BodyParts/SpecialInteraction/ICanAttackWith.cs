﻿//ICanAttackWith.cs
//Description:
//Author: JustSomeGuy
//1/7/2019, 2:58 AM
using CoC.Engine.Combat.Attacks;

namespace  CoC.BodyParts.SpecialInteraction
{
	internal interface ICanAttackWith
	{
		AttackBase attack { get; }
		bool canAttackWith();
	}
}