//UnderBody.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 10:09 PM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoC.Tools;

namespace CoC.BodyParts
{
	class UnderBody : BodyPartBase<UnderBody, UnderBodyType>
	{
		protected UnderBody()
		{
		}

		public override UnderBodyType type { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

		public override bool Restore()
		{
			throw new NotImplementedException();
		}

		public override bool RestoreAndDisplayMessage(Player player)
		{
			throw new NotImplementedException();
		}
	}

	class UnderBodyType : BodyPartBehavior<UnderBodyType, UnderBody>
	{
		private static int indexMaker = 0;
		protected UnderBodyType(//class specific stuff here.
			GenericDescription shortDesc, FullDescription<UnderBody> fullDesc, PlayerDescription<UnderBody> playerDesc, 
			ChangeType<UnderBody> transform, ChangeType<UnderBody> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
		}

		private readonly int _index;
		public override int index => _index;
	}
}
