//Tongue.cs
//Description:
//Author: JustSomeGuy
//1/6/2019, 1:26 AM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoC.Tools;
using static CoC.UI.TextOutput;
using static CoC.Strings.BodyParts.TongueStrings;
namespace CoC.BodyParts
{
	public enum TonguePiercings { FRONT_CENTER, MIDDLE_CENTER, BACK_CENTER }
	public class Tongue : PiercableBodyPart<Tongue, TongueType, TonguePiercings>
	{
#warning add update and update with message functions
		protected Tongue(PiercingFlags flags) : base(flags)
		{

		}

		public override TongueType type { get; protected set; }

		public override bool Restore()
		{
			if (type == TongueType.HUMAN)
			{
				return false;
			}
			type = TongueType.HUMAN;
			return true;
		}

		public override bool RestoreAndDisplayMessage(Player player)
		{
			if (type == TongueType.HUMAN)
			{
				return false;
			}
			OutputText(restoreString(this, player));
			return Restore();
		}

		//could be a one-liner. written this way because maybe people wanna change it, idk.
		protected override bool PiercingLocationUnlocked(TonguePiercings piercingLocation)
		{
			if (piercingFlags.piercingFetishEnabled)
			{
				return true;
			}
			//allow one tongue piercing. must have fetish for more than that.
			else if (currentPiercingCount > 0 && !IsPierced(piercingLocation))
			{
				return false;
			}
			else return true;
		}
	}
	public class TongueType : PiercableBodyPartBehavior<TongueType, Tongue, TonguePiercings>
	{
		private static int indexMaker = 0;
		private readonly int _index;

		protected TongueType(GenericDescription shortDesc, FullDescription<Tongue> fullDesc, PlayerDescription<Tongue> playerDesc, ChangeType<Tongue> transform, ChangeType<Tongue> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
		}
		public override int index => _index;

		public static readonly TongueType HUMAN = new TongueType(HumanDesc, HumanFullDesc, HumanPlayerStr, HumanTransformStr, HumanRestoreStr);
		public static readonly TongueType SNAKE = new TongueType(SnakeDesc, SnakeFullDesc, SnakePlayerStr, SnakeTransformStr, SnakeRestoreStr);
		public static readonly TongueType DEMONIC = new TongueType(DemonicDesc, DemonicFullDesc, DemonicPlayerStr, DemonicTransformStr, DemonicRestoreStr);
		public static readonly TongueType DRACONIC = new TongueType(DraconicDesc, DraconicFullDesc, DraconicPlayerStr, DraconicTransformStr, DraconicRestoreStr);
		public static readonly TongueType ECHIDNA = new TongueType(EchidnaDesc, EchidnaFullDesc, EchidnaPlayerStr, EchidnaTransformStr, EchidnaRestoreStr);
		public static readonly TongueType LIZARD = new TongueType(LizardDesc, LizardFullDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr);
		public static readonly TongueType CAT = new TongueType(CatDesc, CatFullDesc, CatPlayerStr, CatTransformStr, CatRestoreStr);
	}
}
