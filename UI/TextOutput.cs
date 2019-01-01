//TextOutPut.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 11:38 PM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.UI
{
	public static class TextOutput
	{
		private delegate void StringWriter(string data);
		private static readonly StringWriter stringWriter;
		static TextOutput()
		{
			stringWriter = new StringWriter(Console.WriteLine);
		}
		public static void OutputText(string output)
		{
			stringWriter(output);
		}
	}
}
