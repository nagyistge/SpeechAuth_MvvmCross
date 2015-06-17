using System;
using SpeechAuth.Core.Entities;
using Cirrious.MvvmCross.Plugins.Messenger;

namespace SpeechAuth.Core.Messages
{
    public class UserCreatedMessage : MvxMessage
    {
        public User User { get; private set;}

        public UserCreatedMessage(object sender, User user)
            : base (sender)
        {
            User = user;
        }
    }
}

