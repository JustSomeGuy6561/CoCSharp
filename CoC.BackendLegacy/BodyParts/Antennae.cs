﻿//Antennae.cs
//Description:
//Author: JustSomeGuy
//12/30/2018, 10:08 PM


using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

[assembly: InternalsVisibleTo("CoCLibTest")]
namespace CoC.Backend.BodyParts
{
	[DataContract]
	public class Antennae : BehavioralSaveablePart<Antennae, AntennaeType>
	{

		public override AntennaeType type { get; protected set; }

		private protected Antennae(AntennaeType antennaeType)
		{
			type = antennaeType ?? throw new ArgumentNullException();
		}
		public override bool isDefault => type == AntennaeType.NONE;
		internal override bool Validate(bool correctDataIfInvalid = false)
		{
			AntennaeType antennae = type; 
			bool retVal = AntennaeType.Validate(ref antennae, correctDataIfInvalid);
			type = antennae;
			return retVal;
		}

		internal static Antennae GenerateDefault()
		{
			return new Antennae(AntennaeType.NONE);
		}

		internal static Antennae GenerateDefaultOfType(AntennaeType antennaeType)
		{
			return new Antennae(antennaeType);
		}

		internal bool UpdateAntennae(AntennaeType newType)
		{
			if (newType == null || type == newType)
			{
				return false;
			}
			type = newType;
			return type == newType;
		}

		internal override bool Restore()
		{
			if (type == AntennaeType.NONE)
			{
				return false;
			}
			type = AntennaeType.NONE;
			return type == AntennaeType.NONE;
		}

		#region serialization

		private protected override Type[] saveVersions => new Type[] { typeof(AntennaeSurrogateVersion1) };
		private protected override Type currentSaveVersion => typeof(AntennaeSurrogateVersion1);

		private protected override BehavioralSurrogateBase<Antennae, AntennaeType> ToCurrentSave()
		{
			return new AntennaeSurrogateVersion1()
			{
				antennaeType = index
			};
		}

		internal Antennae(AntennaeSurrogateVersion1 surrogate) : this(AntennaeType.Deserialize(surrogate.antennaeType)) { }
		#endregion
	}

	public partial class AntennaeType : SaveableBehavior<AntennaeType, Antennae>
	{
		private static int indexMaker = 0;
		private static readonly List<AntennaeType> antennaes = new List<AntennaeType>();

		//C# 7.2 magic. basically, prevents it from being messed with except internally.
		private protected AntennaeType(SimpleDescriptor desc, DescriptorWithArg<Antennae> fullDesc, TypeAndPlayerDelegate<Antennae> playerDesc,
			ChangeType<Antennae> transformMessage, RestoreType<Antennae> revertToDefault) : base(desc, fullDesc, playerDesc, transformMessage, revertToDefault)
		{
			_index = indexMaker++;
			antennaes.AddAt(this, _index);
		}

		public override int index
		{
			get { return _index; }
		}
		private readonly int _index;

		internal static AntennaeType Deserialize(int index)
		{
			if (index < 0 || index >= antennaes.Count)
			{
				throw new System.ArgumentException("index for antennae type desrialize out of range");
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
					throw new System.ArgumentException("index for antennae type points to an object that does not exist. this may be due to obsolete code");
				}
			}
			//return antennaes[0];
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

	[DataContract]
	public sealed class AntennaeSurrogateVersion1 : BehavioralSurrogateBase<Antennae, AntennaeType>
	{
		[DataMember]
		public int antennaeType;

		public AntennaeSurrogateVersion1() : base() { }

		internal override Antennae ToBodyPart()
		{
			return new Antennae(this);
		}
	}
}
