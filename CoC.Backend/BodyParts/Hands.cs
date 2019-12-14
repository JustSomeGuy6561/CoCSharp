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

		public string HandText(bool plural) => type.HandText(plural);

		public string ShortDescription(bool plural) => type.ShortDescription(plural);

		public string LongDescription() => type.LongDescription(AsReadOnlyData());
		public string LongDescription(bool alternateForm) => type.LongDescription(AsReadOnlyData(), alternateForm);

		public string LongPrimaryDescription() => type.LongPrimaryDescription(AsReadOnlyData());
		public string LongAlternateDescription() => type.LongAlternateDescription(AsReadOnlyData());

		public string LongDescription(bool alternateForm, bool plural) => type.LongDescription(AsReadOnlyData(), alternateForm, plural);

		public string LongPrimaryDescription(bool plural) => type.LongPrimaryDescription(AsReadOnlyData(), plural);
		public string LongAlternateDescription(bool plural) => type.LongAlternateDescription(AsReadOnlyData(), plural);

		public string FullDescription() => type.FullDescription(AsReadOnlyData());
		public string FullDescription(bool alternateForm) => type.FullDescription(AsReadOnlyData(), alternateForm);

		public string FullPrimaryDescription() => type.FullPrimaryDescription(AsReadOnlyData());
		public string FullAlternateDescription() => type.FullAlternateDescription(AsReadOnlyData());

		public string FullDescription(bool alternateForm, bool plural) => type.FullDescription(AsReadOnlyData(), alternateForm, plural);

		public string FullPrimaryDescription(bool plural) => type.FullPrimaryDescription(AsReadOnlyData(), plural);
		public string FullAlternateDescription(bool plural) => type.FullAlternateDescription(AsReadOnlyData(), plural);

		public string LongOrFullDescription(bool alternateForm, bool plural, bool isFull)
		{
			if (isFull) return FullDescription(alternateForm, plural);
			else return LongDescription(alternateForm, plural);
		}
	}

	public partial class HandType : BehaviorBase
	{
		private static int indexMaker = 0;

		protected enum HandStyle { HANDS, CLAWS, PAWS, OTHER }

		private readonly LongPluralDescriptor<HandData> longDescription;
		private readonly LongPluralDescriptor<HandData> fullDescription;
		private readonly SimplePluralDescriptor shortPluralDesc;
		private readonly SimplePluralDescriptor handStr;

		//Text for when you want to say 'hand' but hand my not be accurate. it'll return things like 'clawed hand' or 'talon' or 'gooey "hand"'
		//that still make it clear you're talking about the 'hand', but still respecting the actual hand type.
		public string HandText(bool plural) => handStr(plural);

		public string ShortDescription(bool plural) => shortPluralDesc(plural);

		public string LongDescription(HandData handData) => longDescription(handData);
		public string LongDescription(HandData handData, bool alternateForm) => longDescription(handData, alternateForm);

		public string LongPrimaryDescription(HandData handData) => longDescription(handData, false);
		public string LongAlternateDescription(HandData handData) => longDescription(handData, true);

		public string LongDescription(HandData handData, bool alternateForm, bool plural) => longDescription(handData, alternateForm, plural);

		public string LongPrimaryDescription(HandData handData, bool plural) => longDescription(handData, false, plural);
		public string LongAlternateDescription(HandData handData, bool plural) => longDescription(handData, true, plural);

		public string FullDescription(HandData handData) => fullDescription(handData);
		public string FullDescription(HandData handData, bool alternateForm) => fullDescription(handData, alternateForm);

		public string FullPrimaryDescription(HandData handData) => fullDescription(handData, false);
		public string FullAlternateDescription(HandData handData) => fullDescription(handData, true);

		public string FullDescription(HandData handData, bool alternateForm, bool plural) => fullDescription(handData, alternateForm, plural);

		public string FullPrimaryDescription(HandData handData, bool plural) => fullDescription(handData, false, plural);
		public string FullAlternateDescription(HandData handData, bool plural) => fullDescription(handData, true, plural);

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

		private protected HandType(HandStyle style, SimplePluralDescriptor nounText, SimplePluralDescriptor shortDesc, LongPluralDescriptor<HandData> longDesc, LongPluralDescriptor<HandData> fullDesc)
			: base(PluralHelper(shortDesc))
		{
			_index = indexMaker++;
			longDescription = longDesc ?? throw new ArgumentNullException(nameof(longDesc));
			handStr = nounText ?? throw new ArgumentNullException(nameof(nounText));
			shortPluralDesc = shortDesc;
			fullDescription = fullDesc ?? throw new ArgumentNullException(nameof(longDesc));

			handStyle = style;
		}

		public static readonly HandType HUMAN = new HandType(HandStyle.HANDS, HumanNoun, HumanShort, HumanLongDesc, HumanFullDesc);
		public static readonly HandType LIZARD = new LizardClaws();
		public static readonly HandType DRAGON = new HandType(HandStyle.CLAWS, DragonNoun, DragonShort, DragonLongDesc, DragonFullDesc);
		public static readonly HandType SALAMANDER = new HandType(HandStyle.CLAWS, SalamanderNoun, SalamanderShort, SalamanderLongDesc, SalamanderFullDesc);
		public static readonly HandType CAT = new HandType(HandStyle.PAWS, CatNoun, CatShort, CatLongDesc, CatFullDesc);
		public static readonly HandType DOG = new HandType(HandStyle.PAWS, DogNoun, DogShort, DogLongDesc, DogFullDesc);
		public static readonly HandType FOX = new HandType(HandStyle.PAWS, FoxNoun, FoxShort, FoxLongDesc, FoxFullDesc);
		public static readonly HandType IMP = new ImpClaws();
		public static readonly HandType COCKATRICE = new HandType(HandStyle.CLAWS, CockatriceNoun, CockatriceShort, CockatriceLongDesc, CockatriceFullDesc);
		public static readonly HandType RED_PANDA = new HandType(HandStyle.PAWS, RedPandaNoun, RedPandaShort, RedPandaLongDesc, RedPandaFullDesc);
		public static readonly HandType FERRET = new HandType(HandStyle.PAWS, FerretNoun, FerretShort, FerretLongDesc, FerretFullDesc);
		public static readonly HandType GOO = new HandType(HandStyle.OTHER, GooNoun, GooShort, GooLongDesc, GooFullDesc);
		//public static readonly Hands MANTIS = new Hands(HandStyle.OTHER, MantisNoun, MantisShort MantisLongDesc); //Not even remotely implemented.

		private class LizardClaws : HandType
		{

			public LizardClaws() : base(HandStyle.CLAWS, LizardNoun, LizardShort, LizardLongDesc, LizardFullDesc) { }

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
			public ImpClaws() : base(HandStyle.CLAWS, ImpNoun, ImpShort, ImpLongDesc, ImpFullDesc) { }

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

		public string ShortDescription(bool plural) => type.ShortDescription(plural);

		public string LongDescription() => type.LongDescription(this);
		public string LongDescription(bool alternateForm) => type.LongDescription(this, alternateForm);

		public string LongPrimaryDescription() => type.LongPrimaryDescription(this);
		public string LongAlternateDescription() => type.LongAlternateDescription(this);

		public string LongDescription(bool alternateForm, bool plural) => type.LongDescription(this, alternateForm, plural);

		public string LongPrimaryDescription(bool plural) => type.LongPrimaryDescription(this, plural);
		public string LongAlternateDescription(bool plural) => type.LongAlternateDescription(this, plural);

		public string FullDescription() => type.FullDescription(this);
		public string FullDescription(bool alternateForm) => type.FullDescription(this, alternateForm);

		public string FullPrimaryDescription() => type.FullPrimaryDescription(this);
		public string FullAlternateDescription() => type.FullAlternateDescription(this);

		public string FullDescription(bool alternateForm, bool plural) => type.FullDescription(this, alternateForm, plural);

		public string FullPrimaryDescription(bool plural) => type.FullPrimaryDescription(this, plural);
		public string FullAlternateDescription(bool plural) => type.FullAlternateDescription(this, plural);

		public string LongOrFullDescription(bool alternateForm, bool plural, bool isFull)
		{
			if (isFull) return FullDescription(alternateForm, plural);
			else return LongDescription(alternateForm, plural);
		}

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
