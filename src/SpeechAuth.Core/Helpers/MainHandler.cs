using System;
using System.Collections.Generic;
using SpeechAuth.Core.Exceptions;
using System.IO;
using SpeechAuth.Core.Helpers.Preprocess;
using SpeechAuth.Core.Helpers.Processing;
using SpeechAuth.Core.Entities;
using SpeechAuth.Core.Store;
using System.Threading.Tasks;
using SpeechAuth.Core.Messages;
using Cirrious.CrossCore;

namespace SpeechAuth.Core.Helpers
{
    public static class MainHandler
    {
        public static int FRAME_LENGTH = 2048;
        public static int SAMPLING_FREQ = 16000;
        public static double MAX_DISTANCE = 27.0;
        public static double WMAX_DISTANCE = 19.0;//12.0;
        public static int CLUSTER_NUMBER = 4;
        public static int ITERATION_NUMBER = 100;

        public static async void AddUser(User user, List<double[]> audioBufferF, List<double[]> audioBufferW) 
        {
            var clusterF = Task.Run<List<double[]>> (() => Cluster (audioBufferF));
            var clusterW = Task.Run<List<double[]>> (() => Cluster (audioBufferW));

            await Task.WhenAll (new [] { clusterF, clusterW });

            Save(user, clusterF.Result, clusterW.Result);
        }

        public static void RemoveUser(String id) 
        {
            try {
                Remove(id);
            } catch (IllegalAccessException e) {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
            }
        }

        public static async Task<string> CheckVoice(short[] audioBuffer, Transform transformation)
        {
            double[] mels = await GetMelKreps(audioBuffer, transformation);
            return await Check(mels, transformation);
        }

        private static List<double[]> Cluster (List<double[]> list)
        {
            var arr = new double[list.Count, list[0].Length];
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list[0].Length; j++)
                {
                    arr[i, j] = list[i][j];
                }
            }

            alglib.clusterizerstate s;
            alglib.kmeansreport rep;

            alglib.clusterizercreate(out s);
            alglib.clusterizersetpoints(s, arr, 2);
            alglib.clusterizersetkmeanslimits(s, 5, ITERATION_NUMBER);
            alglib.clusterizerrunkmeans(s, CLUSTER_NUMBER, out rep);

            List<double[]> result = new List<double[]>();
            for (int i = 0; i < CLUSTER_NUMBER; i++)
            {
                int count = rep.c.Length / CLUSTER_NUMBER;
                var subResult = new double[count];
                for (int j = 0; j < count; j++)
                    subResult[j] = rep.c[i, j];

                var temp = Center (new List<double[]> { subResult });
                result.Add(temp);
            }

            return result;
        }

        private static double[] Center(List<double[]> cluster) 
        {
            List<double> result = new List<double>();
            for (int i = 0; i < Mel.NUMBER_OF_FILTERS; i++) {
                double tmp = 0;
                for (int j = 0; j < cluster.Count; j++) {
                    tmp += cluster[j][i];
                }
                result.Add(tmp / cluster.Count);
            }
            return result.ToArray();
        }

        private static string _name = string.Empty;
        private static async Task<string> Check(double[] mels, Transform transformation)
        {         
            var users = await LocalStore.LoadUsersInfo();
            double distance = 99999;
            try 
            {
                foreach (var user in users) 
                {                    
                    double tmp = 99999;
                    if (transformation == Transform.Fourier)
                    {
                        tmp = GetDistance(
                            user.User.Surname + " " + user.User.Name.ToUpperInvariant()[0] + "." + user.User.MidName.ToUpperInvariant()[0],
                            user.CentersF, 
                            mels
                        );
                    }
                    else if (transformation == Transform.Wavelet)
                    {
                        tmp =  GetDistance(
                            user.User.Surname + " " + user.User.Name.ToUpperInvariant()[0] + "." + user.User.MidName.ToUpperInvariant()[0],
                            user.CentersW, 
                            mels
                        );
                    }

                    if (tmp < distance)
                    {
                        distance = tmp;
                        _name = user.User.Surname + " " + user.User.Name.ToUpperInvariant()[0] + "." + user.User.MidName.ToUpperInvariant()[0];
                    }
                }
            } 
            catch (FileNotFoundException)
            {
                System.Diagnostics.Debug.WriteLine("WTF?");
            }

            if (transformation == Transform.Fourier && distance > MAX_DISTANCE)
                _name = "UNKNOWN";
            if (transformation == Transform.Wavelet && distance > WMAX_DISTANCE)
                _name = "UNKNOWN";

            return _name;
        }

        private static double GetDistance(string name, List<double[]> userCenters, double[] mels)
        {
            double distance = 99999;
            for (int i = 0; i < CLUSTER_NUMBER; i++) {
                double tmp = GetDistance(userCenters[i], mels);
                System.Diagnostics.Debug.WriteLine(tmp + " " + name);
                distance = (distance > tmp) ? tmp : distance;
            }
            return distance;
        }

        private static double GetDistance(double[] mels1, double[] mels2) 
        {
            double distance = 0;
            for (int i = 0; i < Mel.NUMBER_OF_FILTERS; i++) {
                distance += Math.Abs(mels1[i] - mels2[i]);
            }
            return distance;
        }

        private static async void Remove(String id)
        {
            await LocalStore.RemoveUser(id);
        }

        private static async void Save(User user, List<double[]> melsF, List<double[]> melsW)
        {
            try 
            {                
                await LocalStore.SaveUserInfo(new UserInfo
                    {
                        Id = Guid.NewGuid().ToString(),
                        User = user,
                        CentersF = melsF,
                        CentersW = melsW
                    });

            } 
            catch (Exception e) 
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
            }
        }

        public static async Task<double[]> GetMelKreps(short[] audioBuffer, Transform transformation)
        {
            double [] tmp = Preprocessing.Handle(Normalize(audioBuffer));
            double [][] audioFrames = DivToFrames(tmp);
            return await Mel.GetMelKrepsCoef(audioFrames, transformation);
        }

        public static double[] Normalize(short[] audioBuffer) 
        {
            short max = FindMax(audioBuffer);
            double[] newBuffer = new double[audioBuffer.Length];
            for(int i = 0; i < audioBuffer.Length; i++){
                newBuffer[i] = (double)audioBuffer[i] / max;
            }
            return newBuffer;
        }

        private static short FindMax(short[] audioBuffer) 
        {
            short max = (short)Math.Abs(audioBuffer[0]);
            for (int i = 1; i < audioBuffer.Length; i++){
                if (Math.Abs(audioBuffer[i]) > max){
                    max = audioBuffer[i];
                }
            }
            return max;
        }

        public static double[][] DivToFrames(double[] buffer) 
        {
            if (buffer.Length < FRAME_LENGTH) {
                throw new ArithmeticException("too short signal");
            }
            int N = 2 * buffer.Length / FRAME_LENGTH - 1;
            double[][] frames = new double[N][];
            for (int i = 0; i < N; i++){
                frames[i] = new double[FRAME_LENGTH];
                Array.Copy(buffer, i * (FRAME_LENGTH / 2), frames[i], 0, FRAME_LENGTH);
            }
            return frames;
        }

        private static ILocalStoreService LocalStore { get { return Mvx.Resolve<ILocalStoreService> (); } }
    }
}

