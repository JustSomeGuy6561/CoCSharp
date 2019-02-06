//Antennae.cs
//Description:
//Author: JustSomeGuy
//12/30/2018, 10:08 PM
using CoC.Creatures;
using CoC.Strings;
using CoC.Tools;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using static CoC.UI.TextOutput;

[assembly: InternalsVisibleTo("CoCLibTest")]
namespace CoC.BodyParts
{
	[DataContract]
	internal class Antennae : BodyPartBase<Antennae, AntennaeType>, ISerializable
	{

		public override AntennaeType type { get; protected set; }

		protected Antennae(AntennaeType antennaeType)
		{
			type = antennaeType;
		}

		public override bool Restore()
		{
			if (type == AntennaeType.NONE)
			{
				return false;
			}
			type = AntennaeType.NONE;
			return type == AntennaeType.NONE;
		}

		public override bool RestoreAndDisplayMessage(Player p)
		{
			if (type == AntennaeType.NONE)
			{
				return false;
			}
			OutputText(restoreString(p));
			type = AntennaeType.NONE;
			return type == AntennaeType.NONE;
		}

		public static Antennae GenerateDefault()
		{
			return new Antennae(AntennaeType.NONE);
		}

		public static Antennae GenerateDefaultOfType(AntennaeType antennaeType)
		{
			return new Antennae(antennaeType);
		}

		public bool UpdateAntennae(AntennaeType newType)
		{
			if (type == newType)
			{
				return false;
			}
			type = newType;
			return type == newType;
		}

		public bool UpdateAntennaeAndDisplayMessage(AntennaeType newType, Player player)
		{
			if (type == newType)
			{
				return false;
			}
			OutputText(transformInto(newType, player));
			type = newType;
			return type == newType;
		}
		#region serialization

		protected Antennae(SerializationInfo info, StreamingContext context)
		{
			int index = 0;
			foreach (SerializationEntry entry in info)
			{
				if (entry.Name == typeof(AntennaeType).Name)
				{
					index = (int)info.GetValue(typeof(AntennaeType).Name, typeof(int));
				}
			}
			type = AntennaeType.Deserialize(index);

		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(typeof(AntennaeType).Name, type.index);
		}
		#endregion
	}

	internal partial class AntennaeType : BodyPartBehavior<AntennaeType, Antennae>
	{
		private static int indexMaker = 0;
		private static List<AntennaeType> antennaes = new List<AntennaeType>();

		protected AntennaeType(SimpleDescriptor desc, DescriptorWithArg<Antennae> fullDesc, TypeAndPlayerDelegate<Antennae> playerDesc,
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

		public static AntennaeType Deserialize(int index)
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
}

