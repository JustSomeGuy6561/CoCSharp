using CoC.Backend;
using CoC.Backend.Creatures;
using CoC.Backend.Perks;
using CoC.Frontend.Perks;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Creatures.NPCs
{

	public class DynamicNPC : DynamicNPCBase, IExtendedCreature
	{
		public DynamicNPC(DynamicNPC_Creator creator) : base(creator)
		{
			extendedPerkModifiers = new ExtendedPerkModifiers(this);
			extendedData = new ExtendedCreatureData(this, extendedPerkModifiers);

			indefinite = creator.indefiniteArticle;
			definite = creator.definiteArticle;
		}

		protected readonly SimpleDescriptor indefinite, definite;

		public override string Article(bool definitiveArticle)
		{
			if (definitiveArticle) return definite?.Invoke() ?? "";
			else return indefinite?.Invoke() ?? "";
		}

		public ExtendedCreatureData extendedData { get; }

		public ExtendedPerkModifiers extendedPerkModifiers { get; }
	}
}
