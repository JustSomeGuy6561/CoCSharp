using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts.SpecialInterraction
{
	internal interface IBodyAware
	{
		void OnBodyChange(object sender, BodyAwareEventArgs e);


	}

	internal class BodyAwareEventArgs : EventArgs
	{

	}
}
