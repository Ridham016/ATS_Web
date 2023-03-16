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
        public ApiResponse AdvancedActionSearch([FromUri] SearchParams, [FromBody] PagingParams searchDetailParams)
        {
            var result = this.entities.USP_ATS_ActionApplicantSearch(SearchParams.StatusId, searchDetailParams.StartDate, searchDetailParams.EndDate).ToList();
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
                StatusName = g.StatusName,
                Reason = g.Reason,
                TotalRecords
            });
            //.AsEnumerable()
            //.AsQueryable().OrderByField(searchDetailParams.OrderByColumn, searchDetailParams.IsAscending)
            //.Skip((searchDetailParams.CurrentPageNumber - 1) * searchDetailParams.PageSize).Take(searchDetailParams.PageSize);
            return this.Response(MessageTypes.Success, string.Empty, advancedsearch);

            //var advancedsearch = entities.USP_ATS_ActionApplicantSearch(CurrentStatus, PreferredLocation, StartDate, EndDate).ToList();
            //    .AsQueryable().OrderByField(applicantDetailParams.OrderByColumn, applicantDetailParams.IsAscending)
            //    .Skip((applicantDetailParams.CurrentPageNumber - 1) * applicantDetailParams.PageSize).Take(applicantDetailParams.PageSize);
            //return this.Response(MessageTypes.Success,string.Empty, advancedsearch);
            //    .Skip((applicantDetailParams.CurrentPageNumber - 1) * applicantDetailParams.PageSize).Take(applicantDetailParams.PageSize);;
            //var applicantlist = entities.USP_ATS_ApplicantsList().AsEnumerable()
            //    .AsQueryable().OrderByField(applicantDetailParams.OrderByColumn, applicantDetailParams.IsAscending)
            //    .Skip((applicantDetailParams.CurrentPageNumber - 1) * applicantDetailParams.PageSize).Take(applicantDetailParams.PageSize);
            //return this.Response(MessageTypes.Success, string.Empty, advancedsearch);
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

