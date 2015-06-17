using System;
using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;
using System.Collections.ObjectModel;
using SpeechAuth.Core.Entities;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using SpeechAuth.Core.Messages;
using SpeechAuth.Core.ViewModels.RecordSoundPage;
using SpeechAuth.Core.ViewModels.GraphsPage;
using SpeechAuth.Core.ViewModels.UserCreationPage;
using SpeechAuth.Core.Store;
using System.Collections.Generic;

namespace SpeechAuth.Core.ViewModels.ProfilesPage
{    
    public class ProfilesPageVM : MvxViewModel
    {
        public ICommand FourierButton { get { return new MvxCommand (() => ShowViewModel<RecordSoundPageVM> (new { mode = PageMode.AuthorizationFourier })); } }
        public ICommand WaveletButton { get { return new MvxCommand (() => ShowViewModel<RecordSoundPageVM> (new { mode = PageMode.AuthorizationWavelet })); } }

        public ICommand AddButton { get { return new MvxCommand (StartCreateUser); } }

        private ObservableCollection<ProfileItemVM> _items;
        public ObservableCollection<ProfileItemVM> Items
        {
            get { return _items; }
            set { _items = value; RaisePropertyChanged(() => Items); }
        }

        public ICommand ShowFourierGraph { get { return new MvxCommand (() => ShowGraphPage(PageMode.GraphsFourier)); } }
        public ICommand ShowWaveletGraph { get { return new MvxCommand (() => ShowGraphPage ()); } }

        private readonly MvxSubscriptionToken _token;

        public ProfilesPageVM (ILocalStoreService localStore, IMvxMessenger messenger)
        {
            _localStore = localStore;
            _token = messenger.Subscribe<UserCreatedMessage> (OnUserCreated);

            Items = new ObservableCollection<ProfileItemVM> ();
            UpdateItemsSource ();
        }

        private void StartCreateUser ()
        {
            ShowViewModel<UserCreationPageVM> ();
        }

        private async void UpdateItemsSource ()
        {
            List<UserInfo> users = await _localStore.LoadUsersInfo ();
            foreach (var user in users)
                Items.Add (new ProfileItemVM (user.User));
        }

        private void ShowGraphPage (PageMode pageMode = PageMode.GraphsWavelet)
        {
            ShowViewModel<GraphsPageVM> (new { mode = pageMode });
        }

        private void OnUserCreated(UserCreatedMessage msg)
        {
            InvokeOnMainThread (() =>
                Items.Add(new ProfileItemVM (msg.User)));
        }

        private readonly ILocalStoreService _localStore;
    }
}

