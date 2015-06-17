using System;
using SpeechAuth.Core.ViewModels.UserCreationPage;
using UIKit;
using SpechAuth.iOS.Views.RecordSoundPage;
using SpeechAuth.Core.ViewModels.RecordSoundPage;
using SpeechAuth.Core.Entities;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace SpechAuth.iOS.Views.UserCreationPage
{
    public partial class UserCreationViewController : MvxViewController<UserCreationPageVM>
    {
        public UserCreationViewController()
        {
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            BindControls ();
        }

        private void BindControls()
        {
            var set = this.CreateBindingSet<UserCreationViewController, UserCreationPageVM> ();
            set.Bind (_surname).To (vm => vm.Surname);
            set.Bind (_name).To (vm => vm.Name);
            set.Bind (_midname).To (vm => vm.MidName);
            set.Bind (_nextStepButton).To (vm => vm.NextCommand);
            set.Apply ();
        }
    }
}

