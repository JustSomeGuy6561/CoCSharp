using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts.SpecialInteraction
{

	public interface IGenderListener : IGenderAware
	{
		string reactToChangeInGender(Gender oldGender);
	}
}
