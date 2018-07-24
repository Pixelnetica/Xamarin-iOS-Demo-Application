using Foundation;
using System;
using UIKit;

namespace DSSDKdemo
{
    public partial class AboutViewController : UIViewController
    {
        private WebViewDelegate webDelegate = new WebViewDelegate();

        public AboutViewController() : base( "AboutViewController", null )
        {
        }

	public override void ViewDidLoad()
	{
            base.ViewDidLoad();

            String plist_path = NSBundle.MainBundle.PathForResource( "Info.plist", null );

            NSDictionary dict = NSDictionary.FromFile( plist_path );

            NSDate buildDate = NSFileManager.DefaultManager.GetAttributes( plist_path ).CreationDate;

            NSDateComponents components = NSCalendar.CurrentCalendar.Components( NSCalendarUnit.Year, buildDate );

            int bulidYear = (int)components.Year;

            string version = NSBundle.MainBundle.ObjectForInfoDictionary( "CFBundleShortVersionString" ).ToString();

            string html = String.Format(
                @"<!DOCTYPE html>
                <html>
                <head>
                <style>
                *{{
                overflow:hidden;
                }}
                body{{
                line-height:1;
                text-align:center;
                font-family: sans-serif;
                }}
                div{{
                background-color:rgb(235,235,235);
                //height:100% !important;
                padding:0.3em;
                border-radius:1em;
                -webkit-border-radius:1em;
                }}
                p{{
                margin-top:0.8em !important;
                margin-bottom:0 !important;
                }}
                </style>
                </head>
                <body>
                <div>
                <p>Pixelnetica Document Scanning SDK
                <p>Version {0}
                <p>For more information, visit
                <p><a target=""_blank"" href=""http://www.pixelnetica.com/products/document-scanning-sdk/mobile-document-capture-sdk.html"">Document Scanning SDK page</a>
                <p>© Pixelnetica {1}
                <p>
                </div>
                </body>
                </html>",
                version,
                bulidYear
                );

            UIWebView webView = this.WebView;

            webView.Delegate = webDelegate;

            webView.LoadHtmlString( html, null );
	}

        partial void CloseView( UIButton sender )
        {
            DismissViewController( true, null );
        }
    }

    class WebViewDelegate : UIWebViewDelegate
    {
		public override bool ShouldStartLoad( UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType )
		{
            if( navigationType == UIWebViewNavigationType.LinkClicked )
            {
                NSUrl url = request.Url;
                if( UIApplication.SharedApplication.CanOpenUrl( url ) )
                    UIApplication.SharedApplication.OpenUrl( url );
                return false;
            }

            return true;
        }
    }
}
