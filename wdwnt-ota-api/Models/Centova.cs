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
    }
}