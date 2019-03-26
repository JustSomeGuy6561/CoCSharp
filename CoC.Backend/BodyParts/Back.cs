//Back.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 1:58 AM
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Save;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace CoC.Backend.BodyParts
{
	//implement i fur aware if you want it to update with the player.
	//note that if you do so you'll need some sort of logic to deal with if it 
	//was dyed recently/ ever.
	[DataContract]
	public class Back : BodyPartBase<Back, BackType>, IDyeable, ISaveableBase//, IFurAware
	{
		public HairFurColors hairFur { get; protected set; } = HairFurColors.NO_HAIR_FUR; //set automatically via type property. can be manually set via dyeing.

		protected Back(BackType backType)
		{
			_type = backType ?? throw new ArgumentNullException();
		}

		public override BackType type
		{
			get => _type;
			protected set
			{
				if (value.usesHair != type.usesHair)
				{
					if (value.usesHair && hairFur == HairFurColors.NO_HAIR_FUR)
					{
						hairFur = ((DragonBackMane)value).defaultColor;
					}
					else if (!value.usesHair)
					{
						hairFur = HairFurColors.NO_HAIR_FUR;
					}
				}
				_type = value;
			}

		}
		private BackType _type = BackType.NORMAL;

		public override bool isDefault => type == BackType.NORMAL;

		internal override bool Validate(bool correctDataIfInvalid = false)
		{
			BackType backType = type;
			var hair = hairFur;
			bool retVal = BackType.Validate(ref backType, ref hair, correctDataIfInvalid);
			hairFur = hair;
			type = backType;
			return retVal;
		}


		internal static Back GenerateDefault()
		{
			return new Back(BackType.NORMAL);
		}

		internal static Back GenerateDefaultOfType(BackType backType)
		{
			return new Back(backType);
		}
		internal static Back GenerateDraconicMane(DragonBackMane dragonMane, HairFurColors maneColor)
		{
			return new Back(dragonMane)
			{
				hairFur = HairFurColors.IsNullOrEmpty(maneColor) ? dragonMane.defaultColor : maneColor
			};
		}

		internal override bool Restore()
		{
			if (type != BackType.NORMAL)
			{
				type = BackType.NORMAL;
				return true;
			}
			return false;
		}

		public bool UpdateBack(BackType newType)
		{
			if (newType == null || type == newType)
			{
				return false;
			}
			type = newType;
			hairFur = (type as DragonBackMane)?.defaultColor ?? HairFurColors.NO_HAIR_FUR;
			return true;
		}

		public bool UpdateBack(DragonBackMane dragonMane, HairFurColors maneColor)
		{
			if (dragonMane == null || type == dragonMane)
			{
				return false;
			}
			type = dragonMane;
			if (!HairFurColors.IsNullOrEmpty(maneColor))
			{
				hairFur = maneColor;
			}
			else
			{
				maneColor = dragonMane.defaultColor;
			}
			return true;
		}

		public bool allowsDye()
		{
			return type.usesHair;
		}

		bool IDyeable.isDifferentColor(HairFurColors dyeColor)
		{
			if (dyeColor == null) throw new ArgumentNullException();
			return dyeColor != hairFur;
		}

		bool IDyeable.attemptToDye(HairFurColors dye)
		{
			if (!allowsDye() || dye == hairFur || HairFurColors.IsNullOrEmpty(dye))
			{
				return false;
			}
			else
			{
				hairFur = dye;
				return true;
			}
		}

		internal override Type currentSaveVersion => typeof(BackSurrogateVersion1);

		internal override Type[] saveVersions => new Type[] { typeof(BackSurrogateVersion1) };
		internal override BodyPartSurrogate<Back, BackType> ToCurrentSave()
		{
			return new BackSurrogateVersion1()
			{
				backType = index,
				hairFur = hairFur
			};
		}

		internal Back(BackSurrogateVersion1 surrogate)
		{
			//allow type property to set hair to default.
			type = BackType.Deserialize(surrogate.backType);
			//override it if we have good hair data.
			if (!HairFurColors.IsNullOrEmpty(surrogate.hairFur) && _type.usesHair)
			{
				hairFur = surrogate.hairFur;
			}
		}
	}

	public partial class BackType : BodyPartBehavior<BackType, Back>
	{
		private static int indexMaker = 0;
		private static readonly List<BackType> backs = new List<BackType>();
		private readonly int _index;
		public override int index => _index;
		public virtual bool usesHair => false;

		protected BackType(SimpleDescriptor shortDesc, DescriptorWithArg<Back> fullDesc, TypeAndPlayerDelegate<Back> playerDesc,
			ChangeType<Back> transform, RestoreType<Back> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			backs.AddAt(this, _index);
		}

		internal static BackType Deserialize(int index)
		{
			if (index < 0 || index >= backs.Count)
			{
				throw new System.ArgumentException("index for back type deserialize out of range");
			}
			else
			{
				BackType back = backs[index];
				if (back != null)
				{
					return back;
				}
				else
				{
					throw new System.ArgumentException("index for arm type points to an object that does not exist. this may be due to obsolete code");
				}
			}
		}

		internal static bool Validate(ref BackType backType, ref HairFurColors hair, bool correctInvalidData = false)
		{
			bool valid = true;
			if (!backs.Contains(backType))
			{
				if (!correctInvalidData)
				{
					return false;
				}
				valid = false;
				backType = NORMAL;
			}
			valid &= backType.ValidateData(ref hair, correctInvalidData);
			return valid;
		}

		internal virtual bool ValidateData(ref HairFurColors hair, bool correctInvalidData = false)
		{
			return true;
		}

		//i'd love to combine these, but they're actually entirely dependant on if the player used hair serum on Ember. 
		//it's such a specific check i can't realistically do it in here. 
		public static readonly BackType NORMAL = new BackType(NormalDesc, NormalFullDesc, NormalPlayerStr, NormalTransformStr, NormalRestoreStr);
		public static readonly DragonBackMane DRACONIC_MANE = new DragonBackMane();
		public static readonly BackType DRACONIC_SPIKES = new BackType(DraconicSpikesDesc, DraconicSpikesFullDesc, DraconicSpikesPlayerStr, DraconicSpikesTransformStr, DraconicSpikesRestoreStr);
		public static readonly BackType SHARK_FIN = new BackType(SharkFinDesc, SharkFinFullDesc, SharkFinPlayerStr, SharkFinTransformStr, SharkFinRestoreStr);

	}
	public class DragonBackMane : BackType
	{
		public readonly HairFurColors defaultColor;
		public override bool usesHair => true;

		internal DragonBackMane() : base(DraconicManeDesc, DraconicManeFullDesc, DraconicManePlayerStr, DraconicManeTransformStr, DraconicManeRestoreStr)
		{
			defaultColor = HairFurColors.GREEN;
		}

		internal override bool ValidateData(ref HairFurColors hair, bool correctInvalidData = false)
		{
			if (!HairFurColors.IsNullOrEmpty(hair))
			{
				return true;
			}
			else if (correctInvalidData)
			{
				hair = defaultColor;
			}
			return false;
		}
	}

	[DataContract]
	public sealed class BackSurrogateVersion1 : BodyPartSurrogate<Back, BackType>
	{
		[DataMember]
		public int backType;
		[DataMember]
		public HairFurColors hairFur;
		internal override Back ToBodyPart()
		{
			return new Back(this);
		}
	}
}
