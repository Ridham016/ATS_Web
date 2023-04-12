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
        public String Subject { get; set; }

        [DataMember]
        public string Body { get; set; }
    }
}