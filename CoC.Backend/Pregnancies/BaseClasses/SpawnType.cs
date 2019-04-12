using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CoC.Backend.Pregnancies
{
	
	public abstract partial class SpawnType
	{
		public readonly SimpleDescriptor father;
		public readonly int hoursToBirth;
		public readonly SimpleDescriptor birthFlavorText;

		public SpawnType(SimpleDescriptor nameOfFather, int birthTime, SimpleDescriptor birthText)
		{
			father = nameOfFather;
			hoursToBirth = birthTime;
			birthFlavorText = birthText;
		}

		//if something special happens during birth, like it maxes your lust (eggs, for example) or grants you a new cock (anemone), you can do it here.
		public virtual void HandleBirth() { }

		//if something special occurs when you take an ovi elixir (beyong just moving up a pregnancy), you can take care of it here.
		public virtual void HandleOviElixir(int strength = 1) {}

		//similarly, if the text that normally displays when consuming an ovi elixir is insufficient, you can override this with a new one.
		//generally if you override the handle above, you should override this.
		public virtual SimpleDescriptor OviElixirText => DefaultOviText;
	}
}
