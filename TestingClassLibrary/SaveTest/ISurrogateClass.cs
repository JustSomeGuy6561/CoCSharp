using System;
using System.Collections.Generic;
using System.Text;

namespace TestingClassLibrary.SaveTest
{
	public interface ISurrogateClass<T> : ISurrogateBase where T : SaveableData
	{
		T ToClass();
	}

	public interface ISurrogateBase
	{
		object ToObject();
	}
}
