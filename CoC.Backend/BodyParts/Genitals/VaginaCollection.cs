using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Engine.Time;
using CoC.Backend.Pregnancies;

namespace CoC.Backend.BodyParts
{
	public sealed partial class VaginaCollection : SimpleSaveablePart<VaginaCollection, VaginaCollectionData>
	{
		#region Vagina Related Constants
		//Not gonna lie, supporting double twats is a huge pain in the ass. (PHRASING! BOOM!)
		public const int MAX_VAGINAS = 2;
		#endregion



		#region Private Vagina Related Members

		private readonly List<Vagina> _vaginas = new List<Vagina>();

		private uint missingVaginaSexCount;
		private uint missingVaginaOrgasmCount;
		private uint missingVaginaDryOrgasmCount;
		private uint missingVaginaPenetratedCount;
		#endregion

		#region Private Clit Related Members
		private uint missingClitPenetrateCount;
		#endregion

		public readonly ReadOnlyCollection<Vagina> vaginas;

		public Vagina this[int index]
		{
			get => _vaginas[index];
		}

		//Bonus capacity is shared between all vaginas. however, some may have different capacities than others due to their looseness/wetness values, and bonuses by vagina type.
		//IMO it'd be nice if those were shared as well, but i suppose it doesn't make sense for one hole that sees a lot of action to be as tight as the other that does not.

		//bonus capacity, for all vaginas, regardless of type or other internal values. this is the
		public ushort standardBonusCapacity { get; private set; }
		public ushort perkBonusCapacity => perkData.perkBonusVaginalCapacity;

		public ushort totalBonusCapacity => standardBonusCapacity.add(perkBonusCapacity);

		#region Public Vagina Related Computed Values


		public int numVaginas => _vaginas.Count;

		public uint vaginalSexCount => missingVaginaSexCount.add((uint)_vaginas.Sum(x => x.sexCount));
		public uint vaginaPenetratedCount => missingVaginaPenetratedCount.add((uint)_vaginas.Sum(x => x.totalPenetrationCount));
		public uint vaginalOrgasmCount => missingVaginaOrgasmCount.add((uint)_vaginas.Sum(x => x.orgasmCount));
		public uint vaginalDryOrgasmCount => missingVaginaDryOrgasmCount.add((uint)_vaginas.Sum(x => x.dryOrgasmCount));

		#endregion

		#region Public Clit Related Computed Values
		//public uint clitCockSexCount => missingClitCockSexCount.add((uint)_vaginas.Sum(x => x.clit.asCockSexCount));
		//public uint clitCockSoundedCount => missingClitCockSoundCount.add((uint)_vaginas.Sum(x => x.clit.asCockSoundCount));
		//public bool clitCockVirgin => missingClitCockSexCount > 0 ? false : clitCockSexCount == 0; //the first one means no aggregate calculation, for efficiency.
		//public uint clitCockOrgasmCount => missingClitCockOrgasmCount.add((uint)_vaginas.Sum(x => x.clit.asCockOrgasmCount));
		//public uint clitCockDryOrgasmCount => missingClitCockDryOrgasmCount.add((uint)_vaginas.Sum(x => x.clit.asCockDryOrgasmCount));

		public uint clitUsedAsPenetratorCount => missingClitPenetrateCount.add((uint)_vaginas.Sum(x => x.clit.penetrateCount));
		#endregion

		#region Constructor

		private readonly Genitals source;

		private GenitalPerkData perkData => source.perkData;

		public float relativeLust => source.relativeLust;

		public Gender gender => source.gender;

		private Creature creature => CreatureStore.GetCreatureClean(creatureID);

		internal VaginaCollection(Genitals parent) : base(parent?.creatureID ?? throw new ArgumentNullException(nameof(parent)))
		{
			source = parent;

			vaginas = new ReadOnlyCollection<Vagina>(_vaginas);
		}

		#endregion

		#region Simple Saveable

		public override VaginaCollectionData AsReadOnlyData()
		{
			return new VaginaCollectionData(this);
		}

		public override string BodyPartName()
		{
			return Name();
		}

		internal override bool Validate(bool correctInvalidData)
		{
			bool valid = true;
			if (_vaginas.Count > MAX_VAGINAS)
			{
				if (!correctInvalidData)
				{
					return false;
				}

				valid = false;
				_vaginas.RemoveRange(MAX_VAGINAS, _vaginas.Count - MAX_VAGINAS);
			}

			if (valid || correctInvalidData)
			{
				foreach (var vag in _vaginas)
				{
					valid |= vag.Validate(correctInvalidData);

					if (!valid && !correctInvalidData)
					{
						break;
					}
				}
			}

			return valid;


		}

		#endregion

		#region Add/Remove Vaginas

		public bool AddVagina(VaginaType newVaginaType)
		{
			if (numVaginas >= MAX_VAGINAS)
			{
				return false;
			}
			var oldGender = gender;

			_vaginas.Add(new Vagina(creatureID, newVaginaType));

			source.CheckGenderChanged(oldGender);
			return true;
		}

		public bool AddVagina(VaginaType newVaginaType, float clitLength)
		{
			if (numVaginas >= MAX_VAGINAS)
			{
				return false;
			}
			var oldGender = gender;

			_vaginas.Add(new Vagina(creatureID, newVaginaType, clitLength));

			source.CheckGenderChanged(oldGender);
			return true;
		}

		public bool AddVagina(VaginaType newVaginaType, float clitLength, VaginalLooseness looseness, VaginalWetness wetness)
		{
			if (numVaginas >= MAX_VAGINAS)
			{
				return false;
			}
			var oldGender = gender;

			_vaginas.Add(new Vagina(creatureID, newVaginaType, clitLength, looseness, wetness, true));

			source.CheckGenderChanged(oldGender);
			return true;
		}

		public string AddedVaginaText()
		{
			if (numVaginas == 0 || !(creature is PlayerBase player)) return "";

			var lastVagina = _vaginas[_vaginas.Count - 1];

			return lastVagina.type.GrewVaginaText(player, (byte)(_vaginas.Count - 1));
		}

		public int RemoveVagina(int count = 1)
		{
			if (numVaginas == 0 || count <= 0)
			{
				return 0;
			}

			int oldCount = numVaginas;
			var oldGender = gender;

			if (count >= numVaginas)
			{
				missingVaginaSexCount.addIn((uint)_vaginas.Sum(x => x.sexCount));
				missingVaginaOrgasmCount.addIn((uint)_vaginas.Sum(x => x.orgasmCount));
				missingVaginaDryOrgasmCount.addIn((uint)_vaginas.Sum(x => x.dryOrgasmCount));
				missingVaginaPenetratedCount.addIn((uint)_vaginas.Sum(x => x.totalPenetrationCount));

				missingClitPenetrateCount.addIn((uint)_vaginas.Sum(x => x.clit.penetrateCount));

				_vaginas.Clear();

				source.CheckGenderChanged(oldGender);
				return oldCount;
			}
			else
			{
				missingVaginaSexCount.addIn((uint)_vaginas.Skip(numVaginas - count).Sum(x => x.sexCount));
				missingVaginaOrgasmCount.addIn((uint)_vaginas.Skip(numVaginas - count).Sum(x => x.orgasmCount));
				missingVaginaDryOrgasmCount.addIn((uint)_vaginas.Skip(numVaginas - count).Sum(x => x.dryOrgasmCount));
				missingVaginaPenetratedCount.addIn((uint)_vaginas.Skip(numVaginas - count).Sum(x => x.totalPenetrationCount));

				missingClitPenetrateCount.addIn((uint)_vaginas.Skip(numVaginas - count).Sum(x => x.clit.penetrateCount));
				_vaginas.RemoveRange(numVaginas - count, count);
				//
				source.CheckGenderChanged(oldGender);
				return oldCount - numVaginas;
			}

		}

		public int RemoveExtraVaginas()
		{
			return RemoveVagina(numVaginas - 1);
		}

		public int RemoveAllVaginas()
		{
			return RemoveVagina(numVaginas);
		}
		#endregion

		#region Updates

		public bool UpdateVagina(int index, VaginaType vaginaType)
		{
			return vaginas[index].UpdateType(vaginaType);
		}

		public bool RestoreVagina(int index)
		{
			return vaginas[index].Restore();
		}
		#endregion

		#region Update Common Vagina Data

		//only affects standard capacity. perks use their own special values.
		public ushort IncreaseBonusCapacity(ushort amount)
		{
			var oldCap = standardBonusCapacity;
			standardBonusCapacity = standardBonusCapacity.add(amount);
			return standardBonusCapacity.subtract(oldCap);
		}

		public ushort DecreaseBonusCapacity(ushort amount)
		{
			var oldCap = standardBonusCapacity;
			standardBonusCapacity = standardBonusCapacity.subtract(amount);
			return oldCap.subtract(standardBonusCapacity);
		}

		public int SetBonusCapacity(ushort targetCapacity)
		{
			var oldCap = standardBonusCapacity;
			standardBonusCapacity = targetCapacity;

			return standardBonusCapacity - oldCap;
		}


		#endregion

		#region Vagina Related Aggregate Functions

		public ushort LargestVaginalCapacity()
		{
			return _vaginas.Max(x => x.VaginalCapacity());
		}

		public Vagina LargestVaginalByCapacity()
		{
			return _vaginas.MaxItem(x => x.VaginalCapacity());
		}
		public ushort SmallestVaginalCapacity()
		{
			return _vaginas.Min(x => x.VaginalCapacity());
		}

		public Vagina SmallestVaginalByCapacity()
		{
			return _vaginas.MinItem(x => x.VaginalCapacity());
		}

		public ushort AverageVaginalCapacity()
		{
			return (ushort)Math.Round(_vaginas.Average(x => x.VaginalCapacity()));
		}

		public VaginalWetness LargestVaginalWetness()
		{
			return _vaginas.Max(x => x.wetness);
		}

		public Vagina LargestVaginalByWetness()
		{
			return _vaginas.MaxItem(x => (byte)x.wetness);
		}
		public VaginalWetness SmallestVaginalWetness()
		{
			return _vaginas.Min(x => x.wetness);
		}

		public Vagina SmallestVaginalByWetness()
		{
			return _vaginas.MinItem(x => (byte)x.wetness);
		}

		public VaginalWetness AverageVaginalWetness()
		{
			return (VaginalWetness)(byte)Math.Round(_vaginas.Average(x => (double)(byte)x.wetness));
		}

		public VaginalLooseness LargestVaginalLooseness()
		{
			return _vaginas.Max(x => x.looseness);
		}

		public Vagina LargestVaginalByLooseness()
		{
			return _vaginas.MaxItem(x => (byte)x.looseness);
		}
		public VaginalLooseness SmallestVaginalLooseness()
		{
			return _vaginas.Min(x => x.looseness);
		}

		public Vagina SmallestVaginalByLooseness()
		{
			return _vaginas.MinItem(x => (byte)x.looseness);
		}

		public VaginalLooseness AverageVaginalLooseness()
		{
			return (VaginalLooseness)(byte)Math.Round(_vaginas.Average(x => (double)(byte)x.looseness));
		}

		public Vagina LargestVaginaByClitSize()
		{
			return _vaginas.MaxItem(x => x.clit.length);
		}

		public Vagina SmallestVaginaByClitSize()
		{
			return _vaginas.MinItem(x => x.clit.length);
		}

		public int CountVaginasOfType(VaginaType vaginaType)
		{
			return _vaginas.Sum(x => x.type == vaginaType ? 1 : 0);
		}

		#endregion

		#region Clit Aggregate Functions

		public float LargestClitSize()
		{
			return _vaginas.Max(x => x.clit.length);
		}

		public float SmallestClitSize()
		{
			return _vaginas.Min(x => x.clit.length);
		}

		public float AverageClitSize()
		{
			return _vaginas.Average(x => x.clit.length);
		}

		public Clit LargestClit()
		{
			return _vaginas.MaxItem(x => x.clit.length).clit;
		}

		public Clit SmallestClit()
		{
			return _vaginas.MinItem(x => x.clit.length).clit;
		}

		public VaginaData AverageVagina()
		{
			if (_vaginas.Count == 0)
			{
				return null;
			}

			VaginaType type = _vaginas[0].type;
			bool virgin = _vaginas.All(x => x.isVirgin == true);
			bool chaste = _vaginas.All(x => x.everPracticedVaginal == true);

			return Vagina.GenerateAggregate(creatureID, type, Clit.GenerateAggregate(creatureID, AverageClitSize()),
				AverageVaginalLooseness(), AverageVaginalWetness(), virgin, chaste, AverageVaginalCapacity());
		}

		#endregion

		#region Vagina Sex-Related Functions
		internal void HandleVaginalPenetration(int vaginaIndex, float length, float girth, float knotWidth, float cumAmount, bool takeVirginity, bool reachOrgasm)
		{
			vaginas[vaginaIndex].PenetrateVagina((ushort)(length * girth), knotWidth, takeVirginity, reachOrgasm);
		}

		//'Dry' orgasm is orgasm without stimulation.
		internal void HandleVaginaOrgasmGeneric(int vaginaIndex, bool dryOrgasm)
		{
			vaginas[vaginaIndex].OrgasmGeneric(dryOrgasm);
		}

		#endregion

		#region Clit Sex-Related Functions

		internal void HandleClitPenetrate(int vaginaIndex, bool reachOrgasm)
		{
			vaginas[vaginaIndex].clit.DoPenetration();
			if (reachOrgasm)
			{
				vaginas[vaginaIndex].OrgasmGeneric(false);
			}
		}
		#endregion

		#region Vagina Common Text
		public string AllVaginasShortDescription() => VaginaCollectionStrings.AllVaginasShortDescription(this);

		public string AllVaginasLongDescription() => VaginaCollectionStrings.AllVaginasLongDescription(this);

		public string AllVaginasFullDescription() => VaginaCollectionStrings.AllVaginasFullDescription(this);


		public string OneVaginaOrVaginasNoun(string pronoun = "your") => VaginaCollectionStrings.OneVaginaOrVaginasNoun(this, pronoun);


		public string OneVaginaOrVaginasShort(string pronoun = "your") => VaginaCollectionStrings.OneVaginaOrVaginasShort(this, pronoun);


		public string EachVaginaOrVaginasNoun(string pronoun = "your") => VaginaCollectionStrings.EachVaginaOrVaginasNoun(this, pronoun);


		public string EachVaginaOrVaginasShort(string pronoun = "your") => VaginaCollectionStrings.EachVaginaOrVaginasShort(this, pronoun);


		public string EachVaginaOrVaginasNoun(string pronoun, out bool isPlural) => VaginaCollectionStrings.EachVaginaOrVaginasNoun(this, pronoun, out isPlural);


		public string EachVaginaOrVaginasShort(string pronoun, out bool isPlural) => VaginaCollectionStrings.EachVaginaOrVaginasShort(this, pronoun, out isPlural);

		#endregion

		#region Vagina Collection Exclusive Text

		internal string AllVaginasPlayerDescription()
		{
			if (creature is PlayerBase player)
			{
				return AllVaginasPlayerText(player);
			}
			else
			{
				return "";
			}
		}

		#endregion

		internal void Initialize(VaginaCreator[] vaginaCreators)
		{
			_vaginas.AddRange(vaginaCreators.Where(x => x != null).Take(MAX_VAGINAS).Select(x => new Vagina(creatureID, x.type, x.validClitLength, x.looseness, x.wetness, x.virgin,
				x.labiaPiercings, x.clitPiercings)));
		}
	}

	public sealed partial class VaginaCollectionData : SimpleData, IVaginaCollection<VaginaData>
	{
		//public readonly bool hasClitCock;

		public ReadOnlyCollection<VaginaData> vaginas;

		public VaginaData this[int index]
		{
			get => vaginas[index];
		}

		public int numVaginas => vaginas.Count;

		ReadOnlyCollection<VaginaData> IVaginaCollection<VaginaData>.vaginas => vaginas;

		public readonly uint vaginalSexCount;
		public readonly uint vaginaPenetratedCount;
		public readonly uint vaginalOrgasmCount;
		public readonly uint vaginalDryOrgasmCount;

		#region Public Clit Related Computed Values

		public readonly uint clitUsedAsPenetratorCount;
		#endregion

		public readonly bool clitCockActive;

		internal VaginaCollectionData(VaginaCollection source) : base(source?.creatureID ?? throw new ArgumentNullException(nameof(source)))
		{
			this.vaginas = new ReadOnlyCollection<VaginaData>(source.vaginas.Select(x => x.AsReadOnlyData()).ToList());
			this.vaginalSexCount = source.vaginalSexCount;
			this.vaginaPenetratedCount = source.vaginaPenetratedCount;
			this.vaginalOrgasmCount = source.vaginalOrgasmCount;
			this.vaginalDryOrgasmCount = source.vaginalDryOrgasmCount;
			this.clitUsedAsPenetratorCount = source.clitUsedAsPenetratorCount;
		}

		#region Vagina Related Aggregate Functions

		public ushort LargestVaginalCapacity()
		{
			return vaginas.Max(x => x.capacity);
		}

		public VaginaData LargestVaginalByCapacity()
		{
			return vaginas.MaxItem(x => x.capacity);
		}
		public ushort SmallestVaginalCapacity()
		{
			return vaginas.Min(x => x.capacity);
		}

		public VaginaData SmallestVaginalByCapacity()
		{
			return vaginas.MinItem(x => x.capacity);
		}

		public ushort AverageVaginalCapacity()
		{
			return (ushort)Math.Round(vaginas.Average(x => x.capacity));
		}

		public VaginalWetness LargestVaginalWetness()
		{
			return vaginas.Max(x => x.wetness);
		}

		public VaginaData LargestVaginalByWetness()
		{
			return vaginas.MaxItem(x => (byte)x.wetness);
		}
		public VaginalWetness SmallestVaginalWetness()
		{
			return vaginas.Min(x => x.wetness);
		}

		public VaginaData SmallestVaginalByWetness()
		{
			return vaginas.MinItem(x => (byte)x.wetness);
		}

		public VaginalWetness AverageVaginalWetness()
		{
			return (VaginalWetness)(byte)Math.Round(vaginas.Average(x => (double)(byte)x.wetness));
		}

		public VaginalLooseness LargestVaginalLooseness()
		{
			return vaginas.Max(x => x.looseness);
		}

		public VaginaData LargestVaginalByLooseness()
		{
			return vaginas.MaxItem(x => (byte)x.looseness);
		}
		public VaginalLooseness SmallestVaginalLooseness()
		{
			return vaginas.Min(x => x.looseness);
		}

		public VaginaData SmallestVaginalByLooseness()
		{
			return vaginas.MinItem(x => (byte)x.looseness);
		}

		public VaginalLooseness AverageVaginalLooseness()
		{
			return (VaginalLooseness)(byte)Math.Round(vaginas.Average(x => (double)(byte)x.looseness));
		}

		public VaginaData LargestVaginaByClitSize()
		{
			return vaginas.MaxItem(x => x.clit.length);
		}

		public VaginaData SmallestVaginaByClitSize()
		{
			return vaginas.MinItem(x => x.clit.length);
		}

		public int CountVaginasOfType(VaginaType vaginaType)
		{
			return vaginas.Sum(x => x.type == vaginaType ? 1 : 0);
		}

		#endregion

		#region Clit Aggregate Functions

		public float LargestClitSize()
		{
			return vaginas.Max(x => x.clit.length);
		}

		public float SmallestClitSize()
		{
			return vaginas.Min(x => x.clit.length);
		}

		public float AverageClitSize()
		{
			return vaginas.Average(x => x.clit.length);
		}

		public ClitData LargestClit()
		{
			return vaginas.MaxItem(x => x.clit.length).clit;
		}

		public ClitData SmallestClit()
		{
			return vaginas.MinItem(x => x.clit.length).clit;
		}

		public VaginaData AverageVagina()
		{
			if (vaginas.Count == 0)
			{
				return null;
			}

			VaginaType type = vaginas[0].type;
			bool virgin = vaginas.All(x => x.isVirgin == true);
			bool chaste = vaginas.All(x => x.everPracticedVaginal == true);

			return Vagina.GenerateAggregate(creatureID, type, Clit.GenerateAggregate(creatureID, AverageClitSize()), AverageVaginalLooseness(),
				AverageVaginalWetness(), virgin, chaste, AverageVaginalCapacity());
		}

		#endregion

		#region Vagina Text
		public string AllVaginasShortDescription() => VaginaCollectionStrings.AllVaginasShortDescription(this);

		public string AllVaginasLongDescription() => VaginaCollectionStrings.AllVaginasLongDescription(this);

		public string AllVaginasFullDescription() => VaginaCollectionStrings.AllVaginasFullDescription(this);


		public string OneVaginaOrVaginasNoun(string pronoun = "your") => VaginaCollectionStrings.OneVaginaOrVaginasNoun(this, pronoun);


		public string OneVaginaOrVaginasShort(string pronoun = "your") => VaginaCollectionStrings.OneVaginaOrVaginasShort(this, pronoun);


		public string EachVaginaOrVaginasNoun(string pronoun = "your") => VaginaCollectionStrings.EachVaginaOrVaginasNoun(this, pronoun);


		public string EachVaginaOrVaginasShort(string pronoun = "your") => VaginaCollectionStrings.EachVaginaOrVaginasShort(this, pronoun);


		public string EachVaginaOrVaginasNoun(string pronoun, out bool isPlural) => VaginaCollectionStrings.EachVaginaOrVaginasNoun(this, pronoun, out isPlural);


		public string EachVaginaOrVaginasShort(string pronoun, out bool isPlural) => VaginaCollectionStrings.EachVaginaOrVaginasShort(this, pronoun, out isPlural);

		VaginaData IVaginaCollection<VaginaData>.AverageVagina()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
