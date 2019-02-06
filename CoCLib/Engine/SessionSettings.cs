//SessionSettings.cs
//Description:
//Author: JustSomeGuy
//1/28/2019, 2:23 AM
using CoC.BodyParts;
using CoC.Serialization;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CoC.Engine
{
	[DataContract]
	internal class SessionSettings
	{
		[DataMember]
		public PiercingFlags piercingFlags;
	}
}
