using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Pregnancies
{
	public abstract partial class StandardSpawnType
	{
		//we'll grant these to derived classes, so protected, not private.
		protected string DefaultOviText(float amountConsumingElixirAdvancedPregnancy, byte OviElixirStrength)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		//only should be called if the current percent along is above threshold, and old percent is below threshold. 
		protected string GenericPregnancyText(float currentPercentAlong)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
	}
}
