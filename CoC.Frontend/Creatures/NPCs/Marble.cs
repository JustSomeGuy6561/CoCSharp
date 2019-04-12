using CoC.Frontend.SaveData;

namespace CoC.Frontend.Creatures.NPCs
{
	internal sealed class Marble
	{
		private static FrontendSessionSave data => FrontendSessionSave.data;

		public static bool isUnlocked => data.MarbleUnlocked;
		public static bool isLover => data.MarbleIsLover;
		public static bool isDisabled => data.MarbleIsDisabled;



	}
}
