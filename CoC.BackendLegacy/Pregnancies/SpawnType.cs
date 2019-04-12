using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CoC.Backend.Pregnancies
{
	[DataContract]
	public abstract class SpawnType
	{
		public readonly SimpleDescriptor father;
		public readonly int hoursToBirth;
		public readonly SimpleDescriptor birthFlavorText;

		public SpawnType(SimpleDescriptor nameOfFather, int birthTime, SimpleDescriptor birthText)
		{
			father = nameOfFather;
			hoursToBirth = birthTime;
			birthFlavorText = birthText;
		}

		public abstract void HandleBirth();
	}
}
