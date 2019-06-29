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

		//handle birth is always called. if the birth requires output directly to the player, set birthRequiresOutput to true.
		//after handle birth, BirthText will immediately be called and invoked, if birthRequiresOutput is true. 
		//if birthRequiresOutput is false, BirthText will not be called externally. You may still find it useful, though, 
		//as a place to store text for other calls - for example, you could have the text be used the next time the PC visits an NPC
		//by adding the birth text to an event queue or whatever equivalent tool we end up using. 

		//handle birth is also required to take care of unique things that happen, like maxing lust when birthing eggs, or growing a new anemone cock from anemone births. 
		protected internal abstract void HandleBirth(bool isVaginal);
		protected internal abstract bool birthRequiresOutput { get; }
		protected internal abstract SimpleDescriptor BirthText { get; }

		//similarly, notifyTimePassed is always called. This provides a means to do anything the pregnancy advancing causes, including (but not limited to) telling the game
		//you have flavor text to display (via the needOutputDueToTimePassed flag). Other examples may include things such as Marbe constructing a nursery during PC Marble pregnancy (for example)
		//immediately after the completion of NotifyTimePassed, NeedsOutputDueToTimePassed will be checked, and TimePassedText will be invoked and appended to the output if it's true. 

		protected internal abstract void NotifyTimePassed(bool isVaginal, ushort hoursToBirth, ushort previousHoursToBirth);
		protected internal abstract bool NeedsOutputDueToTimePassed { get; }
		protected internal abstract SimpleDescriptor TimePassedText { get; }

		//by default, will advance pregnancy to a certain point, and return the amount of time the pregnancy advanced, if any.
		//If you want to change this behavior or add additional behavior (like adding additional eggs in the case of egg pregnancy)
		//override this function. It's recommended you do still call the base if you want the advancing of pregnancy to still happen.
		protected internal virtual float HandleOviElixir(ref ushort timeToBirth, byte strength = 1)
		{
			ushort currTime = timeToBirth;
			if (timeToBirth > 20)
			{
				if (strength == 1)
				{
					timeToBirth -= (ushort)Math.Floor(timeToBirth * 0.3 + 10); //worst case this is 21- 16 = 5
				}
				else if (strength != 0)
				{
					timeToBirth -= (ushort)Math.Floor(timeToBirth * 0.5 + 15); //worst case this is 21- 16 = 5
					if (timeToBirth < 2) timeToBirth = 2;
				}
			}
			return currTime - timeToBirth;
		}

		//This is called immediately after HandleOviElixir, taking the amount it returned as a parameter. By default, it just says consuming the ovi Elixir advanced your pregnancy.
		//or if it can't do so anymore, some generic text about not doing anything. 
		//generally if you override the handle above, you should override this and explain what really happened. 
		protected internal virtual SimpleDescriptor OviElixirText(float advancedBy, byte strength) => () => DefaultOviText(advancedBy, strength);


	}
}
