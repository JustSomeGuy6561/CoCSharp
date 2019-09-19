//Back.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 1:58 AM
using CoC.Backend.Attacks;
using CoC.Backend.Attacks.BodyPartAttacks;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Races;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CoC.Backend.BodyParts
{
	//Moved abdomens for bee, spider here. I dunno, it seemed like that made more sense, though i'm not exactly familiar with the anatomy of anthropomorphized spiders.
	//tail now has the ovipositors. ovipositors are no longer perks. in the event bees and spiders need some back type (remember, wings and back are separate), i could just
	//add an ovipositor type. for now i don't think i need to. the scorpion is still a tail, but i may move it here for ease of coding, though technically it's a tail. idk man.

	//only way data changes is due to dye. i figure that's rare enough not to deal with it.

	public sealed class Back : BehavioralSaveablePart<Back, BackType, BackData>, IDyeable, ICanAttackWith, IBodyPartTimeLazy
	{
		//public HairFurColors hairFur { get; private set; } = HairFurColors.NO_HAIR_FUR; //set automatically via type property. can be manually set via dyeing.
		public EpidermalData backEpidermis => epidermis.AsReadOnlyData();

		private Epidermis epidermis = new Epidermis();

		private AttackBase _attack = AttackBase.NO_ATTACK;
		//since attacks are supposed to be static, resources must be stored here and passed in. we do this by wrapping the get and set for these in a function.
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
					value.ParseEpidermis(epidermis);
				}
				_type = value;
			}

		}
		private BackType _type = BackType.NORMAL;

		public override BackType defaultType => BackType.defaultValue;

		internal Back(Creature source, BackType backType) : base(source)
		{
			_type = backType ?? throw new ArgumentNullException();
			_type.ParseEpidermis(epidermis);
		}

		internal Back(Creature source) : this(source, BackType.defaultValue) { }

		internal Back(Creature source, DragonBackMane dragonMane, HairFurColors maneColor) : this(source, dragonMane)
		{
			if (!HairFurColors.IsNullOrEmpty(maneColor))
			{
				epidermis.ChangeFur(maneColor);
			}
		}

		public override BackData AsReadOnlyData()
		{
			return new BackData(this);
		}

		private void CheckDataChanged(BackData oldData)
		{
			if (!oldData.epidermis.Equals(backEpidermis))
			{
				NotifyDataChanged(oldData);
			}
		}

		internal override bool UpdateType(BackType newType)
		{
			if (newType is null || type == newType)
			{
				return false;
			}
			var oldType = type;
			var oldData = AsReadOnlyData();
			type = newType;

			CheckDataChanged(oldData);
			NotifyTypeChanged(oldType);
			return true;
		}

		//additional, non-standard update.
		internal bool UpdateType(DragonBackMane dragonMane, HairFurColors maneColor)
		{
			if (dragonMane == null || type == dragonMane)
			{
				return false;
			}

			var oldType = type;
			var oldData = AsReadOnlyData();
			type = dragonMane; //sets epidermis to use hair.

			//overrides it if possible.
			if (!HairFurColors.IsNullOrEmpty(maneColor)) //can be null.
			{
				epidermis.ChangeFur(maneColor);
			}

			CheckDataChanged(oldData);
			NotifyTypeChanged(oldType);
			return true;
		}

		//default restore is fine.

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

	public partial class BackType : SaveableBehavior<BackType, Back, BackData>
	{
		private static int indexMaker = 0;
		private static readonly List<BackType> backs = new List<BackType>();
		public static readonly ReadOnlyCollection<BackType> availableTypes = new ReadOnlyCollection<BackType>(backs);
		private readonly int _index;
		public override int index => _index;

		public static BackType defaultValue => NORMAL;


		public virtual bool canDye => false;/* defaultHair => HairFurColors.NO_HAIR_FUR;*/

		internal virtual AttackBase GetAttackOnTransform(Func<ushort> get, Action<ushort> set)
		{
			return AttackBase.NO_ATTACK;
		}


		internal virtual void ParseEpidermis(Epidermis epidermis)
		{
			epidermis.Reset();
		}


		internal virtual SimpleDescriptor dyeDesc => GenericBtnkDesc;
		internal virtual SimpleDescriptor dyeText => GenericLocDesc;
		public virtual bool hasSpecialEpidermis => false; //replaces usesHair, as we now have types that can use tones. we've fixed this with a single epidermis here.

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

		//in some cases order of creation matters. in these cases you will need a static constructor.


		public static readonly BackType NORMAL;
		public static readonly DragonBackMane DRACONIC_MANE;
		public static readonly BackType DRACONIC_SPIKES;
		public static readonly BackType SHARK_FIN;
		public static readonly AttackableBackType SPIDER_ABDOMEN; //web
		public static readonly AttackableBackType BEE_STINGER; //sting
		public static readonly AttackableBackType TENDRILS; //tendril grab

		//these need to be functions or order of initialization can break. These are the things you learn
		//when running unit tests. Testing OP!
		private static ResourceAttackBase SPIDER_ATTACK(Func<ushort> x, Action<ushort> y) => new SpiderWeb(x, y);
		private static ResourceAttackBase BEE_STING(Func<ushort> x, Action<ushort> y) => new BeeSting(x, y);
		private static ResourceAttackBase TENDRIL_GRAB(Func<ushort> x, Action<ushort> y) => new TentaGrab(x, y);

		private static readonly EpidermalData CARAPACE = new EpidermalData(EpidermisType.CARAPACE, Tones.BLACK, SkinTexture.SHINY);
		private static readonly EpidermalData TENDRIL_EPIDERMIS = new EpidermalData(EpidermisType.GOO, Tones.CERULEAN, SkinTexture.SLIMY);

		static BackType()
		{
			NORMAL = new BackType(NormalDesc, NormalFullDesc, NormalPlayerStr, NormalTransformStr, NormalRestoreStr);
			DRACONIC_MANE = new DragonBackMane();
			DRACONIC_SPIKES = new BackType(DraconicSpikesDesc, DraconicSpikesFullDesc, DraconicSpikesPlayerStr, DraconicSpikesTransformStr, DraconicSpikesRestoreStr);
			SHARK_FIN = new BackType(SharkFinDesc, SharkFinFullDesc, SharkFinPlayerStr, SharkFinTransformStr, SharkFinRestoreStr);
			SPIDER_ABDOMEN = new AttackableBackType(SPIDER_ATTACK, CARAPACE, SpiderShortDesc, SpiderFullDesc, SpiderPlayerStr, SpiderTransformStr, SpiderRestoreStr); //web
			BEE_STINGER = new AttackableBackType(BEE_STING, CARAPACE, BeeShortDesc, BeeFullDesc, BeePlayerStr, BeeTransformStr, BeeRestoreStr); //sting
			TENDRILS = new AttackableBackType(TENDRIL_GRAB, TENDRIL_EPIDERMIS, TendrilShortDesc, TenderilFullDesc, TendrilPlayerStr, TendrilTransformStr, TendrilRestoreStr); //tendril grab
		}
	}
	public sealed class DragonBackMane : BackType
	{
		public HairFurColors defaultHair => Species.DRAGON.defaultManeColor;
		internal override SimpleDescriptor dyeDesc => ManeDesc;
		internal override SimpleDescriptor dyeText => YourManeDesc;

		public override bool hasSpecialEpidermis => true;

		internal DragonBackMane() : base(DraconicManeDesc, DraconicManeFullDesc, DraconicManePlayerStr, DraconicManeTransformStr, DraconicManeRestoreStr)
		{ }

		internal override void ParseEpidermis(Epidermis epidermis)
		{
			if (!epidermis.usesFur)
			{
				epidermis.UpdateOrChange(EpidermisType.FUR, new FurColor(defaultHair));
			}
		}
	}

	public sealed class AttackableBackType : BackType
	{
		private readonly EpidermalData baseAppearance;
		//callback madness! Lets us not make this virtual. basically, since we can't create the attack without knowing where we get the resources, we can't create it here.
		//BUT, given a callback to the resources, we can generate the attack here, using another callback. Clarity dictates i not do this, but fuck it.
		private readonly Func<Func<ushort>, Action<ushort>, ResourceAttackBase> getAttack; //a callback. takes another callback (that returns a ushort), and returns an attack that requires resources.
		internal AttackableBackType(Func<Func<ushort>, Action<ushort>, ResourceAttackBase> attackGetter, EpidermalData appearance,
			SimpleDescriptor shortDesc, DescriptorWithArg<Back> fullDesc, TypeAndPlayerDelegate<Back> playerDesc, ChangeType<Back> transform, RestoreType<Back> restore)
			: base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			getAttack = attackGetter ?? throw new ArgumentNullException(nameof(attackGetter));
			baseAppearance = appearance ?? throw new ArgumentNullException(nameof(baseAppearance));
		}

		internal override void ParseEpidermis(Epidermis epidermis)
		{
			//not really designed to EpidermalData => Epidermis, but unfortunately, it's the only way to make the data immutable. 
			if (baseAppearance.isEmpty)
			{
				epidermis.Reset();
			}
			else if (baseAppearance.usesFur)
			{
				epidermis.UpdateOrChange((FurBasedEpidermisType)baseAppearance.currentType, baseAppearance.fur, baseAppearance.furTexture);
			}
			else
			{
				epidermis.UpdateOrChange((ToneBasedEpidermisType)baseAppearance.currentType, baseAppearance.tone, baseAppearance.skinTexture);
			}
		}
		public override bool hasSpecialEpidermis => !baseAppearance.isEmpty;

		internal override AttackBase GetAttackOnTransform(Func<ushort> get, Action<ushort> set)
		{
			return getAttack(get, set);
		}
	}

	public sealed class BackData : BehavioralSaveablePartData<BackData, Back, BackType>
	{
		public readonly EpidermalData epidermis;

		internal BackData(Back back) : base(GetBehavior(back))
		{
			epidermis = back.backEpidermis;
		}
	}
}