using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Touch.Platform;
using UIKit;
using Cirrious.CrossCore;
using SpeechAuth.Core.ViewModels.ProfilesPage;
using SpeechAuth.Views.ProfilesPage;
using SpeechAuth.Core.ViewModels.RecordSoundPage;
using SpechAuth.iOS.Views.RecordSoundPage;
using SpeechAuth.Core.ViewModels.GraphsPage;
using SpechAuth.iOS.Views.GraphsPage;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using SpechAuth.iOS.Views.RecordSoundPage.Controllers;

namespace SpechAuth.iOS
{
	public class Setup : MvxTouchSetup
	{
		public Setup(MvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
		{
		}

		protected override IMvxApplication CreateApp()
		{
            return new SpeechAuth.Core.App();
		}
		
        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

        protected override void InitializeFirstChance ()
        {
            Mvx.ConstructAndRegisterSingleton<IGraphsPageView, GraphsViewController>();
            base.InitializeFirstChance ();
        }

        protected override void FillTargetFactories (Cirrious.MvvmCross.Binding.Bindings.Target.Construction.IMvxTargetBindingFactoryRegistry registry)
        {
            registry.RegisterCustomBindingFactory<UIStepIndicators> (
                "Indicators",
                indicators => new UIStepIndicatorsWithListBinder(indicators)
            );
            base.FillTargetFactories (registry);
        }

        protected override Cirrious.MvvmCross.Touch.Views.Presenters.IMvxTouchViewPresenter CreatePresenter ()
        {
            return new CustomSpeechAuthPresenter((MvxApplicationDelegate)ApplicationDelegate, Window);
        }
	}
}