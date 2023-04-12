using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MVCProject.Api.Models.FilterCriterias
{
    [DataContract]
    public class EmailParams
    {
        [DataMember]
        public string[] emailIdTO { get; set; }
        
        [DataMember]
        public string subject { get; set; }

        [DataMember]
        public string body { get; set; }

        [DataMember]
        public string emailIdFrom { get; set; }
        
        [DataMember]
        public string emailPassword { get; set; }

        [DataMember]
        public string Host { get; set; }

        [DataMember]
        public int? Port { get; set; }

        [DataMember]
        public bool EnableSSL { get; set; }

        [DataMember]
        public string[] emailIdCC { get; set; }
        
        [DataMember]
        public string[] emailIdBC { get; set; }
        
        [DataMember]
        public string attachmentFile { get; set; }
    }
}