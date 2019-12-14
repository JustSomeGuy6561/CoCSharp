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

		//tbh idk how this would even work but whatever
		public uint orgasmCount { get; private set; } = 0;
		public uint dryOrgasmCount { get; private set; } = 0;

		public uint lickCount { get; private set; } = 0;
		public uint penetrateCount { get; private set; } = 0; //times foot has been used to penetrate whatever
		public uint rubCount { get; private set; } = 0; //times jerked off a cock with feet.

		//default update is fine

		public override FootType type { get; protected set; }

		public string FootText(bool plural) => type.FootText(plural);

		public string ShortDescription(bool plural) => type.ShortDescription(plural);

		public string LongDescription() => type.LongDescription(AsReadOnlyData());
		public string LongDescription(bool alternateForm) => type.LongDescription(AsReadOnlyData(), alternateForm);

		public string LongPrimaryDescription() => type.LongPrimaryDescription(AsReadOnlyData());
		public string LongAlternateDescription() => type.LongAlternateDescription(AsReadOnlyData());

		public string LongDescription(bool alternateForm, bool plural) => type.LongDescription(AsReadOnlyData(), alternateForm, plural);

		public string LongPrimaryDescription(bool plural) => type.LongPrimaryDescription(AsReadOnlyData(), plural);
		public string LongAlternateDescription(bool plural) => type.LongAlternateDescription(AsReadOnlyData(), plural);

		public string FullDescription() => type.FullDescription(AsReadOnlyData());
		public string FullDescription(bool alternateForm) => type.FullDescription(AsReadOnlyData(), alternateForm);

		public string FullPrimaryDescription() => type.FullPrimaryDescription(AsReadOnlyData());
		public string FullAlternateDescription() => type.FullAlternateDescription(AsReadOnlyData());

		public string FullDescription(bool alternateForm, bool plural) => type.FullDescription(AsReadOnlyData(), alternateForm, plural);

		public string FullPrimaryDescription(bool plural) => type.FullPrimaryDescription(AsReadOnlyData(), plural);
		public string FullAlternateDescription(bool plural) => type.FullAlternateDescription(AsReadOnlyData(), plural);

		public string LongOrFullDescription(bool alternateForm, bool plural, bool isFull)
		{
			if (isFull) return FullDescription(alternateForm, plural);
			else return LongDescription(alternateForm, plural);
		}

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

		private readonly SimplePluralDescriptor footStr;
		private readonly SimplePluralDescriptor shortPluralDesc;
		private readonly LongPluralDescriptor<FootData> longDescription;
		private readonly LongPluralDescriptor<FootData> fullDescription;


		public string FootText(bool plural) => footStr(plural);

		public string ShortDescription(bool plural) => shortPluralDesc(plural);

		public string LongDescription(FootData footData) => longDescription(footData);
		public string LongDescription(FootData footData, bool alternateForm) => longDescription(footData, alternateForm);

		public string LongPrimaryDescription(FootData footData) => longDescription(footData, false);
		public string LongAlternateDescription(FootData footData) => longDescription(footData, true);

		public string LongDescription(FootData footData, bool alternateForm, bool plural) => longDescription(footData, alternateForm, plural);

		public string LongPrimaryDescription(FootData footData, bool plural) => longDescription(footData, false, plural);
		public string LongAlternateDescription(FootData footData, bool plural) => longDescription(footData, true, plural);

		public string FullDescription(FootData footData) => fullDescription(footData);
		public string FullDescription(FootData footData, bool alternateForm) => fullDescription(footData, alternateForm);

		public string FullPrimaryDescription(FootData footData) => fullDescription(footData, false);
		public string FullAlternateDescription(FootData footData) => fullDescription(footData, true);

		public string FullDescription(FootData footData, bool alternateForm, bool plural) => fullDescription(footData, alternateForm, plural);

		public string FullPrimaryDescription(FootData footData, bool plural) => fullDescription(footData, false, plural);
		public string FullAlternateDescription(FootData footData, bool plural) => fullDescription(footData, true, plural);

		public bool isFeet => footStyle == FootStyle.FEET;
		public bool isPaws => footStyle == FootStyle.PAWS;
		public bool isHooves => footStyle == FootStyle.HOOVES;
		public bool isInsectoid => footStyle == FootStyle.INSECTOID;
		public bool isClaws => footStyle == FootStyle.CLAWS;
		public bool isOther => !(isFeet || isClaws || isHooves || isInsectoid || isPaws);

		private FootType(FootStyle style, SimplePluralDescriptor nounText, SimplePluralDescriptor shortDesc, LongPluralDescriptor<FootData> longDesc,
			LongPluralDescriptor<FootData> fullDesc) : base(PluralHelper(shortDesc))
		{
			_index = indexMaker++;
			footStyle = style;

			shortPluralDesc = shortDesc;

			footStr = nounText ?? throw new ArgumentNullException(nameof(nounText));

			longDescription = longDesc ?? throw new ArgumentNullException(nameof(longDesc));
			fullDescription = fullDesc ?? throw new ArgumentNullException(nameof(fullDesc));
		}

		public override int index => _index;
		private readonly int _index;

		public static FootType HUMAN = new FootType(FootStyle.FEET, HumanNoun, HumanDesc, HumanLongDesc, HumanFullDesc);
		public static FootType HOOVES = new FootType(FootStyle.HOOVES, HoovesNoun, HoovesDesc, HoovesLongDesc, HoovesFullDesc);
		public static FootType PAW = new FootType(FootStyle.PAWS, PawNoun, PawDesc, PawLongDesc, PawFullDesc);
		public static FootType NONE = new FootType(FootStyle.OTHER, NoneNoun, NoneDesc, NoneLongDesc, NoneFullDesc);
		public static FootType DEMON_HEEL = new FootType(FootStyle.OTHER, DemonHeelNoun, DemonHeelDesc, DemonHeelLongDesc, DemonHeelFullDesc);
		public static FootType DEMON_CLAW = new FootType(FootStyle.CLAWS, DemonClawNoun, DemonClawDesc, DemonClawLongDesc, DemonClawFullDesc);
		public static FootType INSECTOID = new FootType(FootStyle.INSECTOID, InsectoidNoun, InsectoidDesc, InsectoidLongDesc, InsectoidFullDesc);
		public static FootType LIZARD_CLAW = new FootType(FootStyle.CLAWS, LizardClawNoun, LizardClawDesc, LizardClawLongDesc, LizardClawFullDesc);
		public static FootType BRONY = new FootType(FootStyle.HOOVES, BronyNoun, BronyDesc, BronyLongDesc, BronyFullDesc);
		public static FootType RABBIT = new FootType(FootStyle.FEET, RabbitNoun, RabbitDesc, RabbitLongDesc, RabbitFullDesc);
		public static FootType HARPY_TALON = new FootType(FootStyle.CLAWS, HarpyTalonNoun, HarpyTalonDesc, HarpyTalonLongDesc, HarpyTalonFullDesc);
		public static FootType KANGAROO = new FootType(FootStyle.FEET, KangarooNoun, KangarooDesc, KangarooLongDesc, KangarooFullDesc);
		public static FootType DRAGON_CLAW = new FootType(FootStyle.CLAWS, DragonClawNoun, DragonClawDesc, DragonClawLongDesc, DragonClawFullDesc);
		public static FootType MANDER_CLAW = new FootType(FootStyle.CLAWS, ManderClawNoun, ManderClawDesc, ManderClawLongDesc, ManderClawFullDesc);
		public static FootType IMP_CLAW = new FootType(FootStyle.CLAWS, ImpClawNoun, ImpClawDesc, ImpClawLongDesc, ImpClawFullDesc);
		public static FootType TENDRIL = new FootType(FootStyle.OTHER, TendrilNoun, TendrilDesc, TendrilLongDesc, TendrilFullDesc);
	}

	public sealed class FootData : BehavioralPartDataBase<FootType>
	{
		public bool isFeet => type.isFeet;
		public bool isPaws => type.isPaws;
		public bool isHooves => type.isHooves;
		public bool isInsectoid => type.isInsectoid;
		public bool isClaws => type.isClaws;
		public bool isOther => type.isOther;

		public string FootText(bool plural) => type.FootText(plural);

		public string ShortDescription(bool plural) => type.ShortDescription(plural);

		public string LongDescription() => type.LongDescription(this);
		public string LongDescription(bool alternateForm) => type.LongDescription(this, alternateForm);

		public string LongPrimaryDescription() => type.LongPrimaryDescription(this);
		public string LongAlternateDescription() => type.LongAlternateDescription(this);

		public string LongDescription(bool alternateForm, bool plural) => type.LongDescription(this, alternateForm, plural);

		public string LongPrimaryDescription(bool plural) => type.LongPrimaryDescription(this, plural);
		public string LongAlternateDescription(bool plural) => type.LongAlternateDescription(this, plural);

		public string FullDescription() => type.FullDescription(this);
		public string FullDescription(bool alternateForm) => type.FullDescription(this, alternateForm);

		public string FullPrimaryDescription() => type.FullPrimaryDescription(this);
		public string FullAlternateDescription() => type.FullAlternateDescription(this);

		public string FullDescription(bool alternateForm, bool plural) => type.FullDescription(this, alternateForm, plural);

		public string FullPrimaryDescription(bool plural) => type.FullPrimaryDescription(this, plural);
		public string FullAlternateDescription(bool plural) => type.FullAlternateDescription(this, plural);

		public string LongOrFullDescription(bool alternateForm, bool plural, bool isFull)
		{
			if (isFull) return FullDescription(alternateForm, plural);
			else return LongDescription(alternateForm, plural);
		}

		public FootData(Guid id, FootType currentType) : base(id, currentType)
		{

		}
	}
}
