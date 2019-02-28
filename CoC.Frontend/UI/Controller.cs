//Controller.cs
//Description:
//Author: JustSomeGuy
//1/19/2019, 8:01 PM
using CoC.Frontend.UI;

namespace CoC.UI
{

	//consider firing custom events instead of INotifyPropertyChanged. 
	public sealed class ModelView
	{
		public string outputField => TextOutput.data;
	}
}
