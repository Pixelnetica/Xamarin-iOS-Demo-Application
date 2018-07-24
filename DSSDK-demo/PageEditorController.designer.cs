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
    [Register ("PageEditorController")]
    partial class PageEditorController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        DSSDKdemo.MyImageView ImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem ProcessButton { get; set; }

        [Action ("CancelAction:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void CancelAction (UIKit.UIBarButtonItem sender);

        [Action ("ProcessAction:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void ProcessAction (UIKit.UIBarButtonItem sender);

        void ReleaseDesignerOutlets ()
        {
            if (ImageView != null) {
                ImageView.Dispose ();
                ImageView = null;
            }

            if (ProcessButton != null) {
                ProcessButton.Dispose ();
                ProcessButton = null;
            }
        }
    }
}