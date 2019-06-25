﻿//Tongue.cs
//Description:
//Author: JustSomeGuy
//1/6/2019, 1:26 AM
using CoC.Creatures;
using CoC.Tools;
using static CoC.UI.TextOutput;
namespace CoC.BodyParts
{
	public enum TonguePiercings { FRONT_CENTER, MIDDLE_CENTER, BACK_CENTER }
	internal class Tongue : PiercableBodyPart<Tongue, TongueType, TonguePiercings>
	{
#warning add update and update with message functions
		protected Tongue()
		{
			type = TongueType.HUMAN;
		}

		public override TongueType type { get; protected set; }

		public bool isLongTongue => type.longTongue;
		public static Tongue Generate()
		{
			return new Tongue();
		}

		public static Tongue Generate(TongueType tongueType)
		{
			return new Tongue()
			{
				type = tongueType
			};
		}

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
			OutputText(restoreString(player));
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
			else if (currentPiercingCount > 0 && !IsPiercedAt(piercingLocation))
			{
				return false;
			}
			else return true;
		}
	}
	internal partial class TongueType : PiercableBodyPartBehavior<TongueType, Tongue, TonguePiercings>
	{
		private static int indexMaker = 0;
		private readonly int _index;
		private readonly short length;

		protected TongueType(short tongueLength, SimpleDescriptor shortDesc, DescriptorWithArg<Tongue> fullDesc, TypeAndPlayerDelegate<Tongue> playerDesc, ChangeType<Tongue> transform, RestoreType<Tongue> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			length = tongueLength;
		}

		public bool longTongue => length >= 12;
		public override int index => _index;

		public static readonly TongueType HUMAN = new TongueType(4, HumanDesc, HumanFullDesc, HumanPlayerStr, HumanTransformStr, HumanRestoreStr);
		public static readonly TongueType SNAKE = new TongueType(6, SnakeDesc, SnakeFullDesc, SnakePlayerStr, SnakeTransformStr, SnakeRestoreStr);
		public static readonly TongueType DEMONIC = new TongueType(DemonicDesc, DemonicFullDesc, DemonicPlayerStr, DemonicTransformStr, DemonicRestoreStr);
		public static readonly TongueType DRACONIC = new TongueType(DraconicDesc, DraconicFullDesc, DraconicPlayerStr, DraconicTransformStr, DraconicRestoreStr);
		public static readonly TongueType ECHIDNA = new TongueType(EchidnaDesc, EchidnaFullDesc, EchidnaPlayerStr, EchidnaTransformStr, EchidnaRestoreStr);
		public static readonly TongueType LIZARD = new TongueType(LizardDesc, LizardFullDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr);
		public static readonly TongueType CAT = new TongueType(4, CatDesc, CatFullDesc, CatPlayerStr, CatTransformStr, CatRestoreStr);
	}
}