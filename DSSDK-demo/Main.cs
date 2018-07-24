using UIKit;

using DocScanningSDK;

namespace DSSDKdemo
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main( string[] args )
        {
            string key = "prpObFZAkWAZNPMGgIA/uMBLJfOj3pAzasxMoglnCmmpCoy5rTE6VqE0Tq6yXy56GafMFiTMeIXtRcgi8MW5Ulmlet889IkpiLRK7rcowpFfJj5yGMnbyI0qXEpIFTqf9X1NwIfNBfpIHrArHCK6HS0P4Jx4w5G8nHvN9sp3yyX9cY67mN9cJKNNN+hPTq+GW9wfn+rd+lLgQUVp5ob5zm/1JHp35TYQq4mq7JpSTbZpS0P7S5YtD61jyXeXpvgik+QIqUUILlPKV2URIuVpm7fH09kanknd33WkDoR0exds04MkyJvzrK7n6+DoQGF1999gA3PzZWtYqkQ00DFM9A==";

            PxLicense.InitializeWithKey( key );

            UIApplication.Main( args, null, nameof( PhotoSliderAppDelegate ) );
        }
    }
}
