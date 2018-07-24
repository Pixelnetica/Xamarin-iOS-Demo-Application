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
    [Register ("TextTableViewCell")]
    partial class TextTableViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ProfileLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField TextField { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ProfileLabel != null) {
                ProfileLabel.Dispose ();
                ProfileLabel = null;
            }

            if (TextField != null) {
                TextField.Dispose ();
                TextField = null;
            }
        }
    }
}