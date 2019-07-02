//Genitals.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 3:16 AM

using CoC.Backend.BodyParts.SpecialInteraction;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CoC.Backend.BodyParts
{
	// this is a fucking mess. 
	// need to: handle a late init to use correct mindefault values for cock, vagina, breasts, nipples.
	// implement time aware - nipple piercings changing nipple status. 
	// alias every fucking function to make values here there and everywhere increase. 
	// every time we get a new cock, set it to default length if one not provided. increase length by delta, regardless.
	// every time we get a vagina set its default wetness, looseness, and clitSize to defaults, unless provided. 
	// breasts - figure out how the fuck we're gonna add new breast rows. 


	public sealed class Genitals : SimpleSaveablePart<Genitals>, IBodyPartTimeLazy, IBaseStatPerkAware //for now all the stuff it contains is lazy, so that's all we need.
	{
		PerkStatBonusGetter perkModifiers;

		public const int MAX_COCKS = 10;
		public const int MAX_VAGINAS = 2;
		//max in game that i can find is 5, but they only ever use 4 rows.
		//apparently Fenoxo said 3 rows, but then after it went open, some shit got 4 rows.
		//i'm not being a dick and reverting that. 4 it is.
		public const int MAX_BREAST_ROWS = 4;

		private readonly bool needsLateInit;
		private readonly bool needsDelta;

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
#warning NYI
		public ushort lactationAmount { get; private set; }

#warning NYI
		public ushort multiplier { get; private set; }

#warning NYI
		public ushort hoursSinceCum { get; private set; }

		private bool alwaysAtMax => perkModifiers?.Invoke().AlwaysProducesMaxCum ?? false;

		private uint perkCumAdd => perkModifiers().BonusCumAdded;
		private float perkCumMultiply => perkModifiers().BonusCumStacked;

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
					baseValue = 1 / 4 * multiplier;
				}
				else
				{
					baseValue = 2.54f * balls.size;
					baseValue = 4.0 / 3 * Math.PI * Math.Pow(baseValue / 2, 3) * balls.count * multiplier;
				}
				if (hoursSinceCum < 12 && !alwaysAtMax) //i'd do 24 but this is Mareth, so.
				{
					baseValue *= hoursSinceCum / 12.0;
				}
				baseValue *= perkCumMultiply;
				baseValue += perkCumAdd;

				if (baseValue > int.MaxValue)
				{
					return int.MaxValue;
				}
				return (int)Math.Floor(baseValue);
			}
		}

#warning NYI
		public ushort largestVaginalCapacity { get; private set; }

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
			femininity = Femininity.GenerateFromGender(gender);
			fertility = Fertility.GenerateDefault();

			needsLateInit = true;
			needsDelta = true;

			initHelper(out cocks, out vaginas, out breasts);
		}

		private Genitals(Ass ass, Breasts[] breasts, Cock[] cocks, Balls balls, Vagina[] vaginas, Femininity femininity, Fertility fertility)
		{
			this.ass = ass;
			CleanCopy(breasts, _breasts, MAX_BREAST_ROWS);
			this.balls = balls;
			CleanCopy(cocks, _cocks, MAX_COCKS);
			CleanCopy(vaginas, _vaginas, MAX_VAGINAS);
			this.femininity = femininity;
			this.fertility = fertility;

			needsLateInit = false;
			needsDelta = true;

			initHelper(out this.cocks, out this.vaginas, out this.breasts);

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
			int min = Math.Min(source.Length, maxEntries);
			int y = 0;
			for (int x = 0; x < min; x++)
			{
				if (source[x] != null)
				{
					destination[y++] = source[x];
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
		internal static Genitals Generate(Ass ass, Breasts[] breasts, Cock[] cocks, Balls balls, Vagina[] vaginas, Femininity femininity, Fertility fertility)
		{
			return new Genitals(ass, breasts, cocks, balls, vaginas, femininity, fertility);
		}
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

		public void SetFemininity(byte newValue)
		{
			femininity.SetFemininity(newValue);
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

#warning TODO: implement this as it becomes possible.
			//increment time since last orgasm, times since last cock cum (if applicable), time since last milked (if applicable).
			//if neither are applicable, feel free to reset them.

			//If nipple is inverted or slightly inverted and is pierced.
			//decrement the pull-out timer. if it's 0 or less, set the nipple status on each breast row accordingly.
			//append the nipple pulled out by piercing to the output StringBuilder.

			//If lactating and something happened via time passing(full, slowed down, etc), set needs output to true, append 
			//the result of this to the output string builder.
			//if some status effect relating to your genitals requires output, parse it and append it.

			return outputBuilder.ToString();

		}

		private bool DoLazy(IBodyPartTimeLazy member, bool isPlayer, byte hoursPassed, out string output)
		{
			output = member.reactToTimePassing(isPlayer, hoursPassed);
			return !string.IsNullOrEmpty(output);
		}

		#endregion
		#region Base Perk Data
		void IBaseStatPerkAware.GetBasePerkStats(PerkStatBonusGetter getter)
		{
			perkModifiers = getter;
			if (needsLateInit)
			{
#error fix me.
			}
		}
		#endregion
	}
}



