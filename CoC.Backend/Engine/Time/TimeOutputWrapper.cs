//TimeOutputWrapper.cs
//Description:
//Author: JustSomeGuy
//6/29/2019, 11:55 PM
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CoC.Backend.Engine.Time
{
	//a wrapper for Events. 
	public sealed class EventWrapper
	{
		internal string text { get; private set; }
		internal SpecialEvent scene { get; private set; }
		internal bool isScene { get; private set; }

		public EventWrapper(string outputText)
		{
			text = outputText;
			scene = null;
			isScene = false;
		}

		public EventWrapper(SpecialEvent majorEvent)
		{
			text = null;
			scene = majorEvent;
			isScene = true;
		}

		public static explicit operator EventWrapper(StringBuilder stringBuilder)
		{
			return new EventWrapper(stringBuilder.ToString());
		}

		public static explicit operator EventWrapper(string output)
		{
			return new EventWrapper(output);
		}

		public static bool IsNullOrEmpty(EventWrapper instance)
		{
			return instance == null || instance.IsEmpty;
		}

		public static EventWrapper Empty => new EventWrapper(outputText:null);

		public bool IsEmpty => (isScene && scene == null) || (!isScene && string.IsNullOrWhiteSpace(text));
	}
}
