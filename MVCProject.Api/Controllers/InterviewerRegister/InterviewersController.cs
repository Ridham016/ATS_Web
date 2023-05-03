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
namespace MVCProject.Api.Controllers.Interviewers
{
    public class InterviewersController : BaseController
    {
        private MVCProjectEntities entities;

        public InterviewersController()
        {
            this.entities = new MVCProjectEntities();
        }
        [HttpPost]
        public ApiResponse GetAllInterviewers(PagingParams interviewerDetailParams)
        {
            if (string.IsNullOrWhiteSpace(interviewerDetailParams.Search))
            {
                interviewerDetailParams.Search = string.Empty;
            }
            var result = entities.USP_ATS_AllInterviewers().Where(x => x.InterviewerName.Trim().ToLower().Contains(interviewerDetailParams.Search.Trim().ToLower())).ToList();
            var TotalRecords = result.Count();
            var interviewerslist = result.Select(d => new
                                    {
                                        InterviewerId = d.InterviewerId,
                                        InterviewerName = d.InterviewerName,
                                        InterviewerEmail = d.InterviewerEmail,
                                        InterviewerPhone = d.InterviewerPhone,
                                        Is_Active = d.Is_Active,
                                        CompanyId = d.CompanyId,
                                        CompanyName = d.CompanyName,
                                        TotalRecords
                                    }).AsQueryable().Skip((interviewerDetailParams.CurrentPageNumber - 1) * interviewerDetailParams.PageSize).Take(interviewerDetailParams.PageSize);
            return this.Response(MessageTypes.Success, string.Empty, interviewerslist);
        }

        [HttpGet]
        public ApiResponse GetInterviewerById(int InterviewerId)
        {
            var InterviewerDetail = this.entities.USP_ATS_InterviewerbyId(InterviewerId).Select(d => new
            {
                InterviewerId = d.InterviewerId,
                InterviewerName = d.InterviewerName,
                InterviewerEmail = d.InterviewerEmail,
                InterviewerPhone = d.InterviewerPhone,
                CompanyId = d.CompanyId,
                CompanyName = d.CompanyName,
                Is_Active = d.Is_Active
            }).SingleOrDefault();
            if (InterviewerDetail != null)
            {
                return this.Response(Utilities.MessageTypes.Success, string.Empty, InterviewerDetail);
            }
            else
            {
                return this.Response(Utilities.MessageTypes.NotFound, string.Empty);
            }
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

        [HttpPost]
        public ApiResponse Register([FromBody] ATS_Interviewer data)
        {
            var InterviewerData = this.entities.ATS_Interviewer.FirstOrDefault(x => x.InterviewerId == data.InterviewerId);

            if (InterviewerData == null)
            {

                data.EntryDate = DateTime.Now;
                entities.ATS_Interviewer.AddObject(data);
                if (!(this.entities.SaveChanges() > 0))
                {
                    return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Interviewer));
                }

                return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.Interviewer));
            }
            else
            {
                InterviewerData.InterviewerName = data.InterviewerName;
                InterviewerData.InterviewerEmail = data.InterviewerEmail;
                InterviewerData.InterviewerPhone = data.InterviewerPhone;
                InterviewerData.CompanyId = data.CompanyId;
                InterviewerData.Is_Active = data.Is_Active;
                InterviewerData.UpdatedBy = UserContext.UserId;
                InterviewerData.UpdateDate = DateTime.Now;

                this.entities.ATS_Interviewer.ApplyCurrentValues(InterviewerData);
                if (!(this.entities.SaveChanges() > 0))
                {
                    return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError), Resource.Interviewer);
                }

                return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.Interviewer));
            }
        }
    }
}