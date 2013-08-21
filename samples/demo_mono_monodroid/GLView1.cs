using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES11;
using OpenTK.Platform;
using OpenTK.Platform.Android;
using Android.Views;
using Android.Content;
using Android.Util;

namespace demo_mono_monodroid
{
	class GLView1 : AndroidGameView
	{
		private PlayScript.Player mPlayer;

		public GLView1 (Context context) : base (context)
		{
			Init ();
		}

		// This gets called when the drawing surface is ready
		protected override void OnLoad (EventArgs e)
		{
			base.OnLoad (e);
			Run ();
		}

		void Init()
		{
			var rect = GetScaledFrame ();

			//PlayScript.Player.ApplicationClass = typeof(_root.Tutorial1);
			PlayScript.Player.ApplicationClass = typeof(_root.MyStarlingTest);

			mPlayer = new PlayScript.Player( rect );
		}

		// This method is called everytime the context needs
		// to be recreated. Use it to set any egl-specific settings
		// prior to context creation
		//
		protected override void CreateFrameBuffer ()
		{
			ContextRenderingApi = GLVersion.ES2;
			
			// the default GraphicsMode that is set consists of (16, 16, 0, 0, 2, false)
			try {
				Log.Verbose ("GLTriangle", "Loading with default settings");

				// if you don't call this, the context won't be created
				base.CreateFrameBuffer ();
				return;
			} catch (Exception ex) {
				Log.Verbose ("GLTriangle", "{0}", ex);
			}

			// this is a graphics setting that sets everything to the lowest mode possible so
			// the device returns a reliable graphics setting.
			try {
				Log.Verbose ("GLTriangle", "Loading with custom Android settings (low mode)");
				GraphicsMode = new AndroidGraphicsMode (0, 0, 0, 0, 0, false);

				// if you don't call this, the context won't be created
				base.CreateFrameBuffer ();
				return;
			} catch (Exception ex) {
				Log.Verbose ("GLTriangle", "{0}", ex);
			}
			throw new Exception ("Can't load egl, aborting");
		}

		// This gets called on each frame render
		protected override void OnRenderFrame (FrameEventArgs e)
		{
			try
			{
				// you only need to call this if you have delegates
				// registered that you want to have called
				base.OnRenderFrame (e);
				base.MakeCurrent ();

				mPlayer.RunUntilPresent( GetScaledFrame() );

				PlayScript.Profiler.Begin("swap");
				SwapBuffers ();
				PlayScript.Profiler.End("swap");

			} catch( Exception ex ) 
			{
				Console.WriteLine ("An exception has occurred. " + BuildExceptionString (ex));
			}
		}

		private System.Drawing.RectangleF GetScaledFrame()
		{
			var rect = new System.Drawing.RectangleF ();
			rect.X 		= 0;
			rect.Y      = 0;
			rect.Width  = this.Width;
			rect.Height = this.Height;
			return rect;
		}

		private static string BuildExceptionString( Exception exception )
		{
			string errMessage = string.Empty;

			errMessage += exception.Message + Environment.NewLine + exception.StackTrace;

			while ( exception.InnerException != null )
			{
				errMessage += BuildInnerExceptionString( exception.InnerException );
				exception = exception.InnerException;
			}

			return errMessage;
		}

		private static string BuildInnerExceptionString( Exception innerException )
		{
			string errMessage = string.Empty;

			errMessage += Environment.NewLine + " InnerException ";
			errMessage += Environment.NewLine + innerException.Message + Environment.NewLine + innerException.StackTrace;

			return errMessage;
		}
	}
}

