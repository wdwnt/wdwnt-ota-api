﻿using System.Configuration;
using System.Runtime.Serialization;

namespace WdwntOtaApi.Models
{
    [DataContract]
    public class BaseResult
    {
        public BaseResult()
        {
            Centova = new Centova();
        }

        [DataMember]
        public Centova Centova { get; set; }
        
        [DataMember]
        public string Ota_stream_url { get; set; }

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