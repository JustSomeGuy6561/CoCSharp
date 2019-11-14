//Feet.cs
//Description:
//Author: JustSomeGuy
//1/24/2019, 9:50 PM
using System;

namespace CoC.Backend.BodyParts
{
	public sealed partial class Feet : PartWithBehavioralEventsBase<Feet, FootType, FootWrapper>
	{
		public override string BodyPartName() => Name();

		internal Feet(Guid creatureID, FootType value) : base(creatureID)
		{
			type = value ?? throw new ArgumentNullException(nameof(value));
		}

		public override FootWrapper AsReadOnlyReference()
		{
			return new FootWrapper(this);
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

		internal void DoGenericOrgasm(bool dryOrgasm)
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

		public static FootType HUMAN = new FootType(FootStyle.FEET, HumanDesc, HumanLongDesc);
		public static FootType HOOVES = new FootType(FootStyle.HOOVES, HoovesDesc, HoovesLongDesc);
		public static FootType PAW = new FootType(FootStyle.PAWS, PawDesc, PawLongDesc);
		public static FootType NONE = new FootType(FootStyle.OTHER, NoneDesc, NoneLongDesc);
		public static FootType DEMON_HEEL = new FootType(FootStyle.OTHER, DemonHeelDesc, DemonHeelLongDesc);
		public static FootType DEMON_CLAW = new FootType(FootStyle.CLAWS, DemonClawDesc, DemonClawLongDesc);
		public static FootType INSECTOID = new FootType(FootStyle.INSECTOID, InsectoidDesc, InsectoidLongDesc);
		public static FootType LIZARD_CLAW = new FootType(FootStyle.CLAWS, LizardClawDesc, LizardClawLongDesc);
		public static FootType BRONY = new FootType(FootStyle.HOOVES, BronyDesc, BronyLongDesc);
		public static FootType RABBIT = new FootType(FootStyle.FEET, RabbitDesc, RabbitLongDesc);
		public static FootType HARPY_TALON = new FootType(FootStyle.CLAWS, HarpyTalonDesc, HarpyTalonLongDesc);
		public static FootType KANGAROO = new FootType(FootStyle.FEET, KangarooDesc, KangarooLongDesc);
		public static FootType DRAGON_CLAW = new FootType(FootStyle.CLAWS, DragonClawDesc, DragonClawLongDesc);
		public static FootType MANDER_CLAW = new FootType(FootStyle.CLAWS, ManderClawDesc, ManderClawLongDesc);
		public static FootType IMP_CLAW = new FootType(FootStyle.CLAWS, ImpClawDesc, ImpClawLongDesc);
	}

	public sealed class FootWrapper : PartWithBehavioralEventsWrapper<FootWrapper, Feet, FootType>
	{
		public bool isFeet => type.isFeet;
		public bool isPaws => type.isPaws;
		public bool isHooves => type.isHooves;
		public bool isInsectoid => type.isInsectoid;
		public bool isClaws => type.isClaws;
		public bool isOther => type.isOther;

		public FootWrapper(Feet source) : base(source)
		{

		}
	}
}
