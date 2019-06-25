﻿

namespace CoC.Backend.BodyParts
{

	public abstract class SimpleSaveablePart<ThisClass> /*: SaveClassBase*/
		where ThisClass : SimpleSaveablePart<ThisClass>
	{
		internal abstract bool Validate(bool correctInvalidData);
	}
}
