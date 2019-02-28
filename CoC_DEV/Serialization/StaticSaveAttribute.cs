//StaticSaveAttribute.cs
//Description:
//Author: JustSomeGuy
//12/30/2018, 10:36 PM
using System;

namespace CoC.Serialization
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class StaticSaveAttribute : Attribute
	{
	}
}