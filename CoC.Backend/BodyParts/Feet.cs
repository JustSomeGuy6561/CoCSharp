//Feet.cs
//Description:
//Author: JustSomeGuy
//1/24/2019, 9:50 PM
using System;

namespace CoC.Backend.BodyParts
{
	//may need to update the text things here to use their own delgates because we need additional info about it.
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

		//foot text in this case is literally just [noun]
		//short desc is basically just [noun], but with they type adjective.
		//long desc is short desc here, but with the option of an article friendly format in singular.

		private readonly ShortMaybePluralDescriptor footStr;
		private readonly ShortMaybePluralDescriptor shortDesc;

		//note that long desc cannot describe the actualy count b/c we don't know it.
		//long desc is literally short desc, but if you set the alternate format flag, and plural is false, it adds the correct article.
		private readonly MaybePluralPartDescriptor<FootData> longDesc;
		//full desc is long desc + 'toes', if applicable.
		private readonly MaybePluralPartDescriptor<FootData> fullDesc;

		//strings: noun text. can be plural or singular, unless the type EXPLICITELY PREVENTS PLURAL, at which point it is singular.
		//short description: same as above.



		//and this doesnt even work because it makes no sense.
		//also we dont need toes, fuck that shit.

		public string FootText(bool pluralIfApplicable) => footStr(pluralIfApplicable, out bool _);
		public string FootText(bool pluralIfApplicable, out bool isPlural) => footStr(pluralIfApplicable, out isPlural);

		public string ShortDescription(bool pluralIfApplicable) => shortDesc(pluralIfApplicable, out bool _);
		public string ShortDescription(bool pluralIfApplicable, out bool isPlural) => shortDesc(pluralIfApplicable, out isPlural);

		public string LongDescription(FootData foot) => longDesc(foot, false, true, out bool _);
		public string LongDescription(FootData foot, out bool isPlural) => longDesc(foot, false, true, out isPlural);

		public string LongDescription(FootData foot, bool pluralIfApplicable) => longDesc(foot, false, pluralIfApplicable, out bool _);
		public string LongDescription(FootData foot, bool pluralIfApplicable, out bool isPlural) => longDesc(foot, false, pluralIfApplicable, out isPlural);

		public string LongDescription(FootData foot, bool alternateFormat, bool pluralIfApplicable) => longDesc(foot, false, pluralIfApplicable, out bool _);
		public string LongDescription(FootData foot, bool alternateFormat, bool pluralIfApplicable, out bool isPlural) => longDesc(foot, false, pluralIfApplicable, out isPlural);

		public string FullDescription(FootData foot) => fullDesc(foot, false, true, out bool _);
		public string FullDescription(FootData foot, out bool isPlural) => fullDesc(foot, false, true, out isPlural);

		public string FullDescription(FootData foot, bool pluralIfApplicable) => fullDesc(foot, false, pluralIfApplicable, out bool _);
		public string FullDescription(FootData foot, bool pluralIfApplicable, out bool isPlural) => fullDesc(foot, false, pluralIfApplicable, out isPlural);

		public string FullDescription(FootData foot, bool alternateFormat, bool pluralIfApplicable) => fullDesc(foot, false, pluralIfApplicable, out bool _);
		public string FullDescription(FootData foot, bool alternateFormat, bool pluralIfApplicable, out bool isPlural) => fullDesc(foot, false, pluralIfApplicable, out isPlural);

		public bool isFeet => footStyle == FootStyle.FEET;
		public bool isPaws => footStyle == FootStyle.PAWS;
		public bool isHooves => footStyle == FootStyle.HOOVES;
		public bool isInsectoid => footStyle == FootStyle.INSECTOID;
		public bool isClaws => footStyle == FootStyle.CLAWS;
		public bool isOther => !(isFeet || isClaws || isHooves || isInsectoid || isPaws);

		private FootType(FootStyle style, ShortMaybePluralDescriptor nounText, ShortMaybePluralDescriptor shortDesc, SimpleDescriptor singleDesc,
			MaybePluralPartDescriptor<FootData> longDesc) : base(PluralHelper(shortDesc),	singleDesc)
		{
			_index = indexMaker++;
			footStyle = style;

			this.shortDesc = shortDesc;
			this.longDesc = longDesc ?? throw new ArgumentNullException(nameof(longDesc));
			this.fullDesc = fullDesc ?? throw new ArgumentNullException(nameof(fullDesc));
			footStr = nounText ?? throw new ArgumentNullException(nameof(nounText));
		}

		public override int index => _index;
		private readonly int _index;

		public static FootType HUMAN = new FootType(FootStyle.FEET, HumanNoun, HumanDesc, HumanSingleDesc, HumanLongDesc);
		public static FootType HOOVES = new FootType(FootStyle.HOOVES, HoovesNoun, HoovesDesc, HoovesSingleDesc, HoovesLongDesc);
		public static FootType PAW = new FootType(FootStyle.PAWS, PawNoun, PawDesc, PawSingleDesc, PawLongDesc);
		public static FootType GOO_NONE = new FootType(FootStyle.OTHER, GooNoun, GooDesc, GooSingleDesc, GooLongDesc);
		public static FootType NAGA_NONE = new FootType(FootStyle.OTHER, NagaNoun, NagaDesc, NagaSingleDesc, NagaLongDesc);
		public static FootType DEMON_HEEL = new FootType(FootStyle.OTHER, DemonHeelNoun, DemonHeelDesc, DemonHeelSingleDesc, DemonHeelLongDesc);
		public static FootType DEMON_CLAW = new FootType(FootStyle.CLAWS, DemonClawNoun, DemonClawDesc, DemonClawSingleDesc, DemonClawLongDesc);
		public static FootType INSECTOID = new FootType(FootStyle.INSECTOID, InsectoidNoun, InsectoidDesc, InsectoidSingleDesc, InsectoidLongDesc);
		public static FootType LIZARD_CLAW = new FootType(FootStyle.CLAWS, LizardClawNoun, LizardClawDesc, LizardClawSingleDesc, LizardClawLongDesc);
		public static FootType BRONY = new FootType(FootStyle.HOOVES, BronyNoun, BronyDesc, BronySingleDesc, BronyLongDesc);
		public static FootType RABBIT = new FootType(FootStyle.FEET, RabbitNoun, RabbitDesc, RabbitSingleDesc, RabbitLongDesc);
		public static FootType HARPY_TALON = new FootType(FootStyle.CLAWS, HarpyTalonNoun, HarpyTalonDesc, HarpyTalonSingleDesc, HarpyTalonLongDesc);
		public static FootType KANGAROO = new FootType(FootStyle.FEET, KangarooNoun, KangarooDesc, KangarooSingleDesc, KangarooLongDesc);
		public static FootType DRAGON_CLAW = new FootType(FootStyle.CLAWS, DragonClawNoun, DragonClawDesc, DragonClawSingleDesc, DragonClawLongDesc);
		public static FootType MANDER_CLAW = new FootType(FootStyle.CLAWS, ManderClawNoun, ManderClawDesc, ManderClawSingleDesc, ManderClawLongDesc);
		public static FootType IMP_CLAW = new FootType(FootStyle.CLAWS, ImpClawNoun, ImpClawDesc, ImpClawSingleDesc, ImpClawLongDesc);
		public static FootType TENDRIL = new FootType(FootStyle.OTHER, TendrilNoun, TendrilDesc, TendrilSingleDesc, TendrilLongDesc);
	}

	public sealed class FootData : BehavioralPartDataBase<FootType>
	{
		public bool isFeet => type.isFeet;
		public bool isPaws => type.isPaws;
		public bool isHooves => type.isHooves;
		public bool isInsectoid => type.isInsectoid;
		public bool isClaws => type.isClaws;
		public bool isOther => type.isOther;

		public FootData(Guid id, FootType currentType) : base(id, currentType)
		{

		}
	}
}
