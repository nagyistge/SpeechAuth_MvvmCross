using System;

namespace SpeechAuth.Core.Helpers.Processing
{
    public static class Window
    {
        /// <summary>
        /// Hamming window
        /// </summary>
        /// <param name="x">Array to convert</param>
        public static double[] Hamming(double[] x) {
            int l = x.Length;
            for (int i = 0; i < l; i++){
                x[i] *= 0.54 - 0.46 * Math.Cos((2 * Math.PI * i)/(l - 1));
            }
            return x;
        }

        /// <summary>
        /// Hann window
        /// </summary>
        /// <param name="x">Array to convert</param>
        public static double[] Hann(double[] x){
            int l = x.Length;
            for (int i = 0; i < l; i++){
                x[i] *= 0.5 - 0.5 * Math.Cos((2 * Math.PI * i)/(l - 1));
            }
            return x;   
        }

        public static void Test() {
            double[] x = {1, 2, 3, 4, 5};
            x = Window.Hamming(x);
            System.Diagnostics.Debug.WriteLine(x[0]);
            System.Diagnostics.Debug.WriteLine(x[1]);
            System.Diagnostics.Debug.WriteLine(x[2]);
            System.Diagnostics.Debug.WriteLine(x[3]);
            System.Diagnostics.Debug.WriteLine(x[4]);
        }
    }
}

