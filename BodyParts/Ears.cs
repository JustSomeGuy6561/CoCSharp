//Ears.cs
//Description:
//Author: JustSomeGuy
//12/27/2018, 12:22 AM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoC.Tools;
namespace CoC.BodyParts
{
	/*
	 * Here's to looking half of this shit up. Following your ear from bottom up around the edge:
	 * LOBE, AURICLE, HELIX, ANTI_HELIX.
	 * LOBE: Lowest part of ear. AURICLE: Thin part of the outside of the ear around the middle
	 * HELIX: the thin upper outside part of your ear. ANTI-HELIX: the thin upper inside part of your ear.
	 * I had more that i looked up but i can't realisticly make them part of anything not human.
	 * I am aware everyone's ears are different and you may have tiny lobes and long-ass helix bits
	 * (which i suppose is also true for elfin ears); i did what i could, sue me. 
	 */
	public enum EAR_PIERCING
	{    
		LEFT_LOBE_1, LEFT_LOBE_2, LEFT_LOBE_3, LEFT_UPPER_LOBE,
		LEFT_AURICAL_1, LEFT_AURICAL_2, LEFT_AURICAL_3, LEFT_AURICAL_4,
		LEFT_HELIX_1, LEFT_HELIX_2,	LEFT_HELIX_3, LEFT_HELIX_4,
		LEFT_ANTI_HELIX,
		RIGHT_LOBE_1, RIGHT_LOBE_2, RIGHT_LOBE_3, RIGHT_UPPER_LOBE,
		RIGHT_AURICAL_1, RIGHT_AURICAL_2, RIGHT_AURICAL_3, RIGHT_AURICAL_4,
		RIGHT_HELIX_1, RIGHT_HELIX_2, RIGHT_HELIX_3, RIGHT_HELIX_4,
		RIGHT_ANTI_HELIX
	}
	class Ears : PiercableBodyPart<Ears, EarType, EAR_PIERCING>
	{
		public override EarType type { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }
		protected override PiercingFlags piercingFlags { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public override bool canPierceAtLocation(EAR_PIERCING piercingLocation)
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
	}
	class EarType : PiercableBodyPartBehavior<EarType, Ears, EAR_PIERCING>
	{
		private static int indexMaker = 0;

		private readonly int _index;
		protected EarType(string desc)
		{
			descriptor = desc;
			_index = indexMaker++;
		}

		public override int index => throw new NotImplementedException();

		public override GenericDescription shortDescription { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }
		public override CreatureDescription<Ears> creatureDescription { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }
		public override PlayerDescription<Ears> playerDescription { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }
		public override ChangeType<EarType> transformFrom { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }
		public override ChangeType<EarType> restoreString { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

		public static readonly Ears HUMAN = new Ears(SpeciesName.HUMAN);
		public static readonly Ears HORSE = new Ears(SpeciesName.HORSE);
		public static readonly Ears DOG = new Ears(SpeciesName.DOG);
		public static readonly Ears COW = new Ears(SpeciesName.COW);
		public static readonly Ears ELFIN = new Ears(SpeciesName.ELFIN);
		public static readonly Ears CAT = new Ears(SpeciesName.CAT);
		public static readonly Ears LIZARD = new Ears(SpeciesName.LIZARD);
		public static readonly Ears BUNNY = new Ears(SpeciesName.BUNNY);
		public static readonly Ears KANGAROO = new Ears(SpeciesName.KANGAROO);
		public static readonly Ears FOX = new Ears(SpeciesName.FOX);
		public static readonly Ears DRAGON = new Ears(SpeciesName.DRAGON);
		public static readonly Ears RACCOON = new Ears(SpeciesName.RACCOON);
		public static readonly Ears MOUSE = new Ears(SpeciesName.MOUSE);
		public static readonly Ears FERRET = new Ears(SpeciesName.FERRET);
		public static readonly Ears PIG = new Ears(SpeciesName.PIG);
		public static readonly Ears RHINO = new Ears(SpeciesName.RHINO);
		public static readonly Ears ECHIDA = new Ears(SpeciesName.ECHIDNA);
		public static readonly Ears DEER = new Ears(SpeciesName.DEER);
		public static readonly Ears WOLF = new Ears(SpeciesName.WOLF);
		public static readonly Ears SHEEP = new Ears(SpeciesName.SHEEP);
		public static readonly Ears IMP = new Ears(SpeciesName.IMP);
		public static readonly Ears COCKATRICE = new Ears(SpeciesName.COCKATRICE);
		public static readonly Ears RED_PANDA = new Ears(SpeciesName.RED_PANDA);


	}
}
