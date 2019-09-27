using System;
using System.Linq;
using CoC.Backend.Pregnancies;

namespace CoC.Backend.BodyParts
{
	public partial class Genitals
	{
		public uint timesHadVaginalSex => missingCockSexCount + (uint)_vaginas.Sum(x => x.numTimesVaginal);
		private uint missingVaginaSexCount;

		internal bool AddVagina(VaginaType newVaginaType)
		{
			if (numVaginas >= MAX_VAGINAS)
			{
				return false;
			}
			_vaginas.Add(new Vagina(creatureID, GetVaginaPerkData(), newVaginaType));
			return true;
		}

		internal bool AddVagina(VaginaType newVaginaType, float clitLength, bool omnibus = false)
		{
			if (numVaginas >= MAX_VAGINAS)
			{
				return false;
			}
			_vaginas.Add(new Vagina(creatureID, GetVaginaPerkData(), newVaginaType, clitLength, omnibus: omnibus));
			return true;
		}

		internal bool AddVagina(VaginaType newVaginaType, float clitLength, VaginalLooseness looseness, VaginalWetness wetness, bool omnibus = false)
		{
			if (numVaginas >= MAX_VAGINAS)
			{
				return false;
			}
			_vaginas.Add(new Vagina(creatureID, GetVaginaPerkData(), newVaginaType, clitLength, looseness, wetness, true, omnibus));
			return true;
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
		public bool HandleVaginalPenetration(int vaginaIndex, float length, float girth, float knotWidth, bool reachOrgasm)
		{
			return HandleVaginalPenetration(vaginaIndex, length, girth, knotWidth, null, 0, reachOrgasm);
		}
		public bool HandleVaginalPenetration(int vaginaIndex, float length, float girth, float knotWidth, SpawnType knockupType, byte virilityBonus, bool reachOrgasm)
		{
			

			_vaginas[vaginaIndex].PenetrateVagina((ushort)(length * girth), knotWidth, true);
			if (vaginaIndex == 0 && womb.canGetPregnant(true))
			{ 
				return womb.normalPregnancy.attemptKnockUp(knockupRate(virilityBonus), knockupType);
			}
			else if (vaginaIndex == 1 && womb.canGetSecondaryNormalPregnant(true))
			{
				return womb.secondaryNormalPregnancy.attemptKnockUp(knockupRate(virilityBonus), knockupType);

			}
			return false;
		}



		public bool HandleVaginalPenetration(int vaginaIndex, Cock sourceCock, SpawnType knockupType, bool reachOrgasm)
		{
			return HandleVaginalPenetration(vaginaIndex, sourceCock.length, sourceCock.girth, sourceCock.knotSize, knockupType, sourceCock.virility, reachOrgasm);
		}
		#endregion

		#region Penetrated

		internal void HandleClitCockPenetrate(int penetratorVaginaIndex, bool penetratorReachesOrgasm)
		{
			throw new NotImplementedException();
		}

		internal void HandleClitPenetrate(int penetratorVaginaIndex, bool penetratorReachesOrgasm)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
