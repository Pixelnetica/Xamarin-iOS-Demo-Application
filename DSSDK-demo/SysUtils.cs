using System;
using UIKit;

namespace DSSDKdemo
{
    public class SysUtils
    {
        public static void ShowAlert( UIViewController vc, string title, string message, params object[] args )
        {
            message = String.Format( message, args );

            Console.WriteLine( message );

            UIAlertController alert = UIAlertController.Create( title, message, UIAlertControllerStyle.Alert );

            UIAlertAction action = UIAlertAction.Create( "OK", UIAlertActionStyle.Default, null );

            alert.AddAction( action );

            vc.PresentViewController( alert, true, null );
        }
    }
}
