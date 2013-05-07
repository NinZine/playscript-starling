using System;
using System.Drawing;
using PlayScript.Application;

namespace StarlingDemo
{
	class MainClass
	{
		static void Main (string[] args)
		{
			Application.Run (args, typeof(_root.Demo_Mobile) );
			// Application.Run (args, typeof(_root.Tutorial1) );
			// Application.Run (args, typeof(_root.MyStarlingTest) );
		}
	}
}

