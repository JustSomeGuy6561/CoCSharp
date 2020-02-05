using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Engine;
using CoC.Frontend.Creatures.PlayerData;

namespace CoC.Frontend.Areas
{
	public abstract class SceneBase
	{
		protected Player player => GameEngine.currentlyControlledCharacter as Player;


	}
}
