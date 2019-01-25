//HandStrings.cs
//Description:
//Author: JustSomeGuy
//12/31/2018, 1:35 AM

namespace CoC.BodyParts
{
	internal partial class HandType
	{
		private static string HumanShort() { return "normal hands"; }
		public static string HumanFullDesc(Hands hands)
		{
			return "normal hands, fingers, and nails.";
		}
		private static string LizardShort() { return "lizard claws"; }

		private static string LizardFullDesc(Hands hands)
		{
			return hands.clawTone.AsString() + "ish " + LizardShort();
		}

		private static string DragonShort() { return "dragon claws"; }
		private static string DragonFullDesc(Hands hands)
		{

		}
		private static string SalamanderShort() { return "salamander claws"; }
		private static string SalamanderFullDesc(Hands hands)
		{

		}
		private static string CatShort() { return "cat paws"; }
		private static string CatFullDesc(Hands hands)
		{

		}
		private static string DogShort() { return "dog paws"; }
		private static string DogFullDesc(Hands hands)
		{

		}
		private static string FoxShort() { return "fox paws"; }
		private static string FoxFullDesc(Hands hands)
		{

		}
		private static string ImpShort() { return "imp claws"; }
		private static string ImpFullDesc(Hands hands)
		{

		}
		private static string CockatriceShort() { return "cockatrice claws"; }
		private static string CockatriceFullDesc(Hands hands)
		{

		}
		private static string RedPandaShort() { return "panda paws"; }
		private static string RedPandaFullDesc(Hands hands)
		{

		}
		private static string FerretShort() { return "ferret paws"; }
		private static string FerretFullDesc(Hands hands)
		{

		}
		//private static string MantisShort() { return "mantis scythes"};
		//private static string MantisFullDesc(Hands hands) { return "mantis scythes"};
	}
}
