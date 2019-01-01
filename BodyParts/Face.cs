//Face.cs
//Description:
//Author: JustSomeGuy
//12/27/2018, 3:04 AM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoC.Tools;

namespace CoC.BodyParts
{
	//Strictly the facial structure. it doesn't include ears or eyes or hair.
	//They're done seperately. if a tf affects all of them, just call each one.
	public class Face : BodyPartBehavior<Face>
	{
		public Facial_Feature feature
		{
			get
			{
				return _feature;
			}
			protected set
			{
				_feature = value;
				index = _feature.index;
			}
		}
		protected Facial_Feature _feature = Facial_Feature.HUMAN;

		public int currentLevel { get; protected set; }

		public override int index => throw new NotImplementedException();

		public override GenericDescription shortDescription {get; protected set;}
		public override CreatureDescription<Face> creatureDescription {get; protected set;}
		public override PlayerDescription<Face> playerDescription {get; protected set;}
		public override ChangeType<Face> transformFrom {get; protected set;}

		protected Face()
		{
			feature = Facial_Feature.HUMAN;
			currentLevel = 0;
		}

		public override string GetDescriptor()
		{
			return feature.GetDescriptor(currentLevel);
		}

		public static Face GenerateFace()
		{
			return new Face();
		}

		public static void Restore(ref Face face)
		{
			face.feature = Facial_Feature.HUMAN;
			face.currentLevel = 0;
		}

		public void Restore()
		{
			feature = Facial_Feature.HUMAN;
			currentLevel = 0;
		}

		public bool UpdateFace(Facial_Feature newFeatures, uint level = 1)
		{
			if (newFeatures == feature)
			{
				return false;
			}
			else
			{
				feature = newFeatures;
				Tools.Utils.Clamp<uint>(ref level, 0, int.MaxValue);
				currentLevel = (feature == Facial_Feature.HUMAN) ? 0 : (int)level;
				return true;
			}
		}

		public bool LessenTransform(uint byAmount = 1)
		{
			//should never procc, but is here in case somebody edited values or some shenanigans.
			if (currentLevel == 0 && feature != Facial_Feature.HUMAN)
			{
				feature = Facial_Feature.HUMAN;
				return true;
			}
			else if (byAmount == 0)
			{
				return false;
			}

			//clamps uint to range of positive integer, prevents weird errors
			Tools.Utils.Clamp<uint>(ref byAmount, 0, int.MaxValue);
			currentLevel -= (int)byAmount;
			if (currentLevel <= 0)
			{
				currentLevel = 0;
				feature = Facial_Feature.HUMAN;
			}
			return true;
		}
		public bool StrengthenTransform(uint byAmount = 1)
		{
			if (currentLevel == feature.numLevels || byAmount == 0)
			{
				return false;
			}
			else if (currentLevel == 0 && feature == Facial_Feature.HUMAN)
			{
					return false;
			}
			//Clamps uint to range of positive integer.
			Tools.Utils.Clamp<uint>(ref byAmount, 0, int.MaxValue);
			currentLevel += (int)byAmount;
			currentLevel = currentLevel > feature.numLevels ? feature.numLevels : currentLevel;
			return true;
		}

		//---------------------------------------------
		//Because of the convenience shit. Standard compares that need to be explicitly defined because
		//the non-standard ones are too.
		public bool Equals(Face other)
		{
			return this == other;
		}

		public static bool operator ==(Face first, Face second)
		{
			return first.currentLevel == second.currentLevel && first.feature == second.feature;
		}

		public static bool operator !=(Face first, Face second)
		{
			return first.currentLevel != second.currentLevel || first.feature != second.feature;
		}

		//Convenience. Because everyone loves that shit
		public bool Equals(Facial_Feature other)
		{
			return feature == other;
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public static bool operator ==(Face first, Facial_Feature second)
		{
			return first.feature == second;
		}

		public static bool operator !=(Face first, Facial_Feature second)
		{
			return first.feature != second;
		}
		//---------------------------------------------
	}

	public abstract class Facial_Feature
	{
		public bool multiLeveled
		{
			get
			{
				return numLevels > 1;
			}
		}
		public readonly int numLevels;

		public readonly int index;
		private static int indexMaker = 0;

		protected Facial_Feature(int maxLevel = 1)
		{
			//forces maxlevel to be 1 or more. because that'd break things.
			Tools.Utils.Clamp(ref maxLevel, 1, int.MaxValue);
			numLevels = maxLevel;
			index = indexMaker++;
		}

		public abstract string GetDescriptor(int level);

		public static readonly Facial_Feature HUMAN = new Generic_Face("Human Face");
		public static readonly Facial_Feature HORSE = new Generic_Face("Horse Face");
		public static readonly Facial_Feature DOG = new Generic_Face("Dog Face");
		public static readonly Facial_Feature COW_MINOTAUR = new Generic_Face("Bull Face");
		public static readonly Facial_Feature SHARK_TEETH = new Generic_Face("Shark Teeth");
		public static readonly Facial_Feature SNAKE_FANGS = new Generic_Face("Snake Fangs");
		public static readonly Facial_Feature CAT = new Cat_Face();
		public static readonly Facial_Feature LIZARD = new Generic_Face("Lizard Face");
		public static readonly Facial_Feature BUNNY = new Bunny_Face();
		public static readonly Facial_Feature KANGAROO = new Generic_Face("Kangaroo Face");
		public static readonly Facial_Feature SPIDER_FANGS = new Generic_Face("Spider Fangs");
		public static readonly Facial_Feature FOX = new Fox_Face(); //Level 1: Kitsune. Level 2: Full fox
		public static readonly Facial_Feature DRAGON = new Generic_Face("Dragon Face");
		public static readonly Facial_Feature RACCOON = new Raccoon_Face();
		public static readonly Facial_Feature MOUSE = new Mouse_Face();
		public static readonly Facial_Feature FERRET = new Ferret_Face();
		public static readonly Facial_Feature PIG = new Pig_Face(); //Level 1: Pig. Level 2: Boar
		public static readonly Facial_Feature RHINO = new Generic_Face("Rhino Face");
		public static readonly Facial_Feature ECHIDNA = new Generic_Face("Echidna Face");
		public static readonly Facial_Feature DEER = new Generic_Face("Deer Face");
		public static readonly Facial_Feature WOLF = new Generic_Face("Wolf Face"); //Maybe combine with dog?
		public static readonly Facial_Feature COCKATRICE = new Generic_Face("Cockatrice Face"); 
		public static readonly Facial_Feature BEAK = new Generic_Face("PlaceHolder Beak Face"); // This is a placeholder for the next beaked face type, so feel free to refactor (rename)
		public static readonly Facial_Feature RED_PANDA = new Generic_Face("Red Panda Face");
		private class Generic_Face : Facial_Feature
		{
			protected string descriptor;
			public Generic_Face(string desc) : base(1)
			{
				descriptor = desc;
			}

			public override string GetDescriptor(int level)
			{
				return descriptor;
			}
		}

		private class Cat_Face : Facial_Feature
		{
			protected readonly string[] descriptors = { "Cat-girl Face", "Feline Face" };
			public Cat_Face() : base(2) {}

			public override string GetDescriptor(int level)
			{
				Tools.Utils.Clamp(ref level, 1, numLevels);
				return descriptors[level-1];
			}
		}
		private class Bunny_Face : Facial_Feature
		{
			protected readonly string[] descriptors = { "Bunny-like Teeth", "Bunny Face" };

			public Bunny_Face() : base(2) { }

			public override string GetDescriptor(int level)
			{
				Tools.Utils.Clamp(ref level, 1, numLevels);
				return descriptors[level-1];
			}
		}
		private class Fox_Face : Facial_Feature
		{
			protected readonly string[] descriptors = { "Kitsune Face", "Fox Face" };
			public Fox_Face() : base(2) { }

			public override string GetDescriptor(int level)
			{
				Tools.Utils.Clamp(ref level, 1, numLevels);
				return descriptors[level - 1];
			}
		}
		private class Raccoon_Face : Facial_Feature
		{
			protected readonly string[] descriptors = { "Raccoon Mask", "Raccoon Face" };
			public Raccoon_Face() : base(2) { }

			public override string GetDescriptor(int level)
			{
				Tools.Utils.Clamp(ref level, 1, numLevels);
				return descriptors[level - 1];
			}
		}
		private class Mouse_Face : Facial_Feature
		{
			protected readonly string[] descriptors = { "Mouse-like Teeth", "Mouse Face" };
			public Mouse_Face() : base(2) { }

			public override string GetDescriptor(int level)
			{
				Tools.Utils.Clamp(ref level, 1, numLevels);
				return descriptors[level - 1];
			}
		}
		private class Ferret_Face : Facial_Feature
		{
			protected readonly string[] descriptors = { "Ferret Mask", "Ferret Face" };
			public Ferret_Face() : base(2) { }

			public override string GetDescriptor(int level)
			{
				Tools.Utils.Clamp(ref level, 1, numLevels);
				return descriptors[level - 1];
			}
		}
		private class Pig_Face : Facial_Feature
		{
			protected readonly string[] descriptors = { "Pig Face", "Boar Face" };
			public Pig_Face() : base(2) { }

			public override string GetDescriptor(int level)
			{
				Tools.Utils.Clamp(ref level, 1, numLevels);
				return descriptors[level - 1];
			}
		}
	}
}