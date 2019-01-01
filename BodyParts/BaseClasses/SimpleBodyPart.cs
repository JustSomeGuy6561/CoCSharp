using CoC.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.BodyParts
{
	public abstract class SimpleBodyPart
	{
		public abstract int index { get; }
		public abstract GenericDescription shortDescription { get; protected set; }
	}
}
