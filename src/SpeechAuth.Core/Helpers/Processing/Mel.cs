using System;
using SpeechAuth.Core.Entities;
using SpeechAuth.Core.WaveletHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeechAuth.Core.Helpers.Processing
{
    public class Mel
    {
        public static int NUMBER_OF_FILTERS = 20;
        private static double LOW_FREQ = 0;
        private static double HIGH_FREQ = 8000;
        private static double[] fSmp = FilterBank();

        private static double Filter(int i, int k){
            double low_freq = (i == 0) ? LOW_FREQ : fSmp[i - 1];
            double high_freq = (i == fSmp.Length - 1) ? HIGH_FREQ : fSmp[i + 1];

            if ((double)k < low_freq) 
                return 0;
            
            if ((double)k < fSmp[i]) 
                return ((double)k - low_freq) / (fSmp[i] -low_freq);
            
            if ((i < NUMBER_OF_FILTERS) && ((double)k < high_freq)) 
                return (high_freq - (double)k) / (high_freq - fSmp[i]);
            
            return 0;
        }

        private static double HzToMel(double f) {
            return 1125 * Math.Log(1 + f / 700);
        }

        public static double MelToHz(double f) {
            return 700 * (Math.Exp(f / 1125) - 1);
        }

        public static async Task<double[]> GetMelKrepsCoef(double[][] x, Transform transformation)
        {
            var result = new double[NUMBER_OF_FILTERS];

            var tasks = new List<Task<double[]>> ();
            foreach (var arr in x)
                tasks.Add (Task.Run<double[]> (() => GetMelKrepsCoef (arr, transformation)));
            
            await Task.WhenAll (tasks);

            foreach (var task in tasks)
                for (int i = 0; i < task.Result.Length; i++) {
                    result[i] += task.Result[i];
                }

            for (int i = 0; i < result.Length; i++){
                result[i] = result[i] / x.Length;
            }

            return result;
        }

        public static async Task<double[]> GetMelKrepsCoef(double[] x, Transform transformation)
        {
            Complex[] spectrum;
            if (transformation == Transform.Wavelet)
            {
                var wspc = await CWT.cWT(x, MainHandler.FRAME_LENGTH, Wavelet.Morlet, 5, MainHandler.FRAME_LENGTH, 0.5, 20);
                var wT = wspc[0] as Complex[][];

                var specLst = new List<Complex> ();
                var tasks = new List<Task<Complex>> ();
                foreach (var item in wT)
                    tasks.Add (Task.Run<Complex> (() => item.Max ()));

                await Task.WhenAll (tasks);

                foreach (var task in tasks)
                    specLst.Add (task.Result);
                
                spectrum = await FFT.Fft (specLst.ToArray ());
            }
            else
                spectrum = await FFT.Fft (DoubleToComplex (Window.Hamming (x)));
            
            double[] powerSpectrum = SpectrumPower(spectrum);
            double[] tmp = new double[NUMBER_OF_FILTERS];
            for(int i = 0; i < NUMBER_OF_FILTERS; i++){
                tmp[i] = 0;
                for(int k = 0; k < MainHandler.FRAME_LENGTH / 2; k++) {
                    tmp [i] += powerSpectrum [k] * Filter (i, k);
                }
                tmp[i] = Math.Log(tmp[i]);
            }
            return Dct(tmp); //Discrete Cosine Transform
        }

        public static double[] Dct(double[] x){
            double[] ret = new double[x.Length];
            for (int j = 0; j < x.Length; j++){
                ret[j] = 0;
                for (int k = 0; k < NUMBER_OF_FILTERS; k++){
                    ret[j] += x[k] * Math.Cos((j + 1) * (k + 0.5) * Math.PI / NUMBER_OF_FILTERS);
                }
            }
            return ret;
        }

        public static Complex[] DoubleToComplex(double[] x){
            Complex[] ret = new Complex[x.Length];
            for (int i = 0; i < x.Length; i++){
                ret[i] = new Complex(x[i], 0);
            }
            return ret;
        }

        public static double SpectrumPower(Complex x) {
            return x.Real * x.Real + x.Imaginary * x.Imaginary;
        }

        public static double[] SpectrumPower(Complex[] x) {
            double[] res = new double[x.Length];
            for (int i = 0; i < x.Length; i++) {
                res[i] = Mel.SpectrumPower(x[i]);
            }
            return res;
        }

        private static double[] FilterBank() 
        {
            int M = MainHandler.FRAME_LENGTH / 2;
            double melLowFreq = HzToMel(LOW_FREQ);
            double melHighFreq = HzToMel(HIGH_FREQ);
            double len = (melHighFreq - melLowFreq) / (NUMBER_OF_FILTERS + 1);

            fSmp = new double[NUMBER_OF_FILTERS];
            for (int i = 0; i < NUMBER_OF_FILTERS; i++)
                fSmp[i] = ((double)M / MainHandler.SAMPLING_FREQ) * MelToHz(melLowFreq + (i + 1) * len);
            
            return fSmp;
        }
    }
}

