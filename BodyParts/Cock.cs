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
	public class Cock : PiercableBodyPart<Cock, CockType, COCK_PIERCING>
	{
	}

	public class CockType : PiercableBodyPartBehavior<CockType, Cock, COCK_PIERCING>
	{
		private static int indexMaker = 0;
		public override int index => _index;
		private readonly int _index;
		//public static readonly CockType DISPLACER = new CockType();

		protected CockType(//any cocktype specific values.
			GenericDescription shortDesc, CreatureDescription<Cock> creatureDesc, PlayerDescription<Cock> playerDesc, 
			ChangeType<Cock> transform, ChangeType<Cock> restore) : base(shortDesc, creatureDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
		}
	}
}
