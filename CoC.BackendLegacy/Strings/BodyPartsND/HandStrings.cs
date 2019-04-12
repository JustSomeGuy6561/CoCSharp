//HandStrings.cs
//Description:
//Author: JustSomeGuy
//12/31/2018, 1:35 AM

using CoC.Backend.Tools;

namespace CoC.Backend.BodyParts
{
	public partial class HandType
	{
		private static string HumanShort() { return "hands"; }
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
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SalamanderShort() { return "salamander claws"; }
		private static string SalamanderFullDesc(Hands hands)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CatShort() { return "cat paws"; }
		private static string CatFullDesc(Hands hands)
		{
			return GenericPawFullDesc();
		}
		private static string DogShort() { return "dog paws"; }
		private static string DogFullDesc(Hands hands)
		{
			return GenericPawFullDesc();
		}
		private static string FoxShort() { return "fox paws"; }
		private static string FoxFullDesc(Hands hands)
		{
			return GenericPawFullDesc();
		}
		private static string ImpShort() { return "imp claws"; }
		private static string ImpFullDesc(Hands hands)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CockatriceShort() { return "cockatrice claws"; }
		private static string CockatriceFullDesc(Hands hands)
		{
			return "deadly looking avian talons";
		}
		private static string RedPandaShort() { return "panda paws"; }
		private static string RedPandaFullDesc(Hands hands)
		{
			return GenericPawFullDesc();
		}
		private static string FerretShort() { return "ferret paws"; }
		private static string FerretFullDesc(Hands hands)
		{
			return GenericPawFullDesc();
		}

		private static string GenericPawFullDesc()
		{
			return "cute, pink paw pads and short claws";
		}
		//private static string MantisShort() { return "mantis scythes"};
		//private static string MantisFullDesc(Hands hands) { return "mantis scythes"};
	}
}
