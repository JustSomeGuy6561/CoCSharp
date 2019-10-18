using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts.BaseClasses
{
	public interface IBodyPart
	{
		string BodyPartName();

		Type BaseType();

		Type DataType();
	}

	public interface IBehavioralBodyPart : IBodyPart
	{

		Type BehaviorType();

	}
}
