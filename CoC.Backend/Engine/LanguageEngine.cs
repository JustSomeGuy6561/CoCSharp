using CoC.Backend.Engine.Language;
using CoC.Backend.SaveData;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CoC.Backend.Engine
{
	//idk - we could do an enum for languages or some sort of lookup tool - get the class and name of the SimpleDescriptor or function name,
	//and then run that through a lookup tool or function. if result found, use it. otherwise display hard-coded text as a fallback.
	//of course, if these languages need variables, which makes sense, we'll need some magic parser for them. idk how we'll do it yet.
	//could possibly generate a massive list of functions for text display via reflection and some serious meta-programming.
	//and then since we have the signature for each of them, generating text for it would be relatively easy, idk.
	public static class LanguageEngine
	{
		private static BackendGlobalSave saveData => BackendGlobalSave.data;

		public static ReadOnlyCollection<LanguageBase> availableLanguages;
		private static List<LanguageBase> availableLanguageStore;

		public static int currentLanguageIndex
		{
			get => saveData.languageIndex;
			set => saveData.languageIndex = value;
		}

		public static LanguageBase currentLanguage => availableLanguages[currentLanguageIndex];

		static LanguageEngine()
		{
			availableLanguageStore = new List<LanguageBase>();
			//add the languages.
			availableLanguageStore.AddAt(new AmericanEnglish(), 0);
			//
			availableLanguages = new ReadOnlyCollection<LanguageBase>(availableLanguageStore);
		}

	}
}
