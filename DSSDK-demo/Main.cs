using UIKit;

using DocScanningSDK;

namespace DSSDKdemo
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main( string[] args )
        {
            string key = "F1KODdLXTAYpmiZIN+HX3UcxRgF3cy+u6ZNYd6k0KWJlgYKuPhH+3qWs+o14CgrmiB0i8pc1SrvG/UhAAjxahuC8yBz/5uaM0I//jopXv2h3T0k5gOyqmXRTGIBZBIydyrDQwULnp1tNRRwSuy4m7c7BmlAf7aO+jpAqqFvjOD79/7RgeTzAQ9jQrn4Tz33yx6L0WqzZcRxpIVx6ADIJUn5DM33wqHgq/cIqAr+lUUpojUogUTSE1zpjJSrpGzBKxG7BIhLeH3jWpG/u63/zp4RKlSOdJG5wM39wume73GYQ/wG1iGM+DVf0t7av7E+iLNxkQj8D3kjVMNHNumXYHA==";

            PxLicense.InitializeWithKey( key );

            UIApplication.Main( args, null, nameof( PhotoSliderAppDelegate ) );
        }
    }
}
