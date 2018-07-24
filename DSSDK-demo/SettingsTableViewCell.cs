using Foundation;
using System;
using UIKit;

namespace DSSDKdemo
{
    public partial class SettingsTableViewCell : UITableViewCell
    {
        public SettingsTableViewCell( IntPtr handle ) : base( handle )
        {
        }

        partial void ChangeSwitch( UISwitch sender )
        {
            NSUserDefaults defaults = NSUserDefaults.StandardUserDefaults;

            nint tag = this.Tag;

            bool on = this.SwitchCell.On;

            if( tag == 100 )
            {
                defaults.SetBool( on, "cropState" );
            }
            else if( tag == 101 )
            {
                defaults.SetBool( on, "cropMode" );
            }
            else if( tag == 102 )
            {
                defaults.SetBool( on, "borderDetector" );
            }
            else if( tag == 103 )
            {
                defaults.SetBool( on, "autoShot" );
            }
            else if( tag == 104 )
            {
                defaults.SetBool( on, "simulateMultipageFile" );
            }
            else
            {
                return;
            }

            defaults.Synchronize();
        }

        public void UpdateLabelText()
        {
            NSUserDefaults defaults = NSUserDefaults.StandardUserDefaults;

            nint tag = this.Tag;

            UISwitch cell = this.SwitchCell;

            UILabel label = this.CellNameLabel;

            if( tag == 100 )
            {
                label.Text = "Smart crop";

                cell.On = defaults.BoolForKey( "cropState" );
            }
            else if( tag == 101 )
            {
                label.Text = "Crop mode";

                cell.On = defaults.BoolForKey( "cropMode" );
            }
            else if( tag == 102 )
            {
                label.Text = "Document borders detector";

                cell.On = defaults.BoolForKey( "borderDetector" );
            }
            else if( tag == 103 )
            {
                label.Text = "AutoShot";

                cell.On = defaults.BoolForKey( "autoShot" );
            }
            else if( tag == 104 )
            {
                label.Text = "Simulate multi-page file";

                cell.On = defaults.BoolForKey( "simulateMultipageFile" );
            }
        }
    }
}
