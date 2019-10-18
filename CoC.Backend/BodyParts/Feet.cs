//Feet.cs
//Description:
//Author: JustSomeGuy
//1/24/2019, 9:50 PM
using System;

namespace CoC.Backend.BodyParts
{
	public sealed partial class Feet : PartWithBehaviorAndEventBase<Feet, FootType, FootData>
	{
		public override string BodyPartName() => Name();

		internal Feet(Guid creatureID, FootType value) : base(creatureID)
		{
			type = value ?? throw new ArgumentNullException(nameof(value));
		}

		public override FootData AsReadOnlyData()
		{
			return new FootData(creatureID, type);
		}

		public uint orgasmCount { get; private set; } = 0;
		public uint dryOrgasmCount { get; private set; } = 0;
		public uint lickCount { get; private set; } = 0;
		public uint penetrateCount { get; private set; } = 0;
		public uint rubCount { get; private set; } = 0;

		//default update is fine

		public override FootType type { get; protected set; }

		public SimpleDescriptor fullDescription => () => type.fullDescription(this);

		internal void GetLicked(bool reachOrgasm)
		{
			lickCount++;
			if (reachOrgasm)
			{
				orgasmCount++;
			}
		}

		internal void DoPenetrate(bool reachOrgasm)
		{
			penetrateCount++;
			if (reachOrgasm)
			{
				orgasmCount++;
			}
		}

		internal void DoRubbing(bool reachOrgasm)
		{
			rubCount++;
			if (reachOrgasm)
			{
				orgasmCount++;
			}
		}

		internal void doGenericOrgasm(bool dryOrgasm)
		{
			orgasmCount++;
			if (dryOrgasm)
			{
				dryOrgasmCount++;
			}
		}
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

	public sealed class FootData : BehavioralPartDataBase<FootType>
	{
		public bool isFeet => currentType.isFeet;
		public bool isPaws => currentType.isPaws;
		public bool isHooves => currentType.isHooves;
		public bool isInsectoid => currentType.isInsectoid;
		public bool isClaws => currentType.isClaws;
		public bool isOther => currentType.isOther;

		public FootData(Guid id, FootType currentType) : base(id, currentType)
		{

		}
	}
}
