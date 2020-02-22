Quick refresher: Difference between a place and location
- A Location is simple in scope: the player goes there, has a single random encounter, then returns to camp. Now, encounters can end up becoming somewhat complex - you may, for example, encounter an NPC, who gives
	you a menu of options to choose from, and the encounter goes from there. Note that it's possible to have a special trigger proc that pre-empts the random scene, but for the most part, randomness is the key here.
- A Place is a much more complicated beast. When the player goes to a Place, they control how things go, and should therefore see a menu when they arrive. Just like location above, it is possible to have a special
	trigger pre-empt the player's ability to make decisions - Katherine interrupting the PC when they enter Tel Adre and meet her on patrol, for example, but for the most part, player choice is key here.

Implementation: You're given pretty much complete freedom here. try to follow the above rules, please. At the end, you have to call some form of UseHoursAndThenGoToCamp, but how you get there is flexible.
	It's even possible to complete a scene, use time, but remain in your current area and continue from some menu (this should be limited to places, though.)

	Additionally, you must add your area to the AreaManager in its constructor. Basically, you're free to create as many Locations or Places you want, but in order for the Backend to know what to do,
	we need to give it to them. But we also don't want to force the backend to keep all of our locations in memory all the time. which leads to a lot of hassle - the easiest solution is just to hard-code everything
	in the backend, but then you don't have true separation of concerns and it makes it harder to modify. from there, the next easiest solution is just to keep everything in memory. but that's not really ideal.

	I've implemented a means that has the best of both worlds - we can add or modify content as we see fit, but don't need to keep everything in memory. The downside is that we need to expressly tell the Area Manager
	what our places and locations are so it can give them to the backend. I've made this as simple as i can, though it's not perfect, I'm aware.

		For Locations:
			add "AddLocationHelper(() => new <your constructor>);" to the constructor
		For Places:
			add "AddPlaceHelper(() => new <your constructor>);" to the constructor

		These hide the horrid Type magic we have to do for this stuff. Note that Generic functions (the stuff that have the weird <T> on them) allow you to never have to deal with types unless you're the one
		implementing this nonsense (though, given Type or Reflection, i'll take this Type stuff any time).


TL;DR: Do what you want with your Areas, but try to make places choice based and locations RNG based. to make it work in-game, though, call the Add(Place|Location)Helper to the AreaManager
with your constructor as a callback.

