//Breasts.cs
//Description:
//Author: JustSomeGuy
//1/6/2019, 1:27 AM
using CoC.BodyParts.SpecialInteraction;
using CoC.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.BodyParts
{


	public class Breasts : IGrowShrinkable
	{
		public CupSize cupsize { get; protected set; }
		public Nipples nipples;

		public bool isMale => throw new NotImplementedException();

		public bool CanGrowPlus()
		{
			throw new NotImplementedException();
		}

		public bool CanReducto()
		{
			throw new NotImplementedException();
		}

		public float UseGroPlus()
		{
			throw new NotImplementedException();
		}

		public float UseReducto()
		{
			throw new NotImplementedException();
		}
	}
}
