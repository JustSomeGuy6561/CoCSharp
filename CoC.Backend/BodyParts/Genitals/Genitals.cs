﻿//Genitals.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 3:16 AM

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CoC.Backend.BodyParts.EventHelpers;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Engine.Time;
using CoC.Backend.Pregnancies;
using CoC.Backend.Strings;
using WeakEvent;

namespace CoC.Backend.BodyParts
{


	public sealed partial class GenitalTattooLocation : TattooLocation
	{

		private static readonly List<GenitalTattooLocation> _allLocations = new List<GenitalTattooLocation>();

		public static readonly ReadOnlyCollection<GenitalTattooLocation> allLocations;

		private readonly byte index;

		static GenitalTattooLocation()
		{
			allLocations = new ReadOnlyCollection<GenitalTattooLocation>(_allLocations);
		}

		private GenitalTattooLocation(byte index, TattooSizeLimit limitSize, SimpleDescriptor btnText, SimpleDescriptor locationDesc) : base(limitSize, btnText, locationDesc)
		{
			this.index = index;
		}

		public static GenitalTattooLocation LEFT_CHEST = new GenitalTattooLocation(0, MediumTattoosOrSmaller, LeftChestButton, LeftChestLocation);
		public static GenitalTattooLocation LEFT_BREAST = new GenitalTattooLocation(1, MediumTattoosOrSmaller, LeftBreastButton, LeftBreastLocation);
		public static GenitalTattooLocation LEFT_UNDER_BREAST = new GenitalTattooLocation(2, SmallTattoosOnly, LeftUnderBreastButton, LeftUnderBreastLocation);
		public static GenitalTattooLocation LEFT_NIPPLE_TEAT = new GenitalTattooLocation(3, SmallTattoosOnly, LeftNippleButton, LeftNippleLocation);

		public static GenitalTattooLocation RIGHT_CHEST = new GenitalTattooLocation(4, MediumTattoosOrSmaller, RightChestButton, RightChestLocation);
		public static GenitalTattooLocation RIGHT_BREAST = new GenitalTattooLocation(5, MediumTattoosOrSmaller, RightBreastButton, RightBreastLocation);
		public static GenitalTattooLocation RIGHT_UNDER_BREAST = new GenitalTattooLocation(6, SmallTattoosOnly, RightUnderBreastButton, RightUnderBreastLocation);
		public static GenitalTattooLocation RIGHT_NIPPLE_TEAT = new GenitalTattooLocation(7, SmallTattoosOnly, RightNippleButton, RightNippleLocation);

		public static GenitalTattooLocation CHEST = new GenitalTattooLocation(8, MediumTattoosOrSmaller, ChestButton, ChestLocation);
		public static GenitalTattooLocation GROIN = new GenitalTattooLocation(9, MediumTattoosOrSmaller, GroinButton, GroinLocation);
		public static GenitalTattooLocation ALL_AVAILABLE_COCKS = new GenitalTattooLocation(10, FullPartTattoo, CockButton, CockLocation);
		public static GenitalTattooLocation ALL_AVAILABLE_VULVA = new GenitalTattooLocation(11, MediumTattoosOrSmaller, VulvaButton, VulvaLocation);
		public static GenitalTattooLocation ASS = new GenitalTattooLocation(12, SmallTattoosOnly, AssButton, AssLocation);
		public static GenitalTattooLocation FULL_GENITALS = new GenitalTattooLocation(13, FullPartTattoo, FullButton, FullLocation);

		public static bool LocationsCompatible(GenitalTattooLocation first, GenitalTattooLocation second)
		{
			//chest and left chest are incompatible.
			//chest and right chest are incompatible.
			//left and right chest are compatible.
			//the remainder of these are compatible.run these checks accordingly.

			//if one is left chest.
			if (first == CHEST || second == CHEST)
			{
				//check to see if other is left left or left right chest.
				GenitalTattooLocation other = (first == CHEST) ? second : first;
				return other != LEFT_CHEST && other != RIGHT_CHEST;
			}
			//otherwise, we're good.s
			else
			{
				return true;
			}
		}
	}

	public sealed class GenitalTattoo : TattooablePart<GenitalTattooLocation>
	{
		public GenitalTattoo(IBodyPart source, CreatureStr allTattoosShort, CreatureStr allTattoosLong) : base(source, allTattoosShort, allTattoosLong)
		{
		}

		public override int MaxTattoos => GenitalTattooLocation.allLocations.Count;

		public override IEnumerable<GenitalTattooLocation> availableLocations => GenitalTattooLocation.allLocations;

		public override bool LocationsCompatible(GenitalTattooLocation first, GenitalTattooLocation second) => GenitalTattooLocation.LocationsCompatible(first, second);
	}

	//I had the relatively brilliant idea of storing the GameTime for when something happened - last milking, etc. it means we don't need to update it every hour - we just need to update it when it happens.
	//it's a relatively simple calculation to get the current hours since - we just diff GameTime now and stored time. Granted, it's now a byte and int instead of just one int, but w/e. I'll trade
	//an extra byte of memory (or 4 bytes if C# does the whole align things to word boundaries, idk on its optimizations) for less maintenance and less cycles managing the data.

#warning Make sure to raise events for gender change and pass it along to femininity. Then implement all the various data changes. theres a ton of them.

	//Genitals is the new master class for all things related to sexual endowments and whatever they entail. That said, it's broken into several sub-classes because
	//it's hard to manage otherwise, and i was going insane. Genitals is primarily responsible for taking each of these subclasses and making them interact with one another
	//when the situation requires. for example, any vaginal sex will deal with the cock (or object) that is doing the penetrating, the vagina being penentrated, and the womb
	//if not pregnant and pregnancy is possible. Another example is gender - altering gender will affect breast, cock, and vagina collections. Some appearance related checks will
	//also factor in femininity, not just endowments. Basically, there's a lot of shit that needs all of this to play nice, so this is where we make that all happen.

	//90% of this class basically aliases the various body parts. it's not ideal, but it's a decent compromise so you don't have to go 4 levels in just to get the largest breast row.
	//Some things, however, are not. these are defined here:

	//Sex related functionality is primarily handled here, though there are some body parts not included here that are used sexually (oral, for example). Generally speaking,
	//we don't


	public sealed partial class Genitals : SimpleSaveablePart<Genitals, GenitalsData>, IBodyPartTimeLazy //for now all the stuff it contains is lazy, so that's all we need.
	{
		#region Vagina Related Constants
		//Not gonna lie, supporting double twats is a huge pain in the ass. (PHRASING! BOOM!)
		public const int MAX_VAGINAS = VaginaCollection.MAX_VAGINAS;
		#endregion

		#region Breast Constants
		//max in game that i can find is 5, but they only ever use 4 rows.
		//apparently Fenoxo said 3 rows, but then after it went open, some shit got 4 rows.
		//i'm not being a dick and reverting that. 4 it is.
		public const int MAX_BREAST_ROWS = BreastCollection.MAX_BREAST_ROWS;
		#endregion

		#region Lactation Related Constants

		public const double MIN_LACTATION_MODIFIER = BreastCollection.MIN_LACTATION_MODIFIER;
		public const double LACTATION_THRESHOLD = BreastCollection.LACTATION_THRESHOLD; //below this: not lactating. above this: lactating.
		public const double MODERATE_LACTATION_THRESHOLD = BreastCollection.MODERATE_LACTATION_THRESHOLD;
		public const double STRONG_LACTATION_THRESHOLD = BreastCollection.STRONG_LACTATION_THRESHOLD;
		public const double HEAVY_LACTATION_THRESHOLD = BreastCollection.HEAVY_LACTATION_THRESHOLD;
		public const double EPIC_LACTATION_THRESHOLD = BreastCollection.EPIC_LACTATION_THRESHOLD;
		public const double MAX_LACTATION_MODIFIER = BreastCollection.MAX_LACTATION_MODIFIER;

		#endregion

		#region Cock Related Constants
		public const int MAX_COCKS = CockCollection.MAX_COCKS;

		public const ushort CUM_MULTIPLIER_CAP = CockCollection.CUM_MULTIPLIER_CAP;

		public double minimumCockLength => perkData.MinCockLength;
		#endregion

		#region Public ReadOnly Members

		public readonly Ass ass;
		public readonly Balls balls;
		public readonly Femininity femininity; //make sure to cap this if not androgynous perk.

		//fertility gets a class because it's not just an int, it also has a bool that determines if the creature is artificially infertile
		//(sand witch pill, contraceptives, whatever.)
		public readonly Fertility fertility;

		public readonly Womb womb;

		public readonly BreastCollection allBreasts;
		public readonly CockCollection allCocks;
		public readonly VaginaCollection allVaginas;

		public readonly GenitalTattoo tattoos;
		#endregion

		#region NonPublic ReadOnly Members

		internal readonly GenitalPerkData perkData;

		#endregion

		#region Private ReadOnly Members

		//creator let's me do delayed init for perks and such.
		private readonly BreastCreator[] breastCreators;
		private readonly CockCreator[] cockCreators;
		private readonly VaginaCreator[] vaginaCreators;

		//using list, because it's easier to keep track of count when it does it for you. array would work, but it has the problem of counting nulls, and keeping track of that manually is tedious.

		#endregion

		#region Public Derived/Helper Properties
		public ReadOnlyCollection<Breasts> breastRows => allBreasts.breastRows;

		public ReadOnlyCollection<Cock> cocks => allCocks.cocks;

		public ReadOnlyCollection<Vagina> vaginas => allVaginas.vaginas;

		public Clit[] clits => vaginas.Select(x => x.clit).ToArray();

		public int numClits => numVaginas;
		#endregion

		#region Private Derived/Helper Properties
		private Creature creature => CreatureStore.GetCreatureClean(creatureID);
		#endregion

		#region Common Derived/Helper Properties

		//public int numCocksOrClitCocks => _cocks.Count == 0 ? (hasClitCock ? 1 : 0) : _cocks.Count;

		internal double relativeLust => creature?.relativeLust ?? Creature.DEFAULT_LUST;

		#endregion

		#region Ass Aliases
		//ass is readonly, always exists. we don't need any alias magic for it.

		public uint analSexCount => ass.totalSexCount;
		public uint analPenetrationCount => ass.totalPenetrateCount;
		public uint analOrgasmCount => ass.orgasmCount;
		public uint analDryOrgasmCount => ass.dryOrgasmCount;

		public ushort standardBonusAnalCapacity => ass.bonusAnalCapacity;

		public AnalLooseness minAnalLooseness => ass.minLooseness;
		public AnalLooseness maxAnalLooseness => ass.maxLooseness;

		public AnalWetness minAnalWetness => ass.minWetness;
		public AnalWetness maxAnalWetness => ass.maxWetness;

		#endregion

		#region Balls Aliases

		public bool hasBalls => balls.hasBalls;
		public bool uniBall => balls.uniBall;

		public byte numberOfBalls => balls.count;
		public byte ballSize => balls.size;

		#endregion

		#region CockCollection Aliases

		public bool hasCock => allCocks.hasCock;

		public int numCocks => allCocks.numCocks;

		public uint timesCocksSounded => allCocks.totalSoundCount;
		public uint timesHadSexWithCock => allCocks.totalSexCount;
		public uint timesHasSelfSexWithCock => allCocks.selfSexCount;

		public uint totalCockOrgasmCount => allCocks.totalOrgasmCount;
		public uint totalCockDryOrgasmCount => allCocks.dryOrgasmCount;

		public bool cockVirgin => allCocks.cockVirgin;

		public bool hasSheath => allCocks.hasSheath;

		#region Public Cum Related Members
		public double cumMultiplierTrue => allCocks.cumMultiplierTrue;

		public ushort additionalCum => allCocks.additionalCum;

		public double additionalCumTrue => allCocks.additionalCumTrue;

		#endregion
		#region Public Cum Related Computed Values
		public ushort cumMultiplier => allCocks.cumMultiplier;

		public int hoursSinceLastCum => allCocks.hoursSinceLastCum;
		//includes any pent up bonuses caused by various items, effects, etc.
		public int simulatedHoursSinceLastCum => allCocks.simulatedHoursSinceLastCum;

		//hours since last cum is now constant - it stores the last time you actually came (or the beginning of the game if you never came). To get around this, we have a pent-up bonus
		//which stacks like the old hours since last came
		public uint pentUpBonus => allCocks.pentUpBonus;

		public int totalCum => allCocks.totalCum;


		#endregion


		#endregion

		#region BreastCollection Aliases

		#region Public Nipple Related Members
		public bool hasBlackNipples => allBreasts.hasBlackNipples;

		public bool hasQuadNipples => allBreasts.hasQuadNipples;

		public NippleStatus nippleType => allBreasts.nippleStatus;

		public double nippleLength => allBreasts.nippleLength;

		public bool dickNipplesEnabled => allBreasts.dickNipplesEnabled;


		#endregion

		#region Public Lactation Related Members

		public double lactation_TotalCapacityMultiplier => allBreasts.lactation_TotalCapacityMultiplier;
		public double lactation_CapacityMultiplier => allBreasts.lactation_CapacityMultiplier;
		public double lactationProductionModifier => allBreasts.lactationProductionModifier;

		public uint overfullBuffer => allBreasts.overfullBuffer;
		public double currentLactationAmount => allBreasts.currentLactationAmount;

		public double lactationAmountPerBreast => allBreasts.lactationAmountPerBreast;
		#endregion

		#region Public Breast Related Computed Values
		public int numBreastRows => allBreasts.numBreastRows;

		public uint timesTitFucked => allBreasts.totalTitFuckCount;
		public uint timesSelfTitFucked => allBreasts.selfTitFuckCount;

		public uint timesTitsOrgasmed => allBreasts.breastOrgasmCount;
		public uint timesTitsDryOrgasmed => allBreasts.breastDryOrgasmCount;

		public CupSize smallestPossibleMaleCupSize => allBreasts.smallestPossibleMaleCupSize;
		public CupSize smallestPossibleFemaleCupSize => allBreasts.smallestPossibleFemaleCupSize;

		public CupSize smallestPossibleCupSize => allBreasts.smallestPossibleCupSize;

		#endregion

		#region Public Nipple Related Computed Properties

		public int nippleCount => allBreasts.nippleCount;

		public uint timesNippleFucked => allBreasts.totalFuckableNippleSexCount;
		public uint timesSelfNippleFucked => allBreasts.selfFuckableNippleSexCount;
		public uint totalDickNippleSexCount => allBreasts.totalDickNippleSexCount;

		#endregion

		#region Public Lactation Related Computed Values
		public bool canLessenCurrentLactationLevels => allBreasts.canLessenCurrentLactationLevels;

		public int hoursSinceLastMilked => allBreasts.hoursSinceLastMilked;

		public bool isOverfull => allBreasts.isOverfull;

		public int hoursOverfull => allBreasts.hoursOverfull;

		public double maximumLactationCapacity => allBreasts.maximumLactationCapacity;
		public double currentLactationCapacity => allBreasts.currentLactationCapacity;

		public double lactationRate => allBreasts.lactationRate;

		public LactationStatus lactationStatus => allBreasts.lactationStatus;

		public bool isLactating => allBreasts.isLactating;
		#endregion

		#endregion

		#region VaginaCollection Aliases

		public ushort standardBonusVaginalCapacity => allVaginas.standardBonusCapacity;
		public ushort perkBonusVaginalCapacity => allVaginas.perkBonusCapacity;
		public ushort totalBonusVaginalCapacity => allVaginas.totalBonusCapacity;

		#region Public Vagina Related Computed Values


		public int numVaginas => allVaginas.numVaginas;

		public uint totalVaginasSexCount => allVaginas.totalSexCount;
		public uint totalVaginasSelfSexCount => allVaginas.selfSexCount;
		public uint totalVaginalPenetrationCount => allVaginas.totalPenetratedCount;
		public uint totalVaginalSelfPenetrationCount => allVaginas.selfPenetratedCount;

		public uint totalVaginalExternalStimulationCount => allVaginas.totalNonPenetratedCount;
		public uint totalVaginalSelfExternaSimulationCount => allVaginas.selfNonPenetratedCount;

		public uint totalVaginalOrgasmCount => allVaginas.totalOrgasmCount;
		public uint totalVaginalDryOrgasmCount => allVaginas.totalDryOrgasmCount;
		public uint totalVaginalBirthCount => allVaginas.totalBirthCount;

		public VaginalLooseness minVaginalLooseness => allVaginas.minVaginalLooseness;
		public VaginalLooseness maxVaginalLooseness => allVaginas.maxVaginalLooseness;

		public VaginalWetness minVaginalWetness => allVaginas.minVaginalWetness;
		public VaginalWetness maxVaginalWetness => allVaginas.maxVaginalWetness;

		#endregion

		#region Public Clit Related Computed Values
		public uint totalClitUsedAsPenetratorCount => allVaginas.clitUsedAsPenetratorCount;
		public uint totalClitUsedAsPenetratorOnSelfCount => allVaginas.clitUsedAsPenetratorOnSelfCount;
		#endregion

		#endregion

		#region Pregnancy Related Computed Properties

		public bool isPregnant => womb.isPregnant;

		public double knockupRate(byte bonusVirility = 0) => (fertility.currentFertility + bonusVirility) / 100f;
		#endregion

		#region Gender Related Properties
		public Gender gender
		{
			get
			{
				Gender retVal = Gender.GENDERLESS;
				retVal |= numVaginas > 0 ? Gender.FEMALE : Gender.GENDERLESS;
				retVal |= numCocks > 0 ? Gender.MALE : Gender.GENDERLESS;
				return retVal;
			}
		}


		/* Trap check. This combines the feminity value and the physical endowments to determine how this creature appears.
		 *
		 * Start with the most obvious
		 * - if the creature very obviously has both sets of endowments (C-Cup+ breasts and 6in+ cock) : herm
		 * - if the creature has obvious female endowments, no obvious male endowments, and appears to be androgynous or female: female
		 * - if the creature has obvious male endowments, no obvious female endowments, and appears to be androgynous or male: male
		 *
		 * Now on to the trap checks.
		 * - if the creature has one set endowments but their femininity/masculinity is very distinctly that of the other gender: herm.
		 * - if the player has no obvious endowments either way, but appears female : female
		 * - if the player has no obvious endowments either way, but appears male : male
		 * - if the player has no obvious endowments either way and looks the part : genderless.
		 *
		 * How you deal with this is up to you, especially when dealing with herm and genderless. A binary version of this exists for male/female if you want it to force a decision
		 *
		 */
		public Gender ApparentGender()
		{
			//noticable bulge and breasts. Note
			if (BiggestCupSize() > CupSize.B && BiggestCockTotalSize() >= 10)
			{
				return Gender.HERM;
			}
			//noticable breasts and sufficiently female
			else if (BiggestCupSize() > CupSize.B && !femininity.atLeastSlightlyMasculine)
			{
				return Gender.FEMALE;
			}
			//noticable dick and sufficiently male
			else if (BiggestCockTotalSize() >= 10 && !femininity.atLeastSlightlyFeminine)
			{
				return Gender.MALE;
			}
			//not noticable assets - go by appearance
			else if (BiggestCockTotalSize() < 6 && BiggestCupSize() <= CupSize.B)
			{
				if (femininity.atLeastSlightlyFeminine)
				{
					return Gender.FEMALE;
				}
				else if (femininity.atLeastSlightlyMasculine)
				{
					return Gender.MALE;
				}

				return Gender.GENDERLESS;
			}
			//noticable breasts or dick, but too masculine or feminine.
			return Gender.HERM;
		}

		//Variant of the apparent gender that limits the results to male/female. This is done in the following order of preference: significant bust size, significant cock size,
		//overly feminine, somewhat masculine. If we still don't have a result, the result is determined by whether or not we have a cock and/or any bust at all.
		//this is functionally equivalent to the old mf function in vanilla, but with a bool result. this means i don't need to see mf("m", "f") == "m" because that's dumb.
		public bool AppearsMoreMaleThanFemale()
		{
			Gender trapGender = ApparentGender();
			//easy checks - appears mostly male
			if (trapGender == Gender.MALE)
			{
				return true;
			}
			//appears mostly female.
			else if (trapGender == Gender.FEMALE)
			{
				return false;
			}
			//appears either herm or genderless.
			else
			{
				CupSize LargestCup = BiggestCupSize();
				double longestCock = BiggestCockTotalSize();

				//at this point, we are either herm or genderless appearing. there are 3 cases for herm: truly apparent breasts and cock bulge, appears male but with breasts,
				//and appears female but with a rather obvious dick bulge. genderless is everything else. Originally this was a lot more concise, but this way of writing it is
				//way more verbose; it tells you exactly what it's checking for. personally, i actually liked it being concise, but had no clue why those values were chosen.
				//written this way, each choice is explained and the end result is the same.

				//breasts get hightest priority - if rather large bust, we treat as female, even if it also has a large dick-bulge. This is because herms are treated as female
				//in this game.
				if (LargestCup > CupSize.B)
				{
					return false;
				}
				//if we're here, we don't have a large bust. at this point, dick takes priority. if it has a large bulge, treat as male.
				else if (longestCock >= 6)
				{
					return true;
				}
				//if we're here, we don't have a large bust or a large bulge. thus, we can only go by femininity.

				//if overly female, return false.
				else if (femininity >= 75)
				{
					return false;
				}
				//if at least slightly male, return true.
				else if (femininity < 45)
				{
					return true;
				}
				//if we've fallen through to this point, we have a small bust, small or no cock, and a relatively androgynous build that may be approaching feminine at most.

				//first, check if we have any bulge and any bust
				else if (LargestCup > CupSize.FLAT && cocks.Count > 0)
				{
					//skew toward male slightly.
					return femininity <= 55;
				}
				//failing that, see if we have any bust. if we do, female. if we don't, male.
				else
				{
					return LargestCup == CupSize.FLAT;
				}
			}

		}

		#endregion

		//despite my attempts to remove status effects wherever possible, i'm not crazy. Heat/Rut/Dsyfunction seem like ideal status effects.
		//in that they are temporary effects. as such, i'm not putting them here.



		internal void CheckGenderChanged(Gender oldGender)
		{
			if (gender != oldGender)
			{
				genderChangedHandler.Raise(this, new GenderChangedEventArgs(oldGender, gender));
			}
		}

		#region Constructors


		internal Genitals(Guid creatureID, Gender initialGender, Womb womb) : base(creatureID)
		{
			ass = new Ass(creatureID);
			balls = new Balls(creatureID, initialGender);

			if (initialGender.HasFlag(Gender.MALE))
			{
				cockCreators = new CockCreator[1] { new CockCreator() };
			}
			else
			{
				cockCreators = new CockCreator[0];
			}
			if (initialGender.HasFlag(Gender.FEMALE))
			{
				vaginaCreators = new VaginaCreator[1] { new VaginaCreator() };
			}
			else
			{
				vaginaCreators = new VaginaCreator[0];
			}
			breastCreators = new BreastCreator[1] { new BreastCreator(initialGender.HasFlag(Gender.FEMALE) ? CupSize.C : CupSize.FLAT) };

			allBreasts = new BreastCollection(this);
			allCocks = new CockCollection(this);
			allVaginas = new VaginaCollection(this);


			femininity = new Femininity(creatureID, initialGender);
			fertility = new Fertility(creatureID, initialGender);

			this.womb = womb ?? throw new ArgumentNullException(nameof(womb));

			tattoos = new GenitalTattoo(this, AllTattoosShort, AllTattoosLong);
		}

		internal Genitals(Guid creatureID, Ass ass, BreastCreator[] breasts, CockCreator[] cocks, Balls balls, VaginaCreator[] vaginas, Womb womb, byte? femininity,
			Fertility fertility) : base(creatureID)
		{
			this.ass = ass ?? throw new ArgumentNullException(nameof(ass));
			breastCreators = breasts?.Where(x => x != null).ToArray() ?? throw new ArgumentNullException(nameof(breastCreators));
			this.balls = balls ?? throw new ArgumentNullException(nameof(balls));
			cockCreators = cocks?.Where(x => x != null).ToArray();
			vaginaCreators = vaginas?.Where(x => x != null).ToArray();

			Gender computedGender = Gender.GENDERLESS;
			if (cockCreators?.Length > 0)
			{
				computedGender |= Gender.MALE;
			}
			if (vaginaCreators?.Length > 0)
			{
				computedGender |= Gender.FEMALE;
			}
			if (breastCreators.Length == 0)
			{
				breastCreators = new BreastCreator[1] { new BreastCreator(computedGender.HasFlag(Gender.FEMALE) ? CupSize.C : CupSize.FLAT) };
			}

			allBreasts = new BreastCollection(this);
			allCocks = new CockCollection(this);
			allVaginas = new VaginaCollection(this);

			this.femininity = femininity != null ? new Femininity(creatureID, computedGender, (byte)femininity) : new Femininity(creatureID, computedGender);
			this.fertility = fertility ?? throw new ArgumentNullException(nameof(fertility));

			this.womb = womb ?? throw new ArgumentNullException(nameof(womb));

			tattoos = new GenitalTattoo(this, AllTattoosShort, AllTattoosLong);
		}

#warning make sure this is up to date when the genitals are finally finished.
		//consider thinking up a nicer way of pulling all that perk data in so i don't have to do deal with making sure all the perk values are correctly updated.
		//can't just reference them b/c they won't notify the source when they change; this will. It also prevents the values from updating these things so they have incorrect values.
		//Atm: i both reference and store the data.

		//Thought: Require the perk data class in the copy constructor, or pull it from the other. when the data is copied, simply pull the values from the perk data.
		//ALSO: convert everything to just store the data. wire up the perks so that each item gets its own update callback. when the perk value is updated,

		protected internal override void PostPerkInit()
		{
			ass.PostPerkInit();
			balls.PostPerkInit();

			allBreasts.Initialize(breastCreators);
			allCocks.Initialize(cockCreators);
			allVaginas.Initialize(vaginaCreators);

			femininity.PostPerkInit();
			fertility.PostPerkInit();

			womb.PostPerkInit();
		}

		protected internal override void LateInit()
		{
			ass.LateInit();
			balls.LateInit();

			cocks.ForEach(x => x.LateInit());
			vaginas.ForEach(x => x.LateInit());
			breastRows.ForEach(x => x.LateInit());

			femininity.LateInit();
			fertility.LateInit();

			womb.LateInit();
		}

		public override string BodyPartName() => Name();

		internal void Milked()
		{ }

		private readonly WeakEventSource<GenderChangedEventArgs> genderChangedHandler = new WeakEventSource<GenderChangedEventArgs>();

		public event EventHandler<GenderChangedEventArgs> onGenderChanged
		{
			add => genderChangedHandler.Subscribe(value);
			remove => genderChangedHandler.Unsubscribe(value);
		}

		private void NotifyGenderChanged(Gender oldGender)
		{
			genderChangedHandler.Raise(this, new GenderChangedEventArgs(oldGender, gender));
		}

		#endregion

		public override GenitalsData AsReadOnlyData()
		{
			return new GenitalsData(this);
		}

		public override bool IsIdenticalTo(GenitalsData original, bool ignoreSexualMetaData)
		{
			return allBreasts.IsIdenticalTo(original.allBreastData, ignoreSexualMetaData) && allCocks.IsIdenticalTo(original.allCockData, ignoreSexualMetaData)
				&& allVaginas.IsIdenticalTo(original.allVaginaData, ignoreSexualMetaData) && femininity.IsIdenticalTo(original.femininity, ignoreSexualMetaData)
				&& fertility.IsIdenticalTo(original.fertility, ignoreSexualMetaData) && ass.IsIdenticalTo(original.ass, ignoreSexualMetaData)
				&& balls.IsIdenticalTo(original.balls) && tattoos.IsIdenticalTo(original.tattoos) && womb.IsIdenticalTo(original.womb);
		}

		#region Genital Exclusive
		internal bool MakeFemale()
		{
			if (numCocks == 0 && numVaginas > 0)
			{
				return false;
			}
			RemoveCock(numCocks);
			if (numVaginas == 0)
			{
				AddVagina(VaginaType.HUMAN);
			}
			return true;
		}

		internal bool MakeMale()
		{
			if (numVaginas == 0 && numBreastRows == 1 && breastRows[0].isMale && numCocks > 0)
			{
				return false;
			}
			RemoveVagina(numVaginas);
			RemoveBreastRows(numBreastRows);
			if (numCocks == 0)
			{
				AddCock(CockType.HUMAN);
			}
			return true;
		}
		#endregion
		#region Validation
		internal override bool Validate(bool correctInvalidWrapper)
		{
#warning correct invalid game date times for last cock cum, milk full, last orgasm.
			if (correctInvalidWrapper)
			{
				return ass.Validate(correctInvalidWrapper) & allBreasts.Validate(correctInvalidWrapper) & allCocks.Validate(correctInvalidWrapper) &
					allVaginas.Validate(correctInvalidWrapper) & femininity.Validate(correctInvalidWrapper) & fertility.Validate(correctInvalidWrapper)
					& womb.Validate(correctInvalidWrapper) & balls.Validate(correctInvalidWrapper);
			}
			else
			{
				return ass.Validate(correctInvalidWrapper) && allBreasts.Validate(correctInvalidWrapper) && allCocks.Validate(correctInvalidWrapper) &&
					allVaginas.Validate(correctInvalidWrapper) && femininity.Validate(correctInvalidWrapper) && fertility.Validate(correctInvalidWrapper)
					&& womb.Validate(correctInvalidWrapper) && balls.Validate(correctInvalidWrapper);
			}


		}

		#endregion

		#region Femininity
		public byte IncreaseFemininity(byte amount)
		{
			return femininity.IncreaseFemininity(amount);
		}

		public byte InreaseMasculinity(byte amount)
		{
			return femininity.IncreaseMasculinity(amount);
		}

		public byte SetFemininity(byte newValue)
		{
			return femininity.SetFemininity(newValue);
		}
		#endregion
		#region TimeListeners

		string IBodyPartTimeLazy.reactToTimePassing(bool isPlayer, byte hoursPassed)
		{
			StringBuilder outputBuilder = new StringBuilder();
			string outputHelper;
			//i have no clue how this would work for multi-snatch configs.
			foreach (Vagina vagina in vaginas)
			{
				if (DoLazy(vagina, isPlayer, hoursPassed, out outputHelper))
				{
					outputBuilder.Append(outputHelper);
				}
			}

			if (DoLazy(ass, isPlayer, hoursPassed, out outputHelper))
			{
				outputBuilder.Append(outputHelper);
			}

			if (DoLazy(femininity, isPlayer, hoursPassed, out outputHelper))
			{
				outputBuilder.Append(outputHelper);
			}

			if (allBreasts.DoLazyLactationCheck(isPlayer, hoursPassed, out outputHelper))
			{
				outputBuilder.Append(outputHelper);
			}

			if (DoLazy(fertility, isPlayer, hoursPassed, out outputHelper))
			{
				outputBuilder.Append(outputHelper);
			}

			return outputBuilder.ToString();

		}

		private bool DoLazy(IBodyPartTimeLazy member, bool isPlayer, byte hoursPassed, out string output)
		{
			output = member.reactToTimePassing(isPlayer, hoursPassed);
			return !string.IsNullOrEmpty(output);
		}

		#endregion

		//Genitals Exclusive

		#region Non-Data Cock Aliases

		public bool LostSheath(CockData previousCockData) => allCocks.LostSheath(previousCockData);


		public bool GainedSheath(CockData previousCockData) => allCocks.GainedSheath(previousCockData);


		public bool HasSheathChanged(CockData previousCockData) => allCocks.HasSheathChanged(previousCockData);

		#endregion

		#region Ass Aliases
		//not really necessary, mostly redundant, but whatever.
		public ushort IncreaseBonusAnalCapacity(ushort amountToAdd) => ass.IncreaseBonusCapacity(amountToAdd);

		public ushort DecreaseBonusAnalCapacity(ushort amountToRemove) => ass.DecreaseBonusCapacity(amountToRemove);
		#endregion

		#region Add/Remove Breasts

		public bool AddBreastRow() => allBreasts.AddBreastRow();

		public bool AddBreastRowAverage() => allBreasts.AddBreastRowAverage();


		public bool AddBreastRow(CupSize cup) => allBreasts.AddBreastRow(cup);


		public int RemoveBreastRows(int count = 1) => allBreasts.RemoveBreastRows(count);


		public int RemoveExtraBreastRows() => allBreasts.RemoveExtraBreastRows();


		#endregion

		#region Update All Breasts Functions
		public bool NormalizeBreasts(bool untilEven = false) => allBreasts.NormalizeBreasts(untilEven);



		public bool AnthropomorphizeBreasts(bool untilEven = false) => allBreasts.AnthropomorphizeBreasts(untilEven);


		#endregion

		#region Nipple Mutators
		public bool SetNippleStatus(NippleStatus desiredStatus, bool limitToCurrentLength = false, bool toggleDickNippleFlagIfNeccesary = false) =>
			allBreasts.SetNippleStatus(desiredStatus, limitToCurrentLength, toggleDickNippleFlagIfNeccesary);

		public bool SetQuadNipples(bool active) => allBreasts.SetQuadNipples(active);

		public bool SetBlackNipples(bool active) => allBreasts.SetBlackNipples(active);

		public bool UnlockDickNipples() => allBreasts.UnlockDickNipples();

		public bool PreventDickNipples() => allBreasts.PreventDickNipples();

		public bool SetDickNippleFlag(bool enabled) => allBreasts.SetDickNippleFlag(enabled);

		public double GrowNipples(double amount, bool ignorePerks = false) => allBreasts.GrowNipples(amount, ignorePerks);

		public double ShrinkNipples(double amount, bool ignorePerks = false) => allBreasts.ShrinkNipples(amount, ignorePerks);

		public double ChangeNippleLength(double delta, bool ignorePerks = false) => allBreasts.ChangeNippleLength(delta, ignorePerks);

		public bool SetNippleLength(double size) => allBreasts.SetNippleLength(size);

		#endregion

		#region Lactation Update Functions
		public bool SetLactationTo(LactationStatus newStatus) => allBreasts.SetLactationTo(newStatus);


		public bool StartLactating() => allBreasts.StartLactating();
		public bool ClearLactation() => allBreasts.ClearLactation();


		public double BoostLactation(double byAmount = 0.1f) => allBreasts.BoostLactation(byAmount);


		public void StartOrBoostLactation() => allBreasts.StartOrBoostLactation();

		#endregion

		#region Add/Remove Balls

		/// <summary>
		/// Tries to grow a pair of balls, failing if the creature already has balls.
		/// </summary>
		/// <returns>True if the creature gained a pair of balls, false if they already had them.</returns>
		public bool GrowBalls()
		{
			return balls.GrowBalls();
		}

		/// <summary>
		/// Tries to grow the number of balls provided, rounded down to the nearest even number, at the given size, if applicable. This will create a minimum of two balls if successful.
		/// Fails if the target already has balls of any kind.
		/// </summary>
		/// <param name="numberOfBalls"></param>
		/// <param name="ballSize"></param>
		/// <returns>true if the target didn't previously have balls and now does, false otherwise.</returns>
		/// <remarks>this function will never create a Uniball. If this is desired, use either GrowUniBall or GrowBallsAny.</remarks>
		public bool GrowBalls(byte numberOfBalls, byte ballSize = Balls.DEFAULT_BALLS_SIZE)
		{
			return balls.GrowBalls(numberOfBalls, ballSize);
		}

		/// <summary>
		/// Tries to grow a uniball. Fails if the target already has balls.
		/// </summary>
		/// <returns>True if the target did not have balls and now has a uniball, false otherwise.</returns>
		public bool GrowUniBall()
		{
			return balls.GrowUniBall();
		}

		/// <summary>
		/// Tries to grow the number of balls provided, at the given size, if applicable and possible.
		/// </summary>
		/// <param name="numBalls"></param>
		/// <param name="newSize"></param>
		/// <returns></returns>
		/// <remarks>Any odd number of balls provided that is not 1 will be rounded down to the nearest even number. </remarks>
		public bool GrowBallsAny(byte numBalls, byte newSize = Balls.DEFAULT_BALLS_SIZE)
		{
			if (numBalls == 1)
			{
				return balls.GrowUniBall();
			}
			else
			{
				return balls.GrowBalls(numBalls, newSize);
			}
		}

		/// <summary>
		/// Grows the given amount of balls, even if the target currently does not have any balls. if they do, the two are added, then rounded down to the nearest even number.
		/// The total amount grown is returned.
		/// </summary>
		/// <param name="ballsToAdd">The number of balls to add.</param>
		/// <returns>The number of balls successfully added.</returns>
		/// <remarks> The number of balls a target can have is capped; The return value will differ from the given value if this cap is reached.</remarks>
		public byte AddBalls(byte ballsToAdd, bool ignoreIfUniball = false)
		{
			if (balls.uniBall && !ignoreIfUniball)
			{
				return 0;
			}
			else
			{
				return balls.AddBalls(ballsToAdd);
			}
		}

		/// <summary>
		/// Adds the given total of balls to the current amount. If the target does not have balls or the target has a uniball and the optional ignore if uniball flag is set,
		/// this will fail to add any balls. Returns the number of balls added.
		/// </summary>
		/// <param name="additionalBalls">double of balls to add.</param>
		/// <param name="ignoreIfUniball">Should this function respect a uniball, if applicable?</param>
		/// <returns>The number of balls successfully added.</returns>
		/// <remarks> The number of balls a target can have is capped; The return value will differ from the given value if this cap is reached.</remarks>
		public byte AddAdditionalBalls(byte additionalBalls, bool ignoreIfUniball = false)
		{
			if (!hasBalls || (balls.uniBall && !ignoreIfUniball))
			{
				return 0;
			}
			else
			{
				return balls.AddBalls(additionalBalls);
			}
		}

		public byte RemoveBalls(byte removeAmount)
		{
			return balls.RemoveBalls(removeAmount);
		}

		public byte RemoveExtraBalls()
		{
			return balls.RemoveExtraBalls();
		}

		public bool RemoveAllBalls()
		{
			return balls.RemoveAllBalls();
		}
		#endregion

		#region Convert Ball Type
		public bool ConvertToNormalBalls()
		{
			return balls.MakeStandard();
		}

		public bool ConvertToUniball()
		{
			return balls.MakeUniBall();
		}

		#endregion

		#region Grow or Convert Balls
		public bool GrowOrConvertToUniball()
		{
			if (hasBalls)
			{
				return ConvertToUniball();
			}
			else
			{
				return GrowUniBall();
			}
		}

		public bool GrowOrConvertToNormalBalls()
		{
			if (hasBalls)
			{
				return ConvertToNormalBalls();
			}
			else
			{
				return GrowBalls();
			}
		}
		#endregion

		#region Change Balls Data

		public byte EnlargeBalls(byte enlargeAmount, bool respectUniball = false, bool ignorePerks = false)
		{
			if (!hasBalls || (balls.uniBall && respectUniball))
			{
				return 0;
			}
			else
			{
				return balls.EnlargeBalls(enlargeAmount, ignorePerks);
			}
		}

		public byte ShrinkBalls(byte shrinkAmount, bool ignorePerks = false)
		{
			if (!hasBalls)
			{
				return 0;
			}
			else
			{
				return balls.ShrinkBalls(shrinkAmount, ignorePerks);
			}
		}

		#endregion

		#region Add/Remove Cocks
		public bool AddCock(CockType newCockType) => allCocks.AddCock(newCockType);


		public bool AddCock(CockType newCockType, double length, double girth, double? knotMultiplier = null) => allCocks.AddCock(newCockType, length, girth, knotMultiplier);


		public string AddedCockText(CockData addedCock) => allCocks.AddedCockText(addedCock);


		public int RemoveCock(int count = 1) => allCocks.RemoveCock(count);

		public int RemoveCockAt(int index, int count = 1)
		{
			return allCocks.RemoveCockAt(index, count);
		}

		public bool RemoveCock(Cock cock)
		{
			return allCocks.RemoveCock(cock);
		}

		public int RemoveExtraCocks() => allCocks.RemoveExtraCocks();


		public int RemoveAllCocks() => allCocks.RemoveAllCocks();

		#endregion

		#region Update Cock Type

		public bool UpdateCock(Cock cock, CockType newType)
		{
			if (allCocks[cock.cockIndex] == cock)
			{
				return allCocks.UpdateCock(cock.cockIndex, newType);
			}
			else
			{
				return false;
			}
		}

		public bool UpdateCockWithLength(Cock cock, CockType newType, double newLength)
		{
			if (allCocks[cock.cockIndex] == cock)
			{
				return allCocks.UpdateCockWithLength(cock.cockIndex, newType, newLength);
			}
			else
			{
				return false;
			}
		}


		public bool UpdateCockWithLengthAndGirth(Cock cock, CockType newType, double newLength, double newGirth)
		{
			if (allCocks[cock.cockIndex] == cock)
			{
				return allCocks.UpdateCockWithLengthAndGirth(cock.cockIndex, newType, newLength, newGirth);
			}
			else
			{
				return false;
			}
		}


		public bool UpdateCockWithKnot(Cock cock, CockType newType, double newKnotMultiplier)
		{
			if (allCocks[cock.cockIndex] == cock)
			{
				return allCocks.UpdateCockWithKnot(cock.cockIndex, newType, newKnotMultiplier);
			}
			else
			{
				return false;
			}
		}


		public bool UpdateCockWithAll(Cock cock, CockType newType, double newLength, double newGirth, double newKnotMultiplier)
		{
			if (allCocks[cock.cockIndex] == cock)
			{
				return allCocks.UpdateCockWithAll(cock.cockIndex, newType, newLength, newGirth, newKnotMultiplier);
			}
			else
			{
				return false;
			}
		}

		public bool UpdateCock(int index, CockType newType) => allCocks.UpdateCock(index, newType);


		public bool UpdateCockWithLength(int index, CockType newType, double newLength) => allCocks.UpdateCockWithLength(index, newType, newLength);


		public bool UpdateCockWithLengthAndGirth(int index, CockType newType, double newLength, double newGirth) => allCocks.UpdateCockWithLengthAndGirth(index, newType, newLength, newGirth);


		public bool UpdateCockWithKnot(int index, CockType newType, double newKnotMultiplier) => allCocks.UpdateCockWithKnot(index, newType, newKnotMultiplier);


		public bool UpdateCockWithAll(int index, CockType newType, double newLength, double newGirth, double newKnotMultiplier) => allCocks.UpdateCockWithAll(index, newType, newLength, newGirth, newKnotMultiplier);

		public bool RestoreCock(Cock cock)
		{
			if (allCocks[cock.cockIndex] == cock)
			{
				return allCocks.RestoreCock(cock.cockIndex);
			}
			else
			{
				return false;
			}
		}
		public bool RestoreCock(int index) => allCocks.RestoreCock(index);

		#endregion

		#region Shrink Cock With Remove

		public bool ShrinkCockAndRemoveIfTooSmall(int index, double shrinkAmount, bool ignorePerks = false) => allCocks.ShrinkCockAndRemoveIfTooSmall(index, shrinkAmount, ignorePerks);
		public bool ShrinkCockAndRemoveIfTooSmall(Cock cock, double shrinkAmount, bool ignorePerks = false)
		{
			if (cock == cocks[cock.cockIndex])
			{
				return allCocks.ShrinkCockAndRemoveIfTooSmall(cock.cockIndex, shrinkAmount, ignorePerks);
			}
			else
			{
				return false;
			}
		}
		#endregion

		#region AllCocks Update Functions
		public void NormalizeDicks(bool untilEven = false) => allCocks.NormalizeDicks(untilEven);

		#endregion

		#region Cum Update Functions
		public double IncreaseCumMultiplier(double additionalMultiplier = 1) => allCocks.IncreaseCumMultiplier(additionalMultiplier);
		public double DecreaseCumMultiplier(double decreaseMultiplier = 1) => allCocks.DecreaseCumMultiplier(decreaseMultiplier);


		public double AddFlatCumAmount(double additionalCum) => allCocks.AddFlatCumAmount(additionalCum);

		//adds a stacking effect to current hours since last cum. this is additive.
		public uint AddPentUpTime(uint additionalTime) => allCocks.AddHoursPentUp(additionalTime);

		//removes some of the additive stacking effect applied to current hours since last cum, if applicable
		public uint RemoveHoursPentUp(uint reliefTime) => allCocks.RemoveHoursPentUp(reliefTime);

		#endregion

		#region Add/Remove Vaginas

		public bool AddVagina() => allVaginas.AddVagina();
		public bool AddVagina(VaginaType newVaginaType) => allVaginas.AddVagina(newVaginaType);


		public bool AddVagina(double clitLength) => allVaginas.AddVagina(clitLength);
		public bool AddVagina(VaginaType newVaginaType, double clitLength) => allVaginas.AddVagina(newVaginaType, clitLength);


		public bool AddVagina(double clitLength, VaginalLooseness looseness, VaginalWetness wetness) => allVaginas.AddVagina(clitLength, looseness, wetness);
		public bool AddVagina(VaginaType newVaginaType, double clitLength, VaginalLooseness looseness, VaginalWetness wetness) => allVaginas.AddVagina(newVaginaType, clitLength, looseness, wetness);


		public string AddedVaginaText() => allVaginas.AddedVaginaText();


		public int RemoveVagina(int count = 1) => allVaginas.RemoveVagina(count);
		public int RemoveVaginaAt(int index, int count = 1)
		{
			return allVaginas.RemoveVaginaAt(index, count);
		}
		public bool RemoveVagina(Vagina vagina)
		{
			return allVaginas.RemoveVagina(vagina);
		}
		public int RemoveExtraVaginas() => allVaginas.RemoveExtraVaginas();


		public int RemoveAllVaginas() => allVaginas.RemoveAllVaginas();

		#endregion

		#region Update Common Vagina Functions

		public bool UpdateVagina(int index, VaginaType vaginaType) => allVaginas.UpdateVagina(index, vaginaType);

		public bool RestoreVagina(int index) => allVaginas.RestoreVagina(index);

		public bool UpdateVagina(Vagina vagina, VaginaType vaginaType)
		{
			if (allVaginas[vagina.vaginaIndex] == vagina)
			{
				return allVaginas.UpdateVagina(vagina.vaginaIndex, vaginaType);
			}
			else
			{
				return false;
			}
		}

		public bool RestoreVagina(Vagina vagina)
		{
			if (allVaginas[vagina.vaginaIndex] == vagina)
			{
				return allVaginas.RestoreVagina(vagina.vaginaIndex);
			}
			else
			{
				return false;
			}
		}

		public ushort IncreaseBonusVaginalCapacity(ushort amount) => allVaginas.IncreaseBonusCapacity(amount);

		public ushort DecreaseBonusVaginalCapacity(ushort amount) => allVaginas.DecreaseBonusCapacity(amount);

		public int SetBonusVaginalCapacity(ushort targetCapacity) => allVaginas.SetBonusCapacity(targetCapacity);

		#endregion

		#region Ass Sex Related Functions

		//ass doesn't have a collection because it always exists, so we don't need any magic to handle it. thus, we do everything here.

		internal bool HandleAnalPenetration(double length, double girth, double knotWidth, StandardSpawnType knockupType, double cumAmount,
			byte virilityBonus, bool takeAnalVirginity, bool reachOrgasm, bool sourceIsSelf)
		{
			//tell the ass itself to handle an insertion, and become looser if necessary.
			ass.PenetrateAsshole((ushort)(length * girth), knotWidth, cumAmount, takeAnalVirginity, reachOrgasm, sourceIsSelf);
			//then try to do an anal knockup.
			if (knockupType != null && knockupType is SpawnTypeIncludeAnal analSpawn && womb.canGetAnallyPregnant(true, analSpawn.ignoreAnalPregnancyPreferences))
			{
				return womb.analPregnancy.attemptKnockUp(knockupRate(virilityBonus), knockupType);
			}
			return false;
		}

		internal bool HandleAnalPenetration(Cock source, StandardSpawnType knockupType, double cumAmountOverride, bool reachOrgasm, bool sourceIsSelf)
		{
			return HandleAnalPenetration(source.length, source.girth, source.knotSize, knockupType, cumAmountOverride, source.virility, true, reachOrgasm, sourceIsSelf);
		}

		internal bool HandleAnalPenetration(Cock source, StandardSpawnType knockupType, bool reachOrgasm, bool sourceIsSelf)
		{
			return HandleAnalPenetration(source.length, source.girth, source.knotSize, knockupType, source.cumAmount, source.virility, true, reachOrgasm, sourceIsSelf);
		}

		internal void HandleAnalPenetration(double length, double girth, double knotWidth, double cumAmount, bool takeAnalVirginity, bool reachOrgasm, bool sourceIsSelf)
		{
			HandleAnalPenetration(length, girth, knotWidth, null, cumAmount, 0, takeAnalVirginity, reachOrgasm, sourceIsSelf);
		}

		internal bool HandleAnalPregnancyOverride(StandardSpawnType knockupType, double knockupRate)
		{
			if (knockupType != null && knockupType is SpawnTypeIncludeAnal analSpawn && womb.canGetAnallyPregnant(true, analSpawn.ignoreAnalPregnancyPreferences))
			{
				return womb.analPregnancy.attemptKnockUp(knockupRate, knockupType);
			}
			return false;
		}

		internal void HandleAnalOrgasmGeneric(bool dryOrgasm)
		{
			ass.OrgasmGeneric(dryOrgasm);
		}


		#endregion

		#region Vagina Sex-Related Functions
		internal bool HandleVaginalPenetration(int vaginaIndex, double length, double girth, double knotWidth, double cumAmount, bool takeVirginity, bool reachOrgasm,
			bool selfPenetrate)
		{
			return HandleVaginalPenetration(vaginaIndex, length, girth, knotWidth, null, cumAmount, 0, takeVirginity, reachOrgasm, selfPenetrate);
		}
		internal bool HandleVaginalPenetration(int vaginaIndex, double length, double girth, double knotWidth, StandardSpawnType knockupType, double cumAmount, byte virilityBonus, bool takeVirginity,
			bool reachOrgasm, bool selfPenetrate)
		{
			allVaginas.HandleVaginalPenetration(vaginaIndex, length, girth, knotWidth, cumAmount, takeVirginity, reachOrgasm, selfPenetrate);

			if (vaginaIndex == 0 && womb.canGetPregnant(true) && knockupType != null)
			{
				return womb.normalPregnancy.attemptKnockUp(knockupRate(virilityBonus), knockupType);
			}
			else if (vaginaIndex == 1 && womb.canGetSecondaryNormalPregnant(true) && knockupType != null)
			{
				return womb.secondaryNormalPregnancy.attemptKnockUp(knockupRate(virilityBonus), knockupType);

			}
			return false;
		}

		internal bool HandleVaginalPenetration(int vaginaIndex, Cock sourceCock, StandardSpawnType knockupType, bool reachOrgasm, bool selfPenetrate)
		{
			return HandleVaginalPenetration(vaginaIndex, sourceCock.length, sourceCock.girth, sourceCock.knotSize, knockupType, sourceCock.cumAmount, sourceCock.virility,
				true, reachOrgasm, selfPenetrate);
		}

		internal bool HandleVaginalPenetration(int vaginaIndex, Cock sourceCock, StandardSpawnType knockupType, double cumAmountOverride, bool reachOrgasm, bool selfPenetrate)
		{
			return HandleVaginalPenetration(vaginaIndex, sourceCock.length, sourceCock.girth, sourceCock.knotSize, knockupType, cumAmountOverride, sourceCock.virility,
				true, reachOrgasm, selfPenetrate);
		}

		internal bool HandleVaginalPregnancyOverride(int vaginaIndex, StandardSpawnType knockupType, double knockupRate)
		{
			if (vaginaIndex == 0 && womb.canGetPregnant(vaginas.Count > 0))
			{
				return womb.normalPregnancy.attemptKnockUp(knockupRate, knockupType);
			}
			else if (vaginaIndex == 1 && womb.canGetPregnant(vaginas.Count > 1))
			{
				return womb.secondaryNormalPregnancy.attemptKnockUp(knockupRate, knockupType);
			}
			return false;
		}

		//'Dry' orgasm is orgasm without stimulation.
		internal void HandleVaginaOrgasmGeneric(int vaginaIndex, bool dryOrgasm)
		{
			allVaginas.HandleVaginaOrgasmGeneric(vaginaIndex, dryOrgasm);
		}

		#endregion

		#region Clit Sex-Related Functions

		internal void HandleClitPenetrate(int vaginaIndex, bool reachOrgasm, bool selfPenetrate)
		{
			allVaginas.HandleClitPenetrate(vaginaIndex, reachOrgasm, selfPenetrate);
		}
		#endregion

		#region Cock Sex Related Functions

		internal void HandleCockSounding(int cockIndex, double penetratorLength, double penetratorWidth, double knotSize, double cumAmount, bool reachOrgasm)
		{
			allCocks.HandleCockSounding(cockIndex, penetratorLength, penetratorWidth, knotSize, cumAmount, reachOrgasm);
		}

		internal void HandleCockSounding(int cockIndex, Cock sourceCock, bool reachOrgasm)
		{
			HandleCockSounding(cockIndex, sourceCock.length, sourceCock.girth, sourceCock.knotSize, sourceCock.cumAmount, reachOrgasm);
		}

		internal void HandleCockSounding(int cockIndex, Cock sourceCock, double cumAmountOverride, bool reachOrgasm)
		{
			HandleCockSounding(cockIndex, sourceCock.length, sourceCock.girth, sourceCock.knotSize, cumAmountOverride, reachOrgasm);
		}

		internal void HandleCockPenetrate(int cockIndex, bool reachOrgasm, bool selfPenetrate)
		{
			allCocks.HandleCockPenetrate(cockIndex, reachOrgasm, selfPenetrate);
		}

		internal void DoCockOrgasmGeneric(int cockIndex, bool dryOrgasm)
		{
			allCocks.DoCockOrgasmGeneric(cockIndex, dryOrgasm);
		}

		#endregion

		#region Breast Sex Related Functions

		public bool CanTitFuck()
		{
			return breastRows.Any(x => x.TittyFuckable());
		}

		//to be frank, idk what would actually orgasm when being titty fucked, but, uhhhh... i guess it can be stored in stats or some shit?
		internal void HandleTittyFuck(int breastIndex, Cock sourceCock, bool reachOrgasm, bool titFuckingSelf)
		{
			HandleTittyFuck(breastIndex, sourceCock.length, sourceCock.girth, sourceCock.knotSize, sourceCock.cumAmount, reachOrgasm, titFuckingSelf);
		}

		internal void HandleTittyFuck(int breastIndex, Cock sourceCock, double cumAmountOverride, bool reachOrgasm, bool titFuckingSelf)
		{
			HandleTittyFuck(breastIndex, sourceCock.length, sourceCock.girth, sourceCock.knotSize, cumAmountOverride, reachOrgasm, titFuckingSelf);
		}

		internal void HandleTittyFuck(int breastIndex, double length, double girth, double knotWidth, double cumAmount, bool reachOrgasm, bool titFuckingSelf)
		{
			allBreasts.HandleTittyFuck(breastIndex, length, girth, knotWidth, cumAmount, reachOrgasm, titFuckingSelf);
		}

		internal void HandleTitOrgasmGeneric(int breastIndex, bool dryOrgasm)
		{
			allBreasts.HandleTitOrgasmGeneric(breastIndex, dryOrgasm);
		}
		#endregion

		#region Nipple Sex Related Functions

		internal void HandleNipplePenetration(int breastIndex, Cock sourceCock, bool reachOrgasm, bool selfPenetrating)
		{
			HandleNipplePenetration(breastIndex, sourceCock.length, sourceCock.girth, sourceCock.knotSize, sourceCock.cumAmount, reachOrgasm, selfPenetrating);
		}

		internal void HandleNipplePenetration(int breastIndex, Cock sourceCock, double cumAmountOverride, bool reachOrgasm, bool selfPenetrating)
		{
			HandleNipplePenetration(breastIndex, sourceCock.length, sourceCock.girth, sourceCock.knotSize, cumAmountOverride, reachOrgasm, selfPenetrating);
		}

		internal void HandleNipplePenetration(int breastIndex, double length, double girth, double knotWidth, double cumAmount, bool reachOrgasm, bool selfPenetrating)
		{
			allBreasts.HandleNipplePenetration(breastIndex, length, girth, knotWidth, cumAmount, reachOrgasm, selfPenetrating);
		}

		internal void HandleNippleDickPenetrate(int breastIndex, bool reachOrgasm)
		{
			allBreasts.HandleNippleDickPenetrate(breastIndex, reachOrgasm);
		}

		#endregion

		#region Lactation Sex Related Functions

		public double MilkOrSuckle() => allBreasts.MilkOrSuckle();

		public double MilkOrSuckle(double maxAmount) => allBreasts.MilkOrSuckle(maxAmount);

		#endregion

		//Genitals and Genitals Data

		#region Breast Text

		public string AllBreastsShortDescription(bool alternateFormat = false) => allBreasts.AllBreastsShortDescription(alternateFormat);
		public string AllBreastsLongDescription(bool alternateFormat = false) => allBreasts.AllBreastsLongDescription(alternateFormat);
		public string AllBreastsFullDescription(bool alternateFormat = false) => allBreasts.AllBreastsFullDescription(alternateFormat);
		public string ChestOrAllBreastsShort(bool alternateFormat = false) => allBreasts.ChestOrAllBreastsShort(alternateFormat);
		public string ChestOrAllBreastsLong(bool alternateFormat = false) => allBreasts.ChestOrAllBreastsLong(alternateFormat);
		public string ChestOrAllBreastsFull(bool alternateFormat = false) => allBreasts.ChestOrAllBreastsFull(alternateFormat);

		#endregion

		#region Cock Text
		public string SheathOrBaseStr() => allCocks.SheathOrBaseStr();


		public string AllCocksShortDescription() => allCocks.AllCocksShortDescription();
		public string AllCocksShortDescription(out bool isPlural) => allCocks.AllCocksShortDescription(out isPlural);


		public string AllCocksLongDescription() => allCocks.AllCocksLongDescription();
		public string AllCocksLongDescription(out bool isPlural) => allCocks.AllCocksLongDescription(out isPlural);


		public string AllCocksFullDescription() => allCocks.AllCocksFullDescription();
		public string AllCocksFullDescription(out bool isPlural) => allCocks.AllCocksFullDescription(out isPlural);


		public string OneCockOrCocksNoun() => allCocks.OneCockOrCocksNoun();
		public string OneCockOrCocksNoun(Conjugate conjugate) => allCocks.OneCockOrCocksNoun(conjugate);


		public string OneCockOrCocksShort() => allCocks.OneCockOrCocksShort();
		public string OneCockOrCocksShort(Conjugate conjugate) => allCocks.OneCockOrCocksShort(conjugate);


		public string EachCockOrCocksNoun() => allCocks.EachCockOrCocksNoun();
		public string EachCockOrCocksNoun(Conjugate conjugate) => allCocks.EachCockOrCocksNoun(conjugate);


		public string EachCockOrCocksShort() => allCocks.EachCockOrCocksShort();
		public string EachCockOrCocksShort(Conjugate conjugate) => allCocks.EachCockOrCocksShort(conjugate);


		public string EachCockOrCocksNoun(Conjugate conjugate, out bool isPlural) => allCocks.EachCockOrCocksNoun(conjugate, out isPlural);

		public string EachCockOrCocksShort(Conjugate conjugate, out bool isPlural) => allCocks.EachCockOrCocksShort(conjugate, out isPlural);

		#endregion

		#region Vagina Text
		public string AllVaginasNoun() => allVaginas.AllVaginasNoun();
		public string AllVaginasNoun(out bool isPlural) => allVaginas.AllVaginasNoun(out isPlural);
		public string AllVaginasShortDescription() => allVaginas.AllVaginasShortDescription();
		public string AllVaginasShortDescription(out bool isPlural) => allVaginas.AllVaginasShortDescription(out isPlural);

		public string AllVaginasLongDescription() => allVaginas.AllVaginasLongDescription();
		public string AllVaginasLongDescription(out bool isPlural) => allVaginas.AllVaginasLongDescription(out isPlural);

		public string AllVaginasFullDescription() => allVaginas.AllVaginasFullDescription();
		public string AllVaginasFullDescription(out bool isPlural) => allVaginas.AllVaginasFullDescription(out isPlural);


		public string OneVaginaOrVaginasNoun() => allVaginas.OneVaginaOrVaginasNoun();
		public string OneVaginaOrVaginasNoun(Conjugate conjugate) => allVaginas.OneVaginaOrVaginasNoun(conjugate);

		public string OneVaginaOrVaginasShort() => allVaginas.OneVaginaOrVaginasShort();
		public string OneVaginaOrVaginasShort(Conjugate conjugate) => allVaginas.OneVaginaOrVaginasShort(conjugate);


		public string EachVaginaOrVaginasNoun() => allVaginas.EachVaginaOrVaginasNoun();
		public string EachVaginaOrVaginasNoun(Conjugate conjugate) => allVaginas.EachVaginaOrVaginasNoun(conjugate);
		public string EachVaginaOrVaginasNoun(Conjugate conjugate, out bool isPlural) => allVaginas.EachVaginaOrVaginasNoun(conjugate, out isPlural);


		public string EachVaginaOrVaginasShort() => allVaginas.EachVaginaOrVaginasShort();
		public string EachVaginaOrVaginasShort(Conjugate conjugate) => allVaginas.EachVaginaOrVaginasShort(conjugate);
		public string EachVaginaOrVaginasShort(Conjugate conjugate, out bool isPlural) => allVaginas.EachVaginaOrVaginasShort(conjugate, out isPlural);

		public string AllClitsNoun() => allVaginas.AllClitsNoun();
		public string AllClitsNoun(out bool isPlural) => allVaginas.AllClitsNoun(out isPlural);

		public string AllClitsShortDescription() => allVaginas.AllClitsShortDescription();
		public string AllClitsShortDescription(out bool isPlural) => allVaginas.AllClitsShortDescription(out isPlural);


		public string AllClitsLongDescription() => allVaginas.AllClitsLongDescription();
		public string AllClitsLongDescription(out bool isPlural) => allVaginas.AllClitsLongDescription(out isPlural);

		public string OneClitOrClitsNoun() => allVaginas.OneClitOrClitsNoun();
		public string OneClitOrClitsNoun(Conjugate conjugate) => allVaginas.OneClitOrClitsNoun(conjugate);

		public string OneClitOrClitsShort() => allVaginas.OneClitOrClitsShort();
		public string OneClitOrClitsShort(Conjugate conjugate) => allVaginas.OneClitOrClitsShort(conjugate);


		public string EachClitOrClitsNoun() => allVaginas.EachClitOrClitsNoun();
		public string EachClitOrClitsNoun(Conjugate conjugate) => allVaginas.EachClitOrClitsNoun(conjugate);
		public string EachClitOrClitsNoun(Conjugate conjugate, out bool isPlural) => allVaginas.EachClitOrClitsNoun(conjugate, out isPlural);


		public string EachClitOrClitsShort() => allVaginas.EachClitOrClitsShort();
		public string EachClitOrClitsShort(Conjugate conjugate) => allVaginas.EachClitOrClitsShort(conjugate);
		public string EachClitOrClitsShort(Conjugate conjugate, out bool isPlural) => allVaginas.EachClitOrClitsShort(conjugate, out isPlural);
		#endregion

		#region Common Nipple Text
		public string CommonShortNippleDescription() => allBreasts.CommonShortNippleDescription();
		public string CommonShortNippleDescription(bool plural, bool allowQuadNippleTextIfApplicable = true)
			=> allBreasts.CommonShortNippleDescription(plural, allowQuadNippleTextIfApplicable);

		public string CommonSingleNippleDescription() => allBreasts.CommonSingleNippleDescription();
		public string CommonSingleNipplpeDescription(bool allowQuadNippleIfApplicable) => allBreasts.CommonSingleNippleDescription(allowQuadNippleIfApplicable);

		public string CommonLongNippleDescription(bool alternateFormat = false, bool plural = true, bool usePreciseMeasurements = false)
			=> allBreasts.CommonLongNippleDescription(alternateFormat, plural, usePreciseMeasurements);
		public string CommonFullNippleDescription(bool alternateFormat = false, bool plural = true, bool usePreciseMeasurements = false)
			=> allBreasts.CommonFullNippleDescription(alternateFormat, plural, usePreciseMeasurements);

		public string CommonOneNippleDescription() => allBreasts.CommonOneNippleDescription();
		public string CommonOneNippleDescription(Conjugate conjugate) => allBreasts.CommonOneNippleDescription(conjugate);

		public string CommonEachNippleDescription() => allBreasts.CommonEachNippleDescription();
		public string CommonEachNippleDescription(Conjugate conjugate) => allBreasts.CommonEachNippleDescription(conjugate);

		public string CommonEachNippleDescription(Conjugate conjugate, out bool isPlural) => allBreasts.CommonEachNippleDescription(conjugate, out isPlural);
		#endregion

		#region Breast Aggregate Functions

		public CupSize BiggestCupSize() => allBreasts.BiggestCupSize();


		public CupSize AverageCupSize() => allBreasts.AverageCupSize();


		public CupSize SmallestCupSize() => allBreasts.SmallestCupSize();


		public Breasts LargestBreast() => allBreasts.LargestBreast();


		public Breasts SmallestBreast() => allBreasts.SmallestBreast();


		public BreastData AverageBreasts() => allBreasts.AverageBreasts();


		#endregion

		#region Cock Aggregate Functions
		public double BiggestCockTotalSize() => allCocks.BiggestCockTotalSize();


		public double LongestCockLength() => allCocks.LongestCockLength();


		public double WidestCockMeasure() => allCocks.WidestCockMeasure();


		public Cock BiggestCockByArea() => allCocks.BiggestCockByArea();


		public Cock LongestCock() => allCocks.LongestCock();


		public Cock WidestCock() => allCocks.WidestCock();


		public double AverageCockSize() => allCocks.AverageCockSize();


		public double AverageCockLength() => allCocks.AverageCockLength();


		public double AverageCockGirth() => allCocks.AverageCockGirth();


		public CockData AverageCock() => allCocks.AverageCock();


		public double SmallestCockTotalSize() => allCocks.SmallestCockTotalSize();


		public double ShortestCockLength() => allCocks.ShortestCockLength();


		public double ThinnestCockMeasure() => allCocks.ThinnestCockMeasure();


		public Cock SmallestCockByArea() => allCocks.SmallestCockByArea();


		public Cock ShortestCock() => allCocks.ShortestCock();


		public Cock ThinnestCock() => allCocks.ThinnestCock();


		public int CountCocksOfType(CockType type) => allCocks.CountCocksOfType(type);

		//for those who aren't familiar with linq these are just helpful aliases for common linq queries.

		public bool HasAnyCocksOfType(CockType type) => allCocks.HasACockOfType(type);

		public bool OnlyHasCocksOfType(CockType type) => allCocks.OnlyHasCocksOfType(type);

		public bool OtherCocksUseSheath(int excludedCockIndex) => allCocks.OtherCocksUseSheath(excludedCockIndex);
		#endregion

		#region Vagina Related Aggregate Functions

		public ushort LargestVaginalCapacity() => allVaginas.LargestVaginalCapacity();


		public Vagina LargestVaginalByCapacity() => allVaginas.LargestVaginalByCapacity();

		public ushort SmallestVaginalCapacity() => allVaginas.SmallestVaginalCapacity();


		public Vagina SmallestVaginalByCapacity() => allVaginas.SmallestVaginalByCapacity();


		public ushort AverageVaginalCapacity() => allVaginas.AverageVaginalCapacity();


		public VaginalWetness LargestVaginalWetness() => allVaginas.LargestVaginalWetness();


		public Vagina LargestVaginalByWetness() => allVaginas.LargestVaginalByWetness();

		public VaginalWetness SmallestVaginalWetness() => allVaginas.SmallestVaginalWetness();


		public Vagina SmallestVaginalByWetness() => allVaginas.SmallestVaginalByWetness();


		public VaginalWetness AverageVaginalWetness() => allVaginas.AverageVaginalWetness();


		public VaginalLooseness LargestVaginalLooseness() => allVaginas.LargestVaginalLooseness();


		public Vagina LargestVaginalByLooseness() => allVaginas.LargestVaginalByLooseness();

		public VaginalLooseness SmallestVaginalLooseness() => allVaginas.SmallestVaginalLooseness();


		public Vagina TightestVagina() => allVaginas.SmallestVaginalByLooseness();


		public VaginalLooseness AverageVaginalLooseness() => allVaginas.AverageVaginalLooseness();


		public Vagina LargestVaginaByClitSize() => allVaginas.LargestVaginaByClitSize();


		public Vagina SmallestVaginaByClitSize() => allVaginas.SmallestVaginaByClitSize();


		public int CountVaginasOfType(VaginaType vaginaType) => allVaginas.CountVaginasOfType(vaginaType);

		//for those who aren't familiar with linq these are just helpful aliases for common linq queries.

		public bool HasAVaginaOfType(VaginaType type) => allVaginas.HasAVaginaOfType(type);

		public bool OnlyHasVaginasOfType(VaginaType type) => allVaginas.OnlyHasVaginasOfType(type);

		public bool HasVirginVagina() => allVaginas.HasVirginVagina();

		#endregion

		#region Clit Aggregate Functions

		public double LargestClitSize() => allVaginas.LargestClitSize();


		public double SmallestClitSize() => allVaginas.SmallestClitSize();


		public double AverageClitSize() => allVaginas.AverageClitSize();


		public Clit LargestClit() => allVaginas.LargestClit();


		public Clit SmallestClit() => allVaginas.SmallestClit();


		public VaginaData AverageVagina() => allVaginas.AverageVagina();


		#endregion
	}


	public sealed partial class GenitalsData : SimpleData
	{
		public readonly CockCollectionData allCockData;
		public readonly VaginaCollectionData allVaginaData;
		public readonly BreastCollectionData allBreastData;

		public ReadOnlyCollection<CockData> cocks => allCockData.cocks;
		public int numCocks => cocks.Count;
		public ReadOnlyCollection<VaginaData> vaginas => allVaginaData.vaginas;
		public int numVaginas => vaginas.Count;

		public ReadOnlyCollection<BreastData> breasts => allBreastData.breasts;

		public readonly Gender gender;

		public IEnumerable<ClitData> clits => vaginas.Select(x => x.clit);

		public readonly FemininityData femininity;
		public readonly FertilityData fertility;

		public readonly AssData ass;
		public readonly BallsData balls;

		public readonly WombData womb;

		public readonly double relativeLust;

		public readonly ReadOnlyTattooablePart<GenitalTattooLocation> tattoos;

		internal GenitalsData(Genitals source) : base(source?.creatureID ?? throw new ArgumentNullException(nameof(source)))
		{
			allBreastData = source.allBreasts.AsReadOnlyData();
			allCockData = source.allCocks.AsReadOnlyData();
			allVaginaData = source.allVaginas.AsReadOnlyData();

			gender = source.gender;
			femininity = source.femininity.AsReadOnlyData();
			fertility = source.fertility.AsReadOnlyData();
			ass = source.ass.AsReadOnlyData();
			balls = source.balls.AsReadOnlyData();

			relativeLust = source.relativeLust;

			tattoos = source.tattoos.AsReadOnlyData();

			womb = source.womb.AsReadOnlyData();
		}



		//Genitals and Genitals Data

		#region Breast Text

		public string AllBreastsShortDescription(bool alternateFormat = false) => allBreastData.AllBreastsShortDescription(alternateFormat);
		public string AllBreastsLongDescription(bool alternateFormat = false) => allBreastData.AllBreastsLongDescription(alternateFormat);
		public string AllBreastsFullDescription(bool alternateFormat = false) => allBreastData.AllBreastsFullDescription(alternateFormat);
		public string ChestOrAllBreastsShort(bool alternateFormat = false) => allBreastData.ChestOrAllBreastsShort(alternateFormat);
		public string ChestOrAllBreastsLong(bool alternateFormat = false) => allBreastData.ChestOrAllBreastsLong(alternateFormat);
		public string ChestOrAllBreastsFull(bool alternateFormat = false) => allBreastData.ChestOrAllBreastsFull(alternateFormat);

		#endregion

		#region Cock Text
		public string SheathOrBaseStr() => allCockData.SheathOrBaseStr();


		public string AllCocksShortDescription() => allCockData.AllCocksShortDescription();
		public string AllCocksShortDescription(out bool isPlural) => allCockData.AllCocksShortDescription(out isPlural);


		public string AllCocksLongDescription() => allCockData.AllCocksLongDescription();
		public string AllCocksLongDescription(out bool isPlural) => allCockData.AllCocksLongDescription(out isPlural);


		public string AllCocksFullDescription() => allCockData.AllCocksFullDescription();
		public string AllCocksFullDescription(out bool isPlural) => allCockData.AllCocksFullDescription(out isPlural);


		public string OneCockOrCocksNoun() => allCockData.OneCockOrCocksNoun();
		public string OneCockOrCocksNoun(Conjugate conjugate) => allCockData.OneCockOrCocksNoun(conjugate);


		public string OneCockOrCocksShort() => allCockData.OneCockOrCocksShort();
		public string OneCockOrCocksShort(Conjugate conjugate) => allCockData.OneCockOrCocksShort(conjugate);


		public string EachCockOrCocksNoun() => allCockData.EachCockOrCocksNoun();
		public string EachCockOrCocksNoun(Conjugate conjugate) => allCockData.EachCockOrCocksNoun(conjugate);


		public string EachCockOrCocksShort() => allCockData.EachCockOrCocksShort();
		public string EachCockOrCocksShort(Conjugate conjugate) => allCockData.EachCockOrCocksShort(conjugate);


		public string EachCockOrCocksNoun(Conjugate conjugate, out bool isPlural) => allCockData.EachCockOrCocksNoun(conjugate, out isPlural);


		public string EachCockOrCocksShort(Conjugate conjugate, out bool isPlural) => allCockData.EachCockOrCocksShort(conjugate, out isPlural);

		#endregion

		#region Vagina Text
		public string AllVaginasNoun() => allVaginaData.AllVaginasNoun();
		public string AllVaginasNoun(out bool isPlural) => allVaginaData.AllVaginasNoun(out isPlural);
		public string AllVaginasShortDescription() => allVaginaData.AllVaginasShortDescription();
		public string AllVaginasShortDescription(out bool isPlural) => allVaginaData.AllVaginasShortDescription(out isPlural);

		public string AllVaginasLongDescription() => allVaginaData.AllVaginasLongDescription();
		public string AllVaginasLongDescription(out bool isPlural) => allVaginaData.AllVaginasLongDescription(out isPlural);

		public string AllVaginasFullDescription() => allVaginaData.AllVaginasFullDescription();
		public string AllVaginasFullDescription(out bool isPlural) => allVaginaData.AllVaginasFullDescription(out isPlural);


		public string OneVaginaOrVaginasNoun() => allVaginaData.OneVaginaOrVaginasNoun();
		public string OneVaginaOrVaginasNoun(Conjugate conjugate) => allVaginaData.OneVaginaOrVaginasNoun(conjugate);

		public string OneVaginaOrVaginasShort() => allVaginaData.OneVaginaOrVaginasShort();
		public string OneVaginaOrVaginasShort(Conjugate conjugate) => allVaginaData.OneVaginaOrVaginasShort(conjugate);


		public string EachVaginaOrVaginasNoun() => allVaginaData.EachVaginaOrVaginasNoun();
		public string EachVaginaOrVaginasNoun(Conjugate conjugate) => allVaginaData.EachVaginaOrVaginasNoun(conjugate);
		public string EachVaginaOrVaginasNoun(Conjugate conjugate, out bool isPlural) => allVaginaData.EachVaginaOrVaginasNoun(conjugate, out isPlural);


		public string EachVaginaOrVaginasShort() => allVaginaData.EachVaginaOrVaginasShort();
		public string EachVaginaOrVaginasShort(Conjugate conjugate) => allVaginaData.EachVaginaOrVaginasShort(conjugate);
		public string EachVaginaOrVaginasShort(Conjugate conjugate, out bool isPlural) => allVaginaData.EachVaginaOrVaginasShort(conjugate, out isPlural);

		public string AllClitsNoun() => allVaginaData.AllClitsNoun();
		public string AllClitsNoun(out bool isPlural) => allVaginaData.AllClitsNoun(out isPlural);

		public string AllClitsShortDescription() => allVaginaData.AllClitsShortDescription();
		public string AllClitsShortDescription(out bool isPlural) => allVaginaData.AllClitsShortDescription(out isPlural);


		public string AllClitsLongDescription() => allVaginaData.AllClitsLongDescription();
		public string AllClitsLongDescription(out bool isPlural) => allVaginaData.AllClitsLongDescription(out isPlural);

		public string OneClitOrClitsNoun() => allVaginaData.OneClitOrClitsNoun();
		public string OneClitOrClitsNoun(Conjugate conjugate) => allVaginaData.OneClitOrClitsNoun(conjugate);

		public string OneClitOrClitsShort() => allVaginaData.OneClitOrClitsShort();
		public string OneClitOrClitsShort(Conjugate conjugate) => allVaginaData.OneClitOrClitsShort(conjugate);


		public string EachClitOrClitsNoun() => allVaginaData.EachClitOrClitsNoun();
		public string EachClitOrClitsNoun(Conjugate conjugate) => allVaginaData.EachClitOrClitsNoun(conjugate);
		public string EachClitOrClitsNoun(Conjugate conjugate, out bool isPlural) => allVaginaData.EachClitOrClitsNoun(conjugate, out isPlural);


		public string EachClitOrClitsShort() => allVaginaData.EachClitOrClitsShort();
		public string EachClitOrClitsShort(Conjugate conjugate) => allVaginaData.EachClitOrClitsShort(conjugate);
		public string EachClitOrClitsShort(Conjugate conjugate, out bool isPlural) => allVaginaData.EachClitOrClitsShort(conjugate, out isPlural);
		#endregion

		#region Common Nipple Text
		public string CommonShortNippleDescription() => allBreastData.CommonShortNippleDescription();
		public string CommonShortNippleDescription(bool plural, bool allowQuadNippleTextIfApplicable = true)
			=> allBreastData.CommonShortNippleDescription(plural, allowQuadNippleTextIfApplicable);

		public string CommonSingleNippleDescription() => allBreastData.CommonSingleNippleDescription();
		public string CommonSingleNipplpeDescription(bool allowQuadNippleIfApplicable) => allBreastData.CommonSingleNippleDescription(allowQuadNippleIfApplicable);

		public string CommonLongNippleDescription(bool alternateFormat = false, bool plural = true, bool usePreciseMeasurements = false)
			=> allBreastData.CommonLongNippleDescription(alternateFormat, plural, usePreciseMeasurements);
		public string CommonFullNippleDescription(bool alternateFormat = false, bool plural = true, bool usePreciseMeasurements = false)
			=> allBreastData.CommonFullNippleDescription(alternateFormat, plural, usePreciseMeasurements);

		public string CommonOneNippleDescription() => allBreastData.CommonOneNippleDescription();
		public string CommonOneNippleDescription(Conjugate conjugate) => allBreastData.CommonOneNippleDescription(conjugate);

		public string CommonEachNippleDescription() => allBreastData.CommonEachNippleDescription();
		public string CommonEachNippleDescription(Conjugate conjugate) => allBreastData.CommonEachNippleDescription(conjugate);

		public string CommonEachNippleDescription(Conjugate conjugate, out bool isPlural) => allBreastData.CommonEachNippleDescription(conjugate, out isPlural);
		#endregion

		#region Breast Aggregate Functions

		public CupSize BiggestCupSize() => allBreastData.BiggestCupSize();


		public CupSize AverageCupSize() => allBreastData.AverageCupSize();


		public CupSize SmallestCupSize() => allBreastData.SmallestCupSize();


		public BreastData LargestBreast() => allBreastData.LargestBreast();


		public BreastData SmallestBreast() => allBreastData.SmallestBreast();


		public BreastData AverageBreasts() => allBreastData.AverageBreasts();


		#endregion

		#region Cock Aggregate Functions
		public double BiggestCockSize() => allCockData.BiggestCockSize();


		public double LongestCockLength() => allCockData.LongestCockLength();


		public double WidestCockMeasure() => allCockData.WidestCockMeasure();


		public CockData BiggestCock() => allCockData.BiggestCock();


		public CockData LongestCock() => allCockData.LongestCock();


		public CockData WidestCock() => allCockData.WidestCock();


		public double AverageCockSize() => allCockData.AverageCockSize();


		public double AverageCockLength() => allCockData.AverageCockLength();


		public double AverageCockGirth() => allCockData.AverageCockGirth();


		public CockData AverageCock() => allCockData.AverageCock();


		public double SmallestCockSize() => allCockData.SmallestCockSize();


		public double ShortestCockLength() => allCockData.ShortestCockLength();


		public double ThinnestCockMeasure() => allCockData.ThinnestCockMeasure();


		public CockData SmallestCock() => allCockData.SmallestCock();


		public CockData ShortestCock() => allCockData.ShortestCock();


		public CockData ThinnestCock() => allCockData.ThinnestCock();


		public int CountCocksOfType(CockType type) => allCockData.CountCocksOfType(type);




		public bool OtherCocksUseSheath(int excludedCockIndex) => allCockData.OtherCocksUseSheath(excludedCockIndex);
		#endregion

		#region Vagina Related Aggregate Functions

		public ushort LargestVaginalCapacity() => allVaginaData.LargestVaginalCapacity();


		public VaginaData LargestVaginalByCapacity() => allVaginaData.LargestVaginalByCapacity();

		public ushort SmallestVaginalCapacity() => allVaginaData.SmallestVaginalCapacity();


		public VaginaData SmallestVaginalByCapacity() => allVaginaData.SmallestVaginalByCapacity();


		public ushort AverageVaginalCapacity() => allVaginaData.AverageVaginalCapacity();


		public VaginalWetness LargestVaginalWetness() => allVaginaData.LargestVaginalWetness();


		public VaginaData LargestVaginalByWetness() => allVaginaData.LargestVaginalByWetness();

		public VaginalWetness SmallestVaginalWetness() => allVaginaData.SmallestVaginalWetness();


		public VaginaData SmallestVaginalByWetness() => allVaginaData.SmallestVaginalByWetness();


		public VaginalWetness AverageVaginalWetness() => allVaginaData.AverageVaginalWetness();


		public VaginalLooseness LargestVaginalLooseness() => allVaginaData.LargestVaginalLooseness();


		public VaginaData LargestVaginalByLooseness() => allVaginaData.LargestVaginalByLooseness();

		public VaginalLooseness SmallestVaginalLooseness() => allVaginaData.SmallestVaginalLooseness();


		public VaginaData SmallestVaginalByLooseness() => allVaginaData.SmallestVaginalByLooseness();


		public VaginalLooseness AverageVaginalLooseness() => allVaginaData.AverageVaginalLooseness();


		public VaginaData LargestVaginaByClitSize() => allVaginaData.LargestVaginaByClitSize();


		public VaginaData SmallestVaginaByClitSize() => allVaginaData.SmallestVaginaByClitSize();


		public int CountVaginasOfType(VaginaType vaginaType) => allVaginaData.CountVaginasOfType(vaginaType);


		#endregion

		#region Clit Aggregate Functions

		public double LargestClitSize() => allVaginaData.LargestClitSize();


		public double SmallestClitSize() => allVaginaData.SmallestClitSize();


		public double AverageClitSize() => allVaginaData.AverageClitSize();


		public ClitData LargestClit() => allVaginaData.LargestClit();


		public ClitData SmallestClit() => allVaginaData.SmallestClit();


		public VaginaData AverageVagina() => allVaginaData.AverageVagina();


		#endregion
	}
}



