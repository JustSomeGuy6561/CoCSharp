using System;
using System.Linq;
using CoC.Backend.Engine.Time;
using CoC.Backend.Pregnancies;

namespace CoC.Backend.BodyParts
{
	public partial class Genitals
	{
		private uint missingVaginaSexCount;
		private uint missingVaginaOrgasmCount;
		private uint missingVaginaDryOrgasmCount;
		private uint missingVaginaPenetratedCount;

		private uint missingClitPenetrateCount;
		private uint missingClitCockSexCount;
		private uint missingClitCockSoundCount;
		private uint missingClitCockOrgasmCount;
		private uint missingClitCockDryOrgasmCount;

		public uint vaginalSexCount => missingVaginaSexCount.add((uint)_vaginas.Sum(x => x.sexCount));
		public uint vaginaPenetratedCount => missingVaginaPenetratedCount.add((uint)_vaginas.Sum(x => x.totalPenetrationCount));
		public uint vaginalOrgasmCount => missingVaginaOrgasmCount.add((uint)_vaginas.Sum(x => x.orgasmCount));
		public uint vaginalDryOrgasmCount => missingVaginaDryOrgasmCount.add((uint)_vaginas.Sum(x => x.dryOrgasmCount));

		public uint clitCockSexCount => missingClitCockSexCount.add((uint)_vaginas.Sum(x => x.clit.asCockSexCount));
		public uint clitCockSoundedCount => missingClitCockSoundCount.add((uint)_vaginas.Sum(x => x.clit.asCockSoundCount));
		public bool clitCockVirgin => missingClitCockSexCount > 0 ? false : clitCockSexCount == 0; //the first one means no aggregate calculation, for efficiency.
		public uint clitCockOrgasmCount => missingClitCockOrgasmCount.add((uint)_vaginas.Sum(x => x.clit.asCockOrgasmCount));
		public uint clitCockDryOrgasmCount => missingClitCockDryOrgasmCount.add((uint)_vaginas.Sum(x => x.clit.asCockDryOrgasmCount));

		public uint clitUsedAsPenetratorCount => missingClitPenetrateCount.add((uint)_vaginas.Sum(x => x.clit.penetrateCount));

		public bool AddVagina(VaginaType newVaginaType)
		{
			if (numVaginas >= MAX_VAGINAS)
			{
				return false;
			}
			var oldGender = gender;

			_vaginas.Add(new Vagina(creatureID, GetVaginaPerkData(), newVaginaType));
			
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

			_vaginas.Add(new Vagina(creatureID, GetVaginaPerkData(), newVaginaType, clitLength, omnibus: omnibus));

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

			_vaginas.Add(new Vagina(creatureID, GetVaginaPerkData(), newVaginaType, clitLength, looseness, wetness, true, omnibus));

			CheckGenderChanged(oldGender);
			return true;
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

		public string cockMultDescript()
		{
			throw new NotImplementedException();
		}

		public string vaginasMultiDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		public int RemoveExtraVaginas()
		{
			return RemoveVagina(numVaginas - 1);
		}

		public int RemoveAllVaginas()
		{
			return RemoveVagina(numVaginas);
		}

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

		#region Penetration
		internal bool HandleVaginalPenetration(int vaginaIndex, float length, float girth, float knotWidth, float cumAmount, bool takeVirginity, bool reachOrgasm)
		{
			return HandleVaginalPenetration(vaginaIndex, length, girth, knotWidth, null, cumAmount, 0, takeVirginity, reachOrgasm);
		}
		internal bool HandleVaginalPenetration(int vaginaIndex, float length, float girth, float knotWidth, StandardSpawnType knockupType, float cumAmount, byte virilityBonus,  bool takeVirginity,
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

		internal void HandleClitCockSounding(int vaginaIndex, Cock source, bool reachOrgasm)
		{
			HandleClitCockSounding(vaginaIndex, source.length, source.girth, source.knotSize, source.cumAmount, reachOrgasm);
		}

		internal void HandleClitCockSounding(int vaginaIndex, Cock source, float cumAmountOverride, bool reachOrgasm)
		{
			HandleClitCockSounding(vaginaIndex, source.length, source.girth, source.knotSize, cumAmountOverride, reachOrgasm);
		}
		//

		#endregion

		#region Penetrates

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

		public int CountVaginasOfType(VaginaType targetType)
		{
			return _vaginas.Sum(x => x.type == targetType ? 1 : 0);
		}

		#endregion
	}
}
