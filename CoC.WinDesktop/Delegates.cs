using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCWinDesktop
{
	//This is purely syntactic sugar, but it exists to denote that the current function callback
	//is expected to work without having to worry about having its last location correctly set. 
	public delegate void SafeAction();
}
