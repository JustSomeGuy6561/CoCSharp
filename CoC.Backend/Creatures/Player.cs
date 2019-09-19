﻿//Player.cs
//Description:
//Author: JustSomeGuy
//2/20/2019, 4:15 PM
using CoC.Backend.BodyParts;
using CoC.Backend.Engine;
using CoC.Backend.SaveData;
using CoC.Backend.Tools;
using System;

namespace CoC.Backend.Creatures
{

	public sealed class Player : CombatCreature
	{
		public const byte DEFAULT_HUNGER = 0;
		internal const byte MAX_HUNGER = 100;

		public byte hunger
		{
			get => _hunger;
			private set => _hunger = Utils.Clamp2(value, minHunger, maxHunger);
		}
		private byte _hunger = 0;

		internal float hungerGainRate = 1.0f;

		private sbyte bonusMinHunger { get; set; }
		public byte minHunger => DEFAULT_HUNGER.delta(bonusMinHunger);

		internal sbyte bonusMaxHunger { get; set; }
		public byte maxHunger => HandleMaxStat(MAX_HUNGER.delta(bonusMaxHunger), minHunger);


		public Player(PlayerCreator creator) : base(creator)
		{
			hunger = DEFAULT_HUNGER;
			//now set up all the listeners.
			//if any listeners are player specifc, and i mean really player specific, add them here.

			//then activate them. 
			//occurs AFTER the creature constructor, so we're fine.
			ActivateTimeListeners();

			//TODO: Add player specific items or whatever.
		}

		public string Appearance()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
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