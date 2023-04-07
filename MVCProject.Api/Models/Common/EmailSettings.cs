using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCProject.Api.Models.Common
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Web.Mvc;

    /// <summary>
    /// Email Settings Model
    /// </summary>
    [DataContract]
    public class EmailSettings
    {
        [DataMember]
        public string SmtpServer { get; set; }

        [DataMember]
        public int SmtpPort { get; set; }


        [DataMember] 
        public bool SmtpUseSsl { get; set; }
        
        [DataMember]
        public string SmtpUsername { get; set; }
        
        [DataMember]
        public string SmtpPassword { get; set; }
    }
}