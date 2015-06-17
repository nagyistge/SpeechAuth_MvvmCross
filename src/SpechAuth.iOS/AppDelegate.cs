using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using SpeechAuth.Views.ProfilesPage;
using SpeechAuth.Views;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Touch.Platform;

namespace SpechAuth.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : MvxApplicationDelegate
    {
        // class-level declarations
        public static UIWindow Window { get; private set; }

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Theme.InitAppearance();

            // create a new window instance based on the screen size
            Window = new UIWindow(UIScreen.MainScreen.Bounds);

            var setup = new Setup(this, Window);
            setup.Initialize();

            var startup = Mvx.Resolve<IMvxAppStart>();
            startup.Start();

//            If you have defined a root view controller, set it here:
//            var rootController = new UINavigationController(new ProfilesViewController());
//            rootController.NavigationBarHidden = false;

//            Window.RootViewController = rootController;
//            RootController = rootController;

            // make the window visible
            Window.MakeKeyAndVisible();
            
            return true;
        }
    }
}

