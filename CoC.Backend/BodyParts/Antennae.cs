﻿//Antennae.cs
//Description:
//Author: JustSomeGuy
//12/30/2018, 10:08 PM


using CoC.Backend.Strings;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

[assembly: InternalsVisibleTo("CoCLibTest")]
namespace CoC.Backend.BodyParts
{
	[DataContract]
	public class Antennae : BodyPartBase<Antennae, AntennaeType>
	{

		public override AntennaeType type { get; protected set; }
		public override bool isDefault => type == AntennaeType.NONE;

		private protected Antennae(AntennaeType antennaeType)
		{
			type = antennaeType;
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
			if (type == newType)
			{
				return false;
			}
			type = newType;
			return type == newType;
		}


		#region serialization

		internal override Type[] saveVersions => throw new NotImplementedException();
		internal override Type currentSaveVersion => typeof(AntennaeSurrogateVersion1);

		internal override BodyPartSurrogate<Antennae, AntennaeType> ToCurrentSave()
		{
			return new AntennaeSurrogateVersion1()
			{
				antennaeType = index
			};
		}

		internal Antennae(AntennaeSurrogateVersion1 surrogate) : this(AntennaeType.Deserialize(surrogate.antennaeType)) { }
		#endregion
	}

	public partial class AntennaeType : BodyPartBehavior<AntennaeType, Antennae>
	{
		private static int indexMaker = 0;
		private static List<AntennaeType> antennaes = new List<AntennaeType>();

		//C# 7.2 magic. basically, prevents it from being messed with except internally.
		private protected AntennaeType(SimpleDescriptor desc, DescriptorWithArg<Antennae> fullDesc, TypeAndPlayerDelegate<Antennae> playerDesc,
			ChangeType<Antennae> transformMessage, RestoreType<Antennae> revertToDefault) : base(desc, fullDesc, playerDesc, transformMessage, revertToDefault)
		{
			_index = indexMaker++;
			antennaes[_index] = this;
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

		//Don't do this to this level lol. I just used lambdas everywhere because i changed the signature in the base to make things behave better globally, and didn't want to deal 
		//with doing that to everything in here. do use lambdas if you need something not there or you want to use the empty string. 
		public static readonly AntennaeType NONE = new AntennaeType(GlobalStrings.None, (x) => GlobalStrings.None(), (x, y) => GlobalStrings.None(), RemoveAntennaeStr, GlobalStrings.RevertAsDefault);

		public static readonly AntennaeType BEE = new AntennaeType(BeeDesc, BeeFullDesc,
			(x, y) => BeePlayer(y), BeeTransform, BeeRestore);

		public static readonly AntennaeType COCKATRICE = new AntennaeType(CockatriceDesc, CockatriceFullDesc,
			(x, y) => CockatricePlayer(y), CockatriceTransform, CockatriceRestore);
	}

	[DataContract]
	public sealed class AntennaeSurrogateVersion1 : BodyPartSurrogate<Antennae, AntennaeType>
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

