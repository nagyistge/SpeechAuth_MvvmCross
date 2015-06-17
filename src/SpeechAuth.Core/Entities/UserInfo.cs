using System.Collections.Generic;

namespace SpeechAuth.Core.Entities
{
    public class UserInfo
    {
        public string Id { get; set; }
        public User User { get; set; }
        public List<double[]> CentersF { get; set; }
        public List<double[]> CentersW { get; set; }
    }
}

