using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using System.Text;

namespace CoC.Backend.Pregnancies
{

	//player variant of spawn type. the mother here is the player. some things can therefore use defaults, though it's recommended you don't. 

	public abstract partial class PlayerSpawnType : SpawnType
	{
		public Player player; 
		//private float 

		protected PlayerSpawnType(SimpleDescriptor nameOfFather, ushort birthTime) : base(nameOfFather, birthTime)
		{
			//get the player now. Prevents weird urta quest errors. 
			player = GameEngine.currentPlayer;
		}
	}
}
