//Antennae.cs
//Description: Contains the Antennae and AntennaeType classes, a body part that helps make up creature.
//Author: JustSomeGuy
//12/30/2018, 10:08 PM

using CoC.Backend.Creatures;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CoC.Backend.BodyParts
{
	//Note: Never fires a data change event, as it has no data that can be changed.

	public sealed class Antennae : BehavioralSaveablePart<Antennae, AntennaeType, AntennaeData>
	{

		public override AntennaeType type { get; protected set; }

		public override AntennaeType defaultType => AntennaeType.defaultValue;

		public bool hasAntennae => type != AntennaeType.NONE;

		internal Antennae(Guid creatureID) : this(creatureID, AntennaeType.defaultValue)
		{ }

		internal Antennae(Guid creatureID, AntennaeType antennaeType) : base(creatureID)
		{
			type = antennaeType ?? throw new ArgumentNullException(nameof(antennaeType));
		}

		//default implementations of update and restore are valid. 

		internal override bool Validate(bool correctInvalidData)
		{
			AntennaeType antennae = type;
			bool retVal = AntennaeType.Validate(ref antennae, correctInvalidData);
			type = antennae;
			return retVal;
		}

		public override AntennaeData AsReadOnlyData()
		{
			return new AntennaeData(this);
		}
	}

	public sealed partial class AntennaeType : SaveableBehavior<AntennaeType, Antennae, AntennaeData>
	{
		private static int indexMaker = 0;
		private static readonly List<AntennaeType> antennaes = new List<AntennaeType>();
		public static readonly ReadOnlyCollection<AntennaeType> availableTypes = new ReadOnlyCollection<AntennaeType>(antennaes);

		public static AntennaeType defaultValue => AntennaeType.NONE;


		//C# 7.2 magic. basically, prevents it from being messed with except internally.
		private AntennaeType(SimpleDescriptor desc, DescriptorWithArg<Antennae> fullDesc, TypeAndPlayerDelegate<Antennae> playerDesc,
			ChangeType<Antennae> transformMessage, RestoreType<Antennae> revertToDefault) : base(desc, fullDesc, playerDesc, transformMessage, revertToDefault)
		{
			_index = indexMaker++;
			antennaes.AddAt(this, _index);
		}

		public override int index => _index;
		private readonly int _index;

		internal static AntennaeType Deserialize(int index)
		{
			if (index < 0 || index >= antennaes.Count)
			{
				throw new ArgumentException("index for antennae type desrialize out of range");
			}
			else
			{
				AntennaeType antennae = antennaes[index];
				if (antennae != null)
				{
					return antennae;
				}
				else
				{
					throw new ArgumentException("index for antennae type points to an object that does not exist. this may be due to obsolete code");
				}
			}
		}

		internal static bool Validate(ref AntennaeType antennae, bool correctInvalidData)
		{
			if (antennaes.Contains(antennae))
			{
				return true;
			}
			else if (correctInvalidData)
			{
				antennae = NONE;
			}
			return false;
		}

		//Don't do this to this level lol. I just used lambdas everywhere because i changed the signature in the base to make things behave better globally, and didn't want to deal 
		//with doing that to everything in here. do use lambdas if you need something not there or you want to use the empty string. 
		public static readonly AntennaeType NONE = new AntennaeType(GlobalStrings.None, (x) => GlobalStrings.None(), (x, y) => GlobalStrings.None(), RemoveAntennaeStr, GlobalStrings.RevertAsDefault);

		public static readonly AntennaeType BEE = new AntennaeType(BeeDesc, BeeFullDesc,
			(x, y) => BeePlayer(y), BeeTransform, BeeRestore);

		public static readonly AntennaeType COCKATRICE = new AntennaeType(CockatriceDesc, CockatriceFullDesc,
			(x, y) => CockatricePlayer(y), CockatriceTransform, CockatriceRestore);
	}

	public sealed class AntennaeData : BehavioralSaveablePartData<AntennaeData, Antennae, AntennaeType>
	{

		internal AntennaeData(Antennae source) : base(GetID(source), GetBehavior(source))
		{ }
	}

}

