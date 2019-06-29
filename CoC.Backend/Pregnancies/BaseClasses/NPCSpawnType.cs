using CoC.Backend.Creatures;
using CoC.Backend.Engine;

namespace CoC.Backend.Pregnancies
{
	//NPC spawntype variant. sets the father to the player. In theory, weird cases could arise if the text is called when controlling another character (see Urta Quest), but since it's a callback,
	//and we should never be making these callbacks when we aren't controlling our player, this isn't an issue. if the need arises, we could capture the string statically, then not update it, so it always 
	//says the current player. 

	public abstract partial class NPCSpawnType : SpawnType
	{
		protected NPCSpawnType(ushort birthTime) : base(() => GameEngine.currentPlayer.name, birthTime)	{}

		//literally nothing else is known, so. 
	}
}
