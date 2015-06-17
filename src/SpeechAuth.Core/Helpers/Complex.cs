using System;
using SpeechAuth.Core.Helpers.Processing;

namespace SpeechAuth.Core.Helpers
{
    public class Complex : IComparable
    {       
        public Complex() {
            Real = 0;
            Imaginary = 0;
        }
        // create a new object with the given real and imaginary parts
        public Complex(double real, double imag) {
            Real = real;
            Imaginary = imag;
        }

        // return a string representation of the invoking Complex object
        public override string ToString()
        {
            if (Imaginary == 0) 
                return Real + "";
            if (Real == 0) 
                return Imaginary + "i";
            if (Imaginary <  0) 
                return Real + " - " + (-Imaginary) + "i";
            
            return Real + " + " + Imaginary + "i";
        }

        // return abs/modulus/magnitude and angle/phase/argument
        public double Abs()   
        { 
            return Math.Sqrt(Real*Real+Imaginary*Imaginary); 
        }
        public double Phase() 
        { 
            return Math.Atan2(Imaginary, Real);   // between -pi and pi
        }

        // return a new Complex object whose value is (this + b)
        public Complex Plus(Complex b) {
            Complex a = this;             // invoking object
            double real = a.Real + b.Real;
            double imag = a.Imaginary + b.Imaginary;
            return new Complex(real, imag);
        }

        // return a new Complex object whose value is (this - b)
        public Complex Minus(Complex b) {
            Complex a = this;
            double real = a.Real - b.Real;
            double imag = a.Imaginary - b.Imaginary;
            return new Complex(real, imag);
        }

        // return a new Complex object whose value is (this * b)
        public Complex Times(Complex b) {
            Complex a = this;
            double real = a.Real * b.Real - a.Imaginary * b.Imaginary;
            double imag = a.Real * b.Imaginary + a.Imaginary * b.Real;
            return new Complex(real, imag);
        }

        // scalar multiplication
        // return a new object whose value is (this * alpha)
        public Complex Times(double alpha) {
            return new Complex(alpha * Real, alpha * Imaginary);
        }

        // return a new Complex object whose value is the conjugate of this
        public Complex Conjugate() {  return new Complex(Real, -Imaginary); }

        // return a new Complex object whose value is the reciprocal of this
        public Complex Reciprocal() {
            double scale = Real*Real + Imaginary*Imaginary;
            return new Complex(Real / scale, -Imaginary / scale);
        }

        // return the real or imaginary part
        public double Real { get; set; }
        public double Imaginary { get; set; }

        // return a / b
        public Complex Divides(Complex b) {
            Complex a = this;
            return a.Times(b.Reciprocal());
        }

        // return a new Complex object whose value is the complex exponential of this
        public Complex Exp() {
            return new Complex(Math.Exp(Real) * Math.Cos(Imaginary), Math.Exp(Real) * Math.Sin(Imaginary));
        }

        // return a new Complex object whose value is the complex sine of this
        public Complex Sin() {
            return new Complex(Math.Sin(Real) * Math.Cosh(Imaginary), Math.Cos(Real) * Math.Sinh(Imaginary));
        }

        // return a new Complex object whose value is the complex cosine of this
        public Complex Cos() {
            return new Complex(Math.Cos(Real) * Math.Cosh(Imaginary), -Math.Sin(Real) * Math.Sinh(Imaginary));
        }

        // return a new Complex object whose value is the complex tangent of this
        public Complex Tan() {
            return Sin().Divides(Cos());
        }

        // a static version of plus
        public static Complex Plus(Complex a, Complex b) {
            double real = a.Real + b.Real;
            double imag = a.Imaginary + b.Imaginary;
            Complex sum = new Complex(real, imag);
            return sum;
        }

        // sample client for testing
        public static void Test() {
            Complex a = new Complex(5.0, 6.0);
            Complex b = new Complex(-3.0, 4.0);

            System.Diagnostics.Debug.WriteLine("a            = " + a);
            System.Diagnostics.Debug.WriteLine("b            = " + b);
            System.Diagnostics.Debug.WriteLine("Re(a)        = " + a.Real);
            System.Diagnostics.Debug.WriteLine("Im(a)        = " + a.Imaginary);
            System.Diagnostics.Debug.WriteLine("b + a        = " + b.Plus(a));
            System.Diagnostics.Debug.WriteLine("a - b        = " + a.Minus(b));
            System.Diagnostics.Debug.WriteLine("a * b        = " + a.Times(b));
            System.Diagnostics.Debug.WriteLine("b * a        = " + b.Times(a));
            System.Diagnostics.Debug.WriteLine("a / b        = " + a.Divides(b));
            System.Diagnostics.Debug.WriteLine("(a / b) * b  = " + a.Divides(b).Times(b));
            System.Diagnostics.Debug.WriteLine("conj(a)      = " + a.Conjugate());
            System.Diagnostics.Debug.WriteLine("|a|          = " + a.Abs());
            System.Diagnostics.Debug.WriteLine("tan(a)       = " + a.Tan());
        }

        #region IComparable implementation
        public int CompareTo (object obj)
        {
            var complex = obj as Complex;
            if (complex == null)
                return 0;

            if (Mel.SpectrumPower(this) < Mel.SpectrumPower(complex))
                return -1;

            if (Mel.SpectrumPower(this) > Mel.SpectrumPower(complex))
                return 1;

            return 0;
        }
        #endregion
    }
}

