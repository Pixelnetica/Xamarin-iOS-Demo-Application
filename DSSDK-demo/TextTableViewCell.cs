using Foundation;
using System;
using UIKit;

namespace DSSDKdemo
{
    public partial class TextTableViewCell : UITableViewCell
    {
        public TextTableViewCell( IntPtr handle ) : base( handle )
        {
        }

        public void UpdateLabelText()
        {
            NSUserDefaults defaults = NSUserDefaults.StandardUserDefaults;

            nint tag = this.Tag;

            UILabel label = this.ProfileLabel;

            UITextField field = this.TextField;

            if( tag == 104 )
            {
                label.Text = "Autoshot delay";

                field.Text = defaults.StringForKey( "DelayValue" );
            }
        }

		public override void AwakeFromNib()
		{
            base.AwakeFromNib();

            this.TextField.Delegate = new Delegate( this );
		}

		private class Delegate : UITextFieldDelegate
        {
            private TextTableViewCell outer;

            public Delegate( TextTableViewCell outer )
            {
                this.outer = outer;
            }

			public override bool ShouldChangeCharacters( UITextField textField, NSRange range, string replacementString )
			{
                string str = outer.TextField.Text;

                str = str.Substring( 0, (int)range.Location ) + replacementString + str.Substring( (int)range.Location + (int)range.Length );

                do {
                    NSUserDefaults defaults = NSUserDefaults.StandardUserDefaults;

                    nint tag = outer.Tag;

                    if( tag == 104 )
                        defaults.SetString( str, "DelayValue" );
                    else
                        break;

                    defaults.Synchronize();
                } while( false );

                return true;
			}
		}
    }
}
