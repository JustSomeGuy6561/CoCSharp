﻿//Player.cs
//Description:
//Author: JustSomeGuy
//2/20/2019, 4:15 PM
using CoC.Backend.BodyParts;
using CoC.Backend.Engine;
using CoC.Backend.Items;
using CoC.Backend.SaveData;
using CoC.Backend.Tools;
using System;

namespace CoC.Backend.Creatures
{
	/// <summary>
	/// PlayerBase is the base class for the player. due to the fact that the player needs a ton of stuff in the frontend, frankly it makes more sense to just make it abstract. I was doing
	/// crazy workarounds with extensions and extra data classes that were abstract here with a constructor passed in from the frontend, and that's just a gaudy band-aid fix.
	/// </summary>
	public abstract class PlayerBase : CombatCreature
	{
		public const byte DEFAULT_HUNGER = 0;
		internal const byte MAX_HUNGER = 100;

		public byte hunger => (byte)Math.Floor(hungerTrue);
		public double hungerTrue
		{
			get => _hunger;
			private set => _hunger = Utils.Clamp2(value, minHunger, maxHunger);
		}
		private double _hunger = 0;

		internal void ValidateHunger()
		{
			hungerTrue = hungerTrue;
		}

		internal double hungerGainRate = 1.0f;

		public byte minHunger => DEFAULT_HUNGER;

		internal sbyte bonusMaxHunger => perks.baseModifiers.maxHungerDelta.GetValue();
		public byte maxHunger => HandleMaxStat(MAX_HUNGER.offset(bonusMaxHunger), minHunger);

		public double hungerRecoveryModifier => perks.baseModifiers.hungerRecoveryMultiplier.GetValue();
		public double hungerGainModifier => perks.baseModifiers.hungerGainMultiplier.GetValue();

		public PlayerBase(PlayerCreatorBase creator) : base(creator)
		{
			hungerTrue = DEFAULT_HUNGER;
			//now set up all the listeners.
			//if any listeners are player specifc, and i mean really player specific, add them here.

			//then activate them.
			//occurs AFTER the creature constructor, so we're fine.
			UnFreezeCreature();

			//TODO: Add player specific items or whatever.
		}

		public void RefillHunger(byte sateHungerAmount)
		{
			throw new NotImplementedException();
		}

		public abstract string Appearance();



		protected override string PlaceItemInCreatureStorageText(CapacityItem item, byte slot)
		{
			return "You place the " + item.ItemName() + " in your " + Tools.Utils.NumberAsPlace(slot) + " pouch. ";
		}

		protected override string ReturnItemToCreatureStorageText(CapacityItem item, byte slot)
		{
			return "You return the " + item.ItemName() + " to your " + Tools.Utils.NumberAsPlace(slot) + " pouch. ";
		}

		protected override string ReplaceItemInCreatureStorageWithNewItemText(CapacityItem newItem, byte slot)
		{
			return "You replace the " + inventory[slot].item.ItemName() + " in your " + Tools.Utils.NumberAsPlace(slot) + " pouch with " + newItem.ItemName() + ". ";
		}

#warning NYI
		public bool hasAnyStashes => false;

		public override uint maxHealth
		{
			get
			{
				double max = 50;
				max += toughness * 2;
				max += perks.baseModifiers.perkBonusMaxHp.GetValue();
				max += GameEngine.difficulties[BackendSessionSave.data.difficulty].basePlayerHP(level);
				if (max > 9999)
				{
					return 9999;
				}
				else if (max < 50)
				{
					return 50;
				}
				return (uint)max;
			}
		}
		//ovpositor handled by tail

		protected string AntennaeAppearance()
		{
			return antennae.PlayerDescription();
		}
		protected string ArmsAppearance()
		{
			return arms.PlayerDescription();
		}
		protected string BackAppearance()
		{
			return back.PlayerDescription();
		}
		//protected string BeardAppearance()
		//{
		//return beard.PlayerDescription();
		//}
		protected string BodyAppearance()
		{
			return body.PlayerDescription();
		}

		protected string EyesAppearance()
		{
			return eyes.PlayerDescription();
		}
		protected string FaceAppearance()
		{
			return face.PlayerDescription();
		}

		protected string AllBreastsAppearance()
		{
			return genitals.allBreasts.AllBreastsPlayerDescription();
		}

		protected string AllCocksAppearance()
		{
			return genitals.allCocks.AllCocksPlayerDescription();
		}


		protected string AllVaginasAppearance()
		{
			return genitals.allVaginas.AllVaginasPlayerDescription();
		}

		protected string GillsAppearance()
		{
			return gills.PlayerDescription();
		}
		protected string HairAppearance()
		{
			return hair.PlayerDescription();
		}
		protected string HornsAppearance()
		{
			return horns.PlayerDescription();
		}
		protected string LowerBodyAppearance()
		{
			return lowerBody.PlayerDescription();
		}
		protected string NeckAppearance()
		{
			return neck.PlayerDescription();
		}

		protected string TailAppearance()
		{
			return tail.PlayerDescription();
		}
		protected string TongueAppearance()
		{
			return tongue.PlayerDescription();
		}
		protected string WingsAppearance()
		{
			return wings.PlayerDescription();
		}

		protected string AssAppearance()
		{
			return ass.PlayerString(this);
		}

		protected string BallsAppearance()
		{
			return balls.PlayerDescription();
		}
	}
}
