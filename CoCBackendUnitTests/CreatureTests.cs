using CoC.Backend.BodyParts;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Perks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CoCBackendUnitTests
{
	//This is a hybrid of unit and integration testing, though it's primarily a unit test.
	//For all intents and purposes, the Creature versions of internal updates simply wrap them
	//and provide a listener, but that's still an integration into creature. however, extra data
	//that the creature needs via external systems is faked with dummy objects. 

	//for all intents and purposes, DynamicNPC, when left alone, is a perfect dummy class for our unit tests. I'd fully expect it to
	//be overridden for NPCs, but if it's not, it'll still work flawlessly in the game, and in its base state, we can use it to test creature without needing a fake.
	[TestClass]
	public class CreatureUnitTests
	{
		[TestMethod]
		public void Creature_NewWithNull_ShouldThrowNullArgment()
		{
			Assert.ThrowsException<ArgumentNullException>(() => new DynamicNPC(null));
		}

		internal class PerkDummy : BasePerkModifiers
		{

		}

		//checks to see if all values are initialized without throwing an error. which happens more often than i really like to admit. 
		[TestMethod]
		public void Creature_InitializationTest()
		{
			GameEngine.constructPerkModifier = () => new PerkDummy();
			DynamicNPC npc = new DynamicNPC(new DynamicNPC_Creator("Batman"));
		}

		[TestMethod]
		public void Creature_DefaultValuesTest()
		{
			//perks modifier needs to be passed in to the backend to function properly. to cheat that i've just created a dummy here 
			//so i can run unit tests without 
			GameEngine.constructPerkModifier = () => new PerkDummy();

			DynamicNPC npc = new DynamicNPC(new DynamicNPC_Creator("Batman"));
			Assert.AreEqual(npc.antennae.type, Antennae.defaultType);
			Assert.AreEqual(npc.arms.type, Arms.defaultType);
			Assert.AreEqual(npc.back.type, Back.defaultType);
			Assert.AreEqual(npc.body.type, Body.defaultType);
			//Assert.AreEqual(npc.build.type, <>.defaultType);
			Assert.AreEqual(npc.ears.type, Ears.defaultType);
			Assert.AreEqual(npc.eyes.type, Eyes.defaultType);
			Assert.AreEqual(npc.face.type, Face.defaultType);
			//Assert.AreEqual(npc.genitals.type, <>.defaultType);
			Assert.AreEqual(npc.gills.type, Gills.defaultType);
			Assert.AreEqual(npc.hair.type, Hair.defaultType);
			Assert.AreEqual(npc.horns.type, Horns.defaultType);
			Assert.AreEqual(npc.lowerBody.type, LowerBody.defaultType);
			Assert.AreEqual(npc.neck.type, Neck.defaultType);
			Assert.AreEqual(npc.tail.type, Tail.defaultType);
			Assert.AreEqual(npc.tongue.type, Tongue.defaultType);
			Assert.AreEqual(npc.wings.type, Wings.defaultType);

		}

		//Integration test of antennae into creature.
		[TestMethod]
		public void Creature_AntennaeIntegrationTests()
		{
			bool hit = false;

			void OnAntennaeChangeEvent(object sender, BodyPartChangedEventArg<Antennae, AntennaeType> args)
			{
				//Console.WriteLine("Old: " + args.oldValue + ", New: " + args.newValue);
				hit = true;
			}
			bool checkHit()
			{
				if (hit)
				{
					hit = false;
					return true;
				}
				return false;
			}


			GameEngine.constructPerkModifier = () => new PerkDummy();
			DynamicNPC npc = new DynamicNPC(new DynamicNPC_Creator("Batman"));
			npc.SubscribeToAntennaeChanged(OnAntennaeChangeEvent);

			Assert.IsTrue(npc.UpdateAntennae(AntennaeType.BEE));
			Assert.IsTrue(checkHit());

			Assert.IsTrue(npc.RestoreAntennae());
			Assert.IsTrue(checkHit());

			npc.UnSubscribeToAntennaeChanged(OnAntennaeChangeEvent);
			Assert.IsTrue(npc.UpdateAntennae(AntennaeType.COCKATRICE));
			Assert.IsFalse(checkHit());

			npc.SubscribeToAntennaeChanged(OnAntennaeChangeEvent);
			Assert.IsTrue(npc.RestoreAntennae());
			Assert.IsTrue(checkHit());
			Assert.IsFalse(npc.UpdateAntennae(Antennae.defaultType));
			Assert.IsFalse(checkHit());
			npc.UnSubscribeToAntennaeChanged(OnAntennaeChangeEvent);

		}
	}
}