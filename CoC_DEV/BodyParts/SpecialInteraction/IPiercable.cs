//IPiercable.cs
//Description: Interface marking body parts as pierceable.
//Author: JustSomeGuy
//12/30/2018, 12:34 AM

using CoC.Wearables.Piercings;

namespace  CoC.BodyParts.SpecialInteraction
{
	//Quick clarification: piercings are permanent in that it remains pierced even if no
	//jewelry is in this piercing. thus, once pierced, isPierced will return true.
	//HasJewelry will only return true if there is some jewelry in that piercing.

	interface IPiercable<PiercingLocationEnum> where PiercingLocationEnum : System.Enum
	{
		int maxPiercingCount { get; }

		int currentPiercingCount { get; }
		int currentJewelryCount { get; }

		bool Pierce(PiercingLocationEnum piercingLocation, PiercingJewelry jewelry);

		bool EquipPiercingJewelry(PiercingLocationEnum piercingLocation, PiercingJewelry jewelry, bool forceIfEnabled = false);
		bool EquipPiercingJewelryAndPierceIfNotPierced(PiercingLocationEnum piercingLocation, PiercingJewelry jewelry, bool forceIfEnabled = false);

		PiercingJewelry RemovePiercingJewelry(PiercingLocationEnum piercingLocation, bool forceRemove = false);

		//provides an option to disable certain piercings unless certain conditions are met. 
		bool canPierce(PiercingLocationEnum piercingLocation);

		bool IsPierced(PiercingLocationEnum piercingLocation);
		bool HasJewelry(PiercingLocationEnum piercingLocation);
	}
}
