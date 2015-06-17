using System;
using System.Collections.Generic;
using SpeechAuth.Core.Helpers;
using SpeechAuth.Core.Helpers.Processing;
using System.Threading.Tasks;

namespace SpeechAuth.Core.WaveletHelpers
{
    public enum Wavelet
    {
        Morlet, 
        DOG,
        Paul
    }

    public class CWT 
    {
        /// <summary>
        /// Continuous wavelet transform for a real, discretely sampled function y = f(t) of length n where n is an integer power of 2
        /// </summary>
        /// <returns>An List[Object] wspcmf[6] where: 
        /// xspcmf[0] is a Complex[n][jtot] multi-resolution matrix, 
        /// wspcmf[1] is a double[jtot] vector of scales, 
        /// wspcmf[2] is a double[jtot] vector of periods, 
        /// wspcmf[3] is a double[n] cone-of-influence vector, 
        /// wspcmf[4] is the signal mean, 
        /// and wspcmf[5] is the Fourier factor.</returns>
        /// <param name="y">The time-sampled signal to transform.</param>
        /// <param name="dt">The sampling increment</param>
        /// <param name="mother">The wavelet to use in the transform (Morlet, DOG, Paul)</param>
        /// <param name="param">wavelet parameter (Morlet param = wave number, Paul param = order, DOG param = m::m-th derivative of Gaussian)</param>
        /// <param name="s0">The smallest desired scale in the transform (try s0=dt for Morlet; s0=dt/4 for Paul)</param>
        /// <param name="dj">The desired spacing between successive scales (try 0.25)</param>
        /// <param name="jtot">The desired number of scales to be computed</param>
        public static async Task<List<object>> cWT(double[] y, double dt, Wavelet mother, double param, double s0, double dj, int jtot) 
        {
            int n = y.Length;

            // Result objects
            List<object> wspcmf = new List<object>();

            var wT = new Complex[jtot][];
            MatrixOps.Initialize(wT, n);
            double[] scale = new double[jtot];
            double[] period = new double[jtot];
            double[] coi = new double[n];
            double signalMean = 0;

            // Find the time-series mean and remove it.
            for (int i = 0; i < n; i++) {
                signalMean = signalMean + y[i];
            }

            signalMean = signalMean / (double) n;

            double[] yNoDC = new double[n];
            for (int i = 0; i < n; i++) {
                yNoDC[i] = y[i] - signalMean;
            }

            // Obtain the Fourier transform of the time series
            Complex[] yfft = await FFT.Fft(Mel.DoubleToComplex(Window.Hamming(yNoDC)));

            // Create a vector of wave numbers such that:
            // w = 2pik/Ndt for k <= N/2 and w = -2pik/Ndt for k > N/2
            double[] w = new double[n];
            w[0] = 0;
            double df = 2.0 * Math.PI / ((double) n * dt);
            for (int i = 2; i <= (n / 2) + 1; i++) {
                w[i - 1] = ((double) i - 1.0) * df;
            }
            for (int i = n / 2 + 2; i <= n; i++) {
                w[i - 1] = -w[n - i + 1];
            }

            // main wavelet loop
            for (int j = 0; j < jtot; j++) {
                scale[j] = s0 * Math.Pow(2.0, (double) j * dj);
                Complex[] daughter = GetDaughter(mother, param, scale[j], w, dt);

                // Convolution(f, g) = ifft( fft(f) X fft(g) )
                Complex[] product = new Complex[n];
                for (int k = 0; k <= n - 1; k++) {
                    product[k] = ComplexCalc.Multiply(daughter[k], yfft[k]);
                }
                wT[j] = await FFT.Ifft(product);
            }

            // compute period and coi vectors
            double fourierFactor = 0;
            double eFold = 0;
            switch (mother.ToString()) {
            case "Morlet":
                fourierFactor = (4.0 * Math.PI)
                    / (param + Math.Sqrt(2.0 + Math.Pow(param, 2)));
                eFold = fourierFactor / Math.Sqrt(2.0);
                break;
            case "Paul":
                fourierFactor = (4.0 * Math.PI) / (2 * param + 1);
                eFold = fourierFactor * Math.Sqrt(2.0);
                break;
            case "DOG":
                fourierFactor = 2 * Math.PI * Math.Sqrt(2 / (2 * param + 1));
                eFold = fourierFactor / Math.Sqrt(2.0);
                break;
            }

            for (int i = 0; i < jtot; i++) {
                period[i] = scale[i] * fourierFactor;
            }

            for (int i = 1; i <= ((n + 1) / 2); i++) {
                coi[i - 1] = eFold * dt * ((i) - 1);
                coi[((n - 1) - i) + 1] = coi[i - 1];
            }

            wspcmf.Add(MatrixOps.Transpose(wT));
            wspcmf.Add(scale);
            wspcmf.Add(period);
            wspcmf.Add(coi);
            wspcmf.Add(signalMean);
            wspcmf.Add(fourierFactor);
            return wspcmf;
        }

        /// <summary>
        /// Get the daughter wavelet for the specified arguments.
        /// </summary>
        /// <returns>Complex[n] vector of daughter</returns>
        /// <param name="mother">The wavelet (Morlet, DOG, Paul)</param>
        /// <param name="param">wavelet parameter (Morlet param = wavenumber, Paul param = order, DOG param = m::m-th derivative)</param>
        /// <param name="scaleJ">The desired wavelet scale to be used for constructing the daughter.</param>
        /// <param name="waveNumbers">The vector of wave numbers to be used to construct the daughter.</param>
        /// <param name="dt">The sampling increment (for normalization).</param>
        private static Complex[] GetDaughter(Wavelet mother, double param, double scaleJ, double[] waveNumbers, double dt) {
            int n = waveNumbers.Length;

            Complex[] daughter = new Complex[n];

            if (mother == Wavelet.Morlet) {
                double norm = Math.Sqrt(2.0 * Math.PI * scaleJ / dt)
                    * Math.Pow(Math.PI, -0.25);
                for (int k = 0; k <= n / 2; k++) {
                    double expnt = -0.5
                        * Math.Pow((scaleJ * waveNumbers[k] - param), 2);
                    daughter[k] = new Complex(norm * Math.Exp(expnt), 0);
                }
                for (int k = n / 2 + 1; k < n; k++) {
                    daughter[k] = ComplexCalc.Zero();
                }
            }

            if (mother == Wavelet.Paul) {
                double f = Gamma.Factorial(2 * (int) param - 1);
                double norm = Math.Sqrt(2 * Math.PI * scaleJ / dt);
                norm *= Math.Pow(2, param) / (Math.Sqrt(param * f));

                for (int k = 0; k <= n / 2; k++) {
                    double expnt = -scaleJ * waveNumbers[k];
                    daughter[k] = new Complex(norm
                        * Math.Pow(scaleJ * waveNumbers[k], param)
                        * Math.Exp(expnt), 0);
                }
                for (int k = n / 2 + 1; k < n; k++) {
                    daughter[k] = ComplexCalc.Zero();
                }
            }

            // Odd DOGs are complex.
            if (mother == Wavelet.DOG) 
            {
                double nf1 = Math.Sqrt(2 * Math.PI * scaleJ / dt)
                    * Math.Sqrt(1 / Gamma.GetGamma(param + 0.5));
                Complex nf2 = ComplexCalc.Pow(ComplexCalc.I(), param);
                Complex norm = ComplexCalc.Multiply(-nf1, nf2);

                for (int k = 0; k < n; k++) 
                {
                    double sw = scaleJ * waveNumbers[k];
                    Complex d1 = ComplexCalc.Multiply(norm,
                        Math.Pow(sw, param));
                    double d2 = Math.Exp(-Math.Pow(sw, 2) / 2);
                    daughter[k] = ComplexCalc.Multiply(d1, d2);
                }
            }

            return daughter;
        }

        /// <summary>
        /// Gets the selected parameter choices.
        /// </summary>
        /// <returns>A partial selection of valid parameters</returns>
        /// <param name="wavelet">Wavelet.</param>
        public static double[] GetSelectedParamChoices(Wavelet wavelet)
        {
            double[] validParams = null;
            if (wavelet == Wavelet.Morlet) {
                validParams = new double[] { 5, 5.336, 6, 7, 8, 10, 12, 14, 16, 20 };
            } else if (wavelet == Wavelet.Paul) {
                validParams = new double[] { 4, 5, 6, 7, 8, 10, 16, 20, 30, 40 };
            } else if (wavelet == Wavelet.DOG) {
                validParams = new double[] { 2, 4, 5, 6, 8, 12, 16, 20, 30, 60 };
            }
            return validParams;
        }

        /**
     * 
     * @param mother
     *            The wavelet.
     * @param param
     *            The wavelet parameter
     * @return An List(Object) psi0C[2], where psi0C[0] is psi(0) and
     *         psi0C[1] is the reconstruction factor as determined empirically
     *         using a delta function. Psi(0) is a Complex for odd
     *         parameters of Paul wavelets, otherwise it is a double precision,
     *         real value.
     */
        private static async Task<List<Object>> GetEmpiricalFactors(Wavelet mother, double param) 
        {
            double dt = 1;
            int n = 2;
            double s0 = 0.01;
            double dj = 0.1;
            int jtot = 120;
            if (mother == Wavelet.DOG)
            {
                n = 256;
                jtot = 160;
            }
            double[] deltaFn = new double[n];
            {
                for (int i = 0; i < deltaFn.Length; i++) {
                    deltaFn[i] = 0;
                }
                deltaFn[0] = 1;
            }

            List<object> wspcmf = await CWT.cWT(deltaFn, dt, mother, param, s0, dj, jtot);

            Complex[][] wave = (Complex[][]) wspcmf[0];
            double[] scale = (double[]) wspcmf[1];
            double fourierFactor = (double) wspcmf[5];
            System.Diagnostics.Debug.WriteLine(mother + " " + param + ": Fourier factor " + fourierFactor);

            double[][] rWave = MatrixOps.GetRealPart(wave);

            for (int j = 0; j < scale.Length; j++) {
                for (int i = 0; i < deltaFn.Length; i++) {
                    rWave[i][j] = rWave[i][j] / Math.Sqrt(scale[j]);
                }
            }
            double[] sums = new double[deltaFn.Length];
            for (int i = 0; i < deltaFn.Length; i++) {
                sums[i] = 0;
                for (int j = 0; j < scale.Length; j++) {
                    sums[i] += rWave[i][j];
                }
            }

            // double max = MatrixOps.vectorInfinityNorm(sums);
            double hi = MatrixOps.Max(sums);
            double lo = MatrixOps.Min(sums);
            double max = hi;
            if (Math.Abs(lo) > hi) {
                max = lo;
            } // signed val of infinity norm

            double sqrtDt = Math.Sqrt(dt);
            double psi0 = 0;
            Complex cPsi0 = ComplexCalc.Zero();

            switch (mother) {
            case Wavelet.Morlet:
                psi0 = 1 / (Math.Pow(Math.PI, 0.25));
                break;
            case Wavelet.Paul:
                int m = (int) param;
                Complex numerator = ComplexCalc.Multiply(ComplexCalc
                    .Multiply(ComplexCalc.Pow(ComplexCalc.I(), m),
                        Math.Pow(2, m)), Gamma.Factorial(m));
                double denominator = Math.Sqrt((Math.PI * Gamma.Factorial(2 * m)));
                cPsi0 = ComplexCalc.Divide(numerator, denominator);
                psi0 = cPsi0.Real;
                break;
            case Wavelet.DOG:
                m = (int) param;
                psi0 = m + 0.5;
                if (m % 2 != 0) {
                    // Asymmetric!
                    psi0 = Hermite.ProbabilistHermitePoly(m, 1)
                        / Math.Sqrt(Gamma.GetGamma(psi0));
                } else {
                    psi0 = Hermite.ProbabilistHermitePoly(m, 0)
                        / Math.Sqrt(Gamma.GetGamma(psi0));
                }

                psi0 = psi0 * Math.Pow(-1, m + 1);
                break;
            }
            List<Object> psi0C = new List<Object>();
            if (mother == Wavelet.Paul && param % 2 != 0) {
                double result = max * dj * sqrtDt;
                Complex C = ComplexCalc.Divide(result, cPsi0);
                psi0C.Add(cPsi0);
                psi0C.Add(C);
                System.Diagnostics.Debug.WriteLine(mother + " " + param + ": psi0 = (" + cPsi0.Real
                    + ", " + cPsi0.Imaginary + "i), C = (" + C.Real + ", "
                    + C.Imaginary + ")");
            } else {
                double C = max * dj * sqrtDt / psi0;
                psi0C.Add(psi0);
                psi0C.Add(C);
                System.Diagnostics.Debug.WriteLine(mother + " " + param + ": psi0 = " + psi0
                    + ", C = " + C);
            }
            System.Diagnostics.Debug.WriteLine("\n");
            return psi0C;
        }

        /// <summary>
        /// Cwts the reconstruct.
        /// </summary>
        /// <returns>Return a reconstructed signal, except when mother is an odd DOG.</returns>
        /// <param name="mother">Mother.</param>
        /// <param name="param">Parameter.</param>
        /// <param name="rWT">Real part of mra matrix</param>
        /// <param name="scales">Scales.</param>
        /// <param name="dj">Dj.</param>
        /// <param name="dt">Dt.</param>
        /// <param name="signalMean">Signal mean.</param>
        public static async Task<double[]> cwtReconstruct(Wavelet mother, double param, double[][] rWT, double[] scales, double dj, double dt, double signalMean)
        {
            List<object> psi0C = await GetEmpiricalFactors(mother, param);

            double factor = 0;

            if (psi0C[0] is double) {
                double psi0 = (double) psi0C[0];
                double c = (double) psi0C[1];
                factor = dj * Math.Sqrt(dt) / (psi0 * c);
            } else {
                Complex cFactor = ComplexCalc.Zero();
                Complex psi0 = (Complex) psi0C[0];
                Complex c = (Complex) psi0C[1];
                cFactor = ComplexCalc.Divide(dj * Math.Sqrt(dt), ComplexCalc.Multiply(psi0, c));
                factor = cFactor.Real;
            }

            double[] y = new double[rWT.Length];
            for (int i = 0; i < rWT.Length; i++) {
                double summer = 0;
                for (int j = 0; j < scales.Length; j++) {
                    summer += (rWT[i][j] / Math.Sqrt(scales[j]));
                }
                y[i] = summer * factor + signalMean;
            }
            return y;
        }
    }
}

