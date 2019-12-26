//Player.cs
//Description:
//Author: JustSomeGuy
//2/20/2019, 4:15 PM
using CoC.Backend.Engine;
using CoC.Backend.UI;
using CoC.Backend.Inventory;
using CoC.Backend.Items;
using CoC.Backend.SaveData;
using CoC.Backend.Tools;
using System;
using CoC.Backend.BodyParts;
using CoC.Backend.Perks;

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
		public float hungerTrue
		{
			get => _hunger;
			private set => _hunger = Utils.Clamp2(value, minHunger, maxHunger);
		}
		private float _hunger = 0;

		internal float hungerGainRate = 1.0f;

		private sbyte bonusMinHunger { get; set; }
		public byte minHunger => DEFAULT_HUNGER.offset(bonusMinHunger);

		internal sbyte bonusMaxHunger { get; set; }
		public byte maxHunger => HandleMaxStat(MAX_HUNGER.offset(bonusMaxHunger), minHunger);


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

		internal void refillHunger(byte sateHungerAmount)
		{
			throw new NotImplementedException();
		}

		public string Appearance()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		public string LowerBodyArmorShort(bool both = true)
		{
			if (wearingArmor && both && wearingLowerGarment)
			{
				return armor.shortName() + " and " + lowerGarment.shortName();
			}
			else if (wearingArmor)
			{
				return armor.shortName();
			}
			else if (wearingLowerGarment)
			{
				return lowerGarment.shortName();
			}
			return null;
		}

		public string LowerBodyArmorTextHelper(string armorAndLowerGarmentText, string armorText, string lowerGarmentText, string nakedText)
		{
			if (wearingArmor && wearingLowerGarment) return armorAndLowerGarmentText;
			else if (wearingArmor) return armorText;
			else if (wearingLowerGarment) return lowerGarmentText;
			else return nakedText;
		}

		public string LowerBodyArmorTextHelper(string armorText, string lowerGarmentText, string nakedText)
		{
			if (wearingArmor) return armorText;
			else if (wearingLowerGarment) return lowerGarmentText;
			else return nakedText;
		}

		public string ClothingOrNakedTextHelper(string clothingText, string nakedText)
		{
			if (wearingAnything) return clothingText;
			else return nakedText;
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

#warning NYI
		public bool hasAnyStashes => false;

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
