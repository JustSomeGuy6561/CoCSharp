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
namespace CoC.BodyParts
{
	class Back : BodyPartBase<Back, BackType>, IDyeable
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
			OutputText(transformFrom(this, player));
			type = newType;
			return true;
		}

	}

	class BackType : BodyPartBehavior<BackType, Back>
	{
		private static int indexMaker = 0;
		private readonly int _index;
		public override int index => _index;
		public readonly HairFurColors defaultColor;
		public readonly bool usesHair;

		protected BackType(GenericDescription shortDesc, CreatureDescription<Back> creatureDesc, PlayerDescription<Back> playerDesc,
			ChangeType<Back> transform, ChangeType<Back> restore) : base(shortDesc, creatureDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			defaultColor = HairFurColors.NO_HAIR_FUR;
			usesHair = false;
		}
		protected BackType(HairFurColors defaultHairFur,
			GenericDescription shortDesc, CreatureDescription<Back> creatureDesc, PlayerDescription<Back> playerDesc,
			ChangeType<Back> transform, ChangeType<Back> restore) : base(shortDesc, creatureDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			defaultColor = defaultHairFur;
			usesHair = true;
		}
		//i'd love to combine these, but they're actually entirely dependant on if the player used hair serum on Ember. 
		//it's such a specific check i can't realistically do it in here. 
		public static readonly BackType NORMAL = new BackType(NormalDesc, NormalCreatureStr, NormalPlayerStr, NormalTransformStr, NormalRestoreStr);
		public static readonly BackType DRACONIC_MANE = new BackType(HairFurColors.GREEN, DraconicManeDesc, DraconicManeCreatureStr, DraconicManePlayerStr, DraconicManeTransformStr, DraconicManeRestoreStr);
		public static readonly BackType DRACONIC_SPIKES = new BackType(DraconicSpikesDesc, DraconicSpikesCreatureStr, DraconicSpikesPlayerStr, DraconicSpikesTransformStr, DraconicSpikesRestoreStr);
		public static readonly BackType SHARK_FIN = new BackType(SharkFinDesc, SharkFinCreatureStr, SharkFinPlayerStr, SharkFinTransformStr, SharkFinRestoreStr);
	}
}
