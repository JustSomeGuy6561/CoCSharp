using CoC.Backend.CoC_Colors;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts
{
	public static class BodyHelpers
	{
		internal static FurColor GetValidFurColor(FurColor overrideColor, FurColor primary, HairFurColors activeHairColor, FurColor fallback)
		{
			return GetValidFurColor(overrideColor, primary, null, activeHairColor, fallback);
		}

		internal static FurColor GetValidFurColor(FurColor overrideColor, FurColor primary, FurColor secondary, HairFurColors activeHairColor, FurColor fallback)
		{
			if (!FurColor.IsNullOrEmpty(overrideColor))
			{
				return overrideColor;
			}
			else if (!FurColor.IsNullOrEmpty(secondary))
			{
				return secondary;
			}
			else if (!FurColor.IsNullOrEmpty(primary))
			{
				return primary;
			}
			else if (!HairFurColors.IsNullOrEmpty(activeHairColor))
			{
				return new FurColor(activeHairColor);
			}
			return fallback;
		}

		internal static Tones GetValidTone(Tones overrideTone, Tones primary, Tones fallback)
		{
			return GetValidTone(overrideTone, primary, null, fallback);
		}


		internal static Tones GetValidTone(Tones overrideTone, Tones primary, Tones secondary, Tones fallback)
		{
			if (!Tones.IsNullOrEmpty(overrideTone))
			{
				return overrideTone;
			}
			else if (!Tones.IsNullOrEmpty(secondary))
			{
				return secondary;
			}
			else if (!Tones.IsNullOrEmpty(primary))
			{
				return primary;
			}
			else return fallback;
		}
	}
}
