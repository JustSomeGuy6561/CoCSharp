using System;
using CoC.Backend.SaveData;

namespace CoC.Backend.Settings.Fetishes
{
	public sealed partial class PiercingFetish : FetishSetting
	{
		public PiercingFetish() : base(PiercingFetishName, new PiercingFetishSetting(false), new PiercingFetishSetting(true))
		{
		}

		public override void PostLocalSessionInit()
		{
			SimpleNullableSetting glob = (SimpleNullableSetting)globalSetting;
			SimpleNullableSetting sess = (SimpleNullableSetting)localSetting;

			sess.setting = glob.setting;
		}
	}

	public sealed partial class PiercingFetishSetting : SimpleNullableSetting
	{
		private BackendSessionSave session => BackendSessionSave.data;
		private BackendGlobalSave glob => BackendGlobalSave.data;

		private readonly bool isGlobal;

		internal PiercingFetishSetting(bool usesGlobal) : base()
		{
			isGlobal = usesGlobal;
		}

		public override bool? setting
		{
			get => isGlobal ? glob.PiercingFetishGlobal : session.piercingFetishStore;
			set
			{
				if (isGlobal)
				{
					glob.PiercingFetishGlobal = value;
				}
				else
				{
					session.piercingFetishStore = value;
				}
			}
		}


		public override string DisabledHint() => DisabledHintFn();

		public override string EnabledHint() => EnabledHintFn();

		public override string WarnPlayersAboutChanging()
		{
			return "";
		}

		public override bool SettingEnabled(bool? possibleSetting, out string whyNot)
		{
			//could have prevented resetting to null on local, but 
			//if you really want to reset the "ask me" flag, we'll let you. fuck it.

			whyNot = ""; //since we return true this is ignored.
			return true;
		}

		public override SimpleDescriptor unsetText => () => PiercingUnsetText(isGlobal);

		public override string UnsetHint() => PiercingUnsetHint(isGlobal);

	}


}
