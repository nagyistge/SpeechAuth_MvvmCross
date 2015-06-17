using UIKit;
using Cirrious.MvvmCross.Touch.Views.Presenters;

namespace SpechAuth.iOS
{
    public class CustomSpeechAuthPresenter : MvxTouchViewPresenter
	{
        public CustomSpeechAuthPresenter(UIApplicationDelegate applicationDelegate, UIWindow window) 
            : base(applicationDelegate, window)
        {
        }

        public override void Show(Cirrious.MvvmCross.Touch.Views.IMvxTouchView view)
        {
            var viewController = view as UIViewController;

            if (MasterNavigationController == null)
            {
                base.Show(view);

                if (viewController != null && viewController.NavigationItem != null)
                    viewController.NavigationItem.BackBarButtonItem = new UIBarButtonItem("", UIBarButtonItemStyle.Plain, (s, e) => MasterNavigationController.PopViewController(true));
                
                return;
            }

            viewController.NavigationItem.BackBarButtonItem = new UIBarButtonItem("", UIBarButtonItemStyle.Plain, (s, e) => MasterNavigationController.PopViewController(true));
            MasterNavigationController.PushViewController(viewController, true);
        }
	}

}