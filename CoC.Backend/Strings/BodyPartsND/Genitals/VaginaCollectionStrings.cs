using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using CoC.Backend.Tools;

namespace CoC.Backend.BodyParts
{
	internal interface IVaginaCollection<T> where T:IVagina
	{
		ReadOnlyCollection<T> vaginas { get; }

		VaginaData AverageVagina();
	}

	public partial class VaginaCollection : IVaginaCollection<Vagina>
	{
		private static string Name()
		{
			return "All Vaginas";
		}


		ReadOnlyCollection<Vagina> IVaginaCollection<Vagina>.vaginas => vaginas;
	}

	internal static class VaginaCollectionStrings
	{
		private static int NumVaginas<T>(this IVaginaCollection<T> collection) where T:IVagina
		{
			return collection.vaginas.Count;
		}

		#region Vagina Text
		internal static string AllVaginasShortDescription<T>(IVaginaCollection<T> collection) where T:IVagina
		{
			if (collection.vaginas.Count == 0)
			{
				return "";
			}
			else if (collection.vaginas.Count == 1)
			{
				return collection.vaginas[0].ShortDescription();
			}
			bool mismatched = collection.vaginas.Any(x => x.type != collection.vaginas[0].type);

			return mismatched ? VaginaType.VaginaNoun(true) : collection.vaginas[0].ShortDescription(false);
		}

		internal static string AllVaginasLongDescription<T>(IVaginaCollection<T> collection) where T:IVagina
		{
			return AllVaginasDesc(collection, false);
		}

		internal static string AllVaginasFullDescription<T>(IVaginaCollection<T> collection) where T:IVagina
		{
			return AllVaginasDesc(collection, true);
		}

		internal static string OneVaginaOrVaginasNoun<T>(IVaginaCollection<T> collection, string pronoun = "your") where T:IVagina
		{
			if (collection.vaginas.Count == 0)
			{
				return "";
			}

			return CommonBodyPartStrings.OneOfDescription(collection.vaginas.Count > 1, pronoun, VaginaType.VaginaNoun(collection.vaginas.Count > 1));
		}

		internal static string OneVaginaOrVaginasShort<T>(IVaginaCollection<T> collection, string pronoun = "your") where T:IVagina
		{
			if (collection.vaginas.Count == 0)
			{
				return "";
			}

			return CommonBodyPartStrings.OneOfDescription(collection.vaginas.Count > 1, pronoun, AllVaginasShortDescription(collection));
		}

		internal static string EachVaginaOrVaginasNoun<T>(IVaginaCollection<T> collection, string pronoun = "your") where T:IVagina
		{
			return EachVaginaOrVaginasNoun(collection, pronoun, out bool _);
		}

		internal static string EachVaginaOrVaginasShort<T>(IVaginaCollection<T> collection, string pronoun = "your") where T:IVagina
		{
			return EachVaginaOrVaginasShort(collection, pronoun, out bool _);
		}

		internal static string EachVaginaOrVaginasNoun<T>(IVaginaCollection<T> collection, string pronoun, out bool isPlural) where T:IVagina
		{
			isPlural = collection.vaginas.Count != 1;
			if (collection.vaginas.Count == 0)
			{
				return "";
			}

			return CommonBodyPartStrings.EachOfDescription(collection.vaginas.Count > 1, pronoun, VaginaType.VaginaNoun(collection.vaginas.Count > 1));
		}

		internal static string EachVaginaOrVaginasShort<T>(IVaginaCollection<T> collection, string pronoun, out bool isPlural) where T:IVagina
		{
			isPlural = collection.vaginas.Count != 1;
			if (collection.vaginas.Count == 0)
			{
				return "";
			}

			return CommonBodyPartStrings.EachOfDescription(collection.vaginas.Count > 1, pronoun, AllVaginasShortDescription(collection));
		}
		#endregion
		private static string RandomMixedVaginasText()
		{
			return Utils.RandomChoice("mixed ", "mixed ", "mismatched ") + VaginaType.VaginaNoun(true);
		}

		private static string AllVaginasDesc<T>(IVaginaCollection<T> collection, bool full) where T:IVagina
		{

			if (collection.vaginas.Count == 0)
			{
				return "";
			}
			//If one, return normal cock descript
			else if (collection.vaginas.Count == 1)
			{
				return collection.vaginas[0].ShortDescription();
			}
			else
			{
				bool mismatched = collection.vaginas.Any(x => x.type != collection.vaginas[0].type);

				string[] countOptions = mismatched ? CommonGenitalStrings.mismatchedPairOptions : CommonGenitalStrings.matchedPairOptions;
				string description = mismatched ? RandomMixedVaginasText() : collection.vaginas[0].ShortDescription(true);

				return Utils.RandomChoice(countOptions) + collection.AverageVagina().AdjectiveText(full) + description;
			}
		}
	}
}
