using System;
using System.Runtime.Serialization;

namespace CoC.Backend.Save.Internals
{
	[DataContract]
	internal sealed class BackendGlobalData : SaveableData
	{
		//A static Guid. realistically, this probably should be constant, but i need some data to demonstrate using this. 
		//for new saves, generate a Guid manually, then copy it in.
		public Guid gameVersion = new Guid("8b4ea31c-f85b-49d1-8b4b-130725f09fab");
		public BackendGlobalData()
		{

		}

		internal BackendGlobalData(BackendGlobalSave saveData)
		{
			gameVersion = saveData.gameVersion;
		}

		public override Type currentSaveType => typeof(BackendGlobalSave);

		public override Type[] saveVersionTypes => new Type[] { typeof(BackendGlobalSave) };

		public override object ToCurrentSaveVersion()
		{
			return new BackendGlobalSave()
			{
				gameVersion = this.gameVersion
			};
		}
	}

	[DataContract]
	[KnownType(typeof(Guid))]
	public sealed class BackendGlobalSave : ISurrogateBase
	{
		//Empty as of right now - i have nothing for it.
		[DataMember]
		public Guid gameVersion;

		object ISurrogateBase.ToSaveable()
		{
			return new BackendGlobalData(this);
		}
	}

}
