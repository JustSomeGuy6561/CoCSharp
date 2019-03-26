//Gills.cs
//Description:
//Author: JustSomeGuy
//12/27/2018, 7:29 PM
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
//using CoC.
namespace CoC.Backend.BodyParts
{
	[DataContract]
	public class Gills : BodyPartBase<Gills, GillType>
	{
		protected Gills()
		{
			type = GillType.NONE;
		}
		public override GillType type { get; protected set; }

		public override bool isDefault => type == GillType.NONE;

		internal static Gills GenerateDefault()
		{
			return new Gills();
		}


		internal static Gills GenerateDefaultOfType(GillType gillType)
		{
			return new Gills()
			{
				type = gillType
			};
		}

		public bool UpdateType(GillType gillType)
		{
			if (type == gillType)
			{
				return false;
			}
			type = gillType;
			return type == gillType;

		}

		internal override bool Restore()
		{
			if (type == GillType.NONE)
			{
				return false;
			}
			type = GillType.NONE;
			return type == GillType.NONE;
		}

		internal override Type currentSaveVersion => typeof(GillSurrogateVersion1);
		internal override Type[] saveVersions => new Type[] { typeof(GillSurrogateVersion1) };

		internal override BodyPartSurrogate<Gills, GillType> ToCurrentSave()
		{
			return new GillSurrogateVersion1()
			{
				gillType = index
			};
		}

		internal Gills(GillSurrogateVersion1 surrogate)
		{
			type = GillType.Deserialize(surrogate.gillType);
		}
	}

	public partial class GillType : BodyPartBehavior<GillType, Gills>
	{
		private static int indexMaker = 0;
		private static readonly List<GillType> gills = new List<GillType>();
		protected GillType(SimpleDescriptor shortDesc, DescriptorWithArg<Gills> fullDesc, TypeAndPlayerDelegate<Gills> playerDesc,
			ChangeType<Gills> transform, RestoreType<Gills> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			gills.AddAt(this, _index);
		}

		internal static GillType Deserialize(int index)
		{
			if (index < 0 || index >= gills.Count)
			{
				throw new System.ArgumentException("index for body type deserialize out of range");
			}
			else
			{
				GillType gill = gills[index];
				if (gill != null)
				{
					return gill;
				}
				else
				{
					throw new System.ArgumentException("index for gill type points to an object that does not exist. this may be due to obsolete code");
				}
			}
		}

		protected readonly int _index;
		public override int index => _index;

		public static readonly GillType NONE = new GillType(GlobalStrings.None, (x) => GlobalStrings.None(), (x, y) => GlobalStrings.None(), GlobalStrings.TransformToDefault<Gills, GillType>, GlobalStrings.RevertAsDefault);
		public static readonly GillType ANEMONE = new GillType(AnemoneDescStr, AnemoneFullDesc, AnemonePlayerStr, AnemoneTransformStr, AnemoneRestoreStr);
		public static readonly GillType FISH = new GillType(FishDescStr, FishFullDesc, FishPlayerStr, FishTransformStr, FishRestoreStr);
	}

	[DataContract]
	public sealed class GillSurrogateVersion1 : BodyPartSurrogate<Gills, GillType>
	{
		[DataMember]
		public int gillType;
		internal override Gills ToBodyPart()
		{
			return new Gills(this);
		}
	}
}
