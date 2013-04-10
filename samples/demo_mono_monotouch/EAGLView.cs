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

using flash.display;
using flash.display3D;
using starling.display;
using OpenGLLayer;

namespace StarlingDemo_ios
{
	[Register ("EAGLView")]
	public class EAGLView : iPhoneOSGameView
	{
		// this is our flash display stage
		flash.display.Stage    mStage;
		// this is our starling context
		starling.core.Starling mStarling;
		Tutorial1 mTutorial;
		_root.Demo_Mobile mDemoMobile;

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
		
			foreach (UITouch touch in touches) {
				var p = touch.LocationInView(this);
				Console.WriteLine ("touches-began {0}", p);

				var te = new flash.events.TouchEvent(flash.events.TouchEvent.TOUCH_BEGIN, true, false, 0, true, p.X, p.Y, 1.0, 1.0, 1.0 );
				if (mStage!=null) mStage.dispatchEvent (te);
			}
		}

		public override void TouchesMoved (NSSet touches, UIEvent evt)
		{
			base.TouchesMoved (touches, evt);
		
			foreach (UITouch touch in touches) {
				var p = touch.LocationInView(this);
				Console.WriteLine ("touches-moved {0}", p);

				var te = new flash.events.TouchEvent(flash.events.TouchEvent.TOUCH_MOVE, true, false, 0, true, p.X, p.Y, 1.0, 1.0, 1.0 );
				if (mStage!=null) mStage.dispatchEvent (te);
			}
		}

		public override void TouchesEnded (NSSet touches, UIEvent evt)
		{
			base.TouchesEnded (touches, evt);

			foreach (UITouch touch in touches) {
				var p = touch.LocationInView(this);
				Console.WriteLine ("touches-ended {0}", p);

				var te = new flash.events.TouchEvent(flash.events.TouchEvent.TOUCH_END, true, false, 0, true, p.X, p.Y, 1.0, 1.0, 1.0 );
				if (mStage!=null) mStage.dispatchEvent (te);
			}
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

		protected override void OnRenderFrame (FrameEventArgs e)
		{
			base.OnRenderFrame (e);
			
			MakeCurrent ();

			if (mStage == null) {
				// construct flash stage
				mStage = new flash.display.Stage ((int)this.Frame.Width, (int)this.Frame.Height);
			}

			if (mTutorial == null) {
				// construct tutorial
				flash.display.DisplayObject.globalStage = mStage;
				//mTutorial = new Tutorial1 ();
				flash.display.DisplayObject.globalStage = null;
			}

			if (mStarling == null) {
				// construct starling  instance
				//starling.core.Starling.multitouchEnabled = true;
				//mStarling = new starling.core.Starling (typeof(MyStarlingTest), mStage);
				//mStarling.start();
			}

			if (mDemoMobile == null) {
				// construct starling demo
				flash.display.DisplayObject.globalStage = mStage;
				mDemoMobile = new _root.Demo_Mobile();
				flash.display.DisplayObject.globalStage = null;
			}

			// Do a clear to check to see if we're working.. (remark out if everything is fine).
			// GL.ClearColor (0.5f, 0.5f, 0.5f, 1.0f);
			// GL.Clear (ClearBufferMask.ColorBufferBit);

			if (mStage != null) {
				mStage.onEnterFrame ();
			}
			
			// update all timer objects
			flash.utils.Timer.advanceAllTimers();

			SwapBuffers ();
		}

	}
}
