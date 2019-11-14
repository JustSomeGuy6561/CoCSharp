//Hands.cs
//Description:
//Author: JustSomeGuy
//12/26/2018, 7:58 PM

using CoC.Backend.CoC_Colors;
using System;

namespace CoC.Backend.BodyParts
{
	//technically claws do update, but we're not messing with them. it's possible to do, but i just dont feel it's necessary. The data change will never occur.
	public sealed partial class Hands : PartWithBehavioralEventsBase<Hands, HandType, HandWrapper>
	{
		public override string BodyPartName() => Name();

		public override HandType type { get; protected set; }

		public Tones clawTone => type.getClawTone(getArmWrapper(true).tone, getArmWrapper(false).tone);

		private readonly Func<bool, EpidermalData> getArmWrapper;
		internal Hands(Guid creatureID, HandType handType, Func<bool, EpidermalData> currentReadOnlyEpidermis) : base(creatureID)
		{
			type = handType;
			getArmWrapper = currentReadOnlyEpidermis;
		}

		public override HandWrapper AsReadOnlyReference()
		{
			return new HandWrapper(this);
		}

		public string FullDescription() => type.fullDescription(this);
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
		public virtual Tones getClawTone(Tones primaryTone, Tones secondaryTone)
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

		public static readonly HandType HUMAN = new HandType(HandStyle.HANDS, HumanShort, HumanLongDesc);
		public static readonly HandType LIZARD = new LizardClaws();
		public static readonly HandType DRAGON = new HandType(HandStyle.CLAWS, DragonShort, DragonLongDesc);
		public static readonly HandType SALAMANDER = new HandType(HandStyle.CLAWS, SalamanderShort, SalamanderLongDesc);
		public static readonly HandType CAT = new HandType(HandStyle.PAWS, CatShort, CatLongDesc);
		public static readonly HandType DOG = new HandType(HandStyle.PAWS, DogShort, DogLongDesc);
		public static readonly HandType FOX = new HandType(HandStyle.PAWS, FoxShort, FoxLongDesc);
		public static readonly HandType IMP = new ImpClaws();
		public static readonly HandType COCKATRICE = new HandType(HandStyle.CLAWS, CockatriceShort, CockatriceLongDesc);
		public static readonly HandType RED_PANDA = new HandType(HandStyle.PAWS, RedPandaShort, RedPandaLongDesc);
		public static readonly HandType FERRET = new HandType(HandStyle.PAWS, FerretShort, FerretLongDesc);
		public static readonly HandType GOO = new HandType(HandStyle.OTHER, GooShort, GooLongDesc);
		//public static readonly Hands MANTIS = new Hands(HandStyle.OTHER, MantisShort MantisLongDesc); //Not even remotely implemented.

		private class LizardClaws : HandType
		{

			public LizardClaws() : base(HandStyle.CLAWS, LizardShort, LizardLongDesc) { }

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
			public ImpClaws() : base(HandStyle.CLAWS, ImpShort, ImpLongDesc) { }

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

	public sealed class HandWrapper : PartWithBehavioralEventsWrapper<HandWrapper, Hands, HandType>
	{
		public Tones clawTone => sourceData.clawTone;

		public string fullDescription() => sourceData.FullDescription();

		public HandWrapper(Hands source) : base(source)
		{
		}
	}
}
