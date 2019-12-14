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
	public abstract class BehavioralSaveablePartData<DataClass, SourceClass, BehaviorClass> : BehavioralPartDataBase<BehaviorClass>
		where DataClass : BehavioralSaveablePartData<DataClass, SourceClass, BehaviorClass>
		where SourceClass : BehavioralSaveablePart<SourceClass, BehaviorClass, DataClass>
		where BehaviorClass : SaveableBehavior<BehaviorClass, SourceClass, DataClass>

	{
		private protected BehavioralSaveablePartData(Guid creatureID, BehaviorClass currentBehavior) : base(creatureID, currentBehavior)
		{
		}

		//99% of the time, this will just say "return this;" but in the event of legacy savers, this will not always be the case.
		public abstract DataClass AsCurrentData();

		//private protected abstract SourceClass Deserialize(StreamingInfo info, DataContext dataa);

		public string LongDescriptionPrimary() => type.LongDescriptionPrimary(AsCurrentData());

		public string LongDescriptionAlternate() => type.LongDescriptionAlternate(AsCurrentData());

		public string LongDescription(bool alternateForm) => type.LongDescription(AsCurrentData(), alternateForm);


		protected static Guid GetID(SourceClass source)
		{
			if (source is null) throw new ArgumentNullException(nameof(source));
			return source.creatureID;
		}

		protected static BehaviorClass GetBehavior(SourceClass source)
		{
			if (source is null) throw new ArgumentNullException(nameof(source));
			return source.type;
		}
	}
}
