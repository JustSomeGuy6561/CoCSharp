//Vagina.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 5:57 PM
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts
{
	public enum LabiaPiercings
	{
		LEFT_1, LEFT_2, LEFT_3, LEFT_4, LEFT_5, LEFT_6,
		RIGHT_1, RIGHT_2, RIGHT_3, RIGHT_4, RIGHT_5, RIGHT_6
	}

	//unlike ass wetness changes, this seems fine by me. the normal case is some wetness, though dryer than that is not terribly strange.
	public enum VaginalWetness : byte { DRY, NORMAL, WET, SLICK, DROOLING, SLAVERING }

	//i have, however, renamed these. gaping-wide-> gaping. gaping-> roomy. could even rename clown car to gaping-wide if clown car is a little too bizarre, but i'm kinda fond of its bizarre-ness.
	public enum VaginalLooseness : byte { TIGHT, NORMAL, LOOSE, ROOMY, GAPING, CLOWN_CAR_WIDE }

	public sealed partial class Vagina : BehavioralSaveablePart<Vagina, VaginaType>, IBodyPartTimeLazy, IBaseStatPerkAware
	{
		private const JewelryType SUPPORTED_LABIA_JEWELRY = JewelryType.BARBELL_STUD | JewelryType.RING | JewelryType.SPECIAL;

		public const ushort BASE_CAPACITY = 10; //you now have a base capacity so you can handle insertions, even if you don't have any wetness or whatever.
		public const ushort MAX_VAGINAL_CAPACITY = ushort.MaxValue;

		public readonly Clit clit;

		private PerkStatBonusGetter baseStats;

		public VaginalLooseness minLooseness { get; private set; }
		public VaginalLooseness maxLooseness { get; private set; }

		public VaginalWetness minWetness { get; private set; }
		public VaginalWetness maxWetness { get; private set; }

		public VaginalWetness wetness
		{
			get => _wetness;
			private set => _wetness = Utils.ClampEnum2(value, minWetness, maxWetness);
		}
		private VaginalWetness _wetness;

		public VaginalLooseness looseness
		{
			get => _looseness;
			private set => _looseness = Utils.ClampEnum2(value, minLooseness, maxLooseness);
		}
		private VaginalLooseness _looseness;

		public ushort numTimesVaginal { get; private set; } = 0;

		public bool virgin { get; private set; }

		public ushort bonusVaginalCapacity { get; private set; } = 0;

		private ushort perkBonusVaginalCapacity => baseStats?.Invoke().PerkBasedBonusVaginalCapacity ?? 0;
		public ushort VaginalCapacity()
		{

			byte loose = (byte)looseness;
			if (!virgin)
			{
				loose++;
			}
			byte wet = ((byte)wetness).add(1);
			uint cap = (uint)Math.Floor(BASE_CAPACITY + bonusVaginalCapacity + perkBonusVaginalCapacity /*+ experience / 10*/ + 6 * loose * loose * wet / 10.0);
			if (cap > MAX_VAGINAL_CAPACITY)
			{
				return MAX_VAGINAL_CAPACITY;
			}
			return (ushort)cap;
		}

		private const ushort LOOSENESS_LOOSE_TIMER = 200;
		private const ushort LOOSENESS_ROOMY_TIMER = 100;
		private const ushort LOOSENESS_GAPING_TIMER = 70;
		private const ushort LOOSENESS_CLOWN_CAR_TIMER = 50;
		private ushort vaginaTightenTimer = 0;

		public readonly Piercing<LabiaPiercings> labiaPiercings;

		#region Constructors
		private Vagina()
		{
			clit = Clit.Generate();
			virgin = true;
			type = VaginaType.HUMAN;
			_wetness = VaginalWetness.NORMAL;
			_looseness = VaginalLooseness.TIGHT;
			labiaPiercings = new Piercing<LabiaPiercings>(SUPPORTED_LABIA_JEWELRY, PiercingLocationUnlocked);
		}

		private Vagina(float clitLength)
		{
			clit = Clit.GenerateWithLength(clitLength);
			virgin = true;
			type = VaginaType.HUMAN;
			_wetness = VaginalWetness.NORMAL;
			_looseness = VaginalLooseness.TIGHT;
			labiaPiercings = new Piercing<LabiaPiercings>(SUPPORTED_LABIA_JEWELRY, PiercingLocationUnlocked);
		}
		#endregion
		public override VaginaType type { get; protected set; }
		public override bool isDefault => type == VaginaType.HUMAN;

		#region Generate
		internal static Vagina GenerateFromGender(Gender gender)
		{
			if (gender.HasFlag(Gender.FEMALE)) return new Vagina();
			else return null;
		}

		internal static Vagina GenerateDefault()
		{
			return new Vagina();
		}

		internal static Vagina GenerateDefaultOfType(VaginaType vaginaType)
		{
			return new Vagina()
			{
				type = vaginaType
			};
		}

		internal static Vagina Generate(VaginaType vaginaType, float clitLength, VaginalLooseness vaginalLooseness = VaginalLooseness.TIGHT, VaginalWetness vaginalWetness = VaginalWetness.NORMAL, bool? virgin = null)
		{
			if (virgin == null)
			{
				virgin = vaginalLooseness == VaginalLooseness.TIGHT;
			}

			return new Vagina(clitLength)
			{
				virgin = (bool)virgin,
				_looseness = vaginalLooseness,
				_wetness = vaginalWetness,
				type = vaginaType
			};
		}

		internal static Vagina GenerateOmnibus(VaginaType vaginaType, float clitLength = 5.0f, VaginalLooseness vaginalLooseness = VaginalLooseness.TIGHT,
			VaginalWetness vaginalWetness = VaginalWetness.NORMAL, bool virgin = false, bool clitCockVirgin = true)
		{
			Vagina retVal = new Vagina(clitLength)
			{
				type = vaginaType,
			};
			retVal.ActivateOmnibusClit();
			return retVal;
		}

		#endregion
		#region Update

		internal bool UpdateType(VaginaType newType)
		{
			if (type == newType)
			{
				return false;
			}
			type = newType;
			return true;
		}
		#endregion
		#region Restore
		internal override bool Restore()
		{
			clit.Restore();
			if (type == VaginaType.HUMAN)
			{
				return false;
			}
			type = VaginaType.HUMAN;
			return true;
		}

		#endregion
		#region Validate
		internal override bool Validate(bool correctInvalidData)
		{
			VaginaType vaginaType = type;
			bool valid = VaginaType.Validate(ref vaginaType, correctInvalidData);
			if (valid || correctInvalidData)
			{
				valid &= labiaPiercings.Validate(correctInvalidData); // = x & so we're fine.
			}
			if (valid || correctInvalidData)
			{
				valid &= clit.Validate(correctInvalidData);
			}
			return valid;
		}
		#endregion
		#region Vagina-Specific
		internal bool Deflower()
		{
			if (!virgin)
			{
				return false;
			}
			virgin = false;
			return true;
		}

		internal byte StretchVagina(byte amount = 1)
		{

			VaginalLooseness oldLooseness = looseness;
			looseness = looseness.ByteEnumAdd(amount);
			return looseness - oldLooseness;
		}

		internal byte ShrinkVagina(byte amount = 1)
		{

			VaginalLooseness oldLooseness = looseness;
			looseness = looseness.ByteEnumSubtract(amount);
			return oldLooseness - looseness;
		}

		internal bool SetVaginalLooseness(VaginalLooseness Looseness)
		{
			if (Looseness >= minLooseness && Looseness <= maxLooseness)
			{
				looseness = Looseness;
				return true;
			}
			return false;
		}

		internal byte MakeWetter(byte amount = 1)
		{
			VaginalWetness oldWetness = wetness;
			wetness = wetness.ByteEnumAdd(amount);
			return wetness - oldWetness;
		}

		internal byte MakeDrier(byte amount = 1)
		{
			VaginalWetness oldWetness = wetness;
			wetness = wetness.ByteEnumSubtract(amount);
			return oldWetness - wetness;
		}
		internal bool SetVaginalWetness(VaginalWetness Wetness)
		{
			if (Wetness >= minWetness && Wetness <= maxWetness)
			{
				wetness = Wetness;
				return true;
			}
			return false;
		}

		internal ushort AddBonusCapacity(ushort amountToAdd)
		{
			ushort currentCapacity = bonusVaginalCapacity;
			bonusVaginalCapacity = bonusVaginalCapacity.add(amountToAdd);
			return bonusVaginalCapacity.subtract(currentCapacity);
		}

		internal ushort SubtractBonusCapacity(ushort amountToRemove)
		{
			ushort currentCapacity = bonusVaginalCapacity;
			bonusVaginalCapacity = bonusVaginalCapacity.subtract(amountToRemove);
			return bonusVaginalCapacity.subtract(currentCapacity);
		}

		internal byte IncreaseMinimumLooseness(byte amount = 1, bool forceIncreaseMax = false)
		{
			VaginalLooseness looseness = minLooseness;
			minLooseness = minLooseness.ByteEnumAdd(amount);
			if (minLooseness > maxLooseness)
			{
				if (forceIncreaseMax)
				{
					maxLooseness = minLooseness;
				}
				else
				{
					minLooseness = maxLooseness;
				}
			}
			return minLooseness - looseness;
		}
		internal byte DecreaseMinimumLooseness(byte amount = 1)
		{
			VaginalLooseness looseness = minLooseness;
			minLooseness = minLooseness.ByteEnumSubtract(amount);
			return looseness - minLooseness;
		}
		internal void SetMinLoosness(VaginalLooseness newValue)
		{
			minLooseness = newValue;
		}

		internal byte IncreaseMaximumLooseness(byte amount = 1)
		{
			VaginalLooseness looseness = maxLooseness;
			maxLooseness = maxLooseness.ByteEnumSubtract(amount);
			return maxLooseness - looseness;
		}
		internal byte DecreaseMaximumLooseness(byte amount = 1, bool forceDecreaseMin = false)
		{
			VaginalLooseness looseness = minLooseness;
			maxLooseness = maxLooseness.ByteEnumSubtract(amount);
			if (minLooseness > maxLooseness)
			{
				if (forceDecreaseMin)
				{
					minLooseness = maxLooseness;
				}
				else
				{
					maxLooseness = minLooseness;
				}
			}
			return looseness - maxLooseness;
		}
		internal void SetMaxLoosness(VaginalLooseness newValue)
		{
			maxLooseness = newValue;
		}

		internal byte IncreaseMinimumWetness(byte amount = 1, bool forceIncreaseMax = false)
		{
			VaginalWetness wetness = minWetness;
			minWetness = minWetness.ByteEnumAdd(amount);
			if (minWetness > maxWetness)
			{
				if (forceIncreaseMax)
				{
					maxWetness = minWetness;
				}
				else
				{
					minWetness = maxWetness;
				}
			}
			return minWetness - wetness;
		}
		internal byte DecreaseMinimumWetness(byte amount = 1)
		{
			VaginalWetness wetness = minWetness;
			minWetness = minWetness.ByteEnumSubtract(amount);
			return wetness - minWetness;
		}
		internal void SetMinWetness(VaginalWetness newValue)
		{
			minWetness = newValue;
		}
		internal byte IncreaseMaximumWetness(byte amount = 1)
		{
			VaginalWetness wetness = maxWetness;
			maxWetness = maxWetness.ByteEnumSubtract(amount);
			return maxWetness - wetness;
		}
		internal byte DecreaseMaximumWetness(byte amount = 1, bool forceDecreaseMin = false)
		{
			VaginalWetness wetness = minWetness;
			maxWetness = maxWetness.ByteEnumSubtract(amount);
			if (minWetness > maxWetness)
			{
				if (forceDecreaseMin)
				{
					minWetness = maxWetness;
				}
				else
				{
					maxWetness = minWetness;
				}
			}
			return wetness - maxWetness;
		}
		internal void SetMaxWetness(VaginalWetness newValue)
		{
			maxWetness = newValue;
		}

		#endregion
		#region Unique Functions
		internal bool VaginalSex(ushort penetratorArea)
		{
			numTimesVaginal++;
			return PenetrateVagina(penetratorArea, true);
		}
		internal bool PenetrateVagina(ushort penetratorArea, bool takeVirginity = false)
		{

			//experience = experience.add(ExperiencedGained);
			VaginalLooseness oldLooseness = looseness;
			ushort capacity = VaginalCapacity();

			//don't have to worry about overflow, as +1 will never overflow our artificial max.
			if (penetratorArea >= capacity * 1.5f)
			{
				looseness++;
			}
			else if (penetratorArea >= capacity && Utils.RandBool())
			{
				looseness++;
			}
			else if (penetratorArea >= capacity * 0.9f && Utils.Rand(4) == 0)
			{
				looseness++;
			}
			else if (penetratorArea >= capacity * 0.75f && Utils.Rand(10) == 0)
			{
				looseness++;
			}
			if (penetratorArea >= capacity / 2)
			{
				vaginaTightenTimer = 0;
			}
			if (virgin && takeVirginity)
			{
				virgin = false;
			}
			return oldLooseness != looseness;
		}
		#endregion

		#region Clit Helpers
		public bool omnibusClit => clit.omnibusClit;

		public bool ActivateOmnibusClit()
		{
			return clit.ActivateOmnibusClit();
		}

		public bool DeactivateOmnibusClit()
		{
			return clit.DeactivateOmnibusClit();
		}
		#endregion
		#region Piercing-Related
		private bool PiercingLocationUnlocked(LabiaPiercings piercingLocation)
		{
			return true;
		}
		#endregion

		#region ITimeListener

		private ushort timerAmount
		{
			get
			{
				if (looseness < VaginalLooseness.LOOSE)
				{
					return 0;
				}
				else if (looseness == VaginalLooseness.LOOSE)
				{
					return LOOSENESS_LOOSE_TIMER;
				}
				else if (looseness == VaginalLooseness.ROOMY)
				{
					return LOOSENESS_ROOMY_TIMER;
				}
				else if (looseness == VaginalLooseness.GAPING)
				{
					return LOOSENESS_GAPING_TIMER;
				}
				else //if (looseness >= VaginalLooseness.CLOWN_CAR_LEVEL)
				{
					return LOOSENESS_CLOWN_CAR_TIMER;
				}
			}
		}

		string IBodyPartTimeLazy.reactToTimePassing(bool isPlayer, byte hoursPassed)
		{
			StringBuilder sb = new StringBuilder();

			if (looseness < minLooseness)
			{
				looseness = minLooseness;
				vaginaTightenTimer = 0;
			}
			else if (looseness > maxLooseness)
			{
				looseness = maxLooseness;
				vaginaTightenTimer = 0;
			}

			else if (looseness > VaginalLooseness.NORMAL && looseness > minLooseness) //whichever is greator.
			{
				vaginaTightenTimer += hoursPassed;
				if (vaginaTightenTimer >= timerAmount)
				{
					if (isPlayer)
					{
						sb.Append(VaginaTightenedUpDueToInactivity(looseness));
					}
					looseness--;
					vaginaTightenTimer = 0;
				}
			}

			else if (vaginaTightenTimer > 0)
			{
				vaginaTightenTimer = 0;
			}

			if (wetness < minWetness)
			{
				wetness = minWetness;
			}
			else if (wetness > maxWetness)
			{
				wetness = maxWetness;
			}

			//if we decide to change behavior so that clit size is forced, call clit react to time passing, appending its result to ours.

			return sb.ToString();
		}

		#endregion

		#region Base Stat Perk
		void IBaseStatPerkAware.GetBasePerkStats(PerkStatBonusGetter getter)
		{
			baseStats = getter;
			clit.GetBasePerkStats(getter);
		}
		#endregion
	}

	public sealed partial class VaginaType : SaveableBehavior<VaginaType, Vagina>
	{
		private static int indexMaker = 0;
		private static readonly List<VaginaType> types = new List<VaginaType>();
		public readonly int typeCapacityBonus;
		private VaginaType(int capacityBonus,
			SimpleDescriptor shortDesc, DescriptorWithArg<Vagina> fullDesc, TypeAndPlayerDelegate<Vagina> playerDesc,
			ChangeType<Vagina> transform, RestoreType<Vagina> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			typeCapacityBonus = capacityBonus;
			types.AddAt(this, _index);
		}

		internal static bool Validate(ref VaginaType vaginaType, bool correctInvalidData)
		{
			if (types.Contains(vaginaType))
			{
				return true;
			}
			else if (correctInvalidData)
			{
				vaginaType = HUMAN;
			}
			return false;
		}

		public override int index => _index;
		private readonly int _index;

		public static readonly VaginaType HUMAN = new VaginaType(0, VagHumanDesc, VagHumanFullDesc, VagHumanPlayerStr, GlobalStrings.TransformToDefault<Vagina, VaginaType>, GlobalStrings.RevertAsDefault);
		public static readonly VaginaType EQUINE = new VaginaType(0, VagEquineDesc, VagEquineFullDesc, VagEquinePlayerStr, VagEquineTransformStr, VagEquineRestoreStr);
		public static readonly VaginaType SAND_TRAP = new VaginaType(0, VagSandTrapDesc, VagSandTrapFullDesc, VagSandTrapPlayerStr, VagSandTrapTransformStr, VagSandTrapRestoreStr);

	}

}
