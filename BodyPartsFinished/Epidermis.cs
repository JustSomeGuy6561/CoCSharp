﻿//EpidermisType.cs
//Description: EpidermisType Sub-Body Part class. used in other body parts. 
//Author: JustSomeGuy
//12/26/2018, 7:58 PM
using CoC.BodyParts.SpecialInteraction;
using CoC.Tools;
using CoC.Items;
using static CoC.Strings.BodyParts.EpidermisString;

namespace CoC.BodyParts
{
	//Epidermis represents the equivalent to skin on all species.

	/*
	 * NOTES: Epidermis can be attached to things that make use of it, like arms, core, etc.
	 * It is up to you to update it. This is done because it is more useful that way - the epidermis
	 * will always allow you to change its tone or fur color, to allow the most use for it.
	 * if you don't want this behavior for your body part, (for example, bee exoskeleton is always black)
	 * don't call these functions when you take care of updating your body part. so, if your arms react to
	 * changes in skin tone because your claws match the body color, but you don't want your arm scales or 
	 * whatever the epidermis is used for to update, simply dont call epidermis.reactToChangeInSkinColor
	 */

	public class Epidermis : SimpleBodyPart<EpidermisType>, IDyeable, IToneable, IToneAware, IFurAware
	{
		protected Epidermis(EpidermisType type) : base(type)
		{
			fur = FurColor.NO_FUR;
			tone = Tones.HUMAN_DEFAULT;
		}

		public static Epidermis Generate(EpidermisType type)
		{
			return new Epidermis(type);
		}

		public FurColor fur { get; protected set; }
		public Tones tone { get; protected set; }

		public virtual GenericDescription descriptionWithColor => () => { return (type.usesTone ? tone.AsString() : fur.AsString()) + " " + shortDescription(); };
		public virtual GenericDescription justColor => () => { return type.usesTone ? tone.AsString() : fur.AsString(); };

		public override EpidermisType type { get; protected set; }

		public bool canDye()
		{
			return type.usesFur && type.hairMutable;
		}

		public bool attemptToDye(HairFurColors dye)
		{
			if (canDye())
			{
				fur.UpdateFurColor(dye);
				return true;
			}
			return false;
		}

		public bool canToneLotion()
		{
			return type.usesTone && type.toneMutable;
		}

		public bool attemptToUseLotion(Tones newTone)
		{
			if (canDye())
			{
				tone = newTone;
				return true;
			}
			return false;
		}
		public void reactToChangeInSkinTone(Tones newTone)
		{
			tone = newTone;
		}

		public void reactToChangeInFurColor(FurColor furColor)
		{
			if (fur != furColor)
			{
				fur.UpdateFurColor(furColor);
			}
		}
	}


	//IMMUTABLE
	public class EpidermisType : SimpleBodyPartType
	{
		private static int indexMaker = 0;

		public readonly bool usesTone;
		public bool usesFur => !usesTone;

		protected readonly bool updateable;
		protected readonly int _index;

		public bool hairMutable => usesFur && updateable;
		public bool toneMutable => usesTone && updateable;

		protected EpidermisType(GenericDescription desc, bool isTone, bool canChange) : base(desc)
		{
			_index = indexMaker++;
			usesTone = isTone;
			updateable = canChange;
		}
		public override int index => _index;

		public static readonly EpidermisType SKIN = new EpidermisType(SkinStr, true, true);
		public static readonly EpidermisType FUR = new EpidermisType(FurStr, false, true);
		public static readonly EpidermisType SCALES = new EpidermisType(ScalesStr, true, true);
		public static readonly EpidermisType GOO = new EpidermisType(GooStr, true, true);
		public static readonly EpidermisType WOOL = new EpidermisType(WoolStr, false, true); //i'd like to merge this with fur but it's more trouble than it's worth
		public static readonly EpidermisType FEATHERS = new EpidermisType(FeathersStr, false, true);
		public static readonly EpidermisType BARK = new EpidermisType(BarkStr, true, true); //do you want the bark to change colors? idk? maybe make that false.
		public static readonly EpidermisType CARAPACE = new EpidermisType(CarapaceStr, true, true);
		//cannot be changed by lotion.
		public static readonly EpidermisType EXOSKELETON = new EpidermisType(ExoskeletonStr, true, false);

	}
}
