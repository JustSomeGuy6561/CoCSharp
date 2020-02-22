using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts
{
	/// <summary>
	/// Base class for any data versions of the given body part.
	/// </summary>
	/// <typeparam name="DataClass">The current data storage object type for this body part.</typeparam>
	/// <typeparam name="SourceClass">The class representing this body part.</typeparam>
	/// <typeparam name="BehaviorClass">The class storing the different types of this body part and the common behaviors therein.</typeparam>
	///
	/// <remarks>Data classes provide a read-only snapshot of the data stored in the current body part, which allows them to
	/// be used for serialization and for capturing the status of the body part before and/or after changing data in it. This allows for greater flexibility
	/// when changing data; you can just return the old data instead of having to worry about each value manually.
	///
	/// Data classes also provide a means of legacy serialization (saving): When a body part's structure or members change, it'd normally break serialization. The data class
	/// is serialized instead, and converted to the source class on deserialization (loading). It's possible to have multiple data classes, though only one is the "current"
	/// data class. We deserialize to whatever version was originally saved, then tell it to convert to the source class. It's possible to be lazy and simply convert to the current
	/// data class and then have that convert to the source class, or optimize it and create a direct conversion for each legacy save - the choice is yours.
	/// </remarks>
	public abstract class FullBehavioralData<DataClass, SourceClass, BehaviorClass> : BehavioralSaveableData<DataClass, SourceClass, BehaviorClass>
		where DataClass : FullBehavioralData<DataClass, SourceClass, BehaviorClass>
		where SourceClass : FullBehavioralPart<SourceClass, BehaviorClass, DataClass>
		where BehaviorClass : FullBehavior<BehaviorClass, SourceClass, DataClass>

	{
		private protected FullBehavioralData(Guid creatureID, BehaviorClass currentBehavior) : base(creatureID, currentBehavior)
		{
		}

		public bool isDefault => type == defaultType;
		public abstract BehaviorClass defaultType { get; }

		public string LongDescriptionPrimary() => type.LongDescriptionPrimary(AsCurrentData());

		public string LongDescriptionAlternate() => type.LongDescriptionAlternate(AsCurrentData());

		public string LongDescription(bool alternateFormat = false) => type.LongDescription(AsCurrentData(), alternateFormat);
	}
}
