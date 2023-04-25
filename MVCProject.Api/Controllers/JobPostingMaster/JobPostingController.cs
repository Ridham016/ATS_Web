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

namespace MVCProject.Api.Controllers.JobPostingMaster
{
    public class JobPostingController : BaseController
    {
        private MVCProjectEntities entities;

        public JobPostingController()
        {
            this.entities = new MVCProjectEntities();
        }

        [HttpGet]
        public ApiResponse GetCompanyDetails()
        {
            var data = this.entities.USP_ATS_GetCompanyDetails().Select(x => new
            {
                Id = x.Id,
                CompanyName = x.CompanyName,
                IsActive = x.IsActive
            }).ToList();
            return this.Response(MessageTypes.Success, string.Empty, data);
        }
        [HttpGet]
        public ApiResponse GetPostingStatus()
        {
            var data = this.entities.USP_ATS_GetPostingstatus().Select(x => new
            {
                Id = x.Id,
                PostingStatus = x.PostingStatus,
                IsActive = x.IsActive
            }).ToList();
            return this.Response(MessageTypes.Success, string.Empty, data);
        }
        //[HttpGet]
        //public ApiResponse GetPositionDetails(string searchText)
        //{
        //    var data = this.entities.USP_ATS_GetPositionDetails().Where(x => x.PositionName.Contains(searchText)).Select(x => new
        //    {
        //        Id = x.Id,
        //        PositionName = x.PositionName,
        //        IsActive = x.IsActive
        //    }).ToList();
        //    return this.Response(MessageTypes.Success, string.Empty, data);
        //}
        [HttpGet]
        public ApiResponse GetPositionDetails()
        {
            var data = this.entities.USP_ATS_GetPositionDetails().Select(x => new
            {
                Id = x.Id,
                PositionName = x.PositionName,
                IsActive = x.IsActive
            }).ToList();
            return this.Response(MessageTypes.Success, string.Empty, data);
        }

        [HttpPost]
        public ApiResponse Register([FromBody] ATS_JobPosting data)
        {
            var postingData = this.entities.ATS_JobPosting.FirstOrDefault(x => x.PostingId == data.PostingId);
            if (postingData == null)
            {
                data.EntryDate = DateTime.Now;
                data.EntryBy = "1";
                entities.ATS_JobPosting.AddObject(data);
                if (!(this.entities.SaveChanges() > 0))
                {
                    return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Job));
                }

                return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.Job));
            }
            else
            {
                postingData.PostingId = data.PostingId;
                postingData.PositionId = data.PositionId;
                postingData.CompanyId = data.CompanyId;
                postingData.Experience = data.Experience;
                postingData.Salary = data.Salary;
                postingData.PostingStatusId = data.PostingStatusId;
                postingData.IsActive = data.IsActive;
                postingData.EntryDate = DateTime.Now;
                postingData.UpdateDate = DateTime.Now;
                this.entities.ATS_JobPosting.ApplyCurrentValues(postingData);
                if (!(this.entities.SaveChanges() > 0))
                {
                    return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError), Resource.Job);
                }

                return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.Job));
            }
        }

        [HttpPost]
        public ApiResponse GetAllPostings(PagingParams jobPostingDetailParams)
        {
            if (string.IsNullOrWhiteSpace(jobPostingDetailParams.Search))
            {
                jobPostingDetailParams.Search = string.Empty;
            }
            var result = entities.USP_ATS_AllPostings().Where(x => x.PositionName.Trim().ToLower().Contains(jobPostingDetailParams.Search.Trim().ToLower())).ToList();
            var TotalRecords = result.Count();
            if(TotalRecords> 0)
            {
                var PostingDetail = result.Select(g => new
                {
                    PostingId = g.PostingId,
                    CompanyId = g.CompanyId,
                    PositionId = g.PositionId,
                    PostingStatusId = g.PostingStatusId,
                    PostingStatus = g.PostingStatus,
                    CompanyName = g.CompanyName,
                    PositionName = g.PositionName,
                    Experience = g.Experience,
                    Salary = g.Salary,
                    EntryDate = g.EntryDate,
                    IsActive = g.IsActive,
                    TotalRecords
                }).AsEnumerable()
                .AsQueryable().OrderByField(jobPostingDetailParams.OrderByColumn, jobPostingDetailParams.IsAscending)
                .Skip((jobPostingDetailParams.CurrentPageNumber - 1) * jobPostingDetailParams.PageSize).Take(jobPostingDetailParams.PageSize);
                return this.Response(MessageTypes.Success, string.Empty, PostingDetail);
            }
            return this.Response(MessageTypes.Error, string.Empty);
        }

        [HttpGet]
        public ApiResponse GetPostingById(int PostingId)
        {
            var PostingDetail = this.entities.USP_ATS_PostingById(PostingId).SingleOrDefault();
            if (PostingDetail != null)
            {
                return this.Response(Utilities.MessageTypes.Success, string.Empty, PostingDetail);
            }
            else
            {
                return this.Response(Utilities.MessageTypes.NotFound, string.Empty);
            }
        }

        [HttpPost]
        public ApiResponse PositionRegister([FromBody] ATS_PositionMaster data)
        {
           
                entities.ATS_PositionMaster.AddObject(data);
                if (!(this.entities.SaveChanges() > 0))
                {
                    return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Position));
                }

            return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.Position),data.Id);
        }

    }
}