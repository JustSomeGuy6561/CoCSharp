using System;
using System.Linq;
using CoC.Backend.Creatures;
using CoC.Backend.Engine.Time;
using CoC.Backend.Pregnancies;

namespace CoC.Backend.BodyParts
{
	public partial class Genitals
	{
		#region Vagina Related Constants
		//Not gonna lie, supporting double twats is a huge pain in the ass. (PHRASING! BOOM!)
		public const int MAX_VAGINAS = 2;
		#endregion

		#region Public Clit Related Members
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
		#endregion

		#region Private Vagina Related Members
		private uint missingVaginaSexCount;
		private uint missingVaginaOrgasmCount;
		private uint missingVaginaDryOrgasmCount;
		private uint missingVaginaPenetratedCount;
		#endregion

		#region Private Clit Related Members
		private uint missingClitPenetrateCount;
		private uint missingClitCockSexCount;
		private uint missingClitCockSoundCount;
		private uint missingClitCockOrgasmCount;
		private uint missingClitCockDryOrgasmCount;
		#endregion

		#region Public Vagina Related Computed Values
		public int numVaginas => _vaginas.Count;

		public uint vaginalSexCount => missingVaginaSexCount.add((uint)_vaginas.Sum(x => x.sexCount));
		public uint vaginaPenetratedCount => missingVaginaPenetratedCount.add((uint)_vaginas.Sum(x => x.totalPenetrationCount));
		public uint vaginalOrgasmCount => missingVaginaOrgasmCount.add((uint)_vaginas.Sum(x => x.orgasmCount));
		public uint vaginalDryOrgasmCount => missingVaginaDryOrgasmCount.add((uint)_vaginas.Sum(x => x.dryOrgasmCount));

		#endregion

		#region Public Clit Related Computed Values
		public uint clitCockSexCount => missingClitCockSexCount.add((uint)_vaginas.Sum(x => x.clit.asCockSexCount));
		public uint clitCockSoundedCount => missingClitCockSoundCount.add((uint)_vaginas.Sum(x => x.clit.asCockSoundCount));
		public bool clitCockVirgin => missingClitCockSexCount > 0 ? false : clitCockSexCount == 0; //the first one means no aggregate calculation, for efficiency.
		public uint clitCockOrgasmCount => missingClitCockOrgasmCount.add((uint)_vaginas.Sum(x => x.clit.asCockOrgasmCount));
		public uint clitCockDryOrgasmCount => missingClitCockDryOrgasmCount.add((uint)_vaginas.Sum(x => x.clit.asCockDryOrgasmCount));

		public uint clitUsedAsPenetratorCount => missingClitPenetrateCount.add((uint)_vaginas.Sum(x => x.clit.penetrateCount));
		#endregion

		#region Add/Remove Vaginas

		public bool AddVagina(VaginaType newVaginaType)
		{
			if (numVaginas >= MAX_VAGINAS)
			{
				return false;
			}
			var oldGender = gender;

			_vaginas.Add(new Vagina(creatureID, GetVaginaPerkWrapper(), newVaginaType));

			CheckGenderChanged(oldGender);
			return true;
		}

		public bool AddVagina(VaginaType newVaginaType, float clitLength, bool omnibus = false)
		{
			if (numVaginas >= MAX_VAGINAS)
			{
				return false;
			}
			var oldGender = gender;

			_vaginas.Add(new Vagina(creatureID, GetVaginaPerkWrapper(), newVaginaType, clitLength, omnibus: omnibus));

			CheckGenderChanged(oldGender);
			return true;
		}

		public bool AddVagina(VaginaType newVaginaType, float clitLength, VaginalLooseness looseness, VaginalWetness wetness, bool omnibus = false)
		{
			if (numVaginas >= MAX_VAGINAS)
			{
				return false;
			}
			var oldGender = gender;

			_vaginas.Add(new Vagina(creatureID, GetVaginaPerkWrapper(), newVaginaType, clitLength, looseness, wetness, true, omnibus));

			CheckGenderChanged(oldGender);
			return true;
		}

		public string AddedVaginaText()
		{
			if (numVaginas == 0 || !(creature is PlayerBase player)) return "";

			var lastVagina = _vaginas[_vaginas.Count - 1];

			return lastVagina.type.GrewVaginaText(player, (byte)(_vaginas.Count -1));
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
				missingVaginaSexCount.addIn((uint)_vaginas.Sum(x=>x.sexCount));
				missingVaginaOrgasmCount.addIn((uint)_vaginas.Sum(x=>x.orgasmCount));
				missingVaginaDryOrgasmCount.addIn((uint)_vaginas.Sum(x=>x.dryOrgasmCount));
				missingVaginaPenetratedCount.addIn((uint)_vaginas.Sum(x=>x.totalPenetrationCount));

				missingClitPenetrateCount.addIn((uint)_vaginas.Sum(x=>x.clit.penetrateCount));
				missingClitCockSexCount.addIn((uint)_vaginas.Sum(x=>x.clit.asCockSexCount));
				missingClitCockSoundCount.addIn((uint)_vaginas.Sum(x=>x.clit.asCockSoundCount));
				missingClitCockOrgasmCount.addIn((uint)_vaginas.Sum(x=>x.clit.asCockOrgasmCount));
				missingClitCockDryOrgasmCount.addIn((uint)_vaginas.Sum(x=>x.clit.asCockDryOrgasmCount));
				_vaginas.Clear();
				//
				CheckGenderChanged(oldGender);
				return oldCount;
			}
			else
			{
				missingVaginaSexCount.addIn((uint)_vaginas.Skip(numVaginas - count).Sum(x => x.sexCount));
				missingVaginaOrgasmCount.addIn((uint)_vaginas.Skip(numVaginas-count).Sum(x => x.orgasmCount));
				missingVaginaDryOrgasmCount.addIn((uint)_vaginas.Skip(numVaginas-count).Sum(x => x.dryOrgasmCount));
				missingVaginaPenetratedCount.addIn((uint)_vaginas.Skip(numVaginas-count).Sum(x => x.totalPenetrationCount));

				missingClitPenetrateCount.addIn((uint)_vaginas.Skip(numVaginas-count).Sum(x => x.clit.penetrateCount));
				missingClitCockSexCount.addIn((uint)_vaginas.Skip(numVaginas-count).Sum(x => x.clit.asCockSexCount));
				missingClitCockSoundCount.addIn((uint)_vaginas.Skip(numVaginas-count).Sum(x => x.clit.asCockSoundCount));
				missingClitCockOrgasmCount.addIn((uint)_vaginas.Skip(numVaginas-count).Sum(x => x.clit.asCockOrgasmCount));
				missingClitCockDryOrgasmCount.addIn((uint)_vaginas.Skip(numVaginas-count).Sum(x => x.clit.asCockDryOrgasmCount));
				_vaginas.RemoveRange(numVaginas - count, count);
				//
				CheckGenderChanged(oldGender);
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

			return Vagina.GenerateAggregate(creatureID, type, Clit.GenerateAggregate(creatureID, AverageClitSize(), hasClitCock, !hasCock), AverageVaginalLooseness(),
				AverageVaginalWetness(), virgin, chaste, AverageVaginalCapacity());
		}

		#endregion

		#region Vagina Sex-Related Functions
		internal bool HandleVaginalPenetration(int vaginaIndex, float length, float girth, float knotWidth, float cumAmount, bool takeVirginity, bool reachOrgasm)
		{
			return HandleVaginalPenetration(vaginaIndex, length, girth, knotWidth, null, cumAmount, 0, takeVirginity, reachOrgasm);
		}
		internal bool HandleVaginalPenetration(int vaginaIndex, float length, float girth, float knotWidth, StandardSpawnType knockupType, float cumAmount, byte virilityBonus, bool takeVirginity,
			bool reachOrgasm)
		{
			_vaginas[vaginaIndex].PenetrateVagina((ushort)(length * girth), knotWidth, takeVirginity, reachOrgasm);

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

		internal bool HandleVaginalPenetration(int vaginaIndex, Cock sourceCock, StandardSpawnType knockupType, bool reachOrgasm)
		{
			return HandleVaginalPenetration(vaginaIndex, sourceCock.length, sourceCock.girth, sourceCock.knotSize, knockupType, sourceCock.cumAmount, sourceCock.virility, true, reachOrgasm);
		}

		internal bool HandleVaginalPenetration(int vaginaIndex, Cock sourceCock, StandardSpawnType knockupType, float cumAmountOverride, bool reachOrgasm)
		{
			return HandleVaginalPenetration(vaginaIndex, sourceCock.length, sourceCock.girth, sourceCock.knotSize, knockupType, cumAmountOverride, sourceCock.virility, true, reachOrgasm);
		}

		internal bool HandleVaginalPregnancyOverride(int vaginaIndex, StandardSpawnType knockupType, float knockupRate)
		{
			if (vaginaIndex == 0 && womb.canGetPregnant(_vaginas.Count > 0))
			{
				return womb.normalPregnancy.attemptKnockUp(knockupRate, knockupType);
			}
			else if (vaginaIndex == 1 && womb.canGetPregnant(_vaginas.Count > 1))
			{
				return womb.secondaryNormalPregnancy.attemptKnockUp(knockupRate, knockupType);
			}
			return false;
		}

		//'Dry' orgasm is orgasm without stimulation.
		internal void HandleVaginaOrgasmGeneric(int vaginaIndex, bool dryOrgasm)
		{

			_vaginas[vaginaIndex].OrgasmGeneric(dryOrgasm);
		}

		internal void HandleClitCockSounding(int vaginaIndex, float penetratorLength, float penetratorWidth, float penetratorKnotSize, float cumAmount, bool reachOrgasm)
		{
			if (hasClitCock)
			{
				_vaginas[vaginaIndex].clit.AsClitCock()?.SoundCock(penetratorLength, penetratorWidth, penetratorKnotSize, reachOrgasm);
				if (reachOrgasm)
				{
					timeLastCum = GameDateTime.Now;
				}
			}
		}
		#endregion

		#region Clit Sex-Related Functions
		internal void HandleClitCockSounding(int vaginaIndex, Cock source, bool reachOrgasm)
		{
			HandleClitCockSounding(vaginaIndex, source.length, source.girth, source.knotSize, source.cumAmount, reachOrgasm);
		}

		internal void HandleClitCockSounding(int vaginaIndex, Cock source, float cumAmountOverride, bool reachOrgasm)
		{
			HandleClitCockSounding(vaginaIndex, source.length, source.girth, source.knotSize, cumAmountOverride, reachOrgasm);
		}


		internal void HandleClitCockPenetrate(int vaginaIndex, bool reachOrgasm)
		{
			if (hasClitCock)
			{
				_vaginas[vaginaIndex].clit.AsClitCock()?.DoSex(reachOrgasm);
			}
		}

		internal void DoClitCockOrgasmGeneric(int vaginaIndex, bool dryOrgasm)
		{
			if (hasClitCock)
			{
				_vaginas[vaginaIndex].clit.AsClitCock()?.OrgasmGeneric(dryOrgasm);
				timeLastCum = GameDateTime.Now;
			}
		}

		internal void HandleClitPenetrate(int vaginaIndex, bool reachOrgasm)
		{
			_vaginas[vaginaIndex].clit.DoPenetration();
			if (reachOrgasm)
			{
				_vaginas[vaginaIndex].OrgasmGeneric(false);
			}
		}
		#endregion

		#region Text
		public string AllVaginasShortDescription()
		{
			if (_vaginas.Count == 0)
			{
				return "";
			}
			else if (_vaginas.Count == 1)
			{
				return _vaginas[0].ShortDescription();
			}
			bool mismatched = _vaginas.Exists(x => x.type != _vaginas[0].type);

			return mismatched ? VaginaType.VaginaNoun(true) : _vaginas[0].ShortDescription(false);
		}

		public string AllVaginasLongDescription()
		{
			return AllVaginasDesc(false);
		}

		public string AllVaginasFullDescription()
		{
			return AllVaginasDesc(true);
		}

		public string OneVaginaOrVaginasNoun(string pronoun = "your")
		{
			if (_vaginas.Count == 0)
			{
				return "";
			}

			return CommonBodyPartStrings.OneOfDescription(_vaginas.Count > 1, pronoun, VaginaType.VaginaNoun(_vaginas.Count > 1));
		}

		public string OneVaginaOrVaginasShort(string pronoun = "your")
		{
			if (_vaginas.Count == 0)
			{
				return "";
			}

			return CommonBodyPartStrings.OneOfDescription(_vaginas.Count > 1, pronoun, AllVaginasShortDescription());
		}

		public string EachVaginaOrVaginasNoun(string pronoun = "your")
		{
			return EachVaginaOrVaginasNoun(pronoun, out bool _);
		}

		public string EachVaginaOrVaginasShort(string pronoun = "your")
		{
			return EachVaginaOrVaginasShort(pronoun, out bool _);
		}

		public string EachVaginaOrVaginasNoun(string pronoun, out bool isPlural)
		{
			isPlural = _vaginas.Count != 1;
			if (_vaginas.Count == 0)
			{
				return "";
			}

			return CommonBodyPartStrings.EachOfDescription(_vaginas.Count > 1, pronoun, VaginaType.VaginaNoun(_vaginas.Count > 1));
		}

		public string EachVaginaOrVaginasShort(string pronoun, out bool isPlural)
		{
			isPlural = _vaginas.Count != 1;
			if (_vaginas.Count == 0)
			{
				return "";
			}

			return CommonBodyPartStrings.EachOfDescription(_vaginas.Count > 1, pronoun, AllVaginasShortDescription());
		}

		public string AllVaginasPlayerDescription()
		{
			return AllVaginasPlayerText();
		}
		#endregion
	}
}
