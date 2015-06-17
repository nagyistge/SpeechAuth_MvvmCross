using System;
using Cirrious.MvvmCross.Plugins.Sqlite;

namespace SpeechAuth.Core.Store
{
    [Table("UserInfo")]
    internal class UserInfoDTO
	{
        [PrimaryKey]
        public string Id { get; set; }
        public string User { get; set; }
        public string CentersF { get; set; }
        public string CentersW { get; set; }
	}
}

