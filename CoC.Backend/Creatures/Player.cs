//Player.cs
//Description:
//Author: JustSomeGuy
//2/20/2019, 4:15 PM
using CoC.Backend.Engine;
using CoC.Backend.Inventory;
using CoC.Backend.Items;
using CoC.Backend.SaveData;
using CoC.Backend.Tools;
using System;

namespace CoC.Backend.Creatures
{

	public sealed class Player : CombatCreature
	{
		public const byte DEFAULT_HUNGER = 0;
		internal const byte MAX_HUNGER = 100;

		public byte hunger => (byte)Math.Floor(hungerTrue);
		public float hungerTrue
		{
			get => _hunger;
			private set => _hunger = Utils.Clamp2(value, minHunger, maxHunger);
		}
		private float _hunger = 0;

		internal float hungerGainRate = 1.0f;

		private sbyte bonusMinHunger { get; set; }
		public byte minHunger => DEFAULT_HUNGER.delta(bonusMinHunger);

		internal sbyte bonusMaxHunger { get; set; }
		public byte maxHunger => HandleMaxStat(MAX_HUNGER.delta(bonusMaxHunger), minHunger);


		public Player(PlayerCreator creator) : base(creator)
		{
			hungerTrue = DEFAULT_HUNGER;
			//now set up all the listeners.
			//if any listeners are player specifc, and i mean really player specific, add them here.

			//then activate them. 
			//occurs AFTER the creature constructor, so we're fine.
			UnFreezeCreature();

			//TODO: Add player specific items or whatever.
		}

		internal void refillHunger(byte sateHungerAmount)
		{
			throw new NotImplementedException();
		}

		public string Appearance()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		protected override string PlaceItemInCreatureStorageText(CapacityItem item, byte slot)
		{
			return "You place the " + item.shortName() + " in your " + Tools.Utils.NumberAsPlace(slot) + " pouch. ";
		}

		protected override string ReturnItemToCreatureStorageText(CapacityItem item, byte slot)
		{
			return "You return the " + item.shortName() + " to your " + Tools.Utils.NumberAsPlace(slot) + " pouch. ";
		}

		protected override string ReplaceItemInCreatureStorageWithNewItemText(CapacityItem newItem, byte slot)
		{
			return "You replace the " + inventory[slot].item.shortName() + " in your " + Tools.Utils.NumberAsPlace(slot) + " pouch with " + newItem.shortName() + ". ";
		}

		public override uint maxHealth
		{
			get
			{
				double max = 50;
				max += toughness * 2;
				max += perks.baseModifiers.bonusMaxHP;
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
	}
}