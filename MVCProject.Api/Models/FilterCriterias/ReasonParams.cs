using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MVCProject.Api.Models.FilterCriterias
{
    public class ReasonParams
    {
        [DataMember]
        public int? ReasonId { get; set; }
        public string Reason { get; set; }
    }
}