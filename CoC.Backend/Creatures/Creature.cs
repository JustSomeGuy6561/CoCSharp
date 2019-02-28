using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Creatures
{
	public abstract class Creature
	{

		public readonly Antennae antennae;
		public readonly Arms arms;
		public readonly Body body;
		internal Epidermis primary => body._primaryEpidermis;
		internal Epidermis secondary => body._secondaryEpidermis;
		internal HairFurColors hairColor => body.hairColor;

		public bool UpdateAntennae(AntennaeType antennaeType)
		{
			return antennae.UpdateAntennae(antennaeType);
		}

		public bool RestoreAntennae()
		{
			return antennae.Restore();
		}

		public bool UpdateArms(ArmType newType)
		{
			return arms.UpdateArms(newType, primary, secondary, hairColor, body.type);
		}

		public bool RestoreArms()
		{
			return arms.Restore();
		}

	}
}
