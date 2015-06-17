using System;
using SpeechAuth.Core.Helpers;
using System.Collections.Generic;

namespace SpeechAuth.Core.WaveletHelpers
{
    public class MatrixOps
    {
        /// <summary>
        /// Machines the epsilon double.
        /// </summary>
        /// <returns>Gives an upper bound on the relative error due to rounding in floating point arithmetic.</returns>
        public static double MachineEpsilonDouble()
        {
            double eps = 1.0;
            do
                eps /= 2.0;
            while ((1.0 + (eps / 2.0)) != 1.0);
            return eps;
        }

        /// <summary>
        /// Eyes the complex.
        /// </summary>
        /// <returns>Identity matrix I[n,n]</returns>
        /// <param name="n">N</param>
        public static Complex[][] EyeComplex(int n)
        {
            var eye = new Complex[n][];
            Initialize (eye, n);

            for (int i = 0; i < n; i++) {
                eye[i][i].Real = 1.0;
            }
            return eye;
        }

        /// <summary>
        /// Eye the specified n.
        /// </summary>
        /// <returns>Identity matrix I[n,n]</returns>
        /// <param name="n">N.</param>
        public static double[][] Eye(int n)
        {
            double[][] eye = new double[n][];
            Initialize (eye, n);
            for (int i = 0; i < n; i++) {
                eye[i][i] = 1.0;
            }
            return eye;
        }

        /**
        * 
        * When a Complex[] is created (new Complex[n]()), its elements
        * are null. If a function includes an assignment where an null element
        * appears on the right-hand side of the assignment (e.g. c = c + a*b) prior
        * to instance creation an error will be thrown. This method initializes the
        * array with zeros and prevents the error.
        */
        public static Complex[] Initialize(Complex[] v, int rowLength) 
        {
            int n = rowLength;
            v = new Complex[rowLength];
            for (int i = 0; i < n; i++) {
                v[i] = new Complex();
            }
            return v;
        }

        /**
     * 
     * When a Complex[][] is created (new Complex[m][n]()), its
     * elements are null. If a function includes an assignment where a null
     * element appears on the right-hand side of the assignment (e.g. c = c +
     * a*b) prior to instance creation an error will be thrown. This method
     * initializes the array and prevents the error.
     */
        public static Complex[][] Initialize(Complex[][] A, int rowLength) 
        {
            int m = A.Length;
            for (int i = 0; i < m; i++) {
                A [i] = Initialize (A [i], rowLength);
            }
                
            return A;
        }

        public static double[] Initialize(double[] v, int rowLength) 
        {
            int n = rowLength;
            v = new double[rowLength];
            for (int i = 0; i < n; i++) {
                v[i] = new double();
            }
            return v;
        }

        public static double[][] Initialize(double[][] A, int rowLength) 
        {
            int m = A.Length;
            for (int i = 0; i < m; i++) {
                A [i] = Initialize (A [i], rowLength);
            }

            return A;
        }

        /// <summary>
        /// To the list.
        /// </summary>
        /// <returns>List of doubles[] such that each double[] is a row from A</returns>
        /// <param name="A">A[rows][columns]</param>
        public static List<double[]> ToList(double[][] A)
        {
            List<double[]> listRows = new List<double[]>();       
            for (int i = 0; i < A.Length; i++) {
                listRows.Add (A [i]);
            }
            return listRows;
        }

        public static double[][] ToDouble(List<double[]> columnVectors) 
        {
            int m = columnVectors[0].Length;
            int n = columnVectors.Count;
            var matrix = new double[m][];
            Initialize (matrix, n);
            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    matrix [i] [j] = columnVectors [j] [i];
                }
            }
            return matrix;
        }

        public static Complex[][] ToComplex(double[][] A) 
        {
            int m = A.Length;
            int n = A[0].Length;
            Complex[][] cA = new Complex[m][];
            Initialize (cA, n);
            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    cA[i][j] = new Complex(A[i][j], 0);
                }
            }
            return cA;
        }

        public static double[][] GetRealPart(Complex[][] A) 
        {
            int m = A.Length;
            int n = A[0].Length;
            var realPart = new double[m][];
            Initialize (realPart, n);
            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    realPart[i][j] = A[i][j].Real;
                }
            }
            return realPart;
        }

        public static double[] GetRealPart(Complex[] v) 
        {
            int m = v.Length;

            double[] realPart = new double[m];
            for (int i = 0; i < m; i++) {

                realPart[i] = v[i].Real;

            }
            return realPart;
        }


        public static double[][] GetImaginaryPart(Complex[][] A) 
        {
            int m = A.Length;
            int n = A[0].Length;
            double[][] realPart = new double[m][];
            Initialize (realPart, n);
            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    realPart[i][j] = A[i][j].Imaginary;
                }
            }
            return realPart;
        }

        public static double[] GetImaginaryPart(Complex[] v)
        {
            int m = v.Length;

            double[] realPart = new double[m];
            for (int i = 0; i < m; i++) {

                realPart[i] = v[i].Imaginary;

            }
            return realPart;
        }

        public static Complex[] GetColumnAsVector(Complex[][] A, int colIndex) 
        {
            int m = A.Length;
            Complex[] col = new Complex[m];
            for (int i = 0; i < m; i++) {
                col[i] = A[i][colIndex];
            }
            return col;
        }

        public static double[] GetColumnAsVector(double[][] A, int colIndex) 
        {
            int m = A.Length;
            double[] col = new double[m];
            for (int i = 0; i < m; i++) {
                col[i] = A[i][colIndex];
            }
            return col;
        }

        public static Complex[] GetRowAsVector(Complex[][] A,
            int rowIndex) {
            int n = A[0].Length;
            System.Diagnostics.Debug.WriteLine(A.Length + "\n");
            System.Diagnostics.Debug.WriteLine(A[0].Length + "\n");
            Complex[] col = new Complex[n];
            for (int i = 0; i < n; i++) {
                col[i] = A[rowIndex][i];
            }
            return col;
        }

        public static double[] GetRowAsVector(double[][] A, int rowIndex) {
            int n = A[0].Length;
            System.Diagnostics.Debug.WriteLine(A.Length + "\n");
            System.Diagnostics.Debug.WriteLine(A[0].Length + "\n");
            double[] col = new double[n];
            for (int i = 0; i < n; i++) {
                col[i] = A[rowIndex][i];
            }
            return col;
        }

        public static double[][] VectorToRowMatrix(double[] v) {
            int n = v.Length;
            double[][] row = new double[1][];
            row [0] = new double[n];
            for (int i = 0; i < n; i++) {
                row[0][i] = v[i];
            }
            return row;
        }

        public static Complex[][] VectorToRowMatrix(Complex[] v) {
            int n = v.Length;
            Complex[][] row = new Complex[1][];
            row [0] = new Complex[n];
            for (int i = 0; i < n; i++) {
                row[0][i] = v[i];
            }
            return row;
        }

        public static double[][] VectorToColumnMatrix(double[] v) {
            int n = v.Length;
            double[][] row = new double[n][];
            Initialize (row, 1);
            for (int i = 0; i < n; i++) {
                row[i][0] = v[i];
            }
            return row;
        }

        public static Complex[][] VectorToColumnMatrix(Complex[] v) {
            int n = v.Length;
            Complex[][] row = new Complex[n][];
            Initialize (row, 1);
            for (int i = 0; i < n; i++) {
                row[i][0] = v[i];
            }
            return row;
        }

        public static Complex[] ToComplex(double[] v) {
            int n = v.Length;
            Complex[] cV = new Complex[n];
            for (int i = 0; i < n; i++) {
                cV[i] = new Complex(v[i], 0);
            }
            return cV;
        }

        /// <summary>
        /// Pads the pow2.
        /// </summary>
        /// <returns>If necessary, expanded sequence such that its length is an even power of 2 by adding additional zero values.</returns>
        /// <param name="x">The sequence to pad</param>
        public static double[] PadPow2(double[] x) {
            int sizeIn = x.Length;
            double log2N = Math.Log(sizeIn) / Math.Log(2);
            double ceiling = Math.Ceiling(log2N);
            if (log2N < ceiling) 
            {
                log2N = ceiling;
                int sizePad = (int) Math.Pow(2, log2N);
                double[] padX = new double[sizePad];
                for (int i = 0; i < sizePad; i++) {
                    if (i < sizeIn) {
                        padX[i] = x[i];
                    } else {
                        padX[i] = 0;
                    }
                }
                return padX;
            } 
            else {
                return x;
            }
        }

        /// <summary>
        /// Pads the pow2.
        /// </summary>
        /// <returns>If necessary, expanded sequence such that its length is an even power of 2 by adding additional zero values.</returns>
        /// <param name="xy">A double[][] where xy[0] = x and xy[1] = f(x)</param>
        public static double[][] PadPow2(double[][] xy) {
            int sizeIn = xy[0].Length;
            double log2N = Math.Log(sizeIn) / Math.Log(2);
            double ceiling = Math.Ceiling(log2N);
            if (log2N < ceiling) {
                log2N = ceiling;
                int sizePad = (int) Math.Pow(2, log2N);
                double[][] padXY = new double[2][];
                Initialize (padXY, sizePad);
                double dx = padXY[0][1] - padXY[0][0];
                for (int i = 0; i < sizePad; i++) {
                    if (i < sizeIn) {
                        padXY[0][i] = xy[0][i];
                        padXY[1][i] = xy[1][i];
                    } else {
                        padXY[0][i] = padXY[0][i - 1] + dx;
                        padXY[1][i] = 0;
                    }
                }
                return padXY;
            } else {
                return xy;
            }
        }

        /// <summary>
        /// Interleaveds the real to complex.
        /// </summary>
        /// <returns>a CUDA compatible complex interleaved</returns>
        /// <param name="real">A vector of real numbers that may be a fixed-stride representation of a matrix</param>
        public static Complex[] InterleavedRealToComplex(float[] real) {
            int len = real.Length;
            Complex[] result = new Complex[len / 2];
            for (int i = 0; i < len; i += 2) {
                result[i / 2].Real = real[i];
                result[i / 2].Imaginary = real[i + 1];
            }
            return result;
        }

        public static double[] DeepCopy(double[] v) {
            int n = v.Length;
            double[] copy = new double[n];
            for (int j = 0; j < n; j++) {
                copy[j] = v[j];
            }
            return copy;
        }

        public static double[][] DeepCopy(double[][] A) {
            int m = A.Length;
            int n = A[0].Length;
            double[][] copy = new double[m][];
            Initialize (copy, n);
            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    copy[i][j] = A[i][j];
                }
            }
            return copy;
        }

        public static Complex[] DeepCopy(Complex[] v) {
            int n = v.Length;
            Complex[] copy = new Complex[n];
            for (int j = 0; j < n; j++) {
                copy[j] = v[j];
            }
            return copy;
        }

        public static Complex[][] DeepCopy(Complex[][] A) {
            int m = A.Length;
            int n = A[0].Length;
            Complex[][] copy = new Complex[m][];

            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    copy[i][j] = A[i][j];
                }
            }
            return copy;
        }

        public static List<double[]> DeepCopy(List<double[]> A){
            List<double[]> copy = new List<double[]>();
            foreach(var vector in A){
                int len = vector.Length;
                double[] cVector=new double[len];
                for(int i = 0; i<len;i++){
                    cVector[i] = vector[i];
                }
                copy.Add(cVector);
            }       
            return copy;
        }


        //Operations

        public static double[][] Modulus(Complex[][] A) {
            int m = A.Length;
            int n = A[0].Length;
            double[][] absA = new double[m][];
            Initialize (absA, n);
            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    absA[i][j] = ComplexCalc.Modulus(A[i][j]);
                }
            }
            return absA;
        }

        public static double[] Modulus(Complex[] v) {
            int n = v.Length;
            double[] absV = new double[n];
            for (int i = 0; i < n; i++) {
                absV[i] = ComplexCalc.Modulus(v[i]);
            }
            return absV;
        }

        public static Complex[] Conjugate(Complex[] v) {
            int n = v.Length;
            Complex[] conj = new Complex[n];
            for (int i = 0; i < n; i++) {
                conj[i] = ComplexCalc.Conjugate(v[i]);
            }
            return conj;
        }

        public static Complex[][] Conjugate(Complex[][] A) {
            int m = A.Length;       
            Complex[][] conj = new Complex[m][];
            for (int i = 0; i < m; i++) {
                int n = A[i].Length;
                conj[i]= new Complex[n];
                for(int j = 0;j<n;j++){

                    conj[i][j] = ComplexCalc.Conjugate(A[i][j]);    
                }           
            }
            return conj;
        }

        // ////////////////// Transposition ////////////

        public static Complex[][] Transpose(Complex[][] A) {
            int m = A.Length;
            int n = A[0].Length;
            Complex[][] AT = new Complex[n][];
            Initialize (AT, m);
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < m; j++) {
                    AT[i][j] = A[j][i];
                }
            }
            return AT;
        }

        public static double[][] Transpose(double[][] A) {
            int m = A.Length;
            int n = A[0].Length;
            double[][] AT = new double[n][];
            Initialize (AT, m);
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < m; j++) {
                    AT[i][j] = A[j][i];
                }
            }
            return AT;
        }

        public static Complex[][] ConjugateTranspose(Complex[][] A) {
            int m = A.Length;
            int n = A[0].Length;
            Complex[][] conjT = new Complex[n][];
            Initialize (conjT, m);
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < m; j++) {
                    conjT[i][j] = ComplexCalc.Conjugate(A[j][i]);
                }
            }
            return conjT;
        }

        // /////////////////// scale ///////////////////////

        public static Complex[][] Scale(Complex alpha, Complex[][] A) 
        {
            int m = A.Length;
            int n = A[0].Length;
            Complex[][] alphaA = new Complex[m][];
            Initialize (alphaA, n);
            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    alphaA[i][j] = ComplexCalc.Multiply(alpha, A[i][j]);
                }
            }
            return alphaA;
        }

        public static Complex[][] Scale(double alpha, Complex[][] A) {
            int m = A.Length;
            int n = A[0].Length;
            Complex[][] alphaA = new Complex[m][];
            Initialize (alphaA, n);
            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    alphaA[i][j] = ComplexCalc.Multiply(alpha, A[i][j]);
                }
            }
            return alphaA;
        }

        public static Complex[][] Scale(Complex alpha, double[][] A) {
            int m = A.Length;
            int n = A[0].Length;
            Complex[][] alphaA = new Complex[m][];
            Initialize (alphaA, n);
            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    alphaA[i][j] = ComplexCalc.Multiply(alpha, A[i][j]);
                }
            }
            return alphaA;
        }

        public static double[][] Scale(double alpha, double[][] A) {
            int m = A.Length;
            int n = A[0].Length;
            double[][] alphaA = new double[m][];
            Initialize (alphaA, n);
            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    alphaA[i][j] = alpha * A[i][j];
                }
            }
            return alphaA;
        }

        public static Complex[] Scale(Complex alpha, Complex[] v) {
            int n = v.Length;
            Complex[] alphaV = new Complex[n];
            for (int i = 0; i < n; i++) {
                alphaV[i] = ComplexCalc.Multiply(alpha, v[i]);
            }
            return alphaV;
        }

        public static Complex[] Scale(double alpha, Complex[] v) {
            int n = v.Length;
            Complex[] alphaV = new Complex[n];
            for (int i = 0; i < n; i++) {
                alphaV[i] = ComplexCalc.Multiply(alpha, v[i]);
            }
            return alphaV;
        }

        public static Complex[] Scale(Complex alpha, double[] v) {
            int n = v.Length;
            Complex[] alphaV = new Complex[n];
            for (int i = 0; i < n; i++) {
                alphaV[i] = ComplexCalc.Multiply(alpha, v[i]);
            }
            return alphaV;
        }

        public static double[] Scale(double alpha, double[] v) {
            int n = v.Length;
            double[] alphaV = new double[n];
            for (int i = 0; i < n; i++) {
                alphaV[i] = alpha * v[i];
            }
            return alphaV;
        }

        // add

        public static Complex[][] Add(Complex[][] A, Complex[][] B) {
            int m = A.Length;
            int n = A[0].Length;
            Complex[][] C = new Complex[m][];
            Initialize (C, n);
            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    C[i][j] = ComplexCalc.Add(A[i][j], B[i][j]);
                }
            }
            return C;
        }

        public static Complex[][] Add(Complex[][] A, double[][] B) {
            int m = A.Length;
            int n = A[0].Length;
            Complex[][] C = new Complex[m][];
            Initialize (C, n);
            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    C[i][j] = ComplexCalc.Add(A[i][j], B[i][j]);
                }
            }
            return C;
        }

        public static Complex[][] Add(double[][] A, Complex[][] B) {
            int m = A.Length;
            int n = A[0].Length;
            Complex[][] C = new Complex[m][];
            Initialize (C, n);
            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    C[i][j] = ComplexCalc.Add(A[i][j], B[i][j]);
                }
            }
            return C;
        }

        public static double[][] Add(double[][] A, double[][] B) {
            int m = A.Length;
            int n = A[0].Length;
            double[][] C = new double[m][];
            Initialize (C, n);
            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    C[i][j] = (A[i][j] + B[i][j]);
                }
            }
            return C;
        }

        public static Complex[] Add(Complex[] v1, Complex[] v2) {
            int n = v1.Length;
            Complex[] v1Pv2 = new Complex[n];
            for (int i = 0; i < n; i++) {
                v1Pv2[i] = ComplexCalc.Add(v1[i], v2[i]);
            }
            return v1Pv2;
        }

        public static Complex[] Add(double[] v1, Complex[] v2) {
            int n = v1.Length;
            Complex[] v1Pv2 = new Complex[n];
            for (int i = 0; i < n; i++) {
                v1Pv2[i] = ComplexCalc.Add(v1[i], v2[i]);
            }
            return v1Pv2;
        }

        public static Complex[] Add(Complex[] v1, double[] v2) {
            int n = v1.Length;
            Complex[] v1Pv2 = new Complex[n];
            for (int i = 0; i < n; i++) {
                v1Pv2[i] = ComplexCalc.Add(v1[i], v2[i]);
            }
            return v1Pv2;
        }

        public static double[] Add(double[] v1, double[] v2) {
            int n = v1.Length;
            double[] v1Pv2 = new double[n];
            for (int i = 0; i < n; i++) {
                v1Pv2[i] = v1[i] + v2[i];
            }
            return v1Pv2;
        }

        // /////////////////// multiply ///////////////////////

        public static Complex[][] Multiply(Complex[][] A,
            Complex[][] B) {
            int m = A.Length;
            int p = A[0].Length;
            int n = B[0].Length;
            Complex[][] C = new Complex[m][];
            Initialize(C, n);
            for (int i = 0; i < m; i++) {
                for (int k = 0; k < p; k++) {
                    for (int j = 0; j < n; j++) {
                        C[i][j] = ComplexCalc.Add(C[i][j],
                            ComplexCalc.Multiply(A[i][k], B[k][j]));
                    }
                }
            }
            return C;
        }

        public static Complex[][] Multiply(double[][] A, Complex[][] B) {
            int m = A.Length;
            int p = A[0].Length;
            int n = B[0].Length;
            Complex[][] C = new Complex[m][];
            Initialize(C, n);
            for (int i = 0; i < m; i++) {
                for (int k = 0; k < p; k++) {
                    for (int j = 0; j < n; j++) {
                        C[i][j] = ComplexCalc.Add(C[i][j],
                            ComplexCalc.Multiply(A[i][k], B[k][j]));
                    }
                }
            }
            return C;
        }

        public static Complex[][] Multiply(Complex[][] A, double[][] B) {
            int m = A.Length;
            int p = A[0].Length;
            int n = B[0].Length;
            Complex[][] C = new Complex[m][];
            Initialize(C, n);
            for (int i = 0; i < m; i++) {
                for (int k = 0; k < p; k++) {
                    for (int j = 0; j < n; j++) {
                        C[i][j] = ComplexCalc.Add(C[i][j],
                            ComplexCalc.Multiply(A[i][k], B[k][j]));
                    }
                }
            }
            return C;
        }

        public static double[][] Multiply(double[][] A, double[][] B) {
            int m = A.Length;
            int p = A[0].Length;
            int n = B[0].Length;
            double[][] C = new double[m][];
            Initialize (C, n);
            for (int i = 0; i < m; i++) {
                for (int k = 0; k < p; k++) {
                    for (int j = 0; j < n; j++) {
                        C[i][j] = C[i][j] + A[i][k] * B[k][j];
                    }
                }
            }
            return C;
        }

        public static double[] Multiply(double[][] A, double[] b) {
            int m = A.Length;
            int n = A[0].Length;
            double[] v = new double[m];
            for (int k = 0; k < n; k++) {
                for (int i = 0; i < m; i++) {
                    v[i] += A[i][k] * b[k];
                }
            }
            return v;
        }

        public static Complex[] Multiply(Complex[][] A, double[] b) {
            int m = A.Length;
            int n = A[0].Length;
            Complex[] v = new Complex[m];
            Initialize(v, m);
            for (int k = 0; k < n; k++) {
                for (int i = 0; i < m; i++) {
                    v[i] = ComplexCalc.Add(v[i],
                        ComplexCalc.Multiply(A[i][k], b[k]));
                }
            }
            return v;
        }

        public static Complex[] Multiply(double[][] A, Complex[] b) {
            int m = A.Length;
            int n = A[0].Length;
            Complex[] v = new Complex[m];
            Initialize(v, m);
            for (int k = 0; k < n; k++) {
                for (int i = 0; i < m; i++) {
                    v[i] = ComplexCalc.Add(v[i],
                        ComplexCalc.Multiply(A[i][k], b[k]));
                }
            }
            return v;
        }

        public static Complex[] Multiply(Complex[][] A,
            Complex[] b) {
            int m = A.Length;
            int n = A[0].Length;
            Complex[] v = new Complex[m];
            Initialize(v, m);
            for (int k = 0; k < n; k++) {
                for (int i = 0; i < m; i++) {
                    v[i] = ComplexCalc.Add(v[i],
                        ComplexCalc.Multiply(A[i][k], b[k]));
                }
            }
            return v;
        }

        // ************************* Hadamand product ************************

        public static Complex[][] Hadamard(Complex[][] A,
            Complex[][] B) {
            int m = A.Length;
            int n = A[0].Length;
            Complex[][] C = new Complex[m][];
            Initialize (C, n);
            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    C[i][j] = ComplexCalc.Multiply(A[i][j], B[i][j]);
                }
            }
            return C;
        }

        public static Complex[][] Hadamard(Complex[][] A, double[][] B) {
            int m = A.Length;
            int n = A[0].Length;
            Complex[][] C = new Complex[m][];
            Initialize (C, n);
            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    C[i][j] = ComplexCalc.Multiply(A[i][j], B[i][j]);
                }
            }
            return C;
        }

        public static Complex[][] Hadamard(double[][] A, Complex[][] B) {
            int m = A.Length;
            int n = A[0].Length;
            Complex[][] C = new Complex[m][];
            Initialize (C, n);
            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    C[i][j] = ComplexCalc.Multiply(A[i][j], B[i][j]);
                }
            }
            return C;
        }

        public static double[][] Hadamard(double[][] A, double[][] B) {
            int m = A.Length;
            int n = A[0].Length;
            double[][] C = new double[m][];
            Initialize (C, n);
            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    C[i][j] = A[i][j] * B[i][j];
                }
            }
            return C;
        }

        public static Complex[] Hadamard(double[] A, Complex[] B) {
            int m = A.Length;
            Complex[] C = new Complex[m];
            for (int i = 0; i < m; i++) {
                C[i] = ComplexCalc.Multiply(A[i], B[i]);
            }
            return C;
        }

        public static Complex[] Hadamard(Complex[] A, double[] B) {
            int m = A.Length;
            Complex[] C = new Complex[m];
            for (int i = 0; i < m; i++) {
                C[i] = ComplexCalc.Multiply(A[i], B[i]);
            }
            return C;
        }

        public static Complex[] Hadamard(Complex[] A, Complex[] B) {
            int m = A.Length;
            Complex[] C = new Complex[m];
            for (int i = 0; i < m; i++) {
                C[i] = ComplexCalc.Multiply(A[i], B[i]);
            }
            return C;
        }

        public static double[] Hadamard(double[] A, double[] B) {
            int m = A.Length;
            double[] C = new double[m];
            for (int i = 0; i < m; i++) {
                C[i] = A[i] * B[i];
            }
            return C;
        }

        // ///// inner product

        /**
     * 
     * @param a
     *            a[n]
     * @param b
     *            a[n]
     * @return ab[n] = a dot b
     */
        public static double VectorInnerProduct(double[] a, double[] b) {
            int n = a.Length;
            double ab = 0;
            for (int i = 0; i < n; i++) {
                ab += a[i] * b[i];
            }
            return ab;
        }

        public static Complex VectorInnerProduct(Complex[] a, double[] b) {
            int n = a.Length;
            Complex ab = new Complex();
            for (int i = 0; i < n; i++) {
                ab = ComplexCalc.Add(ab, ComplexCalc.Multiply(a[i], b[i]));
            }
            return ab;
        }

        public static Complex VectorInnerProduct(double[] a, Complex[] b) {
            int n = a.Length;
            Complex ab = new Complex();
            for (int i = 0; i < n; i++) {
                ab = ComplexCalc.Add(ab, ComplexCalc.Multiply(a[i], b[i]));
            }
            return ab;
        }

        public static Complex VectorInnerProduct(Complex[] a,
            Complex[] b) {
            int n = a.Length;
            Complex ab = new Complex();
            ;
            for (int i = 0; i < n; i++) {
                ab = ComplexCalc.Add(ab, ComplexCalc.Multiply(a[i], b[i]));
            }
            return ab;
        }

        // /// tensor product

        public static double[][] VectorOuterProduct(double[] v1, double[] v2) {
            int m = v1.Length;
            int n = v2.Length;
            double[][] C = new double[m][];
            Initialize (C, n);
            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    C[i][j] += v1[i] * v2[j];
                }
            }
            return C;
        }

        public static Complex[][] VectorOuterProduct(Complex[] v1,
            double[] v2) {
            int m = v1.Length;
            int n = v2.Length;
            Complex[][] C = new Complex[m][];
            Initialize (C, n);
            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    C[i][j] = ComplexCalc.Multiply(v1[i], v2[j]);
                }
            }
            return C;
        }

        public static Complex[][] VectorOuterProduct(double[] v1,
            Complex[] v2) {
            int m = v1.Length;
            int n = v2.Length;
            Complex[][] C = new Complex[m][];
            Initialize (C, n);
            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    C[i][j] = ComplexCalc.Multiply(v1[i], v2[j]);
                }
            }
            return C;
        }

        public static Complex[][] VectorOuterProduct(Complex[] v1,
            Complex[] v2) {
            int m = v1.Length;
            int n = v2.Length;
            Complex[][] C = new Complex[m][];
            Initialize (C, n);
            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    C[i][j] = ComplexCalc.Multiply(v1[i], v2[j]);
                }
            }
            return C;
        }

        // //// norms

        public static double Vector2Norm(double[] v) {
            int n = v.Length;
            double norm = 0;
            for (int i = 0; i < n; i++) {
                norm += Math.Pow(v[i], 2);
            }
            norm = Math.Pow(norm, 0.5);
            return norm;
        }

        public static double Vector2Norm(Complex[] v) {
            int n = v.Length;
            double norm = 0;
            for (int i = 0; i < n; i++) {
                norm += Math.Pow(ComplexCalc.Modulus(v[i]), 2);
            }
            norm = Math.Pow(norm, 0.5);
            return norm;
        }

        public static double MatrixFNorm(double[][] A) {
            int m = A.Length;
            int n = A[0].Length;
            double norm = 0;
            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    norm += (Math.Pow(A[i][j], 2));
                }
            }
            norm = Math.Pow(norm, 0.5);
            return norm;
        }

        /**
     * 
     * @param v
     *            vector v[n] as double[]
     * @return matrix F norm of v
     */
        public static double MatrixFNorm(Complex[][] A) {
            int m = A.Length;
            int n = A[0].Length;
            double norm = 0;
            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    norm += Math.Pow(ComplexCalc.Modulus(A[i][j]), 2);
                }
            }
            norm = Math.Pow(norm, 0.5);
            return norm;
        }

        /**
     * 
     * @param v
     *            vector v[n] as double[]
     * @return InfinityNorm(v) ... absolute value of highest magnitude element
     */
        public static double VectorInfinityNorm(double[] v) {
            double lFinity = 0;
            int n = v.Length;
            lFinity = Math.Abs(v[0]);
            for (int i = 1; i < n; i++) {
                double abs = Math.Abs(v[i]);
                if (abs > lFinity) {
                    lFinity = abs;
                }
            }
            return lFinity;
        }

        public static double VectorInfinityNorm(Complex[] vector) {// Linfty
            // Norm
            int n = vector.Length;
            double max = 0;
            for (int i = 0; i < n; i++) {
                double mod = ComplexCalc.Modulus(vector[i]);
                if (mod > max) {
                    max = mod;
                }
            }

            return max;
        }

        // min / max

        public static double Max(List<double[]> vectorList){
            double max = Double.MinValue;
            for(int i = 0;i<vectorList.Count;i++){
                for(int j = 0;j<vectorList[i].Length;j++){
                    if(vectorList[i][j] > max){
                        max=vectorList[i][j];
                    }
                }
            }       
            return max;     
        }

        public static double Min(List<double[]> vectorList){
            double min=Double.MaxValue;
            for(int i = 0;i<vectorList.Count;i++){
                for(int j = 0;j<vectorList[i].Length;j++){
                    if(vectorList[i][j] < min){
                        min=vectorList[i][j];
                    }
                }
            }       
            return min;     
        }

        public static double Min(double[] A) {
            int n = A.Length;
            double min = Double.MaxValue;
            for (int i = 0; i < n; i++) {
                if (A[i] < min) {
                    min = A[i];
                }
            }
            return min;
        }

        public static double Max(double[] A) {
            int n = A.Length;
            double max = Double.MinValue;
            for (int i = 0; i < n; i++) {
                if (A[i] > max) {
                    max = A[i];
                }
            }
            return max;
        }

        public static double Min(double[][] A) {
            int m = A.Length;
            int n = A[0].Length;
            double min = Double.MaxValue;
            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    if (A[i][j] < min) {
                        min = A[i][j];
                    }
                }
            }
            return min;
        }

        public static double Max(double[][] A) {
            int m = A.Length;
            int n = A[0].Length;
            double max = Double.MinValue;
            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    if (A[i][j] > max) {
                        max = A[i][j];
                    }
                }
            }
            return max;
        }

        public static int Max(int[] vector) {
            int max = vector[0];
            for (int i = 1; i < vector.Length; i++) {
                if (vector[i] > max) {
                    max = vector[i];
                }
            }
            return max;
        }

        public static int Min(int[] vector) {
            int min = vector[0];
            for (int i = 1; i < vector.Length; i++) {
                if (vector[i] < min) {
                    min = vector[i];
                }
            }
            return min;
        }

        /**
     * 
     * @param plots
     *            p sets of xy data as a double[p][index][] array where, for p
     *            sets, plots[0:p-1][0] = x and plots[0:p-1][1] = y
     * @param index
     *            0 for x and 1 for y
     * @return minimum of all x or y
     */
        public static double Min(double[][][] plots, int index) {
            int p = plots.Length;
            double min = Double.MaxValue;
            for (int i = 0; i < p; i++) {
                double[] val = plots[i][index];
                double temp = MatrixOps.Min(val);
                if (temp < min) {
                    min = temp;
                }
            }
            return min;
        }

        /**
     * 
     * @param plots
     *            p sets of xy data as a double[p][index][] array where, for p
     *            sets, plots[0:p-1][0] = x and plots[0:p-1][1] = y
     * @param index
     *            0 for x and 1 for y
     * @return maximum of all x or y
     */
        public static double Max(double[][][] plots, int index) {
            int p = plots.Length;
            double max = Double.MinValue;
            for (int i = 0; i < p; i++) {
                double[] val = plots[i][index];
                double temp = MatrixOps.Max(val);
                if (temp > max) {
                    max = temp;
                }
            }
            return max;
        }

        // linear solutions

        /**
     * 
     * @param A
     *            A[m,n] where A is upper triangular
     * @param b
     *            b[m]
     * @return x[n] where Ax = b
     */
        public static double[] BackCalculateX(double[][] upperTriangularMatrix, double[] b)
        {
            int n = upperTriangularMatrix[0].Length;
            b[n - 1] = b[n - 1] / upperTriangularMatrix[n - 1][n - 1];
            for (int i = n - 2; i >= 0; i += -1) {
                double temp = 0;
                for (int j = n - 1; j >= i + 1; j += -1) {
                    temp = temp - upperTriangularMatrix[i][j] * b[j];
                }
                temp = temp + b[i];
                b[i] = temp / upperTriangularMatrix[i][i];
            }
            return b;
        }

        public static Complex[] BackCalculateX(
            double[][] upperTriangularMatrix, Complex[] b) {
            int n = upperTriangularMatrix[0].Length;
            b[n - 1] = ComplexCalc.Divide(b[n - 1],
                upperTriangularMatrix[n - 1][n - 1]);
            for (int i = n - 2; i >= 0; i += -1) {
                Complex temp = new Complex();
                for (int j = n - 1; j >= i + 1; j += -1) {
                    temp = ComplexCalc
                        .Subtract(temp, ComplexCalc.Multiply(
                            upperTriangularMatrix[i][j], b[j]));
                }
                temp = ComplexCalc.Add(temp, b[i]);
                b[i] = ComplexCalc.Divide(temp, upperTriangularMatrix[i][i]);
            }
            return b;
        }

        public static Complex[] BackCalculateX(
            Complex[][] upperTriangularMatrix, double[] b) {
            int n = upperTriangularMatrix[0].Length;
            Complex[] cB = ToComplex(b);
            cB[n - 1] = ComplexCalc.Divide(cB[n - 1],
                upperTriangularMatrix[n - 1][n - 1]);
            for (int i = n - 2; i >= 0; i += -1) {
                Complex temp = new Complex();
                for (int j = n - 1; j >= i + 1; j += -1) {
                    temp = ComplexCalc.Subtract(temp, ComplexCalc.Multiply(
                        upperTriangularMatrix[i][j], cB[j]));
                }
                temp = ComplexCalc.Add(temp, cB[i]);
                cB[i] = ComplexCalc.Divide(temp, upperTriangularMatrix[i][i]);
            }
            return cB;
        }

        public static Complex[] BackCalculateX(
            Complex[][] upperTriangularMatrix, Complex[] b) {
            int n = upperTriangularMatrix[0].Length;
            b[n - 1] = ComplexCalc.Divide(b[n - 1],
                upperTriangularMatrix[n - 1][n - 1]);
            for (int i = n - 2; i >= 0; i += -1) {
                Complex temp = new Complex();
                for (int j = n - 1; j >= i + 1; j += -1) {
                    temp = ComplexCalc
                        .Subtract(temp, ComplexCalc.Multiply(
                            upperTriangularMatrix[i][j], b[j]));
                }
                temp = ComplexCalc.Add(temp, b[i]);
                b[i] = ComplexCalc.Divide(temp, upperTriangularMatrix[i][i]);
            }
            return b;
        }

        public static double[][] CreateVandermonde(double[] x, int order) {
            int m = x.Length;
            int n = order;
            double[][] V = new double[m][];
            Initialize (V, n + 1);
            for (int i = 0; i < m; i++) {
                for (int j = 0; j <= n; j++) {
                    V[i][j] = Math.Pow(x[i], (double) j);
                }
            }
            return V;
        }

        public static Complex[][] CreateVandermonde(Complex[] x,
            int order) {
            int m = x.Length;
            int n = order;
            Complex[][] V = new Complex[m][];
            Initialize (V, n + 1);
            for (int i = 0; i < m; i++) {
                for (int j = 0; j <= n; j++) {
                    V[i][j] = ComplexCalc.Pow(x[i], (double) j);
                }
            }
            return V;
        }
    }
}

