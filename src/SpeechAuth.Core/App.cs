using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore;
using SpeechAuth.Core.Store;

namespace SpeechAuth.Core
{
    public class App : Cirrious.MvvmCross.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();
            
            Mvx.ConstructAndRegisterSingleton<ILocalStoreService, LocalStoreService>();

            RegisterAppStart<ViewModels.ProfilesPage.ProfilesPageVM>();
        }
    }
}