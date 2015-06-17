using System;

namespace SpeechAuth.Core.Helpers.Preprocess
{
    public static class Preprocessing
    {
        public static double[] Handle(double [] x){
            return DeleteSteadyComponent(x);
        }

        private static double[] DeleteSteadyComponent(double[] x){
            double average = 0;
            foreach (var a in x){
                average += a;
            }
            average /= x.Length;
            for (int i = 0; i < x.Length; i++) {
                x[i] -= average;
            }
            return x;
        }

        public static void Test() {
            double[] x = {1, 2, 3, 4, 5, 6};
            x = DeleteSteadyComponent(x);
            for (int i = 0; i < x.Length; i++) {                
                System.Diagnostics.Debug.WriteLine(x[i] + " ");
            }
        }
    }
}

