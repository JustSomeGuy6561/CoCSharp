//Back.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 1:58 AM
using CoC.Backend.Attacks;
using CoC.Backend.Attacks.BodyPartAttacks;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Races;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
namespace CoC.Backend.BodyParts
{
	//Moved abdomens for bee, spider here. I dunno, it seemed like that made more sense, though i'm not exactly familiar with the anatomy of anthropomorphized spiders.
	//tail now has the ovipositors. ovipositors are no longer perks. in the event bees and spiders need some back type (remember, wings and back are separate), i could just
	//add an ovipositor type. for now i don't think i need to. the scorpion is still a tail, but i may move it here for ease of coding, though technically it's a tail. idk man.

	public sealed class Back : BehavioralSaveablePart<Back, BackType>, IDyeable, ICanAttackWith, IBodyPartTimeLazy
	{
		//public HairFurColors hairFur { get; private set; } = HairFurColors.NO_HAIR_FUR; //set automatically via type property. can be manually set via dyeing.
		public EpidermalData backEpidermis => epidermis.GetEpidermalData();

		private Epidermis epidermis = new Epidermis();
		private AttackBase _attack = AttackBase.NO_ATTACK;
		public ushort resources { get; private set; } = 0;
		public ushort regenRate { get; private set; } = 0;

		public ushort maxCharges => _attack is ResourceAttackBase ? ((ResourceAttackBase)_attack).maxResource : (ushort)0;
		public ushort maxRegen => _attack is ResourceAttackBase ? ((ResourceAttackBase)_attack).maxRechargeRate : (ushort)0;

		public override BackType type
		{
			get => _type;
			protected set
			{
				if (value != _type)
				{
					_attack = value.GetAttackOnTransform(() => resources, (x) => resources = x);
					if (_attack is ResourceAttackBase resourceAttack)
					{
						resources = resourceAttack.initialResource;
						regenRate = resourceAttack.initialRechargeRate;
					}
					else
					{
						resources = 0;
						regenRate = 0;
					}
				}
				_type = value;
			}

		}
		private BackType _type = BackType.NORMAL;

		public override bool isDefault => type == BackType.NORMAL;

		private Back(BackType backType)
		{
			_type = backType ?? throw new ArgumentNullException();
			_type.ParseEpidermis(epidermis);
		}

		internal static Back GenerateDefault()
		{
			return new Back(BackType.NORMAL);
		}

		internal static Back GenerateDefaultOfType(BackType backType)
		{
			return new Back(backType);
		}
		internal static Back GenerateDraconicMane(DragonBackMane dragonMane, HairFurColors maneColor)
		{
			Back newBack = new Back(dragonMane); //throws for null
			if (!HairFurColors.IsNullOrEmpty(maneColor))
			{
				newBack.epidermis.ChangeFur(maneColor);
			}
			return newBack;
		}

		internal bool UpdateBack(BackType newType)
		{
			if (newType == null || type == newType)
			{
				return false;
			}
			type = newType; //automatically sets hair to default.
			return true;
		}

		internal bool UpdateBack(DragonBackMane dragonMane, HairFurColors maneColor)
		{
			if (dragonMane == null || type == dragonMane)
			{
				return false;
			}
			type = dragonMane; //sets epidermis to use hair.
							   //overrides it if possible.
			if (!HairFurColors.IsNullOrEmpty(maneColor)) //can be null.
			{
				epidermis.ChangeFur(maneColor);
			}
			return true;
		}
		internal override bool Restore()
		{
			if (type != BackType.NORMAL)
			{
				type = BackType.NORMAL;
				return true;
			}
			return false;
		}

		internal override bool Validate(bool correctInvalidData)
		{
			BackType backType = type;
			bool retVal = BackType.Validate(ref backType, epidermis, correctInvalidData);
			type = backType;
			return retVal;
		}

		#region IDyeable
		bool IDyeable.allowsDye()
		{
			return epidermis.furMutable;
		}

		bool IDyeable.isDifferentColor(HairFurColors dyeColor) //
		{
			if (dyeColor == null) throw new ArgumentNullException();
			return !epidermis.fur.IsIdenticalTo(dyeColor);
		}

		bool IDyeable.attemptToDye(HairFurColors dye)
		{
			if (!canDye || !dyeable.isDifferentColor(dye))
			{
				return false;
			}
			else
			{
				return epidermis.ChangeFur(new FurColor(dye));
			}
		}

		string IDyeable.buttonText()
		{
			return type.dyeDesc();
		}

		string IDyeable.locationDesc()
		{
			return type.dyeText();
		}

		private bool canDye => dyeable.allowsDye();
		private IDyeable dyeable => this;
		#endregion

		#region ICanAttackWith

		AttackBase ICanAttackWith.attack => _attack;

		bool ICanAttackWith.canAttackWith() => _attack != AttackBase.NO_ATTACK;

		#endregion

		#region ITimeListener
		string IBodyPartTimeLazy.reactToTimePassing(bool isPlayer, byte hoursPassed)
		{
			if (_attack is ResourceAttackBase resourceAttack && resources < maxCharges) //slight optimization. make sure we aren't at max.
			{
				uint regen = regenRate.mult(hoursPassed);
				ushort newCount = (ushort)Math.Min(regen + resources, maxCharges);
				resources = newCount;
			}
			//no output.
			return "";
		}
		#endregion
	}

	public partial class BackType : SaveableBehavior<BackType, Back>
	{
		private static int indexMaker = 0;
		private static readonly List<BackType> backs = new List<BackType>();
		private readonly int _index;
		public override int index => _index;

		public virtual bool canDye => false;/* defaultHair => HairFurColors.NO_HAIR_FUR;*/

		internal virtual AttackBase GetAttackOnTransform(Func<ushort> get, Action<ushort> set)
		{
			return AttackBase.NO_ATTACK;
		}


		public virtual void ParseEpidermis(Epidermis epidermis)
		{
			epidermis.Reset();
		}

		internal virtual SimpleDescriptor dyeDesc => GenericBtnkDesc;
		internal virtual SimpleDescriptor dyeText => GenericLocDesc;
		//public bool usesHair => !defaultHair.isEmpty;

		protected BackType(SimpleDescriptor shortDesc, DescriptorWithArg<Back> fullDesc, TypeAndPlayerDelegate<Back> playerDesc,
			ChangeType<Back> transform, RestoreType<Back> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			backs.AddAt(this, _index);
		}

		internal static BackType Deserialize(int index)
		{
			if (index < 0 || index >= backs.Count)
			{
				throw new System.ArgumentException("index for back type deserialize out of range");
			}
			else
			{
				BackType back = backs[index];
				if (back != null)
				{
					return back;
				}
				else
				{
					throw new System.ArgumentException("index for arm type points to an object that does not exist. this may be due to obsolete code");
				}
			}
		}

		internal static bool Validate(ref BackType backType, Epidermis epidermis, bool correctInvalidData)
		{
			if (backs.Contains(backType))
			{
				return true;
			}
			else if (correctInvalidData)
			{
				backType = NORMAL;
			}
			return false;
		}

		//i'd love to combine these, but they're actually entirely dependant on if the player used hair serum on Ember. 
		//it's such a specific check i can't realistically do it in here. 
		public static readonly BackType NORMAL = new BackType(NormalDesc, NormalFullDesc, NormalPlayerStr, NormalTransformStr, NormalRestoreStr);
		public static readonly DragonBackMane DRACONIC_MANE = new DragonBackMane();
		public static readonly BackType DRACONIC_SPIKES = new BackType(DraconicSpikesDesc, DraconicSpikesFullDesc, DraconicSpikesPlayerStr, DraconicSpikesTransformStr, DraconicSpikesRestoreStr);
		public static readonly BackType SHARK_FIN = new BackType(SharkFinDesc, SharkFinFullDesc, SharkFinPlayerStr, SharkFinTransformStr, SharkFinRestoreStr);
		public static readonly AttackableBackType SPIDER_ABDOMEN = new AttackableBackType(SPIDER_ATTACK, CARAPACE, SpiderShortDesc, SpiderFullDesc, SpiderPlayerStr, SpiderTransformStr, SpiderRestoreStr); //web
		public static readonly AttackableBackType BEE_ABDOMEN = new AttackableBackType(BEE_STING, CARAPACE, BeeShortDesc, BeeFullDesc, BeePlayerStr, BeeTransformStr, BeeRestoreStr); //sting
		public static readonly AttackableBackType TENDRILS = new AttackableBackType(TENDRIL_GRAB, TENDRIL_EPIDERMIS, TendrilShortDesc, TenderilFullDesc, TendrilPlayerStr, TendrilTransformStr, TendrilRestoreStr); //tendril grab

		private static readonly Func<Func<ushort>, Action<ushort>, ResourceAttackBase> SPIDER_ATTACK = (x, y) => new SpiderWeb(x, y);
		private static readonly Func<Func<ushort>, Action<ushort>, ResourceAttackBase> BEE_STING = (x, y) => new BeeSting(x, y);
		private static readonly Func<Func<ushort>, Action<ushort>, ResourceAttackBase> TENDRIL_GRAB = (x, y) => new TentaGrab(x, y);

		private static readonly Epidermis CARAPACE = new Epidermis(EpidermisType.CARAPACE, Tones.BLACK, SkinTexture.SHINY);
		private static readonly Epidermis TENDRIL_EPIDERMIS = new Epidermis(EpidermisType.GOO, Tones.CERULEAN, SkinTexture.SLIMY);
	}
	public sealed class DragonBackMane : BackType
	{
		public HairFurColors defaultHair => Species.DRAGON.defaultManeColor;
		internal override SimpleDescriptor dyeDesc => ManeDesc;
		internal override SimpleDescriptor dyeText => YourManeDesc;

		internal DragonBackMane() : base(DraconicManeDesc, DraconicManeFullDesc, DraconicManePlayerStr, DraconicManeTransformStr, DraconicManeRestoreStr)
		{ }
	}

	public sealed class AttackableBackType : BackType
	{
		private readonly Epidermis baseAppearance;
		//callback madness! Lets us not make this virtual. basically, since we can't create the attack without knowing where we get the resources, we can't create it here.
		//BUT, given a callback to the resources, we can generate the attack here, using another callback. Clarity dictates i not do this, but fuck it.
		private readonly Func<Func<ushort>, Action<ushort>, ResourceAttackBase> getAttack; //a callback. takes another callback (that returns a ushort), and returns an attack that requires resources.
		internal AttackableBackType(Func<Func<ushort>, Action<ushort>, ResourceAttackBase> attackGetter, Epidermis appearance,
			SimpleDescriptor shortDesc, DescriptorWithArg<Back> fullDesc, TypeAndPlayerDelegate<Back> playerDesc, ChangeType<Back> transform, RestoreType<Back> restore)
			: base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			getAttack = attackGetter;
			baseAppearance = appearance;
		}

		public override void ParseEpidermis(Epidermis epidermis)
		{
			epidermis.copyFrom(baseAppearance); //copy the base appearance to the epidermis.
		}

		internal override AttackBase GetAttackOnTransform(Func<ushort> get, Action<ushort> set)
		{
			return getAttack(get, set);
		}
	}
}