using System;
using System.Runtime.Serialization;

namespace CoC.Backend.Save
{
	[DataContract]
	public sealed class BackendSessionData : SaveableData
	{
		public static BackendSessionData data => SaveSystem.GetSessionData<BackendSessionData>();

		public int numTimesSaved { get; private set; }
		public long secondsPlayed { get; private set; }
		public bool piercingFetish { get; private set; }

		internal BackendSessionData()
		{
			numTimesSaved = 0;
			secondsPlayed = 0;
			piercingFetish = false;
		}

		internal BackendSessionData(BackendSessionSave saveData)
		{
			numTimesSaved = saveData.numSaves;
			secondsPlayed = saveData.secondsPlayed;
			piercingFetish = saveData.piercingFetish;
		}


		public override Type currentSaveType => typeof(BackendSessionSave);

		public override Type[] saveVersionTypes => new Type[] { typeof(BackendSessionSave) };

		public override object ToCurrentSaveVersion()
		{
			return new BackendSessionSave()
			{
				numSaves = this.numTimesSaved,
				secondsPlayed = this.secondsPlayed,
				piercingFetish = this.piercingFetish
			};
		}
	}


	[DataContract]
	public sealed class BackendSessionSave : ISurrogateBase
	{
		internal BackendSessionSave() { }
		[DataMember]
		public int numSaves;
		[DataMember]
		public long secondsPlayed;
		[DataMember]
		public bool piercingFetish;

		public object ToSaveable()
		{
			return new BackendSessionData(this);
		}
	}


}
