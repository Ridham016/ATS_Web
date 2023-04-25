using MVCProject.Api.Models;
using MVCProject.Api.Models.FilterCriterias;
using MVCProject.Api.Utilities;
using MVCProject.Common.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MVCProject.Api.Controllers.Dashboard
{
    public class DashboardController : BaseController
    {
        private MVCProjectEntities entities;

        public DashboardController()
        {
            this.entities = new MVCProjectEntities();
        }
        [HttpGet]
        public ApiResponse GetEvents()
        {

            var Calendardisplay = entities.USP_ATS_ScheduleInformation().ToList();
            return this.Response(MessageTypes.Success, string.Empty, Calendardisplay);

        }
        [HttpGet]
        public ApiResponse GetCounts(int timeFrame = 3)
        {

            var counts = entities.USP_ATS_CountForDashboard(timeFrame).FirstOrDefault();
            if(counts != null)
            {
                return this.Response(MessageTypes.Success, string.Empty, counts);
            }
            return this.Response(Utilities.MessageTypes.Error, Resource.Error);

        }
    }
}
