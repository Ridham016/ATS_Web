// -----------------------------------------------------------------------
// <copyright file="SchedulesController.cs" company="ASK E-Sqaure">
// All copy rights reserved @ASK E-Sqaure.
// </copyright>
// ----------------------------------------------------------------------- 

namespace MVCProject.Api.Controllers.ScheduleManagement
{
    using MVCProject.Api.Models;
    using MVCProject.Api.Utilities;
    using MVCProject.Api.ViewModel;
    using MVCProject.Common.Resources;
    using NPOI.HSSF.Record;
    #region Namespaces
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    #endregion
    public class SchedulesController : BaseController
    {
        private MVCProjectEntities entities;

        public SchedulesController()
        {
            this.entities = new MVCProjectEntities();
        }

        [HttpPost]
        public ApiResponse GetApplicantList(PagingParams applicantDetailParams)
        {
            var applcantlist = (from g in this.entities.ApplicantRegisters.Where(x => (x.IsActive.Value)).AsEnumerable()
                                let TotalRecords = this.entities.ApplicantRegisters.Where(x => (x.IsActive.Value)).AsEnumerable().Count()
                                select new
                                {
                                    ApplicantId = g.ApplicantId,
                                    FirstName = g.FirstName,
                                    MiddleName = g.MiddleName,
                                    LastName = g.LastName,
                                    Email = g.Email,
                                    Phone = g.Phone,
                                    Address = g.Address,
                                    DateOfBirth = g.DateOfBirth,
                                    CurrentCompany = g.CurrentCompany,
                                    CurrentDesignation = g.CurrentDesignation,
                                    ApplicantDate = g.ApplicantDate,
                                    TotalExperience = g.TotalExperience,
                                    DetailedExperience = g.DetailedExperience,
                                    CurrentCTC = g.CurrentCTC,
                                    ExpectedCTC = g.ExpectedCTC,
                                    NoticePeriod = g.NoticePeriod,
                                    CurrentLocation = g.CurrentLocation,
                                    PreferedLocation = g.PreferedLocation,
                                    ReasonForChange = g.ReasonForChange,
                                    IsActive = g.IsActive,
                                    TotalRecords
                                }).AsQueryable().Skip((applicantDetailParams.CurrentPageNumber - 1) * applicantDetailParams.PageSize).Take(applicantDetailParams.PageSize);
            return this.Response(MessageTypes.Success, string.Empty, applcantlist);
        }
        
    }
}
