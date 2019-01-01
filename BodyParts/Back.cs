//Back.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 1:58 AM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoC.Tools;

namespace CoC.BodyParts
{
	class Back : BodyPartBehavior<Back>
	{
		private static int indexMaker = 0;
		public Back()
		{
			_index = indexMaker++;
		}
		private readonly int _index;
		public override int index => _index;
		public override GenericDescription shortDescription {get; protected set;}
		public override CreatureDescription creatureDescription {get; protected set;}
		public override PlayerDescription playerDescription {get; protected set;}
		public override ChangeType<Back> transformFrom {get; protected set;}
	}
}
