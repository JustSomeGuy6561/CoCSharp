using CoC.Backend.Creatures;
using System;
using System.Runtime.Serialization;
using CoC.Backend.Save;

namespace CoC.Backend.Save.Internals
{
	[DataContract]
	public sealed class BackendSessionData : SaveableData
	{
		public static BackendSessionData data => SaveSystem.GetSessionData<BackendSessionData>();

		public int numTimesSaved { get; private set; }
		public long secondsPlayed { get; private set; }
		public bool piercingFetish { get; private set; }

		public Player player;

		internal BackendSessionData()
		{
			numTimesSaved = 0;
			secondsPlayed = 0;
			piercingFetish = false;
			player = null;
		}

		internal BackendSessionData(BackendSessionSave saveData)
		{
			numTimesSaved = saveData.numSaves;
			secondsPlayed = saveData.secondsPlayed;
			piercingFetish = saveData.piercingFetish;
			player = saveData.player;
		}


		public override Type currentSaveType => typeof(BackendSessionSave);

		public override Type[] saveVersionTypes => new Type[] { typeof(BackendSessionSave) };

		public override object ToCurrentSaveVersion()
		{
			return new BackendSessionSave()
			{
				numSaves = this.numTimesSaved,
				secondsPlayed = this.secondsPlayed,
				piercingFetish = this.piercingFetish,
				player = this.player
			};
		}
	}


	[DataContract]
	[KnownType(typeof(Player))]
	public sealed class BackendSessionSave : ISurrogateBase
	{
		internal BackendSessionSave() { }
		[DataMember]
		public int numSaves;
		[DataMember]
		public long secondsPlayed;
		[DataMember]
		public bool piercingFetish;
		[DataMember]
		public Player player;
		public object ToSaveable()
		{
			return new BackendSessionData(this);
		}
	}


}
