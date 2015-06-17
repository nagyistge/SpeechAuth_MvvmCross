using System;
using SpeechAuth.Core.Entities;
using Cirrious.MvvmCross.ViewModels;

namespace SpeechAuth.Core.ViewModels.ProfilesPage
{
	public class ProfileItemVM : MvxViewModel
	{
        User _user;

        public User User { 
            get { return _user; } 
            set { _user = value; RaisePropertyChanged(() => User); } 
        }

        private string _name;
        public string Name {
            get { return _name; }
            set { _name = value; RaisePropertyChanged(() => Name); }
        }

        public ProfileItemVM(User user)
        {
            User = user;

            Name = User.Surname + " " + User.Name.ToUpperInvariant()[0] + "." + User.MidName.ToUpperInvariant()[0];
        }
	}
}

