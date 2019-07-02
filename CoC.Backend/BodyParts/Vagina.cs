//Vagina.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 5:57 PM
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
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
		public readonly Clit clit;

		private BasePerkDataGetter baseStats;

		private VaginalLooseness minLooseness => baseStats?.Invoke().minVaginalLooseness ?? VaginalLooseness.TIGHT; //now safe for initializers.
		private VaginalLooseness maxLooseness => baseStats?.Invoke().maxVaginalLooseness ?? VaginalLooseness.CLOWN_CAR_WIDE; //see above.

		private VaginalWetness minWetness => baseStats?.Invoke().minVaginalWetness ?? VaginalWetness.DRY;
		private VaginalWetness maxWetness => baseStats?.Invoke().minVaginalWetness ?? VaginalWetness.SLAVERING;

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
		public bool virgin { get; private set; }


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

		//public 

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
		void IBaseStatPerkAware.GetBasePerkStats(BasePerkDataGetter getter)
		{
			baseStats = getter;
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
