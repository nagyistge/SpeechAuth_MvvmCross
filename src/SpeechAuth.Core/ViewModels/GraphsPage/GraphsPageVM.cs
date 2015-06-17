using System;
using SpeechAuth.Core.Entities;
using Cirrious.MvvmCross.ViewModels;
using SpeechAuth.Core.ViewModels.RecordSoundPage;
using System.Windows.Input;
using System.Threading.Tasks;
using SpeechAuth.Core.Helpers.Preprocess;
using SpeechAuth.Core.Helpers;
using System.Collections.Generic;
using SpeechAuth.Core.WaveletHelpers;
using System.Linq;
using SpeechAuth.Core.Helpers.Processing;
using Cirrious.CrossCore;

namespace SpeechAuth.Core.ViewModels.GraphsPage
{
    public class GraphsPageVM : MvxViewModel
    {
        private ICommand _stopRecord;
        public ICommand StopRecord {
            get { return _stopRecord ?? (_stopRecord = new MvxCommand<short[]> (ComputeCharts)); }
        }

        internal PageMode Mode { get; private set; }

        public string Title { get; private set; }

        private Boolean _loadingExists;
        public Boolean LoadingHidden {
            get { return _loadingExists; }
            set {
                _loadingExists = value;
                RaisePropertyChanged (() => LoadingHidden);
            }
        }

        public void Init (PageMode mode)
        {
            Mode = mode;

            Title = mode == PageMode.GraphsWavelet ? "Wavelet" : "Fourier";
        }

        public GraphsPageVM ()
        {
            LoadingHidden = true;
        }

        private async void ComputeCharts (short[] shortArray)
        {
            LoadingHidden = false;

            var sourceTask = Task.Run (() => InvokeOnMainThread (() => {
                Mvx.Resolve<IGraphsPageView>().UpdateSourceGraph(shortArray);
                /* View.UpdateSourceGraph(shortArray);*/
            }));
            var transformTask = Task.Run (() => {
                var tmp = Preprocessing.Handle (MainHandler.Normalize (shortArray));
                double[][] audioFrames = MainHandler.DivToFrames (tmp);

                if (Mode == PageMode.GraphsWavelet) {
                    var realSpectrum = new List<double> ();
                    WaveletTransform (audioFrames, realSpectrum);
                    InvokeOnMainThread(() => {
                        Mvx.Resolve<IGraphsPageView>().UpdateTransformGraph(realSpectrum.ToArray());
//                        View.UpdateTransformGraph(realSpectrum.ToArray());
                    });
                } else if (Mode == PageMode.GraphsFourier) {
                    var realSpectrum = new List<double> ();
                    FourierTransform (audioFrames, realSpectrum);
                    InvokeOnMainThread(() => {
                        Mvx.Resolve<IGraphsPageView>().UpdateTransformGraph(realSpectrum.ToArray());
//                        View.UpdateTransformGraph(realSpectrum.ToArray());
                    });
                }
            });

            await Task.WhenAll (new [] { sourceTask, transformTask });

            LoadingHidden = true;
        }

        static async void WaveletTransform (double[][] audioFrames, List<double> realSpectrum)
        {
            int j = 0;
            var buffer = new double[MainHandler.FRAME_LENGTH * audioFrames.Length];

            foreach (var item in audioFrames) 
            {
                var dt = MainHandler.FRAME_LENGTH;
                var wspc = await CWT.cWT (item, dt, Wavelet.Morlet, 5, dt, 0.5, 20);
                var wT = wspc [0] as Complex[][];

                for (int i = 0; i < dt; i++, j++) 
                    buffer [j] = wT [i].Max ().Real;
            }

            for (int i = 0; i < buffer.Length; i++)
                realSpectrum.Add (buffer[i]);
        }

        static async void FourierTransform (double[][] audioFrames, List<double> realSpectrum)
        {
            foreach (var frame in audioFrames) 
            {
                Complex[] spectrum = await FFT.Fft (Mel.DoubleToComplex (Window.Hamming (frame)));

                foreach (var spec in spectrum)
                    realSpectrum.Add (spec.Real);
            }
        }
    }
}

