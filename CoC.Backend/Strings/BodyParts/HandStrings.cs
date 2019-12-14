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
			return Utils.Pluralize("hand", plural);
		}

		private static string HumanShort(bool plural)
		{
			return HumanNoun(plural);
		}

		public static string HumanLongDesc(HandData hands, bool alternateForm, bool plural)
		{
			string intro;
			if (alternateForm && plural)
			{
				intro = "a pair of ";
			}
			else if (alternateForm)
			{
				intro = "a ";
			}
			else
			{
				intro = "";
			}

			return intro + Utils.Pluralize("normal hand", plural);

		}

		public static string HumanFullDesc(HandData hands, bool alternateForm, bool plural)
		{
			if (alternateForm && plural)
			{
				return "a pair of normal hands, fingers, and nails.";
			}
			else if (alternateForm)
			{
				return "a normal hand, complete with normal fingers and nails.";
			}
			else if (plural)
			{
				return "normal hands, fingers, and nails";
			}
			else
			{
				return "normal hand, with it's equally normal fingers and nails";
			}
		}

		private static string LizardNoun(bool plural)
		{
			return GenericClawedNoun(plural);
		}

		private static string LizardShort(bool plural)
		{
			return Utils.Pluralize("reptilian claw", plural);
		}

		private static string LizardLongDesc(HandData hands, bool alternateForm, bool plural)
		{
			string intro = "";
			if (alternateForm && plural)
			{
				intro = "a pair of" + hands.clawTone.AsString();
			}
			else if (alternateForm)
			{
				intro = Utils.AddArticle(hands.clawTone.AsString());
			}
			else
			{
				intro = hands.clawTone.AsString();
			}

			return intro + "ish " + LizardShort(plural);
		}

		private static string LizardFullDesc(HandData hands, bool alternateForm, bool plural)
		{
			string intro;
			if (alternateForm && plural)
			{
				intro = "a pair of distinctly reptilian hands,";
			}
			else if (alternateForm)
			{
				intro = "a distinctly reptilian hand,";
			}
			else
			{
				intro = Utils.Pluralize("distinctly reptilian hand", plural);
			}

			return intro + $"with {hands.clawTone.AsString()}ish claws for fingers";
		}

		private static string DragonNoun(bool plural)
		{
			return GenericClawedNoun(plural);
		}

		private static string DragonShort(bool plural)
		{
			return Utils.Pluralize("dragonic claw", plural);
		}
		private static string DragonLongDesc(HandData hands, bool alternateForm, bool plural)
		{
			string intro = "";
			if (alternateForm && plural)
			{
				intro = "a pair of long, " + hands.clawTone.AsString();
			}
			else if (alternateForm)
			{
				intro = "a long, " + hands.clawTone.AsString();
			}
			else
			{
				intro = "long, " + hands.clawTone.AsString();
			}

			return intro + "ish " + DragonShort(plural);
		}

		private static string DragonFullDesc(HandData hands, bool alternateForm, bool plural)
		{
			string intro;
			if (alternateForm && plural)
			{
				intro = "a pair of distinctly draconic hands,";
			}
			else if (alternateForm)
			{
				intro = "a distinctly draconic hand,";
			}
			else
			{
				intro = Utils.Pluralize("distinctly draconic hand", plural);
			}

			return intro + $"with long, {hands.clawTone.AsString()}ish claws for fingers";
		}

		private static string SalamanderNoun(bool plural)
		{
			return GenericClawedNoun(plural);
		}
		private static string SalamanderShort(bool plural)
		{
			return Utils.Pluralize("salamander claw", plural);
		}
		private static string SalamanderLongDesc(HandData hands, bool alternateForm, bool plural)
		{
			string intro;
			if (alternateForm && plural)
			{
				intro = "a pair of scaley ";
			}
			else if (alternateForm)
			{
				intro = "a scaley, ";
			}
			else
			{
				intro = "scaley ";
			}
			return intro + SalamanderShort(plural);
		}

		private static string SalamanderFullDesc(HandData hands, bool alternateForm, bool plural)
		{
			string intro;
			if (alternateForm && plural)
			{
				intro = "a pair of scaley, salamander-like hands";
			}
			else if (alternateForm)
			{
				intro = "a scaley, salamander-like hand";
			}
			else
			{
				intro = Utils.Pluralize("scaley, salamander-like hand", plural);
			}

			return intro + $" with claws for fingers";
		}

		private static string CatNoun(bool plural)
		{
			return GenericPawedNoun(plural);
		}
		private static string CatShort(bool plural)
		{
			return Utils.Pluralize("cat paw", plural);
		}
		private static string CatLongDesc(HandData hands, bool alternateForm, bool plural)
		{
			return GenericPawLongDesc(alternateForm, plural);
		}
		private static string CatFullDesc(HandData hands, bool alternateForm, bool plural)
		{
			return GenericPawFullDesc(alternateForm, plural);
		}

		private static string DogNoun(bool plural)
		{
			return GenericPawedNoun(plural);
		}
		private static string DogShort(bool plural)
		{
			return Utils.Pluralize("dog paw", plural);
		}
		private static string DogLongDesc(HandData hands, bool alternateForm, bool plural)
		{
			return GenericPawLongDesc(alternateForm, plural);
		}
		private static string DogFullDesc(HandData hands, bool alternateForm, bool plural)
		{
			return GenericPawFullDesc(alternateForm, plural);
		}

		private static string FoxNoun(bool plural)
		{
			return GenericPawedNoun(plural);
		}
		private static string FoxShort(bool plural)
		{
			return Utils.Pluralize("fox paw", plural);
		}
		private static string FoxLongDesc(HandData hands, bool alternateForm, bool plural)
		{
			return GenericPawLongDesc(alternateForm, plural);
		}
		private static string FoxFullDesc(HandData hands, bool alternateForm, bool plural)
		{
			return GenericPawFullDesc(alternateForm, plural);
		}

		private static string ImpNoun(bool plural)
		{
			return GenericClawedNoun(plural);
		}
		private static string ImpShort(bool plural)
		{
			return Utils.Pluralize("imp claw", plural);
		}
		private static string ImpLongDesc(HandData hands, bool alternateForm, bool plural)
		{
			string intro;
			if (alternateForm && plural)
			{
				intro = "a pair of ";
			}
			else if (alternateForm)
			{
				intro = "a ";
			}
			else
			{
				intro = "";
			}
			return intro + "clawed, imp-like hands";
		}
		private static string ImpFullDesc(HandData hands, bool alternateForm, bool plural)
		{
			string intro;
			if (alternateForm && plural)
			{
				intro = "a pair of imp-like hands";
			}
			else if (alternateForm)
			{
				intro = "an imp-like hand";
			}
			else
			{
				intro = Utils.Pluralize("imp-like hand", plural);
			}

			return intro + " with short claws for fingers";
		}


		private static string CockatriceNoun(bool plural)
		{
			return Utils.Pluralize("talon", plural);
		}
		private static string CockatriceShort(bool plural)
		{
			return Utils.Pluralize("avian talon", plural);
		}
		private static string CockatriceLongDesc(HandData hands, bool alternateForm, bool plural)
		{
			string intro;
			if (alternateForm && plural)
			{
				intro = "a pair of ";
			}
			else if (alternateForm)
			{
				intro = "a ";
			}
			else
			{
				intro = "";
			}

			return intro + Utils.Pluralize("deadly looking avian talon", plural);
		}

		private static string CockatriceFullDesc(HandData hands, bool alternateForm, bool plural)
		{
			string claws = plural ? ", each ending in sharp claws" : "ending in sharp claws";

			return CockatriceLongDesc(hands, alternateForm, plural) + claws;
		}

		private static string RedPandaNoun(bool plural)
		{
			return GenericPawedNoun(plural);
		}
		private static string RedPandaShort(bool plural)
		{
			return Utils.Pluralize("panda paw", plural);
		}
		private static string RedPandaLongDesc(HandData hands, bool alternateForm, bool plural)
		{
			return GenericPawLongDesc(alternateForm, plural);
		}
		private static string RedPandaFullDesc(HandData hands, bool alternateForm, bool plural)
		{
			return GenericPawFullDesc(alternateForm, plural);
		}


		private static string FerretNoun(bool plural)
		{
			return GenericPawedNoun(plural);
		}
		private static string FerretShort(bool plural)
		{
			return Utils.Pluralize("ferret paw", plural);
		}
		private static string FerretLongDesc(HandData hands, bool alternateForm, bool plural)
		{
			return GenericPawLongDesc(alternateForm, plural);
		}
		private static string FerretFullDesc(HandData hands, bool alternateForm, bool plural)
		{
			return GenericPawFullDesc(alternateForm, plural);
		}


		private static string GooNoun(bool plural)
		{
			return Utils.Pluralize("gooey \"hand\"", plural);
		}
		private static string GooShort(bool plural)
		{
			return Utils.Pluralize("gooey appendage", plural);
		}

		private static string GooLongDesc(HandData hands, bool alternateForm, bool plural)
		{
			string intro;
			if (alternateForm && plural)
			{
				intro = "a pair of ";
			}
			else if (alternateForm)
			{
				intro = "a ";
			}
			else
			{
				intro = "";
			}

			return intro + Utils.Pluralize("gooey, vaguely hand-like appendage", plural);
		}

		private static string GooFullDesc(HandData hands, bool alternateForm, bool plural)
		{
			if (alternateForm && plural)
			{
				return "a pair of gooey, vaguely hand-like appendages, each with two large digits that form a rough mitten shape";
			}
			else if (alternateForm)
			{
				return "a gooey, vaguely hand-like appendage, the two opposing digits forming a rough mitten shape.";
			}
			else if (plural)
			{
				return "gooey, vaguely hand-like appendages, each with two large digits that form a rough mitten shape";
			}
			else
			{
				return "gooey, vaguely hand-like appendage, the two opposing digits forming a rough mitten shape.";

			}
		}

		private static string GenericPawLongDesc(bool alternateForm, bool plural)
		{
			string intro = "";
			if (alternateForm && plural)
			{
				intro = "a pair of ";
			}
			else if (alternateForm)
			{
				intro = "a ";
			}

			return intro + Utils.Pluralize("cute, pink pad paw", plural);
		}

		private static string GenericPawFullDesc(bool alternateForm, bool plural)
		{
			string intro = "";
			if (alternateForm && plural)
			{
				intro = "a pair of ";
			}
			else if (alternateForm)
			{
				intro = "a ";
			}

			return intro + Utils.Pluralize("cute, pink pad paw", plural) + " with short claws";
		}

		//private static string MantisShort(bool plural)
		//{
		//	return Utils.Pluralize("mantis scythe", plural)
		//};
		//private static string MantisLongDesc(HandData hands, bool alternateForm, bool plural)
		//{
		//	return Utils.Pluralize("mantis scythe", plural)
		//};

		private static string GenericClawedNoun(bool plural)
		{
			return Utils.Pluralize("clawed hand", plural);
		}

		private static string GenericPawedNoun(bool plural)
		{
			return Utils.Pluralize("paw", plural);
		}
	}
}
