using System;
using Foundation;
using CorePlot;

namespace SpechAuth.iOS.Views.GraphsPage
{
    public class SourceDataSource : CPTScatterPlotDataSource
	{
        private readonly short[] _data;

        public SourceDataSource (short[] data)
        {
            _data = data;
        }

        public override nint NumberOfRecordsForPlot (CPTPlot plot)
        {
            return _data.Length;
        }

        public override NSNumber NumberForPlot (CPTPlot plot, CPTPlotField forFieldEnum, nuint index)
        {
            return forFieldEnum == CPTPlotField.ScatterPlotFieldX ? new NSNumber (index) : new NSNumber(_data [index]);
        }
	}

}

