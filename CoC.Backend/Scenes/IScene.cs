using CoC.Backend.Engine;
using CoC.Backend.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Scenes
{
	public interface IScene
	{
		//framework version, compatible with any potential GUI output means. in the frontend, we know how our gui will work, so this will be aliased away. 
		//don't worry about it all that much. 

		/// <summary>
		/// Runs the current scene, and returns a means to display the current scene.
		/// </summary>
		/// <returns>a page that displays the scene. </returns>
		DisplayBase RunSceneGetDisplay();
	}
}
