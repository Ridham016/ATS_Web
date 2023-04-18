using MVCProject.Api.Models;
using MVCProject.Api.Utilities;
using MVCProject.Api.ViewModel;
using MVCProject.Common.Resources;
using NPOI.HSSF.Record;
#region namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
#endregion

namespace MVCProject.Api.Controllers.JobListing
{
    public class JobListingController : BaseController
    {
        private MVCProjectEntities entities;

        public JobListingController()
        {
            this.entities = new MVCProjectEntities();
        }

        [HttpGet]
        public ApiResponse GetJobPostingList()
        {
            var data = this.entities.USP_ATS_JobListing().Select(g => new
            {
                PostingId = g.PostingId,
                CompanyId = g.CompanyId,
                PositionId = g.PositionId,
                CompanyName = g.CompanyName,
                CompanyVenue = g.CompanyVenue,
                PositionName = g.PositionName,
                Experience = g.Experience,
                Salary = g.Salary,
                EntryDate = g.EntryDate,
                Posted = g.Posted,
                IsActive = g.IsActive,
            }).ToList();
            return this.Response(MessageTypes.Success, string.Empty, data);
        }

    }
}