using CoC.Backend;
using CoC.Backend.Perks;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Perks
{
	//rubber skin is now a full perk, not some fake skin type to be parsed as -99.
	//it was originally a skin type, but that got to be a huge pain because again, it's some pseudo skin type.
	//sooo... it's now a perk. when you have the perk, your body secrets a natural, breathable rubber, but you have full control over it, so it only does so when you want it to.
	//this perk is stackable, with each stack increasing the thickness and changing the description. i suppose it may be possible to OD and lose control of it or something, which
	//could be a bad end or have some sort of other negative effect like making it hard/impossible to eat or something, idk, but for now, it's pretty passive.
	//
	// This does mean it needs to be checked for manually, though this is still better than the rubber string check from before. unfortunately, well, almost no scenes in the current
	//implementation handle this - not my fault, the original game was this way, i've done what i can to add it in where it makes sense. There was one fight that had a humerous
	//use of the rubber perk to absorb some fall damage, and that's still a thing. some scenes may be implemented in the future that let you go all rubber suit fetish for
	//sex/roleplay/whatever, or have unique interactions with npcs who are aware of your natural rubber skin suit. I suppose you could also get an attack that lets you use your rubber
	//to suffocate enemies, and also add it in for flavor text for some tease attacks, but that's marked for future enhancements, not current core goals.
	public sealed class RubberySkin : StackablePerk
	{
		public RubberySkin() : base(Name, HavePerkText)
		{
		}

		private static string Name()
		{
			throw new NotImplementedException();
		}

		private static string HavePerkText()
		{
			throw new NotImplementedException();
		}

		protected override bool KeepOnAscension => throw new NotImplementedException();

		protected override void OnActivation()
		{
			throw new NotImplementedException();
		}

		protected override void OnRemoval()
		{
			throw new NotImplementedException();
		}

		protected override bool OnStackDecreaseAttempt()
		{
			throw new NotImplementedException();
		}

		protected override bool OnStackIncreaseAttempt()
		{
			throw new NotImplementedException();
		}
	}
}
