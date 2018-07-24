using System;
using UIKit;
using CoreGraphics;
using ObjCRuntime;
using Foundation;

using DocScanningSDK;

namespace DSSDKdemo
{
    public enum CornerType
    {
        LeftTop,
        RightTop,
        LeftBottom,
        RightBottom
    }

    public class CornerView : UIImageView
    {
        private UIImage imageNormal;
        private UIImage imageSelected;
        private bool isSelected;

        public CornerView( UIImage imageNormal, UIImage imageSelected )
            : base( imageNormal )
        {
            this.imageNormal = imageNormal;
            this.imageSelected = imageSelected;
        }

        public CGPoint CenterOffset { get; set; }

        public CGPoint PushCenter { get; set; }

        public CornerType CornerType { get; set; }

        public bool IsSelected
        {
            get {
                return this.isSelected;
            }

            set {
                if( isSelected != value )
                {
                    this.isSelected = value;
                    this.Image = isSelected ? imageSelected : imageNormal;
                    this.SetNeedsDisplay();
                }
            }
        }

    }

    public partial class MyImageView : UIView
    {
        private UIImage image;
        private CGLayer layerBottom; // contains image preview
        private float minScale;
        private float scale;

        private CGPoint startLocation;

        private CornerView[] corners = new CornerView[4];
        private CornerView corverDragged;

        private static nint kCorner1Tag = 201;
        private static nint kCorner2Tag = 202;
        private static nint kCorner3Tag = 203;
        private static nint kCorner4Tag = 204;

        private static nint kPageEditorNavBar = 100;
        private static nint kPageEditorToolBar = 101;
        private static nint kPageEditorProfileBar = 102;

        private const float GAP = 20;
        private const float SCALE = 2.0f;

        public MyImageView( CGRect frame ) : base( frame )
        {
            Initialize();
        }

        public MyImageView( IntPtr handle ) : base( handle )
        {
            Initialize();
        }

        ~MyImageView()
        {
            if( layerBottom != null )
                layerBottom.Dispose();
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            Initialize();
        }

        private void Initialize()
        {
            this.UserInteractionEnabled = true;
            //this.ContentMode = UIViewContentMode.Redraw;
            this.BackgroundColor = UIColor.Black;
            this.ClipsToBounds = true;

            CreateCorner( CornerType.LeftTop, "ltn.png", "lta.png" );
            CreateCorner( CornerType.RightTop, "rtn.png", "rta.png" );
            CreateCorner( CornerType.LeftBottom, "lbn.png", "lba.png" );
            CreateCorner( CornerType.RightBottom, "rbn.png", "rba.png" );

            corners[(uint)CornerType.LeftTop].CenterOffset = new CGPoint( 2.0f, 2.0f );
            corners[(uint)CornerType.RightTop].CenterOffset = new CGPoint( -1.0f, 2.0f );
            corners[(uint)CornerType.LeftBottom].CenterOffset = new CGPoint( 2.0f, -1.0f );
            corners[(uint)CornerType.RightBottom].CenterOffset = new CGPoint( -1.0f, -1.0f );

            corners[(uint)CornerType.LeftTop].Tag = kCorner1Tag;
            corners[(uint)CornerType.RightTop].Tag = kCorner2Tag;
            corners[(uint)CornerType.LeftBottom].Tag = kCorner3Tag;
            corners[(uint)CornerType.RightBottom].Tag = kCorner4Tag;

            this.minScale = 1.0f;
            this.scale = 1.0f;
        }

        private CornerView CreateCorner( CornerType type, string normalImageName, string selectedImageName )
        {
            CornerView cv = new CornerView(
                new UIImage( normalImageName ),
                new UIImage( selectedImageName )
                );

            cv.CornerType = type;

            corners[(uint)type] = cv;

            AddSubview( cv );

            return cv;
        }

        public UIImage Image
        {
            get {
                return this.image;
            }

            set {
                this.image = ImageUtils.NormalizeImageOrientation( value );
//                this.image = value;

                layerBottom = null;

                CalculateMinScale();

                scale = minScale;

                SetDefaultPositionOfCorners();
            }
        }

        private void CalculateMinScale()
        {
            minScale = 1.0f;

            if( image != null )
            {
                CGSize imageSize = image.Size;
                CGRect bounds = this.Bounds;

                // Calculate and set the zoom scale values
                float xscale = ((float)bounds.Size.Width - GAP * 2.0f) / (float)imageSize.Width;
                float yscale = ((float)bounds.Size.Height - GAP * 2.0f) / (float)imageSize.Height;

                minScale = Math.Min( xscale, yscale );
            }
        }

        public void SetDefaultPositionOfCorners()
        {
            CGPoint[] point = new CGPoint[4];
            CGRect rcBounds;

            if( image == null )
                rcBounds = CGRect.Inflate( this.Bounds, GAP, GAP );
            else
                rcBounds = WholeImageRectInView();

            point[0] = rcBounds.Location;
            point[1] = new CGPoint( rcBounds.Location.X + rcBounds.Size.Width, rcBounds.Location.Y );
            point[2] = new CGPoint( rcBounds.Location.X, rcBounds.Location.Y + rcBounds.Size.Height );
            point[3] = new CGPoint( rcBounds.Location.X + rcBounds.Size.Width, rcBounds.Location.Y + rcBounds.Size.Height );

            SetCornersCoordinates( point, false );
        }

        public void GetCornersCoordinates( CGPoint[] points, bool inImage )
        {
            points[0] = corners[(uint)CornerType.LeftTop].Center;
            points[1] = corners[(uint)CornerType.RightTop].Center;
            points[2] = corners[(uint)CornerType.LeftBottom].Center;
            points[3] = corners[(uint)CornerType.RightBottom].Center;

            points[0].X -= corners[(uint)CornerType.LeftTop].CenterOffset.X;
            points[0].Y -= corners[(uint)CornerType.LeftTop].CenterOffset.Y;

            points[1].X -= corners[(uint)CornerType.RightTop].CenterOffset.X;
            points[1].Y -= corners[(uint)CornerType.RightTop].CenterOffset.Y;

            points[2].X -= corners[(uint)CornerType.LeftBottom].CenterOffset.X;
            points[2].Y -= corners[(uint)CornerType.LeftBottom].CenterOffset.Y;

            points[3].X -= corners[(uint)CornerType.RightBottom].CenterOffset.X;
            points[3].Y -= corners[(uint)CornerType.RightBottom].CenterOffset.Y;

            if( inImage )
            {
                points[0] = ConvertFromViewToImagePt( points[0] );
                points[1] = ConvertFromViewToImagePt( points[1] );
                points[2] = ConvertFromViewToImagePt( points[2] );
                points[3] = ConvertFromViewToImagePt( points[3] );
            }
        }

        public void SetCornersCoordinates( CGPoint[] points, bool inImage )
        {
            CGPoint[] pt = new CGPoint[4];

            pt[0] = points[0];
            pt[1] = points[1];
            pt[2] = points[2];
            pt[3] = points[3];

            if( inImage )
            {
                pt[0] = ConvertFromImageToViewPt( pt[0] );
                pt[1] = ConvertFromImageToViewPt( pt[1] );
                pt[2] = ConvertFromImageToViewPt( pt[2] );
                pt[3] = ConvertFromImageToViewPt( pt[3] );
            }

            pt[0].X += corners[(uint)CornerType.LeftTop].CenterOffset.X;
            pt[0].Y += corners[(uint)CornerType.LeftTop].CenterOffset.Y;

            pt[1].X += corners[(uint)CornerType.RightTop].CenterOffset.X;
            pt[1].Y += corners[(uint)CornerType.RightTop].CenterOffset.Y;

            pt[2].X += corners[(uint)CornerType.LeftBottom].CenterOffset.X;
            pt[2].Y += corners[(uint)CornerType.LeftBottom].CenterOffset.Y;

            pt[3].X += corners[(uint)CornerType.RightBottom].CenterOffset.X;
            pt[3].Y += corners[(uint)CornerType.RightBottom].CenterOffset.Y;

            corners[(uint)CornerType.LeftTop].Center = pt[0];
            corners[(uint)CornerType.RightTop].Center = pt[1];
            corners[(uint)CornerType.LeftBottom].Center = pt[2];
            corners[(uint)CornerType.RightBottom].Center = pt[3];

            this.SetNeedsDisplay();
        }

        public CGRect WholeImageRectInView()
        {
            // Calculate destination rectangle for drawing whole image
            CGSize imageSize = image.Size;
            CGRect imageRect = new CGRect( 0, 0, scale * imageSize.Width, scale * imageSize.Height );

            if( imageRect.Size.Width < this.Bounds.Size.Width )
                imageRect.X = (this.Bounds.Size.Width - imageRect.Size.Width) * 0.5f;

            if( imageRect.Size.Height < this.Bounds.Size.Height )
                imageRect.Y = (this.Bounds.Size.Height - imageRect.Size.Height) * 0.5f;

            return imageRect;
        }

        public CornerView NearestCornerView( CGPoint point )
        {
            const float distToSelect = 100.0f * 100.0f;

            CornerView nearest = null;

            float distMin = float.MaxValue;

            for( int i = 0; i < 4; ++i )
            {
                CornerView c = corners[i];

                CGPoint pt = c.Center;

                float dx = (float)(pt.X - point.X);
                float dy = (float)(pt.Y - point.Y);

                float dist = dx * dx + dy * dy;

                if( dist < distMin )
                {
                    distMin = dist;

                    nearest = c;
                }
            }

            return distMin >= distToSelect ? null : nearest;
        }

        public CGPoint GetCornerPoint( CornerView corner )
        {
            CGPoint pt = corner.Center;

            pt.X -= corners[(uint)CornerType.LeftTop].CenterOffset.X;
            pt.Y -= corners[(uint)CornerType.LeftTop].CenterOffset.Y;

            return pt;
        }

        public bool CorrectCornerPosition( CGPoint point, CornerView corner )
        {
            CGRect rect = WholeImageRectInView();

            point.X -= corner.CenterOffset.X;
            point.Y -= corner.CenterOffset.Y;

            point.X = Math.Max( (float)rect.Location.X, Math.Min( (float)point.X, (float)(rect.Location.X + rect.Size.Width) ) );
            point.Y = Math.Max( (float)rect.Location.Y, Math.Min( (float)point.Y, (float)(rect.Location.Y + rect.Size.Height) ) );

            point = ConvertFromViewToImagePt( point );

            {
                CGPoint[] points = new CGPoint[4];

                GetCornersCoordinates( points, true );

                for( int i = 0; i < 4; ++i )
                {
                    if( corner == corners[i] )
                    {
                        points[i] = point;
                        break;
                    }
                }
            }

            point = ConvertFromViewToImagePt( point );

            point.X += corner.CenterOffset.X;
            point.Y += corner.CenterOffset.Y;

            return true;
        }

        public CGPoint ConvertFromViewToImagePt( CGPoint point )
        {
            CGRect rectWhole = WholeImageRectInView();

            CGPoint offset = new CGPoint();

            offset.X = (this.Bounds.Size.Width - rectWhole.Size.Width) * 0.5f;
            offset.Y = (this.Bounds.Size.Height - rectWhole.Size.Height) * 0.5f;

            // Convert to Image coordinates

            point.X = (point.X - offset.X) / scale;
            point.Y = (point.Y - offset.Y) / scale;

            return point;
        }

        public CGPoint ConvertFromImageToViewPt( CGPoint pt )
        {
            CGRect rectWhole = WholeImageRectInView();

            CGPoint offset = new CGPoint();

            offset.X = (this.Bounds.Size.Width - rectWhole.Size.Width) * 0.5f;
            offset.Y = (this.Bounds.Size.Height - rectWhole.Size.Height) * 0.5f;

            CGPoint point = new CGPoint();

            point.X = pt.X * scale + offset.X;
            point.Y = pt.Y * scale + offset.Y;

            return point;
        }

        public override void Draw( CGRect area )
        {
            if( image == null )
                return;
            
            CGContext context = UIGraphics.GetCurrentContext();

            //
            //-----------------------------------------------------------
            // Draw bottom layer containing image preview
            //-----------------------------------------------------------
            //

            if( layerBottom == null )
            {
                CGLayer layer = CGLayer.Create( context, this.Bounds.Size );
                CGContext layerContext = layer.Context;

                layerContext.SetFillColor( this.BackgroundColor.CGColor );
                layerContext.FillRect( this.Bounds );

                CGAffineTransform transform = new CGAffineTransform( 1, 0, 0, -1, 0, this.Bounds.Size.Height );
                layerContext.ConcatCTM( transform );

                layerContext.DrawImage( WholeImageRectInView(), image.CGImage );

                layerBottom = layer;
            }

            context.DrawLayer( layerBottom, new CGPoint( 0, 0 ) );

            //-----------------------------------------------------------
            // Draw transparency layer containing edges around page area
            //-----------------------------------------------------------
            {
                CGPoint[] point = new CGPoint[4];
                GetCornersCoordinates( point, false );

                // Preserve the current drawing state
                context.SaveState();

                context.BeginTransparencyLayer( null );

                context.SetLineWidth( 1.0f );
                context.SetStrokeColor( 0.0f, 0.0f, 1.0f, 1.0f );

                //-----------------------------------------------------------------
                // Create complex path as intersection of subpath 1 with subpath 2
                //-----------------------------------------------------------------

                // Create subpath 1 with the whole image size
                context.SetFillColor( 0, 0, 0, 0.4f );
                context.AddRect( this.Bounds );

                // Create subpath 2 which cuts a page area from the subpath 1
                context.MoveTo( point[0].X, point[0].Y );
                context.AddLineToPoint( point[1].X, point[1].Y );
                context.AddLineToPoint( point[3].X, point[3].Y );
                context.AddLineToPoint( point[2].X, point[2].Y );
                context.AddLineToPoint( point[0].X, point[0].Y );
                context.EOFillPath();

                //-----------------------------------------------------------------
                // Create a new path which draws edges around page area
                //-----------------------------------------------------------------

                // Draw edge around page
                context.SetFillColor( 0, 0, 1, 1 );
                context.BeginPath();
                context.MoveTo( point[0].X, point[0].Y );
                context.AddLineToPoint( point[1].X, point[1].Y );
                context.AddLineToPoint( point[3].X, point[3].Y );
                context.AddLineToPoint( point[2].X, point[2].Y );
                context.AddLineToPoint( point[0].X, point[0].Y );
                context.StrokePath();

                context.EndTransparencyLayer();

                // Restore the previous drawing state.
                context.RestoreState();
            }
        }

        public override void TouchesBegan( NSSet touches, UIEvent evt )
        {
            CGPoint pt = (touches.AnyObject as UITouch).LocationInView( this );

            startLocation = pt;

            UIView view = this.Superview;

            view.BringSubviewToFront( this );

            view.ViewWithTag( kPageEditorNavBar ).Hidden = true;
            view.ViewWithTag( kPageEditorToolBar ).Hidden = true;

            AnimateZoomInAtPoint( startLocation );

            corverDragged = NearestCornerView( startLocation );
            if( corverDragged != null )
            {
                corverDragged.PushCenter = corverDragged.Center;
                corverDragged.IsSelected = true;
            }
        }

        public override void TouchesMoved( NSSet touches, UIEvent evt )
        {
            CGPoint pt = (touches.AnyObject as UITouch).LocationInView( this );

            if( corverDragged != null )
            {
                nfloat dx = pt.X - startLocation.X;
                nfloat dy = pt.Y - startLocation.Y;

                CGPoint newcenter = new CGPoint( dx + corverDragged.PushCenter.X, dy + corverDragged.PushCenter.Y );

                bool isValid = CorrectCornerPosition( newcenter, corverDragged );
                if( isValid )
                    corverDragged.Center = newcenter;

                this.SetNeedsDisplay();
            }
        }

        public override void TouchesEnded( NSSet touches, UIEvent evt )
        {
            AnimateZoomOutAtPoint();
            if( corverDragged != null )
                corverDragged.IsSelected = false;
        }

        public override void TouchesCancelled( NSSet touches, UIEvent evt )
        {
        }

        private void AnimateZoomInAtPoint( CGPoint touchPoint )
        {
            const float GROW_ANIMATION_DURATION_SECONDS = 0.15f;

            UIView.BeginAnimations( null );
            UIView.SetAnimationDuration( GROW_ANIMATION_DURATION_SECONDS );
            UIView.SetAnimationDelegate( this );
            UIView.SetAnimationDidStopSelector( new Selector( "zoomInAnimationDidStop:numFinished:context:" ) );

            CGPoint translate = CalculateTranslationForScale( touchPoint, SCALE );
            CGAffineTransform transform = CGAffineTransform.MakeTranslation( translate.X, translate.Y );
            transform = CGAffineTransform.Scale( transform, SCALE, SCALE );

            this.Transform = transform;

            UIView.CommitAnimations();
        }

        // Calculate the offset for a scaled image to save the selected image point
        // at the same place in the window
        CGPoint CalculateTranslationForScale( CGPoint point, float aScale )
        {
            CGPoint ptCentre = this.Center;
            CGRect toolbarHeight = this.Superview.ViewWithTag( kPageEditorNavBar ).Bounds;

            CGPoint offset = new CGPoint();

            offset.X = (point.X - ptCentre.X) * aScale + ptCentre.X;
            offset.Y = (point.Y - ptCentre.Y) * aScale + ptCentre.Y;

            offset.X = point.X - offset.X;
            offset.Y = point.Y - offset.Y - toolbarHeight.Size.Height;

            return offset;
        }

        [Export( "zoomInAnimationDidStop:numFinished:context:" )]
        private void ZoomInAnimationDidStop( NSString animationId, NSNumber finished, IntPtr context )
        {
        }

        private void AnimateZoomOutAtPoint()
        {
            const float GROW_ANIMATION_DURATION_SECONDS = 0.15f;

            UIView.BeginAnimations( null );
            UIView.SetAnimationDuration( GROW_ANIMATION_DURATION_SECONDS );
            UIView.SetAnimationDelegate( this );
            UIView.SetAnimationDidStopSelector( new Selector( "zoomOutAnimationDidStop:numFinished:context:" ) );

            this.Transform = CGAffineTransform.MakeScale( 1.0f, 1.0f );

            UIView.CommitAnimations();
        }

        [Export( "zoomOutAnimationDidStop:numFinished:context:" )]
        private void ZoomOutAnimationDidStop( NSString animationId, NSNumber finished, IntPtr context )
        {
            UIView view = this.Superview;

            view.ViewWithTag( kPageEditorNavBar ).Hidden = false;
            view.ViewWithTag( kPageEditorToolBar ).Hidden = false;
        }
    }
}
