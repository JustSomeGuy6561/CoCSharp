namespace CoC.Backend.Settings.Gameplay
{
	public partial class SillyModeSettings
	{
		private static string SillySettingStr()
		{
			return "Silly Mode";
		}

		private partial class SillyModeLocal
		{
			private string UnsetTextStr()
			{
				return "Let Me Pick";
			}


			private string EnabledHintStr()
			{
				return "You will occasionally see silly, nonsensical, and/or wall-breaking things";
			}

			private string UnsetHintStr()
			{
				return "Text will default to non-silly versions, but some scenes may allow you to choose a silly variant";
			}

			private string DisabledHintStr()
			{
				return "You're an incorrigable stick in the mud with no sense of humor";
			}
		}

		private partial class SillyModeGlobal
		{
			private string UnsetTextStr()
			{
				return "Let Me Pick";
			}

			private string EnabledHintStr()
			{
				return "Any new games will start with Silly mode 'ON'. You can change this at any time. You can also choose a setting during character creation.";
			}

			private string DisabledHintStr()
			{
				return "Any new games will start with Silly mode 'OFF'. You can change this at any time. You can also choose a setting during character creation.";
			}

			private string UnsetHintStr()
			{
				return "Any new games will start with Silly mode 'LET ME PICK'. You can change this at any time. You can also choose a setting during character creation.";
			}
		}
	}
	

	
}
