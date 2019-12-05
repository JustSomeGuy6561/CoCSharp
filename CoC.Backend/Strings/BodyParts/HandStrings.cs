//HandStrings.cs
//Description:
//Author: JustSomeGuy
//12/31/2018, 1:35 AM

using CoC.Backend.CoC_Colors;
using CoC.Backend.Tools;

namespace CoC.Backend.BodyParts
{
	public partial class Hands
	{
		public static string Name()
		{
			return "Hands";
		}
	}

	public partial class HandType
	{
		private static string HumanShort() { return "hands"; }
		public static string HumanLongDesc(HandData hands)
		{
			return "normal hands, fingers, and nails.";
		}

		private static string LizardShort() { return "lizard claws"; }

		private static string LizardLongDesc(HandData hands)
		{
			return hands.clawTone.AsString() + "ish " + LizardShort();
		}

		private static string DragonShort() { return "dragon claws"; }
		private static string DragonLongDesc(HandData hands)
		{
			return hands.clawTone.AsString() + "ish draconic claws";
		}

		private static string SalamanderShort() { return "salamander claws"; }
		private static string SalamanderLongDesc(HandData hands)
		{
			return "scaley " + SalamanderShort();
		}

		private static string CatShort() { return "cat paws"; }
		private static string CatLongDesc(HandData hands)
		{
			return GenericPawLongDesc();
		}

		private static string DogShort() { return "dog paws"; }
		private static string DogLongDesc(HandData hands)
		{
			return GenericPawLongDesc();
		}

		private static string FoxShort() { return "fox paws"; }
		private static string FoxLongDesc(HandData hands)
		{
			return GenericPawLongDesc();
		}

		private static string ImpShort() { return "imp claws"; }
		private static string ImpLongDesc(HandData hands)
		{
			return ImpShort();
		}

		private static string CockatriceShort() { return "cockatrice claws"; }
		private static string CockatriceLongDesc(HandData hands)
		{
			return "deadly looking avian talons";
		}

		private static string RedPandaShort() { return "panda paws"; }
		private static string RedPandaLongDesc(HandData hands)
		{
			return GenericPawLongDesc();
		}

		private static string FerretShort() { return "ferret paws"; }
		private static string FerretLongDesc(HandData hands)
		{
			return GenericPawLongDesc();
		}

		private static string GooShort()
		{
			return "gooey appendages";
		}

		private static string GooLongDesc(HandData hands)
		{
			return "gooey, vaguely hand-like appendages";
		}


		private static string GenericPawLongDesc()
		{
			return "cute, pink paw pads and short claws";
		}

		//private static string MantisShort() { return "mantis scythes"};
		//private static string MantisLongDesc(HandData hands) { return "mantis scythes"};
	}
}
