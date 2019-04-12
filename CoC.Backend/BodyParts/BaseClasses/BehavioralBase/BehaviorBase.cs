using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CoC.Backend.BodyParts
{
	//Stores a simple body part. if any rules need to apply, add the logic here. 
	public abstract class BehaviorBase
	{
		public abstract int index { get; }

		public readonly SimpleDescriptor shortDescription;
		private protected BehaviorBase(SimpleDescriptor shortDesc)
		{
			shortDescription = shortDesc;
		}
	}
}
