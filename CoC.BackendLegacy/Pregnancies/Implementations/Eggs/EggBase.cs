using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CoC.Backend.Pregnancies.Implementations
{
	[DataContract]
	public abstract class EggBase : SpawnType
	{
		public bool eggColorKnown { get; protected set; }
		public EggBase(SimpleDescriptor nameOfFather, int birthTime, SimpleDescriptor birthText) : base(nameOfFather, birthTime, birthText)
		{

		}
	}
}
