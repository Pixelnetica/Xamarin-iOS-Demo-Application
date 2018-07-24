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
    [Register ("CameraViewController")]
    partial class CameraViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        DSSDKdemo.CameraView CamView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton FlashButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView FlashImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Label { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LabelForDocArea { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LabelForDocDistortion { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        DSSDKdemo.OverlayView OverlayView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        DSSDKdemo.CircularProgressView ProgressView { get; set; }

        [Action ("CloseView:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void CloseView (UIKit.UIButton sender);

        [Action ("FlashModeAction:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void FlashModeAction (UIKit.UIButton sender);

        [Action ("SnapShot:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void SnapShot (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (CamView != null) {
                CamView.Dispose ();
                CamView = null;
            }

            if (FlashButton != null) {
                FlashButton.Dispose ();
                FlashButton = null;
            }

            if (FlashImageView != null) {
                FlashImageView.Dispose ();
                FlashImageView = null;
            }

            if (Label != null) {
                Label.Dispose ();
                Label = null;
            }

            if (LabelForDocArea != null) {
                LabelForDocArea.Dispose ();
                LabelForDocArea = null;
            }

            if (LabelForDocDistortion != null) {
                LabelForDocDistortion.Dispose ();
                LabelForDocDistortion = null;
            }

            if (OverlayView != null) {
                OverlayView.Dispose ();
                OverlayView = null;
            }

            if (ProgressView != null) {
                ProgressView.Dispose ();
                ProgressView = null;
            }
        }
    }
}