using CoC.Backend.Tools;

namespace CoC.Backend.BodyParts.SpecialInteraction
{
	public sealed class FemininityData
	{
		public readonly Femininity femininity;

		//enums are passed by value, so this should be fine.
		internal FemininityData(Femininity fem)
		{
			femininity = fem.copy();
		}
	}
}
