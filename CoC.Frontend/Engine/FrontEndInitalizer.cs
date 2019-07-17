using CoC.Backend.Perks;
using CoC.Frontend.Perks;
using CoC.Frontend.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Engine
{
	//this class is responsible for initializing all relevant data for the frontend. It then calls the backend's initalizer, passing it the relevant data it needs from the frontend to set up the 
	//overall engine. This the most technical part of the frontend, but it's necessary to allow you to create and alter content here in the frontend, but still have it work in the backend. 
	public static class FrontendInitalizer
	{
		public static void Init()
		{
			BasePerkModifiers getExtraData() => new ExtraPerkModifiers();
			//Backend.Engine.BackendInitializer.Init(MenuHelpers.DoNext, TextOutput.OutputText, );
		}


	}
}
