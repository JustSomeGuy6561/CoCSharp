using CoC.Backend.Pregnancies;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts
{
	partial class Genitals
	{
		//ass is readonly, always exists. we don't need any alias magic for it.
		#region Public Ass Related Computed Values

		public uint analSexCount => ass.sexCount;
		public uint analPenetrationCount => ass.penetrateCount;
		public uint analOrgasmCount => ass.orgasmCount;
		public uint analDryOrgasmCount => ass.dryOrgasmCount;
		#endregion

		#region Ass Sex Related Functions

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


		#endregion
	}
}
