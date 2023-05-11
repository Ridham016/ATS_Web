// -----------------------------------------------------------------------
// <copyright file="PostingController.cs" company="ASK E-Sqaure">
// All copy rights reserved @ASK E-Sqaure.
// </copyright>
// ----------------------------------------------------------------------- 

namespace MVCProject.Api.Controllers.PostingManagement
{
    using MVCProject.Api.Models;
    using MVCProject.Api.Models.FilterCriterias;
    using MVCProject.Api.Utilities;
    using MVCProject.Api.ViewModel;
    using MVCProject.Common.Resources;
    using NPOI.HSSF.Record.Aggregates;
    #region Namespaces
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Web.Http;
    #endregion
    public class PostingController : BaseController
    {
        private MVCProjectEntities entities;

        public PostingController()
        {
            this.entities = new MVCProjectEntities();
        }

        [HttpPost]
        public ApiResponse GetApplicantList(PagingParams applicantDetailParams)
        {
            if (string.IsNullOrWhiteSpace(applicantDetailParams.Search))
            {
                applicantDetailParams.Search = string.Empty;
            }
            var result = entities.USP_ATS_ApplicantPostingList(applicantDetailParams.StatusId).Where(x => x.ApplicantName.Trim().ToLower().Contains(applicantDetailParams.Search.Trim().ToLower())).ToList();
            var TotalRecords = result.Count();
            var applicantlist = result.Select(g => new
            {
                ApplicantId = g.ApplicantId,
                ApplicantName = g.ApplicantName,
                Email = g.Email,
                Phone = g.Phone,
                Address = g.Address,
                PostingId = g.PostingId,
                EntryDate = g.EntryDate,
                IsActive = g.IsActive,
                StatusId = g.StatusId,
                StatusName = g.StatusName,
                TotalRecords
            }).AsEnumerable()
                .AsQueryable().OrderByField(applicantDetailParams.OrderByColumn, applicantDetailParams.IsAscending)
                .Skip((applicantDetailParams.CurrentPageNumber - 1) * applicantDetailParams.PageSize).Take(applicantDetailParams.PageSize);
            return this.Response(MessageTypes.Success, string.Empty, applicantlist);
        }

        [HttpGet]
        public ApiResponse GetApplicant(int ApplicantId)
        {
            var applicantDetail = this.entities.USP_ATS_ApplicantPostingById(ApplicantId)
                .Select(g => new
                {
                    ApplicantId = g.ApplicantId,
                    ApplicantName = g.ApplicantName,
                    Email = g.Email,
                    Phone = g.Phone,
                    Address = g.Address,
                    PostingId = g.PostingId,
                    EntryDate = g.EntryDate,
                    IsActive = g.IsActive,
                    StatusId = g.StatusId,
                    StatusName = g.StatusName
                }).SingleOrDefault();
            if (applicantDetail != null)
            {
                return this.Response(Utilities.MessageTypes.Success, string.Empty, applicantDetail);
            }
            else
            {
                return this.Response(Utilities.MessageTypes.NotFound, string.Empty);
            }
        }

        [HttpGet]
        public ApiResponse GetStatus()
        {
            var status = this.entities.USP_ATS_GetStatus().Where(x => x.StatusId == 1 || x.StatusId == 3 || x.StatusId == 7).Select(g => new
            {
                g.StatusId,
                g.StatusName
            });
            return this.Response(MessageTypes.Success, string.Empty, status);
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


        [HttpPost]
        public ApiResponse Register([FromBody]ATS_ApplicantRegister data)
        {
            var Applicant = this.entities.ATS_ApplicantRegister.Where(x => x.ApplicantId == data.ApplicantId).FirstOrDefault();
            Applicant.PostingId = data.PostingId;
            this.entities.ATS_ApplicantRegister.ApplyCurrentValues(Applicant);
            if (!(this.entities.SaveChanges() > 0))
            {
                return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError), Resource.Posting);
            }

            return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.Posting));
        }


        
    }
}
