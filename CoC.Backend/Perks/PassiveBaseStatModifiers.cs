//PassiveBaseStatModifiers.cs
//Description:
//Author: JustSomeGuy
//6/30/2019, 7:45 PM
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Perks
{
	//These stats are used in the backend to passively (read: silently) modify base stats or body parts. These alter the behavior of certain things without notifying the user. 
	//variables that would cause certain actions to fail (for example, a function increasing or decreasing wetness/looseness would fail as it would go beyond what a perk set for the min/max looseness/wetness)
	//are not included in here. 

	//In the redesign, there was a point where i was deciding if i should fix the current structure - that is, there's no way to "cancel" a change. Some perks exist that force the player
	//to always have a certain attribute or stat, like Bimbo/Bro/Futa perks, Basilisk Womb, Fera Perks, Mareth Perks, etc. It's possible to intercept these changes via an event system, 
	//where the new result is calculated, but not implemented. the events can then alter this result or cancel it entirely.
	//For example, Consider the interaction between a Large Pink Egg and a PC with the Bro Body perk. Consuming the egg would remove all cocks, but the Bro perk means you "always" have a cock.
	//Currently, the cock gets removed, but after some time passes, you regrow it, complete with Bro Body flavor text. It'd be possible to prevent the pink egg from removing your last cock, if we
	//send an event that the last cock was removed to all the perks that care, and Bro Body tells it to cancel that change. 
	//I've decided against this, because while it may be the smart thing to do, it's difficult to implement if you don't know what you're doing.

	//I have, however, decided to compromise - i've made most of the backend stuff internal, and wrapped it in the creature class. these wrappers will call the source function, and if it succeeds, raise 
	//an event saying that particular body part or whatever has updated. other classes, notably perks (though NPCs such as Exgartuan may also find it useful to do so), may then subscribe to these events
	//and react accordingly. If these perks or whatever require something to change, they may generate a "Reaction." A Reaction is like a Time Event Listener, but it only runs once. The next chance it gets,
	//the Reaction will activate, change the player's stats or whatever it needs to, then optionally notify the player why it reacted. Reactions can optionally wait a few hours, or occur as soon as possible. 
	//You can, of course, simply ignore this, and just do what you normally do, and add checks to the time events, but this is cleaner.

	//A few non-perk examples:
	
	//For example, a Weapon may subscribe to the Strength attribute when equipped. If the player drops below a certain strength level, the Weapon generates a Reaction that uneqipts the weapon and returns it
	//to your inventory, and unsubscribes to the strength attribute. It also notifies the player that they're too weak to lift it with some unique flavor text.

	//Alternatively, Exgartuan may subscribe to the player's equipment when he possesses the player. When the player equips armor, he grants the "bulge armor" perk (granting +5 to tease skill)
	//when the player removes armor, he removes that perk. When Exgartuan stops possessing the player, he unsubscribes. 

	//As for a perk example:
	//Bro/Bimbo/Futa Body and Brains may subscribe to the player's genitals upon activation. if the player loses their junk (cock, vagina/breasts, and both, respectively), it generates a Reaction
	//that will restore these to their defaults, and explain that it was due to ingesting Bro Brew/Bimbo Liquor

	//WARNING: Beware of runaway changes/self referencing/infinite loops. It's generally bad practice to change something that just notified you it changed - that'll cause it to notify you it changed AGAIN
	//(because it did change again). If you're not careful when doing this, it'll cause an infinite loop. So, in the above example of Bro/Bimbo/Futa perks, immediately adding back the missing junk would cause
	//it to notify you it changed again. In this case, we need to do it, so we need to make sure the _second_ time it notifies us we just ignore it. 
	
	//Even worse is a contradiction - imagine you had A perk that forced you to be female, and thus removed all cocks, while at the same time having Bro Body, which forces you to have a cock. These two would
	//constantly interact, hanging the game. The first would remove a cock, causing the GenitalsChanged to fire, which would proc the Bro Body perk, which would add the cock back, again causing the GenitalsChanged
	//to fire, which would proc the FemaleOnly perk to proc, which would remove the cock, and so on. 

	//TL;DR: a subscription system exists, letting you know when a certain part or item changes. if your perk or whatever needs to check that all the time, consider subscribing and letting it tell you it changed.
	//But be careful you aren't causing infinite loops, either by contradicting something else, or by creating a situation where your changes cause you to get notified again (which causes you to change them again, etc)
	//If this scares you or is beyond your current programming level, feel free to do it the old way. 

	public sealed class PassiveBaseStatModifiers
	{
		//The game uses the following variables to 

		//unless otherwise noted, these are added to the base stats. If you fuck up and the min > max, behavior is undefined. 
		//note that regardless of bonus values here, some stats may be capped at an absolute bonus level. Note that all of these could change over development (particularly maxes, notably lust)

		//new way of dealing with initial endowments - they are permanent. so if you pick smart, you get +5 int and your min intelligence is now 5.

		public ushort bonusMaxHP;
		
		public byte minStrength;
		public sbyte bonusMaxStrength;

		public byte minSpeed;
		public sbyte bonusMaxSpeed;

		public byte minIntelligence;
		public sbyte bonusMaxIntelligence;

		public byte minToughness;
		public sbyte bonusMaxToughness;

		public byte minSensitivity;
		public sbyte bonusMaxSensitivity;

		public byte minLust;
		public sbyte bonusMaxLust;

		public byte minLibido;
		public sbyte bonusMaxLibido;

		public byte minCorruption;
		public sbyte bonusMaxCorruption;

		public sbyte bonusMaxFatigue;

		public sbyte bonusMaxHunger;
		//bonus pregnancy speed stacked additively. We're going to do the same, but we're going to be up-front about it. 
		//a perk can increase or decrease pregnancy speed, and we now support half gains. Max speed is 63.5, min speed -64.
		//behind the scenes, this is stored as a sbyte, not a float, so we can avoid floating point, and we just double the amount added. 
		//so 1/2 = 1, 1 = 2. the actual value in float is calculated via the property. 
		//Note that if it's possible to have a negative bonus. negative bonuses lengthen the pregnancy time, instead of shortening it, but do so additively. 
		//negative are equivalent in relative speed as their positive counterpart. so a bonus of one takes half time, a bonus of -1 takes double time, and so on. 

		public void incPregSpeedByOne()
		{
			bonusPregnancySpeedCounter = bonusPregnancySpeedCounter.add(2);
		}
		public void incPregSpeedByHalf()
		{
			bonusPregnancySpeedCounter = bonusPregnancySpeedCounter.add(1);
		}

		public void decPregSpeedByOne()
		{
			bonusPregnancySpeedCounter = bonusPregnancySpeedCounter.subtract(2);
		}
		public void decPregSpeedByHalf()
		{
			bonusPregnancySpeedCounter = bonusPregnancySpeedCounter.subtract(1);
		}
		private sbyte bonusPregnancySpeedCounter = 0;
		public float bonusPregnancySpeed => bonusPregnancySpeedCounter / 2.0f;
		//below is the actual formula. 
		internal float pregnancyMultiplier => bonusPregnancySpeed >= 0 ? bonusPregnancySpeed + 1 : -1.0f / (bonusPregnancySpeed - 1); 
		//0: no change;  0.5: 1.5x faster;  1: 2x faster... etc
		//0: no change; -0.5: 1.5x slower; -1: 2x slower... etc

		//Default size is only used if the size is not provided. 

		public float NewCockSizeDelta; //how much do we add or remove for new cocks? //big cock perk for now. would allow a small cock perk as well
		public float CockGrowthMultiplier; //how much more/less should we grow a cock over the base amount? //big cock perk, cockSock;
		public float CockShrinkMultiplier; //how much more/less should we shrink a cock over base amount? //big cock, cockSock;
		public float NewCockDefaultSize; //minimum size for any new cocks; //bro/futa perks for now

		public float NewClitSizeDelta; //how much do we add or remove to base amount for new Clits? //NYI, but BigClit Perks
		public float ClitGrowthMultiplier; //how much more/less should we grow a Clit over the base amount?
		public float ClitShrinkMultiplier; //how much more/less should we shrink a Clit over base amount?
		public float MinNewClitSize; //minimum size for any new Clits; //bro/futa perks for now

		public byte NewBreastCupSizeDelta; //how much do we add or remove to base amount for new Breast Rows?// BigTits Perks
		public float TitsGrowthMultiplier; //how much more/less should we grow the breasts over the base amount?
		public float TitsShrinkMultiplier; //how much more/less should we shrink the breasts over base amount?
		public CupSize NewBreastDefaultCupSize; //minimum size for any new row of breasts; //bro/futa perks for now

		public float NewNippleSizeDelta; //how much do we add or remove to base amount for new Nipples? //NYI, but BigNipple Perks
		public float NippleGrowthMultiplier; //how much more/less should we grow a Nipple over the base amount?
		public float NippleShrinkMultiplier; //how much more/less should we shrink a Nipple over base amount?
		public float NewNippleDefaultLength; //minimum size for any new Nipples; //bro/futa perks for now

		public byte NewBallsSizeDelta; //how much do we add or remove to base amount for new Balls? //note, will only go to max size for uniball if uniball. 
		public float BallsGrowthMultiplier; //how much more/less should we grow the Balls over the base amount? 1-3, expecting roughly 1.5
		public float BallsShrinkMultiplier; //how much more/less should we shrink the Balls over base amount? 1-3, expecting roughly 1.5
		public byte NewBallsDefaultSize; //note: will only go to uniball max if uniball.

		public bool AlwaysProducesMaxCum; //pilgrim perk
		public float BonusCumStacked = 1; //muliplicative 
		public uint BonusCumAdded = 0; //additive. 

		public VaginalWetness NewVaginaDefaultWetness;
		public VaginalLooseness NewVaginaDefaultLooseness;

		public bool femininityLockedByGender = true;

		public byte bonusFertility;

		public ushort PerkBasedBonusVaginalCapacity; //vag of holding, elastic innards
		public ushort PerkBasedBonusAnalCapacity; //elastic innards

		//values that would cause issues (min cock size, vag wetness, etc) are purposely excluded. see top argument. 
		//it'd be possible to have a perk that forces this to course correct almost immediately, if that's your desire - create a reaction that fires ASAP.

		//i can't think of anything else for this.
		//a bunch of old perks can just be attributes of classes, or fire a one-off "Reaction" instead of existing (looking at you Post-Akbal submit/whatever "perks")

		//it may be possible to get a perk that prioritizes certain fur colors or skin tones, and that realistically could/should be handled here. 
		//but that's not implemented, and i don't have any idea how they'd want it to work. 


	}
}
