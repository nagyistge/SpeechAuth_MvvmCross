using System;
using CorePlot;
using Foundation;

namespace SpechAuth.iOS.Views.GraphsPage
{
    public class TransformDataSource : CPTScatterPlotDataSource
    {
        private readonly double[] _data;
//        public static int Increment = -1;

        public TransformDataSource (double[] data)
        {
            _data = data;
        }

        public override nint NumberOfRecordsForPlot (CPTPlot plot)
        {
            return _data.Length;
        }

        public override NSNumber NumberForPlot (CPTPlot plot, CPTPlotField forFieldEnum, nuint index)
        {
//            Increment++;
            var number = forFieldEnum == CPTPlotField.ScatterPlotFieldX ? new NSNumber (index) : new NSNumber(_data [index]);
            return number;
        }
    }
}

