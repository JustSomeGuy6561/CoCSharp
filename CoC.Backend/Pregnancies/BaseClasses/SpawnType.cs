//SpawnType.cs
//Description:
//Author: JustSomeGuy
//5/1/2019, 9:32 PM
using CoC.Backend.Engine.Time;
using System;

namespace CoC.Backend.Pregnancies
{

	//spawn type is abstract, and must be initiated each time something uses it to mark that they've become pregnant.
	//but this allows you to do all kinds of pregnancies, any way you'd like. I was having trouble making womb/pregnancy store player and NPC friendly, but now realize i'm dumb -
	//the SpawnType is responsible for this. It's recommended (though not required) that you have different spawn types for different mothers. As such, i've provided a PlayerPregnantSpawn
	//And NPCPregnantSpawn, which are basically identical aside from the fact that player pregnancies are more formulaic, so they have default values. 

	//It's possible to create one spawntype that works for player=>npc and npc=>player pregnancies (and even, i suppose npc=>other npc pregnancies, if we ever do that), by requiring additional
	//variables, but IMO each case seems so unique that it warrants its own class. Evidently if you find the opposite to be true and are copy-pasting between 3 classes virtually the same text,
	//feel free to do them as one class with constructor paramters to determine the differences. Also, => denotes "impregnates" if that helps. 

	public abstract partial class SpawnType
	{
		public readonly SimpleDescriptor father;
		public readonly ushort hoursToBirth;

		// will probably need father text, youngling text. but for now all i need is the father, i guess.
		protected SpawnType(SimpleDescriptor nameOfFather, ushort birthTime)
		{
			father = nameOfFather;
			hoursToBirth = birthTime;
		}

		protected float percentAlong(ushort currentTimeLeft) => 1 - (currentTimeLeft / hoursToBirth);

		//handle birth is always called. If there is text to display, return true, false otherwise. The text to display should be set in outputWrapper, and if the output requires its own page,
		//set outputOnOwnPage to true. If you return false, outputWrapper and outputOnOwnPage will be ignored. Due to the "out" parameter, you must set them to something. i'd recommend OutputWrapper.Empty
		//and "false" be the defaults, and change them to whatever you need if it actually needs to spit out text.

		protected internal abstract EventWrapper HandleBirth(bool isVaginal);

		//similarly, notifyTimePassed is always called. Generally, this will just be used to tell the game whether or not you have progress text to display, and what it is, but there may be cases where
		//you want to do additional things as the pregnancy progresses. For example, when the PC is pregnant w/ Marble's kid, Marble will attempt to build a nursery, but it depends on how much time she has
		//and how often she gets to check in with the PC. Note that this will not run every hour, to prevent edge cases where in the same waiting span, you see two progress texts. Instead, it will be "lazy"
		//and run as often as the pc is aware that time passed. So if the PC gets knocked out for 8 hours, it'll only run once, in the last hour of that 8 hour span. 

		protected internal abstract EventWrapper NotifyTimePassed(bool isVaginal, float hoursToBirth, float previousHoursToBirth);

		//by default, will advance pregnancy to a certain point, and return the amount of time the pregnancy advanced, if any.
		//If you want to change this behavior or add additional behavior (like adding additional eggs in the case of egg pregnancy)
		//override this function. It's recommended you do still call the base if you want the advancing of pregnancy to still happen.
		protected internal virtual string HandleOviElixir(ref ushort timeToBirth, byte strength = 1)
		{
			ushort currTime = timeToBirth;
			if (timeToBirth > 20 && strength == 1)
			{
				timeToBirth -= (ushort)Math.Floor(timeToBirth * 0.3 + 10); //worst case this is 21- 16 = 5
			}
			else if (timeToBirth > 20 && strength > 1)
			{
				if (timeToBirth > 34)
				{
					timeToBirth -= (ushort)Math.Floor(timeToBirth * 0.5 + 15); //has to be 30 or more to be positive, but we care more about < 2. that's at 34.
				}
				else
				{
					timeToBirth = 2;
				}
			}

			return DefaultOviText(currTime - timeToBirth, strength);
		}
	}
}
