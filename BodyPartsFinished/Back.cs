//Back.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 1:58 AM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoC.BodyParts.SpecialInteraction;
using CoC.EpidermalColors;
using CoC.Tools;
using static CoC.UI.TextOutput;
using static CoC.Strings.BodyParts.BackStrings;
namespace CoC.BodyParts
{
	//implement i fur aware if you want it to update with the player.
	//note that if you do so you'll need some sort of logic to deal with if it 
	//was dyed recently/ ever.
	class Back : BodyPartBase<Back, BackType>, IDyeable//, IFurAware
	{
		public override BackType type
		{
			get => _type;
			protected set
			{
				if (value != _type)
				{
					hairFur = value.defaultColor;
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
			OutputText(restoreString(this, player));
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

		public bool UpdateBackAndDisplayMessage(BackType newType, Player player)
		{
			if (type == newType)
			{
				return false;
			}
			OutputText(transformInto(newType, player));
			type = newType;
			return true;
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

	class BackType : BodyPartBehavior<BackType, Back>
	{
		private static int indexMaker = 0;
		private readonly int _index;
		public override int index => _index;
		public readonly HairFurColors defaultColor;
		public readonly bool usesHair;

		protected BackType(GenericDescription shortDesc, FullDescription<Back> fullDesc, PlayerDescription<Back> playerDesc,
			ChangeType<Back> transform, ChangeType<Back> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			defaultColor = HairFurColors.NO_HAIR_FUR;
			usesHair = false;
		}
		protected BackType(HairFurColors defaultHairFur,
			GenericDescription shortDesc, FullDescription<Back> fullDesc, PlayerDescription<Back> playerDesc,
			ChangeType<Back> transform, ChangeType<Back> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			defaultColor = defaultHairFur;
			usesHair = true;
		}
		//i'd love to combine these, but they're actually entirely dependant on if the player used hair serum on Ember. 
		//it's such a specific check i can't realistically do it in here. 
		public static readonly BackType NORMAL = new BackType(NormalDesc, NormalFullDesc, NormalPlayerStr, NormalTransformStr, NormalRestoreStr);
		public static readonly BackType DRACONIC_MANE = new BackType(HairFurColors.GREEN, DraconicManeDesc, DraconicManeFullDesc, DraconicManePlayerStr, DraconicManeTransformStr, DraconicManeRestoreStr);
		public static readonly BackType DRACONIC_SPIKES = new BackType(DraconicSpikesDesc, DraconicSpikesFullDesc, DraconicSpikesPlayerStr, DraconicSpikesTransformStr, DraconicSpikesRestoreStr);
		public static readonly BackType SHARK_FIN = new BackType(SharkFinDesc, SharkFinFullDesc, SharkFinPlayerStr, SharkFinTransformStr, SharkFinRestoreStr);
	}
}
