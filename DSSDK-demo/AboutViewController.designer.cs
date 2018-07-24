// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace DSSDKdemo
{
    [Register ("AboutViewController")]
    partial class AboutViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIWebView WebView { get; set; }

        [Action ("CloseView:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void CloseView (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (WebView != null) {
                WebView.Dispose ();
                WebView = null;
            }
        }
    }
}