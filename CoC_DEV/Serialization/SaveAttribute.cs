//SaveAttribute.cs
//Description:
//Author: JustSomeGuy
//1/31/2019, 12:08 AM
using System;

namespace CoC.Serialization
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	class SaveAttribute : Attribute
	{}
}
