using System;

namespace SpeechAuth.Core.Exceptions
{
    public class IllegalAccessException : Exception
    {
        public IllegalAccessException(string msg)
            : base (msg)
        {
            
        }
    }
}

