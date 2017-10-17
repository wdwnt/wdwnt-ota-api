using System.Runtime.Serialization;

namespace WdwntOtaApi.Models
{
    [DataContract]
    public class ApiV2Result : BaseResult
    {
        public ApiV2Result()
        {
            Airtime = new Airtime();
        }

        [DataMember]
        public Airtime Airtime { get; set; }
    }
}