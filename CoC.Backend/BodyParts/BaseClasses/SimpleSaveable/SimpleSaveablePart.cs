//SimpleSaveablePart.cs
//Description:
//Author: JustSomeGuy
//3/26/2019, 8:40 PM


namespace CoC.Backend.BodyParts
{

	public abstract class SimpleSaveablePart<ThisClass> /*: SaveClassBase*/
		where ThisClass : SimpleSaveablePart<ThisClass>
	{
		internal abstract bool Validate(bool correctInvalidData);
	}
}
