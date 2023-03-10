// -----------------------------------------------------------------------
// <copyright file="SchedulesController.cs" company="ASK E-Sqaure">
// All copy rights reserved @ASK E-Sqaure.
// </copyright>
// ----------------------------------------------------------------------- 

namespace MVCProject.Api.Controllers.ScheduleManagement
{
    using iTextSharp.text.log;
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
            var applicantlist = entities.USP_ATS_ApplicantsList().AsEnumerable()
                .AsQueryable().OrderByField(applicantDetailParams.OrderByColumn, applicantDetailParams.IsAscending)
                .Skip((applicantDetailParams.CurrentPageNumber - 1) * applicantDetailParams.PageSize).Take(applicantDetailParams.PageSize);
            return this.Response(MessageTypes.Success, string.Empty, applicantlist);
        }

        [HttpGet]
        public ApiResponse GetApplicant(int ApplicantId)
        {
            var applicantDetail = this.entities.USP_ATS_SingleApplicant(ApplicantId)
                .Select(g => new
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
                    StatusName = g.StatusName
                }).SingleOrDefault();
            //var StatusID = applicantDetail.Select(x => x.StatusId).SingleOrDefault();
            //var button = this.entities.USP_ATS_GetButton(StatusID).ToList();
            //var applicant = applicantDetail.Concat(button).ToList();
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
        public ApiResponse GetButtons(int StatusId)
        {
            var buttons = this.entities.USP_ATS_GetButton(StatusId).ToList();
            return this.Response(Utilities.MessageTypes.Success, string.Empty, buttons);
        }

        [HttpPost]
        public ApiResponse UpdateStatus(int StatusId, int ApplicantId)
        {
            var level = 0;
            if(StatusId == 4)
            {
               var getlevel = entities.USP_ATS_GetLevel(ApplicantId).SingleOrDefault();
                if(getlevel != null)
                {
                    level = (int)getlevel.Level + 1;
                }
            }
            var addtoaction = new ATS_ActionHistory()
            {
                ApplicantId = ApplicantId,
                StatusId = StatusId,
                IsActive = true,
                Level = level,
                EntryDate = DateTime.Now
            };
            entities.ATS_ActionHistory.AddObject(addtoaction);
            if (!(this.entities.SaveChanges() > 0))
            {
                return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.StatusName));
            }
            int[] Result;
            Result = new int[] { StatusId, addtoaction.ActionId };

            return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.StatusName),Result);
        }
        [HttpGet]
        public ApiResponse GetInterviewers()
        {
            var interviewerslist = (from d in this.entities.USP_ATS_InterviewerList().AsEnumerable()
                                    select new
                                    {
                                        InterviewerId = d.InterviewerId,
                                        InterviewerName = d.InterviewerName,
                                    });
            return this.Response(MessageTypes.Success, string.Empty, interviewerslist);
        }

        [HttpPost]
        public ApiResponse ScheduleInterview([FromBody]ATS_ScheduleInformation data)
        {
            entities.ATS_ScheduleInformation.AddObject(new ATS_ScheduleInformation
            {
                ScheduleDateTime = data.ScheduleDateTime,
                ScheduleLink = data.ScheduleLink,
                ActionId = data.ActionId,
                Description = data.Description,
                InterviewerId = data.InterviewerId,
                IsActive = true,
                EntryDate = DateTime.Now
            });
            //entities.ATS_ActionHistory.AddObject(Action);
            //this.entities.ATS_ActionHistory.AddObject(new ATS_ActionHistory()
            //{
            //    ApplicantId = data.ApplicantId,
            //    StatusId = 1,
            //    Level = 0,
            //    IsActive = true,
            //    EntryDate = DateTime.Now
            //});
            if (!(this.entities.SaveChanges() > 0))
            {
                return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Interviewer));
            }

            return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.Interviewer));
        }

        [HttpGet]
        public ApiResponse GetReasons()
        {
            var reasons = this.entities.USP_ATS_GetOtherReasons().ToList();
            return this.Response(MessageTypes.Success, string.Empty, reasons);
        }

        [HttpPost]
        public ApiResponse UpdateReason([FromBody]ATS_MasterOtherReason data, int ActionId)
        {
            var Action = entities.ATS_ActionHistory.Where(x => x.ActionId == ActionId).SingleOrDefault();
            if (Action != null)
            {
                Action.ReasonId = data.ReasonId;
            }
            entities.ATS_ActionHistory.ApplyCurrentValues(Action);
            if (!(this.entities.SaveChanges() > 0))
            {
                return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Interviewer));
            }

            return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.Applicant));
        }


    }
}
