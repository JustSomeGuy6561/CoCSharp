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


	[TestClass]
	public class CreatureUnitTests
	{
		#warning Consider moving everything to DynamicNPC to allow simpler unit tests. DynamicNPC is kinda a dummy for Creature


		[TestMethod]
		public void Creature_NewWithNull_ShouldThrowNullArgment()
		{
			Assert.ThrowsException<ArgumentNullException>(() => new Player(null));
		}

		internal class PerkDummy : BasePerkModifiers
		{

		}

		//checks to see if all values are initialized without throwing an error. which happens more often than i really like to admit. 
		[TestMethod]
		public void Creature_InitializationTest()
		{
			GameEngine.constructPerkModifier = () => new PerkDummy();
			Player player = new Player(new PlayerCreator("Batman"));
		}

		[TestMethod]
		public void Creature_DefaultValuesTest()
		{
			//perks modifier needs to be passed in to the backend to function properly. to cheat that i've just created a dummy here 
			//so i can run unit tests without 
			GameEngine.constructPerkModifier = () => new PerkDummy();

			Player player = new Player(new PlayerCreator("Batman"));
			Assert.AreEqual(player.antennae.type, Antennae.defaultType);
			Assert.AreEqual(player.arms.type, Arms.defaultType);
			Assert.AreEqual(player.back.type, Back.defaultType);
			Assert.AreEqual(player.body.type, Body.defaultType);
			//Assert.AreEqual(player.build.type, <>.defaultType);
			Assert.AreEqual(player.ears.type, Ears.defaultType);
			Assert.AreEqual(player.eyes.type, Eyes.defaultType);
			Assert.AreEqual(player.face.type, Face.defaultType);
			//Assert.AreEqual(player.genitals.type, <>.defaultType);
			Assert.AreEqual(player.gills.type, Gills.defaultType);
			Assert.AreEqual(player.hair.type, Hair.defaultType);
			Assert.AreEqual(player.horns.type, Horns.defaultType);
			Assert.AreEqual(player.lowerBody.type, LowerBody.defaultType);
			Assert.AreEqual(player.neck.type, Neck.defaultType);
			Assert.AreEqual(player.tail.type, Tail.defaultType);
			Assert.AreEqual(player.tongue.type, Tongue.defaultType);
			Assert.AreEqual(player.wings.type, Wings.defaultType);

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
			Player player = new Player(new PlayerCreator("Batman"));
			player.SubscribeToAntennaeChanged(OnAntennaeChangeEvent);

			Assert.IsTrue(player.UpdateAntennae(AntennaeType.BEE));
			Assert.IsTrue(checkHit());

			Assert.IsTrue(player.RestoreAntennae());
			Assert.IsTrue(checkHit());

			player.UnSubscribeToAntennaeChanged(OnAntennaeChangeEvent);
			Assert.IsTrue(player.UpdateAntennae(AntennaeType.COCKATRICE));
			Assert.IsFalse(checkHit());

			player.SubscribeToAntennaeChanged(OnAntennaeChangeEvent);
			Assert.IsTrue(player.RestoreAntennae());
			Assert.IsTrue(checkHit());
			Assert.IsFalse(player.UpdateAntennae(Antennae.defaultType));
			Assert.IsFalse(checkHit());
			player.UnSubscribeToAntennaeChanged(OnAntennaeChangeEvent);

		}
	}
}