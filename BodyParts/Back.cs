//Back.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 1:58 AM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoC.BodyParts.SpecialInteraction;
using CoC.Tools;
using static CoC.UI.TextOutput;
namespace CoC.BodyParts
{
	class Back : BodyPartBase<Back, BackType>, IDyeable
	{
		public override BackType type { get; protected set; }

		protected Back(BackType backType)
		{
			type = backType;
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
			if (type != BackType.NORMAL)
			{
				OutputText(restoreString(this, player));
				type = BackType.NORMAL;
				return true;
			}
			return false;
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

		protected BackType(GenericDescription shortDesc, CreatureDescription<Back> creatureDesc, PlayerDescription<Back> playerDesc, 
			ChangeType<Back> transform, ChangeType<Back> restore) : base(shortDesc, creatureDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
		}
		protected BackType(GenericDescription shortDesc, CreatureDescription<Back> creatureDesc, PlayerDescription<Back> playerDesc,
	ChangeType<Back> transform, ChangeType<Back> restore) : base(shortDesc, creatureDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
		}
		public static readonly BackType NORMAL
        public static readonly BackType DRACONIC_MANE
        public static readonly BackType DRACONIC_SPIKES
        public static readonly BackType SHARK_FIN
	}
}
