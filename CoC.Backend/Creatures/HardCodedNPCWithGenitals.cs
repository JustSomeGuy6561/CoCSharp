using CoC.Backend.BodyParts;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Creatures
{
	//This isn't a true "Creature", but more or less a "hack." Most NPCs in game now aren't initialized as creatures - there's no point, because they'll never change or update
	//any of their body parts, aside from maybe their genitals. as such, they don't need to inherit anything. This class provides a means of using the creature's Genitals body part
	//in any NPC, without forcing me to make genitals and all that shit public. Basically, it allows you to use the desriptions for cocks/vaginas/breasts/nipples/etc. without needing the 
	//whole creature. It also allows you to easily implement changing gender on any NPC, or changing cockType, etc. Basically, allows you to not have to do the shit Katherine does in vanilla
	//actionscript. I mean, that was elegant, memory-wise - it was just an array of bits. but holy shit was it ugly to parse. This is the "standardized" alternative to that, so anyone can understand it.

	//Note that due to the variable nature of pregnancy stores, they aren't part of genitals. They are, however, public. so feel free to add them as needed. 

	//NOTE TO SELF: Consider adding womb to genitals, so we can easily attach sex and knockup. we'll make womb abstract, but use a default one unless one is provided. This would allow us to easily implement
	//satyr sexuality - give everything an anal pregnancy store, but the default womb disables it unless the impregnator has satyr sexuality. obviously the player character would not use the default womb.
	//similarly, any NPC that lays eggs could use their own womb, complete with self-impregnation with eggs whenever they do that. it'll also allow double vaginas and thus double vaginal pregnancies
	//if we ever actually implement that.
	public class HardCodedNPCWithGenitals
	{
#warning Not even remotely implemented

		public readonly Genitals genitals;
	}
}
