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
		/// <param name="displaySubsequentPagesCallback">A callback that will display any subsequent pages if your scene requires multiple pages.</param>
		/// <returns>a page that displays the scene. </returns>
		DisplayBase RunSceneGetDisplay(Action<DisplayBase> displaySubsequentPagesCallback);
	}
}
