using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Wearables;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts
{
	public enum LipPiercingLocation { LABRET, MEDUSA, MONROE_LEFT, MONROE_RIGHT, LOWER_LEFT_1, LOWER_LEFT_2, LOWER_RIGHT_1, LOWER_RIGHT_2 }
	public enum EyebrowPiercingLocation { LEFT_1, LEFT_2, RIGHT_1, RIGHT_2 }
	public enum NosePiercingLocation { LEFT_NOSTRIL, RIGHT_NOSTRIL, SEPTIMUS, BRIDGE }


	public class Face : BehavioralSaveablePart<Face, FaceType>
	{
		private const JewelryType SUPPORTED_LIP_JEWELRY = JewelryType.CURVED_BARBELL | JewelryType.STUD | JewelryType.RING;
		private const JewelryType SUPPORTED_NOSE_JEWELRY = JewelryType.STUD | JewelryType.RING | JewelryType.CURVED_BARBELL;
		private const JewelryType SUPPORTED_EYEBROW_JEWELRY = JewelryType.STUD | JewelryType.CURVED_BARBELL | JewelryType.RING;
		public override FaceType type { get; protected set; }
		public override bool isDefault => throw new NotImplementedException();

		public readonly Piercing<LipPiercingLocation> lipPiercings;
		public readonly Piercing<NosePiercingLocation> nosePiercings;
		public readonly Piercing<EyebrowPiercingLocation> eyebrowPiercings;

		private protected Face()
		{
			lipPiercings = new Piercing<LipPiercingLocation>(SUPPORTED_LIP_JEWELRY, LipPiercingUnlocked);
			nosePiercings = new Piercing<NosePiercingLocation>(SUPPORTED_NOSE_JEWELRY, NosePiercingUnlocked);
			eyebrowPiercings = new Piercing<EyebrowPiercingLocation>(SUPPORTED_EYEBROW_JEWELRY, EyebrowPiercingUnlocked);
		}

		internal static Face GenerateDefault()
		{
			return new Face();
		}

		internal override bool Restore()
		{
			throw new NotImplementedException();
		}

		private bool LipPiercingUnlocked(LipPiercingLocation piercingLocation)
		{
			return true;
		}
		private bool NosePiercingUnlocked(NosePiercingLocation piercingLocation)
		{
			return true;
		}

		private bool EyebrowPiercingUnlocked(EyebrowPiercingLocation piercingLocation)
		{
			return true;
		}

		internal override bool Validate(bool correctDataIfInvalid = false)
		{
			throw new NotImplementedException();
		}
		private protected override Type currentSaveVersion => throw new NotImplementedException();

		private protected override Type[] saveVersions => throw new NotImplementedException();

		private protected override BehavioralSurrogateBase<Face, FaceType> ToCurrentSave()
		{
			throw new NotImplementedException();
		}
	}

	public class FaceType : SaveableBehavior<FaceType, Face>
	{
		public FaceType(SimpleDescriptor shortDesc, DescriptorWithArg<Face> fullDesc, TypeAndPlayerDelegate<Face> playerDesc, ChangeType<Face> transform, RestoreType<Face> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
		}

		public override int index => throw new NotImplementedException();
	}
}
