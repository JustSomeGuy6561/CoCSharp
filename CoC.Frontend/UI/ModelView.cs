//ModelView.cs
//Description:
//Author: JustSomeGuy
//2/27/2019, 10:15 PM

//Controller.cs
//Description:
//Author: JustSomeGuy
//1/19/2019, 8:01 PM
using CoC.Frontend.UI;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CoC.UI
{

	//consider firing custom events instead of INotifyPropertyChanged. 
	public sealed class ModelView
	{
#warning Consider moving all static functions for buttons, output, input, selector here. makes it easier to link everything in.

		public static ModelView instance;

		//public Image outputImage; 
		public string outputField => TextOutput.data;

		public ReadOnlyCollection<ButtonData> buttons => ButtonData.buttons;

		public InputField inputField => InputField.instance;
		//WILL THROW NULL REFERENCE EXCEPTION IF INPUT FIELD IS NULL. it'd do so if you accessed inputField and asked for it there, so this seems the most consistent.
		//may return null, though this is GUI dependant and hopefully wont happen. empty string is preferable.
		internal string inputText => inputField.input;

		//NYI, placeholder. will likely be its own class.
		public List<object> inputSelect;
		public int inputSelectIndex;


	}
}
