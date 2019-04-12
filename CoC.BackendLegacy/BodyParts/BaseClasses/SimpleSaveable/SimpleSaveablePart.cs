using CoC.Backend.Save;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CoC.Backend.BodyParts
{
	[DataContract]
	public abstract class SimpleSaveablePart<ThisClass> /*: SaveClassBase*/
		where ThisClass : SimpleSaveablePart<ThisClass> 
	{
		internal abstract bool Validate(bool correctDataIfInvalid = false);
	}
}
