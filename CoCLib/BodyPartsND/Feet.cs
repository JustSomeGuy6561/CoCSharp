using System;
using System.Collections.Generic;
using System.Text;
using CoC.Tools;

namespace CoC.BodyParts
{
	internal class Feet : SimpleBodyPart<FootType>
	{
		public Feet(FootType value) : base(value)
		{
		}

		public override FootType type { get; protected set; }
	}

	internal class FootType : SimpleBodyPartType
	{
		private static int indexMaker = 0;

		protected enum FootStyle { FEET, PAWS, CLAWS, OTHER}

		protected readonly FootStyle footStyle;

		protected FootType(FootStyle style, SimpleDescriptor shortDesc) : base(shortDesc)
		{
			_index = indexMaker++;
			footStyle = style;
		}

		public override int index => _index;
		private readonly int _index;
	}
}
