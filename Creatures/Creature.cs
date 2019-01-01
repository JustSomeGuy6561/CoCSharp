﻿//Creature.cs
//Description:
//Author: JustSomeGuy
//12/30/2018, 10:36 PM
using CoC.BodyParts;
using CoC.Tools;

namespace CoC
{
	public class Creature
	{
		public Antennae antennae;
		public Hair hair;
		public Gender gender;

		public Face face;

		//OutputText("You are a " + player.height.toString() + " tall " + player.gender.asText() + " " + player.race.toString() + ", with " + player.bodyTypeString() + ".");
		public GenericDescription generalDescription;

		//
		public GenericDescription describeFacialFeatures;

		public GenericDescription/*WithArg<BoobStack>*/ describeAllBoobs;
		public GenericDescription/*WithArg<CockStack>*/ describeAllCocks;
		public GenericDescription/*WithArg<CockStack>*/ describePiercings;

	}
}
