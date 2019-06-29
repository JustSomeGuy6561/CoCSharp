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

		//handle birth is still unique, so we'll leave that to be implemented later. same with the birth text, but we do know such text is required.
		protected internal override bool birthRequiresOutput => true;

		//use these to help with time output related stuff.
		protected bool timeOutputFlag = false;
		protected StringBuilder timeOutputBuilder = new StringBuilder();

		//we have a generic for NotifyTimePassed. it's recommended you override it, but you don't need to.
		protected internal override void NotifyTimePassed(bool isVaginal, ushort hoursToBirth, ushort previousHoursToBirth)
		{

		}
		protected internal override bool NeedsOutputDueToTimePassed => timeOutputFlag;
		protected internal override SimpleDescriptor TimePassedText => () => timeOutputBuilder.ToString();

		//by default the egg stuff is fine. 
	}
}
