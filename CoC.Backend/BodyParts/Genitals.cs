//Genitals.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 3:16 AM

using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Engine.Time;
using CoC.Backend.Perks;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CoC.Backend.BodyParts
{
	// this is a fucking mess. 
	// need to: handle a late init to use correct minDefault values for cock, vagina, breasts, nipples.
	// alias every fucking function to make values here there and everywhere increase. 
	// every time we get a new cock, set it to default length if one not provided. increase length by delta, regardless.
	// every time we get a vagina set its default wetness, looseness, and clitSize to defaults, unless provided. 
	// breasts - figure out how the fuck we're gonna add new breast rows. 

	//EVERY PIERCING NOW NEEDS TO ALIAS THE PIERCINGS! WOOOO!

	//I had the relatively brilliant idea of storing the GameTime for when something happened - last milking, etc. it means we don't need to update it every hour - we just need to update it when it happens.
	//it's a relatively simple calculation to get the current hours since - we just diff GameTime now and stored time. Granted, it's now a byte and int instead of just one int, but w/e. I'll trade
	//an extra byte of memory (or 4 bytes if C# does the whole align things to word boundaries, idk on its optimizations) for less maintenance and less cycles managing the data. 

#warning Make sure to raise events for gender change and pass it along to femininity. 

	public enum LactationStatus { NOT_LACTATING, LIGHT, MODERATE, STRONG, HEAVY, EPIC }

	//Genitals is the new "master class" for all things sex-related. NPCs that don't require the whole creature class can just use this if they can impregnate. I'll create a base class for this.
	//Note that you could just use cock or vagina or whatever if that's all that's important, but this provides an automated process for all things - it automates milking, time since sexing, 
	//descriptions, knock-ups, etc, which allows you to not have to worry about it. The only downside so far is that BreastStore != BreastRow, so i've combined them and had to deal with the headache 
	//that caused. some variables are available in here to allow PC behavior (namely, 48 hours until full breasts, regardless of how high the lactation multiplier is) or normal behavior (breasts fill quicker
	//the higher the lactation multiplier is). Remember, this ruleset works across all NPCs, because lactation amount dependant on breast size, breast count, and lactation multiplier. 
	//which means that despite the fact that Marble and Katherine can have the same lactation multiplier, Marble would take longer to fill up and not be complaining every 3 hours she needs a milking.
	//RIP katherine, lol.

	public sealed partial class Genitals : SimpleSaveablePart<Genitals>, IBodyPartTimeLazy, IBaseStatPerkAware //for now all the stuff it contains is lazy, so that's all we need.
	{
		PerkStatBonusGetter perkModifiers;

		public const int MAX_COCKS = 10;
		public const int MAX_VAGINAS = 2;
		//max in game that i can find is 5, but they only ever use 4 rows.
		//apparently Fenoxo said 3 rows, but then after it went open, some shit got 4 rows.
		//i'm not being a dick and reverting that. 4 it is.
		public const int MAX_BREAST_ROWS = 4;

		public const float LACTATION_THRESHOLD = 1.0f;
		public const float MODERATE_LACTATION_THRESHOLD = 2.0f;
		public const float STRONG_LACTATION_THRESHOLD = 3.0f;
		public const float HEAVY_LACTATION_THRESHOLD = 5.0f;
		public const float EPIC_LACTATION_THRESHOLD = 7.5f;

		private readonly bool needsLateInit;
		private readonly bool lateInitNew;//true: new. false: delta


		public readonly Ass ass;
		public readonly Balls balls;
		public readonly Femininity femininity; //make sure to cap this if not androgynous perk.

		//fertility gets a class because it's not just an int, it also has a bool that determines if the creature is artificially infertile
		//(sand witch pill, contraceptives, whatever.)
		public readonly Fertility fertility;

		//using list, because it's easier to keep track of count when it does it for you. array would work, but it has the problem of counting nulls, and keeping track of that manually is tedious.
		private readonly List<Breasts> _breasts = new List<Breasts>(MAX_BREAST_ROWS);
		private readonly List<Cock> _cocks = new List<Cock>(MAX_COCKS);
		private readonly List<Vagina> _vaginas = new List<Vagina>(MAX_VAGINAS);

		//public readonly Womb womb;

		#region public properties for Cock/Breast/Vagina
		public readonly ReadOnlyCollection<Cock> cocks;
		public readonly ReadOnlyCollection<Vagina> vaginas;

		public readonly ReadOnlyCollection<Breasts> breasts;
		public int numCocks => _cocks.Count + (hasClitCock ? _vaginas.Count : 0);
		public int numBreastRows => _breasts.Count;
		public int numVaginas => _vaginas.Count;

		public ReadOnlyCollection<Clit> clits => new ReadOnlyCollection<Clit>(_vaginas.ConvertAll(x => x.clit));
		public ReadOnlyCollection<Nipples> nipples => new ReadOnlyCollection<Nipples>(_breasts.ConvertAll(x => x.nipples));

		public int nippleCount => _breasts.Count * (quadNipples ? 4 : 1);
		#endregion


		#region Nipple Properties
		public bool blackNipples
		{
			get => _blackNipples;
			private set
			{
				if (_blackNipples != value)
				{
					_breasts.ForEach(x => x.nipples.setBlackNipple(value));
				}
				_blackNipples = value;
			}
		}
		private bool _blackNipples;
		public bool quadNipples
		{
			get => _quadNipples;
			private set
			{
				if (_quadNipples != value)
				{
					_breasts.ForEach(x => x.nipples.setQuadNipple(value));
				}
				_quadNipples = value;
			}
		}
		private bool _quadNipples;
		public NippleStatus nippleType
		{
			get => _nippleType;
			private set
			{
				if (_nippleType != value)
				{
					_breasts.ForEach(x => x.nipples.setNippleStatus(value));
				}
				_nippleType = value;
			}
		}
		private NippleStatus _nippleType;
		#endregion
		//breasts
		//Lactation: we're gonna create a new ruleset. It's more realistic (i guess), this way, but also more uniform for both the PC and any random NPC that uses this. Allows one nice ruleset. 
		//amount you can carry is gonna be a capacity multiplier (capped, idk at what) * total breast volume
		//idk on how we'll do volume. 
		//lactation multiplier affects how quickly you fill up. this allows creatures with ungodly amounts of milk capacity to fill up in a reasonable time, or smaller capacities to fill slowly
		//or quickly, depending on the scenario. we'll also use it to make the PC take roughly 48 hours to get full 
		//formula is roughly numBreasts * baseFillRate * lactationMultiplier.

		//we'll also use the lactation multiplier for how much milk is released when being suckled or otherwise stimulated. it'll likely be a percent of overall fill rate, multiplied by how long 
		//the stimulation lasts and some factor of character's sensitivity

		//also, we can now allow partial milking, so one NPC doesn't milk the PC's 4 rows of Q Cups dry in one sitting (realism op), though obviously the cow milker would do that.  
		//Two options for this: a duration - how long are they at it? or a flat, maximum amount, where the creature is drained up to that maximum value.  

		//Also, we'll alter how the creature produces milk based on consumption - if the character is often drained to empty, we'll increase production after a time. if they're overful for a period of time
		//it'll decrease. allows characters to induce lactation, and also causes lactation to stop after a period of inactivity.
		//we'll also provide the option to prevent decrease or increase, for things like the feeder perk or its opposite (notably, the PC telling an NPC to prevent it from being to high). 

		//we'll need to rework some items to increase either the capacity multiplier or refill rate, or both. 

		//overall maximum capacity. you can produce more than your current capacity, and be overful, but not past your overall max. when this is hit,
		//it really slows down your production rate, unless otherwise prevented. 
		public float maximumLactationCapacity => capacityMultiplier * (float)breasts.Sum(x => volumeFromCupSize(x.cupSize) * x.numBreasts);
		//current maximum capacity. if you aren't lactating, this is 0. 
		public float lactationCapacity => maximumLactationCapacity * lactationRate;
		private double volumeFromCupSize(CupSize cup)
		{
			//fun fact, i'm now more versed in how cup size works than most women - apparently 60% don't get proper fitting cup/bra measures. The more you know. 
			//Short version: actual capacity varies by figure, notably how wide the person is. basically, cupsize is the delta from measurement at sternum and across the breasts at the nipple level.
			//for our calculations, we're gonna assume a "true" cup size - that is, ~a 30inch measurement at sternum, or a 34band size. 

			//we'll simulate storage by using breast size combined with the internals that don't exactly show on a person's physique, so flat/a cups will still have some lactation, albeit very low amounts.

			byte cupByte = (byte)cup;

			//fudging this - basically, cupsize ~= measurement@nipples - (measurement@sternum+4) which roughly means breast height is 1/2 the cupsize value, but b/c breasts are curved, it's actually even less
			//so we fudge it to ~1/2.54. then we convert that to CM, and it cancels out. i figure that conversion is probably a little off, so i fudge back in an extra .25cm. 
			double height = cupByte + .25;

			//radius of the breast where it meets the chest. we'll assume the figure gets slightly more accomodating as the breasts get bigger, so we widen the radius slightly per cup size.
			double chestRadius = 2.5 + cupByte / 40.0;

			//note: 

			//tiny, not enough volume to be spherical. using an ellipsoid instead. the heights are small enough here that we can use a full ellipsoid and chalk it up to internal storage. 
			if (cupByte < 3)
			{
				return 2 * chestRadius * chestRadius * height * 2 / 3 * Math.PI;
			}
			//big enough to have volume, but still small enough to remain firm and thus are roughly spherical. 
			if (cupByte < 5)
			{
				double breastRadius = cupByte;
				//this might over-estimate internal storage a little, but this isn't precision math for rocket science, so cut me some slack lol. 
				return 4 / 3 * Math.PI * Math.Pow(breastRadius, 3);
			}
			//biggest formula. no longer spherical, because gravity. we'll use a truncated sphere (think cylinder that gets wider as it goes along), that ends in a half-ellipsoid. 
			//i'm more or less ignoring internal storage here because we have so much of it outside. i just add an extra 10 and call it a day. 
			else
			{
				//ellipsoid at the end.
				double breastRadius = 5 + cupByte / 16.0f;
				double breastHeight = breastRadius / 2;
				//the length of the cone part
				double breastLength = height - breastRadius;
				//radius of cone at base: chestRadius. radius at end: breastRadius

				return 10 + Math.PI / 3 * (2 * breastHeight * Math.Pow(breastRadius, 2) + Math.Pow(breastRadius + chestRadius, 2) * breastLength);
			}

		}
		//how much over base can you handle? i'd assume some items will update this.
		public float capacityMultiplier { get; private set; } = 0;

		//how much are you lactating? this value will change naturally depending on how often you sate your needs - if you're constantly overful, your body will produce less.
		//this will combine with your capacity multiplier to fill your breasts over time. 
		public float lactationModifier
		{
			get => _lactationModifier;
			private set
			{
				_lactationModifier = Utils.Clamp2(value, 0, 10f);
				_breasts.ForEach(x => x.SetLactation(_lactationModifier));
			}
		}
		private float _lactationModifier = 0;

		//use this to explain how crazy your nipple leakage/scene whatever is. 
		public LactationStatus lactationStatus
		{
			get
			{
				if (lactationModifier < LACTATION_THRESHOLD)
				{
					return LactationStatus.NOT_LACTATING;
				}
				else if (lactationModifier < MODERATE_LACTATION_THRESHOLD)
				{
					return LactationStatus.LIGHT;
				}
				else if (lactationModifier < STRONG_LACTATION_THRESHOLD)
				{
					return LactationStatus.MODERATE;
				}
				else if (lactationModifier < HEAVY_LACTATION_THRESHOLD)
				{
					return LactationStatus.STRONG;
				}
				else if (lactationModifier < EPIC_LACTATION_THRESHOLD)
				{
					return LactationStatus.HEAVY;
				}
				else
				{
					return LactationStatus.EPIC;
				}
			}
		}

		public bool isLactating => lactationStatus != LactationStatus.NOT_LACTATING;
		private float lactationRate => Utils.Lerp((byte)LactationStatus.NOT_LACTATING, (byte)LactationStatus.EPIC, (byte)lactationStatus, 0, 1.0f);

		public LactationStatus setLactationTo(LactationStatus newStatus)
		{
			lactationModifier = newStatus.MinThreshold();
			return lactationStatus;
		}

		public bool clearLactation()
		{
			return setLactationTo(LactationStatus.NOT_LACTATING) == LactationStatus.NOT_LACTATING;
		}

		public float boostLactation(float byAmount = 0.1f)
		{
			var modifier = lactationModifier;
			lactationModifier += byAmount;
			return lactationModifier - modifier;
		}

		private GameDateTime timeLastMilked;
		public int hoursSinceLastMilked => timeLastMilked.hoursToNow();

#warning TODO: implement time aware for this to fill up over time, a milking and/or suckling method. a means to min/max lactationStatus. 
		//likely need a times ran out of m


#warning NYI
		public ushort cumMultiplier { get; private set; }

		private GameDateTime timeLastCum { get; set; }
		public int hoursSinceLastCum => timeLastCum.hoursToNow();
		private GameDateTime timeLastOrgasm { get; set; }
		public int hoursSinceLastOrgasm => timeLastOrgasm.hoursToNow();

		private bool alwaysAtMax => perkModifiers().AlwaysProducesMaxCum;

		private uint perkCumAdd => perkModifiers().BonusCumAdded;
		private float perkCumMultiply => perkModifiers().BonusCumStacked;

		//we use mL for amount here, but store ball size in inches. Let's do it right, no? 1cm^3 = 1mL
		public int cumAmount
		{
			get
			{
				if (numCocks == 0)
				{
					return 0;
				}
				double baseValue = 0;
				if (!balls.hasBalls)
				{
					baseValue = 1 / 4 * cumMultiplier;
				}
				else
				{
					baseValue = balls.size * Measurement.TO_CENTIMETERS;
					baseValue = 4.0 / 3 * Math.PI * Math.Pow(baseValue / 2, 3) * balls.count * cumMultiplier;
				}
				if (hoursSinceLastCum < 12 && !alwaysAtMax) //i'd do 24 but this is Mareth, so.
				{
					baseValue *= hoursSinceLastCum / 12.0;
				}
				baseValue *= perkCumMultiply;
				baseValue += perkCumAdd;

				if (baseValue > int.MaxValue)
				{
					return int.MaxValue;
				}
				else if (baseValue < 2)
				{
					baseValue = 2;
				}
				return (int)Math.Floor(baseValue);
			}
		}

#warning NYI
		public ushort largestVaginalCapacity { get; private set; }

		//ass is readonly, always exists. we don't need any alias magic for it. 
		public uint timesHadAnalSex => ass.numTimesAnal;
#warning NYI
		public uint timesHadVaginalSex { get; private set; } = 0;
#warning NYI
		public uint timesCockSounded { get; private set; } = 0;
#warning NYI
		public uint timesHadSexWithCock { get; private set; } = 0;

		public bool cockVirgin => timesHadSexWithCock == 0;
		//wetness, looseness.

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


		private Genitals(Gender gender)
		{
			ass = Ass.GenerateDefault();
			_breasts.Add(Breasts.GenerateFromGender(gender));
			balls = Balls.GenerateFromGender(gender);
			_cocks.Add(Cock.GenerateFromGender(gender));
			_vaginas.Add(Vagina.GenerateFromGender(gender));
			initHelper(out cocks, out vaginas, out breasts);

			femininity = Femininity.Generate(gender);
			fertility = Fertility.GenerateFromGender(gender);

			needsLateInit = true;
			lateInitNew = true;

		}

		private Genitals(Ass ass, Breasts[] breasts, Cock[] cocks, Balls balls, Vagina[] vaginas, byte? femininity, Fertility fertility)
		{
			this.ass = ass;
			CleanCopy(breasts, _breasts, MAX_BREAST_ROWS);
			this.balls = balls;
			CleanCopy(cocks, _cocks, MAX_COCKS);
			CleanCopy(vaginas, _vaginas, MAX_VAGINAS);
			initHelper(out this.cocks, out this.vaginas, out this.breasts);

			this.femininity = femininity != null ? Femininity.Generate(gender, (byte)femininity) : Femininity.Generate(gender);
			this.fertility = fertility;

			needsLateInit = true;
			lateInitNew = false;
		}

		//delta not needed when we load from a save. 

		private void initHelper(out ReadOnlyCollection<Cock> cocks, out ReadOnlyCollection<Vagina> vaginas, out ReadOnlyCollection<Breasts> breasts)
		{
			cocks = new ReadOnlyCollection<Cock>(_cocks);
			vaginas = new ReadOnlyCollection<Vagina>(_vaginas);
			breasts = new ReadOnlyCollection<Breasts>(_breasts);
		}

		//similar in fashion to array.copy, though a bit slower as there aren't any optimizations. 
		//it will, however, ignore any null values in the source, so the destination is "clean", and may therefore be shorter than the source.
		private void CleanCopy<T>(in T[] source, List<T> destination, int maxEntries)
		{
			if (source is null) return;

			int y = 0;
			for (int x = 0; x < source.Length; x++)
			{
				if (source[x] != null)
				{
					destination.Add(source[x]);
					if (++y == maxEntries) return;
				}
			}
		}
		#endregion
		#region Generate
		internal static Genitals GenerateDefault(Gender gender)
		{
			return new Genitals(gender);
		}

		//in deserialization land or creator constructor, generate the shit beforehand, then pass it in here. 
		//ideally we should use copy constructors on the passed in data, but fuck it, it's internal, so it's "guarenteed" to be valid.
		//and not referenced anywhere that would fuck this shit up.
		internal static Genitals Generate(Ass ass, Breasts[] breasts, Cock[] cocks, Balls balls, Vagina[] vaginas, byte? femininity, Fertility fertility)
		{
			return new Genitals(ass, breasts, cocks, balls, vaginas, femininity, fertility);
		}

		//internal static Genitals GenerateWithWomb(Ass ass, Breasts[] breasts, Cock[] cocks, Balls balls, Vagina[] vaginas, Femininity femininity, Fertility fertility)
		//{
		//	return new Genitals(ass, breasts, cocks, balls, vaginas, femininity, fertility);
		//}

		#endregion
		#region Update
		#region Add
		//Dog and wolf both make breast size one smaller than previous.
		//Everything else keeps the size.
		//Nipple status and blackness vary.
		//new behavior is that's uniform between all breasts.

		//nipple length will be the size of the average of all the other nipples.
		internal bool AddBreastRow()
		{
#warning May want to make this use previous row size, idk.
			if (numBreastRows >= MAX_BREAST_ROWS)
			{
				return false;
			}
			//linq ftw!
			//i find it funny that linq was created for databases, but it really is used for functional programming.
			double avgLength = _breasts.Average((x) => (double)x.nipples.length);
			double avgCup = _breasts.Average((x) => (double)x.cupSize);
			byte cup = (byte)Math.Ceiling(avgCup);
			_breasts.Add(Breasts.Generate((CupSize)cup, (float)avgLength));
			_breasts[_breasts.Count - 1].nipples.setBlackNipple(blackNipples);
			_breasts[_breasts.Count - 1].nipples.setQuadNipple(quadNipples);
			_breasts[_breasts.Count - 1].nipples.setNippleStatus(nippleType);
			return true;
		}

		internal bool AddBreastRow(CupSize cup)
		{
			if (numBreastRows >= MAX_BREAST_ROWS)
			{
				return false;
			}
			double avgLength = _breasts.Average((x) => (double)x.nipples.length);
			_breasts.Add(Breasts.Generate(cup, (float)avgLength));

			_breasts[_breasts.Count - 1].nipples.setBlackNipple(blackNipples);
			_breasts[_breasts.Count - 1].nipples.setQuadNipple(quadNipples);
			_breasts[_breasts.Count - 1].nipples.setNippleStatus(nippleType);
			return true;
		}

		internal bool AddCock(CockType newCockType)
		{
			if (numCocks == MAX_COCKS)
			{
				return false;
			}
			_cocks.Add(Cock.GenerateDefaultOfType(newCockType));

			return true;
		}

		internal bool AddCock(CockType newCockType, float length, float girth)
		{
			if (numCocks >= MAX_COCKS)
			{
				return false;
			}
			_cocks.Add(Cock.Generate(newCockType, length, girth));

			return true;
		}

		internal bool AddVagina(VaginaType newVaginaType)
		{
			if (numVaginas >= MAX_VAGINAS)
			{
				return false;
			}
			_vaginas.Add(Vagina.GenerateDefaultOfType(newVaginaType));
			return true;
		}

		internal bool AddVagina(VaginaType newVaginaType, float clitLength)
		{
			if (numVaginas >= MAX_VAGINAS)
			{
				return false;
			}
			_vaginas.Add(Vagina.Generate(newVaginaType, clitLength));
			return true;
		}
		#endregion
		#region  Remove
		internal int RemoveBreastRow(int count = 1)
		{
			if (count < 0 || numBreastRows == 1 && _breasts[0].isMale)
			{
				return 0;
			}

			int oldCount = numBreastRows;
			//if over the number of breasts, reset the first one and set the number to remove to one less than the total.
			if (count >= numBreastRows)
			{
				_breasts[0].MakeMale();
				count = numBreastRows - 1;
			}
			_breasts.RemoveRange(numBreastRows - count, count);

			return oldCount - numBreastRows;
		}

		internal int RemoveExtraBreastRows()
		{
			return RemoveBreastRow(numBreastRows - 1);
		}

		internal int RemoveCock(int count = 1)
		{
			if (numCocks == 0 || count <= 0)
			{
				return 0;
			}
			int oldCount = numCocks;

			if (count > numCocks)
			{
				_cocks.Clear();
			}
			else
			{
				_cocks.RemoveRange(numCocks - count, count);
			}

			return oldCount - numCocks;
		}

		internal int RemoveExtraCocks()
		{
			return RemoveCock(numCocks - 1);
		}

		internal int RemoveAllCocks()
		{
			return RemoveCock(numCocks);
		}

		internal int RemoveVagina(int count = 1)
		{
			if (numVaginas == 0 || count <= 0)
			{
				return 0;
			}

			int oldCount = numVaginas;
			if (count >= numVaginas)
			{
				_vaginas.Clear();
				return oldCount;
			}
			else
			{
				_vaginas.RemoveRange(numVaginas - count, count);
				return oldCount - numVaginas;
			}

		}

		internal int RemoveExtraVaginas()
		{
			return RemoveVagina(numVaginas - 1);
		}

		internal int RemoveAllVaginas()
		{
			return RemoveVagina(numVaginas);
		}
		#endregion
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
				if (BiggestTitSize() > CupSize.B && BiggestCockSize() >= 6)
				{
					return Gender.HERM;
				}
				//noticable breasts and sufficiently female
				else if (BiggestTitSize() > CupSize.B && !femininity.atLeastSlightlyMasculine)
				{
					return Gender.FEMALE;
				}
				//noticable dick and sufficiently male
				else if (BiggestCockSize() >= 6 && !femininity.atLeastSlightlyFeminine)
				{
					return Gender.MALE;
				}
				//not noticable assets - go by appearance
				else if (BiggestCockSize() < 6 && BiggestTitSize() <= CupSize.B)
				{
					if (femininity.atLeastSlightlyFeminine) return Gender.FEMALE;
					else if (femininity.atLeastSlightlyMasculine) return Gender.MALE;
					return Gender.GENDERLESS;
				}
				//noticable breasts or dick, but too masculine or feminine. 
				return Gender.HERM;
			}
		}
		#endregion

		#region Biggest
		internal CupSize BiggestTitSize()
		{
			byte retVal = _breasts.Max((x) => (byte)x.cupSize);
			return (CupSize)retVal;
		}

		internal Breasts BiggestTit()
		{
			//this should work, though it's probably a perversion of what 'aggregate. means.
			//the correct name of something like this is probably fold; aggregate is the closest thing to fold in linq.

			return _breasts.Aggregate((x, y) => y.cupSize > x.cupSize ? y : x);
		}

		internal float BiggestCockSize()
		{
			return _cocks.Max(x => x.cockArea);
		}

		internal Cock BiggestCock()
		{
			return _cocks.Aggregate((x, y) => y.cockArea > x.cockArea ? y : x);
		}
		#endregion
		#region Averages
		internal float AverageCockSize()
		{
			return _cocks.Average(x => x.cockArea);
		}

		internal float AverageCockLength()
		{
			return _cocks.Average(x => x.cockLength);
		}

		internal float AverageCockGirth()
		{
			return _cocks.Average(x => x.cockGirth);
		}

		internal CupSize AverageTitSize()
		{
			return (CupSize)(byte)Math.Ceiling(_breasts.Average(x => (double)x.cupSize));
		}
		#endregion
		#region Normalize
		/// <summary>
		/// Evens out all breast rows so they are closer to the average nipple length and cup size, rounding up.
		/// large ones are shrunk, small ones grow. only does one unit of change, unless until even is set, then
		/// will completely average all values.
		/// </summary>
		/// <param name="untilEven">if true, forces all breast rows to average value, if false, only one unit.</param>
		internal void NormalizeBreasts(bool untilEven = false)
		{
			if (numBreastRows == 1)
			{
				return;
			}
			CupSize averageSize = AverageTitSize();
			if (untilEven)
			{
				foreach (var row in _breasts)
				{
					row.setCupSize(averageSize);
				}
			}
			else
			{
				foreach (var row in _breasts)
				{
					if (row.cupSize > averageSize)
					{
						row.ShrinkBreasts(1);
					}
					else if (row.cupSize < averageSize)
					{
						row.GrowBreasts(1);
					}
				}
			}
		}

		internal void NormalizeDicks(bool untilEven = false)
		{
			if (numCocks == 1)
			{
				return;
			}
			float avgLength = AverageCockLength();
			float avgGirth = AverageCockGirth();
			if (untilEven)
			{
				foreach (var cock in _cocks)
				{
					cock.SetLengthAndGirth(avgLength, avgGirth);
				}
			}
			else
			{
				foreach (var cock in _cocks)
				{
					if (cock.cockGirth < avgGirth - 0.5f)
					{
						cock.ThickenCock(0.5f);
					}
					else if (cock.cockGirth > avgGirth + 0.5f)
					{
						cock.ThinCock(0.5f);
					}
					else
					{
						cock.SetGirth(avgGirth);
					}

					if (cock.cockLength < avgLength - 1)
					{
						cock.LengthenCock(1);
					}
					else if (cock.cockLength > avgLength + 1)
					{
						cock.ShortenCock(1);
					}
					else
					{
						cock.SetLength(avgLength);
					}
				}
			}
		}
		#endregion
		#endregion
		#region Breast Aliases

		#endregion
		#region Cock Aliases

		#endregion
		#region Balls Aliases

		#endregion
		#region Vagina Aliases

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
		#region Femininity Aware and Listener
		internal void SetupFemininityAware(IFemininityAware femininityAware)
		{
			femininity.SetupFemininityAware(femininityAware);
		}

		internal bool RegisterFemininityListener(IFemininityListener listener)
		{
			return femininity.RegisterListener(listener);
		}

		internal bool DeregisterFemininityListener(IFemininityListener listener)
		{
			return femininity.DeregisterListener(listener);
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

			if (nipples[0].DoPiercingTimeNonsense(isPlayer, hoursPassed, numBreastRows > 1, out outputHelper))
			{
				nippleType = nipples[0].nippleStatus;
				outputBuilder.Append(outputHelper);
			}

			if (DoLazy(femininity, isPlayer, hoursPassed, out outputHelper))
			{
				outputBuilder.Append(outputHelper);
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
		#region Base Perk Data

		//in the rare case this matters (see femininity)
		//perks are hooked up AFTER Body Part Awares/Listeners. so 
		//if it matters, you an assume that any listeners are already active when running late perk inits.
		void IBaseStatPerkAware.GetBasePerkStats(PerkStatBonusGetter getter)
		{
			perkModifiers = getter;
			BasePerkModifiers data = getter();
			PassOnBasePerkStatsGetter(ass, getter);
			_cocks.ForEach(x => PassOnBasePerkStatsGetter(x, getter));
			_vaginas.ForEach(x => PassOnBasePerkStatsGetter(x, getter));
			_breasts.ForEach(x => PassOnBasePerkStatsGetter(x, getter));
			PassOnBasePerkStatsGetter(balls, getter);
			PassOnBasePerkStatsGetter(fertility, getter);
			PassOnBasePerkStatsGetter(femininity, getter);

			if (needsLateInit)
			{
				_vaginas.ForEach(x => x.DoLateInit(data, lateInitNew));
				_cocks.ForEach(x => x.DoLateInit(data, lateInitNew));
				_breasts.ForEach(x => x.DoLateInit(gender, data, lateInitNew));
				balls.DoLateInit(data, lateInitNew);
				femininity.DoLateInit(data);
			}
		}

		private void PassOnBasePerkStatsGetter(IBaseStatPerkAware perkAware, PerkStatBonusGetter getter)
		{
			perkAware.GetBasePerkStats(getter);
		}

		#endregion
	}
}



