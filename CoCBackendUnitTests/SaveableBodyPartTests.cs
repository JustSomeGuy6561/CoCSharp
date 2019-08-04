using CoC;
using CoC.Backend.BodyParts;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Perks;
using CoC.Backend.SaveData;
using CoC.Backend.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
namespace CoCBackendUnitTests
{
	public class FakePerkData : BasePerkModifiers
	{

	}

	[TestClass]
	public class SaveableBodyPartTests
	{
		[TestMethod]
		public void Ass_FullUnitTests()
		{
			Ass ass = Ass.GenerateDefault();
			Assert.IsTrue(ass.virgin);
			Assert.AreEqual(AnalLooseness.NORMAL, ass.looseness);
			Assert.AreEqual(AnalWetness.NORMAL, ass.wetness);
			Assert.AreEqual(AnalLooseness.NORMAL, ass.minLooseness);
			Assert.AreEqual(AnalWetness.NORMAL, ass.minWetness);
			Assert.AreEqual(AnalLooseness.GAPING, ass.maxLooseness);
			Assert.AreEqual(AnalWetness.SLIME_DROOLING, ass.maxWetness);
			Assert.AreEqual<ushort>(0, ass.bonusAnalCapacity);
			Assert.AreEqual(Ass.BASE_CAPACITY, ass.analCapacity());
			Assert.AreEqual<ushort>(0, ass.numTimesAnal);
			Assert.AreEqual<byte>(3, ass.StretchAnus(3));
			Assert.AreEqual(AnalLooseness.STRETCHED, ass.looseness);
			Assert.AreEqual<byte>(4, ass.MakeWetter(4));
			Assert.AreEqual(AnalWetness.DROOLING, ass.wetness);
			Assert.AreNotEqual<byte>(7, ass.ShrinkAnus(7));
			Assert.AreEqual(AnalLooseness.NORMAL, ass.looseness);
			Assert.AreNotEqual(3, ass.MakeWetter(3));
			Assert.AreEqual(AnalWetness.SLIME_DROOLING, ass.wetness);

			Assert.AreEqual(Ass.BASE_CAPACITY, ass.analCapacity());
			Utils.DebugSetRandomSeed(1500);
			ass.PenetrateAsshole(45);
			Assert.AreEqual(AnalLooseness.ROOMY, ass.looseness);
			Assert.IsTrue(ass.virgin);
			Assert.IsTrue(ass.everPracticedAnal);
			Assert.AreNotEqual(Ass.BASE_CAPACITY, ass.analCapacity());
			Assert.AreEqual((ushort)Math.Floor(10 + 0.6 * (byte)ass.wetness.ByteEnumAdd(1) * (byte)ass.looseness.ByteEnumAdd(1) * (byte)ass.looseness.ByteEnumAdd(1)), ass.analCapacity());
			Assert.AreEqual<ushort>(0, ass.numTimesAnal);

			ass.AnalSex(9001);
			Assert.AreEqual<ushort>(1, ass.numTimesAnal);
			Assert.IsFalse(ass.virgin);
			Assert.AreEqual(AnalLooseness.GAPING, ass.looseness);
			Assert.AreEqual((ushort)Math.Floor(10 + 0.6 * (byte)ass.wetness.ByteEnumAdd(1) * (byte)ass.looseness.ByteEnumAdd(1) * (byte)ass.looseness.ByteEnumAdd(1)), ass.analCapacity());

			ass = Ass.Generate(AnalWetness.DAMP, AnalLooseness.LOOSE, true);
			Assert.IsTrue(ass.everPracticedAnal);
			Assert.IsTrue(ass.virgin);
			Utils.DebugSetRandomSeed(1776); //rolls 2, 3, 0, 1, assuming rollsizes of 4,4,4,2. Note that 1 in randBool is true, 0 is false.
			ass.PenetrateAsshole(14, true);
			Assert.AreEqual(AnalLooseness.LOOSE, ass.looseness);
			ass.PenetrateAsshole(14, true);
			Assert.AreEqual(AnalLooseness.LOOSE, ass.looseness);
			ass.PenetrateAsshole(14, true);
			Assert.AreEqual(AnalLooseness.ROOMY, ass.looseness);

			ass.SetAnalLooseness(AnalLooseness.LOOSE);
			Assert.AreEqual(AnalLooseness.LOOSE, ass.looseness);
			ass.SetAnalWetness(AnalWetness.MOIST);
			//capacity: 17.
			Assert.AreEqual<ushort>(17, ass.analCapacity());
			ass.AnalSex(18);
			Assert.AreEqual(AnalLooseness.ROOMY, ass.looseness);
			//ROOMY timer: 48
			((IBodyPartTimeLazy)ass).reactToTimePassing(false, 48);
			Assert.AreEqual(AnalLooseness.LOOSE, ass.looseness);
			ass.SetAnalLooseness(AnalLooseness.ROOMY);
			((IBodyPartTimeLazy)ass).reactToTimePassing(false, 36); //timer now 12.
			ass.AnalSex(15); //less than .75, so we won't roll for making bigger. but >0.5 so it should reset the timer.
			((IBodyPartTimeLazy)ass).reactToTimePassing(false, 12); //if failed, timer is 0. if successful, timer is now 36.
			Assert.AreEqual(AnalLooseness.ROOMY, ass.looseness);
			((IBodyPartTimeLazy)ass).reactToTimePassing(false, 36); //timer now 0.
			Assert.AreEqual(AnalLooseness.LOOSE, ass.looseness);

			SaveSystem.AddSessionSave(new BackendSessionData()); //dummy data to allow strings to work. 

			Utils.DebugSetRandomSeed(0);
			string expected = Environment.NewLine + SafelyFormattedString.FormattedText("Your " + ass.shortDescription() + " recovers from your ordeals, tightening up a bit.", StringFormats.BOLD) + Environment.NewLine;
			Utils.DebugSetRandomSeed(0);
			Assert.AreEqual(expected, ((IBodyPartTimeLazy)ass).reactToTimePassing(true, 96));//timer now 0
			Assert.AreEqual(AnalLooseness.NORMAL, ass.looseness);

			ass.AddBonusCapacity(25);
			Assert.AreEqual<ushort>(25, ass.bonusAnalCapacity);
			Assert.AreEqual<ushort>(37, ass.analCapacity());

			//unit test perks with fake data.
			FakePerkData fakePerkData = new FakePerkData() { PerkBasedBonusAnalCapacity = 20 };
			BasePerkModifiers perkStat() => fakePerkData; //slut perk, i guess.
			((IBaseStatPerkAware)ass).GetBasePerkStats(perkStat);
			Assert.AreEqual<ushort>(57, ass.analCapacity());
			fakePerkData.minAnalLooseness = AnalLooseness.ROOMY; //give it the buttslut perk. 
			((IBodyPartTimeLazy)ass).reactToTimePassing(false, 1); //hack to proc the buttslut since we're not currently enforcing perks. 
			Assert.AreEqual(AnalLooseness.ROOMY, ass.looseness);
			((IBodyPartTimeLazy)ass).reactToTimePassing(false, 48); //timer is now 0 if ignoring the min looseness.
			Assert.AreEqual(AnalLooseness.ROOMY, ass.looseness); //this would fail if the above is true.

			fakePerkData.maxAnalLooseness = AnalLooseness.STRETCHED;
			ass.SetAnalLooseness(AnalLooseness.GAPING);
			Assert.AreEqual(AnalLooseness.STRETCHED, ass.looseness);

			fakePerkData.maxAnalWetness = AnalWetness.DROOLING;
			ass.SetAnalWetness(AnalWetness.SLIME_DROOLING);
			Assert.AreEqual(AnalWetness.DROOLING, ass.wetness);
			fakePerkData.minAnalWetness = AnalWetness.MOIST;
			ass.MakeDrier(4);
			Assert.AreEqual(AnalWetness.MOIST, ass.wetness);
			fakePerkData.maxAnalWetness = AnalWetness.SLIME_DROOLING;
			ass.SetAnalWetness(AnalWetness.SLIME_DROOLING);
			Assert.AreEqual(AnalWetness.SLIME_DROOLING, ass.wetness);
		}

		[TestMethod]
		public void Balls_FullUnitTests()
		{
			Balls balls = Balls.GenerateDefault(false);
			Assert.IsFalse(balls.hasBalls);
			Assert.AreEqual<byte>(0, balls.size);
			Assert.AreEqual<byte>(0, balls.count);

			FakePerkData fakePerkData = new FakePerkData();

			Assert.IsTrue(balls.growBalls());
			Assert.AreEqual(Balls.DEFAULT_BALLS_SIZE, balls.size);
			Assert.AreEqual(Balls.DEFAULT_BALLS_COUNT, balls.count);

			//balls will actually function without this, but still.
			((IBaseStatPerkAware)balls).GetBasePerkStats(() => fakePerkData);

			Assert.AreEqual<byte>(4, balls.addBalls(4));
			Assert.AreEqual<byte>(6, balls.count);

			Assert.AreNotEqual<byte>(4, balls.addBalls(4));
			Assert.AreEqual<byte>(8, balls.count);

			Assert.AreNotEqual<byte>(3, balls.removeBalls(3));
			Assert.AreEqual<byte>(4, balls.count);

			Assert.IsFalse(balls.growUniBall());
			Assert.IsTrue(balls.removeAllBalls());
			Assert.IsFalse(balls.hasBalls);
			Assert.IsTrue(balls.growUniBall());
			Assert.AreEqual<byte>(1, balls.count);
			Assert.AreEqual(Balls.UNIBALL_SIZE_THRESHOLD, balls.size);
			Assert.IsTrue(balls.uniBall);
			Assert.IsFalse(balls.makeUniBall());

			Assert.AreEqual<byte>(0, balls.ShrinkBalls(4, true));
			Assert.AreEqual<byte>(2, balls.EnlargeBalls(2, false));
			Assert.AreEqual<byte>(3, balls.size);
			Assert.AreEqual<byte>(2, balls.count);
			Assert.IsFalse(balls.uniBall);

			balls = Balls.GenerateDefault(true);
			Assert.IsTrue(balls.hasBalls);
			Assert.AreEqual(Balls.DEFAULT_BALLS_SIZE, balls.size);
			Assert.AreEqual(Balls.DEFAULT_BALLS_COUNT, balls.count);
			Assert.IsFalse(balls.makeStandard());

			balls = Balls.GenerateFromGender(Gender.FEMALE);
			Assert.IsFalse(balls.hasBalls);
			balls = Balls.GenerateFromGender(Gender.MALE);
			Assert.IsTrue(balls.hasBalls);
			balls = Balls.GenerateFromGender(Gender.GENDERLESS);
			Assert.IsFalse(balls.hasBalls);
			balls = Balls.GenerateFromGender(Gender.HERM);
			Assert.IsTrue(balls.hasBalls);

			Assert.IsTrue(balls.makeUniBall());
			Assert.AreEqual<byte>(1, balls.count);
			Assert.AreEqual(Balls.UNIBALL_SIZE_THRESHOLD, balls.size);
			Assert.IsTrue(balls.uniBall);
			Assert.IsTrue(balls.makeStandard());
			Assert.AreEqual(Balls.DEFAULT_BALLS_SIZE, balls.size);
			Assert.AreEqual(Balls.DEFAULT_BALLS_COUNT, balls.count);

			//balls will actually function without this, but still.
			((IBaseStatPerkAware)balls).GetBasePerkStats(() => fakePerkData);
			fakePerkData.BallsGrowthMultiplier = 2.0f;
			fakePerkData.BallsShrinkMultiplier = 0.5f;
			fakePerkData.NewBallsDefaultSize = 3;
			balls.removeAllBalls();
			balls.growBalls();
			Assert.AreEqual<byte>(3, balls.size);
			balls.EnlargeBalls(2, false);
			Assert.AreEqual<byte>(7, balls.size);
			balls.ShrinkBalls(4, true);
			Assert.AreEqual<byte>(3, balls.size);
			balls.ShrinkBalls(2, false);
			Assert.AreEqual<byte>(2, balls.size);
			balls.EnlargeBalls(2, true);
			Assert.AreEqual<byte>(4, balls.size);

			balls = Balls.GenerateBalls(4, 7);
			Assert.AreEqual<byte>(4, balls.count);
			Assert.AreEqual<byte>(7, balls.size);
			((IBaseStatPerkAware)balls).GetBasePerkStats(() => fakePerkData);
			fakePerkData.NewBallsSizeDelta = 1;
			//note: all fake perk data previously set is still set. 
			balls.removeAllBalls();
			balls.growBalls(4, 5);
			Assert.AreEqual<byte>(4, balls.count);
			Assert.AreEqual<byte>(6, balls.size);
			fakePerkData.NewBallsSizeDelta = 0;
			balls.removeAllBalls();
			balls.growBalls(4, 5);
			Assert.AreEqual<byte>(4, balls.count);
			Assert.AreEqual<byte>(5, balls.size);

			IGrowable growable = balls;
			IShrinkable shrinkable = balls;
			Assert.IsTrue(growable.CanGroPlus());
			float delta = growable.UseGroPlus();
			Assert.IsTrue(delta > 0);
			Assert.AreEqual(((byte)delta).add(5), balls.size);

			balls.EnlargeBalls(byte.MaxValue, true);
			Assert.AreEqual(Balls.MAX_BALLS_SIZE, balls.size);
			Assert.IsFalse(growable.CanGroPlus());
			Assert.AreEqual(0f, growable.UseGroPlus());

			Assert.IsTrue(shrinkable.CanReducto());
			delta = shrinkable.UseReducto();
			Assert.AreEqual(Balls.MAX_BALLS_SIZE.subtract((byte)delta), balls.size);

			balls.ShrinkBalls(byte.MaxValue, true);
			Assert.IsTrue(balls.hasBalls);
			Assert.AreEqual(Balls.MIN_BALLS_SIZE, balls.size);
			Assert.IsFalse(shrinkable.CanReducto());
			delta = shrinkable.UseReducto();
			Assert.AreEqual(0f, delta);
		}

		//Breasts are technically equal parts integration and unit due to nipples.
		//we're going to skip them until nipples are tested. 

		//Same with build, but with butt and hips. 
		[TestMethod]
		public void Butt_FullUnitTests()
		{
			Butt butt = Butt.GenerateDefault();
			Assert.AreEqual(Butt.AVERAGE, butt.size);
			Assert.IsTrue(butt.hasButt);
			butt.GrowButt(byte.MaxValue);
			Assert.AreEqual(Butt.INCONCEIVABLY_BIG, butt.size);
			Assert.AreEqual<byte>(Butt.INCONCEIVABLY_BIG - Butt.TIGHT, butt.ShrinkButt(byte.MaxValue));
			butt = Butt.GenerateButtless();
			Assert.IsFalse(butt.hasButt);
			butt.GrowButt(byte.MaxValue);
			Assert.AreEqual(Butt.BUTTLESS, butt.size);
			butt.ShrinkButt(byte.MaxValue);
			Assert.AreEqual(Butt.BUTTLESS, butt.size);
			butt = Butt.Generate(Butt.NOTICEABLE);
			Assert.AreEqual(Butt.NOTICEABLE, butt.size);
			Assert.IsTrue(butt.hasButt);
			butt.GrowButt(4);
			Assert.AreEqual(Butt.JIGGLY, butt.size);
			Assert.AreEqual<byte>(5, butt.ShrinkButt(5));
			Assert.AreEqual<byte>(5, butt.size);
			butt.SetButtSize(Butt.LARGE);
			Assert.AreEqual(Butt.LARGE, butt.size);
			butt = Butt.Generate(Butt.BUTTLESS);
			Assert.IsFalse(butt.hasButt);
			butt.GrowButt(4);
			Assert.AreNotEqual(Butt.AVERAGE, butt.size);
			Assert.AreNotEqual<byte>(3, butt.ShrinkButt(3));
			Assert.AreNotEqual<byte>(1, butt.size);
			butt.SetButtSize(Butt.LARGE);
			Assert.AreNotEqual(Butt.LARGE, butt.size);

			butt = Butt.Generate(Butt.HUGE);
			Assert.IsNull((object)butt as IGrowable);
			IShrinkable shrinkable = butt;
			byte oldSize = butt.size;
			Assert.IsTrue(shrinkable.CanReducto());
			float delta = shrinkable.UseReducto();
			Assert.IsTrue(delta > 0);
			Assert.AreEqual((byte)Math.Round(oldSize * 2.0 / 3), butt.size);
			Assert.AreEqual(oldSize.subtract(butt.size), delta);
			Assert.AreEqual<byte>(11, butt.size); //16*2/3 = 10.667 or 11 after rounding.
			Assert.IsTrue(shrinkable.CanReducto());
			delta = shrinkable.UseReducto();
			Assert.AreEqual(3f, delta);
			Assert.AreEqual<byte>(8, butt.size);
			Utils.DebugSetRandomSeed(0); //reducto uses rng when <10
			delta = shrinkable.UseReducto();
			Assert.IsTrue(delta > 0);
			Assert.AreEqual(3f, delta);
			Assert.AreEqual<byte>(5, butt.size);
		}

		//note: piercings are getting their own test page. 
		[TestMethod]
		public void Clit_FullUnitTests()
		{
			SaveSystem.AddSessionSave(new BackendSessionData()); //dummy data to allow piercings to initialize properly.

			FakePerkData fakePerkData = new FakePerkData();
			Clit clit = Clit.Generate();
			Assert.AreEqual(Clit.DEFAULT_CLIT_SIZE, clit.length);
			Assert.IsFalse(clit.omnibusClit);
			Assert.IsNull(clit.AsCock());

			clit = Clit.GenerateWithLength(3f);
			Assert.AreEqual(3f, clit.length);
			Assert.IsFalse(clit.omnibusClit);
			Assert.IsNull(clit.AsCock());
			Assert.IsTrue(clit.ActivateOmnibusClit());
			Assert.IsTrue(clit.omnibusClit);
			Assert.AreEqual(3f, clit.length);
			Assert.IsNotNull(clit.AsCock());
			Assert.IsFalse(clit.ActivateOmnibusClit());
			Assert.IsTrue(clit.DeactivateOmnibusClit());
			Assert.AreEqual(1f, clit.SetClitSize(1));
			Assert.IsTrue(clit.ActivateOmnibusClit());
			Assert.AreNotEqual(1f, clit.SetClitSize(1));
			Assert.AreNotEqual(1f, clit.length);
			Assert.AreEqual(Clit.MIN_CLITCOCK_SIZE, clit.length);

			((IBaseStatPerkAware)clit).GetBasePerkStats(() => fakePerkData);
			fakePerkData.MinClitSize = 4.0f;//not enforced currently.
			clit.SetClitSize(4.0f);//so manually enforce it.
			Assert.AreEqual(clit.AsCock().cockLength, 9f);
			clit.shrinkClit(2f, true);
			Assert.AreNotEqual(2f, clit.length);
			fakePerkData.MinClitSize = 1f;//lessen it so we can see if if affects Restore().
			clit.Restore();
			Assert.IsFalse(clit.omnibusClit);
			Assert.AreEqual(1f, clit.length);
			fakePerkData.MinClitSize = 0f;//lessen it so we can see if if affects Restore().
			clit.shrinkClit(float.MaxValue, true);
			Assert.AreEqual(Clit.MIN_CLIT_SIZE, clit.length);
			clit.growClit(float.MaxValue, true);
			Assert.AreEqual(Clit.MAX_CLIT_SIZE, clit.length);

			//min new size and min delta cannot be enforced internally. they are
			//therefore tested via genitals integration. 

			fakePerkData.ClitGrowthMultiplier = 2.0f;
			fakePerkData.ClitShrinkMultiplier = 2.0f;
			Assert.AreEqual(4f, clit.shrinkClit(2, false));
			Assert.AreEqual(2f, clit.growClit(1));
			Assert.AreEqual(7f, clit.shrinkClit(7, true));
			Assert.AreEqual(8f, clit.growClit(8, true));
			fakePerkData.ClitShrinkMultiplier = 2 / 3f;
			Assert.AreEqual(2f, clit.shrinkClit(3f));
			fakePerkData.ClitGrowthMultiplier = 0.25f;
			Assert.AreEqual(1.5f, clit.growClit(6f));

			IShrinkable shrinkable = clit;
			Assert.IsTrue(shrinkable.CanReducto());
			float oldLength = clit.length;
			shrinkable.UseReducto();
			Assert.AreEqual(oldLength / 1.7f, clit.length);
			clit.SetClitSize(0);//sets it to current min.
			Assert.IsFalse(shrinkable.CanReducto());
			Assert.AreEqual(0f, shrinkable.UseReducto());

			IGrowable growable = clit;
			Assert.IsTrue(growable.CanGroPlus());
			oldLength = clit.length;
			growable.UseGroPlus();
			Assert.AreEqual(oldLength + 1, clit.length);
			clit.SetClitSize(Clit.MAX_CLIT_SIZE);//sets it to current max.
			Assert.IsFalse(growable.CanGroPlus());
			Assert.AreEqual(0f, growable.UseGroPlus());
		}


		private class FemininityListenerDummy : IFemininityListener
		{
			private bool hitFemininityData;
			private bool reactedToChange;
			public bool GotFemininity()
			{
				bool retVal = hitFemininityData;
				hitFemininityData = false;
				return retVal;
			}

			public bool Reacted()
			{
				bool retVal = reactedToChange;
				reactedToChange = false;
				return retVal;
			}

			void IFemininityAware.GetFemininityData(FemininityDataGetter getter)
			{
				hitFemininityData = true;
			}

			string IFemininityListener.reactToChangeInFemininity(byte oldFemininity)
			{
				reactedToChange = true;
				return "";
			}
		}

		[TestMethod]
		public void Femininity_FullUnitTests()
		{
			Gender currGender = Gender.GENDERLESS;
			FakePerkData fakePerkData = new FakePerkData();

			Gender genderGetter() => currGender;
			BasePerkModifiers perkGetter() => fakePerkData;

			Femininity femininity = Femininity.Generate(currGender);
			Assert.AreEqual(Femininity.GENDERLESS_DEFAULT, femininity.value);
			currGender = Gender.FEMALE;
			femininity = Femininity.Generate(currGender);
			Assert.AreEqual(Femininity.FEMALE_DEFAULT, femininity.value);
			currGender = Gender.MALE;
			femininity = Femininity.Generate(currGender);
			Assert.AreEqual(Femininity.MALE_DEFAULT, femininity.value);
			currGender = Gender.HERM;
			femininity = Femininity.Generate(currGender);
			Assert.AreEqual(Femininity.HERM_DEFAULT, femininity.value);

			//during initialization, we don't know if we have androgyny, so we need to allow
			//all valid values. we'll correct once we know about our perks.
			currGender = Gender.MALE;
			femininity = Femininity.Generate(currGender, 75);
			Assert.AreEqual(75, femininity.value);
			IBaseStatPerkAware perkAware = femininity;
			IGenderListener genderListener = femininity;
			genderListener.GetGenderData(genderGetter);
			perkAware.GetBasePerkStats(perkGetter);
			femininity.DoLateInit(perkGetter());
			Assert.AreNotEqual(75, femininity.value);

			currGender = Gender.HERM;
			femininity = Femininity.Generate(currGender, 100);
			Assert.AreEqual(100, femininity.value);
			perkAware = femininity;
			genderListener = femininity;
			genderListener.GetGenderData(genderGetter);
			perkAware.GetBasePerkStats(perkGetter);
			femininity.DoLateInit(perkGetter());
			Assert.AreNotEqual(100, femininity.value);
			Assert.AreEqual(85, femininity.value);

			currGender = Gender.HERM;
			femininity = Femininity.Generate(currGender, 100);
			Assert.AreEqual(100, femininity.value);
			perkAware = femininity;

			fakePerkData.femininityLockedByGender = false;

			genderListener = femininity;
			genderListener.GetGenderData(genderGetter);
			perkAware.GetBasePerkStats(perkGetter);
			femininity.DoLateInit(perkGetter());
			Assert.AreEqual(100, femininity.value);
			Assert.AreNotEqual(85, femininity.value);

			fakePerkData.femininityLockedByGender = true;
			femininity.SetFemininity(85);
			currGender = Gender.MALE;
			genderListener.reactToChangeInGender(Gender.HERM);
			Assert.AreEqual(70, femininity.value);

			femininity.SetFemininity(0);
			Assert.AreEqual(0, femininity.value);

			currGender = Gender.GENDERLESS;
			genderListener.reactToChangeInGender(Gender.MALE);
			Assert.AreEqual(20, femininity.value);

			currGender = Gender.FEMALE;
			genderListener.reactToChangeInGender(Gender.GENDERLESS);
			Assert.AreEqual(30, femininity.value);

			femininity.feminize(byte.MaxValue);
			Assert.AreEqual(Femininity.MOST_FEMININE, femininity.value);

			femininity.masculinize(byte.MaxValue);
			Assert.AreEqual(30, femininity.value);

			Assert.AreEqual<byte>(30, femininity.feminize(30));
			Assert.AreEqual<byte>(60, femininity.value);
			Assert.AreEqual<byte>(7, femininity.masculinize(7));
			Assert.AreEqual<byte>(53, femininity.value);

			FemininityListenerDummy dummy = new FemininityListenerDummy();

			femininity.RegisterListener(dummy);
			Assert.IsTrue(dummy.GotFemininity());
			femininity.feminize(10);
			Assert.IsTrue(dummy.Reacted());
			femininity.SetFemininity(100);
			Assert.IsTrue(dummy.Reacted());
			femininity.feminize(23);
			Assert.IsFalse(dummy.Reacted());
			femininity.DeregisterListener(dummy);
			femininity.masculinize(33);
			Assert.IsFalse(dummy.Reacted());
		}

		[TestMethod]
		public void Fertility_FullUnitTests()
		{
			FakePerkData fakePerkData = new FakePerkData();

			BasePerkModifiers perkGetter() => fakePerkData;

			Fertility fertility = Fertility.GenerateFromGender(Gender.GENDERLESS);
			Assert.AreEqual<byte>(5, fertility.baseFertility);
			Assert.AreEqual<byte>(5, fertility.totalFertility);
			Assert.AreEqual<byte>(5, fertility.currentFertility);
			Assert.IsFalse(fertility.isInfertile);
			fertility = Fertility.GenerateFromGender(Gender.FEMALE);
			Assert.AreEqual<byte>(10, fertility.baseFertility);
			Assert.AreEqual<byte>(10, fertility.totalFertility);
			Assert.AreEqual<byte>(10, fertility.currentFertility);
			fertility = Fertility.GenerateFromGender(Gender.MALE);
			Assert.AreEqual<byte>(5, fertility.baseFertility);
			Assert.AreEqual<byte>(5, fertility.totalFertility);
			Assert.AreEqual<byte>(5, fertility.currentFertility);
			fertility = Fertility.GenerateFromGender(Gender.HERM);
			Assert.AreEqual<byte>(10, fertility.baseFertility);
			Assert.AreEqual<byte>(10, fertility.totalFertility);
			Assert.AreEqual<byte>(10, fertility.currentFertility);

			fertility = Fertility.GenerateFromGender(Gender.GENDERLESS, true);
			Assert.AreEqual<byte>(0, fertility.currentFertility);
			fertility = Fertility.GenerateFromGender(Gender.FEMALE, true);
			Assert.AreEqual<byte>(10, fertility.baseFertility);
			Assert.AreEqual<byte>(0, fertility.currentFertility);
			fertility = Fertility.GenerateFromGender(Gender.MALE, true);
			Assert.AreEqual<byte>(5, fertility.baseFertility);
			Assert.AreEqual<byte>(0, fertility.currentFertility);
			fertility = Fertility.GenerateFromGender(Gender.HERM, true);
			Assert.AreEqual<byte>(10, fertility.baseFertility);
			Assert.AreEqual<byte>(0, fertility.currentFertility);
			Assert.IsTrue(fertility.isInfertile);


			//currently infertile. so we'll just test that for now.
			fertility.IncreaseFertility(); //increase by 1, ignore infertile.
			Assert.AreEqual<byte>(11, fertility.baseFertility);
			fertility.IncreaseFertility(increaseIfInfertile:false);//increase by 1, but only if fertile.
			Assert.AreEqual<byte>(11, fertility.baseFertility);
			fertility.IncreaseFertility(5, false);//increase by 5, but only if fertile.
			Assert.AreEqual<byte>(11, fertility.baseFertility);
			fertility.IncreaseFertility(5);//increase by 5
			Assert.AreEqual<byte>(16, fertility.baseFertility);

			fertility.DecreaseFertility(); //decrease by 1, ignore infertile.
			Assert.AreEqual<byte>(15, fertility.baseFertility);
			fertility.DecreaseFertility(decreaseIfInfertile: false);//decrease by 1, but only if fertile.
			Assert.AreEqual<byte>(15, fertility.baseFertility);
			fertility.DecreaseFertility(5, false);//decrease by 5, but only if fertile.
			Assert.AreEqual<byte>(15, fertility.baseFertility);
			fertility.DecreaseFertility(5);//decrease by 5
			Assert.AreEqual<byte>(10, fertility.baseFertility);

			Assert.IsTrue(fertility.isInfertile);
			Assert.IsFalse(fertility.MakeInfertile());
			Assert.IsTrue(fertility.MakeFertile());
			Assert.IsFalse(fertility.isInfertile);
			Assert.IsFalse(fertility.MakeFertile());

			IBaseStatPerkAware perkAware = fertility;
			perkAware.GetBasePerkStats(perkGetter);
			fakePerkData.bonusFertility = 23;
			Assert.AreEqual<byte>(10, fertility.baseFertility);
			Assert.AreEqual<byte>(33, fertility.totalFertility);
			Assert.AreEqual<byte>(33, fertility.currentFertility);
			fertility.MakeInfertile();
			Assert.AreEqual<byte>(33, fertility.totalFertility);
			Assert.AreEqual<byte>(0, fertility.currentFertility);
			fertility.MakeFertile();
			Assert.AreEqual<byte>(33, fertility.currentFertility);

			fertility.IncreaseFertility(byte.MaxValue, true);
			Assert.AreEqual(Fertility.MAX_BASE_FERTILITY, fertility.baseFertility);
			fertility.DecreaseFertility(byte.MaxValue, true);
			Assert.AreEqual<byte>(0, fertility.baseFertility);
		}

		[TestMethod]
		public void Hips_FullUnitTests()
		{
			Hips hips = Hips.Generate(1);
			Assert.AreEqual<byte>(1, hips.size);
			hips = Hips.Generate();
			Assert.AreEqual<byte>(4, hips.size);

			hips.SetHipSize(7);
			Assert.AreEqual<byte>(7, hips.size);

			Assert.AreNotEqual(byte.MaxValue, hips.GrowHips(byte.MaxValue));
			Assert.AreEqual(Hips.INHUMANLY_WIDE, hips.size);
			Assert.AreNotEqual(byte.MaxValue, hips.ShrinkHips(byte.MaxValue));
			Assert.AreEqual(Hips.BOYISH, hips.size);
			Assert.AreEqual<byte>(5, hips.GrowHips(5));
			Assert.AreEqual<byte>(5, hips.size);
			Assert.AreEqual<byte>(2, hips.ShrinkHips(2));
			Assert.AreEqual<byte>(3, hips.size);

			Assert.IsNull((object)hips as IGrowable);
			IShrinkable shrinkable = hips;
			Assert.IsTrue(shrinkable.CanReducto());
			Utils.DebugSetRandomSeed(13452);//majora's mask, anyone?
			float delta = shrinkable.UseReducto();
			Assert.IsTrue(delta > 0);
			Console.WriteLine(hips.size);
			Assert.IsFalse(shrinkable.CanReducto());
			delta = shrinkable.UseReducto();
			Assert.IsFalse(delta > 0);

		}

		[TestMethod]
		public void Build_FullTests()
		{
			Build build = Build.GenerateDefault();
			Assert.IsNotNull(build.hips);
			Assert.AreEqual(Hips.AVERAGE, build.hips.size);
			Assert.IsNotNull(build.butt);
			Assert.AreEqual(Butt.AVERAGE, build.butt.size);
			Assert.AreEqual(Build.DEFAULT_HEIGHT, build.heightInInches);
			Assert.AreEqual(Build.THICKNESS_NORMAL, build.thickness);
			Assert.AreEqual(Build.TONE_SOFT, build.muscleTone);

			build = Build.Generate(72);
			Assert.IsNotNull(build.hips);
			Assert.AreEqual(Hips.AVERAGE, build.hips.size);
			Assert.IsNotNull(build.butt);
			Assert.AreEqual(Butt.AVERAGE, build.butt.size);
			Assert.AreEqual<byte>(72, build.heightInInches);
			Assert.AreEqual(Build.THICKNESS_NORMAL, build.thickness);
			Assert.AreEqual(Build.TONE_SOFT, build.muscleTone);

			build = Build.Generate(Build.DEFAULT_HEIGHT, thickness:50);
			Assert.AreEqual<byte>(50, build.thickness);

			build = Build.Generate(Build.DEFAULT_HEIGHT, muscleTone: 47);
			Assert.AreEqual<byte>(47, build.muscleTone);

			build = Build.Generate(Build.DEFAULT_HEIGHT, hipSize: 13);
			Assert.AreEqual<byte>(13, build.hips.size);

			build = Build.Generate(Build.DEFAULT_HEIGHT, buttSize: 13);
			Assert.AreEqual<byte>(13, build.butt.size);

			//for now i'm letting that set buttless. it seems to make sense given
			//it's using the creator. still, if this changes, update these test cases.
			build = Build.Generate(Build.DEFAULT_HEIGHT, buttSize: 0);
			Assert.AreEqual<byte>(0, build.butt.size);
			Assert.IsFalse(build.butt.hasButt);

			build = Build.GenerateButtless(Build.DEFAULT_HEIGHT, thickness: 50);
			Assert.AreEqual<byte>(50, build.thickness);
			Assert.AreEqual<byte>(0, build.butt.size);
			Assert.IsFalse(build.butt.hasButt);

			build = Build.GenerateButtless(Build.DEFAULT_HEIGHT, muscleTone: 47);
			Assert.AreEqual<byte>(47, build.muscleTone);

			build = Build.GenerateButtless(Build.DEFAULT_HEIGHT, hipSize: 13);
			Assert.AreEqual<byte>(13, build.hips.size);

			Assert.AreEqual<byte>(5, build.GainMuscle(5));
			Assert.AreEqual<byte>(35, build.muscleTone);
			Assert.AreNotEqual(byte.MaxValue, build.LoseMuscle(byte.MaxValue));
			Assert.AreEqual<byte>(0, build.muscleTone);
			Assert.AreEqual<byte>(0, build.LoseMuscle(17));
			Assert.AreEqual<byte>(17, build.GainMuscle(17));
			Assert.AreNotEqual(byte.MaxValue, build.GainMuscle(byte.MaxValue));
			Assert.AreEqual<byte>(100, build.muscleTone);

			build.SetMuscleTone(57);
			Assert.AreEqual<byte>(57, build.muscleTone);

			Assert.AreEqual<byte>(6, build.GetThicker(6));
			Assert.AreEqual<byte>(41, build.thickness);
			Assert.AreNotEqual(byte.MaxValue, build.GetThinner(byte.MaxValue));
			Assert.AreEqual<byte>(0, build.thickness);
			Assert.AreEqual<byte>(0, build.GetThinner(31));
			Assert.AreEqual<byte>(31, build.GetThicker(31));
			Assert.AreNotEqual(byte.MaxValue, build.GetThicker(byte.MaxValue));
			Assert.AreEqual<byte>(100, build.thickness);

			build.SetThickness(43);
			Assert.AreEqual<byte>(43, build.thickness);

			Assert.AreEqual<byte>(9, build.GetTaller(9));
			Assert.AreEqual<byte>(69, build.heightInInches);
			Assert.AreNotEqual(byte.MaxValue, build.GetShorter(byte.MaxValue));
			Assert.AreEqual(Build.MIN_HEIGHT, build.heightInInches);
			Assert.AreEqual<byte>(0, build.GetShorter(47));
			Assert.AreEqual<byte>(47, build.GetTaller(47));
			Assert.AreEqual<byte>(71, build.heightInInches);
			Assert.AreNotEqual(byte.MaxValue, build.GetTaller(byte.MaxValue));
			Assert.AreEqual<byte>(Build.MAX_HEIGHT, build.heightInInches);

			build.SetHeight(43);
			Assert.AreEqual<byte>(43, build.heightInInches);


			//integration tests.
			Butt butt = Butt.GenerateButtless();
			Assert.AreEqual(butt.GrowButt(5), build.GrowButt(5));
			Assert.AreEqual(butt.ShrinkButt(5), build.ShrinkButt(5));
			Assert.AreEqual(butt.SetButtSize(7), build.SetButtSize(7));

			butt = Butt.Generate(4);
			build = Build.Generate(Build.DEFAULT_HEIGHT, buttSize: 4);

			Assert.AreEqual(butt.GrowButt(5), build.GrowButt(5));
			Assert.AreEqual(butt.ShrinkButt(5), build.ShrinkButt(5));
			Assert.AreEqual(butt.SetButtSize(7), build.SetButtSize(7));

			Hips hips = Hips.Generate(4);
			build = Build.Generate(Build.DEFAULT_HEIGHT, hipSize: 4);

			Assert.AreEqual(hips.GrowHips(5), build.GrowHips(5));
			Assert.AreEqual(hips.ShrinkHips(5), build.ShrinkHips(5));
			Assert.AreEqual(hips.SetHipSize(7), build.SetHipSize(7));
			
		}

		[TestMethod]
		public void Nipples_FullUnitTests()
		{
			Assert.Fail("Not Yet Implemented");
		}

		[TestMethod]
		public void Breasts_FullUnitTests()
		{
			Assert.Fail("Not Yet Implemented");
		}

#warning TODO: add genital tests when completed 
		//[TestMethod]
		//public void Genitals_FullUnitTests()
		//{

		//}

		[TestMethod]
		public void Womb_FullUnitTests()
		{
			Assert.Fail("Not Yet Implemented");
		}
	}
}
