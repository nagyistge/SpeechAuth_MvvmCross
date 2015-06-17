using System;
using SpeechAuth.Core.ViewModels.ProfilesPage;
using UIKit;
using SpechAuth.iOS;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Foundation;

namespace SpeechAuth.Views.ProfilesPage
{
    public partial class ProfilesViewController : MvxViewController<ProfilesPageVM>
    {
        public ProfilesViewController()
        {
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            BindControls ();
        }

        public override void ViewWillDisappear (bool animated)
        {
            HideAuthButtons ();
            base.ViewWillDisappear (animated);
        }

        protected void BindControls()
        {
            var source = new TableSource (_items);

            var set = this.CreateBindingSet<ProfilesViewController, ProfilesPageVM>();
            set.Bind(source).To(vm => vm.Items);
            set.Bind (_addButton).To (vm => vm.AddButton);
            set.Bind (_waveletButton).To (vm => vm.WaveletButton);
            set.Bind (_fourierButton).To (vm => vm.FourierButton);
            set.Apply ();

            _items.RowHeight = 53; 
            _items.Source = source;
            _items.ReloadData();

            var rightItem = new UIBarButtonItem (UIImage.FromFile ("Images/GraphIcon.png"), UIBarButtonItemStyle.Plain, null);
            rightItem.Clicked += ShowGraphsDialog;
            NavigationItem.SetRightBarButtonItem (rightItem, false);

            _authButton.TouchUpInside += OnAuthButtonTouch;
        }

        private bool _enabled = false;
        private void OnAuthButtonTouch (object sender, EventArgs e)
        {
            if (ViewModel.Items.Count > 0) {
                if (!_enabled)
                    ShowAuthButtons ();
                else                    
                    HideAuthButtons ();                
            }
        }

        public void ShowAuthButtons ()
        {
            _enabled = true;

            UIView.Animate (0.3, () => {
                _waveletButton.Frame = new CoreGraphics.CGRect (41, View.Frame.Height - 227, 52.5f, 52.5f);
                _waveletButton.Alpha = 1;
            });

            UIView.Animate (0.2, () => {
                _fourierButton.Frame = new CoreGraphics.CGRect (41, View.Frame.Height - 161.5f, 52.5f, 52.5f);
                _fourierButton.Alpha = 1;
            });
        }

        public void HideAuthButtons ()
        {
            if (View == null || _fourierButton == null || _waveletButton == null)
                return;

            _enabled = false;

            UIView.Animate (0.4, () => {
                _fourierButton.Frame = new CoreGraphics.CGRect (41, View.Frame.Height - 100, 52.5f, 52.5f);
                _fourierButton.Alpha = 0;
            });

            UIView.Animate (0.2, () => {
                _waveletButton.Frame = new CoreGraphics.CGRect (41, View.Frame.Height - 150, 52.5f, 52.5f);
                _waveletButton.Alpha = 0;
            });
        }

        private void ShowGraphsDialog (object sender, EventArgs e)
        {
            ShowGraphDialog ();
        }

        private UIView _shadow;
        private UIImageView _dialog;
        public void ShowGraphDialog ()
        {
            var window = AppDelegate.Window;

            var xOffset = (window.Frame.Width - 305) / 2;
            var yOffset = (window.Frame.Height - 275) / 2;

            window.AddSubviews (
                _shadow,
                _dialog
            );

            UIView.Animate (0.3, () => {
                _shadow.Alpha = 1;
                _dialog.Frame = new CoreGraphics.CGRect (xOffset, yOffset, 305, 275);
            });
        }

        public void HideGraphDialog (Action callback)
        {
            var window = AppDelegate.Window;

            var xOffset = (window.Frame.Width - 305) / 2;

            UIView.Animate (0.3, () => {
                _shadow.Alpha = 0;
                _dialog.Frame = new CoreGraphics.CGRect (xOffset, window.Frame.Height, 305, 275);
            }, () => {
                _shadow.RemoveFromSuperview ();
                _dialog.RemoveFromSuperview ();

                if (callback != null)
                    callback ();
            });
        }

        public class TableSource : MvxTableViewSource
        {
            public TableSource (UITableView tableView)
                : base(tableView)
            {
                UseAnimations = true;
                AddAnimation = UITableViewRowAnimation.Top;
                RemoveAnimation = UITableViewRowAnimation.Middle;

                tableView.RegisterNibForCellReuse(ProfileCell.Nib, ProfileCell.Key);
            }

            public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
            {
                return 53;
            }

            protected override UITableViewCell GetOrCreateCellFor (UITableView tableView, NSIndexPath indexPath, object item)
            {
                return (UITableViewCell)tableView.DequeueReusableCell(ProfileCell.Key, indexPath);
            }
        }
    }
}

