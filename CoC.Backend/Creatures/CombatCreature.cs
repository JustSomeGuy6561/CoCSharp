//CombatCreature.cs
//Description:
//Author: JustSomeGuy
//2/20/2019, 4:14 PM
using CoC.Backend.Items;
using CoC.Backend.Items.Wearables.Armor;
using CoC.Backend.Items.Wearables.LowerGarment;
using CoC.Backend.Items.Wearables.UpperGarment;
using CoC.Backend.Tools;
using CoC.Backend.UI;
using System;

namespace CoC.Backend.Creatures
{

	public abstract class CombatCreature : Creature
	{


		public const byte DEFAULT_FATIGUE = 0;
		//public const byte DEFAULT_HUNGER = 0;


		internal const byte BASE_MAX_FATIGUE = 100;

		public void AddHP(uint flatAmount)
		{
			var res = flatAmount * perks.baseModifiers.HungerGainRate;
			throw new NotImplementedException();
		}

		public void AddHPPercent(double percent)
		{
			throw new NotImplementedException();
		}

		public void TakeDamage(uint amount)
		{
			throw new NotImplementedException();
		}

		//Note: Min stat is given priority for all of these - if a computed max value is less than the current minimum, the minimum is the maximum.
		//all max stats are floored (capped below) to 50, meaning they cannot drop below 50.
		//ideally i'd prefer to cap mins to the same value, but that doesn't seem to be the case in given code.

		public byte level
		{
			get;
			private protected set;
		} = 1;

		public virtual byte maxLevel => 50; //idk, could be anything really. iirc the player max is 30.

		public uint totalExperience
		{
			get => _experience;
			private protected set => _experience = Math.Min(value, maxExperience);
		}
		private uint _experience = 0;

#warning fix me!
		public uint maxExperience => ushort.MaxValue;

		public uint currentExperience => throw new Tools.InDevelopmentExceptionThatBreaksOnRelease(); //computed based on level and total experience.
		public uint experienceRequiredForNextLevel => throw new Tools.InDevelopmentExceptionThatBreaksOnRelease(); //computed based on level.

		public uint currentHealth
		{
			get => _currentHealth;
			private protected set => _currentHealth = Utils.Clamp2(value, (uint)0, maxHealth);
		}
		private uint _currentHealth = 0;


		public byte fatigue => (byte)Math.Floor(fatigueTrue);
		public double fatigueTrue
		{
			get => _fatigue;
			private protected set => _fatigue = Utils.Clamp2(value, minFatigue, maxFatigue);
		}
		private double _fatigue = 0;

		public double relativeFatigue => fatigueTrue * (100f / maxFatigue);



		protected internal virtual sbyte bonusMinFatigue { get; set; }
		protected virtual byte baseMinFatigue => 0;
		public byte minFatigue => baseMinFatigue.offset(bonusMinFatigue);

		//public byte minHunger => 0;

		public abstract uint maxHealth { get; }

		protected internal int perkBonusHealth { get; set; }

		protected internal virtual byte baseMaxFatigue => BASE_MAX_FATIGUE;
		protected internal virtual sbyte bonusMaxFatigue { get; set; } = 0;
		public byte maxFatigue => HandleMaxStat(baseMaxFatigue.offset(bonusMaxFatigue), minFatigue);

		protected internal double FatigueRegenRate = 1.0f;

		//public virtual byte maxHunger => BASE_MAX_HUNGER.offset(modifiers.bonusMaxHunger);

		//will, etc are player-specific.

		//Combat Attributes - taunt strength, bow skill, magic skill, magical aptitude, etc.
		//Equipment
		//Inventory



		//public void dynStats(sbyte str = 0, sbyte tou = 0, sbyte spd = 0, sbyte inte = 0, sbyte lib = 0, sbyte sens = 0, sbyte corr = 0, sbyte lus = 0)
		//{
		//	strength = strength.offset(str);
		//	toughness = toughness.offset(tou);
		//	speed = speed.offset(spd);
		//	intelligence = intelligence.offset(inte);
		//	libido = libido.offset(lib);
		//	sensitivity = sensitivity.offset(sens);
		//	corruption = corruption.offset(corr);
		//	lust = lust.offset(lus);
		//}

		public CombatCreature(CombatCreatureCreator creator) : base(creator)
		{
			//modifiers is valid already because perks is not null.
			//creator is not null or the base would have thrown.
			totalExperience = creator.initialXP;
			level = creator.initialLevel;

			strengthTrue = creator.strength ?? DEFAULT_STRENGTH;
			toughnessTrue = creator.toughness ?? DEFAULT_TOUGHNESS;
			speedTrue = creator.speed ?? DEFAULT_SPEED;
			intelligenceTrue = creator.intelligence ?? DEFAULT_INTELLIGENCE;

			fatigueTrue = DEFAULT_FATIGUE;
			//hunger = DEFAULT_HUNGER;

			currentHealth = maxHealth;
		}

		//public uint currentHealth
		//{
		//	get => _currentHealth;
		//	private protected set => _currentHealth = Utils.Clamp2(value, (uint)0, maxHealth);
		//}
		//private uint _currentHealth = 0;

		public double GainFatigue(double amount, bool ignorePerks = false)
		{
			var oldValue = fatigueTrue;
			fatigueTrue += amount;
			return fatigueTrue - oldValue;
		}

		public double RecoverFatigue(double amount, bool ignorePerks = false)
		{
			var oldValue = fatigueTrue;
			if (!ignorePerks)
			{
				amount *= FatigueRegenRate;
			}
			fatigueTrue -= amount;
			return oldValue - fatigueTrue;
		}
		public double ChangeFatigue(short delta, bool ignorePerks = false)
		{
			bool lose = delta < 0;

			if (delta < 0)
			{
				delta *= -1;
			}
			Utils.Clamp(ref delta, byte.MinValue, byte.MaxValue);
			byte amount = (byte)delta;
			if (lose)
			{

				return RecoverFatigue(amount, ignorePerks);
			}
			else
			{
				return GainFatigue(amount, ignorePerks);
			}
		}
		public double spellCost(double baseCost)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}


		public double physicalCost(double baseCost)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		public bool hasEnoughStamina(double baseCost, bool isPhysical)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		//Combat variations. Might want to be internal since they are only used by the combat system, idk.

		public DisplayBase UseItemDuringCombatManual(CapacityItem item, CombatCreature opponent, UseItemCombatCallback onUseItemReturn)
		{
			if (item is null)
			{
				throw new ArgumentNullException(nameof(item));
			}

			if (onUseItemReturn is null)
			{
				throw new ArgumentNullException(nameof(onUseItemReturn));
			}

			if (item.CanUse(this, true, out string whyNot))
			{
				return item.UseItemInCombat(this, opponent, onUseItemReturn);
			}
			else
			{
				onUseItemReturn(false, false, whyNot, item.Author(), item);
				return null;
			}
		}

		public DisplayBase UseItemInInventoryDuringCombatManual(byte index, CombatCreature opponent, UseItemCombatCallback onUseItemReturn)
		{
			if (onUseItemReturn is null)
			{
				throw new ArgumentNullException(nameof(onUseItemReturn));
			}

			if (index >= inventory.Count)
			{
				throw new IndexOutOfRangeException("inventory does not have that many slots currently.");
			}

			if (inventory[index].isEmpty)
			{
				onUseItemReturn(false, false, NoItemInSlotErrorText(), string.Empty, null);
				return null;
			}
			else if (!inventory[index].item.CanUse(this, true, out string whyNot))
			{
				onUseItemReturn(false, false, whyNot, inventory[index].item.Author(), null);
				return null;
			}
			else
			{
				CapacityItem item = inventoryStore.RemoveItem(index);
				return item.UseItemInCombat(this, opponent, onUseItemReturn);
			}
		}

		public DisplayBase EquipArmorDuringCombatManual(ArmorBase armor, CombatCreature opponent, UseItemCombatCallbackSafe<ArmorBase> postEquipCallback)
		{
			if (ArmorBase.IsNullOrNothing(armor))
			{
				postEquipCallback(false, false, YouGaveMeANull(), null, null);
				return null;
			}
			else if (!armor.CanUse(this, true, out string whyNot))
			{
				postEquipCallback(false, false, whyNot, armor.Author(), armor);
				return null;
			}
			else
			{
				return armor.UseItemInCombatSafe(this, opponent, postEquipCallback);
			}
		}

		public DisplayBase EquipArmorFromInventoryDuringCombatManual(byte index, CombatCreature opponent, UseItemCombatCallbackSafe<ArmorBase> postEquipCallback)
		{
			if (inventory[index].isEmpty)
			{
				postEquipCallback(false, false, NoItemInSlotErrorText(), string.Empty, null);
				return null;
			}
			else if (!(inventory[index].item is ArmorBase armorItem))
			{
				postEquipCallback(false, false, InCorrectTypeErrorText(typeof(ArmorBase)), string.Empty, null);
				return null;
			}
			else if (!armorItem.CanUse(this, true, out string whyNot))
			{
				postEquipCallback(false, false, whyNot, armorItem.Author(), null);
				return null;
			}
			else
			{
				ArmorBase item = (ArmorBase)inventoryStore.RemoveItem(index);
				return item.UseItemInCombatSafe(this, opponent, postEquipCallback);
			}
		}

		//remove the current armor, and replace it with the current one. since it may be possible for an armor equip to use a menu, the removed armor is sent along to the callback.
		//you will need to manually parse the display returned by this. if the removed armor destroys itself when being removed, the corresponding value passed along to the
		//callback will be null.
		public DisplayBase ReplaceArmorDuringCombatManual(ArmorBase armor, CombatCreature opponent, UseItemCombatCallbackSafe<ArmorBase> postReplaceArmorCallback)
		{
			if (ArmorBase.IsNullOrNothing(armor))
			{
				ArmorBase item = RemoveArmorManual(out string removeText);
				postReplaceArmorCallback(true, false, removeText, item.Author(), item);
				return null;
			}
			else if (!armor.CanUse(this, true, out string whyNot))
			{
				postReplaceArmorCallback(false, false, whyNot, armor.Author(), armor);
				return null;
			}
			else
			{
				return armor.UseItemInCombatSafe(this, opponent, postReplaceArmorCallback);
			}
		}

		public DisplayBase ReplaceArmorFromInventoryDuringCombatManual(byte index, CombatCreature opponent, UseItemCombatCallbackSafe<ArmorBase> postReplaceArmorCallback)
		{
			if (inventory[index].isEmpty)
			{
				postReplaceArmorCallback(false, false, NoItemInSlotErrorText(), string.Empty, null);
				return null;
			}
			else if (!(inventory[index].item is ArmorBase armorItem))
			{
				postReplaceArmorCallback(false, false, InCorrectTypeErrorText(typeof(ArmorBase)), string.Empty, null);
				return null;
			}
			else if (!armorItem.CanUse(this, true, out string whyNot))
			{
				postReplaceArmorCallback(false, false, whyNot, armorItem.Author(), null);
				return null;
			}
			else
			{
				ArmorBase item = (ArmorBase)inventoryStore.RemoveItem(index);
				return item.UseItemInCombatSafe(this, opponent, postReplaceArmorCallback);
			}
		}

		public DisplayBase EquipUpperGarmentDuringCombatManual(UpperGarmentBase upperGarment, CombatCreature opponent, UseItemCombatCallbackSafe<UpperGarmentBase> postEquipCallback)
		{
			if (UpperGarmentBase.IsNullOrNothing(upperGarment))
			{
				postEquipCallback(false, false, YouGaveMeANull(), null, null);
				return null;
			}
			else if (!upperGarment.CanUse(this, true, out string whyNot))
			{
				postEquipCallback(false, false, whyNot, upperGarment.Author(), upperGarment);
				return null;
			}
			else
			{
				return upperGarment.UseItemInCombatSafe(this, opponent, postEquipCallback);
			}
		}

		public DisplayBase EquipUpperGarmentFromInventoryDuringCombatManual(byte index, CombatCreature opponent, UseItemCombatCallbackSafe<UpperGarmentBase> postEquipCallback)
		{
			if (inventory[index].isEmpty)
			{
				postEquipCallback(false, false, NoItemInSlotErrorText(), string.Empty, null);
				return null;
			}
			else if (!(inventory[index].item is UpperGarmentBase upperGarmentItem))
			{
				postEquipCallback(false, false, InCorrectTypeErrorText(typeof(UpperGarmentBase)), string.Empty, null);
				return null;
			}
			else if (!upperGarmentItem.CanUse(this, true, out string whyNot))
			{
				postEquipCallback(false, false, whyNot, upperGarmentItem.Author(), null);
				return null;
			}
			else
			{
				UpperGarmentBase item = (UpperGarmentBase)inventoryStore.RemoveItem(index);
				return item.UseItemInCombatSafe(this, opponent, postEquipCallback);
			}
		}

		public DisplayBase ReplaceUpperGarmentDuringCombatManual(UpperGarmentBase upperGarment, CombatCreature opponent, UseItemCombatCallbackSafe<UpperGarmentBase> postReplaceUpperGarmentCallback)
		{
			if (UpperGarmentBase.IsNullOrNothing(upperGarment))
			{
				UpperGarmentBase item = RemoveUpperGarmentManual(out string removeText);
				postReplaceUpperGarmentCallback(true, false, removeText, item.Author(), item);
				return null;
			}
			else if (!upperGarment.CanUse(this, true, out string whyNot))
			{
				postReplaceUpperGarmentCallback(false, false, whyNot, upperGarment.Author(), upperGarment);
				return null;
			}
			else
			{
				return upperGarment.UseItemInCombatSafe(this, opponent, postReplaceUpperGarmentCallback);
			}
		}

		public DisplayBase ReplaceUpperGarmentFromInventoryDuringCombatManual(byte index, CombatCreature opponent, UseItemCombatCallbackSafe<UpperGarmentBase> postReplaceUpperGarmentCallback)
		{
			if (inventory[index].isEmpty)
			{
				postReplaceUpperGarmentCallback(false, false, NoItemInSlotErrorText(), "", null);
				return null;
			}
			else if (!(inventory[index].item is UpperGarmentBase upperGarmentItem))
			{
				postReplaceUpperGarmentCallback(false, false, InCorrectTypeErrorText(typeof(UpperGarmentBase)), "", null);
				return null;
			}
			else if (!upperGarmentItem.CanUse(this, true, out string whyNot))
			{
				postReplaceUpperGarmentCallback(false, false, whyNot, upperGarmentItem.Author(), null);
				return null;
			}
			else
			{
				UpperGarmentBase item = (UpperGarmentBase)inventoryStore.RemoveItem(index);
				return item.UseItemInCombatSafe(this, opponent, postReplaceUpperGarmentCallback);
			}
		}

		public DisplayBase EquipLowerGarmentDuringCombatManual(LowerGarmentBase lowerGarment, CombatCreature opponent, UseItemCombatCallbackSafe<LowerGarmentBase> postEquipCallback)
		{
			if (LowerGarmentBase.IsNullOrNothing(lowerGarment))
			{
				postEquipCallback(false, false, YouGaveMeANull(), null, null);
				return null;
			}
			else if (!lowerGarment.CanUse(this, true, out string whyNot))
			{
				postEquipCallback(false, false, whyNot, lowerGarment.Author(), lowerGarment);
				return null;
			}
			else
			{
				return lowerGarment.UseItemInCombatSafe(this, opponent, postEquipCallback);
			}
		}

		public DisplayBase EquipLowerGarmentFromInventoryDuringCombatManual(byte index, CombatCreature opponent, UseItemCombatCallbackSafe<LowerGarmentBase> postEquipCallback)
		{
			if (inventory[index].isEmpty)
			{
				postEquipCallback(false, false, NoItemInSlotErrorText(), "", null);
				return null;
			}
			else if (!(inventory[index].item is LowerGarmentBase lowerGarmentItem))
			{
				postEquipCallback(false, false, InCorrectTypeErrorText(typeof(LowerGarmentBase)), "", null);
				return null;
			}
			else if (!lowerGarmentItem.CanUse(this, true, out string whyNot))
			{
				postEquipCallback(false, false, whyNot, lowerGarmentItem.Author(), null);
				return null;
			}
			else
			{
				LowerGarmentBase item = (LowerGarmentBase)inventoryStore.RemoveItem(index);
				return item.UseItemInCombatSafe(this, opponent, postEquipCallback);
			}
		}

		public DisplayBase ReplaceLowerGarmentDuringCombatManual(LowerGarmentBase lowerGarment, CombatCreature opponent, UseItemCombatCallbackSafe<LowerGarmentBase> postReplaceLowerGarmentCallback)
		{
			if (LowerGarmentBase.IsNullOrNothing(lowerGarment))
			{
				LowerGarmentBase item = RemoveLowerGarmentManual(out string removeText);
				postReplaceLowerGarmentCallback(true, false, removeText, "", item);
				return null;
			}
			else if (!lowerGarment.CanUse(this, true, out string whyNot))
			{
				postReplaceLowerGarmentCallback(false, false, whyNot, lowerGarment.Author(), lowerGarment);
				return null;
			}
			else
			{
				return lowerGarment.UseItemInCombatSafe(this, opponent, postReplaceLowerGarmentCallback);
			}
		}

		public DisplayBase ReplaceLowerGarmentFromInventoryDuringCombatManual(byte index, CombatCreature opponent, UseItemCombatCallbackSafe<LowerGarmentBase> postReplaceLowerGarmentCallback)
		{
			if (inventory[index].isEmpty)
			{
				postReplaceLowerGarmentCallback(false, false, NoItemInSlotErrorText(), "", null);
				return null;
			}
			else if (!(inventory[index].item is LowerGarmentBase lowerGarmentItem))
			{
				postReplaceLowerGarmentCallback(false, false, InCorrectTypeErrorText(typeof(LowerGarmentBase)), "", null);
				return null;
			}
			else if (!lowerGarmentItem.CanUse(this, true, out string whyNot))
			{
				postReplaceLowerGarmentCallback(false, false, whyNot, lowerGarmentItem.Author(), null);
				return null;
			}
			else
			{
				LowerGarmentBase item = (LowerGarmentBase)inventoryStore.RemoveItem(index);
				return item.UseItemInCombatSafe(this, opponent, postReplaceLowerGarmentCallback);
			}
		}


		//internal CombatCreature(SurrogateCombatCreator surrogateCreator) : base(surrogateCreator)
		//{

		//}
	}
}
