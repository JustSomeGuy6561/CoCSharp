//Cock.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 10:55 PM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoC.Tools;

namespace CoC.BodyParts
{
	public enum COCK_PIERCING
	{
		ALBERT,
		FRENUM_1, FRENUM_2, FRENUM_3, FRENUM_4,
		FRENUM_5, FRENUM_6, FRENUM_7, FRENUM_8
	}
	class Cock : PiercableBodyPart<Cock, CockType, COCK_PIERCING>
	{
		protected Cock(CockType type, PiercingFlags flags)
		{
			this.type = type;
		}

		protected override PiercingFlags piercingFlags { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public override CockType type { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

		#region Compare stuff
		//---------------------------------------------
		//Because of the convenience shit. Standard compares that need to be explicitly defined because
		//the non-standard ones are too.
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public bool Equals(Cock other)
		{
			return this == other;
		}

		public static bool operator ==(Cock first, Cock second)
		{
			throw new NotImplementedException();
		}

		public static bool operator !=(Cock first, Cock second)
		{
			throw new NotImplementedException();
		}

		//Convenience. Because everyone loves that shit
		public bool Equals(CockType other)
		{
			return this == other;
		}

		public override bool canPierceAtLocation(COCK_PIERCING piercingLocation)
		{
			throw new NotImplementedException();
		}

		public override void Restore()
		{
			throw new NotImplementedException();
		}

		public override void RestoreAndDisplayMessage(Player player)
		{
			throw new NotImplementedException();
		}

		public static bool operator ==(Cock first, CockType second)
		{
			return first.cockType == second;
		}

		public static bool operator !=(Cock first, CockType second)
		{
			return first.cockType != second;
		}
		#endregion
	}

	class CockType : PiercableBodyPartBehavior<CockType, Cock, COCK_PIERCING>
	{
		public override int index => throw new NotImplementedException();

		public override GenericDescription shortDescription { get; protected set; }
		public override CreatureDescription creatureDescription { get; protected set; }
		public override PlayerDescription playerDescription { get; protected set; }
		public override ChangeType<CockType> transformFrom { get; protected set; }

		public static readonly CockType DISPLACER = new CockType();
	}
}
