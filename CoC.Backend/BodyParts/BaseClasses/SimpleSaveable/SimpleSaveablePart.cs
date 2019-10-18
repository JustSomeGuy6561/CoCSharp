﻿//SimpleSaveablePart.cs
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

	public abstract class SimpleSaveablePart<ThisClass, DataClass> : IBodyPart where ThisClass : SimpleSaveablePart<ThisClass, DataClass> where DataClass : SimpleData
	{
		internal abstract bool Validate(bool correctInvalidData);

		public readonly Guid creatureID;

		private protected SimpleSaveablePart(Guid parentGuid)
		{
			creatureID = parentGuid;
		}

		public abstract DataClass AsReadOnlyData();

		private protected readonly WeakEventSource<SimpleDataChangeEvent<ThisClass, DataClass>> dataChangeSource =
			new WeakEventSource<SimpleDataChangeEvent<ThisClass, DataClass>>();
		public event EventHandler<SimpleDataChangeEvent<ThisClass, DataClass>> dataChange
		{
			add => dataChangeSource.Subscribe(value);
			remove => dataChangeSource.Unsubscribe(value);
		}

		private protected void NotifyDataChanged(DataClass oldData)
		{
			dataChangeSource.Raise(this, new SimpleDataChangeEvent<ThisClass, DataClass>(oldData, AsReadOnlyData()));
		}

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
			return typeof(DataClass);
		}
	}
}
