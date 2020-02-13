//PlayerCreator.cs
//Description:
//Author: JustSomeGuy
//3/22/2019, 6:13 PM

using System;
using CoC.Backend.BodyParts;

namespace CoC.Backend.Creatures
{
	public class DynamicNPC_Creator : CreatureCreator
	{
		public SimpleDescriptor indefiniteArticle;
		public SimpleDescriptor definiteArticle;

		public DynamicNPC_Creator(string name) : base(name) { }
	}
}
