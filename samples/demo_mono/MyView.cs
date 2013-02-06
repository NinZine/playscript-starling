
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.CoreAnimation;
using MonoMac.CoreGraphics;
using MonoMac.OpenGL;

namespace OpenGLLayer
{
	public partial class MyView : MonoMac.AppKit.NSView
	{

		static OpenGLLayer movingLayer;

        #region Constructors

		// Called when created from unmanaged code
		public MyView (IntPtr handle) : base(handle)
		{
			Initialize ();
		}

		// Called when created directly from a XIB file
		[Export("initWithCoder:")]
		public MyView (NSCoder coder) : base(coder)
		{
			Initialize ();
		}

		// Shared initialization code
		void Initialize ()
		{
		}

        #endregion

		public override void AwakeFromNib ()
		{
			// create OpenGL layer
			movingLayer = new OpenGLLayer ();
			movingLayer.Frame = this.Frame;

			// create core animation layer
			Layer = new CALayer ();
			Layer.AddSublayer (movingLayer);
			WantsLayer = true;
		}

		public override void MouseDown (NSEvent theEvent)
		{
			//PointF location =  ConvertPointFromView(theEvent.LocationInWindow, null);
			//movingLayer.Position = new PointF(location.X, location.Y);
		}
		
		partial void toggle (NSButton sender)
		{
			// movingLayer.Animate = !movingLayer.Animate;
		}
	}
}

