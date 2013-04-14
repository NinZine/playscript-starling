
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using MonoMac.Foundation;
using MonoMac.AppKit;

namespace PlayScriptApp
{
        public partial class MainWindowController : MonoMac.AppKit.NSWindowController
        {

                Player player;
		bool isAnimating;

                #region Constructors

                // Call to load from the XIB/NIB file
                public MainWindowController () : base("MainWindow")
                {
 
                }

                #endregion

                //strongly typed window accessor
                public new MainWindow Window {
                        get { return (MainWindow)base.Window; }
                }

		public override void AwakeFromNib ()
		{
			// Allocate the Player object
			player = new Player (openGLView.Bounds);

			// set window title
			Window.Title = Player.Title;

			// Assign the view's MainController to us
			openGLView.MainController = this;
			
			// reset the viewport and update OpenGL Context
			openGLView.UpdateView ();
			
			// Activate the display link now
			openGLView.StartAnimation ();
			
			isAnimating = true;
		}


		public void startAnimation ()
		{
			if (isAnimating)
				return;
			
			openGLView.StartAnimation ();

			isAnimating = true;
		}

		public void stopAnimation ()
		{
			if (!isAnimating)
				return;
			
			openGLView.StopAnimation ();

			isAnimating = false;
		}

                // Accessor property for our Player object
                public Player Player {
                        get { return player; }
                }
                
        
        }
}

