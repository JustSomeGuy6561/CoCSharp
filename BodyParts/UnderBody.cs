//UnderBody.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 10:09 PM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoC.Tools;

namespace CoC.BodyParts
{
	class UnderBody : BodyPartBehavior<UnderBody>
	{
		public override int index => throw new NotImplementedException();
		protected Epidermis underbody;

		public override GenericDescription shortDescription {get; protected set;}
		public override CreatureDescription creatureDescription {get; protected set;}
		public override PlayerDescription playerDescription {get; protected set;}
		public override ChangeType<UnderBody> transformFrom {get; protected set;}
	}
}
