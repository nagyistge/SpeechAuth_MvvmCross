using System;
using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;
using SpeechAuth.Core.ViewModels.RecordSoundPage;
using SpeechAuth.Core.Entities;

namespace SpeechAuth.Core.ViewModels.UserCreationPage
{
    public class UserCreationPageVM : MvxViewModel
    {
        private String _surname;
        public String Surname {
            get { return _surname; }
            set {
                _surname = value;
                RaisePropertyChanged (() => Surname);
            }
        }

        private String _name;
        public String Name {
            get { return _name; }
            set {
                _name = value;
                RaisePropertyChanged (() => Name);
            }
        }

        private String _midName;
        public String MidName 
        {
            get { return _midName; }
            set { _midName = value; RaisePropertyChanged (() => MidName); }
        }

        private ICommand _nextCommand;
        public ICommand NextCommand {
            get { 
                return _nextCommand ??
                (_nextCommand = new MvxCommand (() => 
                        {
                            if (!string.IsNullOrEmpty(Surname) && !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(MidName))
                                ShowViewModel<RecordSoundPageVM> (new {
                                        mode = PageMode.UserCreation, 
                                        sn = Surname,
                                        n = Name,
                                        mn = MidName
                                    }
                                );
                        }
                    )
                );
            }
        }

        public UserCreationPageVM()
        {
            
        }
    }
}

