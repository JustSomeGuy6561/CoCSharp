////Genitals.cs
////Description:
////Author: JustSomeGuy
////1/5/2019, 3:16 AM
//using CoC.Backend.Tools;
//using System;
//using System.Linq;
using CoC.Backend.Engine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CoC.Backend.BodyParts
{
	public sealed class Genitals : SimpleSaveablePart<Genitals>, ITimeAwareWithOutput  //, IPerkAware
	{
		public const int MAX_COCKS = 10;
		public const int MAX_VAGINAS = 2;
		//max in game that i can find is 5, but they only ever use 4 rows.
		//apparently Fenoxo said 3 rows, but then after it went open, some shit got 4 rows.
		//i'm not being a dick and reverting that. 4 it is.
		public const int MAX_BREAST_ROWS = 4;

		public readonly Ass ass;

		//honestly, this is too much power for what we need, but it's a hell of a lot simpler to implement.
		//it's a glorified array, but minus the hassle of keeping track of what's actually not null.
		private readonly List<Breasts> _breasts = new List<Breasts>(MAX_BREAST_ROWS);
		private readonly List<Cock> _cocks = new List<Cock>(MAX_COCKS);
		private readonly List<Vagina> _vaginas = new List<Vagina>(MAX_VAGINAS);

		public int numCocks => _cocks.Count;
		public int numBreastRows => _breasts.Count;
		public int numVaginas => _vaginas.Count;

		public readonly Balls balls;
		public readonly Femininity femininity; //make sure to cap this if not androgynous perk.
		public readonly Fertility fertility;

		public ReadOnlyCollection<Cock> cocks => Array.AsReadOnly(_cocks.ToArray());
		public ReadOnlyCollection<Vagina> vaginas => Array.AsReadOnly(_vaginas.ToArray());
		public ReadOnlyCollection<Breasts> breasts => Array.AsReadOnly(_breasts.ToArray());





		//fertility gets a class because it's not just an int, it also has a bool that determines if the creature is artificially infertile
		//(sand witch pill, contraceptives, whatever.)

		//nipple effects
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

		//breasts
		public ushort lactationAmount { get; private set; }
		//public CupSize smallestBreast =>
		//public CupSize largestBreast =>

		//cocks
		//public ushort longestCock => cocks.longestCock;
		//public ushort shortestCock => cocks.shortestCock;

		public ushort cumAmount { get; private set; }

		//vagoos
		public ushort largestVaginalCapacity { get; private set; }
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


		private bool needsOutput = false;
		private Genitals(Gender gender)
		{
			ass = Ass.GenerateDefault();
			_breasts.Add(Breasts.GenerateFromGender(gender));
			balls = Balls.GenerateFromGender(gender);
			_cocks.Add(Cock.GenerateFromGender(gender));
			_vaginas.Add(Vagina.GenerateFromGender(gender));
			femininity = Femininity.GenerateFromGender(gender);
			fertility = Fertility.GenerateDefault();
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

		bool ITimeAwareWithOutput.RequiresOutput => needsOutput;

		void ITimeAware.ReactToTimePassing(byte hoursPassed)
		{
			needsOutput = false;
			output.Clear();

			//i have no clue how this would work for multi-snatch configs. 
			foreach (var vagina in _vaginas)
			{
				DoTimeAware(vagina, hoursPassed);
			}
			DoTimeAware(ass, hoursPassed);

#warning TODO: implement this as it becomes possible.
			//increment time since last orgasm, times since last cock cum (if applicable), time since last milked (if applicable).
			//if neither are applicable, feel free to reset them.

			//If nipple is inverted or slightly inverted and is pierced.
			//decrement the pull-out timer. if it's 0 or less, set the nipple status on each breast row accordingly.
			//append the nipple pulled out by piercing to the output StringBuilder.

			//If lactating and something happened via time passing(full, slowed down, etc), set needs output to true, append 
			//the result of this to the output string builder.
			//if some status effect relating to your genitals requires output, parse it and append it.
		}

		private void DoTimeAware(ITimeAware member, byte hoursPassed)
		{
			member.ReactToTimePassing(hoursPassed);
			if (member is ITimeAwareWithOutput memberWithOutput)
			{
				bool hasOutput = memberWithOutput.RequiresOutput;
				needsOutput |= hasOutput;
				if (hasOutput)
				{
					output.Append(memberWithOutput.Output());
				}
			}
		}

		private readonly StringBuilder output = new StringBuilder();

		string ITimeAwareWithOutput.Output()
		{
			return output.ToString();
		}

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

		internal bool MakeFemale()
		{
			if (numCocks == 0 && !hasClitCock)
			{
				return false;
			}
			RemoveCock(numCocks);

			hasClitCock = false;
			return true;
		}

		internal bool MakeMale()
		{
			if (numVaginas == 0 && numBreastRows == 1 && _breasts[0].isMale)
			{
				return false;
			}
			RemoveVagina(numVaginas);
			RemoveBreastRow(numBreastRows);
			return true;
		}

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

		internal override bool Validate(bool correctDataIfInvalid = false)
		{
			//#error FIX ME!
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
	}
}



