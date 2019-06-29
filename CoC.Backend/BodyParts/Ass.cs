//Ass.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 5:21 PM
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Tools;
using System.Text;

namespace CoC.Backend.BodyParts
{
	//as with looseness, anal wetness defaults to dry, so that should be normal. 
	//Revised ruling: Normal - no wetness. a rather uncomfortable experience unless you're used to it. DAMP - slightly wet, should be considered barely self-lubricating. 
	//Not exactly strange (especially by mareth standards), it just suggests you're rather into recieving anal penetration. obtained naturally if you have an anal fetish 
	//or if you have a lot of experience in anal sex.
	//moist and up: same as before. not naturally achievable, though of course TF items or interactions can cause it. 
	public enum AnalWetness : byte { NORMAL, DAMP, MOIST, SLIMY, DROOLING, SLIME_DROOLING }

	//Pretty sure normal for ass size is pretty tight. so

	public enum AnalLooseness : byte { NORMAL, LOOSE, ROOMY, STRETCHED, GAPING } //if you want to add a clown car level here, may i suggest RENT_ASUNDER?
	public sealed partial class Ass: IBodyPartTimeLazy //, IPerkAware
	{
		public const ushort MAX_ANAL_CAPACITY = ushort.MaxValue;

		private const byte LOOSENESS_LOOSE_TIMER = 72;
		private const byte LOOSENESS_ROOMY_TIMER = 48;
		private const byte LOOSENESS_STRETCHED_TIMER = 24;
		private const byte LOOSENESS_GAPING_TIMER = 12;

		private byte buttTightenTimer = 0;

		public AnalWetness wetness
		{
			get => _analWetness;
			private set
			{
				byte val = (byte)value;
				Utils.Clamp(ref val, (byte)AnalWetness.NORMAL, (byte)AnalWetness.SLIME_DROOLING);
				_analWetness = (AnalWetness)val;
			}
		}
		private AnalWetness _analWetness = 0;

		public AnalLooseness looseness
		{
			get => _analLooseness;
			private set
			{
				byte val = (byte)value;
				//if we shrink or grow the looseness, reset the timer. 
				if (value != _analLooseness)
				{
					buttTightenTimer = 0;
				}
				Utils.Clamp(ref val, (byte)minAnalLooseness, (byte)AnalLooseness.GAPING);
				_analLooseness = (AnalLooseness)val;
			}
		}
		private AnalLooseness _analLooseness = AnalLooseness.NORMAL;

		private AnalLooseness minAnalLooseness = AnalLooseness.NORMAL;

		public ushort bonusAnalCapacity { get; private set; }

		public ushort analCapacity()
		{
			ushort capacity = 0;

			byte loose = (byte)looseness;
			byte wet = (byte)wetness;
			//get current bonus capacity from perks
			uint cap = (uint)(capacity + bonusAnalCapacity + experience / 10 + 6 * loose * loose * (1 + wet) / 10);
			if (cap > ushort.MaxValue)
			{
				return ushort.MaxValue;
			}
			return (ushort)cap;
		}

		//how "experienced" the character is with anal sex. not used atm. as of now, it just increases by 1 with each experience. 
		//idk, maybe change this.
		public byte experience
		{
			get => _experience;
			set
			{
				_experience = Utils.Clamp2<byte>(value, 0, 100);
			}
		}
		private byte _experience;


		public ushort numTimesAnal
		{
			get; private set;
		}
		public bool virgin { get; private set; } = true;

		SimpleDescriptor shortDescription => shortDesc;
		SimpleDescriptor fullDescription => fullDesc;

		private Ass()
		{
			looseness = 0;
			wetness = 0;
			numTimesAnal = 0;
		}

		internal static Ass GenerateDefault()
		{
			return new Ass();
		}

		//default behavior is to let the ass determine if it's still virgin.
		//allows PC to masturbate/"practice" w/o losing anal virginity.
		//if set to false, it will be ignored if looseness is still virgin.
		//nothing here can be null so we're fine.
		internal static Ass Generate(AnalWetness analWetness, AnalLooseness analLooseness, bool? virginAnus = null)
		{
			Ass ass = new Ass()
			{
				wetness = analWetness,
				looseness = analLooseness
			};
			//if not set or explicitly null
			if (virginAnus == null)
			{
				ass.virgin = analLooseness == AnalLooseness.NORMAL;
			}
			else
			{
				ass.virgin = (bool)virginAnus;
			}
			return ass;
		}

		internal byte StretchAnus(byte amount = 1, bool permanent = false)
		{
			byte oldLooseness = (byte)looseness;
			looseness += amount;
			return ((byte)looseness).subtract(oldLooseness);
		}

		internal byte ShrinkAnus(byte amount = 1)
		{
			byte oldLooseness = (byte)looseness;
			looseness -= amount;
			return oldLooseness.subtract((byte)looseness);
		}

		internal void SetMinAnalLooseness(AnalLooseness minLooseness)
		{
			minAnalLooseness = minLooseness;
			if (looseness < minAnalLooseness)
			{
				looseness = minLooseness;
			}
		}

		internal bool SetAnalLooseness(AnalLooseness analLooseness, bool forceIfLessThanCurrentMin = false)
		{
			if (forceIfLessThanCurrentMin && analLooseness < minAnalLooseness)
			{
				minAnalLooseness = analLooseness;
			}
			if (analLooseness >= minAnalLooseness)
			{
				looseness = analLooseness;
				return true;
			}
			return false;
		}

		internal byte AddWetness(byte amount = 1)
		{
			byte oldWetness = (byte)wetness;
			wetness += amount;
			return ((byte)wetness).subtract(oldWetness);
		}

		internal byte SubtractWetness(byte amount = 1)
		{
			byte oldWetness = (byte)wetness;
			wetness -= amount;
			return oldWetness.subtract((byte)wetness);
		}

		internal void ForceAnalWetness(AnalWetness analWetness)
		{
			wetness = analWetness;
		}
		


		//move this helper to the creature class.
		////takes a cock, optionally attempting to impregnate the character with it. 
		//internal bool analSex(Cock cock, bool canPreggers = false, byte analExperienceGained = 1)
		//{
		//	numTimesAnal++;
		//	return analPenetrate(cock.CockArea(), analExperienceGained);
		//	//todo: hold off return until attempted knockup. 
		//}

		//allows non-dicks to count - finger/fist/large clits/dildo, idk.
		internal bool analPenetrate(ushort penetratorArea, bool takeAnalVirginity = false, byte analExperiencedGained = 1)
		{

			experience += analExperiencedGained;
			AnalLooseness oldLooseness = looseness;
			ushort capacity = analCapacity();

			if (penetratorArea >= capacity * 1.5f)
			{
				looseness++;
			}
			else if (penetratorArea >= capacity && Utils.RandBool())
			{
				looseness++;
			}
			else if (penetratorArea >= analCapacity() * 0.9f && Utils.Rand(4) == 0)
			{
				looseness++;
			}
			else if (penetratorArea >= analCapacity() * 0.75f && Utils.Rand(10) == 0)
			{
				looseness++;
			}
			if (penetratorArea >= capacity / 2)
			{
				buttTightenTimer = 0;
			}
			if (virgin && takeAnalVirginity)
			{
				virgin = false;
			}
			return oldLooseness != looseness;
		}
		private byte timerAmount
		{
			get
			{
				if (looseness < AnalLooseness.LOOSE)
				{
					return 0;
				}
				else if (looseness == AnalLooseness.LOOSE)
				{
					return LOOSENESS_LOOSE_TIMER;
				}
				else if (looseness == AnalLooseness.ROOMY)
				{
					return LOOSENESS_ROOMY_TIMER;
				}
				else if (looseness == AnalLooseness.STRETCHED)
				{
					return LOOSENESS_STRETCHED_TIMER;
				}
				else //if (looseness >= AnalLooseness.GAPING)
				{
					return LOOSENESS_GAPING_TIMER;
				}
			}
		}

		bool IBodyPartTimeLazy.reactToTimePassing(bool isPlayer, byte hoursPassed, out string output)
		{
			bool needsOutput = false;
			StringBuilder outputBuilder = new StringBuilder();
			//if has a perk that makes ass a certain looseness
			//parse it. set any output flags accordingly.
			/*else */if (looseness > minAnalLooseness)
			{
				buttTightenTimer += hoursPassed;
				if (buttTightenTimer >= timerAmount)
				{
					if (isPlayer)
					{
						needsOutput = true;
						outputBuilder.Append(AssTightenedUpDueToInactivity(looseness));
					}
					looseness--;
					buttTightenTimer = 0;
				}
			}

			else if (buttTightenTimer > 0)
			{
				buttTightenTimer = 0;
			}

			output = outputBuilder.ToString();
			return needsOutput;
		}

	}
}