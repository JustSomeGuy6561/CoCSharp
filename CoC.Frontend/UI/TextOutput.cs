//TextOutput.cs
//Description:
//Author: JustSomeGuy
//2/27/2019, 10:15 PM

//TextOutput.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 11:38 PM
using CoC.Backend;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.UI
{
	internal static class TextOutput
	{

		private static string imageID = null;

		private static readonly StringBuilder data = new StringBuilder();

		private static bool updatedSincePreviousQuery = false;

		public static bool OutputImage(string uniqueImageIdentifier)
		{
			updatedSincePreviousQuery = true;
			if (imageID == null)
			{
				imageID = uniqueImageIdentifier;
				return false;
			}
			else
			{
				imageID = uniqueImageIdentifier;
				return true;
			}
		}

		public static void OutputText(string output)
		{
			if (!string.IsNullOrEmpty(output))
			{
				updatedSincePreviousQuery = true;
				data.Append(output);
			}
		}

		public static void ClearText()
		{
			if (data.Length != 0)
			{
				updatedSincePreviousQuery = true;
			}
			data.Clear();
		}

		internal static bool QueryData(out StringBuilder _outputField, out string _outputImagePath)
		{
			bool retVal = updatedSincePreviousQuery;
			updatedSincePreviousQuery = false;
			_outputField = data;
			_outputImagePath = imageID;
			return retVal;
		}


		//public 
	}
}
