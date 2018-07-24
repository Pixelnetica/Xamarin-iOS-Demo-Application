using Foundation;
using System;
using UIKit;

namespace DSSDKdemo
{
    public partial class SettingsProfileTableViewCell : UITableViewCell
    {
        public SettingsProfileTableViewCell( IntPtr handle ) : base( handle )
        {
        }

        public void Configure( string imageName, string label )
        {
            this.ProfileLabel.Text = label;
        }
    }
}
