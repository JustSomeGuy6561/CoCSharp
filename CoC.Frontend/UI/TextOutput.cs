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

		public static bool OutputImage(string uniqueImageIdentifier)
		{
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
			//Controller.
			textMaker.AddLast(output);
			//stringWriter(output);
		}

		public static void ClearOutput()
		{
			textMaker.Clear();
		}

		//in the event this class becomes public (it shouldn't), this should still be internal, hence internal
		internal static StringBuilder data
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
		internal static string image => imageID;
		//public 
	}
}
