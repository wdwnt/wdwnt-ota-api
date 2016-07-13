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
        public string Ota_stream_url => "http://50.7.96.210:8290/stream";

        [DataMember]
        public string Wbzw_stream_url => "http://14033.live.streamtheworld.com:3690/WBZWAMAAC_SC";

        [DataMember]
        public bool Suggest_radio { get; set; }

        [DataMember]
        public string Error { get; set; }
    }
}