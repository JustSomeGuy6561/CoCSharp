//SimpleSaveablePart.cs
//Description:
//Author: JustSomeGuy
//3/26/2019, 8:40 PM


using CoC.Backend.Creatures;
using System;

namespace CoC.Backend.BodyParts
{

	public abstract class SimpleSaveablePart<ThisClass, DataClass> where ThisClass : SimpleSaveablePart<ThisClass, DataClass> where DataClass : class
	{
		internal abstract bool Validate(bool correctInvalidData);

		private protected readonly Creature source;

		private protected SimpleSaveablePart(Creature parent)
		{
			source = parent ?? throw new ArgumentNullException(nameof(parent));
		}

		public abstract DataClass AsReadOnlyData();

		protected internal virtual void PostPerkInit()
		{ }

		protected internal virtual void LateInit()
		{ }
	}
}
