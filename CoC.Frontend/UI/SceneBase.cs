//SceneBase.cs
//Description:
//Author: JustSomeGuy
//Note: date follows MMDDYYYY format.
//10/14/2019 10:49:00 AM

using CoC.Backend;
using CoC.Backend.Areas;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Scenes;
using CoC.Backend.Tools;
using CoC.Backend.UI;
using CoC.Frontend.Engine;
using System;

namespace CoC.Frontend.UI
{
	public abstract class SceneBase : IScene
	{
		protected StandardDisplay currentDisplay = new StandardDisplay();

		//protected

		public void RunAndDisplayScene()
		{
			DisplayManager.LoadDisplay(((IScene)this).RunSceneGetDisplay());
		}
	
		DisplayBase IScene.RunSceneGetDisplay()
		{
			RunScene();
			return currentDisplay;
		}

		protected abstract void RunScene();
	}
}
