using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Engine.Time;
using CoC.Backend.Pregnancies;
using CoC.Backend.Strings;
using CoC.Backend.Tools;

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
		private uint missingVaginaSelfSexCount;
		private uint missingVaginaOrgasmCount;
		private uint missingVaginaDryOrgasmCount;
		private uint missingVaginaPenetratedCount;
		private uint missingVaginaSelfPenetratedCount;
		private uint missingVaginaNonPenetratedCount;
		private uint missingVaginaSelfNonPenetratedCount;
		private uint missingVaginaBirthCount;
		#endregion

		#region Private Clit Related Members
		private uint missingClitPenetrateCount;
		private uint missingClitSelfPenetrateCount;
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

		public VaginalLooseness minVaginalLooseness => perkData.minVaginalLooseness;
		public VaginalLooseness maxVaginalLooseness => perkData.maxVaginalLooseness;

		public VaginalWetness minVaginalWetness => perkData.minVaginalWetness;
		public VaginalWetness maxVaginalWetness => perkData.maxVaginalWetness;

		internal VaginalLooseness? defaultNewVaginaLooseness => perkData.defaultNewVaginaLooseness;
		internal VaginalWetness? defaultNewVaginaWetness => perkData.defaultNewVaginaWetness;

		public uint totalSexCount => missingVaginaSexCount.add((uint)_vaginas.Sum(x => x.totalSexCount));
		public uint selfSexCount => missingVaginaSelfSexCount.add((uint)_vaginas.Sum(x => x.selfSexCount));
		public uint totalPenetratedCount => missingVaginaPenetratedCount.add((uint)_vaginas.Sum(x => x.totalPenetrationCount));
		public uint selfPenetratedCount => missingVaginaSelfPenetratedCount.add((uint)_vaginas.Sum(x => x.selfPenetrationCount));

		public uint totalNonPenetratedCount => missingVaginaNonPenetratedCount.add((uint)_vaginas.Sum(x => x.totalNonPenetrationCount));
		public uint selfNonPenetratedCount => missingVaginaSelfNonPenetratedCount.add((uint)_vaginas.Sum(x => x.selfNonPenetrationCount));

		public uint totalOrgasmCount => missingVaginaOrgasmCount.add((uint)_vaginas.Sum(x => x.totalOrgasmCount));
		public uint totalDryOrgasmCount => missingVaginaDryOrgasmCount.add((uint)_vaginas.Sum(x => x.dryOrgasmCount));
		public uint totalBirthCount => missingVaginaBirthCount.add((uint)_vaginas.Sum(x => x.totalBirths));

		#endregion

		#region Public Clit Related Computed Values

		public uint clitUsedAsPenetratorCount => missingClitPenetrateCount.add((uint)_vaginas.Sum(x => x.clit.totalPenetrateCount));
		public uint clitUsedAsPenetratorOnSelfCount => missingClitSelfPenetrateCount.add((uint)_vaginas.Sum(x => x.clit.selfPenetrateCount));
		#endregion

		private readonly Genitals source;
		private uint currentVaginaID = 0;

		#region Constructor

		private GenitalPerkData perkData => source.perkData;

		public double relativeLust => source.relativeLust;

		public Gender gender => source.gender;

		private Creature creature => CreatureStore.GetCreatureClean(creatureID);

		internal VaginaCollection(Genitals parent) : base(parent?.creatureID ?? throw new ArgumentNullException(nameof(parent)))
		{
			source = parent;

			vaginas = new ReadOnlyCollection<Vagina>(_vaginas);
		}

		#endregion

		#region Late Init
		internal void Initialize(VaginaCreator[] vaginaCreators)
		{
			IEnumerable<VaginaCreator> vags = vaginaCreators.Where(x => x != null).Take(MAX_VAGINAS);

			foreach (VaginaCreator vag in vags)
			{
				_vaginas.Add(new Vagina(this, currentVaginaID, vag.type, vag.validClitLength, vag.looseness, vag.wetness, vag.virgin, vag.labiaPiercings, vag.clitPiercings));
				currentVaginaID++;
			}
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
				foreach (Vagina vag in _vaginas)
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

		public override bool IsIdenticalTo(VaginaCollectionData original, bool ignoreSexualMetaData)
		{
			if (original is null)
			{
				return false;
			}

			return totalBonusCapacity == original.totalBonusCapacity && standardBonusCapacity == original.standardBonusCapacity
				&& (ignoreSexualMetaData || (totalSexCount == original.totalSexCount && selfSexCount == original.selfSexCount
				&& totalPenetratedCount == original.totalPenetratedCount && selfPenetratedCount == original.selfPenetratedCount
				&& totalNonPenetratedCount == original.totalNonPenetratedCount && selfNonPenetratedCount == original.selfNonPenetratedCount
				&& totalOrgasmCount == original.totalOrgasmCount && totalDryOrgasmCount == original.totalDryOrgasmCount && totalBirthCount == original.totalBirthCount))
				&& !CollectionChanged(original, ignoreSexualMetaData);
		}

		#endregion

		public bool CollectionChanged(VaginaCollectionData original, bool ignoreSexualMetaData)
		{
			Dictionary<uint, Vagina> items = _vaginas.ToDictionary(x => x.collectionID, x => x);
			Dictionary<uint, VaginaData> dataItems = original.vaginas.ToDictionary(x => (uint)x.collectionID, x => x);

			return items.Keys.Count != dataItems.Keys.Count || items.Any(x => !dataItems.ContainsKey(x.Key) || !x.Value.IsIdenticalTo(dataItems[x.Key], ignoreSexualMetaData));
		}

		public IEnumerable<Vagina> AddedVaginas(VaginaCollectionData original)
		{
			Dictionary<uint, Vagina> items = _vaginas.ToDictionary(x => x.collectionID, x => x);
			Dictionary<uint, VaginaData> dataItems = original.vaginas.ToDictionary(x => (uint)x.collectionID, x => x);

			return items.Where(x => !dataItems.ContainsKey(x.Key)).Select(x => x.Value);
		}

		public IEnumerable<VaginaData> RemovedVaginas(VaginaCollectionData original)
		{
			Dictionary<uint, Vagina> items = _vaginas.ToDictionary(x => x.collectionID, x => x);
			Dictionary<uint, VaginaData> dataItems = original.vaginas.ToDictionary(x => (uint)x.collectionID, x => x);

			return dataItems.Where(x => !items.ContainsKey(x.Key)).Select(x => x.Value);
		}

		public IEnumerable<ValueDifference<VaginaData>> ChangedVaginas(VaginaCollectionData original, bool ignoreSexualMetaData)
		{
			Dictionary<uint, Vagina> items = _vaginas.ToDictionary(x => x.collectionID, x => x);
			Dictionary<uint, VaginaData> dataItems = original.vaginas.ToDictionary(x => (uint)x.collectionID, x => x);

			return items.Where(x => dataItems.ContainsKey(x.Key) && !x.Value.IsIdenticalTo(dataItems[x.Key], ignoreSexualMetaData))
				.Select(x => new ValueDifference<VaginaData>(dataItems[x.Key], x.Value.AsReadOnlyData()));
		}

		public IEnumerable<Vagina> UnchangedVaginas(VaginaCollectionData original, bool ignoreSexualMetaData)
		{
			Dictionary<uint, Vagina> items = _vaginas.ToDictionary(x => x.collectionID, x => x);
			Dictionary<uint, VaginaData> dataItems = original.vaginas.ToDictionary(x => (uint)x.collectionID, x => x);

			return items.Where(x => dataItems.ContainsKey(x.Key) && x.Value.IsIdenticalTo(dataItems[x.Key], ignoreSexualMetaData)).Select(x => x.Value);
		}
		#region Add/Remove Vaginas

		public bool AddVagina() => AddVagina(VaginaType.defaultValue);

		public bool AddVagina(VaginaType newVaginaType)
		{
			if (numVaginas >= MAX_VAGINAS)
			{
				return false;
			}
			Gender oldGender = gender;

			_vaginas.Add(new Vagina(this, currentVaginaID, newVaginaType));
			currentVaginaID++;

			source.CheckGenderChanged(oldGender);
			return true;
		}

		public bool AddVagina(double clitLength) => AddVagina(VaginaType.defaultValue, clitLength);
		public bool AddVagina(VaginaType newVaginaType, double clitLength)
		{
			if (numVaginas >= MAX_VAGINAS)
			{
				return false;
			}
			Gender oldGender = gender;

			_vaginas.Add(new Vagina(this, currentVaginaID, newVaginaType, clitLength));
			currentVaginaID++;

			source.CheckGenderChanged(oldGender);
			return true;
		}

		public bool AddVagina(double clitLength, VaginalLooseness looseness, VaginalWetness wetness) => AddVagina(VaginaType.defaultValue, clitLength, looseness, wetness);
		public bool AddVagina(VaginaType newVaginaType, double clitLength, VaginalLooseness looseness, VaginalWetness wetness)
		{
			if (numVaginas >= MAX_VAGINAS)
			{
				return false;
			}
			Gender oldGender = gender;

			_vaginas.Add(new Vagina(this, currentVaginaID, newVaginaType, clitLength, looseness, wetness, true));
			currentVaginaID++;

			source.CheckGenderChanged(oldGender);
			return true;
		}

		public string AddedVaginaText()
		{
			if (numVaginas == 0 || !(creature is PlayerBase player))
			{
				return "";
			}

			Vagina lastVagina = _vaginas[_vaginas.Count - 1];

			return lastVagina.type.GrewVaginaText(player, (byte)(_vaginas.Count - 1));
		}

		public int RemoveVagina(int count = 1)
		{
			if (numVaginas == 0 || count <= 0)
			{
				return 0;
			}

			int oldCount = numVaginas;
			Gender oldGender = gender;

			if (count >= numVaginas)
			{
				missingVaginaSexCount.addIn((uint)_vaginas.Sum(x => x.totalSexCount));
				missingVaginaSelfSexCount.addIn((uint)_vaginas.Sum(x => x.selfSexCount));
				missingVaginaOrgasmCount.addIn((uint)_vaginas.Sum(x => x.totalOrgasmCount));
				missingVaginaDryOrgasmCount.addIn((uint)_vaginas.Sum(x => x.dryOrgasmCount));
				missingVaginaPenetratedCount.addIn((uint)_vaginas.Sum(x => x.totalPenetrationCount));
				missingVaginaSelfPenetratedCount.addIn((uint)_vaginas.Sum(x => x.selfPenetrationCount));

				missingVaginaNonPenetratedCount.addIn((uint)_vaginas.Sum(x => x.totalNonPenetrationCount));
				missingVaginaSelfNonPenetratedCount.addIn((uint)_vaginas.Sum(x => x.selfNonPenetrationCount));

				missingClitPenetrateCount.addIn((uint)_vaginas.Sum(x => x.clit.totalPenetrateCount));
				missingClitSelfPenetrateCount.addIn((uint)_vaginas.Sum(x => x.clit.selfPenetrateCount));

				missingVaginaBirthCount.addIn((uint)_vaginas.Sum(x => x.totalBirths));

				_vaginas.Clear();

				source.CheckGenderChanged(oldGender);
				return oldCount;
			}
			else
			{
				IEnumerable<Vagina> toRemove = _vaginas.Skip(numVaginas - count);

				missingVaginaSexCount.addIn((uint)toRemove.Sum(x => x.totalSexCount));
				missingVaginaSelfSexCount.addIn((uint)toRemove.Sum(x => x.selfSexCount));

				missingVaginaOrgasmCount.addIn((uint)toRemove.Sum(x => x.totalOrgasmCount));
				missingVaginaDryOrgasmCount.addIn((uint)toRemove.Sum(x => x.dryOrgasmCount));

				missingVaginaPenetratedCount.addIn((uint)toRemove.Sum(x => x.totalPenetrationCount));
				missingVaginaSelfPenetratedCount.addIn((uint)toRemove.Sum(x => x.selfPenetrationCount));

				missingClitPenetrateCount.addIn((uint)toRemove.Sum(x => x.clit.totalPenetrateCount));
				missingClitSelfPenetrateCount.addIn((uint)toRemove.Sum(x => x.clit.selfPenetrateCount));

				missingVaginaBirthCount.addIn((uint)toRemove.Sum(x => x.totalBirths));

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
			ushort oldCap = standardBonusCapacity;
			standardBonusCapacity = standardBonusCapacity.add(amount);
			return standardBonusCapacity.subtract(oldCap);
		}

		public ushort DecreaseBonusCapacity(ushort amount)
		{
			ushort oldCap = standardBonusCapacity;
			standardBonusCapacity = standardBonusCapacity.subtract(amount);
			return oldCap.subtract(standardBonusCapacity);
		}

		public int SetBonusCapacity(ushort targetCapacity)
		{
			ushort oldCap = standardBonusCapacity;
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

		public bool HasAVaginaOfType(VaginaType type)
		{
			return _vaginas.Any(x => x.type == type);
		}

		public bool OnlyHasVaginasOfType(VaginaType type)
		{
			return _vaginas.All(x => x.type == type);
		}

		public bool HasVirginVagina()
		{
			return _vaginas.Any(x => x.isVirgin);
		}

		#endregion

		#region Clit Aggregate Functions

		public double LargestClitSize()
		{
			return _vaginas.Max(x => x.clit.length);
		}

		public double SmallestClitSize()
		{
			return _vaginas.Min(x => x.clit.length);
		}

		public double AverageClitSize()
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
			bool everPenetrated = _vaginas.Any(x => x.everPracticedVaginal == true);
			bool chaste = _vaginas.All(x => x.isChaste == true);

			return Vagina.GenerateAggregate(creatureID, type, Clit.GenerateAggregate(creatureID, AverageClitSize()),
				AverageVaginalLooseness(), AverageVaginalWetness(), AverageVaginalCapacity(), virgin, everPenetrated, chaste);
		}

		#endregion

		#region Vagina Sex-Related Functions
		internal void HandleVaginalPenetration(int vaginaIndex, double length, double girth, double knotWidth, double cumAmount, bool takeVirginity, bool reachOrgasm,
			bool sourceIsSelf)
		{
			vaginas[vaginaIndex].PenetrateVagina((ushort)(length * girth), knotWidth, takeVirginity, reachOrgasm, sourceIsSelf);
		}

		internal void HandleVaginalStimulation(int vaginaIndex, bool reachOrgasm, bool sourceIsSelf)
		{
			vaginas[vaginaIndex].StimulateVagina(reachOrgasm, sourceIsSelf);
		}

		//'Dry' orgasm is orgasm without stimulation.
		internal void HandleVaginaOrgasmGeneric(int vaginaIndex, bool dryOrgasm)
		{
			vaginas[vaginaIndex].OrgasmGeneric(dryOrgasm);
		}

		#endregion

		#region Clit Sex-Related Functions

		internal void HandleClitPenetrate(int vaginaIndex, bool reachOrgasm, bool sourceIsSelf)
		{
			vaginas[vaginaIndex].clit.DoPenetration(sourceIsSelf);
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


		public string OneVaginaOrVaginasNoun() => VaginaCollectionStrings.OneVaginaOrVaginasNoun(this);
		public string OneVaginaOrVaginasNoun(Conjugate conjugate) => VaginaCollectionStrings.OneVaginaOrVaginasNoun(this, conjugate);


		public string OneVaginaOrVaginasShort() => VaginaCollectionStrings.OneVaginaOrVaginasShort(this);
		public string OneVaginaOrVaginasShort(Conjugate conjugate) => VaginaCollectionStrings.OneVaginaOrVaginasShort(this, conjugate);


		public string EachVaginaOrVaginasNoun() => VaginaCollectionStrings.EachVaginaOrVaginasNoun(this);
		public string EachVaginaOrVaginasNoun(Conjugate conjugate) => VaginaCollectionStrings.EachVaginaOrVaginasNoun(this, conjugate);


		public string EachVaginaOrVaginasShort() => VaginaCollectionStrings.EachVaginaOrVaginasShort(this);
		public string EachVaginaOrVaginasShort(Conjugate conjugate) => VaginaCollectionStrings.EachVaginaOrVaginasShort(this, conjugate);


		public string EachVaginaOrVaginasNoun(Conjugate conjugate, out bool isPlural) => VaginaCollectionStrings.EachVaginaOrVaginasNoun(this, conjugate, out isPlural);


		public string EachVaginaOrVaginasShort(Conjugate conjugate, out bool isPlural) => VaginaCollectionStrings.EachVaginaOrVaginasShort(this, conjugate, out isPlural);

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

		public readonly ushort standardBonusCapacity;
		public readonly ushort totalBonusCapacity;

		public readonly uint totalSexCount;
		public readonly uint selfSexCount;
		public readonly uint totalPenetratedCount;
		public readonly uint selfPenetratedCount;

		public readonly uint totalNonPenetratedCount;
		public readonly uint selfNonPenetratedCount;

		public readonly uint totalOrgasmCount;
		public readonly uint totalDryOrgasmCount;

		public readonly uint totalBirthCount;

		#region Public Clit Related Computed Values

		public readonly uint clitUsedAsPenetratorCount;
		#endregion

		internal VaginaCollectionData(VaginaCollection source) : base(source?.creatureID ?? throw new ArgumentNullException(nameof(source)))
		{
			vaginas = new ReadOnlyCollection<VaginaData>(source.vaginas.Select(x => x.AsReadOnlyData()).ToList());

			standardBonusCapacity = source.standardBonusCapacity;
			totalBonusCapacity = source.totalBonusCapacity;

			totalSexCount = source.totalSexCount;
			selfSexCount = source.selfSexCount;
			totalPenetratedCount = source.totalPenetratedCount;
			selfPenetratedCount = source.selfPenetratedCount;

			totalNonPenetratedCount = source.totalNonPenetratedCount;
			selfNonPenetratedCount = source.selfNonPenetratedCount;

			totalOrgasmCount = source.totalOrgasmCount;
			totalDryOrgasmCount = source.totalDryOrgasmCount;

			totalBirthCount = source.totalBirthCount;

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

		public double LargestClitSize()
		{
			return vaginas.Max(x => x.clit.length);
		}

		public double SmallestClitSize()
		{
			return vaginas.Min(x => x.clit.length);
		}

		public double AverageClitSize()
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
			bool virgin = vaginas.All(x => x.isVirgin);
			bool everPenetrated = vaginas.Any(x => x.everPracticedVaginal);
			bool chaste = vaginas.All(x => x.isChaste);

			return Vagina.GenerateAggregate(creatureID, type, Clit.GenerateAggregate(creatureID, AverageClitSize()), AverageVaginalLooseness(),
				AverageVaginalWetness(), AverageVaginalCapacity(), virgin, everPenetrated, chaste);
		}

		#endregion

		#region Vagina Common Text
		public string AllVaginasShortDescription() => VaginaCollectionStrings.AllVaginasShortDescription(this);

		public string AllVaginasLongDescription() => VaginaCollectionStrings.AllVaginasLongDescription(this);

		public string AllVaginasFullDescription() => VaginaCollectionStrings.AllVaginasFullDescription(this);


		public string OneVaginaOrVaginasNoun() => VaginaCollectionStrings.OneVaginaOrVaginasNoun(this);
		public string OneVaginaOrVaginasNoun(Conjugate conjugate) => VaginaCollectionStrings.OneVaginaOrVaginasNoun(this, conjugate);


		public string OneVaginaOrVaginasShort() => VaginaCollectionStrings.OneVaginaOrVaginasShort(this);
		public string OneVaginaOrVaginasShort(Conjugate conjugate) => VaginaCollectionStrings.OneVaginaOrVaginasShort(this, conjugate);


		public string EachVaginaOrVaginasNoun() => VaginaCollectionStrings.EachVaginaOrVaginasNoun(this);
		public string EachVaginaOrVaginasNoun(Conjugate conjugate) => VaginaCollectionStrings.EachVaginaOrVaginasNoun(this, conjugate);


		public string EachVaginaOrVaginasShort() => VaginaCollectionStrings.EachVaginaOrVaginasShort(this);
		public string EachVaginaOrVaginasShort(Conjugate conjugate) => VaginaCollectionStrings.EachVaginaOrVaginasShort(this, conjugate);


		public string EachVaginaOrVaginasNoun(Conjugate conjugate, out bool isPlural) => VaginaCollectionStrings.EachVaginaOrVaginasNoun(this, conjugate, out isPlural);


		public string EachVaginaOrVaginasShort(Conjugate conjugate, out bool isPlural) => VaginaCollectionStrings.EachVaginaOrVaginasShort(this, conjugate, out isPlural);

		#endregion
	}
}
