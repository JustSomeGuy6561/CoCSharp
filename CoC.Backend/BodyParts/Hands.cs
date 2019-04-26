//Hands.cs
//Description:
//Author: JustSomeGuy
//12/26/2018, 7:58 PM

using CoC.Backend.CoC_Colors;
using System;

namespace CoC.Backend.BodyParts
{
	
	public sealed class Hands : BehavioralPartBase<HandType>
	{

		public override HandType type { get; protected set; }

		public Tones clawTone => type.getClawTone(getArmData(true).tone, getArmData(false).tone);

		private readonly Func<bool, EpidermalData> getArmData;
		private Hands(HandType handType, Func<bool, EpidermalData> currentEpidermalData)
		{
			type = handType;
			getArmData = currentEpidermalData;
		}

		public SimpleDescriptor fullDescription => () => type.fullDescription(this);

		internal static Hands Generate(HandType handType, Func<bool, EpidermalData> currentEpidermalData)
		{
			return new Hands(handType, currentEpidermalData);
		}

		public void UpdateHands(HandType newType)
		{
			type = newType;
		}
	}

	public partial class HandType : BehaviorBase
	{
		private static int indexMaker = 0;

		protected enum HandStyle { HANDS, CLAWS, PAWS, OTHER }

		public DescriptorWithArg<Hands> fullDescription;
		public override int index => _index;
		protected readonly int _index;

		public virtual bool canTone()
		{
			return false;
		}
		public bool isClaws => handStyle == HandStyle.CLAWS;
		public bool isPaws => handStyle == HandStyle.PAWS;
		public bool isHands => handStyle == HandStyle.HANDS;

		//default case. never procs, though that may change in the future, idk.
		public bool isOther => !(isClaws || isHands || isPaws);

		protected readonly HandStyle handStyle;
		public virtual Tones getClawTone (Tones primaryTone, Tones secondaryTone)
		{
			if (canTone())
			{
				return primaryTone;
			}
			return Tones.NOT_APPLICABLE;
		}

		private protected HandType(HandStyle style, SimpleDescriptor shortDesc, DescriptorWithArg<Hands> fullDesc) : base(shortDesc)
		{
			_index = indexMaker++;
			fullDescription = fullDesc;
			handStyle = style;
		}

		public static readonly HandType HUMAN = new HandType(HandStyle.HANDS, HumanShort, HumanFullDesc);
		public static readonly HandType LIZARD = new LizardClaws();
		public static readonly HandType DRAGON = new HandType(HandStyle.CLAWS, DragonShort, DragonFullDesc);
		public static readonly HandType SALAMANDER = new HandType(HandStyle.CLAWS, SalamanderShort, SalamanderFullDesc);
		public static readonly HandType CAT = new HandType(HandStyle.PAWS, CatShort, CatFullDesc);
		public static readonly HandType DOG = new HandType(HandStyle.PAWS, DogShort, DogFullDesc);
		public static readonly HandType FOX = new HandType(HandStyle.PAWS, FoxShort, FoxFullDesc);
		public static readonly HandType IMP = new ImpClaws();
		public static readonly HandType COCKATRICE = new HandType(HandStyle.CLAWS, CockatriceShort, CockatriceFullDesc);
		public static readonly HandType RED_PANDA = new HandType(HandStyle.PAWS, RedPandaShort, RedPandaFullDesc);
		public static readonly HandType FERRET = new HandType(HandStyle.PAWS, FerretShort, FerretFullDesc);
		public static readonly HandType GOO = new HandType(HandStyle.OTHER, GooShort, GooFullDesc);
		//public static readonly Hands MANTIS = new Hands(HandStyle.OTHER, MantisShort MantisFullDesc); //Not even remotely implemented.

		private class LizardClaws : HandType
		{

			public LizardClaws() : base(HandStyle.CLAWS, LizardShort, LizardFullDesc) { }

			public override bool canTone()
			{
				return true;
			}

			public override Tones getClawTone(Tones primaryTone, Tones secondaryTone)
			{
				//do some magic to the tone to make it lizard claw compatible
				return primaryTone;
			}
		}

		private class ImpClaws : HandType
		{
			public ImpClaws() : base(HandStyle.CLAWS, ImpShort, ImpFullDesc) { }

			public override bool canTone()
			{
				return true;
			}

			public override Tones getClawTone(Tones primaryTone, Tones secondaryTone)
			{
				//do some magic to the tone to make it imp claw compatible
				return primaryTone;
			}
		}
	}
}
