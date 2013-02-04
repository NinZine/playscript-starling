
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.CoreAnimation;
using MonoMac.CoreGraphics;
using MonoMac.CoreVideo;
using MonoMac.OpenGL;
using System.Runtime.InteropServices;

using flash.display;
using flash.display3D;
using OpenGLLayer;

using starling.display;

namespace OpenGLLayer
{
	public partial class OpenGLLayer : MonoMac.CoreAnimation.CAOpenGLLayer
	{
		bool animate;

		public OpenGLLayer () : base()
		{
			Initialize ();
		}

		// Called when created from unmanaged code
		public OpenGLLayer (IntPtr handle) : base(handle)
		{
			Initialize ();
		}

		// Called when created directly from a XIB file
		[Export("initWithCoder:")]
		public OpenGLLayer (NSCoder coder) : base(coder)
		{
			Initialize ();
		}

		Tutorial1 mTutorial;

		// Shared initialization code
		void Initialize ()
		{
			Animate = true;
			this.Asynchronous = true;

		}

		public bool Animate {
			get { return animate; }
			set { animate = value; }
		}

		public override bool CanDrawInCGLContext (CGLContext glContext, CGLPixelFormat pixelFormat, double timeInterval, CVTimeStamp timeStamp)
		{
			return animate;
		}

		// this is our flash display stage
		flash.display.Stage    mStage;
		// this is our starling context
		starling.core.Starling mStarling;

		_root.Demo_Mobile mDemoMobile;

		public override void DrawInCGLContext (MonoMac.OpenGL.CGLContext glContext, CGLPixelFormat pixelFormat, double timeInterval, CVTimeStamp timeStamp)
		{
			if (mStage == null) {
				// construct flash stage
				mStage = new flash.display.Stage (960, 640);
			}

			if (mDemoMobile == null) {
				// construct starling demo
				flash.display.DisplayObject.globalStage = mStage;
				//mDemoMobile = new _root.Demo_Mobile();
				flash.display.DisplayObject.globalStage = null;
			}

			if (mStarling == null) {
				// construct starling  instance
				mStarling = new starling.core.Starling (typeof(MyStarlingTest), mStage);
			}

			if (mTutorial == null) {
				// construct tutorial
				flash.display.DisplayObject.globalStage = mStage;
				// mTutorial = new Tutorial1 ();
				flash.display.DisplayObject.globalStage = null;
			}


			//GL.ShadeModel (ShadingModel.Smooth);
			GL.ClearColor (NSColor.Blue); //NSColor.Clear.UsingColorSpace (NSColorSpace.CalibratedRGB));
			GL.Clear (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			if (mStage != null) {
				mStage.onEnterFrame ();
			}

			if (mStarling != null) {
				// $$TODO use timeInterval
				mStarling.stage.advanceTime (1.0 / 60.0);
			}

			if (mTutorial != null) {
				mTutorial.dispatchEvent (new flash.events.Event (flash.events.Event.ENTER_FRAME));
			}

			// update all timer objects
			flash.utils.Timer.advanceAllTimers(1000.0 / 60.0);

			GL.Flush ();
		}

		public override CGLPixelFormat CopyCGLPixelFormatForDisplayMask (uint mask)
		{
			// make sure to add a null value
			CGLPixelFormatAttribute[] attribs = new CGLPixelFormatAttribute[] { 
				CGLPixelFormatAttribute.Accelerated, 
				CGLPixelFormatAttribute.DoubleBuffer,
				CGLPixelFormatAttribute.ColorSize,
				(CGLPixelFormatAttribute)24,
				CGLPixelFormatAttribute.DepthSize,
				(CGLPixelFormatAttribute)16,
				(CGLPixelFormatAttribute)0
			};
                        
			int numPixs = -1;
			CGLPixelFormat pixelFormat = new CGLPixelFormat (attribs, out numPixs);
			return pixelFormat;
		}


 
	}
}

