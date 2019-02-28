//TextOutput.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 11:38 PM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.UI
{
	internal static class TextOutput
	{
		private delegate void StringWriter(string data);
		private static readonly StringWriter stringWriter;

		private static StringBuilder outputData = new StringBuilder();

		static TextOutput()
		{
			stringWriter = new StringWriter(Console.WriteLine);
		}
		public static void OutputText(string output)
		{
			//Controller.
			stringWriter(output);
		}

		public static void ClearOutput()
		{
			outputData.Clear();
		}

		//public 
	}
}
