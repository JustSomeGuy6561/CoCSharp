using CoC.Backend.Creatures;
using CoC.Backend.Pregnancies;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts
{
	//generic womb allows anal and vaginal pregnancies, but anal pregnancies are only allows if the source can anally impregnate. 
	public class GenericWomb : Womb
	{
		public GenericWomb(Guid creatureID) : base(creatureID, new VaginalPregnancyStore(creatureID, 0), new AnalPregnancyStore(creatureID), null) {}

		protected override bool ExtraValidations(bool currentlyValid, bool correctInvalidData)
		{
			return true;
		}
	}
}
