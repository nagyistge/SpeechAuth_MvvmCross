using System;
using SpeechAuth.Core.ViewModels.ResultPage;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using UIKit;

namespace SpechAuth.iOS.Views.ResultPage
{
    public partial class ResultViewController : MvxViewController<ResultPageVM>
    {
        public ResultViewController()
        {
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            BindControls ();
        }

        private void BindControls ()
        {
            Title = "Результат";

            NavigationController.SetNavigationBarHidden (ViewModel.Message.Contains ("успешно создан"), false);

            var set = this.CreateBindingSet<ResultViewController, ResultPageVM> ();
            set.Bind (_message).To (vm => vm.Message);
            set.Apply ();

            _toRootButton.Hidden = !ViewModel.Message.Contains ("успешно создан");
            _toRootButton.TouchUpInside += (sender, e) => ShowRootPage();

            View.AddSubview (
                new UIImageView (ViewModel.Succeeded ? UIImage.FromFile ("Images/Success.png") : UIImage.FromFile ("Images/Error.png"))
                .WithFrame ((View.Frame.Width - 104) / 2, 125, 104, 104)
            );
        }

        public void ShowRootPage ()
        {
            NavigationController.SetNavigationBarHidden (false, true);
            NavigationController.PopToRootViewController (true);
        }
    }
}

