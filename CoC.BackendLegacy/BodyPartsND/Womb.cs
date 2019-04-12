using CoC.Backend.Pregnancies;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts
{
	//even though technically butt pregnancies dont occur in the womb, i'm going to store them here. 
	//itimeaware: if lays eggs and not pregnant and day % layseggsday = 0, set pregnancy store to egg pregnant.
	public class Womb : BehavioralSaveablePart<Womb, WombType> //itimeaware
	{
		private readonly PregnancyStore pregnancy = new PregnancyStore();
		private readonly PregnancyStore analPregnancy = new PregnancyStore();

		public bool basiliskWombNowDefault { get; protected set; } = false;

		public bool isStandard => type == WombType.STANDARD;
		public override bool isDefault => basiliskWombNowDefault ? type == WombType.BASILISK : type == WombType.STANDARD;
		
		public override WombType type
		{
			get => _type;
			protected set
			{
				if (value == WombType.BASILISK)
				{
					basiliskWombNowDefault = true;
				}
				_type = value;
			}
		}
		private WombType _type;

		private protected Womb()
		{
			type = WombType.STANDARD;
		}

		private protected Womb(WombType wombType)
		{
			type = wombType;
		}

		internal static Womb GenerateDefault()
		{
			return new Womb();
		}

		internal static Womb GenerateDefaultOfType(WombType wombType)
		{
			return new Womb(wombType);
		}

		internal bool UpdateWomb(WombType wombType)
		{
			if (type == wombType)
			{
				return false;
			}
			else if (basiliskWombNowDefault && type == WombType.BASILISK && wombType == WombType.STANDARD)
			{
				return false;
			}
			type = wombType;
			return true;
		}

		internal bool UpdateWombAndEggPregnate(EggWomb eggWomb, out bool nowPregnantWithEggs)
		{
			bool changedType = true;
			if (type == eggWomb)
			{
				changedType = false;
			}
			if (pregnancy.isPregnant)
			{
				nowPregnantWithEggs = false;
			}
			else
			{
				nowPregnantWithEggs = true;
				throw new NotImplementedException();
			}
			return changedType;
		}

		internal override bool Restore()
		{
			if (isDefault)
			{
				return false;
			}
			else if (basiliskWombNowDefault)
			{
				type = WombType.BASILISK;
			}
			else
			{
				type = WombType.STANDARD;
			}
			return true;
		}

		internal void Reset(bool removePregnancies = false)
		{
			basiliskWombNowDefault = false;
			type = WombType.STANDARD;
			if (removePregnancies)
			{
				analPregnancy.Reset();
				pregnancy.Reset();
			}
		}

		internal override bool Validate(bool correctDataIfInvalid = false)
		{
			bool valid = true;
			if (type == null)
			{
				if (correctDataIfInvalid)
				{
					type = WombType.STANDARD;
				}
				valid = false;
			}
			else if (type == WombType.BASILISK && !basiliskWombNowDefault)
			{
				if (correctDataIfInvalid)
				{
					basiliskWombNowDefault = true;
				}
			}
			return valid && pregnancy.Validate(correctDataIfInvalid) && analPregnancy.Validate(correctDataIfInvalid);
		}
		private protected override Type currentSaveVersion => throw new NotImplementedException();

		private protected override Type[] saveVersions => throw new NotImplementedException();

		private protected override BehavioralSurrogateBase<Womb, WombType> ToCurrentSave()
		{
			throw new NotImplementedException();
		}
	}

	public partial class WombType : SaveableBehavior<WombType, Womb>
	{
		private static int indexMaker = 0;
		private static readonly List<WombType> wombs = new List<WombType>();

		private readonly int _index;
		public virtual bool laysEggs => false;

		public WombType(
			SimpleDescriptor shortDesc, DescriptorWithArg<Womb> fullDesc, TypeAndPlayerDelegate<Womb> playerDesc, 
			ChangeType<Womb> transform, RestoreType<Womb> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			wombs.AddAt(this, _index);
		}
		public override int index => _index;

		public static WombType STANDARD = new WombType(StandardDesc, StandardFullDesc, StandardPlayerStr, StandardTransformStr, StandardRestoreStr);
		public static EggWomb EGG_LAYING = new EggWomb(15, EggDesc, EggFullDesc, EggPlayerStr, EggTransformStr, EggRestoreStr);
		public static EggWomb BUNNY = new EggWomb(15, BunnyDesc, BunnyFullDesc, BunnyPlayerStr, BunnyTransformStr, BunnyRestoreStr);
		public static EggWomb BASILISK = new EggWomb(15, BasiliskDesc, BasiliskFullDesc, BasiliskPlayerStr, BasiliskTransformStr, BasiliskRestoreStr);
	}

	public class EggWomb : WombType
	{
		public override bool laysEggs => true;
		public readonly int eggsSpawnEveryXDays;
		public EggWomb(int  daysBetweenEggSpawns, 
			SimpleDescriptor shortDesc, DescriptorWithArg<Womb> fullDesc, TypeAndPlayerDelegate<Womb> playerDesc, 
			ChangeType<Womb> transform, RestoreType<Womb> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			eggsSpawnEveryXDays = daysBetweenEggSpawns;
		}
	}

	public sealed class WombSerializerVersion1s
	{

	}
}
