﻿//InDevelopmentExceptionThatBreaksOnRelease.cs
//Description:
//Author: JustSomeGuy
//3/21/2019, 12:35 PM
#if DEBUG
namespace CoC.Backend.Tools
{
	using System;
	using System.Diagnostics.CodeAnalysis;

	/// <summary>
	/// An exception that can be thrown before a member has been
	/// implemented, will cause the build to fail when not built in 
	/// debug mode.
	/// </summary>
	[Serializable]
	[SuppressMessage("Microsoft.Design",
	"CA1032:ImplementStandardExceptionConstructors",
	Justification = "Never used in production.")]
	public  class InDevelopmentExceptionThatBreaksOnRelease : Exception
	{
	}

}
#endif
