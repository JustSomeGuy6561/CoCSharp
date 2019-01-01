//Balls.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 10:57 PM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoC.Tools;

namespace CoC.BodyParts
{
	class Balls : BodyPartBehavior<Balls>
	{
		public override int index => throw new NotImplementedException();

		public override GenericDescription shortDescription {get; protected set;}
		public override CreatureDescription creatureDescription {get; protected set;}
		public override PlayerDescription playerDescription {get; protected set;}
		public override ChangeType<Balls> transformFrom {get; protected set;}
	}
}
