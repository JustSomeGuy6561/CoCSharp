﻿using CoC.Backend.Creatures;
using CoC.Frontend.Perks;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Creatures
{
	/// <summary>
	/// class that stores extra data attached to extended creatures.
	/// </summary>
	/// <remarks>This allows us to not require rewriting the code 100x, though it does require one extra step when getting the data from here</remarks>
	public sealed class ExtendedCreatureData
	{
		public readonly Creature source;

		private readonly ExtendedPerkModifiers extendedPerkData;
		internal int TotalTransformCount;
		internal bool hasDoggoWarning;
		internal bool hasHorseWarning;
		internal int horseWarningCount = 0;
		internal bool hasFerretWarning;
		internal bool hasFoxWarning;
		internal bool knowsAboutBlackEggs;
		internal bool everUsedDragonshellShield;

		public ExtendedCreatureData(Creature creature, ExtendedPerkModifiers extraPerkData)
		{
			source = creature ?? throw new ArgumentNullException(nameof(creature));
			extendedPerkData = extraPerkData ?? throw new ArgumentNullException(nameof(extraPerkData));
		}

		public sbyte deltaTransforms => extendedPerkData.numTransformsDelta.GetValue();

		public bool resistsTFBadEnds => extendedPerkData.resistsTFBadEnds.GetValue();

	}
}
