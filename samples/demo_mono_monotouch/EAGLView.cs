using System;

using OpenTK;
using OpenTK.Graphics.ES20;
using GL1 = OpenTK.Graphics.ES11.GL;
using All1 = OpenTK.Graphics.ES11.All;
using OpenTK.Platform.iPhoneOS;

using MonoTouch.Foundation;
using MonoTouch.CoreAnimation;
using MonoTouch.ObjCRuntime;
using MonoTouch.OpenGLES;
using MonoTouch.UIKit;

namespace StarlingDemo_ios
{
	[Register ("EAGLView")]
	public class EAGLView : iPhoneOSGameView
	{
		// this is our playscript player
		PlayScript.Player      mPlayer;

		[Export("initWithCoder:")]
		public EAGLView (NSCoder coder) : base (coder)
		{
			LayerRetainsBacking = true;
			LayerColorFormat = EAGLColorFormat.RGBA8;
		}
		
		[Export ("layerClass")]
		public static new Class GetLayerClass ()
		{
			return iPhoneOSGameView.GetLayerClass ();
		}
		
		protected override void ConfigureLayer (CAEAGLLayer eaglLayer)
		{
			eaglLayer.Opaque = true;
		}
		
		protected override void CreateFrameBuffer ()
		{
			try {
				ContextRenderingApi = EAGLRenderingAPI.OpenGLES2;
				base.CreateFrameBuffer ();
			} catch (Exception) {
				ContextRenderingApi = EAGLRenderingAPI.OpenGLES1;
				base.CreateFrameBuffer ();
			}
		}
		
		protected override void DestroyFrameBuffer ()
		{
			base.DestroyFrameBuffer ();
		}

		public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
			base.TouchesBegan (touches, evt);

			mPlayer.OnTouchesBegan(touches, evt);
		}

		public override void TouchesMoved (NSSet touches, UIEvent evt)
		{
			base.TouchesMoved (touches, evt);
		
			mPlayer.OnTouchesMoved(touches, evt);
		}

		public override void TouchesEnded (NSSet touches, UIEvent evt)
		{
			base.TouchesEnded (touches, evt);

			mPlayer.OnTouchesEnded(touches, evt);
		}


		
		#region DisplayLink support
		
		int frameInterval;
		CADisplayLink displayLink;
		
		public bool IsAnimating { get; private set; }
		
		// How many display frames must pass between each time the display link fires.
		public int FrameInterval {
			get {
				return frameInterval;
			}
			set {
				if (value <= 0)
					throw new ArgumentException ();
				frameInterval = value;
				if (IsAnimating) {
					StopAnimating ();
					StartAnimating ();
				}
			}
		}
		
		public void StartAnimating ()
		{
			if (IsAnimating)
				return;
			
			CreateFrameBuffer ();
			CADisplayLink displayLink = UIScreen.MainScreen.CreateDisplayLink (this, new Selector ("drawFrame"));
			displayLink.FrameInterval = frameInterval;
			displayLink.AddToRunLoop (NSRunLoop.Current, NSRunLoop.NSDefaultRunLoopMode);
			this.displayLink = displayLink;

			if (mPlayer == null) {
				InitPlayer();
			}

			IsAnimating = true;
		}
		
		public void StopAnimating ()
		{
			if (!IsAnimating)
				return;
			displayLink.Invalidate ();
			displayLink = null;
			DestroyFrameBuffer ();
			IsAnimating = false;
		}
		
		[Export ("drawFrame")]
		void DrawFrame ()
		{
			OnRenderFrame (new FrameEventArgs ());
		}
		
		#endregion

		protected void InitPlayer()
		{
			// create player
			mPlayer = new PlayScript.Player(this.Frame);
			
			// load swf application
			mPlayer.LoadClass(typeof(_root.Demo_Mobile));
			//mPlayer.LoadClass(typeof(_root.Tutorial1));
			//mPlayer.LoadClass(typeof(_root.MyStarlingTest));
		}

		protected override void OnRenderFrame (FrameEventArgs e)
		{
			base.OnRenderFrame (e);
			
			MakeCurrent ();

			if (mPlayer != null) {
				mPlayer.OnFrame();
			}

			SwapBuffers ();
		}

	}
}
