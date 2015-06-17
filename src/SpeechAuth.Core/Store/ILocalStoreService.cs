using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using SpeechAuth.Core.Entities;

namespace SpeechAuth.Core.Store
{
    public interface ILocalStoreService
    {
        Task<List<UserInfo>> LoadUsersInfo ();
        Task SaveUserInfo (UserInfo info);
        Task RemoveUser(string id);
    }
}

