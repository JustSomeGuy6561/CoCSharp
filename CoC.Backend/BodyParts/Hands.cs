//Hands.cs
//Description:
//Author: JustSomeGuy
//12/26/2018, 7:58 PM

using CoC.Backend.CoC_Colors;
using System;

namespace CoC.Backend.BodyParts
{
	//technically claws do update, but we're not messing with them. it's possible to do, but i just dont feel it's necessary. The data change will never occur.
	public sealed partial class Hands : PartWithBehaviorAndEventBase<Hands, HandType, HandData>
	{
		public override string BodyPartName() => Name();

		public override HandType type { get; protected set; }

		public Tones clawTone => type.getClawTone(getArmData(true).tone, getArmData(false).tone);

		private readonly Func<bool, EpidermalData> getArmData;
		internal Hands(Guid creatureID, HandType handType, Func<bool, EpidermalData> currentEpidermalData) : base(creatureID)
		{
			type = handType;
			getArmData = currentEpidermalData;
		}

		public override HandData AsReadOnlyData()
		{
			return new HandData(this);
		}

		public SimpleDescriptor LongDescription => () => type.LongDescription(AsReadOnlyData());
	}

	public partial class HandType : BehaviorBase
	{
		private static int indexMaker = 0;

		protected enum HandStyle { HANDS, CLAWS, PAWS, OTHER }

		public readonly DescriptorWithArg<HandData> LongDescription;
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

		private protected HandType(HandStyle style, SimpleDescriptor shortDesc, DescriptorWithArg<HandData> longDesc) : base(shortDesc)
		{
			_index = indexMaker++;
			LongDescription = longDesc ?? throw new ArgumentNullException(nameof(longDesc));
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

	public sealed class HandData : BehavioralPartDataBase<HandType>
	{
		public readonly Tones clawTone;

		public bool isClaws => type.isClaws;
		public bool isPaws => type.isPaws;
		public bool isHands => type.isHands;

		//default case. never procs, though that may change in the future, idk.
		public bool isOther => type.isOther;

		public string LongDescription() => type.LongDescription(this);

		public HandData(Hands source) : base(GetID(source), GetBehavior(source))
		{
			this.clawTone = source.clawTone;
		}

		private static Guid GetID(Hands source)
		{
			if (source is null) throw new ArgumentNullException(nameof(source));
			return source.creatureID;
		}

		private static HandType GetBehavior(Hands source)
		{
			if (source is null) throw new ArgumentNullException(nameof(source));
			return source.type;
		}


	}
}
