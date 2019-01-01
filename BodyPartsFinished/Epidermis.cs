//Epidermis.cs
//Description: Epidermis Sub-Body Part class. used in other body parts. 
//Author: JustSomeGuy
//12/26/2018, 7:58 PM
using CoC.BodyParts.SpecialInteraction;
using CoC.Tools;
using CoC.Items;
using static CoC.Strings.BodyParts.EpidermisString;
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
		public GenericDescriptorWithArg<Tones> shortDescriptionWithColor => (x) => { return (canTone() ? x.AsString() : "") + shortDescription(); };

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

		public static readonly Epidermis SKIN = new Epidermis(SkinStr);
		public static readonly Epidermis FUR = new Epidermis(FurStr);
		public static readonly Epidermis SCALES = new Epidermis(ScalesStr);
		public static readonly Epidermis GOO = new Epidermis(GooStr);
		public static readonly Epidermis WOOL = new Epidermis(WoolStr); //i'd like to merge this with fur but it's more trouble than it's worth
		public static readonly Epidermis FEATHERS = new Epidermis(FeathersStr);
		public static readonly Epidermis BARK = new Epidermis(BarkStr);
		public static readonly Epidermis CARAPACE = new Epidermis(CarapaceStr);
		public static readonly Epidermis EXOSKELETON = new Epidermis(ExoskeletonStr);

	}
}
