﻿using CoC.Backend.Creatures;
using System;

namespace CoC.Backend.BodyParts
{
	public sealed class EmptyWomb : Womb
	{
		public EmptyWomb(Guid creatureID ) : base(creatureID, null, null, null) { }

		protected override bool ExtraValidations(bool currentlyValid, bool correctInvalidData)
		{
			return true;
		}
	}
}
