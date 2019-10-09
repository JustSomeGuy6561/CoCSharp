//Genitals.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 3:16 AM

using CoC.Backend.BodyParts.EventHelpers;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Engine.Time;
using CoC.Backend.Pregnancies;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using WeakEvent;

namespace CoC.Backend.BodyParts
{
	// this is a fucking mess. 
	// need to: handle a late init to use correct minDefault values for cock, vagina, breasts, nipples.
	// alias every fucking function to make values here there and everywhere increase. 
	// every time we get a new cock, set it to default length if one not provided. increase length by delta, regardless.
	// every time we get a vagina set its default wetness, looseness, and clitSize to defaults, unless provided. 
	// breasts - figure out how the fuck we're gonna add new breast rows. 

	//I had the relatively brilliant idea of storing the GameTime for when something happened - last milking, etc. it means we don't need to update it every hour - we just need to update it when it happens.
	//it's a relatively simple calculation to get the current hours since - we just diff GameTime now and stored time. Granted, it's now a byte and int instead of just one int, but w/e. I'll trade
	//an extra byte of memory (or 4 bytes if C# does the whole align things to word boundaries, idk on its optimizations) for less maintenance and less cycles managing the data. 

#warning Make sure to raise events for gender change and pass it along to femininity. Then implement all the various data changes. theres a ton of them.


	//Genitals is the new "master class" for all things sex-related. Note that the old breast store and breast rows have been combined, so there's new behavior for everyone, 
	//but it's even more flexible. Lactation now works like cum - it builds with time, and the amount produced is altered by a multiplier and adder. additionally, it has a fill rate,
	//which mimicks the old breast store levels 'enum'. To allow some leniency in time before overfull causes fill rate to drop, a buffer timer has been added. 
	
	//all sex related functions are available here, with exception to the one that handles receiving oral sex. These are much more specific than the original, but this allows us
	//to store more metadata and statistics for various achievements, perks, or whatever you may need them for. I'll try to provide helpers where i can, but specific cases like gangbangs/threesomes
	//and/or strage sexual situations may require you to deal with these directly. Generally, there are two functions that will need to be called - one for each creature involved, with the corresponding part.
	//so if A is sticking their cock in B's ass, you'd call A.HandleCockPenetrate and B.HandleAnalPenetration. This will handle everything for you, even knockup, if you provide a spawn type. 
	//you can handle specifics, like if the penetrator or penetratee orgasms or not, and whether or not to count this towards the creature's orgasm count (useful for when multiple body parts
	//orgasm simultaneously, which should only count as one orgasm). This also applies to group sex or penetration free sex (tribbing, hotdogging, etc)
	
	//note that orgasms without the corresponding part being penetrated or penetrating, as the situation requires, is called a "dry" orgasm.
	

	//some variables are available in here to allow PC behavior (namely, 48 hours until full breasts, regardless of how high the lactation multiplier is) or normal behavior (breasts fill quicker
	//the higher the lactation multiplier is). Remember, this ruleset works across all NPCs, because lactation amount dependant on breast size, breast count, and lactation multiplier. 
	//which means that despite the fact that Marble and Katherine can have the same lactation multiplier, Marble would take longer to fill up and not be complaining every 3 hours she needs a milking.
	//RIP katherine, lol.

	public sealed partial class Genitals : SimpleSaveablePart<Genitals, GenitalsData>, IBodyPartTimeLazy //for now all the stuff it contains is lazy, so that's all we need.
	{
		public const int MAX_COCKS = 10;
		public const int MAX_VAGINAS = 2;
		//max in game that i can find is 5, but they only ever use 4 rows.
		//apparently Fenoxo said 3 rows, but then after it went open, some shit got 4 rows.
		//i'm not being a dick and reverting that. 4 it is.
		public const int MAX_BREAST_ROWS = 4;

		//creator let's me do delayed init for perks and such. 
		private readonly BreastCreator[] breastCreators;
		private readonly CockCreator[] cockCreators;
		private readonly VaginaCreator[] vaginaCreators;

		public readonly Ass ass;
		public readonly Balls balls;
		public readonly Femininity femininity; //make sure to cap this if not androgynous perk.

		//fertility gets a class because it's not just an int, it also has a bool that determines if the creature is artificially infertile
		//(sand witch pill, contraceptives, whatever.)
		public readonly Fertility fertility;

		public readonly Womb womb;

		//using list, because it's easier to keep track of count when it does it for you. array would work, but it has the problem of counting nulls, and keeping track of that manually is tedious.
		private readonly List<Breasts> _breasts = new List<Breasts>(MAX_BREAST_ROWS);
		private readonly List<Cock> _cocks = new List<Cock>(MAX_COCKS);
		private readonly List<Vagina> _vaginas = new List<Vagina>(MAX_VAGINAS);

		//public readonly Womb womb;
		private Creature creature => CreatureStore.GetCreatureClean(creatureID);

		#region public properties for Cock/Breast/Vagina
		public readonly ReadOnlyCollection<Cock> cocks;

		public readonly ReadOnlyCollection<Vagina> vaginas;
		
		public ReadOnlyCollection<Cock> allCocks => new ReadOnlyCollection<Cock>(_cocks.Union(_vaginas.Where(x => x.omnibusClit).Select(x => x.clit.AsClitCock())).ToList());

		public readonly ReadOnlyCollection<Breasts> breastRows;
		public int numCocks => _cocks.Count + (hasClitCock ? _vaginas.Count : 0);
		public int numBreastRows => _breasts.Count;
		public int numVaginas => _vaginas.Count;

		public ReadOnlyCollection<Clit> clits => new ReadOnlyCollection<Clit>(_vaginas.ConvertAll(x => x.clit));
		public ReadOnlyCollection<Nipples> nipples => new ReadOnlyCollection<Nipples>(_breasts.ConvertAll(x => x.nipples));

		public int nippleCount => _breasts.Count * Breasts.NUM_BREASTS * (quadNipples ? 4 : 1);
		#endregion

		#region Nipple Properties
		public bool blackNipples
		{
			get => _blackNipples;
			private set
			{
				_blackNipples = value;
			}
		}
		private bool _blackNipples;
		public bool quadNipples
		{
			get => _quadNipples;
			private set
			{
				_quadNipples = value;
			}
		}
		private bool _quadNipples;
		public NippleStatus nippleType
		{
			get => _nippleType;
			private set
			{
				_nippleType = value;
			}
		}
		private NippleStatus _nippleType;

		public bool unlockedDickNipples
		{
			get => _unlockedDickNipples;
			private set => _unlockedDickNipples = value;
		}
		private bool _unlockedDickNipples = false;
		#endregion
		//breasts


		public ushort cumMultiplier { get; private set; } = 1; //0-65535 seems like a valid range imo. i don't think i need to cap it. 
		public ushort additionalCum { get; private set; } = 0;//0-65535 seems like a valid range imo. i don't think i need to cap it. 

		private GameDateTime timeLastCum { get; set; }
		public int hoursSinceLastCum => timeLastCum.hoursToNow();
		



		//we use mL for amount here, but store ball size in inches. Let's do it right, no? 1cm^3 = 1mL
		public int totalCum
		{
			get
			{
				if (numCocks == 0)
				{
					return 0;
				}
				double baseValue = additionalCum;
				//each cock will add a small amount

				if (balls.hasBalls)
				{
					double ballSizeCm = balls.size * Measurement.TO_CENTIMETERS;
					baseValue += 4.0 / 3 * Math.PI * Math.Pow(ballSizeCm / 2, 3) * balls.count * cumMultiplier;
				}

				double multiplier = 1;
				if (!balls.hasBalls)
				{
					multiplier = 0.25;
				}
				if (hoursSinceLastCum < 12 && !alwaysProducesMaxCum) //i'd do 24 but this is Mareth, so.
				{
					int hoursOffset = hoursSinceLastCum;
					if (hoursSinceLastCum <= 0) //0 is possible and likely valid, but below 0 means we broke shit. this is just a catch-all. 
					{
						hoursOffset = 1;
					}
					multiplier *= hoursOffset / 12.0;
				}
				multiplier *= bonusCumMultiplier;
				baseValue = baseValue * multiplier + bonusCumAdded;

				if (baseValue > int.MaxValue)
				{
					return int.MaxValue;
				}

				//at absolute min, 2units per cock. some large cocks may increase this minimum further. this just makes the sex related functions easier to handle for multiple cocks being used at once
				double minimumValue = cocks.Sum(x => x.minCumAmount);

				if (baseValue < minimumValue)
				{
					baseValue = minimumValue;
				}
				return (int)Math.Floor(baseValue);
			}
		}

		public float knockupRate(byte bonusVirility = 0) => (fertility.currentFertility + bonusVirility) / 100f;

		//ass is readonly, always exists. we don't need any alias magic for it. 
		public uint analSexCount => ass.sexCount;
		public uint analPenetrationCount => ass.penetrateCount;
		public uint analOrgasmCount => ass.orgasmCount;
		public uint analDryOrgasmCount => ass.dryOrgasmCount;

		internal bool HandleAnalPenetration(float length, float girth, float knotWidth, StandardSpawnType knockupType, float cumAmount, byte virilityBonus, bool takeAnalVirginity, bool reachOrgasm)
		{
			ass.PenetrateAsshole((ushort)(length * girth), knotWidth, cumAmount, takeAnalVirginity, reachOrgasm);
			if (knockupType != null && knockupType is SpawnTypeIncludeAnal analSpawn && womb.canGetAnallyPregnant(true, analSpawn.ignoreAnalPregnancyPreferences))
			{
				return womb.analPregnancy.attemptKnockUp(knockupRate(virilityBonus), knockupType);
			}
			return false;
		}

		internal bool HandleAnalPenetration(Cock source, StandardSpawnType knockupType, float cumAmountOverride, bool reachOrgasm)
		{
			return HandleAnalPenetration(source.length, source.girth, source.knotSize, knockupType, cumAmountOverride, source.virility, true, reachOrgasm);
		}

		internal bool HandleAnalPenetration(Cock source, StandardSpawnType knockupType, bool reachOrgasm)
		{
			return HandleAnalPenetration(source.length, source.girth, source.knotSize, knockupType, source.cumAmount, source.virility, true, reachOrgasm);
		}

		internal void HandleAnalPenetration(float length, float girth, float knotWidth, float cumAmount, bool takeAnalVirginity, bool reachOrgasm)
		{
			HandleAnalPenetration(length, girth, knotWidth, null, cumAmount, 0, takeAnalVirginity, reachOrgasm);
		}

		internal bool HandleAnalPregnancyOverride(StandardSpawnType knockupType, float knockupRate)
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

		//despite my attempts to remove status effects wherever possible, i'm not crazy. Heat/Rut/Dsyfunction seem like ideal status effects. 
		//in that they are temporary effects. as such, i'm not putting them here. 

		public Gender gender
		{
			get
			{
				Gender retVal = Gender.GENDERLESS;
				retVal |= numVaginas > 0 ? Gender.FEMALE : Gender.GENDERLESS;
				retVal |= numCocks > 0 || hasClitCock ? Gender.MALE : Gender.GENDERLESS;
				return retVal;
			}
		}

		public bool hasClitCock
		{
			get => _hasClitCock;
			private set
			{

				if (hasClitCock != value)
				{
					_vaginas.ForEach((x) => { if (value) x.ActivateOmnibusClit(); else x.DeactivateOmnibusClit(); });
				}
				_hasClitCock = value;
			}

		}
		private bool _hasClitCock = false;

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

			initHelper(out cocks, out vaginas, out breastRows);

			femininity = new Femininity(creatureID, initialGender);
			fertility = new Fertility(creatureID, initialGender);

			this.womb = womb ?? throw new ArgumentNullException(nameof(womb));
		}

		internal Genitals(Guid creatureID, Ass ass, BreastCreator[] breasts, CockCreator[] cocks, Balls balls, VaginaCreator[] vaginas, Womb womb, byte? femininity, Fertility fertility) : base(creatureID)
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

			initHelper(out this.cocks, out this.vaginas, out this.breastRows);

			this.femininity = femininity != null ? new Femininity(creatureID, computedGender, (byte)femininity) : new Femininity(creatureID, computedGender);
			this.fertility = fertility ?? throw new ArgumentNullException(nameof(fertility));

			this.womb = womb ?? throw new ArgumentNullException(nameof(womb));
		}

		protected internal override void PostPerkInit()
		{
			ass.PostPerkInit();
			balls.PostPerkInit();

			CockPerkHelper CockPerkData = GetCockPerkData();
			BreastPerkHelper BreastPerkData = GetBreastPerkData();
			VaginaPerkHelper VaginaPerkData = GetVaginaPerkData();

			if (cockCreators != null)
			{
				_cocks.AddRange(cockCreators.Where(x => x != null).Select(x => new Cock(creatureID, CockPerkData, x.type, x.validLength, x.validGirth, x.knot)).Take(MAX_COCKS));
			}
			if (vaginaCreators != null)
			{
				_vaginas.AddRange(vaginaCreators.Where(x => x != null).Select(x => new Vagina(creatureID, VaginaPerkData, x.type, x.validClitLength, x.looseness,
					x.wetness, x.virgin, x.hasClitCock)).Take(MAX_VAGINAS));
			}
			_breasts.AddRange(breastCreators.Where(x => x != null).Select(x => new Breasts(creatureID, BreastPerkData, x.cupSize, x.validNippleLength)).Take(MAX_BREAST_ROWS));

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

		public override GenitalsData AsReadOnlyData()
		{
			return new GenitalsData(this, GetCockPerkData(), GetVaginaPerkData(), GetBreastPerkData());
		}

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

		private void initHelper(out ReadOnlyCollection<Cock> cocks, out ReadOnlyCollection<Vagina> vaginas, out ReadOnlyCollection<Breasts> breasts)
		{
			cocks = new ReadOnlyCollection<Cock>(_cocks);
			vaginas = new ReadOnlyCollection<Vagina>(_vaginas);
			breasts = new ReadOnlyCollection<Breasts>(_breasts);
		}

		#endregion
		#region Genital Exclusive
		internal bool MakeFemale()
		{
			if (numCocks == 0 && !hasClitCock && numVaginas > 0)
			{
				return false;
			}
			RemoveCock(numCocks);
			if (numVaginas == 0)
			{
				AddVagina(VaginaType.HUMAN);
			}
			hasClitCock = false;
			return true;
		}

		internal bool MakeMale()
		{
			if (numVaginas == 0 && numBreastRows == 1 && _breasts[0].isMale && numCocks > 0)
			{
				return false;
			}
			RemoveVagina(numVaginas);
			RemoveBreastRow(numBreastRows);
			if (numCocks == 0)
			{
				AddCock(CockType.HUMAN);
			}
			return true;
		}

		public SimpleDescriptor AllCocksShortDesc => AllCocksShort;

		public SimpleDescriptor AllCocksFullDesc => AllCocksFull;

		public SimpleDescriptor AllVaginasShortDesc => AllVaginasShort;
		public SimpleDescriptor AllVaginasFullDesc => AllVaginasFull;

		#region ExtraGenderChecks
		//allows players with clit-dicks (a hard-to-obtain omnibus trait) to appear female, and do female scenes. 
		//NYI, but also allows players to "surprise" NPCs expecting lesbian sex (or males expecting straight sex)
		//Not to be confused with the "traps" check - this is a check for your junk
		public Gender genderWithoutOmnibusClit
		{
			get
			{
				if (gender == Gender.HERM && numCocks == 0 && hasClitCock)
				{
					return Gender.FEMALE;
				}
				return gender;
			}
		}

		/* Trap check. use this where player appearance is more important than actual assets, or for trappy sex, idk.
		 * 
		 * Female: C-cup breasts and >35 masculinity OR <6in Dick and >65 masculinity
		 * Male: 6in+ Dick and <65 masculinity OR B-Cup or smaller breasts and <35 masculinity
		 * Genderless: <6in Dick, B-cup or smaller breasts, and 35-65 masculinity.
		 * Herm: everything else.
		 * 
		 * How you deal with androgynous and herm is up to you. Note that b/c this is a trap check, something may appear
		 * to be a herm, but not be (large breasts and a dick, for example, but no vag).
		 * 
		*/
		public Gender trappyGender
		{
			get
			{
				//noticable bulge and breasts
				if (BiggestCupSize() > CupSize.B && BiggestCockSize() >= 6)
				{
					return Gender.HERM;
				}
				//noticable breasts and sufficiently female
				else if (BiggestCupSize() > CupSize.B && !femininity.atLeastSlightlyMasculine)
				{
					return Gender.FEMALE;
				}
				//noticable dick and sufficiently male
				else if (BiggestCockSize() >= 6 && !femininity.atLeastSlightlyFeminine)
				{
					return Gender.MALE;
				}
				//not noticable assets - go by appearance
				else if (BiggestCockSize() < 6 && BiggestCupSize() <= CupSize.B)
				{
					if (femininity.atLeastSlightlyFeminine) return Gender.FEMALE;
					else if (femininity.atLeastSlightlyMasculine) return Gender.MALE;
					return Gender.GENDERLESS;
				}
				//noticable breasts or dick, but too masculine or feminine. 
				return Gender.HERM;
			}
		}

		//Parses the current gender information and returns a 'normalized' state for things requiring "Male" or "Female"
		//if you find this term offensive, by all means, rename it - i couldn't think of a better way to explain what i was going for.

		public bool trappyGenderToMaleFemaleIsMale()
		{
			Gender trapGender = trappyGender;
			if (trapGender == Gender.HERM)
			{
				return BiggestCupSize() < CupSize.B && femininity < 50 || BiggestCupSize() == CupSize.FLAT && femininity < 75;
			}
			else if (trapGender == Gender.MALE)
			{
				return true;
			}
			else if (trapGender == Gender.FEMALE)
			{
				return false;
			}
			else
			{
				return (BiggestCupSize() < CupSize.C && femininity < 75);
			}

		}
		#endregion
		#endregion
		#region Validation
		internal override bool Validate(bool correctInvalidData)
		{
#warning FIX ME!
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		#endregion

		#region Femininity
		public byte feminize(byte amount)
		{
			return femininity.feminize(amount);
		}

		public byte masculinize(byte amount)
		{
			return femininity.masculinize(amount);
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
			foreach (var vagina in _vaginas)
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

			if (isLactating)
			{
				outputBuilder.Append(DoLactationCheck(isPlayer, hoursPassed));
			}
			//If lactating and something happened via time passing(full, slowed down, etc), set needs output to true, append 
			//the result of this to the output string builder.

			return outputBuilder.ToString();

		}

		private bool DoLazy(IBodyPartTimeLazy member, bool isPlayer, byte hoursPassed, out string output)
		{
			output = member.reactToTimePassing(isPlayer, hoursPassed);
			return !string.IsNullOrEmpty(output);
		}

		#endregion
	}

	public sealed class GenitalsData : SimpleData
	{
		internal readonly CockPerkHelper cockPerks;
		internal readonly BreastPerkHelper breastPerks;
		internal readonly VaginaPerkHelper vaginaPerks;

		public readonly ReadOnlyCollection<CockData> cocks;
		public readonly ReadOnlyCollection<VaginaData> vaginas;
		public readonly ReadOnlyCollection<BreastData> breasts;

		public readonly FemininityData femininity;
		public readonly FertilityData fertility;

		public readonly AssData ass;
		public readonly BallsData balls;

		//cum amount
		//lactation amount


		internal GenitalsData(Genitals source, CockPerkHelper cockData, VaginaPerkHelper vaginaData, BreastPerkHelper breastData) 
			: base(source?.creatureID ?? throw new ArgumentNullException(nameof(source)))
		{
			cockPerks = cockData ?? throw new ArgumentNullException(nameof(cockData));
			vaginaPerks = vaginaData ?? throw new ArgumentNullException(nameof(vaginaData));
			breastPerks = breastData ?? throw new ArgumentNullException(nameof(breastData));

			cocks = new ReadOnlyCollection<CockData>(source.cocks.Select(x => x.AsReadOnlyData()).ToList());
			vaginas = new ReadOnlyCollection<VaginaData>(source.vaginas.Select(x => x.AsReadOnlyData()).ToList());
			breasts = new ReadOnlyCollection<BreastData>(source.breastRows.Select(x => x.AsReadOnlyData()).ToList());

			ass = source.ass.AsReadOnlyData();
			balls = source.balls.AsReadOnlyData();

			femininity = source.femininity.AsReadOnlyData();
			fertility = source.fertility.AsReadOnlyData();
		}
	}
}



