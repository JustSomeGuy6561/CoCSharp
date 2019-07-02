using CoC.Backend.BodyParts;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Engine;
using CoC.Backend.Engine.Time;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CoC.Backend.Creatures
{

	public abstract class Creature
	{

		public readonly string name;

		public readonly Antennae antennae;
		public readonly Arms arms;
		public readonly Back back;
		//public readonly Beard beard;
		public readonly Body body;
		public readonly Build build;
		public readonly Ears ears;
		public readonly Eyes eyes;
		public readonly Face face;

		public readonly Genitals genitals;
		public readonly Gills gills;
		public readonly Hair hair;
		public readonly Horns horns;
		public readonly LowerBody lowerBody;
		public readonly Neck neck;
		public readonly Tail tail;
		public readonly Tongue tongue;
		public readonly Wings wings;

		//aliases for build.
		public Butt butt => build.butt;
		public Hips hips => build.hips;

		//aliases for genitals.
		public Ass ass => genitals.ass;
		public ReadOnlyCollection<Breasts> breasts => genitals.breasts;
		public ReadOnlyCollection<Cock> cocks => genitals.cocks;
		public ReadOnlyCollection<Vagina> vaginas => genitals.vaginas;
		public Balls balls => genitals.balls;

		public ReadOnlyCollection<Clit> clits => genitals.clits;
		public ReadOnlyCollection<Nipples> nipples => genitals.nipples;

		//aliases for arms/legs
		public Hands hands => arms.hands;
		public Feet feet => lowerBody.feet;

		//NOTE: WE HAVE A DESTRUCTOR/FINALIZER. IT'S AT THE BOTTOM!
		#region Constructors
		protected Creature(CreatureCreator creator)
		{
			if (creator == null) throw new ArgumentNullException();

			name = creator.name;
			//semantically, we Should do the things other parts can depend on first, but as long as we
			//dont actually require the data in the generate functions (which we generally shouldn't - that's why we're lazy)
			//it won't matter. Anything that needs this stuff for validation 


			if (creator.bodyType == null)
			{
				body = Body.GenerateDefault();
				//if (creator != null)
				//{
				//	body.C
				//}
			}
			else if (creator.bodyType is SimpleToneBodyType simpleToneBodyType)
			{
				SkinTexture skinTexture = creator.skinTexture ?? SkinTexture.NONDESCRIPT;

				body = Body.GenerateTonedNoUnderbody(simpleToneBodyType, creator.complexion, skinTexture);
			}
			else if (creator.bodyType is CompoundToneBodyType compoundToneBodyType)
			{
				SkinTexture primary = creator.skinTexture ?? SkinTexture.NONDESCRIPT;
				SkinTexture secondary = creator.underBodySkinTexture ?? SkinTexture.NONDESCRIPT;

				body = Body.GenerateToneWithUnderbody(compoundToneBodyType, creator.complexion, creator.underTone, primary, secondary);
			}
			else if (creator.bodyType is SimpleFurBodyType simpleFur)
			{
				FurTexture primary = creator.furTexture ?? FurTexture.NONDESCRIPT;

				body = Body.GenerateFurredNoUnderbody(simpleFur, creator.furColor, primary);
			}
			else if (creator.bodyType is CompoundFurBodyType compoundFur)
			{
				FurTexture primary = creator.furTexture ?? FurTexture.NONDESCRIPT;
				FurTexture secondary = creator.underBodyFurTexture ?? FurTexture.NONDESCRIPT;

				body = Body.GenerateFurredWithUnderbody(compoundFur, creator.furColor, creator.underFurColor, primary, secondary);
			}
			else if (creator.bodyType is KitsuneBodyType)
			{
				FurTexture fur = creator.furTexture ?? FurTexture.NONDESCRIPT;
				SkinTexture skin = creator.skinTexture ?? SkinTexture.NONDESCRIPT;

				body = Body.GenerateKitsune(creator.complexion, creator.furColor, skin, fur);
			}
			else if (creator.bodyType is CockatriceBodyType)
			{
				SkinTexture scales = creator.skinTexture ?? SkinTexture.NONDESCRIPT;
				FurTexture feather = creator.furTexture ?? FurTexture.NONDESCRIPT;

				body = Body.GenerateCockatrice(creator.furColor, creator.complexion, feather, scales);
			}
			else
			{
				body = Body.GenerateDefaultOfType(creator.bodyType);
			}
			//build
			if (creator.heightInInches == 0)
			{
				creator.heightInInches = Build.DEFAULT_HEIGHT;
			}
			build = Build.Generate(creator.heightInInches, creator?.thickness, creator.tone, creator.hipSize, creator.buttSize);

			//genitals


			//antennae
			antennae = creator.antennaeType != null ? Antennae.GenerateDefaultOfType(creator.antennaeType) : Antennae.GenerateDefault();
			//arms
			arms = creator.armType != null ? Arms.GenerateDefaultOfType(creator.armType) : Arms.GenerateDefault();
			//back
			if (creator.backType == null)
			{
				back = Back.GenerateDefault();
			}
			else if (creator.backType == BackType.DRACONIC_MANE && !HairFurColors.IsNullOrEmpty(creator.backHairFur))
			{
				back = Back.GenerateDraconicMane(BackType.DRACONIC_MANE, creator.backHairFur);
			}
			else
			{
				back = Back.GenerateDefaultOfType(creator.backType);
			}
			//ears
			ears = creator.earType != null ? Ears.GenerateDefaultOfType(creator.earType) : Ears.GenerateDefault();
			//eyes
			if (creator.eyeType == null)
			{
				eyes = Eyes.GenerateDefault();
			}
			else if (creator.leftEyeColor == null && creator.rightEyeColor == null)
			{
				eyes = Eyes.GenerateDefaultOfType(creator.eyeType);
			}
			else if (creator.leftEyeColor == null || creator.rightEyeColor == null)
			{
				EyeColor eyeColor = creator.leftEyeColor ?? (EyeColor)creator.rightEyeColor;
				eyes = Eyes.GenerateWithColor(creator.eyeType, eyeColor);
			}
			else
			{
				eyes = Eyes.GenerateWithHeterochromia(creator.eyeType, (EyeColor)creator.leftEyeColor, (EyeColor)creator.rightEyeColor);
			}
			//gills
			gills = creator.gillType != null ? Gills.GenerateDefaultOfType(creator.gillType) : Gills.GenerateDefault();

			if (creator?.hairType == null)
			{
				hair = Hair.GenerateDefault();
			}
			else if (creator.hairColor == null)
			{
				if (creator.hairLength == null)
				{
					hair = Hair.GenerateDefaultOfType(creator.hairType);
				}
				else
				{
					hair = Hair.GenerateWithLength(creator.hairType, (float)creator.hairLength);
				}
			}
			else if (creator.hairHighlightColor == null)
			{
				hair = Hair.GenerateWithColor(creator.hairType, creator.hairColor, creator.hairLength);
			}
			else
			{
				hair = Hair.GenerateWithColorAndHighlight(creator.hairType, creator.hairColor, creator.hairHighlightColor, creator.hairLength);
			}
			//hair = Hair.GenerateDefault();

			//horns
			if (creator?.hornType == null)
			{
				horns = Horns.GenerateDefault();
			}
			else if (creator.hornCount != null && creator.hornSize != null)
			{
				horns = Horns.GenerateOverride(creator.hornType, (byte)creator.hornSize, (byte)creator.hornCount);
			}
			else if (creator.additionalHornTransformStrength != 0)
			{
				horns = Horns.GenerateWithStrength(creator.hornType, creator.additionalHornTransformStrength, creator.forceUniformHornGrowthOnCreate);
			}
			else
			{
				horns = Horns.GenerateDefaultOfType(creator.hornType);
			}
			//horns.ReactToChangeInFemininity(genitals.femininity);
			//tongue
			tongue = creator?.tongueType == null ? Tongue.GenerateDefault() : Tongue.GenerateDefaultOfType(creator.tongueType);
			//wings
			if (creator?.wingType == null)
			{
				wings = Wings.GenerateDefault();
			}
			else if (creator.wingType is FeatheredWings && !HairFurColors.IsNullOrEmpty(creator.wingFeatherColor))
			{
				if (creator.largeWings == null)
				{
					wings = Wings.GenerateColored((FeatheredWings)creator.wingType, creator.wingFeatherColor);
				}
				else
				{
					wings = Wings.GenerateColoredWithSize((FeatheredWings)creator.wingType, creator.wingFeatherColor, (bool)creator.largeWings);
				}
			}
			else if (creator.wingType is TonableWings && !Tones.IsNullOrEmpty(creator.primaryWingTone) && !Tones.IsNullOrEmpty(creator.secondaryWingTone))
			{
				if (creator.largeWings == null)
				{
					wings = Wings.GenerateColored((TonableWings)creator.wingType, creator.primaryWingTone, creator.secondaryWingTone);
				}
				else
				{
					wings = Wings.GenerateColoredWithSize((TonableWings)creator.wingType, creator.primaryWingTone, creator.secondaryWingTone, (bool)creator.largeWings);
				}
			}
			else if (creator.wingType is TonableWings && !Tones.IsNullOrEmpty(creator.primaryWingTone))
			{
				if (creator.largeWings == null)
				{
					wings = Wings.GenerateColored((TonableWings)creator.wingType, creator.primaryWingTone);
				}
				else
				{
					wings = Wings.GenerateColoredWithSize((TonableWings)creator.wingType, creator.primaryWingTone, (bool)creator.largeWings);
				}
			}
			else if (creator.largeWings != null)
			{
				wings = Wings.GenerateDefaultWithSize(creator.wingType, (bool)creator.largeWings);
			}
			else
			{
				wings = Wings.GenerateDefaultOfType(creator.wingType);
			}

			//body.InializePiercings(creator?.navelPiercings);
			//ears.InitializePiercings(creator?.earPiercings);
			//face.InitializePiercings(creator?.eyebrowPiercings, creator?.lipPiercings, creator?.nosePiercings);
			//tongue.InitializePiercings(creator?.tonguePiercings);

			//genitals.InitializePiercings(creator?.nipplePiercings, creator?.clitPiercings, creator?.labiaPiercings, creator?.cockPiercings);
			//tail.InitializePiercings(creator?.tailPiercings);

			SetupBindings();
		}

		//internal Creature(CreatureSaveFormat format)
		//{
		//	//pull data from format
		//	SetupBindings();
		//	//ValidateData();
		//}
		private void SetupBindings()
		{
			body.SetupBodyAware(arms);
			body.SetupBodyAware(build);
			body.SetupBodyAware(ears);
			body.SetupBodyAware(face);
			body.SetupBodyAware(lowerBody);
			body.SetupBodyAware(tail);

			build.SetupBuildAware(hair);
			genitals.RegisterFemininityListener(horns);

			hair.SetupHairAware(body);
			lowerBody.SetupLowerBodyAware(build);
		}
		#endregion


		#region Updates

		public bool UpdateAntennae(AntennaeType antennaeType)
		{
			if (antennaeType == null) throw new System.ArgumentNullException();
			return antennae.UpdateAntennae(antennaeType);
		}

		public bool RestoreAntennae()
		{
			return antennae.Restore();
		}

		public bool UpdateArms(ArmType newType)
		{
			if (newType == null) throw new System.ArgumentNullException();
			return arms.UpdateArms(newType);
		}

		public bool RestoreArms()
		{
			return arms.Restore();
		}

		public bool UpdateEyes(EyeType newType)
		{
			if (newType == null) throw new System.ArgumentNullException();
			return eyes.UpdateEyeType(newType);
		}

		public bool RestoreEyes()
		{
			return eyes.Restore();
		}

		public void ResetEyes()
		{
			eyes.Reset();
		}

		public bool UpdateTongue(TongueType newType)
		{
			if (newType == null) throw new System.ArgumentNullException();
			return tongue.UpdateTongue(newType);
		}

		public bool RestoreTongue()
		{
			return tongue.Restore();
		}

		#endregion
		#region Time Listeners

		private bool isPlayer => this is Player;

		#region "Anonymous" classes
		//C# isn't java, so we don't have anonymous classes. This is the closest we can achieve. YMMV on which is better. 
		//basically, we need to wrap all the body part events into something the game engine can handle. i'd do it all in creature, but 
		//there's the whole multipage mess to deal with. 
		private sealed class LazyWrapper : ITimeLazyListener
		{
			public readonly IBodyPartTimeLazy listener;
			private readonly bool isPlayer;

			public EventWrapper reactToTimePassing(byte hoursPassed)
			{
				return new EventWrapper(listener.reactToTimePassing(isPlayer, hoursPassed));
			}

			public LazyWrapper(bool player, IBodyPartTimeLazy lazyMember)
			{
				isPlayer = player;
				listener = lazyMember;
			}
		}

		private sealed class ActiveWrapper : ITimeActiveListener
		{
			public readonly IBodyPartTimeActive listener;
			private readonly bool isPlayer;

			public EventWrapper reactToHourPassing()
			{
				return new EventWrapper(listener.reactToHourPassing(isPlayer));
			}

			public ActiveWrapper(bool player, IBodyPartTimeActive activeMember)
			{
				isPlayer = player;
				listener = activeMember;
			}
		}

		private sealed class DailyWrapper : ITimeDailyListener
		{
			public readonly IBodyPartTimeDaily listener;
			private readonly bool isPlayer;

			public byte hourToTrigger => listener.hourToTrigger;

			public EventWrapper reactToDailyTrigger()
			{
				return new EventWrapper(listener.reactToDailyTrigger(isPlayer));
			}

			public DailyWrapper(bool player, IBodyPartTimeDaily activeMember)
			{
				isPlayer = player;
				listener = activeMember;
			}
		}

		private sealed class DayMultiWrapper : ITimeDayMultiListener
		{
			public readonly IBodyPartTimeDayMulti listener;
			private readonly bool isPlayer;

			public byte[] triggerHours => listener.triggerHours;

			public EventWrapper reactToTrigger(byte currHour)
			{
				return new EventWrapper(listener.reactToTrigger(isPlayer, currHour));
			}

			public DayMultiWrapper(bool player, IBodyPartTimeDayMulti activeMember)
			{
				isPlayer = player;
				listener = activeMember;
			}
		}
		#endregion
		//we store a reference to all the listeners, bot the ones we use and the ones the game engine uses, so when we create or destroy this class, we don't "leak" events.

		private protected bool listenersActive { get; private set; } = false;
		private readonly Dictionary<IBodyPartTimeLazy, LazyWrapper> lazyListeners = new Dictionary<IBodyPartTimeLazy, LazyWrapper>();
		private readonly Dictionary<IBodyPartTimeActive, ActiveWrapper> activeListeners = new Dictionary<IBodyPartTimeActive, ActiveWrapper>();
		private readonly Dictionary<IBodyPartTimeDaily, DailyWrapper> dailyListeners = new Dictionary<IBodyPartTimeDaily, DailyWrapper>();
		private readonly Dictionary<IBodyPartTimeDayMulti, DayMultiWrapper> dayMultiListeners = new Dictionary<IBodyPartTimeDayMulti, DayMultiWrapper>();

		#region Register/Deregister
		private protected bool AddTimeListener(IBodyPartTimeLazy listener)
		{
			if (lazyListeners.ContainsKey(listener))
			{
				return false;
			}
			else
			{
				LazyWrapper wrapper = new LazyWrapper(isPlayer, listener);
				lazyListeners.Add(listener, wrapper);
				if (listenersActive)
				{
					GameEngine.RegisterLazyListener(wrapper);
				}
				return true;
			}
		}

		private protected bool RemoveTimeListener(IBodyPartTimeLazy listener)
		{
			if (!lazyListeners.ContainsKey(listener))
			{
				return false;
			}
			else
			{
				LazyWrapper wrapper = lazyListeners[listener];
				lazyListeners.Remove(listener);
				if (listenersActive)
				{
					GameEngine.DeregisterLazyListener(wrapper);
				}
				return true;
			}
		}

		private protected bool AddTimeListener(IBodyPartTimeActive listener)
		{
			if (activeListeners.ContainsKey(listener))
			{
				return false;
			}
			else
			{
				ActiveWrapper wrapper = new ActiveWrapper(isPlayer, listener);
				activeListeners.Add(listener, wrapper);
				if (listenersActive)
				{
					GameEngine.RegisterActiveListener(wrapper);
				}
				return true;
			}
		}

		private protected bool RemoveTimeListener(IBodyPartTimeActive listener)
		{
			if (!activeListeners.ContainsKey(listener))
			{
				return false;
			}
			else
			{
				ActiveWrapper wrapper = activeListeners[listener];
				activeListeners.Remove(listener);
				if (listenersActive)
				{
					GameEngine.DeregisterActiveListener(wrapper);
				}
				return true;
			}
		}

		private protected bool AddTimeListener(IBodyPartTimeDaily listener)
		{
			if (dailyListeners.ContainsKey(listener))
			{
				return false;
			}
			else
			{
				DailyWrapper wrapper = new DailyWrapper(isPlayer, listener);
				dailyListeners.Add(listener, wrapper);
				if (listenersActive)
				{
					GameEngine.RegisterDailyListener(wrapper);
				}
				return true;
			}
		}

		private protected bool RemoveTimeListener(IBodyPartTimeDaily listener)
		{
			if (!dailyListeners.ContainsKey(listener))
			{
				return false;
			}
			else
			{
				DailyWrapper wrapper = dailyListeners[listener];
				dailyListeners.Remove(listener);
				if (listenersActive)
				{
					GameEngine.DeregisterDailyListener(wrapper);
				}
				return true;
			}
		}

		private protected bool AddTimeListener(IBodyPartTimeDayMulti listener)
		{
			if (dayMultiListeners.ContainsKey(listener))
			{
				return false;
			}
			else
			{
				DayMultiWrapper wrapper = new DayMultiWrapper(isPlayer, listener);
				dayMultiListeners.Add(listener, wrapper);
				if (listenersActive)
				{
					GameEngine.RegisterDayMultiListener(wrapper);
				}
				return true;
			}
		}

		private protected bool RemoveTimeListener(IBodyPartTimeDayMulti listener)
		{
			if (!dayMultiListeners.ContainsKey(listener))
			{
				return false;
			}
			else
			{
				DayMultiWrapper wrapper = dayMultiListeners[listener];
				dayMultiListeners.Remove(listener);
				if (listenersActive)
				{
					GameEngine.DeregisterDayMultiListener(wrapper);
				}
				return true;
			}
		}
		#endregion
		protected void ActivateTimeListeners()
		{
			if (!listenersActive)
			{
				listenersActive = true;
				foreach (var listener in lazyListeners.Values)
				{
					GameEngine.RegisterLazyListener(listener);
				}
				foreach (var listener in activeListeners.Values)
				{
					GameEngine.RegisterActiveListener(listener);
				}
				foreach (var listener in dailyListeners.Values)
				{
					GameEngine.RegisterDailyListener(listener);
				}
				foreach (var listener in dayMultiListeners.Values)
				{
					GameEngine.RegisterDayMultiListener(listener);
				}
			}
		}

		protected void DeactivateTimeListeners()
		{
			if (listenersActive)
			{
				listenersActive = false;
				foreach (var listener in lazyListeners.Values)
				{
					GameEngine.DeregisterLazyListener(listener);
				}
				foreach (var listener in activeListeners.Values)
				{
					GameEngine.DeregisterActiveListener(listener);
				}
				foreach (var listener in dailyListeners.Values)
				{
					GameEngine.DeregisterDailyListener(listener);
				}
				foreach (var listener in dayMultiListeners.Values)
				{
					GameEngine.DeregisterDayMultiListener(listener);
				}
			}
		}

		#endregion

		#region DESTRUCTOR/FINALIZER
		~Creature()
		{
			CleanupCreatureForDeletion();
		}

		internal void CleanupCreatureForDeletion()
		{
			if (listenersActive)
			{
				//remove all events if any exist. 
				DeactivateTimeListeners();

				lazyListeners.Clear();
				activeListeners.Clear();
				dailyListeners.Clear();
				dayMultiListeners.Clear();
			}
		}
		#endregion

	}
}
