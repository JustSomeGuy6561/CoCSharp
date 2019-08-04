using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoCWinDesktop.Strings
{
	public static class LanguageLookup
	{
#warning ToDo: Clean this up, and make the StatDisplay not use this.
		public static string Lookup(string englishText)
		{
			return char.ToUpper(englishText[0]) + englishText.Substring(1);
		}
	}

	public static class StringUtils
	{
		private static readonly Dictionary<string, string> textReplacementLookup = new Dictionary<string, string>()
		{

			//quick note: @ means treat string as literal, so it's looking for '\', not an escape character.
			// @"\" is identical to "\\" So it won't replace "\r" or "\n"
			[@"\"] = @"\\", //if you're having bash nightmares right now i'm sorry. 
			[@"{"] = @"\{", //see above

			[@"<b>"] = @"\b ",
			[@"</b>"] = @"\b0 ",
			[@"<i>"] = @"\i ",
			[@"</i>"] = @"\i0 ",
			[@"<em>"] = @"\i ", //who even uses this?
			[@"</em>"] = @"\i0 ",

			[@"<pre>"] = @"",
			[@"</pre>"] = @"",

			//note the lack of @ on these strings - these strings we actually want to escape and look for the special character. 
			//more fun facts: IIRC Environment.NewLine returns "\n" for all things not Windows, though idk if it has funny rules for Xamarin on Mac. 
			["\r\n"] = @"\par ", //You're on Windows and used the Environment.NewLine. Fun fact, the older parser stripped out the \r in \r\n
			["\n"] = @"\par ", //The 'standard'. IMO the safe route of throwing both (windows) is better, though then there are parsers that treat that as \n\n and that's bad.
			["\r"] = @"\par ", //It was written on a Mac. I hate you. JK, but damn - I'm surprised the old parser didn't blow up with \r
			["\t"] = @"\tab ",

			//This is a simplistic way of simulating an unordered list. Hacky as all hell, and it'll break with nested lists, but we never have nested lists so this should work fine. 
			//also this will break with a random <li> or if there are ever ordered lists (<ol>). It's possible to do it correctly with about 10 different bullshit RTF tags, but fuck that.
			//I mean, if you have a hard-on for perfectly formed RTF, i think you're playing the wrong game here ¯\_(ツ)_/¯. But by all means, fix it.   
			[@"<ul>"] = @"",
			[@"</ul>"] = @"\line ",
			[@"<li>"] = @"\line  \bullet  ",
			[@"</li>"] = @"",

			[@"<u>"] = @"\ul ",
			[@"</u>"] = @"\ul0 ",
			[@"<br/>"] = @"\line ",
			[@"<br />"] = @"\line ",
			[@"<br>"] = @"\line ",
			[@"</br>"] = @"", //i actually hate you.
		};

		public static StringParserUtil GetParser { get; }

		static StringUtils()
		{
			GetParser = new StringParserUtil(textReplacementLookup);
		}
	}

	public class StringParserUtil
	{
		private readonly Dictionary<string, string> targetAndReplacement;
		private readonly Dictionary<char, LetterNode> letterLookups;

		private class LetterNode
		{
			public readonly char lookup; //the char. useful for nextChar primarily. 
			public readonly LetterNode previousNode;

			public List<LetterNode> nextChar; //i'm not making this immutable - not worth the cost, and so long as this is left alone it will work. AKA don't be a dick.
			public bool isATargetString = false; //this, however, is updated outside of this class. 
			//this is a property and therefore lazy - it only calulates when executed. so it won't cause crazy levels of stepping all the time. 
			public bool wasATargetString => previousNode?.isATargetString == true || previousNode?.wasATargetString == true;

			public LetterNode(char startChar)
			{
				lookup = startChar;
				previousNode = null;
			}

			private LetterNode(LetterNode prevNode, char currChar) : this(currChar)
			{
				if (prevNode.nextChar is null)
				{
					prevNode.nextChar = new List<LetterNode>() { this };
				}
				else
				{
					prevNode.nextChar.Add(this);
				}

				previousNode = prevNode ?? throw new ArgumentNullException(nameof(prevNode)); //not necessary. but if i mucked this up, this'll help debug.
				lookup = currChar;
			}

			public LetterNode AddNextChar(char v)
			{
					LetterNode node = nextChar?.Find(x => x.lookup == v);
					if (node == null)
					{
						node = new LetterNode(this, v);
					}
					return node;
			}

			public string GetString()
			{
				LetterNode node = this;
				StringBuilder sb = new StringBuilder();
				do
				{
					sb.Insert(0, node.lookup);
					node = node.previousNode;
				}
				while (node != null);

				return sb.ToString();
			}
		}

		public StringParserUtil(Dictionary<string, string> lookupReplace)
		{
			if (lookupReplace is null) throw new ArgumentNullException(nameof(lookupReplace));
			targetAndReplacement = new Dictionary<string, string>();
			letterLookups = new Dictionary<char, LetterNode>();
			if (lookupReplace.Count == 0)
			{
				return;
			}
			foreach (string str in lookupReplace.Keys)
			{
				if (String.IsNullOrEmpty(str)) //we'll allow whitespace chars, b/c \r, \n, \t, etc. Don't abuse this.
				{
					continue;
				}
				targetAndReplacement.Add(str, lookupReplace[str] ?? "");
			}

			Dictionary<LetterNode, List<string>> nextIteration = new Dictionary<LetterNode, List<string>>();

			Dictionary<LetterNode, List<string>> currentIteration = new Dictionary<LetterNode, List<string>>();
			int index = 0;

			//O(n*m) where n is the length of the longest string and m is the number of strings. 
			foreach (string str in targetAndReplacement.Keys)
			{
				char ch = str[index];
				if (!letterLookups.ContainsKey(ch))
				{
					LetterNode node = new LetterNode(ch);
					letterLookups.Add(ch, node);
					currentIteration.Add(node, new List<string>() { str });
				}
				else
				{
					LetterNode node = letterLookups[ch];
					currentIteration[node].Add(str);
				}

			}
			index++;

			while (currentIteration.Count != 0)
			{
				foreach (KeyValuePair<LetterNode, List<string>> pair in currentIteration)
				{
					foreach (string str in pair.Value)
					{
						if (str.Length == index)
						{
							pair.Key.isATargetString = true;
						}
						else
						{
							LetterNode node = pair.Key.AddNextChar(str[index]);
							if (!nextIteration.ContainsKey(node))
							{
								nextIteration.Add(node, new List<string>() { str });
							}
							else
							{
								nextIteration[node].Add(str);
							}
						}
					}
				}

				index++;
				//we're done with current iteration, and need to set the next iteration to the current iteration so the next loop works.
				//we also need the next iteration to be empty. we can save memory by being clever here. 
				currentIteration.Clear(); //clear current.
				var temp = currentIteration; //set a temp to the cleared iteration.
				currentIteration = nextIteration; //move next to curr.
				nextIteration = temp; //no need to allocate a new dictionary. 
			}
		}

		//public string Parse(StringBuilder builder)
		//{
		//	if (builder == null || builder.Length == 0)
		//	{
		//		return "";
		//	}
		//	char[] data = new char[builder.Length];
		//	builder.CopyTo()
		//	return Parse(
		//}

		public string Parse(StringBuilder builder)
		{
			if (builder == null || builder.Length == 0)
			{
				return "";
			}

			//List of letter nodes that can be built from the previous character(s). It will be updated in place. Note that due to the nature of replace, it may
			//be possible for a valid target to be obtained, but it's part of a longer target (for example, \r or \n and \r\n). Worse still, some targets may be
			//fully contained within longer containers. (for example, feels and eel). To handle this, we have a unique boolean that signals that the current
			//element has gone to completion, but is waiting to make sure no other targers starting before it's starting location exist. 
			LinkedList<Pair<LetterNode, bool>> currSubstrings = new LinkedList<Pair<LetterNode, bool>>();
			int index = 0; //current character in string to check. 

			//loop through entire string. 
			while (index < builder.Length)
			{
				bool foundSomethingThisLoop = false;
				//loop through all available nodes. note that there is only ever one node for a given character index, and the nodes are stored by index; the 
				//node that started at the lowest index (and is still a possible target) is always first in the list. 
				var node = currSubstrings.First;
				while (node != null)
				{
					Pair<LetterNode, bool> pair = node.Value;
					LetterNode letter = pair.first;
					bool skipOverIfNotFirst = pair.second;

					//if we've set the special flag to say to skip this if we're not the first because other, longer targets may exist before us. 
					if (skipOverIfNotFirst)
					{
						//but we are the first
						if (node == currSubstrings.First)
						{
							//do the replace. 
							DoReplace(builder, letter.GetString(), ref index);
							if (node == currSubstrings.Last)
							{
								node = null;
								currSubstrings.Clear();
							}
							else
							{
								node = node.Next;
								currSubstrings.Remove(node.Previous);
							}
							continue;
						}
						//otherwise let the loop handle it. 
					}
					//if we haven't set that flag, proceed normally.
					else
					{
						//find the next character in the current possibility node that matches the current letter in the string, if possible.
						LetterNode next = letter.nextChar?.Find(x => x.lookup == builder[index]);
						//if not, it returns null. 
						if (next is null)
						{
							//if we are the edge case where we hit a possible target but could also have hit a longer one (like \r in \r\n), but we didn't.
							if (letter.wasATargetString || letter.isATargetString)
							{
								//walk back the pair until we are a target once again. 
								while (!letter.isATargetString)
								{
									letter = letter.previousNode;
								}
								//and then set our current node to with the last good target, and mark it true. 
								node.Value = new Pair<LetterNode, bool>(letter, true);
							}
							//otherwise, we have no need to stay in the list, so we get removed - womp womp.
							else
							{
								node = node.Previous; //move to the previous. we'll increment at the end of this loop, to the next one
								currSubstrings.Remove(pair); //because this is being removed.
							}
						}
						//otherwise, we do have a valid next letter
						else
						{
							//check if it's a target.
							if (next.isATargetString) 
							{
								//If it is, then every element in the list past us cannot happen - we start before they do. so, for example, we'd want to remove
								//a \n from out possible list if we just found \r\n. 
								currSubstrings.RemoveAfter(node);
								foundSomethingThisLoop = true;
							}
							//regardless, we have a valid next letter, so we'll update our current node.
							node.Value = new Pair<LetterNode, bool>(next, false);
						}
					}
					node = node.Next;
				}

				if (!foundSomethingThisLoop && letterLookups.TryGetValue(builder[index], out var elem))
				{
					currSubstrings.AddLast(new Pair<LetterNode, bool>(elem, false));
				}
				index++;
			}
			return builder.ToString();
		}

		private void DoReplace(StringBuilder targetString, string strToReplace, ref int index)
		{
			string replaceStr = targetAndReplacement[strToReplace];
			int len = strToReplace.Length;
			int startPos = index + 1 - len;
			targetString.Replace(strToReplace, replaceStr, startPos, len);
			index += replaceStr.Length - len; //add the delta. we'll move forward automatically in the while loop. 
		}

		public string Possibilities()
		{
			StringBuilder sb = new StringBuilder();
			foreach (var p in letterLookups)
			{
				var l = p.Value;
				sb.AppendLine("*********************First Letter: " + p.Key + "*************************");
				sb.AppendLine(makeWord(l));
			}
			return sb.ToString();
		}

		private string makeWord(LetterNode node)
		{
			if (node.nextChar == null)
			{
				string val = node.GetString();
				return val + " => " + this.targetAndReplacement[val] + Environment.NewLine + "---------------------------------------" + Environment.NewLine;
			}
			else
			{
				StringBuilder sb = new StringBuilder();
				

				foreach (var l in node.nextChar)
				{
					sb.Append(makeWord(l));
					if (node.isATargetString)
					{
						string val = l.GetString();
						sb.AppendLine(val + " => " + this.targetAndReplacement[val] + Environment.NewLine + "---------------------------------------");
					}

				}
				return sb.ToString();
			}
		}


		//parse: best case: Amortized O(n), with all unique strings. 
		//This is better than the naive O(m*n*o) where o is the longest length of the lookup strings. 
		//it would take an additional O(n * o) to generate the parser every time, but assuming it's stored
		//and immutable, that is only done once and is therefore amortized. 

		//technically this is the same time complexity as a simple iterative approach, but given a

	}

	public static class Helpers
	{
		public static void RemoveAfter<T>(this LinkedList<T> list, LinkedListNode<T> node)
		{
			while (node.Next != null)
			{
				list.Remove(node.Next);
			}
		}
	}

}

/*


--------------------Possibilities-----------------
*********************First Letter: \*************************
\ => \\
---------------------------------------

*********************First Letter: {*************************
{ => \{
---------------------------------------

*********************First Letter: <*************************
<b> => \b 
---------------------------------------
<br/> => \line 
---------------------------------------
<br /> => \line 
---------------------------------------
<br> => \line 
---------------------------------------
</b> => \b0 
---------------------------------------
</br> => 
---------------------------------------
</i> => \i0 
---------------------------------------
</em> => \i0 
---------------------------------------
</pre> => 
---------------------------------------
</ul> => \line 
---------------------------------------
</u> => \ul0 
---------------------------------------
</li> => 
---------------------------------------
<i> => \i 
---------------------------------------
<em> => \i 
---------------------------------------
<pre> => 
---------------------------------------
<ul> => 
---------------------------------------
<u> => \ul 
---------------------------------------
<li> => \line  \bullet  
---------------------------------------

*********************First Letter: 
*************************

 => \par 
---------------------------------------

 => \par 
---------------------------------------

*********************First Letter: 
*************************

 => \par 
---------------------------------------

*********************First Letter: 	*************************
	 => \tab 
---------------------------------------
*/