//Vagina.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 5:57 PM
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System.Collections.Generic;

namespace CoC.Backend.BodyParts
{
	public enum LabiaPiercings
	{
		LEFT_1, LEFT_2, LEFT_3, LEFT_4, LEFT_5, LEFT_6,
		RIGHT_1, RIGHT_2, RIGHT_3, RIGHT_4, RIGHT_5, RIGHT_6
	}

	public sealed class Vagina : BehavioralSaveablePart<Vagina, VaginaType>
	{
		private const JewelryType SUPPORTED_LABIA_JEWELRY = JewelryType.BARBELL_STUD | JewelryType.RING | JewelryType.SPECIAL;
		public readonly Clit clit;
		public bool virgin { get; private set; }

		public readonly Piercing<LabiaPiercings> labiaPiercings;

		private Vagina()
		{
			clit = Clit.Generate();
			virgin = true;
			type = VaginaType.HUMAN;
			labiaPiercings = new Piercing<LabiaPiercings>(SUPPORTED_LABIA_JEWELRY, PiercingLocationUnlocked);
		}
		public override VaginaType type { get; protected set; }

		internal static Vagina GenerateFromGender(Gender gender)
		{
			if (gender.HasFlag(Gender.FEMALE)) return new Vagina();
			else return null;
		}

		internal static Vagina GenerateDefault()
		{
			return new Vagina();
		}

		internal static Vagina GenerateDefaultOfType(VaginaType vaginaType)
		{
			return new Vagina()
			{
				type = vaginaType
			};
		}

		internal static Vagina Generate(VaginaType vaginaType, float clitLength)
		{
			Vagina retVal = new Vagina()
			{
				type = vaginaType
			};
			retVal.clit.growClit(clitLength - retVal.clit.length);
			return retVal;
		}

		internal static Vagina GenerateOmnibus(VaginaType vaginaType)
		{
			Vagina retVal = new Vagina()
			{
				type = vaginaType,
			};
			retVal.ActivateOmnibusClit();
			return retVal;
		}

		public override bool isDefault => type == VaginaType.HUMAN;

		internal bool Deflower()
		{
			if (!virgin)
			{
				return false;
			}
			virgin = false;
			return true;
		}

		internal bool UpdateType(VaginaType newType)
		{
			if (type == newType)
			{
				return false;
			}
			type = newType;
			return true;
		}

		#region aliases
		public bool omnibusClit => clit.omnibusClit;

		public bool ActivateOmnibusClit()
		{
			return clit.ActivateOmnibusClit();
		}

		public bool DeactivateOmnibusClit()
		{
			return clit.DeactivateOmnibusClit();
		}
		#endregion

		#region Piercing-Related
		private bool PiercingLocationUnlocked(LabiaPiercings piercingLocation)
		{
			return true;
		}
		#endregion

		#region Restore
		internal override bool Restore()
		{
			clit.Restore();
			if (type == VaginaType.HUMAN)
			{
				return false;
			}
			type = VaginaType.HUMAN;
			return true;
		}

		#endregion
		internal override bool Validate(bool correctDataIfInvalid = false)
		{
			VaginaType vaginaType = type;
			bool valid = VaginaType.Validate(ref vaginaType, correctDataIfInvalid);
			if (valid || correctDataIfInvalid)
			{
				valid &= labiaPiercings.Validate();
			}
			if (valid || correctDataIfInvalid)
			{
				valid &= clit.Validate(correctDataIfInvalid);
			}
			return valid;
		}
	}

	public sealed partial class VaginaType : SaveableBehavior<VaginaType, Vagina>
	{
		private static int indexMaker = 0;
		private static readonly List<VaginaType> types = new List<VaginaType>();
		public readonly int typeCapacityBonus;
		private VaginaType(int capacityBonus,
			SimpleDescriptor shortDesc, DescriptorWithArg<Vagina> fullDesc, TypeAndPlayerDelegate<Vagina> playerDesc,
			ChangeType<Vagina> transform, RestoreType<Vagina> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			typeCapacityBonus = capacityBonus;
			types.AddAt(this, _index);
		}

		internal static bool Validate(ref VaginaType vaginaType, bool correctInvalidData = false)
		{
			if (types.Contains(vaginaType))
			{
				return true;
			}
			else if (correctInvalidData)
			{
				vaginaType = HUMAN;
			}
			return false;
		}

		public override int index => _index;
		private readonly int _index;

		public static readonly VaginaType HUMAN = new VaginaType(0, VagHumanDesc, VagHumanFullDesc, VagHumanPlayerStr, GlobalStrings.TransformToDefault<Vagina, VaginaType>, GlobalStrings.RevertAsDefault);
		public static readonly VaginaType EQUINE = new VaginaType(0, VagEquineDesc, VagEquineFullDesc, VagEquinePlayerStr, VagEquineTransformStr, VagEquineRestoreStr);
		public static readonly VaginaType SAND_TRAP = new VaginaType(0, VagSandTrapDesc, VagSandTrapFullDesc, VagSandTrapPlayerStr, VagSandTrapTransformStr, VagSandTrapRestoreStr);

	}

}
