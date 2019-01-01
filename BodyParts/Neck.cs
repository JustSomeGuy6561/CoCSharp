//Neck.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 10:12 PM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoC.Tools;

namespace CoC.BodyParts
{
	class Neck : BodyPartBehavior<Neck>
	{
		private const int MIN_NECK_LENGTH = 2;
		public int length { get; protected set; }
		public NeckType neckType { get; protected set; }

		public override int index => throw new NotImplementedException();

		protected Neck()
		{
			
		}

		public override GenericDescription shortDescription {get; protected set;}
		public override CreatureDescription creatureDescription {get; protected set;}
		public override PlayerDescription playerDescription {get; protected set;}
		public override ChangeType<Neck> transformFrom {get; protected set;}

		protected string neckDescription()
		{
			return neckType.shortDescription(length);
		}

		public bool canGrowNeck()
		{
			return neckType.canGrowNeck(length);
		}

		public static void Restore(ref Neck neck)
		{
			neck.Restore();
		}

		public void Restore()
		{
			length = MIN_NECK_LENGTH;
			neckType = NeckType.HUMANOID;
		}

		public static Neck GenerateNeck()
		{
			return new Neck();
		}

	}

	class NeckType
	{
		private static int indexMaker = 0;
		private const int MIN_NECK_LENGTH = 2;
		public readonly int index;

		protected readonly int maxNeckLength;

		public bool canGrowNeck(int currNeckLength)
		{
			return currNeckLength < maxNeckLength;
		}

		protected NeckType(int maxLength, GenericDescription shortDesc, CreatureDescription creatureDesc, PlayerDescription playerDesc, ChangeType<NeckType> changeFrom)
		{
			_index = indexMaker++;
			shortDescription = shortDesc;
			creatureDescription = creatureDesc;
			playerDescription = playerDesc;
			transformFrom = changeFrom;
		}
		public override GenericDescription shortDescription {get; protected set;}
		public override CreatureDescription creatureDescription {get; protected set;}
		public override PlayerDescription playerDescription {get; protected set;}
		public override ChangeType<NeckType> transformFrom {get; protected set;}
	}
}
