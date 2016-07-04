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
        public string OtaStreamUrl => "http://23.95.25.17:8142/;";

        [DataMember(Name = "wbzw_stream_url")]
        public string WbzwStreamUrl => "http://14033.live.streamtheworld.com:3690/WBZWAMAAC_SC";

        [DataMember(Name = "suggest_radio")]
        public bool SuggestRadio { get; set; }

        [DataMember]
        public string Error { get; set; }
    }
}