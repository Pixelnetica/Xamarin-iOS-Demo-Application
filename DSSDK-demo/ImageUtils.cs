using System;
using UIKit;
using CoreGraphics;

namespace DSSDKdemo
{
    public class ImageUtils
    {
        public static UIImage ScaleImage( UIImage image, CGSize size )
        {
            // Create a graphics image context
            UIGraphics.BeginImageContext( size );

            // Tell the old image to draw in this new context, with the desired new size
//            image.DrawAsPatternInRect( new CGRect( 0, 0, size.Width, size.Height ) );
            image.Draw( new CGRect( 0, 0, size.Width, size.Height ) );
    
            // Get the new image from the context
            image = UIGraphics.GetImageFromCurrentImageContext();

            // End the context
            UIGraphics.EndImageContext();
    
            return image;
        }

        public static UIImage NormalizeImageOrientation( UIImage image )
        {
            if( image.Orientation == UIImageOrientation.Up )
                return image;

            CGSize size = image.Size;

            UIGraphics.BeginImageContextWithOptions( size, false, image.CurrentScale );

            image.Draw( new CGRect( 0, 0, size.Width, size.Height ) );
//            image.DrawAsPatternInRect( new CGRect( 0, 0, size.Width, size.Height ) );

            UIImage normalizedImage = UIGraphics.GetImageFromCurrentImageContext();

            UIGraphics.EndImageContext();

            return normalizedImage;
        }
    }
}
