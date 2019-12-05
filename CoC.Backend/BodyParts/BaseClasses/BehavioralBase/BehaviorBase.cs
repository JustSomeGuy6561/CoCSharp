//BehaviorBase.cs
//Description:
//Author: JustSomeGuy
//3/26/2019, 8:35 PM
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

		private readonly SimpleDescriptor shortDesc;

		public string ShortDescription()
		{
			return shortDesc();
		}

		private protected BehaviorBase(SimpleDescriptor shortDescFn)
		{
			shortDesc = shortDescFn ?? throw new ArgumentNullException(nameof(shortDescFn));
		}
	}
}
