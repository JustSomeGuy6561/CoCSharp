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
	class Cock : BodyPartBehavior<CockType>
	{
		public override int index => throw new NotImplementedException();
		public CockType cockType { get; protected set; }
		public override GenericDescription shortDescription {get; protected set;}
		public override CreatureDescription creatureDescription {get; protected set;}
		public override PlayerDescription playerDescription {get; protected set;}
		public override ChangeType<CockType> transformFrom {get; protected set;}

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

	class CockType : BodyPartBehavior<CockType>
	{
		public override int index => throw new NotImplementedException();

		public override GenericDescription shortDescription {get; protected set;}
		public override CreatureDescription creatureDescription {get; protected set;}
		public override PlayerDescription playerDescription {get; protected set;}
		public override ChangeType<CockType> transformFrom {get; protected set;}

		public static readonly CockType DISPLACER = new CockType();
	}
}
