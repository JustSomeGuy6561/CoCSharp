//SimpleSaveablePart.cs
//Description:
//Author: JustSomeGuy
//3/26/2019, 8:40 PM


using CoC.Backend.BodyParts.BaseClasses;
using CoC.Backend.BodyParts.EventHelpers;
using CoC.Backend.Creatures;
using System;
using WeakEvent;

namespace CoC.Backend.BodyParts
{

	public abstract class SimpleSaveablePart<ThisClass, WrapperClass> : IBodyPart 
		where ThisClass : SimpleSaveablePart<ThisClass, WrapperClass> 
		where WrapperClass : SimpleWrapper<WrapperClass, ThisClass>
	{
		internal abstract bool Validate(bool correctInvalidData);

		public readonly Guid creatureID;

		private protected SimpleSaveablePart(Guid parentGuid)
		{
			creatureID = parentGuid;
		}

		public abstract WrapperClass AsReadOnlyReference();

		protected internal virtual void PostPerkInit()
		{ }

		protected internal virtual void LateInit()
		{ }

		public abstract string BodyPartName();


		public Type BaseType()
		{
			return typeof(ThisClass);
		}

		public Type DataType()
		{
			return typeof(WrapperClass);
		}
	}
}
