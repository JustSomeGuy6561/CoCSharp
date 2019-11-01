using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.StatusEffect
{
	public abstract class ConditionalStatusEffect : StatusEffectBase
	{
		protected ConditionalStatusEffect(SimpleDescriptor name) : base(name)
		{
		}
	}
}
