using UIKit;

using DocScanningSDK;

namespace DSSDKdemo
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main( string[] args )
        {
            string key = "bcNeXMdKkGoUhnN/VkkrqRV2SQslaUHSLJjQo1PeJXv5j78WqCyaV9pnUnPwMQWotlI3onOh8WR47VGC+tpI+d09J07VaDxjckexVybSCpqWYdlzFwGU8Cfi2IUXZ9fffInqJtUXFwY4g/ieHQWXb268bDuC/X/Vvq3jPs7iIoOLWss6d2A92pjDMNBXd7uuRzjO7TnRk4wVlDK2rXRW6izP/qewSqGxDPHKFi153thVgTrHtf5Fgw0LJeqMT66aoxaXLg2CqPQfJPszda6v3eLG1Hlnvrvz6SVqkE+QVvI3FArunyrKxfkfSXYAXwCVgbPpN3Z1Bhmg+Q+ZERZ3KA==";

            PxLicense.InitializeWithKey( key );

            UIApplication.Main( args, null, nameof( PhotoSliderAppDelegate ) );
        }
    }
}
