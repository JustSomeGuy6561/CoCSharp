using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CoC.Backend.Tools
{
	//a wrapper for output. Generally speaking, we can just do this manually via OutputText and OutputImage,
	//but there are some cases where this is not possible. So we're creating a helper.
	public sealed class OutputWrapper
	{
		public readonly string text;
		public readonly string imagePath;


		public OutputWrapper(string outputText)
		{
			text = outputText;
			imagePath = null;
		}

		public OutputWrapper(string outputText, string imageLocation)
		{
			text = outputText;
			imagePath = imageLocation;
		}

		public static explicit operator OutputWrapper(StringBuilder stringBuilder)
		{
			return new OutputWrapper(stringBuilder.ToString());
		}

		public static explicit operator OutputWrapper(string output)
		{
			return new OutputWrapper(output);
		}

		public static bool IsNullOrEmpty(OutputWrapper instance)
		{
			return instance == null || instance.IsEmpty;
		}

		public bool IsEmpty => string.IsNullOrWhiteSpace(imagePath) && string.IsNullOrWhiteSpace(text);
		
	}
}
