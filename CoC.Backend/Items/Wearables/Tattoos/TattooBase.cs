using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Items.Wearables.Tattoos
{
	public enum TattooSize { SMALL, MEDIUM, LARGE, FULL}

	public abstract class TattooBase : IEquatable<TattooBase>
	{
		public readonly bool scaleable;

		public readonly TattooSize tattooSize;

		//by default, we d
		protected readonly bool toneable;

		public abstract Tones tattooColor { get; }
		private Tones fallbackTattooColor => Tones.BLACK;

		public TattooBase(TattooSize size, bool toneable, bool scalesUp)
		{
			tattooSize = size;
			scaleable = scalesUp;

			this.toneable = toneable;
		}


		public abstract string ShortDescription(bool alternateFormat);
		public abstract string LongDescription(bool alternateFormat);

		//for generic tattoos that will work anywhere, this is fine. be sure to override this to only allow the target type (say, arms or whatever) for tattoos that only
		//work on one body part. it's always possible to derive this class with another abstract class (using arms, we might call it "ArmSleeves") that defines a group of tattoos
		//that only work for one part that all follow the same ruleset.
		public virtual bool CanTattooOn<T>(TattooablePart<T> source) where T:TattooLocation
		{
			//for type specific:
			//return T is ArmTattoos;
			return true;
		}

		public abstract bool Equals(TattooBase other);

		public abstract bool EqualsIgnoreColor(TattooBase other);

		public static bool MatchingTattoos(TattooBase first, TattooBase second)
		{
			if (first is null || second is null)
			{
				return false;
			}
			return first.Equals(second);
		}

		public static bool MatchingTattoosIgnoreColor(TattooBase first, TattooBase second)
		{
			if (first is null || second is null)
			{
				return false;
			}
			return first.EqualsIgnoreColor(second);
		}
	}

	public abstract class GenericTattooBase : TattooBase
	{
		public GenericTattooBase(TattooSize size, bool scalesUp) : base(size, false, scalesUp)
		{
		}

		public override bool EqualsIgnoreColor(TattooBase other)
		{
			return Equals(other);
		}
	}

	public abstract class GenericToneableTattooBase : TattooBase
	{
		public override Tones tattooColor { get; }

		public GenericToneableTattooBase(TattooSize size, Tones inkColor, bool scalesUp) : base(size, true, scalesUp)
		{
			tattooColor = Tones.IsNullOrEmpty(inkColor) ? defaultTone : inkColor;
		}

		protected internal virtual Tones defaultTone => Tones.BLACK;
	}
}
