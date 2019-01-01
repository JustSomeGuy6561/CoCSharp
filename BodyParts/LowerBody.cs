//LowerBody.cs
//Description:
//Author: JustSomeGuy
//12/28/2018, 10:09 PM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoC.Tools;

namespace CoC.BodyParts
{
	//FUCK THIS. I just finished horns. Can't i get something easy? TBD.
	class LowerBody : BodyPartBehavior<LowerBody>
	{
		//No magic constants. Woo!
		//not even remotely necessary, but it makes it a hell of a lot easier to debug
		//when numbers aren't magic constants. (running grep with a string is much easier
		//than a regular expression looking for legs.count = [A-Za-z]+ or something worse
		public const byte MONOPED_LEG_COUNT = 1;
		public const byte BIPED_LEG_COUNT = 2;
		public const byte QUADROPED_LEG_COUNT = 4;
		public const byte OCTOPED_LEG_COUNT = 8;
		
		Butt butt;
		Hips hips;
		QuoteUnquoteLegs legs;
		UnderBody underBody;

		readonly GenericDescriptorWithArg<LowerBody> lowerBodyDescriptor;

		public LowerBody(GenericDescription shortdesc, GenericDescriptorWithArg<LowerBody> descriptor, CreatureDescription creatureDesc, GenericDescription playerDesc, ChangeType<LowerBody> transformFunc)
		{
			butt = Butt.NewButt();
			hips = Hips.NewHips();
			legs = Legs.HUMAN;
			underBody = underBody.NONE;
			lowerBodyDescriptor = descriptor;
			shortDescription = shortdesc;
			creatureDescription = creatureDesc;
			playerDescription = playerDesc;
			transformFrom = transformFunc;
		}

		public bool isBiped()
		{
			return legs.legCount == BIPED_LEG_COUNT;
		}

		public bool isQuadruped()
		{
			return legs.legCount == QUADROPED_LEG_COUNT;
		}

		public bool isMonoped()
		{
			return legs.legCount == MONOPED_LEG_COUNT;
		}

		public bool isOctoped()
		{
			return legs.legCount == OCTOPED_LEG_COUNT;
		}

		public override int index => legs.index;

		public override GenericDescription shortDescription { get; protected set; }
		public override CreatureDescription creatureDescription {get; protected set;}
		public override PlayerDescription playerDescription { get; protected set; }
		public override ChangeType<LowerBody> transformFrom {get; protected set;}
	}



	class QuoteUnquoteLegs : BodyPartBehavior<QuoteUnquoteLegs>
	{
		readonly int legCount;
		public static implicit operator int(QuoteUnquoteLegs legs)
		{
			return legs.legCount;
		}
	}
}
