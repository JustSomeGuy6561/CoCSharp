//Back.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 1:58 AM
using CoC.Creatures;
using CoC.EpidermalColors;
using CoC.Tools;
using  CoC.BodyParts.SpecialInteraction;

using static CoC.UI.TextOutput;
using static   CoC.BodyParts.BackStrings;
namespace  CoC.BodyParts
{
	//implement i fur aware if you want it to update with the player.
	//note that if you do so you'll need some sort of logic to deal with if it 
	//was dyed recently/ ever.
	public class Back : BodyPartBase<Back, BackType>, IDyeable//, IFurAware
	{
		public override BackType type
		{
			get => _type;
			protected set
			{
				if (value.usesHair != type.usesHair )
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
		private BackType _type;
		public HairFurColors hairFur { get; protected set; }

		protected Back(BackType backType)
		{
			type = backType;
		}

		public static Back Generate()
		{
			return GenerateNonStandard(BackType.NORMAL);
		}

		public static Back GenerateNonStandard(BackType backType)
		{
			return new Back(backType);
		}

		public static Back GenerateDraconicMane(DragonBackMane dragonMane, HairFurColors maneColor)
		{
			return new Back(dragonMane)
			{
				hairFur = maneColor == HairFurColors.NO_HAIR_FUR ? dragonMane.defaultColor : maneColor
			};
		}

		public override bool Restore()
		{
			if (type != BackType.NORMAL)
			{
				type = BackType.NORMAL;
				return true;
			}
			return false;
		}

		public override bool RestoreAndDisplayMessage(Player player)
		{
			if (type == BackType.NORMAL)
			{
				return false;
			}
			OutputText(restoreString(player));
			Restore();
			return true;
		}

		public bool UpdateBack(BackType newType)
		{
			if (type == newType)
			{
				return false;
			}
			type = newType;
			return true;
		}

		public bool UpdateBack(DragonBackMane dragonMane, HairFurColors maneColor)
		{
			if (type == dragonMane)
			{
				return false;
			}
			type = dragonMane;
			return true;
		}

		public bool UpdateBackAndDisplayMessage(BackType newType, Player player)
		{
			if (type == newType)
			{
				return false;
			}
			OutputText(transformInto(newType, player));
			return UpdateBack(newType);
		}

		public bool UpdateBackAndDisplayMessage(DragonBackMane dragonMane, HairFurColors maneColor, Player player)
		{
			if (type == dragonMane)
			{
				return false;
			}
			OutputText(transformInto(dragonMane, player));
			return UpdateBack(dragonMane, maneColor);
		}

		public bool canDye()
		{
			return type.usesHair;
		}

		public bool attemptToDye(HairFurColors dye)
		{
			if (!canDye() || dye == hairFur)
			{
				return false;
			}
			else
			{ 
				hairFur = dye;
				return true;
			}
		}
	}

	public class BackType : BodyPartBehavior<BackType, Back>
	{
		private static int indexMaker = 0;
		private readonly int _index;
		public override int index => _index;
		public bool usesHair => this is DragonBackMane;

		protected BackType(SimpleDescriptor shortDesc, DescriptorWithArg<Back> fullDesc, TypeAndPlayerDelegate<Back> playerDesc,
			ChangeType<Back> transform, RestoreType<Back> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
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
		public DragonBackMane() : base(DraconicManeDesc, DraconicManeFullDesc, DraconicManePlayerStr, DraconicManeTransformStr, DraconicManeRestoreStr)
		{
			defaultColor = HairFurColors.GREEN;
		}
	}

}
