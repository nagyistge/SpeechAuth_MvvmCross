using System;
using SpeechAuth.Core.Entities;
using System.Collections.Generic;
using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Cirrious.MvvmCross.Plugins.Messenger;
using System.Threading.Tasks;
using SpeechAuth.Core.Helpers;
using SpeechAuth.Core.Messages;
using SpeechAuth.Core.ViewModels.ResultPage;

namespace SpeechAuth.Core.ViewModels.RecordSoundPage
{
    public class RecordSoundPageVM : MvxViewModel
    {
        private ICommand _stopRecord;
        public ICommand StopRecord {
            get { return _stopRecord ?? (_stopRecord = new MvxCommand<short[]> (StopRecordAction)); }
        }

        private ObservableCollection<bool> _stepIndicators;
        public ObservableCollection<bool> StepIndicators { 
            get { return _stepIndicators; } 
            set {
                _stepIndicators = value;
                RaisePropertyChanged (() => StepIndicators);
            } 
        }

        private String _secretWord;
        public String SecretWord {
            get { return _secretWord; }
            set {
                _secretWord = value;
                RaisePropertyChanged (() => SecretWord);
            }
        }

        private String _title;
        public String Title {
            get { return _title; }
            set {
                _title = value;
                RaisePropertyChanged (() => Title);
            }
        }

        internal PageMode Mode { get; private set; }
        internal User User { get; private set; }

        private readonly IMvxMessenger _messenger;

        public void Init (PageMode mode, string sn, string n, string mn)
        {
            Mode = mode;

            if (sn != null && n != null && mn != null)
                User = new User { Surname = sn, Name = n, MidName = mn };

            Title = (Mode == PageMode.AuthorizationFourier || Mode == PageMode.AuthorizationWavelet) ? "Авторизация" : "Запись голоса";

            if (Mode == PageMode.UserCreation)
            {
                UpdateIndicatorSource();
                SelectSecretWord();
            }
        }

        public RecordSoundPageVM(IMvxMessenger messenger)
        {
            _messenger = messenger;

            StepIndicators = new ObservableCollection<bool>();
        }

        private int _index = 0;
        void SelectSecretWord()
        {
            if (_index < Words.Items.Count) {
                SecretWord = string.Format ("Произнесите:\n{0}", Words.Items [_index]);
                _index++;
            }
        }

        void StopRecordAction (short[] shortArray)
        {
            if (Mode == PageMode.UserCreation)
                StopCreation (shortArray);
            else
                StopAuthorization (shortArray);            
        }

        private List<double[]> _arrF = new List<double[]>();
        private List<double[]> _arrW = new List<double[]>();
        private int _qwerty = 0;
        async void StopCreation(short[] array)
        {
            var time = DateTime.Now;

            var fourier = Task.Run<double[]>(() => MainHandler.GetMelKreps(array, Transform.Fourier));
            var wavelet = Task.Run<double[]>(() => MainHandler.GetMelKreps(array, Transform.Wavelet));

            await Task.WhenAll<double[]> (new [] { fourier, wavelet });

            var tempF = fourier.Result;
            _arrF.Add(tempF);

            var tempW = wavelet.Result;
            _arrW.Add (tempW);

            var delta = DateTime.Now - time;
            System.Diagnostics.Debug.WriteLine ("Compute time {0} s".FormatWith (delta.TotalSeconds));

            InvokeOnMainThread (() => {
                StepIndicators [_qwerty] = true;
                RaisePropertyChanged(() => StepIndicators);

                _qwerty++;
                if (_qwerty == 10) 
                {
                    MainHandler.AddUser (User, _arrF, _arrW);
                    _qwerty = 0;
                    ShowViewModel<ResultPageVM> (new { succeeded = true, message = "Пользователь успешно создан!" });
                }

                SelectSecretWord ();
            });
        }

        async void StopAuthorization(short[] array)
        {
            var time = DateTime.Now;

            if (Mode == PageMode.AuthorizationFourier) 
            {
                var result = await MainHandler.CheckVoice (array, Transform.Fourier);

                var delta = DateTime.Now - time;
                System.Diagnostics.Debug.WriteLine ("Compute time {0} s".FormatWith (delta.TotalSeconds));

                if (result != "UNKNOWN")
                    ShowViewModel<ResultPageVM> (new { succeeded = true, message = string.Format ("Возможный пользователь: {0}", result)});
                else 
                    ShowViewModel<ResultPageVM> (new { succeeded = false, message = "Пользователь не найден"});
            }
            else if (Mode == PageMode.AuthorizationWavelet)
            {
                var result = await MainHandler.CheckVoice (array, Transform.Wavelet);

                var delta = DateTime.Now - time;
                System.Diagnostics.Debug.WriteLine ("Compute time {0} s".FormatWith (delta.TotalSeconds));

                if (result != "UNKNOWN")
                    ShowViewModel<ResultPageVM> (new { succeeded = true, message = string.Format ("Возможный пользователь: {0}", result) });
                else
                    ShowViewModel<ResultPageVM> (new { succeeded = false, message = "Пользователь не найден" });
            }
        }

        void UpdateIndicatorSource()
        {
            var dataSource = new ObservableCollection<bool>();
            for (int i = 0; i < 10; i++)
                dataSource.Add(false);
            StepIndicators = dataSource;
        }
    }
}

