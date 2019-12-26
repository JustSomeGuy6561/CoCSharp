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

		public string HandText(bool plural = true) => type.HandText(plural);
		public string NailsText(bool plural = true) => type.NailsText(plural);

		public string ShortDescription(bool plural) => type.ShortDescription(plural);



		public string LongDescription() => type.LongDescription(AsReadOnlyData());
		public string LongDescription(bool alternateFormat) => type.LongDescription(AsReadOnlyData(), alternateFormat);

		public string LongPrimaryDescription() => type.LongPrimaryDescription(AsReadOnlyData());
		public string LongAlternateDescription() => type.LongAlternateDescription(AsReadOnlyData());

		public string LongDescription(bool alternateFormat, bool plural) => type.LongDescription(AsReadOnlyData(), alternateFormat, plural);

		public string LongPrimaryDescription(bool plural) => type.LongPrimaryDescription(AsReadOnlyData(), plural);
		public string LongAlternateDescription(bool plural) => type.LongAlternateDescription(AsReadOnlyData(), plural);
	}

	public partial class HandType : BehaviorBase
	{
		private static int indexMaker = 0;

		protected enum HandStyle { HANDS, CLAWS, PAWS, OTHER }

		private readonly PluralPartDescriptor<HandData> longDescription;

		private readonly ShortPluralDescriptor shortPluralDesc;
		private readonly ShortPluralDescriptor handStr;
		private readonly ShortPluralDescriptor nailsStr;

		//Text for when you want to say 'hand' but hand my not be accurate. it'll return things like 'clawed hand' or 'talon' or 'gooey "hand"'
		//that still make it clear you're talking about the 'hand', but still respecting the actual hand type.
		public string HandText(bool plural) => handStr(plural);
		public string NailsText(bool plural) => nailsStr(plural);

		public string ShortDescription(bool plural) => shortPluralDesc(plural);


		public string LongDescription(HandData handData) => longDescription(handData);
		public string LongDescription(HandData handData, bool alternateFormat) => longDescription(handData, alternateFormat);

		public string LongPrimaryDescription(HandData handData) => longDescription(handData, false);
		public string LongAlternateDescription(HandData handData) => longDescription(handData, true);

		public string LongDescription(HandData handData, bool alternateFormat, bool plural) => longDescription(handData, alternateFormat, plural);

		public string LongPrimaryDescription(HandData handData, bool plural) => longDescription(handData, false, plural);
		public string LongAlternateDescription(HandData handData, bool plural) => longDescription(handData, true, plural);

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

		private protected HandType(HandStyle style, ShortPluralDescriptor nounText, ShortPluralDescriptor nailsText, ShortPluralDescriptor shortDesc, SimpleDescriptor singleDesc,
			PluralPartDescriptor<HandData> longDesc) : base(PluralHelper(shortDesc), singleDesc)
		{
			_index = indexMaker++;
			longDescription = longDesc ?? throw new ArgumentNullException(nameof(longDesc));
			handStr = nounText ?? throw new ArgumentNullException(nameof(nounText));
			shortPluralDesc = shortDesc;

			nailsStr = nailsText;
			handStyle = style;
		}

		public static readonly HandType HUMAN = new HandType(HandStyle.HANDS, HumanNoun, HumanNails, HumanShort, HumanSingle, HumanLongDesc);
		public static readonly HandType LIZARD = new LizardClaws();
		public static readonly HandType DRAGON = new HandType(HandStyle.CLAWS, DragonNoun, DragonNails, DragonShort, DragonSingle, DragonLongDesc);
		public static readonly HandType SALAMANDER = new HandType(HandStyle.CLAWS, SalamanderNoun, SalamanderNails, SalamanderShort, SalamanderSingle, SalamanderLongDesc);
		public static readonly HandType CAT = new HandType(HandStyle.PAWS, CatNoun, CatNails, CatShort, CatSingle, CatLongDesc);
		public static readonly HandType DOG = new HandType(HandStyle.PAWS, DogNoun, DogNails, DogShort, DogSingle, DogLongDesc);
		public static readonly HandType FOX = new HandType(HandStyle.PAWS, FoxNoun, FoxNails, FoxShort, FoxSingle, FoxLongDesc);
		public static readonly HandType IMP = new ImpClaws();
		public static readonly HandType COCKATRICE = new HandType(HandStyle.CLAWS, CockatriceNoun, CockatriceNails, CockatriceShort, CockatriceSingle, CockatriceLongDesc);
		public static readonly HandType RED_PANDA = new HandType(HandStyle.PAWS, RedPandaNoun, RedPandaNails, RedPandaShort, RedPandaSingle, RedPandaLongDesc);
		public static readonly HandType FERRET = new HandType(HandStyle.PAWS, FerretNoun, FerretNails, FerretShort, FerretSingle, FerretLongDesc);
		public static readonly HandType GOO = new HandType(HandStyle.OTHER, GooNoun, GooNails, GooShort, GooSingle, GooLongDesc);
		//public static readonly Hands MANTIS = new Hands(HandStyle.OTHER, MantisNoun, MantisNails, MantisShort MantisLongDesc); //Not even remotely implemented.

		private class LizardClaws : HandType
		{

			public LizardClaws() : base(HandStyle.CLAWS, LizardNoun, LizardNails, LizardShort, LizardSingle, LizardLongDesc) { }

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
			public ImpClaws() : base(HandStyle.CLAWS, ImpNoun, ImpNails, ImpShort, ImpSingle, ImpLongDesc) { }

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

		public string HandText(bool plural) => type.HandText(plural);
		public string NailsText(bool plural) => type.NailsText(plural);

		public string ShortDescription(bool plural) => type.ShortDescription(plural);



		public string LongDescription() => type.LongDescription(this);
		public string LongDescription(bool alternateFormat) => type.LongDescription(this, alternateFormat);

		public string LongPrimaryDescription() => type.LongPrimaryDescription(this);
		public string LongAlternateDescription() => type.LongAlternateDescription(this);

		public string LongDescription(bool alternateFormat, bool plural) => type.LongDescription(this, alternateFormat, plural);

		public string LongPrimaryDescription(bool plural) => type.LongPrimaryDescription(this, plural);
		public string LongAlternateDescription(bool plural) => type.LongAlternateDescription(this, plural);


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
