using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Items.Wearables.Tattoos;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Items.Wearables.Tattoos
{
	//a series of curves and vaguely rune-like shapes that seem to move around the players body. a full body tattoo for kitsune, mostly. they move around/coexist with other tattoos.
	public class RunicBodyTattoo : TattooBase
	{
		public RunicBodyTattoo() : base(TattooSize.FULL, false, false)
		{
		}

		public override Tones tattooColor => Tones.SILVER;

		public override bool Equals(TattooBase other)
		{
			return other is RunicBodyTattoo;
		}

		public override bool EqualsIgnoreColor(TattooBase other)
		{
			return other is RunicBodyTattoo;
		}

		public override string LongDescription(bool alternateFormat)
		{
			throw new NotImplementedException();
		}

		public override string ShortDescription(bool alternateFormat)
		{
			throw new NotImplementedException();
		}

		public override bool CanTattooOn<T>(TattooablePart<T> source)
		{
			return source is Backend.BodyParts.FullBodyTattoo;
		}
	}
}
