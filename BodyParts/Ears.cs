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
	class Ears : BodyPartBehavior
	{
		private static int indexMaker = 0;
		public override string GetDescriptor()
		{
			return descriptor;
		}

		protected Ears(string desc)
		{
			descriptor = desc;
			index = indexMaker++;
		}

		public static readonly Ears HUMAN      = new Ears(SpeciesName.HUMAN);
		public static readonly Ears HORSE      = new Ears(SpeciesName.HORSE);
		public static readonly Ears DOG        = new Ears(SpeciesName.DOG);
		public static readonly Ears COW        = new Ears(SpeciesName.COW);
		public static readonly Ears ELFIN      = new Ears(SpeciesName.ELFIN);
		public static readonly Ears CAT        = new Ears(SpeciesName.CAT);
		public static readonly Ears LIZARD     = new Ears(SpeciesName.LIZARD);
		public static readonly Ears BUNNY      = new Ears(SpeciesName.BUNNY);
		public static readonly Ears KANGAROO   = new Ears(SpeciesName.KANGAROO);
		public static readonly Ears FOX        = new Ears(SpeciesName.FOX);
		public static readonly Ears DRAGON     = new Ears(SpeciesName.DRAGON);
		public static readonly Ears RACCOON    = new Ears(SpeciesName.RACCOON);
		public static readonly Ears MOUSE      = new Ears(SpeciesName.MOUSE);
		public static readonly Ears FERRET     = new Ears(SpeciesName.FERRET);
		public static readonly Ears PIG        = new Ears(SpeciesName.PIG);
		public static readonly Ears RHINO      = new Ears(SpeciesName.RHINO);
		public static readonly Ears ECHIDA     = new Ears(SpeciesName.ECHIDNA);
		public static readonly Ears DEER       = new Ears(SpeciesName.DEER);
		public static readonly Ears WOLF       = new Ears(SpeciesName.WOLF);
		public static readonly Ears SHEEP      = new Ears(SpeciesName.SHEEP);
		public static readonly Ears IMP        = new Ears(SpeciesName.IMP);
		public static readonly Ears COCKATRICE = new Ears(SpeciesName.COCKATRICE);
		public static readonly Ears RED_PANDA  = new Ears(SpeciesName.RED_PANDA);

		public static void Restore(ref Ears ears)
		{
			ears = HUMAN;
		}
	}
}
