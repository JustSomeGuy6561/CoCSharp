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
		//private delegate void StringWriter(string data);
		//private static readonly StringWriter stringWriter;

		private static readonly LinkedList<SimpleDescriptor> textMaker = new LinkedList<SimpleDescriptor>();
		private static string imageID = null;
		//static TextOutput()
		//{
		//	stringWriter = new StringWriter(Console.WriteLine);
		//}

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

		public static void AddOutput(SimpleDescriptor output)
		{
			updatedSincePreviousQuery = true;
			//Controller.
			textMaker.AddLast(output);
			//stringWriter(output);
		}

		public static void ClearText()
		{
			if (textMaker.Count != 0)
			{
				updatedSincePreviousQuery = true;
			}
			textMaker.Clear();
		}

		//in the event this class becomes public (it shouldn't), this should still be internal, hence internal
		private static StringBuilder data
		{
			get
			{
				StringBuilder sb = new StringBuilder();
				LinkedListNode<SimpleDescriptor> node = textMaker.First;
				while (node != null)
				{
					sb.Append(node.Value?.Invoke());
					node = node.Next;
				}
				return sb;
			}
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
