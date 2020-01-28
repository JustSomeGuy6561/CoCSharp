using CoC.Backend;
using CoC.Backend.Perks;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Perks.SpeciesPerks
{
	//class Incorporeal : StackablePerkBase
	class Incorporeal : PerkBase
	{
		public bool affectsLegs => sourceCreature != null;

		//add other flags here for other body parts affected here. i recommend having some stack mechanic (i.e. making this a stackable perk) to allow it to spread to other parts.
		//for now, it's just nice and simple.

		//Ghost perk. Grants a possess attack during combat, and increases evade chance. Unlike most species perks, this perk is not disabled when the creature loses their
		//ghost-like traits. It can be removed artificially, of course, as is the case (as of a few patches before this port, at least) with the human tf item.
		public Incorporeal() : base()
		{
		}

		public override string Name()
		{
			throw new Backend.Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		public override string HasPerkText()
		{
			throw new Backend.Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		protected override bool KeepOnAscension => throw new Backend.Tools.InDevelopmentExceptionThatBreaksOnRelease();


		protected override void OnActivation()
		{

			//grant creature possess attack (if a combat creature)
			throw new Backend.Tools.InDevelopmentExceptionThatBreaksOnRelease();

		}

		protected override void OnRemoval()
		{
			//
			//remove possess attack from creature (if applicable)
			throw new Backend.Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
	}
}
