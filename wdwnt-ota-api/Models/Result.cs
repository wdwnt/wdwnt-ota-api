using System.Configuration;
using System.Runtime.Serialization;

namespace wdwnt_ota_api.Models
{
    [DataContract]
    public class Result
    {
        public Result()
        {
            Centova = new Centova();
        }

        [DataMember]
        public Centova Centova { get; set; }

        [DataMember]
        public string Ota_stream_url => ConfigurationManager.AppSettings["OTAStreamUrl"];

        [DataMember]
        public string Wbzw_stream_url => ConfigurationManager.AppSettings["WBZWStreamUrl"];

        [DataMember]
        public string Radio_stream_url => ConfigurationManager.AppSettings["RadioStreamUrl"];

        [DataMember]
        public bool Suggest_radio { get; set; }

        [DataMember]
        public string Error { get; set; }
    }
}