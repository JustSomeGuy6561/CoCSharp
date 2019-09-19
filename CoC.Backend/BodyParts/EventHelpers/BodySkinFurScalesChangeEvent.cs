using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts.EventHelpers
{
	public sealed class BodyOuterLayerChangedEventArg : EventArgs
	{
		public readonly EpidermalData oldSkinData; //skin is always available (under the fur, for example)
		public readonly EpidermalData oldPrimaryEpidermis;
		public readonly EpidermalData oldSecondaryEpidermis;

		public readonly BodyData bodyData;

		public BodyOuterLayerChangedEventArg(EpidermalData oldSkin, EpidermalData oldPrimary, EpidermalData oldSecondary, BodyData currentBody)
		{
			oldSkinData = oldSkin;
			oldPrimaryEpidermis = oldPrimary ?? throw new ArgumentNullException(nameof(oldPrimary));
			oldSecondaryEpidermis = oldSecondary ?? throw new ArgumentNullException(nameof(oldSecondary));
		}
	}

	public interface IBodyOuterLayerChangeListener
	{
		void OnBodyOuterLayerChanged(object sender, BodyOuterLayerChangedEventArg e);
	}
}
