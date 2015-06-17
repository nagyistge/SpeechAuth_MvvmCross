using System;

namespace SpeechAuth.Core.Entities
{
    public enum PageMode
    {
        Unknown = 0,
        AuthorizationFourier,
        AuthorizationWavelet,
        GraphsFourier,
        GraphsWavelet,
        UserCreation
    }
}

