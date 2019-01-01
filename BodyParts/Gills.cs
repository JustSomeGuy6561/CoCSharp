//Gills.cs
//Description:
//Author: JustSomeGuy
//12/27/2018, 7:29 PM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoC.Tools;
using CoC.
namespace CoC.BodyParts
{
	public class Gills : BodyPartBehavior<Gills>
	{
		private static int indexMaker = 0;
		protected Gills(string desc)
		{
			descriptor = desc;
			_index = indexMaker++;
		}

		//public 

		protected readonly int _index;
		public override int index => _index;

		public override GenericDescription shortDescription {get; protected set;}
		public override CreatureDescription creatureDescription {get; protected set;}
		public override PlayerDescription playerDescription {get; protected set;}
		public override ChangeType<Gills> transformFrom {get; protected set;}

		public static readonly Gills NONE = new Gills();
		public static readonly Gills ANEMONE = new Gills("Anemone Gills");
		public static readonly Gills FISH = new Gills("Fish Gills");
	}
}
