using System;

namespace SpeechAuth.Core.ViewModels.GraphsPage
{
    public interface IGraphsPageView
    {
        void UpdateSourceGraph (short[] data);
        void UpdateTransformGraph (double[] data);
    }
}

