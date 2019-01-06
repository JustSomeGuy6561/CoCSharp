//Vagina.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 5:57 PM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoC.Strings;
using CoC.Tools;
using static CoC.Strings.BodyParts.ClitVaginaStrings;
using static CoC.UI.TextOutput;
namespace CoC.BodyParts
{
	public enum LabiaPiercings
	{
		LEFT_1, LEFT_2, LEFT_3, LEFT_4, LEFT_5, LEFT_6,
		RIGHT_1, RIGHT_2, RIGHT_3, RIGHT_4, RIGHT_5, RIGHT_6
	}

	public class Vagina : PiercableBodyPart<Vagina, VaginaType, LabiaPiercings>
	{
		public readonly Clit clit;
		public bool virgin { get; protected set; }
		protected Vagina(PiercingFlags flags) : base(flags)
		{
			clit = Clit.Generate(flags);
			virgin = true;
			type = VaginaType.HUMAN;
		}

		public static  Vagina GenerateDefault(PiercingFlags flags)
		{
			return new Vagina(flags);
		}

		public static Vagina Generate(PiercingFlags flags, VaginaType vaginaType)
		{
			return new Vagina(flags)
			{
				type = vaginaType
			};
		}

		public static Vagina GenerateOmnibus(PiercingFlags flags)
		{
			Vagina retVal = new Vagina(flags);
			retVal.ActivateOmnibusClit();
			return retVal;
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
			OutputText(newType.transformFrom(this, player));
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

		public bool canPierce(ClitPiercing piercingLocation)
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
			OutputText(restoreString(this, player));
			type = VaginaType.HUMAN;
			return true;
		}
		#endregion
	}

	public class VaginaType : PiercableBodyPartBehavior<VaginaType, Vagina, LabiaPiercings>
	{
		private static int indexMaker = 0;
		public readonly int typeCapacityBonus;
		protected VaginaType(int capacityBonus,
			GenericDescription shortDesc, CreatureDescription<Vagina> creatureDesc, PlayerDescription<Vagina> playerDesc, 
			ChangeType<Vagina> transform, ChangeType<Vagina> restore) : base(shortDesc, creatureDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			typeCapacityBonus = capacityBonus;
		}

		public override int index => _index;
		protected readonly int _index;

		public static readonly VaginaType HUMAN = new VaginaType(0, VagHumanDesc, VagHumanCreatureStr, VagHumanPlayerStr, GlobalStrings.TransformToDefault<Vagina, VaginaType>, GlobalStrings.RevertAsDefault<Vagina>);
		public static readonly VaginaType EQUINE = new VaginaType(0, VagEquineDesc, VagEquineCreatureStr, VagEquinePlayerStr, VagEquineTransformStr, VagEquineRestoreStr);
		public static readonly VaginaType SAND_TRAP = new VaginaType(0, VagSandTrapDesc, VagSandTrapCreatureStr, VagSandTrapPlayerStr, VagSandTrapTransformStr, VagSandTrapRestoreStr);

	}

}
