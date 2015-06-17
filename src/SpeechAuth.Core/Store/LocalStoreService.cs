using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using SpeechAuth.Core.Entities;
using System.Linq;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Sqlite;
using Cirrious.MvvmCross.Plugins.Messenger;
using SpeechAuth.Core.Messages;

namespace SpeechAuth.Core.Store
{
    public class LocalStoreService : ILocalStoreService
    {
        private readonly ISQLiteConnection _connection;
        private readonly IMvxMessenger _messenger;

        public LocalStoreService (ISQLiteConnectionFactory factory, IMvxMessenger messenger)
        {
            _messenger = messenger;
            _connection = factory.Create ("users.sql");

            CreateTables ();
        }

        protected void UpgradeFromVersion(int version)
        {
            _connection.DropTable<UserInfoDTO> ();

            CreateTables();
        }

        protected void CreateTables()
        {
            _connection.CreateTable<UserInfoDTO> ();
        }

        public Task<List<UserInfo>> LoadUsersInfo()
        {
            return Task<List<UserInfo>>.Factory.StartNew (() => {                
                var users = _connection.Table<UserInfoDTO> ()
                        .ToList ();
                return users.Select (
                    x => new UserInfo {
                        Id = x.Id,
                        User = Newtonsoft.Json.JsonConvert.DeserializeObject<User> (x.User),
                        CentersF = Newtonsoft.Json.JsonConvert.DeserializeObject<List<double[]>> (x.CentersF),
                        CentersW = Newtonsoft.Json.JsonConvert.DeserializeObject<List<double[]>> (x.CentersW)
                    }).ToList ();                
            });
        }

        public Task SaveUserInfo(UserInfo info)
        {
            return Task.Factory.StartNew (() => {
                _connection.BeginTransaction ();
                try {
                    _connection.Insert (new UserInfoDTO {
                        Id = info.Id,
                        User = Newtonsoft.Json.JsonConvert.SerializeObject (info.User),
                        CentersF = Newtonsoft.Json.JsonConvert.SerializeObject (info.CentersF),
                        CentersW = Newtonsoft.Json.JsonConvert.SerializeObject (info.CentersW)
                    });

                    _connection.Commit ();

                    _messenger.Publish<UserCreatedMessage>(
                        new UserCreatedMessage (this, info.User)
                    );
                } catch {
                    _connection.Rollback ();
                }
            });
        }

        public Task RemoveUser(string id)
        {
            return Task.Factory.StartNew (() => {                                    
                _connection.BeginTransaction ();

                try {
                    _connection.Delete<UserInfoDTO> (id);

                    _connection.Commit ();
                } catch {
                    _connection.Rollback ();
                }
            });
        }
    }
}

