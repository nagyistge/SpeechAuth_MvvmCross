using System;
using SpeechAuth.Core.ViewModels.RecordSoundPage;
using AVFoundation;
using Foundation;
using System.IO;
using UIKit;
using SpeechAuth.Core.ViewModels.ResultPage;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace SpechAuth.iOS.Views.RecordSoundPage
{
    public partial class RecordSoundViewController : MvxViewController<RecordSoundPageVM>
    {
        private AVAudioRecorder _recorder;

        private NSError _error = new NSError(new NSString("error"), 1);
        private NSUrl _url;

        private AudioSettings _settings;

        private string _audioFilePath;

        public RecordSoundViewController()
        {
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            BindControls ();
        }

        private void BindControls()
        {
            Title = ViewModel.Title;

            foreach (var item in ViewModel.StepIndicators)
                StepIndicators.AddIndicator ();

            var set = this.CreateBindingSet<RecordSoundViewController, RecordSoundPageVM> ();
            set.Bind (_secretWord).To (vm => vm.SecretWord);
            set.Bind (StepIndicators).For("Indicators").To (vm => vm.StepIndicators);
            set.Apply ();

            _startRecord.TouchUpInside += (sender, e) => StartRecord();
            _stopRecord.TouchUpInside += (sender, e) => StopRecord();

            InitializeRecorder();
        }

        private void InitializeRecorder()
        {
            //Declare string for application temp path and tack on the file extension
            string fileName = "code_phrase.wav";
            _audioFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), fileName);

            _url = NSUrl.FromFilename(_audioFilePath);
            NSObject[] values = new NSObject[]
            {
                NSNumber.FromFloat (16000.0f), //Sample Rate
                NSNumber.FromInt32 ((int)AudioToolbox.AudioFormatType.LinearPCM), //AVFormat
                NSNumber.FromInt32 (1), //Channels
                NSNumber.FromInt32 (16), //PCMBitDepth
                NSNumber.FromBoolean (false), //IsBigEndianKey
                NSNumber.FromBoolean (false) //IsFloatKey
            };

            //Set up the NSObject Array of keys that will be combined with the values to make the NSDictionary
            NSObject[] keys = new NSObject[]
            {
                AVAudioSettings.AVSampleRateKey,
                AVAudioSettings.AVFormatIDKey,
                AVAudioSettings.AVNumberOfChannelsKey,
                AVAudioSettings.AVLinearPCMBitDepthKey,
                AVAudioSettings.AVLinearPCMIsBigEndianKey,
                AVAudioSettings.AVLinearPCMIsFloatKey
            };

            //Set Settings with the Values and Keys to create the NSDictionary
            _settings = new AudioSettings(NSDictionary.FromObjectsAndKeys (values, keys));

            //Set recorder parameters
            _recorder = AVAudioRecorder.Create(_url, _settings, out _error);

            //Set Recorder to Prepare To Record
            _recorder.PrepareToRecord();
        }

        public void StartRecord()
        {
            StartAnimation ();
            var session = AVAudioSession.SharedInstance ();

            // added for iOS privacy permission requests
            if (UIDevice.CurrentDevice.CheckSystemVersion (8, 0))
                session.RequestRecordPermission(delegate (bool granted) { });

            session.SetCategory (AVAudioSession.CategoryRecord, out _error);
            if (_error != null) {
                Console.WriteLine (_error);
                return;
            }
            session.SetActive (true, out _error);
            if (_error != null) {
                Console.WriteLine (_error);
                return;
            }

            _recorder.Record ();
        }

        public void StopRecord()
        {
            _recorder.Stop ();

            StopAnimation();

            ViewModel.StopRecord.Execute (GetShortArrayFromBytes ());
        }

        private short[] GetShortArrayFromBytes()
        {
            if (!File.Exists(_audioFilePath))
                return null;

            byte[] buffer = File.ReadAllBytes(_audioFilePath);

            if (buffer == null || buffer.Length == 0)
                return null;

            int startByte = 0;

            //ищем начало блока данных
            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] == 'd' && buffer[i + 1] == 'a' && buffer[i + 2] == 't' && buffer[i + 3] == 'a')
                {
                    startByte = i + 8; //+16??
                    break;
                }
            }

            if (startByte == buffer.Length)
                return null;

            // получаем вещественное число из 16 / 8 = 2 байт
            var data = new short[buffer.Length / 2];                    
            for (int s = startByte, i = 0; s + 1 < buffer.Length; s = s + 2, i++)
                data[i] = BitConverter.ToInt16(buffer, s);

            return data;
        }

        private System.Timers.Timer _timer;
        private int _index = 1;
        private bool _increment = true, _decrement = false;
        private void StartAnimation ()
        {
            _timer = new System.Timers.Timer (41) {
                Interval = 41,
                Enabled = true
            };
            _timer.Elapsed += (sender, e) => InvokeOnMainThread (() => {
                if (_increment && !_decrement) {
                    _micro.Image = UIImage.FromFile (string.Format ("Images/Animation/{0}.png", _index));
                    _index++;
                    _decrement = _index == 24;
                    _increment = _index != 24;
                } 
                else if (_decrement && !_increment) {
                    _micro.Image = UIImage.FromFile (string.Format ("Images/Animation/{0}.png", _index));
                    _index--;
                    _increment = _index == 1;
                    _decrement = _index != 1;
                }
            });
        }

        private void StopAnimation ()
        {
            _timer.Dispose ();
            _timer = null;

            _index = 1;
            _increment = true;
            _decrement = false;

            _micro.Image = UIImage.FromFile ("Images/Animation/1.png");
        }
    }
}

