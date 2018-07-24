// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace DSSDKdemo
{
    [Register ("PhotoSliderViewController")]
    partial class PhotoSliderViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem LoadButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton RotateLeft { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton RotateRight { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem SaveButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem SettingsButton { get; set; }

        [Action ("LoadAction:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void LoadAction (UIKit.UIBarButtonItem sender);

        [Action ("RotateLeftAction:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void RotateLeftAction (UIKit.UIButton sender);

        [Action ("RotateRightAction:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void RotateRightAction (UIKit.UIButton sender);

        [Action ("SaveAction:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void SaveAction (UIKit.UIBarButtonItem sender);

        [Action ("SettingsAction:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void SettingsAction (UIKit.UIBarButtonItem sender);

        void ReleaseDesignerOutlets ()
        {
            if (ImageView != null) {
                ImageView.Dispose ();
                ImageView = null;
            }

            if (LoadButton != null) {
                LoadButton.Dispose ();
                LoadButton = null;
            }

            if (RotateLeft != null) {
                RotateLeft.Dispose ();
                RotateLeft = null;
            }

            if (RotateRight != null) {
                RotateRight.Dispose ();
                RotateRight = null;
            }

            if (SaveButton != null) {
                SaveButton.Dispose ();
                SaveButton = null;
            }

            if (SettingsButton != null) {
                SettingsButton.Dispose ();
                SettingsButton = null;
            }
        }
    }
}