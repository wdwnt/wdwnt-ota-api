using System.Configuration;
using System.Runtime.Serialization;

namespace wdwnt_ota_api.Models
{
    [DataContract]
    public class Centova
    {
        [DataMember]
        public string Album { get; set; }

        [DataMember]
        public string Artist { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public int Refresh_interval => int.Parse(ConfigurationManager.AppSettings["RefreshInterval"]);
    }
}