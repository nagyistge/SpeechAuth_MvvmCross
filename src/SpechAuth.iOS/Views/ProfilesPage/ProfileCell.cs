using System;
using SpeechAuth.Core.ViewModels.ProfilesPage;
using UIKit;
using Foundation;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace SpeechAuth.Views.ProfilesPage
{
    public partial class ProfileCell : MvxTableViewCell
    {
        public static readonly UINib Nib = UINib.FromName ("ProfileCell", NSBundle.MainBundle);
        public static readonly NSString Key = new NSString ("ProfileCell");

        public const string BindingText = @"
UserName Name;";

        public ProfileCell ()
            : base (BindingText)
        {
            
        }

        public ProfileCell (IntPtr handle) 
            : base (BindingText, handle)
        {
        }

        public string UserName {
            get { return this.UserNameLabel.Text; }
            set { this.UserNameLabel.Text = value; }
        }

        public static ProfileCell Create ()
        {
            return (ProfileCell)Nib.Instantiate (null, null) [0];
        }
    }
}

