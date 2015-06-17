using System;
using System.Threading.Tasks;

namespace SpeechAuth.Core.Helpers.Processing
{
    public class FFT
    {
        /// <summary>
        /// Compute the FFT of x[], assuming its Length is a power of 2
        /// </summary>
        /// <param name="x">Array of Complex values of a discrete function</param>
        public static async Task<Complex[]> Fft(Complex[] x) 
        {
            int N = x.Length;

            if (N == 1)
                return new Complex[] { x[0] };

            if (N % 2 != 0)
                throw new Exception("N is not a power of 2");

            Complex[] even = new Complex[N / 2];
            for (int k = 0; k < N / 2; k++)
            {
                even[k] = x[2 * k];
            }
            Complex[] q = await Fft(even);

            Complex[] odd = even;
            for (int k = 0; k < N / 2; k++)
            {
                odd[k] = x[2 * k + 1];
            }
            Complex[] r = await Fft(odd);

            Complex[] y = new Complex[N];
            for (int k = 0; k < N / 2; k++)
            {
                double kth = -2 * k * Math.PI / N;
                Complex wk = new Complex(Math.Cos(kth), Math.Sin(kth));
                y[k] = q[k].Plus(wk.Times(r[k]));
                y[k + N / 2] = q[k].Minus(wk.Times(r[k]));
            }
            return y;
        }

        /// <summary>
        /// Compute the inverse FFT of x[], assuming its Length is a power of 2
        /// </summary>
        /// <param name="x">Array of Complex values of a discrete function</param>
        /// <returns>Converted array</returns>
        public static async Task<Complex[]> Ifft(Complex[] x) 
        {
            int N = x.Length;
            Complex[] y = new Complex[N];

            for (int i = 0; i < N; i++)
            {
                y[i] = x[i].Conjugate();
            }

            y = await Fft(y);

            for (int i = 0; i < N; i++)
            {
                y[i] = y[i].Conjugate();
            }

            for (int i = 0; i < N; i++)
            {
                y[i] = y[i].Times(1.0 / N);
            }

            return y;
        }

        /// <summary>
        /// Compute the circular convolution of x and y
        /// </summary>
        /// <param name="x">Array of complex values of a discrete function</param>
        /// <param name="y">Array of complex values of a discrete function</param>
        public static async Task<Complex[]> Cconvolve(Complex[] x, Complex[] y)
        {

            // should probably pad x and y with 0s so that they have same Length and are powers of 2
            if (x.Length != y.Length)
                throw new Exception("Dimensions don't agree");

            int N = x.Length;

            // compute FFT of each sequence
            var taskA = Task.Run<Complex[]> (() => Fft (x));
            var taskB = Task.Run<Complex[]> (() => Fft (y));

            await Task.WhenAll (new [] { taskA, taskB });

            Complex[] a = taskA.Result;
            Complex[] b = taskB.Result;

            // point-wise multiply
            Complex[] c = new Complex[N];
            for (int i = 0; i < N; i++)
            {
                c[i] = a[i].Times(b[i]);
            }

            // compute inverse FFT
            return await Ifft(c);
        }

        /// <summary>
        /// Compute the linear convolution of x and y
        /// </summary>
        /// <param name="x">Array of complex values of a discrete function</param>
        /// <param name="y">Array of complex values of a discrete function</param>
        public static async Task<Complex[]> Convolve(Complex[] x, Complex[] y)
        {
            Complex ZERO = new Complex(0, 0);

            Complex[] a = new Complex[2 * x.Length];
            for (int i = 0; i < x.Length; i++)
                a[i] = x[i];
            for (int i = x.Length; i < 2 * x.Length; i++)
                a[i] = ZERO;

            Complex[] b = new Complex[2 * y.Length];
            for (int i = 0; i < y.Length; i++)
                b[i] = y[i];
            for (int i = y.Length; i < 2 * y.Length; i++)
                b[i] = ZERO;

            return await Cconvolve(a, b);
        }

        /// <summary>
        /// Display an array of Complex numbers to standard output
        /// </summary>
        /// <param name="x">Array of Complex numbers.</param>
        /// <param name="title">Title</param>
        public static void Show(Complex[] x, String title) 
        {
            System.Diagnostics.Debug.WriteLine(title);
            System.Diagnostics.Debug.WriteLine("-------------------");
            for (int i = 0; i < x.Length; i++) {
                System.Diagnostics.Debug.WriteLine(x[i]);
            }
            System.Diagnostics.Debug.WriteLine("");
        }
    }
}

