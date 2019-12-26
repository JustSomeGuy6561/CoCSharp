//HandStrings.cs
//Description:
//Author: JustSomeGuy
//12/31/2018, 1:35 AM


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
		private static string HumanNoun(bool plural)
		{
			return Utils.PluralizeIf("hand", plural);
		}

		private static string HumanNails(bool plural)
		{
			return Utils.PluralizeIf("nail", plural);
		}
		private static string HumanShort(bool plural)
		{
			return HumanNoun(plural);
		}
		private static string HumanSingle()
		{
			return "a human " + HumanNoun(false);
		}

		public static string HumanLongDesc(HandData hands, bool alternateFormat, bool plural)
		{
			if (!plural)
			{
				return "a hand with fingers and nails";
			}
			else
			{
				return "normal hands, fingers, and nails";
			}
		}

		private static string LizardNoun(bool plural)
		{
			return GenericClawedNoun(plural);
		}

		private static string LizardNails(bool plural)
		{
			return ClawNails(plural);
		}
		private static string LizardShort(bool plural)
		{
			return Utils.PluralizeIf("reptilian claw", plural);
		}
		private static string LizardSingle()
		{
			return "a reptilian claw";
		}

		private static string LizardLongDesc(HandData hands, bool alternateFormat, bool plural)
		{
			string intro;
			if (alternateFormat && plural)
			{
				intro = "distinctly reptilian hands,";
			}
			else if (alternateFormat)
			{
				intro = "a distinctly reptilian hand,";
			}
			else
			{
				intro = Utils.PluralizeIf("distinctly reptilian hand", plural);
			}

			return intro + $"with {hands.clawTone.AsString()}ish claws for fingers";
		}

		private static string DragonNoun(bool plural)
		{
			return GenericClawedNoun(plural);
		}

		private static string DragonNails(bool plural)
		{
			return ClawNails(plural);
		}
		private static string DragonShort(bool plural)
		{
			return Utils.PluralizeIf("dragonic claw", plural);
		}
		private static string DragonSingle()
		{
			return "a dragonic claw";
		}

		private static string DragonLongDesc(HandData hands, bool alternateFormat, bool plural)
		{
			string intro;
			if (alternateFormat && plural)
			{
				intro = "distinctly draconic hands,";
			}
			else if (alternateFormat)
			{
				intro = "a distinctly draconic hand,";
			}
			else
			{
				intro = Utils.PluralizeIf("distinctly draconic hand", plural);
			}

			return intro + $"with long, {hands.clawTone.AsString()}ish claws for fingers";
		}

		private static string SalamanderNoun(bool plural)
		{
			return GenericClawedNoun(plural);
		}
		private static string SalamanderNails(bool plural)
		{
			return ClawNails(plural);
		}
		private static string SalamanderShort(bool plural)
		{
			return Utils.PluralizeIf("salamander claw", plural);
		}
		private static string SalamanderSingle()
		{
			return "a salamander claw";
		}


		private static string SalamanderLongDesc(HandData hands, bool alternateFormat, bool plural)
		{
			string intro;
			if (alternateFormat && plural)
			{
				intro = "a pair of scaley, salamander-like hands";
			}
			else if (alternateFormat)
			{
				intro = "a scaley, salamander-like hand";
			}
			else
			{
				intro = Utils.PluralizeIf("scaley, salamander-like hand", plural);
			}

			return intro + $" with claws for fingers";
		}

		private static string CatNoun(bool plural)
		{
			return GenericPawedNoun(plural);
		}
		private static string CatNails(bool plural)
		{
			return ClawNails(plural);
		}
		private static string CatShort(bool plural)
		{
			return Utils.PluralizeIf("cat paw", plural);
		}
		private static string CatSingle()
		{
			return "a cat paw";
		}

		private static string CatLongDesc(HandData hands, bool alternateFormat, bool plural)
		{
			return GenericPawLongDesc(alternateFormat, plural);
		}

		private static string DogNoun(bool plural)
		{
			return GenericPawedNoun(plural);
		}
		private static string DogNails(bool plural)
		{
			return ClawNails(plural);
		}
		private static string DogShort(bool plural)
		{
			return Utils.PluralizeIf("dog paw", plural);
		}
		private static string DogSingle()
		{
			return "a dog paw";
		}

		private static string DogLongDesc(HandData hands, bool alternateFormat, bool plural)
		{
			return GenericPawLongDesc(alternateFormat, plural);
		}

		private static string FoxNoun(bool plural)
		{
			return GenericPawedNoun(plural);
		}
		private static string FoxNails(bool plural)
		{
			return ClawNails(plural);
		}
		private static string FoxShort(bool plural)
		{
			return Utils.PluralizeIf("fox paw", plural);
		}
		private static string FoxSingle()
		{
			return "a fox paw";
		}
		private static string FoxLongDesc(HandData hands, bool alternateFormat, bool plural)
		{
			return GenericPawLongDesc(alternateFormat, plural);
		}

		private static string ImpNoun(bool plural)
		{
			return GenericClawedNoun(plural);
		}
		private static string ImpNails(bool plural)
		{
			return ClawNails(plural);
		}
		private static string ImpShort(bool plural)
		{
			return Utils.PluralizeIf("imp claw", plural);
		}
		private static string ImpSingle()
		{
			return "an imp claw";
		}

		private static string ImpLongDesc(HandData hands, bool alternateFormat, bool plural)
		{
			string intro;
			if (alternateFormat && plural)
			{
				intro = "imp-like hands";
			}
			else if (alternateFormat)
			{
				intro = "an imp-like hand";
			}
			else
			{
				intro = Utils.PluralizeIf("imp-like hand", plural);
			}

			return intro + " with short claws for fingers";
		}


		private static string CockatriceNoun(bool plural)
		{
			return Utils.PluralizeIf("talon", plural);
		}
		private static string CockatriceNails(bool plural)
		{
			return ClawNails(plural);
		}
		private static string CockatriceShort(bool plural)
		{
			return Utils.PluralizeIf("avian talon", plural);
		}
		private static string CockatriceSingle()
		{
			return "an avian talon";
		}

		private static string CockatriceLongDesc(HandData hands, bool alternateFormat, bool plural)
		{
			if (!plural)
			{
				return (alternateFormat ? "an " : "") + "avian talon ending in sharp claws";
			}
			else
			{
				return "avian talons ending in sharp claws";
			}
		}

		private static string RedPandaNoun(bool plural)
		{
			return GenericPawedNoun(plural);
		}
		private static string RedPandaNails(bool plural)
		{
			return ClawNails(plural);
		}
		private static string RedPandaShort(bool plural)
		{
			return Utils.PluralizeIf("panda paw", plural);
		}
		private static string RedPandaSingle()
		{
			return "a panda paw";
		}

		private static string RedPandaLongDesc(HandData hands, bool alternateFormat, bool plural)
		{
			return GenericPawLongDesc(alternateFormat, plural);
		}


		private static string FerretNoun(bool plural)
		{
			return GenericPawedNoun(plural);
		}
		private static string FerretNails(bool plural)
		{
			return ClawNails(plural);
		}
		private static string FerretShort(bool plural)
		{
			return Utils.PluralizeIf("ferret paw", plural);
		}
		private static string FerretSingle()
		{
			return "a ferret paw";
		}
		private static string FerretLongDesc(HandData hands, bool alternateFormat, bool plural)
		{
			return GenericPawLongDesc(alternateFormat, plural);
		}

		private static string GooNoun(bool plural)
		{
			return Utils.PluralizeIf("gooey \"hand\"", plural);
		}
		private static string GooNails(bool plural)
		{
			if (plural) return "goo";
			else return "little bit of goo";
		}
		private static string GooShort(bool plural)
		{
			return Utils.PluralizeIf("gooey appendage", plural);
		}
		private static string GooSingle()
		{
			return "a gooey appendage";
		}

		private static string GooLongDesc(HandData hands, bool alternateFormat, bool plural)
		{
			if (plural)
			{
				return "gooey, vaguely hand-like appendages, with two large digits that form a rough mitten shape";
			}
			else if (alternateFormat)
			{
				return "a gooey, vaguely hand-like appendage, the two opposing digits forming a rough mitten shape";
			}
			else
			{
				return "gooey, vaguely hand-like appendage, the two opposing digits forming a rough mitten shape";
			}
		}

		private static string ClawNails(bool plural)
		{
			return Utils.PluralizeIf("claw", plural);
		}

		private static string GenericPawLongDesc(bool alternateFormat, bool plural)
		{
			string intro = "";
			if (alternateFormat && !plural)
			{
				intro = "a ";
			}

			return intro + Utils.PluralizeIf("cute, pink pad paw", plural) + " with short claws";
		}

		//private static string MantisNails(bool plural)
		//{

		//}
		//private static string MantisShort(bool plural)
		//{
		//	return Utils.Pluralize("mantis scythe", plural)
		//};
		//private static string DELETE_MEHandData hands, bool alternateFormat, bool plural)
		//{
		//	return Utils.Pluralize("mantis scythe", plural)
		//};

		private static string GenericClawedNoun(bool plural)
		{
			return Utils.PluralizeIf("clawed hand", plural);
		}

		private static string GenericPawedNoun(bool plural)
		{
			return Utils.PluralizeIf("paw", plural);
		}
	}
}
