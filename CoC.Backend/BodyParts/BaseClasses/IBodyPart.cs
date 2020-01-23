using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts
{
	public interface IBodyPart
	{
		string BodyPartName();

		Type BaseType();

		Type DataType();

		Guid creatureID { get; }
	}

	public interface IBehavioralBodyPart : IBodyPart
	{

		Type BehaviorType();

	}

	public interface IBodyPartData
	{
		string BodyPartName();

		Type BaseType();

		Type DataType();
	}


}
