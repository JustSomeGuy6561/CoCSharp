//Tongue.cs
//Description:
//Author: JustSomeGuy
//1/6/2019, 1:26 AM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.BodyParts
{
	public enum TonguePiercings {TONGUE_1, TONGUE_2}
	public class Tongue : PiercableBodyPart<Tongue, TongueType, TonguePiercings>
	{
	}
	public class TongueType : PiercableBodyPartBehavior<TongueType, Tongue, TonguePiercings>
	{

	}
}
