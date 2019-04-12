using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CoC.Backend.Wearables
{
	[DataContract]
	public abstract class JewelryMaterial
	{
		public readonly SimpleDescriptor materialName;
		public readonly SimpleDescriptor hueDescriptor;

		protected JewelryMaterial(SimpleDescriptor name, SimpleDescriptor hue)
		{
			materialName = name;
			hueDescriptor = hue;
		}
	}
}
