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
    [Register ("SettingsTableViewCell")]
    partial class SettingsTableViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel CellNameLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch SwitchCell { get; set; }

        [Action ("ChangeSwitch:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void ChangeSwitch (UIKit.UISwitch sender);

        void ReleaseDesignerOutlets ()
        {
            if (CellNameLabel != null) {
                CellNameLabel.Dispose ();
                CellNameLabel = null;
            }

            if (SwitchCell != null) {
                SwitchCell.Dispose ();
                SwitchCell = null;
            }
        }
    }
}