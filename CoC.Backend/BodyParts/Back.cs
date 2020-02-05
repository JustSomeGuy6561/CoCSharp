//Back.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 1:58 AM
using CoC.Backend.Attacks;
using CoC.Backend.Attacks.BodyPartAttacks;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CoC.Backend.BodyParts
{
	public sealed partial class BackTattooLocation : TattooLocation
	{

		private static readonly List<BackTattooLocation> _allLocations = new List<BackTattooLocation>();

		public static readonly ReadOnlyCollection<BackTattooLocation> allLocations;

		private readonly byte index;

		static BackTattooLocation()
		{
			allLocations = new ReadOnlyCollection<BackTattooLocation>(_allLocations);
		}

		private BackTattooLocation(byte index, TattooSizeLimit limitSize, SimpleDescriptor btnText, SimpleDescriptor locationDesc) : base(limitSize, btnText, locationDesc)
		{
			this.index = index;
		}

		//tramp stamp = LeftLower + RightLower
		//upper back = LeftShoulderblade + Rightshoulderblade

		public static BackTattooLocation LEFT_SHOULDERBLADE = new BackTattooLocation(0, SmallTattoosOnly, LeftShoulderbladeButton, LeftShoulderbladeLocation);
		public static BackTattooLocation RIGHT_SHOULDERBLADE = new BackTattooLocation(1, SmallTattoosOnly, RightShoulderbladeButton, RightShoulderbladeLocation);
		public static BackTattooLocation UPPER_BACK = new BackTattooLocation(2, MediumTattoosOrSmaller, UpperBackButton, UpperBackLocation);
		public static BackTattooLocation LEFT_LOWER_BACK = new BackTattooLocation(3, SmallTattoosOnly, LeftLowerBackButton, LeftLowerBackLocation);
		public static BackTattooLocation RIGHT_LOWER_BACK = new BackTattooLocation(4, SmallTattoosOnly, RightLowerBackButton, RightLowerBackLocation);
		public static BackTattooLocation TRAMP_STAMP = new BackTattooLocation(5, MediumTattoosOrSmaller, LowerBackButton, LowerBackLocation);
		public static BackTattooLocation LARGE_BACK = new BackTattooLocation(6, LargeTattoosOrSmaller, LargeBackButton, LargeBackLocation);
		public static BackTattooLocation FULL_BACK = new BackTattooLocation(7, FullPartTattoo, FullBackButton, FullBackLocation);

		public static bool LocationsCompatible(BackTattooLocation first, BackTattooLocation second)
		{
			//upper back is not compatible with the shoulderblades (it basically is a combo of them)
			//lower back (tramp stamp or slag tag) is not compatible with the left/right lower back (it basically combines them)
			//the remainder of these are compatible.run these checks accordingly.

			//if one is left forearm.
			if (first == UPPER_BACK || second == UPPER_BACK)
			{
				//check to see if other is left inner or left outer forearm.
				var other = (first == UPPER_BACK) ? second : first;
				return other != LEFT_SHOULDERBLADE && other != RIGHT_SHOULDERBLADE;
			}
			//ditto for right forearm.
			else if (first == TRAMP_STAMP || second == TRAMP_STAMP)
			{
				var other = (first == TRAMP_STAMP) ? second : first;
				return other != LEFT_LOWER_BACK && other != RIGHT_LOWER_BACK;
			}
			//otherwise, we're good.s
			else
			{
				return true;
			}
		}
	}

	public sealed class BackTattoo : TattooablePart<BackTattooLocation>
	{
		public BackTattoo(IBodyPart source, GenericCreatureText allTattoosShort, GenericCreatureText allTattoosLong) : base(source, allTattoosShort, allTattoosLong) {}

		public override int MaxTattoos => BackTattooLocation.allLocations.Count;

		public override IEnumerable<BackTattooLocation> availableLocations => BackTattooLocation.allLocations;

		public override bool LocationsCompatible(BackTattooLocation first, BackTattooLocation second) => BackTattooLocation.LocationsCompatible(first, second);
	}

	//only way data changes is due to dye. i figure that's rare enough not to deal with it.
	public sealed partial class Back : BehavioralSaveablePart<Back, BackType, BackData>, IDyeable, ICanAttackWith, IBodyPartTimeLazy
	{
		public override string BodyPartName() => Name();

		public EpidermalData epidermalData => epidermis.AsReadOnlyData();

		private Epidermis epidermis = new Epidermis();

		private AttackBase _attack = AttackBase.NO_ATTACK;
		//since attacks are supposed to be static, resources must be stored here and passed in. we do this by wrapping the get and set for these in a function.
		public ushort resources { get; private set; } = 0;
		public ushort regenRate { get; private set; } = 0;

		public ushort maxCharges => _attack is ResourceAttackBase ? ((ResourceAttackBase)_attack).maxResource : (ushort)0;
		public ushort maxRegen => _attack is ResourceAttackBase ? ((ResourceAttackBase)_attack).maxRechargeRate : (ushort)0;
		public ushort minRegen => _attack is ResourceAttackBase ? ((ResourceAttackBase)_attack).minRechargeRate : (ushort)0;

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

		public readonly BackTattoo tattoos;

		internal Back(Guid creatureID, BackType backType) : base(creatureID)
		{
			type = backType ?? throw new ArgumentNullException();

			tattoos = new BackTattoo(this, AllTattoosShort, AllTattoosLong);
		}

		internal Back(Guid creatureID) : this(creatureID, BackType.defaultValue) { }

		internal Back(Guid creatureID, DragonBackMane dragonMane, HairFurColors maneColor) : this(creatureID, dragonMane)
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
			if (!oldData.epidermis.Equals(epidermalData))
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

		public bool IsDraconic() => type.IsDraconic();

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

		string IDyeable.locationDesc(out bool isPlural)
		{
			return type.dyeText(out isPlural);
		}

		string IDyeable.postDyeDescription()
		{
			return type.postText(this.epidermis.fur.primaryColor);
		}

		private bool canDye => dyeable.allowsDye();
		private IDyeable dyeable => this;
		#endregion

		#region ICanAttackWith
		public void UpdateResources(short resourceDelta = 0, short regenRateDelta = 0)
		{
			if (!(_attack is ResourceAttackBase) || (resourceDelta == 0 && regenRateDelta == 0))
			{
				return;
			}

			if (regenRateDelta != 0)
			{
				regenRate = (ushort)Utils.Clamp2(regenRateDelta + regenRate, minRegen, maxRegen);
			}
			if (resourceDelta != 0)
			{
				resources = (ushort)Utils.Clamp2(resourceDelta + resources, 0, maxCharges);
			}
		}

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
		public override int id => _index;

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


		internal virtual string dyeDesc() => GenericBtnkDesc();
		internal virtual string dyeText(out bool isPlural) => GenericLocDesc(out isPlural);
		internal virtual string postText(HairFurColors hairColor) => GenericPostUseDesc(hairColor);
		public virtual bool hasSpecialEpidermis => false; //replaces usesHair, as we now have types that can use tones. we've fixed this with a single epidermis here.

		protected BackType(ShortDescriptor shortDesc, PartDescriptor<BackData> longDesc, PlayerBodyPartDelegate<Back> playerDesc,
			ChangeType<BackData> transform, RestoreType<BackData> restore) : base(shortDesc, longDesc, playerDesc, transform, restore)
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
		public static readonly AttackableBackType TENDRILS; //tendril grab
		public static readonly BehemothBack BEHEMOTH;

		//these need to be functions or order of initialization can break. These are the things you learn
		//when running unit tests. Testing OP!

		private static ResourceAttackBase TENDRIL_GRAB(Func<ushort> x, Action<ushort> y) => new TentaGrab(x, y);
		private static readonly EpidermalData TENDRIL_EPIDERMIS = new EpidermalData(EpidermisType.GOO, Tones.CERULEAN, SkinTexture.SLIMY);

		static BackType()
		{
			NORMAL = new BackType(NormalDesc, NormalLongDesc, NormalPlayerStr, NormalTransformStr, NormalRestoreStr);
			DRACONIC_MANE = new DragonBackMane();
			DRACONIC_SPIKES = new BackType(DraconicSpikesDesc, DraconicSpikesLongDesc, DraconicSpikesPlayerStr, DraconicSpikesTransformStr, DraconicSpikesRestoreStr);
			SHARK_FIN = new BackType(SharkFinDesc, SharkFinLongDesc, SharkFinPlayerStr, SharkFinTransformStr, SharkFinRestoreStr);
			TENDRILS = new AttackableBackType(TENDRIL_GRAB, TENDRIL_EPIDERMIS, TendrilShortDesc, TendrilLongDesc, TendrilPlayerStr, TendrilTransformStr, TendrilRestoreStr); //tendril grab
			BEHEMOTH = new BehemothBack();

		}


		public bool IsDraconic()
		{
			return this == DRACONIC_MANE || this == DRACONIC_SPIKES;
		}

	}
	public sealed class DragonBackMane : BackType
	{
		public HairFurColors defaultHair => DefaultValueHelpers.defaultDragonManeColor;
		internal override string dyeDesc() => ManeDesc();
		internal override string dyeText(out bool isPlural) => YourManeDesc(out isPlural);

		internal override string postText(HairFurColors hairColor) => ManePostDyeText(hairColor);

		public override bool hasSpecialEpidermis => true;

		internal DragonBackMane() : base(DraconicManeDesc, DraconicManeLongDesc, DraconicManePlayerStr, DraconicManeTransformStr, DraconicManeRestoreStr)
		{ }

		internal override void ParseEpidermis(Epidermis epidermis)
		{
			if (!epidermis.usesFurColor)
			{
				epidermis.UpdateOrChange(EpidermisType.FUR, new FurColor(defaultHair));
			}
		}
	}

	public sealed class BehemothBack : BackType
	{
		public HairFurColors defaultHair => DefaultValueHelpers.defaultDragonManeColor;
		internal override string dyeDesc() => ManeDesc();
		internal override string dyeText(out bool isPlural) => YourManeDesc(out isPlural);
		internal override string postText(HairFurColors hairColor) => ManePostDyeText(hairColor);

		public override bool hasSpecialEpidermis => true;

		internal BehemothBack() : base(BehemothDesc, BehemothLongDesc, BehemothPlayerStr, BehemothTransformStr, BehemothRestoreStr)
		{ }

		internal override void ParseEpidermis(Epidermis epidermis)
		{
			if (!epidermis.usesFurColor)
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
		private readonly GenerateResourceAttack getAttack; //a callback. takes another callback (that returns a ushort), and returns an attack that requires resources.
		internal AttackableBackType(GenerateResourceAttack attackGetter, EpidermalData appearance,
			ShortDescriptor shortDesc, PartDescriptor<BackData> longDesc, PlayerBodyPartDelegate<Back> playerDesc, ChangeType<BackData> transform, RestoreType<BackData> restore)
			: base(shortDesc, longDesc, playerDesc, transform, restore)
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
			else if (baseAppearance.usesFurColor)
			{
				epidermis.UpdateOrChange((FurBasedEpidermisType)baseAppearance.type, baseAppearance.fur, baseAppearance.furTexture);
			}
			else
			{
				epidermis.UpdateOrChange((ToneBasedEpidermisType)baseAppearance.type, baseAppearance.tone, baseAppearance.skinTexture);
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

		public override BackData AsCurrentData()
		{
			return this;
		}

		internal BackData(Back back) : base(GetID(back), GetBehavior(back))
		{
			epidermis = back.epidermalData;
		}
	}
}
