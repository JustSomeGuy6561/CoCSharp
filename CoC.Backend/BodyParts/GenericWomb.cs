using CoC.Backend.Pregnancies;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts
{
	//generic womb allows anal and vaginal pregnancies, but anal pregnancies are only allows if the source can anally impregnate. 
	public class GenericWomb : Womb
	{
		public GenericWomb() : base(new PregnancyStore(true), new PregnancyStore(false), null) {}
	}
}
