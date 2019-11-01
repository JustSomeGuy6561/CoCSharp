using CoC.Backend.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Races
{
	public static class SpeciesHelpers
	{
		public static byte ImpScore(this Creature creature)
		{
			return Species.IMP.Score(creature);
		}

		public static byte DogScore(this Creature creature)
		{
			return Species.DOG.Score(creature);
		}
	}
}
