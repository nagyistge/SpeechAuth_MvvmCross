using System;
using SpeechAuth.Core.ViewModels.GraphsPage;
using AVFoundation;
using UIKit;
using System.IO;
using Foundation;
using CorePlot;
using CoreGraphics;
using System.Linq;
using System.Collections.Generic;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace SpechAuth.iOS.Views.GraphsPage
{
    public partial class GraphsViewController : MvxViewController<GraphsPageVM>, IGraphsPageView
    {
        private AVAudioRecorder _recorder;

        private NSError _error = new NSError(new NSString("error"), 1);
        private NSUrl _url;

        private AudioSettings _settings;

        private string _audioFilePath;

        public GraphsViewController ()
        {
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            BindControls ();
        }

        private void BindControls ()
        {
            Title = ViewModel.Title;

            _startRecord.TouchUpInside += (sender, e) => StartRecord();
            _stopRecord.TouchUpInside += (sender, e) => StopRecord();

            var set = this.CreateBindingSet<GraphsViewController, GraphsPageVM> ();
//            _loading.RemoveFromSuperview ();
            set.Bind (_loading).For (v => v.Hidden).To (vm => vm.LoadingHidden);
            set.Apply ();
//            _loading.BindFrame (ViewModel.Loading.InjectManualControlledVisibility((view, visible) => {
//                if (!visible)                                    
//                    (View.Subviews[0] as UIScrollView).ScrollRectToVisible(new CGRect(0,0,View.Frame.Width,5), true);
//                
//                view.Hidden = !visible;
//            }));

            InitializeRecorder ();
        }

        private void InitializeRecorder()
        {
            //Declare string for application temp path and tack on the file extension
            string fileName = "test_transform.wav";
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

        public void StartRecord ()
        {
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

        public void StopRecord ()
        {
            _recorder.Stop ();

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

        private static CPTColor _lineColor = CPTColor.WhiteColor;
        public void UpdateSourceGraph (short[] data)
        {
            InvokeInBackground (() => {
                var plot = new CPTScatterPlot { 
                    DataSource = new SourceDataSource (data),
                    DataLineStyle = new CPTLineStyle {
                        LineColor = _lineColor,
                        LineWidth = (nfloat)1.0
                    }
                };

                _sourceGraph.AddPlot (plot);

                var space = _sourceGraph.DefaultPlotSpace as CPTXYPlotSpace;
                space.ScaleToFitPlots (new CPTPlot[] { plot });
            });
        }

        public void UpdateTransformGraph (double[] data)
        {
            InvokeInBackground (() => {
                var plot = new CPTScatterPlot { 
                    DataSource = new TransformDataSource (data),
                    DataLineStyle = new CPTLineStyle {
                        LineColor = _lineColor,
                        LineWidth = (nfloat)1.0
                    }
                };

                _transformGraph.AddPlot (plot);                
                
                var space = _transformGraph.DefaultPlotSpace as CPTXYPlotSpace;
                space.ScaleToFitPlots (new [] { plot });
                ChangeLineColor ();
            });
        }

        private static int _colorCount = 0;
        private void ChangeLineColor ()
        {
            switch (_colorCount) {
            case 0:
                _lineColor = CPTColor.BlueColor;
                break;
            case 1:
                _lineColor = CPTColor.BrownColor;
                break;
            case 2:
                _lineColor = CPTColor.CyanColor;
                break;
            case 3:
                _lineColor = CPTColor.GreenColor;
                break;
            case 4:
                _lineColor = CPTColor.LightGrayColor;
                break;
            case 5:
                _lineColor = CPTColor.MagentaColor;
                break;
            case 6:
                _lineColor = CPTColor.OrangeColor;
                break;
            case 7:
                _lineColor = CPTColor.PurpleColor;
                break;
            case 8:
                _lineColor = CPTColor.RedColor;
                break;
            case 9:
                _lineColor = CPTColor.YellowColor;
                break;
            default:
                {
                    var random = new Random ();
                    _lineColor = CPTColor.FromRgba (random.Next (0, 255), random.Next (0, 255), random.Next (0, 255), 1);
                    break;
                }
            }

            _colorCount++;
        }
    }
}

