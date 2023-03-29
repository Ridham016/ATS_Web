using MVCProject.Api.Models;
using MVCProject.Api.Utilities;
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
    }
}
