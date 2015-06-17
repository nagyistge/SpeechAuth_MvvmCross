using System;
using SpeechAuth.Core.Helpers;

namespace SpeechAuth.Core.WaveletHelpers
{
    public class ComplexCalc
    {
        private static Complex negitive_i;

        public static Complex E() {
            Complex e = new Complex(Math.E, 0);
            return e;
        }

        public static Complex Pi() {
            Complex pi = new Complex(Math.PI, 0);
            return pi;
        }

        public static Complex I() {
            Complex sqrtMinus1 = new Complex(0, 1.0);
            return sqrtMinus1;
        }

        public static Complex MinusOne() {
            Complex cM1 = new Complex(-1.0, 0);
            return cM1;
        }

        public static Complex Zero() {
            Complex zero = new Complex();
            return zero;
        }

        public static Complex MinusI() {
            Complex minusI = new Complex(0, -1);
            return minusI;
        }

        public static Complex PointFive() {
            Complex point5 = new Complex(0.5, 0);
            return point5;
        }

        public static Complex One() {
            Complex one = new Complex(1.0, 0);
            return one;
        }

        public static Complex Two() {
            Complex two = new Complex(2.0, 0);
            return two;
        }

        public static Boolean IsEqual(Complex x, Complex y) {
            if (x.Real == y.Real && x.Imaginary == y.Imaginary) {
                return true;
            } else {
                return false;
            }
        }

        public static Boolean IsZero(Complex z) {
            if (z.Real == 0 && z.Imaginary == 0) {
                return true;
            } else {
                return false;
            }
        }

        public static Complex ToComplex(double real) {
            Complex result = new Complex(real, 0);
            return result;
        }

        public static Complex[] ToComplex(double[] real) {
            int len = real.Length;
            Complex[] result = new Complex[len];
            for (int i = 0; i < len; i++) {
                result[i].Real = real[i];
            }
            return result;
        }

        public static Complex Add(Complex x, Complex y) {
            return new Complex(x.Real + y.Real, x.Imaginary + y.Imaginary);
        }

        public static Complex Add(double x, Complex y) {
            return Add(ToComplex(x), y);
        }

        public static Complex Add(Complex x, double y) {
            return Add(x, ToComplex(y));
        }

        public static Complex Add(double x, double y) {
            return ToComplex(x + y);
        }

        public static Complex Subtract(Complex x, Complex y) {
            return new Complex(x.Real - y.Real, x.Imaginary - y.Imaginary);
        }

        public static Complex Subtract(double x, Complex y) {
            return Subtract(ToComplex(x), y);
        }

        public static Complex Subtract(Complex x, double y) {
            return Subtract(x, ToComplex(y));
        }

        public static Complex Subtract(double x, double y) {
            return ToComplex(x - y);
        }

        public static Complex Multiply(Complex x, Complex y) {
            Complex result = new Complex();
            result.Real = (x.Real * y.Real) - (x.Imaginary * y.Imaginary);
            result.Imaginary = (x.Real * y.Imaginary) + (x.Imaginary * y.Real);
            return result;
        }

        public static Complex Multiply(double x, Complex y) {
            return Multiply(ToComplex(x), y);
        }

        public static Complex Multiply(Complex x, double y) {
            return Multiply(x, ToComplex(y));
        }

        public static Complex Multiply(double x, double y) {
            return ToComplex(x * y);
        }

        public static Complex Divide(Complex num, Complex denom) {
            Complex result = new Complex();
            double divisor = (Math.Pow(denom.Real, 2))
                + (Math.Pow(denom.Imaginary, 2));
            result.Real = ((num.Real * denom.Real) + (num.Imaginary * denom.Imaginary))
                / divisor;
            result.Imaginary = ((num.Imaginary * denom.Real) - (num.Real * denom.Imaginary))
                / divisor;
            return result;
        }

        public static Complex Divide(double num, Complex denom) {
            return Divide(ToComplex(num), denom);
        }

        public static Complex Divide(Complex num, double denom) {
            return Divide(num, ToComplex(denom));
        }

        public static Complex Divide(double num, double denom) {
            return ToComplex(num / denom);
        }

        public static double Modulus(Complex z) {
            return Math.Pow((Math.Pow(z.Real, 2) + Math.Pow(z.Imaginary, 2)), 0.5);
        }

        public static double Modulus(double z) {
            return Math.Abs(z);
        }

        public static double Arg(Complex z) {
            if (z.Real == 0) {
                if (z.Imaginary < 0) {
                    return (-.5 * Math.PI);
                } else if (z.Imaginary > 0) {
                    return (.5 * Math.PI);
                } else {
                    return 0;
                }
            } else if (z.Imaginary == 0) {
                if (z.Real > 0) {
                    return 0;
                } else {
                    return Math.PI;
                }
            } else {
                return Math.Atan2(z.Imaginary, z.Real);

            }
        }

        public static double Arg(double z) {
            if (z >= 0) {
                return 0;
            } else {
                return Math.PI;
            }
        }

        public static Complex Conjugate(Complex z) {
            Complex result = new Complex(z.Real, -z.Imaginary);
            return result;
        }

        public static Complex Conjugate(double z) {
            return ToComplex(z);
        }

        public static double AbsoluteSquare(Complex z) {
            Complex result = new Complex();
            result = Multiply(z, Conjugate(z));
            return result.Real;
        }

        public static Complex Ln(Complex z) {
            Complex result = new Complex();
            result.Real = Math.Log(Modulus(z));
            result.Imaginary = Arg(z);
            return result;
        }

        public static Complex Ln(double x) {
            return Ln(ToComplex(x));
        }

        public static Complex LogBaseB(Complex z, Complex Base) {
            Complex result = new Complex();
            result = Divide(Ln(z), Ln(Base));
            return result;
        }

        public static Complex Sin(Complex z) {
            Complex denom = new Complex(0, 2);
            Complex num1 = new Complex(-z.Imaginary, z.Real);
            Complex num2 = new Complex(z.Imaginary, -z.Real);
            return Divide(Subtract(Exp(num1), Exp(num2)), denom);
        }

        public static Complex Sin(double z) {
            return ToComplex(Math.Sin(z));
        }

        public static Complex ArcSin(Complex z) {
            Complex result = new Complex();
            Complex f = new Complex();
            f = Ln(Add(Multiply(I(), z),
                Pow(Subtract(1, Pow(z, Two())), PointFive())));
            result = Multiply(negitive_i, f);
            return result;
        }

        public static Complex Cos(Complex z) {
            Complex num1 = new Complex(-z.Imaginary, z.Real);
            Complex num2 = new Complex(z.Imaginary, -z.Real);
            return Divide(Add(Exp(num1), Exp(num2)), 2);
        }

        public static Complex Cos(double z) {
            return ToComplex(Math.Cos(z));
        }

        public static Complex ArcCos(Complex z) {
            return Subtract(Divide(Pi(), Two()), ArcSin(z));
        }

        public static Complex Tan(Complex z) {
            return Divide(Sin(z), Cos(z));
        }

        public static Complex ArcTan(Complex z) {
            Complex result = new Complex();
            result = Subtract(Ln(Subtract(1, Multiply(I(), z))),
                Ln(Add(1, Multiply(I(), z))));
            result = Multiply(Divide(I(), Two()), result);
            return result;
        }

        public static Complex Sinh(Complex z) {
            Complex neg = new Complex();
            neg.Real = -z.Real;
            neg.Imaginary = -z.Imaginary;
            return Divide(Subtract(Exp(z), Exp(neg)), Two());
        }

        public static Complex ArcSinh(Complex z) {
            return Ln(Add(z, Pow(Add(Pow(z, Two()), 1), PointFive())));
        }

        public static Complex Cosh(Complex z) {
            Complex neg = new Complex(-z.Real, -z.Imaginary);
            return Divide(Add(Exp(z), Exp(neg)), Two());
        }

        public static Complex ArcCosh(Complex z) {
            return Ln(Add(
                z,
                Multiply(Pow(Add(z, MinusOne()), PointFive()),
                    Pow(Add(z, 1), PointFive()))));
        }

        public static Complex Tanh(Complex z) {
            return Divide(Sinh(z), Cosh(z));
        }

        public static Complex ArcTanh(Complex z) {
            return Divide(Subtract(Ln(Add(1, z)), Ln(Subtract(1, z))), Two());
        }

        public static Complex Reciprocal(Complex z) {
            return Divide(1, z);
        }

        public static Complex Reciprocal(double z) {
            return ToComplex(1 / z);
        }

        public static Complex Exp(double z) {
            return Pow(E(), z);
        }

        public static Complex Exp(Complex z) {
            return Pow(E(), z);
        }

        public static Complex Pow(double z, double exponent) {
            return new Complex(Math.Pow(z, exponent), 0);
        }

        public static Complex Pow(Complex z, double exponent) {
            Complex result = new Complex();
            if (z.Real > 0 && z.Imaginary == 0) {
                result.Imaginary = 0;
                result.Real = Math.Pow(z.Real, exponent);
            }
            double iDbl = 0;
            int i;
            int j;

            if (exponent >= 1 || exponent <= -1) {
                iDbl = Math.Abs((int) exponent);
                i = (int) iDbl; // truncate
                result = z;
                for (j = 1; j <= i - 1; j++) {
                    result = Multiply(z, result);
                }
                double newPow = Math.Abs(exponent) - iDbl;
                if (newPow != 0) {
                    Complex temp = new Complex();
                    double r = Modulus(z);
                    double coeff = Math.Pow(r, newPow);
                    double arg = Arg(z);
                    temp.Real = coeff * (Math.Cos(arg * newPow));
                    temp.Imaginary = coeff * (Math.Sin(arg * newPow));
                    result = Multiply(temp, result);
                }
                if (exponent < 0) {
                    return Divide(1, result);
                } else {
                    return result;
                }
            } else {
                double r = Modulus(z);
                double coeff = Math.Pow(r, exponent);
                double arg = Arg(z);
                if (z.Real < 0 && z.Imaginary == 0
                    && (exponent == 0.5 || exponent == -0.5)) {
                    result.Real = 0; // cos(pi/2) should be 0 but isn't in floating
                    // point precision
                } else {
                    result.Real = coeff * (Math.Cos(arg * exponent));
                }
                result.Imaginary = coeff * (Math.Sin(arg * exponent));
                return result;
            }
        }

        public static Complex Pow(Complex z, Complex exponent) {
            if (exponent.Imaginary == 0) {
                return Pow(z, exponent.Real);
            }
            double coeff = (Math.Pow(
                Math.Pow(z.Real, 2) + Math.Pow(z.Imaginary, 2),
                exponent.Real / 2))
                * Math.Exp(-exponent.Imaginary * Arg(z));
            Complex result = new Complex();
            result.Real = coeff
                * Math.Cos((exponent.Real * Arg(z))
                    + (exponent.Imaginary
                        * Math.Log(Math.Pow(z.Real, 2)
                            + Math.Pow(z.Imaginary, 2)) / 2));
            result.Imaginary = coeff
                * Math.Sin((exponent.Real * Arg(z))
                    + (exponent.Imaginary
                        * Math.Log(Math.Pow(z.Real, 2)
                            + Math.Pow(z.Imaginary, 2)) / 2));
            return result;
        }

        public static Complex Pow(double z, Complex exponent) {
            return Pow(ToComplex(z), exponent);
        }

        public static Complex Sign(Complex z) {
            if (IsZero(z)) {
                return Zero();
            } else {
                return Divide(z, Modulus(z));
            }
        }
    }
}

