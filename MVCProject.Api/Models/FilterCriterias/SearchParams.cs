using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MVCProject.Api.Models.FilterCriterias
{
    public class SearchParams
    {
        [DataMember]
        public int? StatusId { get; set; }

        [DataMember]
        public int? CompanyId { get; set; }

        [DataMember]
        public int? PositionId { get; set; }

        [DataMember]
        public DateTime? StartDate { get; set; }

        [DataMember]
        public DateTime? EndDate { get; set; }
    }
}