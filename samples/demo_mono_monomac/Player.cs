// Copyright 2013 Zynga Inc.
//	
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//		
//      Unless required by applicable law or agreed to in writing, software
//      distributed under the License is distributed on an "AS IS" BASIS,
//      WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//      See the License for the specific language governing permissions and
//      limitations under the License.

using System;
using System.Collections.Generic;
using System.Drawing;
using MonoMac.OpenGL;

namespace PlayScriptApp
{
	public class Player
	{
		public string Title
		{
			get; set;
		}

		public Player(RectangleF bounds)
		{
			// construct flash stage
			mStage = new flash.display.Stage ((int)bounds.Width, (int)bounds.Height);

			// construct demo
			// $$TODO come up with a better demo chooser!
			flash.display.DisplayObject.globalStage = mStage;
			//mDemoSprite = new _root.Basic_View();
			//mDemoSprite = new _root.Basic_SkyBox();
			//mDemoSprite = new _root.Basic_Particles();
			//mDemoSprite = new _root.Intermediate_ParticleExplosions();
			//mDemoSprite = new _root.Basic_Shading();
			mDemoSprite = new _root.Demo_Mobile();
			//mDemoSprite = new PlayScriptApp.Tutorial1();
			//new starling.core.Starling (typeof(MyStarlingTest), mStage)
			flash.display.DisplayObject.globalStage = null;

			// add sprite to stage
			mStage.addChild(mDemoSprite);

			Title = mDemoSprite.GetType().ToString();
		}

		public void OnKeyDown (uint charCode, uint keyCode)
		{
			var ke = new flash.events.KeyboardEvent(flash.events.KeyboardEvent.KEY_DOWN, true, false, charCode, keyCode);
			mStage.dispatchEvent (ke);
		}
		
		public void OnKeyUp (uint charCode, uint keyCode)
		{
			var ke = new flash.events.KeyboardEvent(flash.events.KeyboardEvent.KEY_UP, true, false, charCode, keyCode);
			mStage.dispatchEvent (ke);
		}
		
		public void OnMouseDown (PointF p, uint buttonMask)
		{
			mStage.mouseX = p.X;
			mStage.mouseY = p.Y;
			
			// dispatch touch event
			var te = new flash.events.TouchEvent(flash.events.TouchEvent.TOUCH_BEGIN, true, false, 0, true, p.X, p.Y, 1.0, 1.0, 1.0 );
			mStage.dispatchEvent (te);
			
			// dispatch mouse event
			var me = new flash.events.MouseEvent(flash.events.MouseEvent.MOUSE_DOWN, true, false, p.X, p.Y, mStage);
			mStage.dispatchEvent (me);
		}
		
		public void OnMouseUp (PointF p, uint buttonMask)
		{
			mStage.mouseX = p.X;
			mStage.mouseY = p.Y;
			
			// dispatch touch event
			var te = new flash.events.TouchEvent(flash.events.TouchEvent.TOUCH_END, true, false, 0, true, p.X, p.Y, 1.0, 1.0, 1.0 );
			mStage.dispatchEvent (te);
			
			// dispatch mouse event
			var me = new flash.events.MouseEvent(flash.events.MouseEvent.MOUSE_UP, true, false, p.X, p.Y, mStage);
			mStage.dispatchEvent (me);
		}
		
		public void OnMouseMoved(PointF p, uint buttonMask)
		{
			mStage.mouseX = p.X;
			mStage.mouseY = p.Y;
			
			// dispatch touch event
			var te = new flash.events.TouchEvent(flash.events.TouchEvent.TOUCH_MOVE, true, false, 0, true, p.X, p.Y, 1.0, 1.0, 1.0 );
			mStage.dispatchEvent (te);
			
			// dispatch mouse event
			var me = new flash.events.MouseEvent(flash.events.MouseEvent.MOUSE_MOVE, true, false, p.X, p.Y, mStage);
			mStage.dispatchEvent (me);
		}

		public void OnResize (RectangleF bounds)
		{
			// Reset The Current Viewport
			GL.Viewport (0, 0, (int)bounds.Size.Width, (int)bounds.Size.Height);
			mStage.onResize((int)bounds.Size.Width, (int)bounds.Size.Height);
		}

		public void OnFrame()
		{
			//GL.ClearColor (1,0,1,0);
			//GL.Clear (ClearBufferMask.ColorBufferBit);

			// stage enter frame
			mStage.onEnterFrame ();

			// update all timer objects
			flash.utils.Timer.advanceAllTimers();

			// stage exit frame
			mStage.onExitFrame ();
		}

		private flash.display.Stage    mStage;
		private flash.display.Sprite   mDemoSprite;

	}
}

