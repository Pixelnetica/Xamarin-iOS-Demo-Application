using Foundation;
using System;
using UIKit;

namespace DSSDKdemo
{
    public partial class SettingsViewController : UIViewController
    {
        private static readonly string CELL_ID = "settingsCell";
        private static readonly string PROFILE_CELL_ID = "settingsProfileCell";
        private static readonly string TEXT_PROFILE_CELL_ID = "textSettingsProfileCell";

        public SettingsViewController() : base( "SettingsViewController", null )
        {
        }

        public override void ViewDidLoad()
		{
            /*
            {
                var topView = this.NavBar;
                var bottomView = this.TableView;

                var layoutGuide = this.View.SafeAreaLayoutGuide;
                if (layoutGuide == null)
                {
                    topView.TopAnchor.ConstraintEqualTo(this.View.TopAnchor).Active = true;
                    bottomView.BottomAnchor.ConstraintEqualTo(this.View.BottomAnchor).Active = true;
                }
                else
                {
                    topView.TopAnchor.ConstraintEqualTo(layoutGuide.TopAnchor).Active = true;
                    bottomView.BottomAnchor.ConstraintEqualTo(layoutGuide.BottomAnchor).Active = true;
                }
            }
            */

            base.ViewDidLoad();

            UITableView tableView = this.TableView;

            tableView.RegisterNibForCellReuse( UINib.FromName( "SettingsTableViewCell", null ), CELL_ID );
            tableView.RegisterNibForCellReuse( UINib.FromName( "SettingsProfileTableViewCell", null ), PROFILE_CELL_ID );
            tableView.RegisterNibForCellReuse( UINib.FromName( "TextTableViewCell", null ), TEXT_PROFILE_CELL_ID );

            tableView.Source = new TableViewSource( this );
        }

		public override void ViewDidUnload()
		{
            this.TableView.Source = null;

            base.ViewDidUnload();
        }

        partial void CloseView( UIButton sender )
        {
            DismissViewController( true, null );
        }

        private class TableViewSource : UITableViewSource 
        {
            private UIViewController viewController;

            public TableViewSource( UIViewController viewController )
            {
                this.viewController = viewController;
            }

            public override nint NumberOfSections( UITableView tableView )
            {
                return 7;
            }

            public override string TitleForHeader( UITableView tableView, nint section )
            {
                switch( section )
                {
                case 0:
                    return "Crop settings";

                case 2:
                    return "Smart Camera";

                case 3:
                    return "AutoShot settings";

                case 4:
                    return "Save format";

                case 6:
                    return "About";
                }

                return "";
            }

            public override string TitleForFooter( UITableView tableView, nint section )
            {
                switch( section )
                {
                case 0:
                    return "Turn «SmartCrop» ON to automatically process images with well detected borders. Manual correction will be offered only in case of low quality document border detection. When «SmartCrop» is OFF - image will always be processed after manual confirmation.";

                case 5:
                    return "The «Simulate multi-page file» setting simulates multi-page files by writing the same image three times in a row. Applicable for PDF files only.";

                default:
                    return "";
                }
            }

            public override nint RowsInSection( UITableView tableView, nint section )
            {
                switch( section )
                {
                case 0:
                    return 1;

                case 1:
                    return 4;

                case 2:
                    return 2;

                case 3:
                    return 1;

                case 4:
                    return 5;

                default:
                    return 1;
                }
            }

            public override UITableViewCell GetCell( UITableView tableView, NSIndexPath indexPath )
            {
                SettingsTableViewCell cell;
                SettingsProfileTableViewCell cell2;
                TextTableViewCell cell3;

                int section = indexPath.Section;
                int row = indexPath.Row;

                if( section == 0 )
                {
                    if( row == 0 )
                    {
                        cell = (SettingsTableViewCell)tableView.DequeueReusableCell( CELL_ID, indexPath );
                        cell.Tag = 100;
                        cell.UpdateLabelText();

                        return cell;
                    }
                    else if( row == 1 )
                    {
                        cell = (SettingsTableViewCell)tableView.DequeueReusableCell( CELL_ID, indexPath );
                        cell.Tag = 101;
                        cell.UpdateLabelText();

                        return cell;
                    }
                }
                else if( section == 1 )
                {
                    cell2 = (SettingsProfileTableViewCell)tableView.DequeueReusableCell( PROFILE_CELL_ID, indexPath );

                    NSUserDefaults defaults = NSUserDefaults.StandardUserDefaults;

                    nint row_ = defaults.IntForKey( "selectedProfile" );

                    if( row_ == row )
                        cell2.Accessory = UITableViewCellAccessory.Checkmark;
                    else
                        cell2.Accessory = UITableViewCellAccessory.None;

                    switch( row )
                    {
                    case 0:
                        cell2.Configure( "profile_button_original", "Original" );
                        break;
                    case 1:
                        cell2.Configure( "profile_button_bw", "Black & White" );
                        break;
                    case 2:
                        cell2.Configure( "profile_button_gray", "Gray" );
                        break;
                    case 3:
                        cell2.Configure( "profile_button_color", "Color" );
                        break;
                    }

                    return cell2;
                }
                else if( section == 2 )
                {
                    cell = (SettingsTableViewCell)tableView.DequeueReusableCell( CELL_ID, indexPath );
                    cell.Accessory = UITableViewCellAccessory.None;

                    if( row == 0 )
                        cell.Tag = 102;
                    else
                        cell.Tag = 103;

                    cell.UpdateLabelText();

                    return cell;
                }
                else if( section == 3 )
                {
                    cell3 = (TextTableViewCell)tableView.DequeueReusableCell( TEXT_PROFILE_CELL_ID, indexPath );
                    cell3.Accessory = UITableViewCellAccessory.None;

                    if( row == 0 )
                        cell3.Tag = 104;

                    cell3.UpdateLabelText();

                    return cell3;
                }
                else if( section == 4 )
                {
                    cell2 = (SettingsProfileTableViewCell)tableView.DequeueReusableCell( PROFILE_CELL_ID, indexPath );

                    NSUserDefaults defaults = NSUserDefaults.StandardUserDefaults;

                    nint row_ = defaults.IntForKey( "selectedSaveFormat" );

                    cell2.Accessory =
                        row_ == row ?
                        UITableViewCellAccessory.Checkmark :
                        UITableViewCellAccessory.None
                        ;

                    switch( row )
                    {
                    case 0:
                        cell2.Configure( "save_format_button_pdf", "PDF" );
                        break;

                    case 1:
                        {
                            cell2.Configure( "save_format_button_pdf_from_png", "PDF from PNG" );
                            break;
                        }

                    case 2:
                        {
                            cell2.Configure( "save_format_button_tiff", "TIFF G4" );
                            break;
                        }

                    case 3:
                        {
                            cell2.Configure( "save_format_button_png", "PNG" );
                            break;
                        }

                    case 4:
                        {
                            cell2.Configure( "save_format_button_jpg", "JPG" );
                            break;
                        }
                    }

                    return cell2;
                }
                else if( section == 5 )
                {
                    cell = (SettingsTableViewCell)tableView.DequeueReusableCell( CELL_ID, indexPath );
                    cell.Accessory = UITableViewCellAccessory.None;
                    cell.Tag = 104;

                    cell.UpdateLabelText();

                    return cell;
                }
                else if( section == 6 )
                {
                    cell2 = (SettingsProfileTableViewCell)tableView.DequeueReusableCell( PROFILE_CELL_ID, indexPath );
                    cell2.Accessory = UITableViewCellAccessory.None;
                    cell2.Configure( "", "About product" );

                    return cell2;
                }

                return null;
            }

            public override void RowSelected( UITableView tableView, NSIndexPath indexPath )
            {
                int section = indexPath.Section;
                int row = indexPath.Row;

                switch( section )
                {
                case 1:
                    {
                        foreach( UITableViewCell c in tableView.VisibleCells )
                        {
                            c.Accessory = UITableViewCellAccessory.None;
                        }

                        tableView.DeselectRow( indexPath, true );

                        SettingsProfileTableViewCell cell = (SettingsProfileTableViewCell)tableView.CellAt( indexPath );

                        cell.Accessory = UITableViewCellAccessory.Checkmark;

                        NSUserDefaults defaults = NSUserDefaults.StandardUserDefaults;

                        defaults.SetInt( row, "selectedProfile" );
                        defaults.Synchronize();

                        break;
                    }

                case 4:
                    {
                        foreach( UITableViewCell c in tableView.VisibleCells )
                        {
                            c.Accessory = UITableViewCellAccessory.None;
                        }

                        tableView.DeselectRow( indexPath, true );

                        SettingsProfileTableViewCell cell = (SettingsProfileTableViewCell)tableView.CellAt( indexPath );

                        cell.Accessory = UITableViewCellAccessory.Checkmark;

                        NSUserDefaults defaults = NSUserDefaults.StandardUserDefaults;

                        defaults.SetInt( row, "selectedSaveFormat" );
                        defaults.Synchronize();

                        break;
                    }

                case 6:
                    {
                        AboutViewController vc = new AboutViewController();

                        vc.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;

                        viewController.PresentViewController( vc, true, null );

                        break;
                    }

                default:
                    {
                        tableView.DeselectRow( indexPath, false );
                        break;
                    }
                }
            }
        }
    }
}
