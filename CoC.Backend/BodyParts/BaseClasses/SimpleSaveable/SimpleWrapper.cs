using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts
{
	/// <summary>
	/// Encapsulates the Source Class in a read-only fashion. The data stored within the source class is stored by reference, not by value, so any changes to the underlying type
	/// are kept here. Exposes any members or functions of the underlying class that are also read-only. This needs to be done manually, everywhere that uses this.
	/// </summary>
	/// <typeparam name="ThisClass">This class. Required parameter because the source class needs it. </typeparam>
	/// <typeparam name="SourceClass">The source class to encapsulate. </typeparam>
	public abstract class SimpleWrapper<ThisClass, SourceClass> where SourceClass: SimpleSaveablePart<SourceClass, ThisClass> where ThisClass : SimpleWrapper<ThisClass, SourceClass>
	{
		protected readonly SourceClass sourceData;
		protected readonly Guid creatureID;

		//protected SimpleData(Guid creatureID)
		//{
		//	this.creatureID = creatureID;
		//	sourceData = null;
		//}

		protected SimpleWrapper(SourceClass source)
		{
			sourceData = source;
		}
	}
}
