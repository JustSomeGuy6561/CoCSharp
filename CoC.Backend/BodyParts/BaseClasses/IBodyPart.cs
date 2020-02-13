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

	public interface IBodyPart<U> : IBodyPart where U: class
	{
		U AsReadOnlyData();

		bool IsIdenticalTo(U data, bool ignoreSexualMetaData);
		bool IsIdenticalTo(U data);
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

	public static class BodyPartHelpers
	{
		public static bool AreIdentical<U>(IBodyPart<U> source, U originalData) where U:class
		{
			if (source is null)
			{
				return originalData is null;
			}
			else
			{
				return source.IsIdenticalTo(originalData);
			}
		}
		public static bool AreIdentical<U>(IBodyPart<U> source, U originalData, bool ignoreSexualMetaData) where U:class
		{
			if (source is null)
			{
				return originalData is null;
			}
			else
			{
				return source.IsIdenticalTo(originalData, ignoreSexualMetaData);
			}
		}
	}
}
