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
        [DataMember(Name = "ota_stream_url")]
        public string OtaStreamUrl { get; set; }
        [DataMember(Name = "wbzw_stream_url")]
        public string WbzwStreamUrl { get; set; }
        [DataMember(Name = "suggest_radio")]
        public bool SuggestRadio { get; set; }
        public string Error { get; set; }
    }
}