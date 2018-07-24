using Foundation;
using System;
using UIKit;
using CoreGraphics;

namespace DSSDKdemo
{
    public interface PageEditorControllerDelegate
    {
        void PageEditorControllerDidFinishedEditingPage( PageEditorController editor, CGPoint[] points );
        void PageEditorControllerCancel( PageEditorController editor );
    }

    public partial class PageEditorController : UIViewController
    {
        CGPoint[] pt = new CGPoint[4];

        public PageEditorController( IntPtr handle ) : base( handle )
        {
        }

        public PageEditorController( UIImage image, CGPoint[] points )
        {
            this.ModalTransitionStyle = UIModalTransitionStyle.CoverVertical;
            this.Image = image;

            SetPoints( points );
        }

        void SetPoints( CGPoint[] points )
        {
            for( int i = 0; i < 4; ++i )
                pt[i] = points[i];
        }

        public PageEditorControllerDelegate Delegate { get; set; }

        public UIImage Image { get; set; }

        public UIBarButtonItem ProfileButton { get; set;  }

		public override void ViewDidAppear( bool animated )
		{
            base.ViewDidAppear( animated );

            ImageView.Image = this.Image;
            ImageView.SetCornersCoordinates( pt, true );
        }

		public override void ViewDidUnload()
		{
            this.Image = null;

            base.ViewDidUnload();
		}

		partial void ProcessAction( UIBarButtonItem sender )
        {
            // Save corners position to the page..
            CGPoint[] points = new CGPoint[4];
            ImageView.GetCornersCoordinates( points, true );

            Delegate.PageEditorControllerDidFinishedEditingPage( this, points );
        }

        partial void CancelAction( UIBarButtonItem sender )
        {
            Delegate.PageEditorControllerCancel( this );
        }
    }
}
