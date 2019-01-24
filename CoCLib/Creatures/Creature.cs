//Creature.cs
//Description:
//Author: JustSomeGuy
//12/30/2018, 10:36 PM
using  CoC.BodyParts;
using CoC.EpidermalColors;
using CoC.Tools;

namespace CoC.Creatures
{
	//base class for all creatures that deign to use it. allows NPCs to be built out of parts like mosters and the PC
	//as of now no NPCs use it. there's evidence this may not have always been the case or plans were to use it, idk.
	public abstract class Creature
	{
		protected readonly Tones defaultTone = Tones.LIGHT;
		protected readonly HairFurColors defaultHair = HairFurColors.BLACK;

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
		protected Creature(string creatureName)
		{
			name = creatureName;
			InitBody(out antennae, out arms, out back, out body, out ears, out face, out genitals, out gills, out horns, out lowerBody, out neck, out tail, out tongue, out wings, out facialHair, piercingFlags);

			if (piercingFlags == null)
			{
				piercingFlags = PiercingFlags.Generate();
			}
			if (antennae == null)
			{
				antennae = Antennae.Generate(AntennaeType.NONE);
			}
			if (arms == null)
			{
				arms = Arms.Generate(ArmType.HUMAN);
			}
			if (back == null)
			{
				back = Back.Generate();
			}
			if (body == null)
			{
				body = Body.GenerateHumanoid(piercingFlags, defaultTone, hair.color);
			}
			if (ears == null)
			{
				ears = Ears.Generate(piercingFlags);
			}
			if (face == null)
			{
				face = Face.Generate(piercingFlags);
			}
			if (genitals == null)
			{
				genitals = Genitals.Generate(piercingFlags, Gender.MALE, false, false);
			}
			if (gills == null)
			{
				gills = Gills.Generate();
			}
			if (horns == null)
			{
				horns = Horns.Generate();
			}
			if (lowerBody == null)
			{
				lowerBody = LowerBody.Generate(LowerBodyType.HUMAN);
			}
			if (neck == null)
			{
				neck = Neck.Generate();
			}
			if (tail == null)
			{
				tail = Tail.GenerateNoTail(piercingFlags);
			}
			if (tongue == null)
			{
				tongue = Tongue.Generate(piercingFlags);
			}
			if (wings == null)
			{
				wings = Wings.Generate();
			}
			if (hair == null)
			{
				hair = Hair.Generate();
			}
			if (facialHair == null)
			{
				facialHair = FacialHair.Generate();
			}
		}
		//only way to do readonly and still compile. ik it's weird, but with visual studio it'll auto complete and you'll be fine.
		//simple assign somethihg to whatever values you want to change from their default. 
		public abstract void InitBody(out Antennae antennae, out Arms arms, out Back back, out Body body, out Ears ears, out Face face, out Genitals genitals, out Gills gills, 
			out Horns horns, out LowerBody lowerBody, out Neck neck, out Tail tail, out Tongue tongue, out Wings wings, out FacialHair facialHair, PiercingFlags piercingFlags);

		//public abstract void InitInventory


		//OutputText("You are a " + player.height.toString() + " tall " + player.gender.asText() + " " + player.race.toString() + ", with " + player.bodyTypeString() + ".");
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
}
