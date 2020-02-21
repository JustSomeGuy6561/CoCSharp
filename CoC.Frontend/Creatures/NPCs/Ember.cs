using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Frontend.SaveData;

namespace CoC.Frontend.Creatures.NPCs
{
	class Ember
	{
		private static FrontendSessionSave data => FrontendSessionSave.data;

		public static bool hatched => data.emberHatched;

		public static Gender gender => data.EmberGender;

	}
}
