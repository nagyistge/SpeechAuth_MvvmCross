using System;
using Cirrious.MvvmCross.ViewModels;

namespace SpeechAuth.Core.ViewModels.ResultPage
{
    public class ResultPageVM : MvxViewModel
    {
        private String _message;
        public String Message {
            get { return _message; }
            set {
                _message = value;
                RaisePropertyChanged (() => Message);
            }
        }

        private Boolean _succeeded;
        public Boolean Succeeded {
            get { return _succeeded; }
            set {
                _succeeded = value;
                RaisePropertyChanged (() => Succeeded);
            }
        }

        public void Init (bool succeeded, string message)
        {
            Succeeded = succeeded;
            Message = message;
        }

        public ResultPageVM()
        {
            
        }
    }
}

