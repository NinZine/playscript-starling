using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace PlayScriptApp
{
        public partial class MainWindow : MonoMac.AppKit.NSWindow
        {
                public MainWindow (IntPtr handle) : base(handle)
                {
                }

                public MainWindow (NSCoder coder) : base(coder)
                {
                }
        }
}

