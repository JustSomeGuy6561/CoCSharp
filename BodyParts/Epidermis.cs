//Epidermis.cs
//Description: Epidermis Sub-Body Part class. used in other body parts. 
//Author: JustSomeGuy
//12/26/2018, 7:58 PM
using CoC.BodyParts.SpecialInteraction;
using CoC.Tools;
using CoC.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.BodyParts
{
	//Epidermis: outer-most layer of human skin.
	//I'm using it to represent this type of thing
	//on the various species. So: skin, scales, fur, etc.

	//IMMUTABLE
	public class Epidermis : SimpleBodyPart, IImmutableDyeable, IImmutableToneable
	{
		private static int indexMaker = 0;

		protected readonly bool dyeable;
		protected readonly bool toneable;

		protected Epidermis(GenericDescription desc)
		{
			_index = indexMaker++;
			shortDescription = desc;
		}
		public override int index => _index;

		public override GenericDescription shortDescription { get; protected set; }

		protected readonly int _index;

		public virtual bool canTone()
		{
			return toneable;
		}
		//if you want this to fail for some reason based on 
		public virtual bool tryToTone(ref Tones currentTone, Tones newTone)
		{
			if (canTone())
			{
				currentTone = newTone;
				return currentTone == newTone;
			}
			return false;
		}
		public virtual bool canDye()
		{
			return dyeable;
		}
		public virtual bool tryToDye(ref Dyes currentColor, Dyes newColor)
		{
			if (canDye())
			{
				currentColor = newColor;
				return currentColor == newColor;
			}
			return false;
		}


		public static readonly Epidermis SKIN = new Skin();
		public static readonly Epidermis SCALES = new Scales();
		public static readonly Epidermis FUR = new Fur();
		public static readonly Epidermis FEATHERS = new Feathers();
		public static readonly Epidermis CARAPACE = new Carapace();
		/*
		private class Skin : Epidermis
		{
			public override GenericDescription shortDescription { get; protected set; }

			public Skin() : base()
			{
				shortDescription = 
			}

			public override bool canTone()
			{
				return true;
			}

			public override bool canDye()
			{
				return false;
			}

			public override bool tryToTone(ref Tones currentTone, Tones newTone)
			{
				currentTone = newTone;
				return currentTone == newTone;
			}

			public override bool tryToDye(ref Dyes currentColor, Dyes newColor)
			{
				return false;
			}
		}

		private class Scales : Epidermis
		{
			public override string GetDescriptor()
			{
				return "scales";
			}

			public Scales() : base() { }
		}

		private class Feathers : Epidermis
		{
			public override string GetDescriptor()
			{
				return "feathers";
			}

			public Feathers() : base() { }
		}

		private class Fur : Epidermis
		{
			public override string GetDescriptor()
			{
				return "fur";
			}

			public Fur() : base() { }
		}

		//Hard exoskeleton for things like a turtle or spiders or whatever.
		private class Carapace : Epidermis
		{
			public override string GetDescriptor()
			{
				return "hard";
			}
			public Carapace() : base() { }
		}
		*/
	}
}
