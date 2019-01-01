using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoC.Items;
using CoC.Tools;

namespace CoC.BodyParts
{
	class Core : EpidermalBodyPart<Core, CoreType>
	{
		public override CoreType type { get; protected set; }

		public override void Restore()
		{
			type = CoreType.PLAIN;
		}
	}

	class CoreType : EpidermalBodyPartBehavior<CoreType, Core>
	{
		private static int indexMaker = 0;
		protected CoreType(string defaultAdjective, Epidermis epidermis)
		{
			_index = indexMaker++;
			adjective = defaultAdjective;
			_epidermis = epidermis;
		}
		protected readonly string adjective;
		public override Epidermis epidermis => _epidermis;
		private readonly Epidermis _epidermis;
		public override int index => _index;
		private readonly int _index;

		public override GenericDescription shortDescription { get; protected set; }
		public override CreatureDescription<Core> creatureDescription {get; protected set;}
		public override PlayerDescription<Core> playerDescription {get; protected set;}
		public override ChangeType<CoreType> transformFrom {get; protected set;}

		public override bool canDye()
		{
			return epidermis.canDye();
		}

		public override bool canTone()
		{
			return epidermis.canTone();
		}

		public override bool tryToDye(ref Dyes currentColor, Dyes newColor)
		{
			return epidermis.tryToDye(ref currentColor, newColor);
		}

		public override bool tryToTone(ref Tones currentTone, Tones newTone)
		{
			return epidermis.tryToTone(ref currentTone, newTone);
		}

		public override string defaultEpidermalAdjective()
		{
			return adjective;
		}
	}
}
