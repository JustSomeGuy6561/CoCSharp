//Test.cs
//Description:
//Author: JustSomeGuy
//12/26/2018, 11:46 PM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CoC.BodyParts;

namespace CoC
{
	class Test
	{
		static void Main(string[] args)
		{
			Console.WriteLine("No Antennae: " + Antennae.NONE.GetDescriptor() + ", Index: " + Antennae.NONE.index);
			Console.WriteLine("Bee Antennae " + Antennae.BEE.GetDescriptor() + ", Index: " + Antennae.BEE.index);
			Console.WriteLine("Cockatrice Antennae: " + Antennae.COCKATRICE.GetDescriptor() + ", Index: " + Antennae.COCKATRICE.index);
			Console.WriteLine("Comparing Eyes: " + (Eyes.batman == Eyes.robin).ToString() + ", Should be true");
			Console.WriteLine("Comparing Eyes: " + (Eyes.batman == Eyes.nightwing).ToString() + ", Should be false");
			Console.WriteLine("Comparing Eyes: " + (Eyes.batman == EyeType.HUMAN).ToString() + ", Should be true");
			Console.WriteLine("Comparing Eyes: " + (Eyes.batman == EyeType.SPIDER).ToString() + ", Should be false");


			Console.ReadLine();
		}
	}
}
