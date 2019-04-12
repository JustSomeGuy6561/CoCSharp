using System;
using System.Runtime.Serialization;

namespace CoC.Backend.Save
{
	//serialization is super funky with interfaces. this is a basic implementation of it to get around that. inherit this instead.
	//note that c# doesn't allow double inheritance, so copy-pasting this elsewhere also works, as long as it is into an abstract class.
	[DataContract]
	public abstract class SaveClassBase : ISaveableBase
	{
		//guarentees it's added to the DataContractSystem, which makes this a hell of a lot easier to manage. 
		protected SaveClassBase()
		{
			DataContractSystem.AddSurrogateData(this);
		}

		Type ISaveableBase.currentSaveType => currentSave;

		Type[] ISaveableBase.saveVersionTypes => saveVersions;

		object ISaveableBase.ToCurrentSaveVersion()
		{
			return ToCurrentSave();
		}

		protected abstract Type currentSave { get; }
		protected abstract Type[] saveVersions { get; }
		protected abstract object ToCurrentSave();
	}
}
