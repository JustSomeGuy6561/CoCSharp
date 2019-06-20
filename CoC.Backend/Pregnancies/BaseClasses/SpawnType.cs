using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CoC.Backend.Pregnancies
{
	
	//originally, this was designed to be static, and the constructor hidden away. But the fact of the matter is I can't store all the variables necessary for all types of pregnancies
	//in here, and referencing it from elsewhere is just plain annoying. (For example, you could be pregnant with multiple entities, or eggs could be large, or numerous, etc)
	//This also allows you to set up things like gender of the baby (or babies) when the instance is created, for realism, i suppose. at the moment, there's no way to check baby gender
	//pre birth, but idk man, they could add that shit later. or they could go all nuts and add height/weight of birth baby, and this would still work. or not, doesn't matter to me.

	public abstract partial class SpawnType
	{
		public readonly SimpleDescriptor father;
		public readonly ushort hoursToBirth;

		//private float 

		protected SpawnType(SimpleDescriptor nameOfFather, ushort birthTime)
		{
			father = nameOfFather;
			hoursToBirth = birthTime;
		}

		//override this is the spawn has special text for pregnancy progression. 
		//The general idea is a sort of "checkpoint" for your pregnancy. for example, when the character is 25% into pregnancy, display some text about knowing you're pregnant.
		//and so on. by default, we do 25%, 50%, 75%. In order to only do each text once, the previous value for hoursTilBirth is also passed in.
		//so if you're more than 25% into the pregnancy, but you were more than 25% into the pregnancy last time, don't print out the 25% text again. 
		//It's recommended (though not required) that you only print out the most recent checkpoint. so, check for the lower values first. see below. 
		protected internal virtual bool HasPregnancyProgressText(ushort hoursTilBirth, ushort previousHoursTilBirth, out string output)
		{
			#warning Replace This with generic pregnancy progress text at 75%, 50%, and 25%.
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
			//output = "";
			//return false;
		}

		//if something special happens during birth, like it maxes your lust (eggs, for example) or grants you a new cock (anemone), you can do it here.
		protected internal abstract void HandleBirth();

		protected internal virtual void HandleOviElixir(byte strength = 1)
		{

		}

		//similarly, if the text that normally displays when consuming an ovi elixir is insufficient, you can override this with a new one.
		//generally if you override the handle above, you should override this.
		protected internal virtual SimpleDescriptor OviElixirText(byte strength) => DefaultOviText;

		protected internal abstract SimpleDescriptor BirthText { get; }
	}
}
