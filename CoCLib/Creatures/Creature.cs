//Creature.cs
//Description:
//Author: JustSomeGuy
//12/30/2018, 10:36 PM
using CoC.BodyParts;
using CoC.EpidermalColors;
using CoC.Tools;

namespace CoC.Creatures
{
	//base class for all creatures that deign to use it. allows NPCs to be built out of parts like mosters and the PC
	//as of now no NPCs use it. there's evidence this may not have always been the case or plans were to use it, idk.
	internal abstract class Creature
	{
		public readonly Antennae antennae;
		public readonly Arms arms;
		public readonly Back back;
		public readonly Body body;
		public readonly Ears ears;
		public readonly Face face;
		public readonly Genitals genitals;
		public readonly Gills gills;
		public readonly Horns horns;
		public readonly LowerBody lowerBody;
		public readonly Neck neck;
		public readonly Tail tail;
		public readonly Tongue tongue;
		public readonly Wings wings;

		public readonly FacialHair facialHair;

		public virtual Species race => startingRace;
		public readonly Species startingRace;

		//may want to change this into a more global flag system, idk. 
		public readonly PiercingFlags piercingFlags = PiercingFlags.Generate();

		public Gender gender => genitals.gender;
		public Hair hair => body.hair;

		//properties for piercing related parts that aren't already available.
		public Eyebrow eyebrow => face.eyebrow;
		public Lip lip => face.lip;
		public Nose nose => face.nose;

		public Hips hips => lowerBody.hips;
		public Butt butt => lowerBody.butt;
		public Ass ass => lowerBody.butt.ass;
		public readonly string name;

		public int heightInInches { get; protected set; }

		protected Creature(string creatureName, Species initialRace)
		{
			name = creatureName;
			startingRace = initialRace;
			CreatureCreator cc = creator;
			//flags that affect the remaining creatures
			Gender gender = cc?.gender ?? Gender.MALE;
			piercingFlags = cc?.piercingFlags ?? PiercingFlags.Generate();

			antennae = cc?.antennae ?? Antennae.GenerateDefault();
			arms = cc?.arms ?? Arms.GenerateDefault();
			back = cc?.back ?? Back.GenerateDefault();
			Balls balls = cc?.balls ?? Balls.GenerateDefault(gender);
			Breasts breasts = cc?.breasts ?? Breasts.GenerateDefault(piercingFlags, gender, false);
			Cock[] cocks = cc?.cocks ?? (gender.HasFlag(Gender.MALE) ? new Cock[]{ Cock.GenerateDefault(piercingFlags, false)} : new Cock[]{});
			ears = cc?.ears ?? Ears.GenerateDefault(piercingFlags);
			Eyes eyes = cc?.eyes ?? Eyes.GenerateDefault();
			hair = cc?.hair ?? Hair.GenerateDefault();
			gills = cc?.gills ?? Gills.GenerateDefault();
			horns = cc?.horns ?? Horns.GenerateDefault();
			neck = cc?.neck ?? Neck.GenerateDefault();
			tail = cc?.tail ?? Tail.GenerateDefault(piercingFlags);
			tongue = cc?.tongue ?? Tongue.GenerateDefault(piercingFlags);
			Vagina[] vaginas = cc?.vaginas ?? (gender.HasFlag(Gender.FEMALE) ? new Vagina[] { Vagina.GenerateDefault(piercingFlags) } : new Vagina[] { });
			wings = cc?.wings ?? Wings.GenerateDefault();
			facialHair = cc?.facialHair ?? FacialHair.GenerateDefault();

			//composite classes.
			face = cc?.face ?? Face.GenerateDefault(piercingFlags);
			body = cc?.body ?? Body.GenerateDefault(piercingFlags);
			lowerBody = cc?.lowerBody ?? LowerBody.GenerateDefault();
			genitals = cc?.genitals ?? Genitals.GenerateDefault(piercingFlags, gender, false, false);
		}

		public abstract CreatureCreator creator { get; }

		//internal abstract void InitInventory


		//publicputText("You are a " + player.height.toString() + " tall " + player.gender.asText() + " " + player.race.toString() + ", with " + player.bodyTypeString() + ".");
		public SimpleDescriptor generalDescription;

		//
		public SimpleDescriptor describeFacialFeatures;

		public SimpleDescriptor/*WithArg<BoobStack>*/ describeAllBoobs;
		public SimpleDescriptor/*WithArg<CockStack>*/ describeAllCocks;
		public SimpleDescriptor/*WithArg<CockStack>*/ describePiercings;

		public FurColor GetFurColor()
		{
			return body.primaryEpidermis.fur;
		}

	}

	internal abstract class CreatureCreator
	{
		public Antennae antennae;
		public Arms arms;
		public Back back;
		public Body body;
		public Ears ears;
		public Face face;
		public Genitals genitals;
		public Gills gills;
		public Horns horns;
		public LowerBody lowerBody;
		public Neck neck;
		public Tail tail;
		public Tongue tongue;
		public Wings wings;
		public FacialHair facialHair;
		public PiercingFlags piercingFlags;

		public int? height = null;
		public int? 

		//anything that needs to be done that can't be done via setting the above variables.
		//note that when this is called, it is guranteed that all body parts will be initialized.
		//there is no need to reinitialize them. primarily, this is for piercings, though perhaps you'd like to fine-tune 
		//a character post init - override default behaviors or reset updated hair colors or something. 
		public abstract void postInit();
	}
}
