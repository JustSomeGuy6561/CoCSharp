//TextOutput.cs
//Description:
//Author: JustSomeGuy
//2/27/2019, 10:15 PM

//TextOutput.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 11:38 PM
using System.Text;

namespace CoC.Frontend.UI
{
	internal static class TextOutput
	{
		//private delegate void StringWriter(string data);
		//private static readonly StringWriter stringWriter;

		private static StringBuilder outputData = new StringBuilder();

		//static TextOutput()
		//{
		//	stringWriter = new StringWriter(Console.WriteLine);
		//}
		public static void OutputText(string output)
		{
			//Controller.
			outputData.Append(output);
			//stringWriter(output);
		}

		public static void ClearOutput()
		{
			outputData.Clear();
		}

		//in the event this class becomes public (it shouldn't), this should still be internal, hence internal
		internal static string data => outputData.ToString();
		//public 
	}
}
