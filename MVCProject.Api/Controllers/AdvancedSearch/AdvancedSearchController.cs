using iTextSharp.text.log;
using MVCProject.Api.Models;
using MVCProject.Api.Models.FilterCriterias;
using MVCProject.Api.Utilities;
using MVCProject.Api.ViewModel;
using MVCProject.Common.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MVCProject.Api.Controllers.AdvancedSearch
{
    public class AdvancedSearchController : BaseController
    {
        private MVCProjectEntities entities;

        public AdvancedSearchController()
        {
            this.entities = new MVCProjectEntities();
        }
        [HttpPost]
        public ApiResponse AdvancedActionSearch([FromBody] PagingParams searchDetailParams, [FromUri]SearchParams searchParams)
        {
            var result = this.entities.USP_ATS_ActionApplicantSearch(searchParams.StatusId, searchParams.StartDate, searchParams.EndDate).ToList();
            var TotalRecords = result.Count();
            var advancedsearch = result.Select(g => new
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
                StatusId = g.StatusId,
                Level = g.Level,
                StatusName = g.StatusName,
                Reason = g.Reason,
                EntryDate = g.EntryDate,
                TotalRecords
            }).AsEnumerable()
            .Skip((searchDetailParams.CurrentPageNumber - 1) * searchDetailParams.PageSize).Take(searchDetailParams.PageSize);
            return this.Response(MessageTypes.Success, string.Empty, advancedsearch);
        }

        [HttpGet]
        public ApiResponse ApplicantTimeline(int ApplicantId)
        {
            var advancedsearch = this.entities.USP_ATS_ApplicantTimeLine(ApplicantId).Select(g => new
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
                StatusId = g.StatusId,
                StatusName = g.StatusName,
                Level = g.Level,
                Reason = g.Reason,
                EntryDate = g.EntryDate,
            }).AsEnumerable();
            return this.Response(MessageTypes.Success, string.Empty, advancedsearch);
        }

        [HttpGet]
        public ApiResponse GetStatus()
        {
            var status = this.entities.USP_ATS_GetStatus().Select(g => new
            {
                g.StatusId,
                g.StatusName
            });
            return this.Response(MessageTypes.Success, string.Empty, status);
        }
    }
}

