using MVCProject.Api.Models;
using MVCProject.Api.Models.FilterCriterias;
using MVCProject.Api.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MVCProject.Api.Controllers.Calendar
{
    public class CalendarController : BaseController
    {
        private MVCProjectEntities entities;

        public CalendarController()
        {
            this.entities = new MVCProjectEntities();
        }
        [HttpGet]
        public ApiResponse GetEvents()
        {

            var Calendardisplay = entities.USP_ATS_ScheduleInformation().ToList();
            return this.Response(MessageTypes.Success, string.Empty, Calendardisplay);

        }
        [HttpPost]
        public ApiResponse GetEventWithDate([FromBody]SearchParams searchParams)
        {

            var Calendardisplay = entities.USP_ATS_GetScheduleData(searchParams.StartDate,searchParams.EndDate).ToList();
            return this.Response(MessageTypes.Success, string.Empty, Calendardisplay);

        }
    }
}
