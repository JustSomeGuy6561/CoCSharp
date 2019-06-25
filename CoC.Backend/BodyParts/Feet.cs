﻿//Feet.cs
//Description:
//Author: JustSomeGuy
//1/24/2019, 9:50 PM
using CoC.Backend.Tools;

namespace CoC.Backend.BodyParts
{
	public sealed class Feet : BehavioralPartBase<FootType>
	{
		private Feet(FootType value)
		{
			type = value;
		}

		internal static Feet GenerateDefault(FootType type)
		{
			return new Feet(type);
		}

		internal bool UpdateType(FootType newType)
		{
			if (type == newType)
			{
				return false;
			}
			type = newType;
			return true;
		}

		public override FootType type { get; protected set; }

		public SimpleDescriptor fullDescription => () => type.fullDescription(this);

	}

	public sealed partial class FootType : BehaviorBase
	{
		private static int indexMaker = 0;

		private enum FootStyle { FEET, PAWS, HOOVES, INSECTOID, CLAWS, OTHER }

		private readonly FootStyle footStyle;

		public readonly DescriptorWithArg<Feet> fullDescription;

		public bool isFeet => footStyle == FootStyle.FEET;
		public bool isPaws => footStyle == FootStyle.PAWS;
		public bool isHooves => footStyle == FootStyle.HOOVES;
		public bool isInsectoid => footStyle == FootStyle.INSECTOID;
		public bool isClaws => footStyle == FootStyle.CLAWS;
		public bool isOther => !(isFeet || isClaws || isHooves || isInsectoid || isPaws);
		private FootType(FootStyle style, SimpleDescriptor shortDesc, DescriptorWithArg<Feet> fullDesc) : base(shortDesc)
		{
			_index = indexMaker++;
			footStyle = style;
			fullDescription = fullDesc;
		}

		public override int index => _index;
		private readonly int _index;

		public static FootType HUMAN = new FootType(FootStyle.FEET, HumanDesc, HumanFullDesc);
		public static FootType HOOVES = new FootType(FootStyle.HOOVES, HoovesDesc, HoovesFullDesc);
		public static FootType PAW = new FootType(FootStyle.PAWS, PawDesc, PawFullDesc);
		public static FootType NONE = new FootType(FootStyle.OTHER, NoneDesc, NoneFullDesc);
		public static FootType DEMON_HEEL = new FootType(FootStyle.OTHER, DemonHeelDesc, DemonHeelFullDesc);
		public static FootType DEMON_CLAW = new FootType(FootStyle.CLAWS, DemonClawDesc, DemonClawFullDesc);
		public static FootType INSECTOID = new FootType(FootStyle.INSECTOID, InsectoidDesc, InsectoidFullDesc);
		public static FootType LIZARD_CLAW = new FootType(FootStyle.CLAWS, LizardClawDesc, LizardClawFullDesc);
		public static FootType BRONY = new FootType(FootStyle.HOOVES, BronyDesc, BronyFullDesc);
		public static FootType RABBIT = new FootType(FootStyle.FEET, RabbitDesc, RabbitFullDesc);
		public static FootType HARPY_TALON = new FootType(FootStyle.CLAWS, HarpyTalonDesc, HarpyTalonFullDesc);
		public static FootType KANGAROO = new FootType(FootStyle.FEET, KangarooDesc, KangarooFullDesc);
		public static FootType DRAGON_CLAW = new FootType(FootStyle.CLAWS, DragonClawDesc, DragonClawFullDesc);
		public static FootType MANDER_CLAW = new FootType(FootStyle.CLAWS, ManderClawDesc, ManderClawFullDesc);
		public static FootType IMP_CLAW = new FootType(FootStyle.CLAWS, ImpClawDesc, ImpClawFullDesc);
	}
}