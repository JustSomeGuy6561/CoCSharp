//Womb.cs
//Description:
//Author: JustSomeGuy
//6/28/2019, 10:30 PM
using CoC.Backend.BodyParts;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Engine.Time;
using CoC.Backend.Pregnancies;
using CoC.Frontend.Pregnancies;
using System;


//womb is annoying because there are no common rules, so you have to make some stuff virtual and override it as needed. i really hate doing this, but i suppose it works fine.

//Pregnancy Store is generic. it can be used for anyone, by anything. but the SpawnType variable should be unique for player->NPC, NPC->player, or NPC->NPC (or other) spawns.
//if the spawnType is not unique, it must provide a constructor that lets the user determine which behavior it will have (like an ENUM or pair of bools or something, idk)
//it's not my job to make sure you give the right SpawnType to the right PregnancyStore, but it's pretty straightforward imo.

//This version is suited toward the player, allowing the player to obtain or remove oviposition and/or diapause. it may be possible to use this elsewhere if it suits you.

namespace CoC.Frontend.Creatures.PlayerData
{
	//even though technically butt pregnancies dont occur in the womb, i'm going to store them here.

	public sealed partial class PlayerWomb : Womb
	{
		//egg related.
		public bool laysEggs => basiliskWomb || hasOviposition;
		//prevents laysEggs from being false;
		public bool basiliskWomb { get; private set; } = false;

		public override bool allowsDiapause => true;
		public override bool allowsDiapauseRemoval => true;

		public override bool allowsOviposition => true;
		public override bool allowsOvipositionRemoval => !basiliskWomb;



		//the player can get anally pregnant, even if the source does not force it. This allows things like bunny eggs in the ass, etc.
		//note that this only applies if the player has an anus (which they should) and the spawn type allows anal pregnancies.
		protected override bool AllowsAnalPregnancies(bool sourceIgnoresAnalPreferences)
		{
			return true;
		}

		public PlayerWomb(Guid creatureID) : base(creatureID, true, true)
		{

		}

		public override WombData AsReadOnlyData()
		{
			return new PlayerWombData(this);
		}

		public bool GrantBasiliskWomb()
		{
			hasOviposition = true;
			if (basiliskWomb)
			{
				return false;
			}
			basiliskWomb = true;
			return true;
		}

		public void ClearBasiliskWomb(bool clearOviposition = true)
		{
			basiliskWomb = false;
			if (hasOviposition && clearOviposition)
			{
				hasOviposition = false;
			}
		}

		protected override bool ExtraValidations(bool currentlyValid, bool correctInvalidData)
		{
			return currentlyValid;
		}

		#region ITimeListeners

		protected override byte eggHourTrigger => 0;

		protected override string ReactToDailyTrigger()
		{
			if (!normalPregnancy.isPregnant && laysEggs && GameEngine.CurrentDay % eggsEveryXDays == 0)
			{
				AttemptNormalKnockUp(1, new PlayerEggPregnancy(creatureID));
				return EggSpawnText();
			}
			return null;
		}

		#endregion
	}

	public sealed class PlayerWombData : WombData
	{
		public bool laysEggs => basiliskWomb || hasOviposition;
		//prevents laysEggs from being false;
		public readonly bool basiliskWomb;


		internal PlayerWombData(PlayerWomb womb) : base(womb)
		{
			basiliskWomb = womb.basiliskWomb;
		}
	}


	//public sealed class Womb : BehavioralSaveablePart<Womb, WombType>, ITimeActiveListener, IBodyPartTimeDaily
	//{
	//	internal readonly PregnancyStore pregnancy = new PregnancyStore(true);
	//	internal readonly PregnancyStore analPregnancy = new PregnancyStore(false);

	//	public bool basiliskWombNowDefault { get; private set; } = false;

	//	public bool isStandard => type == WombType.STANDARD;
	//	public override bool isDefault => basiliskWombNowDefault ? type == WombType.BASILISK : type == WombType.STANDARD;

	//	public override WombType type
	//	{
	//		get => _type;
	//		protected set
	//		{
	//			if (value == WombType.BASILISK)
	//			{
	//				basiliskWombNowDefault = true;
	//			}
	//			_type = value;
	//		}
	//	}

	//	private WombType _type;

	//	private Womb()
	//	{
	//		type = WombType.STANDARD;
	//	}

	//	private Womb(WombType wombType)
	//	{
	//		type = wombType;
	//	}

	//	internal static Womb GenerateDefault()
	//	{
	//		return new Womb();
	//	}

	//	internal static Womb GenerateDefaultOfType(WombType wombType)
	//	{
	//		return new Womb(wombType);
	//	}

	//	internal override bool UpdateType(WombType newType)
	//	{
	//		if (type == wombType)
	//		{
	//			return false;
	//		}
	//		else if (basiliskWombNowDefault && wombType == WombType.STANDARD)
	//		{
	//			return false;
	//		}
	//		type = wombType;
	//		return true;
	//	}

	//	//new Harpy womb perk - old perk never expressly forced you to have eggs.
	//	//but made any eggs you had from that point forward always start out large.
	//	//now, you can force eggs to start out either small or large via this function,

	//	//Note that this is a wrapper for the pregnancy store function, as it's internal and there's no other way to access it.
	//	//this is so that we can make it work for both pregnancy stores at once, and only give you one place to call it.
	//	//it's easier to debug if there's only one way to make something happen. this also forces this to apply to anal pregnancies if eggs are ever allowed there
	//	//it's possible to have something lay eggs in your ass right now, though they're already fertilized, or they dissolve over time, making both irrelevant.
	//	//i have no idea if future changes may allow anal egg laying, but if it does happen, this perk will affect it.

	//	public void SetEggSize(bool isLarge = true)
	//	{
	//		pregnancy.SetEggSize(isLarge);
	//		analPregnancy.SetEggSize(isLarge);
	//	}

	//	//clears egg size "perk". now eggs are sized randomly.
	//	public void ClearEggSize()
	//	{
	//		pregnancy.ClearEggSize();
	//		analPregnancy.ClearEggSize();
	//	}

	//	internal override bool Restore()
	//	{
	//		if (isDefault)
	//		{
	//			return false;
	//		}
	//		else if (basiliskWombNowDefault)
	//		{
	//			type = WombType.BASILISK;
	//		}
	//		else
	//		{
	//			type = WombType.STANDARD;
	//		}
	//		return true;
	//	}

	//	internal void Reset(bool removePregnancies = false)
	//	{
	//		basiliskWombNowDefault = false;
	//		type = WombType.STANDARD;
	//		if (removePregnancies)
	//		{
	//			analPregnancy.Reset();
	//			pregnancy.Reset();
	//		}
	//	}

	//	internal override bool Validate(bool correctInvalidData)
	//	{
	//		bool valid = true;
	//		if (type == null)
	//		{
	//			if (correctInvalidData)
	//			{
	//				type = WombType.STANDARD;
	//			}
	//			valid = false;
	//		}
	//		else if (type == WombType.BASILISK && !basiliskWombNowDefault)
	//		{
	//			if (correctInvalidData)
	//			{
	//				basiliskWombNowDefault = true;
	//			}
	//		}
	//		valid &= pregnancy.Validate(correctInvalidData);
	//		valid &= analPregnancy.Validate(correctInvalidData);

	//		//if we're correcting data or currently valid and either the pregnancy stores don't agree on eggSize.
	//		if ((correctInvalidData || valid) && (pregnancy.eggSizeKnown != analPregnancy.eggSizeKnown || (pregnancy.eggSizeKnown && pregnancy.eggsLarge != analPregnancy.eggsLarge)))
	//		{
	//			//prioritize the normal pregnancy stat.
	//			if (correctInvalidData)
	//			{
	//				//if normal is clear, clear anal egg size.
	//				if (!pregnancy.eggSizeKnown)
	//				{
	//					analPregnancy.ClearEggSize();
	//				}
	//				//othwerwise set the anal egg size to match the normal one.
	//				else
	//				{
	//					analPregnancy.SetEggSize(pregnancy.eggsLarge);
	//				}
	//			}
	//			valid = false;
	//		}
	//		return valid;
	//	}

	//	#region ITimeListener


	//	void ITimeListener.ReactToTimePassing(byte hoursPassed)
	//	{
	//		bool needsOutput = false;
	//		StringBuilder outputBuilder = new StringBuilder();

	//		PregnancyStore[] pregnancyStores = new PregnancyStore[] { pregnancy, analPregnancy };

	//		foreach (PregnancyStore store in pregnancyStores)
	//		{
	//			if (store is ITimeListenerWithShortOutput outputListener)
	//			{
	//				outputListener.ReactToTimePassing(hoursPassed);
	//				if (outputListener.RequiresSmallOutput)
	//				{
	//					needsOutput = true;
	//					outputBuilder.Append(outputListener.SmallOutput());
	//				}
	//			}
	//		}
	//		if (!pregnancy.isPregnant && timeListener.OnceDailyCheck(hoursPassed, 6))
	//		{
	//			DoDaily();
	//		}
	//	}
	//	bool ITimeListenerWithShortOutput.RequiresSmallOutput => needsOutput;

	//	private ITimeListener timeListener => this;

	//	string ITimeListenerWithShortOutput.SmallOutput()
	//	{
	//		return outputBuilder.ToString();
	//	}

	//	private void DoDaily()
	//	{
	//		if (type is EggWomb eggWomb && timeListener.CurrentDay() % eggWomb.eggsSpawnEveryXDays == 0)
	//		{
	//			needsOutput = true;
	//			outputBuilder.Append(eggWomb.eggsSpawnedInWombStr());
	//			pregnancy.attemptKnockUp(1, new PlayerEggPregnancy());
	//		}
	//	}
	//	#endregion

	//}
	//public partial class WombType : SaveableBehavior<WombType, Womb>
	//{
	//	private static int indexMaker = 0;
	//	private static readonly List<WombType> wombs = new List<WombType>();

	//	private readonly int _index;
	//	public virtual bool laysEggs => false;

	//	public WombType(
	//		SimpleDescriptor shortDesc, DescriptorWithArg<Womb> fullDesc, TypeAndPlayerDelegate<Womb> playerDesc,
	//		ChangeType<Womb> transform, RestoreType<Womb> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
	//	{
	//		_index = indexMaker++;
	//		wombs.AddAt(this, _index);
	//	}
	//	public override int index => _index;

	//	public static WombType STANDARD = new WombType(StandardDesc, StandardFullDesc, StandardPlayerStr, StandardTransformStr, StandardRestoreStr);
	//	public static EggWomb EGG_LAYING = new EggWomb(15, EggSpawnText, EggDesc, EggFullDesc, EggPlayerStr, EggTransformStr, EggRestoreStr);
	//	//harpy womb is not really a womb, i guess. it just forces all eggs laid to be large.
	//	public static EggWomb BUNNY = new EggWomb(15, BunnySpawnText, BunnyDesc, BunnyFullDesc, BunnyPlayerStr, BunnyTransformStr, BunnyRestoreStr);
	//	public static EggWomb BASILISK = new EggWomb(15, BasiliskSpawnText, BasiliskDesc, BasiliskFullDesc, BasiliskPlayerStr, BasiliskTransformStr, BasiliskRestoreStr);
	//	//public static WombType KANGAROO = new KangarooWomb(); //i have no idea how to implement this.

	//}

	//public enum EggWombSources { OVI_POTION, BASILISK, BUNNY }

	//public class EggWomb : WombType
	//{
	//	public override bool laysEggs => true;
	//	public readonly int eggsSpawnEveryXDays;

	//	public readonly SimpleDescriptor eggsSpawnedInWombStr;
	//	public EggWomb(int daysBetweenEggSpawns, SimpleDescriptor eggsSpawnedInWombText,
	//		SimpleDescriptor shortDesc, DescriptorWithArg<Womb> fullDesc, TypeAndPlayerDelegate<Womb> playerDesc,
	//		ChangeType<Womb> transform, RestoreType<Womb> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
	//	{
	//		eggsSpawnEveryXDays = daysBetweenEggSpawns;
	//		eggsSpawnedInWombStr = eggsSpawnedInWombText;
	//	}
	//}



}
