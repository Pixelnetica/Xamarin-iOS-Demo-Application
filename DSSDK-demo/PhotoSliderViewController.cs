using Foundation;
using System;
using UIKit;
using CoreGraphics;
using AssetsLibrary;

using DocScanningSDK;

namespace DSSDKdemo
{
    public partial class PhotoSliderViewController : UIViewController, PageEditorControllerDelegate
    {
        private PxMetaImage inImage;
        private PxMetaImage outImage;
        private UIDocumentInteractionController dic;
        private UIImagePickerController imagePickerController;
        private PxRectangle points;
        private int rotationAngle = 0;

        class NavigationControllerDelegate : UINavigationControllerDelegate
        {
            private PhotoSliderViewController viewController;

            public NavigationControllerDelegate( PhotoSliderViewController viewController )
            {
                this.viewController = viewController;
            }
        }

        class ImagePickerControllerDelegate : UIImagePickerControllerDelegate
        {
            private PhotoSliderViewController viewController;

            public ImagePickerControllerDelegate( PhotoSliderViewController viewController )
            {
                this.viewController = viewController;
            }

            public override void FinishedPickingImage( UIImagePickerController picker, UIImage image, NSDictionary editingInfo )
            {
                NSDictionary dict = NSDictionary.FromObjectAndKey( image, (NSString)"UIImagePickerControllerOriginalImage" );

                FinishedPickingMedia( picker, dict );
            }

			public override void FinishedPickingMedia( UIImagePickerController picker, NSDictionary info )
			{
                //got selected image

                NSUrl assetURL = (NSUrl)info.ObjectForKey( UIImagePickerController.ReferenceUrl );
                if( assetURL != null )
                {
                    Action<ALAsset> resultBlock = delegate( ALAsset myasset ) {
                        NSDictionary metadata = myasset.DefaultRepresentation.Metadata;

                        UIImage image = new UIImage( myasset.DefaultRepresentation.GetFullScreenImage() );

                        LoadPhotoBlock( metadata, image );
                    };

                    Action<NSError> failureBlock = delegate( NSError myerror ) {
                        Console.WriteLine( "cant get image - {0}", myerror.LocalizedDescription );

                        LoadPhotoBlock( null, null );
                    };

                    ALAssetsLibrary assetsLib = new ALAssetsLibrary();

                    assetsLib.AssetForUrl( assetURL, resultBlock, failureBlock );
                }
			}

            void LoadPhotoBlock( NSDictionary metadata, UIImage image )
            {
                //@strongify( self )
                this.viewController.DismissViewController(
                    true,
                    delegate {
                        this.viewController.LoadPhoto( image, metadata );
                        this.viewController.SelectCropArea(); //try to get crop
                    }
                    );
            }

			public override void Canceled( UIImagePickerController picker )
			{
                this.viewController.DismissViewController( true, null );
			}
		}

        class DocumentInteractionControllerDelegate : UIDocumentInteractionControllerDelegate
        {
            private PhotoSliderViewController viewController;

            public DocumentInteractionControllerDelegate( PhotoSliderViewController viewController )
            {
                this.viewController = viewController;
            }

			public override UIViewController ViewControllerForPreview( UIDocumentInteractionController controller )
			{
                return this.viewController;
			}
		}

        public PhotoSliderViewController( IntPtr handle ) : base( handle )
        {
        }

		public override void ViewDidLoad()
		{
            base.ViewDidLoad();

            this.SaveButton.Enabled = false;
            this.RotateLeft.Enabled = false;
            this.RotateRight.Enabled = false;

            this.imagePickerController = new UIImagePickerController();
            this.imagePickerController.Delegate = new ImagePickerControllerDelegate( this );

            PxLicenseStatus status = PxLicense.Info.Status;

            if( status != PxLicenseStatus.Active )
            {
                UIAlertView alert = new UIAlertView(
                    "License error",
                    "There is a problem with your license key. Will operate in the demo mode.",
                    (IUIAlertViewDelegate)null,
                    "OK"
                    );

                //this.loadButton.Enabled = false;

                alert.Show();
            }
		}

		public override void ViewDidUnload()
		{
            base.ViewDidUnload();

            this.SaveButton = null;
            this.LoadButton = null;
            this.RotateLeft = null;
            this.RotateRight = null;
            this.ImageView = null;
            this.dic = null;
		}

		partial void SettingsAction( UIBarButtonItem sender )
        {
            SettingsViewController vc = new SettingsViewController();

            vc.ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen;

            PresentViewController( vc, false, null );
        }

        partial void LoadAction( UIBarButtonItem sender )
        {
            //[Weakify( this )]
            UIActionSheet actionSheet = new UIActionSheet( "Please, select image source" );

            actionSheet.AddButton( "Camera" );
            actionSheet.AddButton( "Photo album" );
            actionSheet.AddButton( "Cancel" );

            actionSheet.CancelButtonIndex = 2;

            actionSheet.Clicked += delegate ( object a, UIButtonEventArgs b ) {
                switch( b.ButtonIndex )
                {
                case 0:
                    // load camera view
                    if( UIDevice.CurrentDevice.UserInterfaceIdiom != UIUserInterfaceIdiom.Pad )
                        this.OpenCameraView();
                    else
                    {
                        this.DismissViewController(
                            false,
                            delegate {
                                this.OpenCameraView();
                            }
                            );
                    }

                    break;

                case 1:
                    if( UIDevice.CurrentDevice.UserInterfaceIdiom != UIUserInterfaceIdiom.Pad )
                        this.ShowImagePicker( UIImagePickerControllerSourceType.PhotoLibrary );
                    else
                    {
                        this.DismissViewController(
                            false,
                            delegate {
                                this.ShowImagePicker( UIImagePickerControllerSourceType.PhotoLibrary );
                            }
                            );
                    }

                    break;

                }
            };

            actionSheet.ShowInView( this.View );
        }

        private void OpenCameraView()
        {
            UIAlertView alert = new UIAlertView(
                "Not implemented",
                "Camera is not implemented yet.",
                (IUIAlertViewDelegate)null,
                "OK"
                );

            alert.Show();
        }

        private void ShowImagePicker( UIImagePickerControllerSourceType sourceType )
        {
            if( this.ImageView.IsAnimating )
                this.ImageView.StopAnimating();
    
            if( UIImagePickerController.IsSourceTypeAvailable( sourceType ) )
            {
                this.SetupImagePicker( sourceType );
                this.PresentViewController( imagePickerController, true, null );
            }
        }

        private void SetupImagePicker( UIImagePickerControllerSourceType sourceType )
        {
            imagePickerController.SourceType = sourceType;
            imagePickerController.AllowsEditing = false;
            imagePickerController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
        }

        private void LoadPhoto( UIImage image, string path )
        {
            PxMetaImage metaImage = PxMetaImage.CreateWithPath(image, path);
            
            LoadPhoto( metaImage );
        }

        private void LoadPhoto( UIImage image, NSDictionary metadata )
        {
            PxMetaImage metaImage = PxMetaImage.CreateWithMetadata( image, metadata );

            LoadPhoto( metaImage );
        }

        private void LoadPhoto( PxMetaImage metaImage )
        {
            using( NSAutoreleasePool autoreleasePool = new NSAutoreleasePool() )
            {
                UIImage image = metaImage.Image;
                CGSize maxSuportedSize = PxSDK.SupportedImageSize( image.Size );
                if( image.Size != maxSuportedSize )
                    // Resize input image if its size is greater than the supported maximum
                    metaImage.Image = ImageUtils.ScaleImage( image, maxSuportedSize );

                this.inImage = metaImage;
                this.outImage = null;
                this.ImageView.Image = metaImage.Image;
                this.SaveButton.Enabled = false;
            }
        }

        private bool ProcessCrop( PxMetaImage source, out PxRectangle pts )
        {
            PxDocCorners docCorners;
            bool result = PxSDK.DetectDocumentCorners( source, out docCorners );

            pts.leftTop = docCorners.ptUL;
            pts.rightTop = docCorners.ptUR;
            pts.leftBotton = docCorners.ptBL;
            pts.rightBotton = docCorners.ptBR;

            bool isSmartCropMode = false;

            if( result )
                isSmartCropMode = docCorners.isSmartCropMode;

            return isSmartCropMode;
        }

        private bool SelectCropArea()
        {
            bool smartCrop = ProcessCrop( this.inImage, out points );

            NSUserDefaults defaults = NSUserDefaults.StandardUserDefaults;

            bool b = defaults.BoolForKey( "cropState" );

            if( b && smartCrop )
                Process();
            else
            {
                CGPoint[] cg_points = points.ToCGPointRect();

                PageEditorController pageEditor = new PageEditorController( this.inImage.Image, cg_points );
                pageEditor.Delegate = this;
                pageEditor.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;

                this.PresentViewController( pageEditor, false, null );
            }

            return smartCrop;
        }

        public void PageEditorControllerDidFinishedEditingPage( PageEditorController editor, CGPoint[] pts )
        {
            points = PxRectangle.FromCGPointRect( pts );

            editor.DismissViewController( true, null );

            this.Process();
        }

        public void PageEditorControllerCancel( PageEditorController editor )
        {
            editor.DismissViewController( true, null );
        }

        private void Process()
        {
            PxMetaImage outImg = null;

            using( NSAutoreleasePool autoreleasePool = new NSAutoreleasePool() )
            {
                outImg = ProcessPerspective( this.inImage );

                this.inImage = null;

                if( outImg != null )
                {
                    NSUserDefaults defaults = NSUserDefaults.StandardUserDefaults;

                    int profile = (int)defaults.IntForKey( "selectedProfile" );

                    switch( profile )
                    {
                    case 0:
                        outImg = DoOriginal( outImg );
                        break;
                
                    case 1:
                        outImg = DoBW( outImg );
                        break;
                    
                    case 2:
                        outImg = DoGray( outImg );
                        break;
                    
                    case 3:
                        outImg = DoColor( outImg );
                        break;
                    }
                }
            }
    
            switch( outImg.Orientation )
            {
            case PxMetaImage_Orientation.Normal:
                this.rotationAngle = 0;
                break;
            case PxMetaImage_Orientation.Rotate90:
                this.rotationAngle = 90;
                break;
            case PxMetaImage_Orientation.Rotate180:
                this.rotationAngle = 180;
                break;
            case PxMetaImage_Orientation.Rotate270:
                this.rotationAngle = 270;
                break;
            default:
                this.rotationAngle = 0;
                break;
            }

            if( outImg == null )
                this.ImageView.Image = null;
            else
            {
                this.RotateLeft.Enabled = true;
                this.RotateRight.Enabled = true;
                this.SaveButton.Enabled = true;

                this.ImageView.Image = outImg.Image;
            }

            this.outImage = outImg;
        }

        private PxMetaImage ProcessPerspective( PxMetaImage source )
        {
            // Get image without perspective distortion.
            return PxSDK.CorrectDocument( source, ref points );
        }

        private static PxMetaImage DoOriginal( PxMetaImage source )
        {
            return PxSDK.ImageOriginal( source );
        }

        private static PxMetaImage DoBW( PxMetaImage source )
        {
            return PxSDK.ImageBWBinarization( source );
        }

        private static PxMetaImage DoGray( PxMetaImage source )
        {
            return PxSDK.ImageGrayBinarization( source );
        }

        private static PxMetaImage DoColor( PxMetaImage source )
        {
            return PxSDK.ImageColorBinarization( source );
        }

        partial void RotateLeftAction( UIButton sender )
        {
            RotateOnAngle( -90 );
        }

        partial void RotateRightAction( UIButton sender )
        {
            RotateOnAngle( 90 );
        }

        private void RotateOnAngle( int angle )
        {
            rotationAngle += angle;

            if( rotationAngle < 0 )
                rotationAngle += 360;
            else if( rotationAngle >= 360 )
                rotationAngle -= 360;

            if( rotationAngle == 0 )
                this.outImage.Orientation = PxMetaImage_Orientation.Normal;
            else if( rotationAngle == 90 )
                this.outImage.Orientation = PxMetaImage_Orientation.Rotate90;
            else if( rotationAngle == 180 )
                this.outImage.Orientation = PxMetaImage_Orientation.Rotate180;
            else if( rotationAngle == 270 )
                this.outImage.Orientation = PxMetaImage_Orientation.Rotate270;

            this.ImageView.Image = this.outImage.Image;
        }

        partial void SaveAction( UIBarButtonItem sender )
        {
            if( this.outImage == null )
                return;

            NSUserDefaults defaults = NSUserDefaults.StandardUserDefaults;

            PxImageWriter_Type img_writer_type = PxImageWriter_Type.Jpeg;

            string file_ext = "jpg";

            string uti = "public.jpeg";

            uint save_format = (uint)defaults.IntForKey( "selectedSaveFormat" );

            switch( save_format )
            {
            case 0:
                {
                    img_writer_type = PxImageWriter_Type.Pdf;
                    file_ext = "pdf";
                    uti = "com.adobe.pdf";
                    break;
                }

            case 1:
                {
                    img_writer_type = PxImageWriter_Type.PngExt;
                    file_ext = "pdf";
                    uti = "com.adobe.pdf";
                    break;
                }

            case 2:
                {
                    img_writer_type = PxImageWriter_Type.Tiff;
                    file_ext = "tiff";
                    uti = "public.tiff";
                    break;
                }

            case 3:
                {
                    img_writer_type = PxImageWriter_Type.PngExt;
                    file_ext = "png";
                    uti = "public.png";
                    break;
                }

            //case 4:
            //    {
            //        img_writer_type = PxImageWriter_Type.Jpeg;
            //        break;
            //    }
            }

            PxImageWriter img_writer = PxImageWriter.CreateWithType( img_writer_type );

            string file_name = "image." + file_ext;

            string dir_path = System.IO.Path.GetTempPath();

            string file_local_path = dir_path + file_name;

            string file_local_path2 = file_local_path;
            if( save_format == 1 )
                // PDF from PNG => build PNG file first
                file_local_path2 = file_local_path + ".png";

            if( !img_writer.Open( file_local_path2 ) )
            {
                SysUtils.ShowAlert( this, "Error", "failed to start sequence '{0}'!", file_local_path2 );
                return;
            }

            bool simulate_multi_page_file = defaults.BoolForKey( "simulateMultipageFile" );
            if( save_format > 2 )
                simulate_multi_page_file = false;

            int c = 1;
            if( simulate_multi_page_file && save_format != 1 )
                c += 2;

            string s;

            // Pick original orientation
            PxMetaImage_Orientation original_orientation = this.outImage.Orientation;

            if( save_format == 1 )
                this.outImage.Orientation = PxMetaImage_Orientation.Normal;

            try {
                do {
                    if( (s = img_writer.Write( this.outImage )) == null )
                    {
                        SysUtils.ShowAlert( this, "Error", "failed to write '{0}'!", file_local_path2 );
                        break;
                    }
                } while( --c > 0 );
            } catch( MonoTouchException ) {
                // License error
                s = null;
            }

            img_writer.Close();

            if( s == null )
                return;

            if( save_format == 1 )
            {
                this.outImage.Orientation = original_orientation;

                img_writer = PxImageWriter.CreateWithType( PxImageWriter_Type.Pdf );

                if( !img_writer.Open( file_local_path ) )
                {
                    SysUtils.ShowAlert( this, "Error", "failed to start sequence '{0}'!", file_local_path );
                    return;
                }

                c = 1;
                if(simulate_multi_page_file )
                    c += 2;

                try {
                    do {
                        if( (s = img_writer.WriteFile( file_local_path2, PxImageWriter_FileType.Png, original_orientation )) == null )
                        {
                            SysUtils.ShowAlert( this, "Error", "failed to write '{0}'!", file_local_path );
                            break;
                        }
                    } while( --c > 0 );
                } catch( MonoTouchException ) {
                    // License error
                    s = null;
                }

                img_writer.Close();

                if( s == null )
                    return;
            }

            NSUrl file_url = new NSUrl( file_local_path, false );

            dic = UIDocumentInteractionController.FromUrl( file_url );
            dic.Uti = uti;

            if( !dic.PresentOpenInMenu( CGRect.Empty, this.View, true ) )
            {
                SysUtils.ShowAlert( this, "Error", "failure opening the file sharing dialog" );
                return;
            }

            //self.saveButton.enabled = false;
        }
    }
}
