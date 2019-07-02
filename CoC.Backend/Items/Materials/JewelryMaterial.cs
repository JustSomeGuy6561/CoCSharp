//JewelryMaterial.cs
//Description:
//Author: JustSomeGuy
//4/11/2019, 9:22 PM

namespace CoC.Backend.Items.Materials
{

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
