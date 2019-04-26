using CoC.Backend.Tools;

namespace CoC.Backend.BodyParts.SpecialInteraction
{
	public sealed class GenderData
	{
		public readonly Femininity femininity;
		public readonly Gender gender;

		//enums are passed by value, so this should be fine.
		internal GenderData(Femininity fem, Gender g)
		{
			femininity = fem.copy();
			gender = g;
		}
	}
}
