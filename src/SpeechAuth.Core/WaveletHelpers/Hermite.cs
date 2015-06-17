﻿using System;

namespace SpeechAuth.Core.WaveletHelpers
{
    public class Hermite
    {
        /*************************************************************************
            Calculation of the value of the Hermite polynomial.

            Parameters:
                n   -   degree, n>=0
                x   -   argument

            Result:
                the value of the Hermite polynomial Hn at x
            *************************************************************************/
        public static double PhysisistHermitePoly(int n,
            double x)
        {
            double result = 0;
            int i = 0;
            double a = 0;
            double b = 0;


            //
            // Prepare A and B
            //
            a = 1;
            b = 2 * x;

            //
            // Special cases: N=0 or N=1
            //
            if (n == 0)
            {
                result = a;
                return result;
            }
            if (n == 1)
            {
                result = b;
                return result;
            }

            //
            // General case: N>=2
            //
            for (i = 2; i <= n; i++)
            {
                result = 2 * x * b - 2 * (i - 1) * a;
                a = b;
                b = result;
            }
            return result;
        }


        /*************************************************************************
            Summation of Hermite polynomials using Clenshaw’s recurrence formula.

            This routine calculates
                c[0]*H0(x) + c[1]*H1(x) + ... + c[N]*HN(x)

            Parameters:
                n   -   degree, n>=0
                x   -   argument

            Result:
                the value of the Hermite polynomial at x
            *************************************************************************/
        public static double Hermitesum(double[] c,
            int n,
            double x)
        {
            double result = 0;
            double b1 = 0;
            double b2 = 0;
            int i = 0;

            b1 = 0;
            b2 = 0;
            for (i = n; i >= 0; i--)
            {
                result = 2 * (x * b1 - (i + 1) * b2) + c[i];
                b2 = b1;
                b1 = result;
            }
            return result;
        }


        /*************************************************************************
            Representation of Hn as C[0] + C[1]*X + ... + C[N]*X^N

            Input parameters:
                N   -   polynomial degree, n>=0

            Output parameters:
                C   -   coefficients
            *************************************************************************/
        public static void Hermitecoefficients(int n,
            double[] c)
        {
            int i = 0;

            c = new double[n + 1];
            for (i = 0; i <= n; i++)
            {
                c[i] = 0;
            }
            c[n] = Math.Exp(n * Math.Log(2));
            for (i = 0; i <= n / 2 - 1; i++)
            {
                c[n - 2 * (i + 1)] = -(c[n - 2 * i] * (n - 2 * i) * (n - 2 * i - 1) / 4 / (i + 1));
            }
        }

        public static double ProbabilistHermitePoly(int n, double z)
        {
            double m = (double)n;
            double pHpoly = PhysisistHermitePoly(n, z / Math.Pow(2, .5));
            double result = pHpoly / (Math.Pow(2, m / 2));
            return result;
        }           

    }
}

