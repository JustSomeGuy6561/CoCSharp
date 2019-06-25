﻿//Vagina.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 5:57 PM
using CoC.Creatures;
using CoC.Strings;
using CoC.Tools;
using static CoC.UI.TextOutput;
namespace CoC.BodyParts
{
	public enum LabiaPiercings
	{
		LEFT_1, LEFT_2, LEFT_3, LEFT_4, LEFT_5, LEFT_6,
		RIGHT_1, RIGHT_2, RIGHT_3, RIGHT_4, RIGHT_5, RIGHT_6
	}

	internal class Vagina : PiercableBodyPart<Vagina, VaginaType, LabiaPiercings>
	{
#warning add all the helpers.
		public readonly Clit clit;
		public bool virgin { get; protected set; }
		protected Vagina()
		{
			clit = Clit.Generate();
			virgin = true;
			type = VaginaType.HUMAN;
		}

		public static Vagina GenerateDefault()
		{
			return new Vagina();
		}

		public static Vagina Generate(VaginaType vaginaType)
		{
			return new Vagina()
			{
				type = vaginaType
			};
		}

		public static Vagina Generate(VaginaType vaginaType, float clitLength)
		{
			Vagina retVal = new Vagina()
			{
				type = vaginaType
			};
			retVal.clit.growClit(clitLength - retVal.clit.length);
			return retVal;
		}

		public static Vagina GenerateOmnibus()
		{
			Vagina retVal = new Vagina();
			retVal.ActivateOmnibusClit();
			return retVal;
		}

		public bool Deflower()
		{
			if (!virgin)
			{
				return false;
			}
			virgin = false;
			return true;
		}

		public bool Update(VaginaType newType)
		{
			if (type == newType)
			{
				return false;
			}
			type = newType;
			return true;
		}

		public bool UpdateTypeAndDisplayMessage(VaginaType newType, Player player)
		{
			if (type == newType)
			{
				return false;
			}
			OutputText(transformInto(newType, player));
			type = newType;
			return true;
		}

		public override VaginaType type { get; protected set; }

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
		protected override bool PiercingLocationUnlocked(LabiaPiercings piercingLocation)
		{
			return true;
		}

		public bool canPierce(ClitPiercings piercingLocation)
		{
			return clit.canPierce(piercingLocation);
		}
		#endregion

		#region Restore
		public override bool Restore()
		{
			clit.Restore();
			if (type == VaginaType.HUMAN)
			{
				return false;
			}
			type = VaginaType.HUMAN;
			return true;
		}

		public override bool RestoreAndDisplayMessage(Player player)
		{
			clit.Restore();
			if (type == VaginaType.HUMAN)
			{
				return false;
			}
			OutputText(restoreString(player));
			type = VaginaType.HUMAN;
			return true;
		}
		#endregion
	}

	internal partial class VaginaType : PiercableBodyPartBehavior<VaginaType, Vagina, LabiaPiercings>
	{
		private static int indexMaker = 0;
		public readonly int typeCapacityBonus;
		protected VaginaType(int capacityBonus,
			SimpleDescriptor shortDesc, DescriptorWithArg<Vagina> fullDesc, TypeAndPlayerDelegate<Vagina> playerDesc,
			ChangeType<Vagina> transform, RestoreType<Vagina> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			typeCapacityBonus = capacityBonus;
		}

		public override int index => _index;
		protected readonly int _index;

		public static readonly VaginaType HUMAN = new VaginaType(0, VagHumanDesc, VagHumanFullDesc, VagHumanPlayerStr, GlobalStrings.TransformToDefault<Vagina, VaginaType>, GlobalStrings.RevertAsDefault<Vagina>);
		public static readonly VaginaType EQUINE = new VaginaType(0, VagEquineDesc, VagEquineFullDesc, VagEquinePlayerStr, VagEquineTransformStr, VagEquineRestoreStr);
		public static readonly VaginaType SAND_TRAP = new VaginaType(0, VagSandTrapDesc, VagSandTrapFullDesc, VagSandTrapPlayerStr, VagSandTrapTransformStr, VagSandTrapRestoreStr);

	}

}